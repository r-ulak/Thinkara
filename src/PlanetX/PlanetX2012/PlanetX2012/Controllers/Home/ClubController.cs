using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanetX2012.DataCache;

namespace PlanetX2012.Controllers.Home
{
    public class ClubController : Controller
    {
        //
        // GET: /Club/

        public JsonResult GetClubAndFriendList(string term)
        {
            IFriendClubRepository friendClubStore = new FriendClubRepository();
            var friendClublist = friendClubStore.GetFriendClubs((int)Session["WebUserId"])
                 .Where(x => x.Value.StartsWith(term, StringComparison.OrdinalIgnoreCase)).Select(x => new
                 {
                     id = x.Id,
                     value = x.Value.Trim(),
                     type = x.MemberType
                 }).Take(10);

            return Json(friendClublist, JsonRequestBehavior.AllowGet);
        }


    }
}
