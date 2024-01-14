using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class AdvertisementsController : Controller
    {
        //
        // GET: /AdvertisementsController/
        public ActionResult GetAdvertisements()
        {
            return PartialView("_Advertisements");
        }
    }
}