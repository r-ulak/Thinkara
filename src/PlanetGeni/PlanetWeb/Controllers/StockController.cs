using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class StockController : Controller
    {
        //
        // GET: /Stock/
        public ActionResult GetStockView()
        {
            return PartialView("_Stock");
        }
        public ActionResult GetStockForecastView()
        {
            return PartialView("_StockForecast");
        }
    }
}