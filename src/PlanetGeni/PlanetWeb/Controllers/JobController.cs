﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class JobController : Controller
    {
        //
        // GET: /Job/
        public ActionResult GetJobs()
        {
            return PartialView("_Job");
        }

    }
}