using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO.DAO;
using DAO.Models;
using PlanetX2012.Models.DAO;

namespace PlanetX2012.Controllers.Home
{
    public class FriendController : Controller
    {

        //
        // GET: /Friends List/

        public ActionResult ListFriends(int webUserId)
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", webUserId);
            return PartialView("../PartialViews/Friend/FriendList",
                sp.GetSqlData<FriendsInfo>("GetFriendsInfo", dictionary));
        }

    }
}