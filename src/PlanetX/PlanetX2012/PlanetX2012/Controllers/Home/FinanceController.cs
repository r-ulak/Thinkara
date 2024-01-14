using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DAO.Models;
using PlanetX2012.DataCache;
using PlanetX2012.Models.ContentModel;
using PlanetX2012.Models.DAO;

namespace PlanetX2012.Controllers.Home.Finance
{
    public class FinanceController : Controller
    {
        //
        // GET: /Finance/
        PlanetXContext db = new PlanetXContext();

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Finance/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Finance/Create

        public ActionResult CreateNewBusiness()
        {
            BusinessContent businessViewModel = new BusinessContent();

            businessViewModel.CreateModal = true;
            return PartialView("../PartialViews/Finance/Modal/NewBusiness", businessViewModel);
        }

        //
        // POST: /Finance/Create

        [HttpPost]
        public ActionResult CreateNewBusiness(BusinessContent businessViewModel)
        {
            try
            {
                // TODO: Add insert logic here
                if (!ModelState.IsValid)
                    return PartialView("../PartialViews/Finance/Modal/NewBusiness", businessViewModel);

                businessViewModel.CreateModal = false;
                PlanetXContext db = new PlanetXContext();
                businessViewModel.Business.UpdatedAt = DateTime.Now;
                businessViewModel.Business.UserId = Convert.ToInt32(Session["WebUserId"]);
                db.Businesses.Add(businessViewModel.Business);
                db.SaveChanges();

                return PartialView("../PartialViews/Finance/Modal/NewBusiness", businessViewModel);
            }
            catch (Exception )
            {
                return View();
            }
        }

        //
        // GET: /Finance/Edit/5

        public ActionResult Manage()
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", Convert.ToInt32(Session["WebUserId"]));
            ManageBusinessContent businessContent = new ManageBusinessContent();
            businessContent.BusinessList = sp.GetSqlData<Business>("GetBusinessContent", dictionary);

            return PartialView("../PartialViews/Finance/Modal/EditBusiness", businessContent);
        }

        //
        // POST: /Finance/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        public JsonResult Remove(int id)
        {
            try
            {
                PlanetXContext db = new PlanetXContext();
                Business business = db.Businesses.First(i => i.BusinessId == id);
                StringBuilder errorlist = new StringBuilder("<ul>");
                if (business.LoanFromBusinesses.Count > 0)
                {
                    errorlist.Append("<li>Cannot Remove the Business that has Loans</li>");
                    errorlist.Append("<li>Cannot Remove the Business that has Loans</li>");
                    errorlist.Append("</ul>");
                    return Json(new { success = 0, businessId = business.BusinessId, ex = errorlist.ToString() });

                }
                db.Businesses.Remove(business);

                db.SaveChanges();

                return Json(new { success = 1, businessId = business.BusinessId, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = 0, businessId = id, ex = ex.Message.ToString() });
            }
        }
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public ActionResult BusinessModal()
        {
            BusinessContent businessViewModel = new BusinessContent();
            businessViewModel.CreateModal = true;

            return PartialView("../PartialViews/Finance/Modal/BusinessModal", businessViewModel);
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
        public JsonResult GetBusinessTypeList()
        {
            IBusinessCodeRepository businessCodeStore = new BusinessCodeRepository();

            return Json(businessCodeStore.GetBusinessCodes().Select(x => new
            {
                text = x.Code,
                value = x.BusinessTypeId,
                selected = false,
                description = x.Description,
                imageSrc = x.Picture,

            }), JsonRequestBehavior.AllowGet);

        }


        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client, VaryByParam = "*")]
        public JsonResult GetBusinessSubTypeList(int businessTypeId)
        {
            IBusinessSubCodeRepository businessSubCodeStore = new BusinessSubCodeRepository();

            return Json(businessSubCodeStore.GetBusinessSubCodes(businessTypeId).Select(x => new
            {
                text = x.Code,
                value = x.BusinessSubtypeId,
                selected = false,
                description = x.Description,
                imageSrc = x.Picture,
                success = true

            }), JsonRequestBehavior.AllowGet);

        }

        private IEnumerable<Business> GetBusinessList(int webUserId)
        {
            PlanetXContext db = new PlanetXContext();
            IEnumerable<Business> businessList =
             (from q in db.Businesses
              where q.UserId == webUserId
              select q);
            return businessList;
        }

    }
}
