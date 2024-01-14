using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    [OutputCache(CacheProfile = "Cache12Hour")]
    public class PostCommentController : Controller
    {
        //
        // GET: /UserTask/
        public ActionResult GetCommentView()
        {
            return PartialView("../TimeLine/_Comment");
        }

        // GET: /UserTask/
        public ActionResult GetPostView()
        {
            return PartialView("../TimeLine/_TimeLine");
        }
        public ActionResult GetReplyView()
        {
            return PartialView("../TimeLine/_ReplyComment");
        }
    }
}