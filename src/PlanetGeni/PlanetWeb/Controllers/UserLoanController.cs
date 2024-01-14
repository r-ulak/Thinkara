using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlanetWeb.Controllers
{
    [Authorize]    [RequireHttps]    [OutputCache(CacheProfile = "Cache12Hour")]
    public class UserLoanController : Controller
    {
        //
        // GET: /UserLoan/
        public ActionResult GetUserLoans()
        {
            return PartialView("_LoanHome");
        }

        public ActionResult GetPayLoans()
        {
            return PartialView("_PayLoan");
        }
        public ActionResult GetApproveUserLoans()
        {
            return PartialView("_ApproveLoan");
        }
    }
}