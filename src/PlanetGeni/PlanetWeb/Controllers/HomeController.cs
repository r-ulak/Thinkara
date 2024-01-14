using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        //public ActionResult Index(string webUserId, string webPassword)
        //{
        //    //if (String.IsNullOrEmpty(webUserId) || String.IsNullOrEmpty(webPassword))
        //    //{
        //    //    return View("Login");
        //    //}
        //    //StringBuilder pwd = new StringBuilder("Kathmandu");
        //    //pwd.Append(DateTime.UtcNow.Day + DateTime.UtcNow.Month);
        //    //if (webPassword != pwd.ToString())
        //    //{
        //    //    return View("Login");

        //    //}

        //    //int userId = Convert.ToInt32(webUserId);
        //    //Session["UserId"] = userId;
        //    //Session["CountryId"] = webRepo.GetCountryId(userId);

        //    string emailaddress = User.Identity.GetUserName();

        //    int userId = webRepo.GetUserIdByEmail(emailaddress);
        //    Session["UserId"] = userId;
        //    Session["CountryId"] = webRepo.GetCountryId(userId);
        //    return View();
        //}
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public ActionResult Index(string view)
        {
            string emailaddress = User.Identity.GetUserName();
            int userId = webRepo.GetUserIdByEmail(emailaddress);
            if (userId == 0)
            {
                   AuthenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
            }
            Session["UserId"] = userId;
            Session["CountryId"] = webRepo.GetCountryId(userId);
            ViewBag.NextView = view;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}