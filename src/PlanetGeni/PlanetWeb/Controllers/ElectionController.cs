using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class ElectionController : Controller
    {
        //
        // GET: /Election/
        public ActionResult GetElections()
        {
            return PartialView("_Election");
        }
        public ActionResult GetElectionTicket()
        {
            return PartialView("_ElectionTicket");
        }
        public ActionResult GetElectionCandidateView()
        {
            return PartialView("_Candidate");
        }
        public ActionResult GetElectionResultView()
        {
            return PartialView("_CandidateResult");
        }
        public ActionResult GetRunForOfficeView()
        {
            return PartialView("_RunForOffice");
        }
    }
}