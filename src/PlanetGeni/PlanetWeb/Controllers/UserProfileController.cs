using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class UserProfileController : Controller
    {
        //
        // GET: /Job/
        public ActionResult GetUserProfile()
        {
            return PartialView("_Profile");
        }
        public ActionResult GetManageUserProfile()
        {
            return PartialView("_ProfileManage");
        }
    }
}