using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetX2012.Controllers.Home
{
    public class ChatController : Controller
    {
        //
        // GET: /Chat/

        public ActionResult AddChat(int friendUserID, string message, string imageUrl)
        {
            ViewBag.friendUserID = friendUserID;
            ViewBag.message = message;
            ViewBag.imageUrl = imageUrl;
            return PartialView("../PartialViews/Chat/Chat");
        }

    }
}
