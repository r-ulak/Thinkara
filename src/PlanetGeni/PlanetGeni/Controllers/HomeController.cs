using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Mvc.Facebook;
using Microsoft.AspNet.Mvc.Facebook.Client;
using PlanetGeni.Models;
using System;

namespace PlanetGeni.Controllers
{
    public class HomeController : Controller
    {
        [FacebookAuthorize("email", "user_photos")]
        public async Task<ActionResult> Index(FacebookContext context)
        {
            if (ModelState.IsValid)
            {
                var user = await context.Client.GetCurrentUserAsync<MyAppUser>();
                return View(user);
            }

            return View("Error");
        }

      //  [FacebookAuthorize("email", "friends_birthday")]
        public async Task<ActionResult> GetFriends(FacebookContext context)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    var user = await context.Client.GetCurrentUserAsync<MyAppUser>();
                    var friendsWithUpcomingBirthdays = user.Friends.Data.Take(100);
                    user.Friends.Data = friendsWithUpcomingBirthdays.ToList();
                    return PartialView("MyFriends", user);
                }

                return View("Error");
            }
            catch (Exception ex)
            {

                return View("Error");
            }

        }

        public ActionResult MyGetFriends()
        {
            return View();
        }


        // This action will handle the redirects from FacebookAuthorizeFilter when
        // the app doesn't have all the required permissions specified in the FacebookAuthorizeAttribute.
        // The path to this action is defined under appSettings (in Web.config) with the key 'Facebook:AuthorizationRedirectPath'.
        public ActionResult Permissions(FacebookRedirectContext context)
        {
            if (ModelState.IsValid)
            {
                return View(context);
            }

            return View("Error");
        }
    }
}
