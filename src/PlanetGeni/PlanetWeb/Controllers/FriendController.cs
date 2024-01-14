using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class FriendController : Controller
    {
        public ActionResult SendInvite()
        {
            return PartialView("_Invite");
        }
        public ActionResult ImportContacts()
        {
            return PartialView("_FindFriendsItem");
        }
    }
}