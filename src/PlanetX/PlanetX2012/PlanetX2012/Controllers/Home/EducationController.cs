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

namespace PlanetX2012.Controllers.Home
{
    public class EducationController : Controller
    {
        //
        // GET: /Education/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult JoinSchool(Education educationViewModel)
        {
            try
            {
                using (var db = new PlanetXContext())
                {
                    educationViewModel.UpdatedAt = DateTime.Now;
                    educationViewModel.UserId = Convert.ToInt32(Session["WebUserId"]);
                    db.Educations.Add(educationViewModel);
                    db.SaveChanges();
                    return Json(new { success = 1, ex = "" });

                }


            }
            catch (Exception )
            {

                return Json(new { success = 0, ex = "Server Error" });
            }
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
        [HttpGet]
        public ActionResult EducationModal()
        {

            return PartialView("../PartialViews/Education/EducationModal");
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client)]
        [HttpGet]
        public ActionResult Start()
        {

            return PartialView("../PartialViews/Education/EnrollToSchool");
        }



        public JsonResult Remove(int id)
        {
            try
            {
                PlanetXContext db = new PlanetXContext();
                Education education = db.Educations.First(i => i.EducationId == id);
                db.Educations.Remove(education);
                db.SaveChanges();
                return Json(new { success = 1, educationId = education.EducationId, ex = "" });
            }
            catch (Exception ex)
            {
                return Json(new { success = 0, educationId = id, ex = ex.Message.ToString() });
            }
        }

       
        [HttpGet]
        public ActionResult Manage()
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", Convert.ToInt32(Session["WebUserId"]));

            ManageEducationContent educationContent = new ManageEducationContent();
            educationContent.EducationList = sp.GetSqlData<Education>("GetEducationContent", dictionary);


            return PartialView("../PartialViews/Education/EditEducation", educationContent);
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client, VaryByParam = "*")]
        public JsonResult GetUniversityList(string term)
        {
            IUniversityCodeRepository universityCodeStore = new UniversityCodeRepository();
            var universitylist = universityCodeStore.GetUniversityCodes()
                 .Where(x => x.Name.StartsWith(term, StringComparison.OrdinalIgnoreCase)).Select(x => new
                 {
                     UniversityId = x.UniversityId,
                     University = x.Name.Trim()
                 }).Take(3).OrderByDescending(x => x.UniversityId);

            return Json(universitylist, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client, VaryByParam = "*")]
        public JsonResult GetDegreeTypeList()
        {
            IDegreeCodeRepository degreeCodeStore = new DegreeCodeRepository();
            var degreelist = degreeCodeStore.GetDegreeCodes().Select(x => new
                 {
                     value = x.DegreeType,
                     selected = false,
                     description = " This is a Description",
                     text = x.Degree.Trim()
                 }).OrderByDescending(x => x.value);

            return Json(degreelist, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client, VaryByParam = "*")]
        public JsonResult GetMajorTypeList()
        {
            IMajorCodeRepository majorCodeStore = new MajorCodeRepository();
            var majorlist = majorCodeStore.GetMajorCodes().Select(x => new
                 {
                     value = x.MajorType,
                     selected = false,
                     description = " This is a Description",
                     text = x.Major.Trim()
                 }).OrderByDescending(x => x.value);

            return Json(majorlist, JsonRequestBehavior.AllowGet);
        }
    }
}
