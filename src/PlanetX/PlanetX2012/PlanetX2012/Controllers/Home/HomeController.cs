using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanetX2012.Models.DAO;
using PlanetX2012.Models.ContentModel;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using DAO.Models;
using DAO.DAO;
namespace PlanetX2012.Controllers
{
    public class HomeController : Controller
    {
        private StoredProcedure sp = new StoredProcedure();
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index(string webUserId)
        {
            ViewBag.Title = "PlanetX";
            if (webUserId == String.Empty || webUserId == null)
            {
                return View("Login");
            }
            return View(GetUserInfo(Convert.ToInt32(webUserId)));
        }

        [HttpGet]
        public PartialViewResult SomeAction()
        {
            return PartialView("../PartialViews/Finance/NewBusiness");
        }

        [HttpPost]
        public ActionResult GetIndex(string webUserId)
        {
            ViewBag.Title = "PlanetX";
            if (webUserId == String.Empty || webUserId == null)
            {
                return View("Login");
            }
            Session["WebUserId"]=Convert.ToInt32( webUserId);
            return View("Index", GetUserInfo(Convert.ToInt32(webUserId)));
        }

        private HomeContent GetUserInfo(int webUserId)
        {            
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", webUserId);
            FinanceContent userFinance = sp.GetSqlDataSignleRow<FinanceContent>("GetFinanceContent", dictionary);

            WebUser user = sp.GetSqlDataSignleRow<WebUser>("GetUserById", dictionary);
            HomeContent homeContent = new HomeContent
            {
                useraccount = user,
                finance = userFinance
            };
            return homeContent;
        }

    }
}
