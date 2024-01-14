using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Manager.ServiceController;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Authorize]
    [RequireHttps]
    public class AdvertisementServiceController : ApiController
    {
        IAdvertisementDetailsDTORepository _repository;
        AdsManager manager;
        public AdvertisementServiceController(IAdvertisementDetailsDTORepository repo)
        {
            _repository = repo;
            manager = new AdsManager(_repository);
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public AdvertisementServiceController()
        {
            _repository = new AdvertisementDetailsDTORepository();
            manager = new AdsManager(_repository);
        }




        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetAdsType()
        {
            string result = _repository.GetAdsTypesJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;

            return resp;
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveAds(AdvertisementPostDTO adsDetail)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            adsDetail.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessAds(adsDetail));
            return new PostResponseDTO
            {
                Message = "Ads Successfully Submitted",
                StatusCode = 200
            };
        }

    }
}
