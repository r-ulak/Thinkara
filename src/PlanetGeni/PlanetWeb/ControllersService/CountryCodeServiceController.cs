using Common;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{

    public class CountryCodeServiceController : ApiController
    {

        ICountryCodeRepository _repository;
        ICountryLeaderRepository leaderRepo;
        public CountryCodeServiceController(ICountryCodeRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public CountryCodeServiceController()
        {
            _repository = new CountryCodeRepository();
            leaderRepo = new CountryLeaderRepository();
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 604800, MustRevalidate = true)]
        public HttpResponseMessage GetCountryCodes()
        {
            string result = _repository.GetCountryCodes();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;

            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [RequireHttps]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public string GetHostLocation()
        {
            string ip = HttpContext.Current.Request.UserHostAddress;
            return ip;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Authorize]
        [RequireHttps]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetCountryProfileDTO(string countryid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetCountryProfileDTO(countryid);
                StringContent sc = new StringContent(result);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
                return resp;
            }
            else
            {
                return null;
            }

        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Authorize]
        [RequireHttps]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetCountryRankingProfile(string countryid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetCountryRankingProfile(countryid);
                StringContent sc = new StringContent(result);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
                return resp;
            }
            else
            {
                return null;
            }

        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [Authorize]
        [RequireHttps]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetActiveLeadersProfile(string countryid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = leaderRepo.GetActiveLeadersProfile(countryid);
                StringContent sc = new StringContent(result);
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                resp.Content = sc;
                return resp;
            }
            else
            {
                return null;
            }

        }
    }
}
