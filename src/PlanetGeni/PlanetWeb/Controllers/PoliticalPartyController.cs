using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class PoliticalPartyController : Controller
    {
        //
        // GET: /Election/
        public ActionResult GetParty()
        {
            return PartialView("_PoliticalParty");
        }
        public ActionResult GetPartyInviteView()
        {
            return PartialView("_MyPartyInvite");
        }
        public ActionResult GetManagePartyView()
        {
            return PartialView("_MyPartyManage");
        }
        public ActionResult GetPartyInfoView()
        {
            return PartialView("_ViewParty");
        }
    }
}