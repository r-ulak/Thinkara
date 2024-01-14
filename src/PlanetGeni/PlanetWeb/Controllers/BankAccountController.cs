using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class BankAccountController : Controller
    {
        public ActionResult GetBankAcount()
        {
            return PartialView("_BankAccount");
        }
        public ActionResult BuyMetal()
        {
            return PartialView("_BuyMetal");
        }
        public ActionResult SellMetal()
        {
            return PartialView("_SellMetal");
        }
        public ActionResult BankStatement()
        {
            return PartialView("_BankStatement");
        }
        public ActionResult Top10Richesht()
        {
            return PartialView("_Top10Richesht");
        }
    }
}