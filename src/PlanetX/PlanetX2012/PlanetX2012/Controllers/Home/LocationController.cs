using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DAO.Models;
using PlanetX2012.DataCache;
using PlanetX2012.Models.DAO;

namespace PlanetX2012.Controllers.Home
{
    public class LocationController : Controller
    {
        private PlanetXContext db = new PlanetXContext();

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client, VaryByParam = "*")]
        public JsonResult GetCityList(string term)
        {
            ICityCodeRepository cityCodeStore = new CityCodeRepository();
            var citylist = cityCodeStore.GetCityCodes()
                 .Where(x => x.City.StartsWith(term, StringComparison.OrdinalIgnoreCase)).Select(x => new
                     {
                         CountryId = x.CountryId,
                         CityId = x.CityId,
                         City = x.City.Trim()
                     }).Take(3).OrderByDescending(x => x.CountryId);

            return Json(citylist, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Client, VaryByParam = "*")]
        public JsonResult GetCountryList(string term)
        {
            ICountryCodeRepository countryCodeStore = new CountryCodeRepository();
            var countrylist = countryCodeStore
                .GetCountryCodes().Where(x => x.Code.StartsWith(term, StringComparison.OrdinalIgnoreCase)).Select(x => new
                    {
                        CountryId = x.CountryId,
                        Code = x.Code.Trim()
                    }); ;

            return Json(countrylist, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.Any)]
        public JsonResult GetAllCountryList()
        {
            ICountryCodeRepository countryCodeStore = new CountryCodeRepository();
            var countrylist = countryCodeStore
                .GetCountryCodes().Select(x => new
                    {
                        CountryId = x.CountryId,
                        Code = x.Code.Trim()
                    });

            return Json(countrylist, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProvinceList(string term)
        {
            var results = from c in db.ProvinceCodes
                          where c.Province.StartsWith(term, StringComparison.OrdinalIgnoreCase)
                          select new { label = c.Province, id = c.ProvinceId };
            return Json(results.ToArray(), JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}