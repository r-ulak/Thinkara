using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class NationalSecurityController : Controller
    {
        //
        // GET: /BuyMerchandise/
        public ActionResult Buy()
        {
            return PartialView("../NationalSecurity/_NationalDefense");
        }

        [ApiValidateAntiForgeryToken]
        [HttpGet]
        //[Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
        public ActionResult RequestWarKey()
        {
            return PartialView("../NationalSecurity/_RequestWarKey");
        }
	}
}