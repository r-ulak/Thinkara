using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PlanetWeb.Models;
using DAO.Models;
using Repository;
using System.Collections.Generic;
using DTO.Custom;
using Common;
using System.Text;
using Manager.ServiceController;
namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private WebUserManager webuserManager = new WebUserManager();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // The Authorize Action is the end point which gets called when you access any
        // protected Web API. If the user is not logged in then they will be redirected to 
        // the Login page. After a successful login you can call a Web API.
        [HttpGet]
        public ActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, "Bearer");
            AuthenticationManager.SignIn(identity);
            return new EmptyResult();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public ActionResult EmailNotConfirmed()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            try
            {
                if (AppSettings.OnlyAllowedUsers)
                {
                    if (!webRepo.IsAllowedUsers(model.Email))
                    {
                        ModelState.AddModelError("", "Invalid login attempt.(Code:OAU)");
                        return View(model);
                    }
                }

                var userid = UserManager.FindByEmail(model.Email).Id;
                if (!UserManager.IsEmailConfirmed(userid))
                {
                    ViewBag.UserId = userid;
                    return View("EmailNotConfirmed");
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Login");
                ModelState.AddModelError("", "Invalid login attempt.(Code:EXC)");
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    Session.Add("loginProvider", "Native");

                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.(Code:FA)");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register(string invitationId)
        {
            Guid guidOutput = new Guid();
            bool isValid = Guid.TryParse(invitationId, out guidOutput);
            if (isValid)
            {
                Session.Add("invitationId", invitationId);
            }
            else if (AppSettings.RegisterInviteOnly)
            {
                ModelState.AddModelError("", "Email Invite Only allowed at this time.");
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                WebUserContact webcontact = new WebUserContact();
                if (AppSettings.OnlyAllowedUsers)
                {
                    if (!webRepo.IsAllowedUsers(model.Email))
                    {
                        ModelState.AddModelError("", "Invalid login attempt.(Code:OAU)");
                        return View(model);
                    }
                }

                string inviataionid = string.Empty;
                if (Session["invitationId"] != null)
                {
                    inviataionid = Session["invitationId"].ToString();
                    webcontact = webRepo.GetInvitationSender(inviataionid, model.Email);
                }

                if (AppSettings.RegisterInviteOnly)
                {
                    if (webcontact.UserId == 0)
                    {
                        ModelState.AddModelError("", "Invalid Invitation token or Email.");
                        return View(model);
                    }
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    //Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    StringBuilder emailBody = new StringBuilder(AppSettings.ConformEmailTemplate);
                    emailBody.Replace(":FullName", model.FirstName);
                    emailBody.Replace(":Link", "<a href=        \"" + callbackUrl + "\">link</a>");
                    emailBody.Replace(":NakedLink", callbackUrl);
                    await UserManager.SendEmailAsync(user.Id,
                            "Confirm your account",
                         emailBody.ToString()
                            );
                    WebUser userInfo = new WebUser
                    {
                        NameFirst = model.FirstName,
                        NameLast = model.LastName,
                        EmailId = model.Email,
                        CountryId = model.CountryCode
                    };
                    InitializeWebUser webInfo = new InitializeWebUser();
                    webInfo.UserInfo = userInfo;
                    userInfo.UserId = webRepo.InitializeUser(webInfo);
                    webuserManager.AddWebUserIndex(userInfo);
                    //return RedirectToAction("Index", "Home");
                    webcontact.FriendUserId = userInfo.UserId;

                    Task taskA = Task.Factory.StartNew(() => webRepo.GiveCreditForAcceptedInvitationandFollowInvitee(webcontact));

                    return View("DisplayEmail");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SendAccountConfirmEmail(string userID)
        {
            if (userID == null)
            {
                return View("Error");
            }
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(userID,
                    "Confirm your account",
                    ("Please confirm your account by clicking this: <a href=        \"" + callbackUrl + "\">link</a>"));
            return View("DisplayEmail");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                StringBuilder emailBody = new StringBuilder(AppSettings.ResetPasswordEmailTemplate);
                emailBody.Replace(":Link", "<a href=        \"" + callbackUrl + "\">link</a>");
                emailBody.Replace(":NakedLink", callbackUrl);
                emailBody.Replace(":IPaddress", Request.ServerVariables["REMOTE_ADDR"]);
                emailBody.Replace(":DateTime", DateTime.UtcNow.ToString("F"));
                emailBody.Replace(":Email", model.Email);
                await UserManager.SendEmailAsync(user.Id,
                        "Reset Password",
                     emailBody.ToString()
                        );

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            if (AppSettings.OnlyAllowedUsers && !string.IsNullOrEmpty(loginInfo.Email))
            {
                if (!webRepo.IsAllowedUsers(loginInfo.Email))
                {
                    ModelState.AddModelError("", "Invalid login attempt.(Code:OAU)");
                    return RedirectToAction("Login");
                }
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            Session.Add("Email", loginInfo.Email);
            Session.Add("loginProvider", loginInfo.Login.LoginProvider.ToLower());
            switch (result)
            {
                case SignInStatus.Success:
                    //foreach (var item in loginInfo.ExternalIdentity.Claims)
                    //{
                    //    System.Diagnostics.Debug.WriteLine(item.Issuer);
                    //    System.Diagnostics.Debug.WriteLine(item.Subject);
                    //    System.Diagnostics.Debug.WriteLine(item.Properties);
                    //    System.Diagnostics.Debug.WriteLine(item.Type);
                    //    System.Diagnostics.Debug.WriteLine(item.Value);
                    //}
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    //If the user does not have an account, then prompt the user to create an account
                    //foreach (var item in loginInfo.ExternalIdentity.Claims)
                    //{
                    //    System.Diagnostics.Debug.WriteLine(item.Subject);
                    //    System.Diagnostics.Debug.WriteLine(item.Properties);
                    //    System.Diagnostics.Debug.WriteLine(item.Type);
                    //    System.Diagnostics.Debug.WriteLine(item.Value);
                    //}
                    ViewBag.ReturnUrl = returnUrl;

                    return View("ExternalLoginConfirmation", GetClaimData(loginInfo));
            }
        }
        private ExternalLoginConfirmationViewModel GetClaimData(ExternalLoginInfo logininfo)
        {
            ExternalLoginConfirmationViewModel claimData = new ExternalLoginConfirmationViewModel
            {
                Email = logininfo.Email
            };
            switch (logininfo.Login.LoginProvider.ToLower())
            {
                case "google":
                    claimData.FirstName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == ClaimTypes.GivenName)
              .Select(c => c.Value).SingleOrDefault();
                    claimData.LastName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == ClaimTypes.Surname)
                        .Select(c => c.Value).SingleOrDefault();

                    Session.Add("accessToken", logininfo.ExternalIdentity.Claims.Where(f => f.Type == "urn:tokens:gooleplus:accesstoken")
                        .Select(c => c.Value).SingleOrDefault());

                    break;
                case "facebook":
                    claimData.FirstName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == "urn:facebook:first_name")
            .Select(c => c.Value).SingleOrDefault();
                    claimData.LastName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == "urn:facebook:last_name")
                        .Select(c => c.Value).SingleOrDefault();

                    Session.Add("accessToken", logininfo.ExternalIdentity.Claims.Where(f => f.Type == "FacebookAccessToken")
                        .Select(c => c.Value).SingleOrDefault());
                    break;
                case "twitter":
                    claimData.FirstName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == ClaimTypes.Name)
            .Select(c => c.Value).SingleOrDefault();
                    break;
                case "yahoo":
                    claimData.FirstName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == ClaimTypes.Name)
            .Select(c => c.Value).SingleOrDefault();
                    Session.Add("accessToken", logininfo.ExternalIdentity.Claims.Where(f => f.Type == "oauth_token")
                       .Select(c => c.Value).SingleOrDefault());
                    Session.Add("accessTokenSecret", logininfo.ExternalIdentity.Claims.Where(f => f.Type == "oauth_token_secret")
                       .Select(c => c.Value).SingleOrDefault());
                    Session.Add("yahooGuid", logininfo.ExternalIdentity.Claims.Where(f => f.Type == ClaimTypes.NameIdentifier)
                 .Select(c => c.Value).SingleOrDefault());
                    break;
                case "microsoft":
                    claimData.FirstName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == "urn:microsoftaccount:first_name")
                  .Select(c => c.Value).SingleOrDefault();
                    claimData.LastName = logininfo.ExternalIdentity.Claims.Where(f => f.Type == "urn:microsoftaccount:last_name")
                        .Select(c => c.Value).SingleOrDefault();
                    Session.Add("accessToken", logininfo.ExternalIdentity.Claims.Where(f => f.Type == "urn:microsoftaccount:access_token").Select(c => c.Value).SingleOrDefault());
                    break;
            }

            claimData.LoginProvider = logininfo.Login.LoginProvider.ToLower();
            return claimData;

        }
        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }


            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                if (AppSettings.OnlyAllowedUsers)
                {
                    if (!webRepo.IsAllowedUsers(model.Email))
                    {
                        ModelState.AddModelError("", "Invalid login attempt.(Code:OAU)");
                        return RedirectToAction("Login");
                    }
                }
                WebUserContact webcontact = new WebUserContact();
                string inviataionid = string.Empty;
                if (Session["invitationId"] != null)
                {
                    inviataionid = Session["invitationId"].ToString();
                    webcontact = webRepo.GetInvitationSender(inviataionid, model.Email);
                }

                if (AppSettings.RegisterInviteOnly)
                {
                    if (webcontact.UserId == 0)
                    {
                        ModelState.AddModelError("", "Invalid Invitation token or Email.");
                        return RedirectToAction("Login");
                    }
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        if (Session["Email"] == null)
                        {
                            Session["Email"] = model.Email;
                        }
                        if (Session["Email"].ToString() != model.Email)
                        {
                            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);

                            StringBuilder emailBody = new StringBuilder(AppSettings.ConformEmailTemplate);
                            emailBody.Replace(":FullName", model.FirstName);
                            emailBody.Replace(":Link", "<a href=        \"" + callbackUrl + "\">link</a>");
                            emailBody.Replace(":NakedLink", callbackUrl);
                            await UserManager.SendEmailAsync(user.Id,
                                    "Confirm your account",
                                 emailBody.ToString()
                                    );

                        }
                        WebUser userInfo = new WebUser
                        {
                            NameFirst = model.FirstName,
                            NameLast = model.LastName,
                            EmailId = model.Email,
                            CountryId = model.CountryCode
                        };

                        string accessToken = Session["accessToken"] == null ? string.Empty : Session["accessToken"].ToString();

                        string accessTokenSecret = Session["accessTokenSecret"] == null ? string.Empty : Session["accessTokenSecret"].ToString();
                        string yahooGuid = Session["yahooGuid"] == null ? string.Empty : Session["yahooGuid"].ToString();


                        InitializeWebUser webInfo = new InitializeWebUser();
                        webInfo.UserInfo = userInfo;
                        webInfo.AccessToken = accessToken;
                        webInfo.AccessTokenSeceret = accessTokenSecret;
                        webInfo.YahooUserGuid = yahooGuid;
                        webInfo.LoginProvider = Session["loginProvider"].ToString();



                        userInfo.UserId = webRepo.InitializeUser(webInfo);
                        webuserManager.AddWebUserIndex(userInfo);

                        webcontact.FriendUserId = userInfo.UserId;

                        Task taskA = Task.Factory.StartNew(() => webRepo.GiveCreditForAcceptedInvitationandFollowInvitee(webcontact));

                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [Authorize]
        [RequireHttps]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}
