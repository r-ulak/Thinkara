using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    [OutputCache(CacheProfile = "Cache12Hour")]
    public class CasinoController : Controller
    {
        //
        // GET: /Casino/
        public ActionResult GetCasinoMachines()
        {
            return PartialView("_Casino");
        }

    }
}