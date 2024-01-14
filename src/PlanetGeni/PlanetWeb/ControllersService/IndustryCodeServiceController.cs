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
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]

    public class IndustryCodeServiceController : ApiController
    {

        IIndustryCodeRepository _repository;
        public IndustryCodeServiceController(IIndustryCodeRepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public IndustryCodeServiceController()
        {
            _repository = new IndustryCodeRepository();
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]

        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetIndustryCodes()
        {
            string result = _repository.GetIndustryCodes();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;

            return resp;
        }
    }
}
