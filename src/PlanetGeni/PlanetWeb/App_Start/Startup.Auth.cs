using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PlanetWeb.Models;
using PlanetWeb.Providers;
using System.Configuration;
using Microsoft.Owin.Security.Facebook;
using System.Security.Claims;
using Owin.Security.Providers.Yahoo;

namespace PlanetWeb
{
    public partial class Startup
    {
        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup()
        {
            PublicClientId = "web";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(20),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            //app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers

            var ms = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationOptions();
            ms.Scope.Add("wl.contacts_emails");
            ms.Scope.Add("wl.basic");
            ms.ClientId = ConfigurationManager.AppSettings["Microsoft:ClientId"];
            ms.ClientSecret = ConfigurationManager.AppSettings["Microsoft:ClientSecret"];
            ms.Provider = new Microsoft.Owin.Security.MicrosoftAccount.MicrosoftAccountAuthenticationProvider()
            {
                OnAuthenticated = async context =>
                {
                    context.Identity.AddClaim(new System.Security.Claims.Claim("urn:microsoftaccount:access_token", context.AccessToken));

                    foreach (var claim in context.User)
                    {
                        var claimType = string.Format("urn:microsoftaccount:{0}", claim.Key);
                        string claimValue = claim.Value.ToString();
                        if (!context.Identity.HasClaim(claimType, claimValue))
                            context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Microsoft"));
                    }
                }
            };

            app.UseMicrosoftAccountAuthentication(ms);

            app.UseTwitterAuthentication(
               consumerKey: ConfigurationManager.AppSettings["Twitter:ConsumerKey"],
               consumerSecret: ConfigurationManager.AppSettings["Twitter:ConsumerSecret"]);




            var googleOAuth2AuthenticationOptions = new GoogleOAuth2AuthenticationOptions
            {
                ClientId = ConfigurationManager.AppSettings["Google:ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["Google:ClientSecret"],
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                    {
                        context.Identity.AddClaim(new Claim("urn:tokens:gooleplus:accesstoken", context.AccessToken));

                    }
                }
            };

            googleOAuth2AuthenticationOptions.Scope.Add("https://www.googleapis.com/auth/contacts.readonly");
            googleOAuth2AuthenticationOptions.Scope.Add("email");
            googleOAuth2AuthenticationOptions.Scope.Add("profile");

            app.UseGoogleAuthentication(googleOAuth2AuthenticationOptions);

            var yahooAuth2AuthenticationOptions = new YahooAuthenticationOptions
            {

                ConsumerKey = ConfigurationManager.AppSettings["Yahoo:ClientId"],
                ConsumerSecret = ConfigurationManager.AppSettings["Yahoo:ClientSecret"],
                Provider = new YahooAuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                    {
                        context.Identity.AddClaim(new Claim("oauth_token", context.AccessToken));
                        context.Identity.AddClaim(new Claim("oauth_token_secret", context.AccessTokenSecret));

                        //foreach (var claim in context.User)
                        //{
                        //    var claimType = string.Format("urn:yahoo:{0}", claim.Key);
                        //    string claimValue = claim.Value.ToString();
                        //    if (!context.Identity.HasClaim(claimType, claimValue))
                        //        context.Identity.AddClaim(new Claim(claimType, claimValue, "XmlSchemaString", "Yahoo"));

                        //}
                    }
                }
            };

            app.UseYahooAuthentication(yahooAuth2AuthenticationOptions);
            var x = new FacebookAuthenticationOptions();
            x.Scope.Add("email");
            x.Scope.Add("publish_actions");
            x.AppId = ConfigurationManager.AppSettings["Facebook:AppId"];
            x.AppSecret = ConfigurationManager.AppSettings["Facebook:AppSecret"];
            x.Provider = new FacebookAuthenticationProvider()
            {
                OnAuthenticated = async context =>
                {
                    context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                    foreach (var claim in context.User)
                    {
                        var claimType = string.Format("urn:facebook:{0}", claim.Key);
                        string claimValue = claim.Value.ToString();
                        if (!context.Identity.HasClaim(claimType, claimValue))
                            context.Identity.AddClaim(new Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));

                    }

                }
            };

            x.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            app.UseFacebookAuthentication(x);
        }
    }
}