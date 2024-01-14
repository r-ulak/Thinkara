using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class UserVoteController : Controller
    {
        //
        // GET: /UserVoteController/
        public ActionResult GetUserVote()
        {
            return PartialView("_UserVote");
        }
    }
}