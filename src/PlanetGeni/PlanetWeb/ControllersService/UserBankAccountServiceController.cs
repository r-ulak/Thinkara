using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Manager.ServiceController;
using Newtonsoft.Json;
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
    public class UserBankAccountServiceController : ApiController
    {
        IUserBankAccountDTORepository _repository;
        BankAccountManager manager;
        public UserBankAccountServiceController(IUserBankAccountDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public UserBankAccountServiceController()
        {
            _repository = new UserBankAccountDTORepository();
            manager = new BankAccountManager(_repository);
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        public UserBankAccount GetUserBankDetails()
        {
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            UserBankAccount userbank = _repository.GetUserBankDetails(userId);
            return userbank;
        }
        [ApiValidateAntiForgeryToken]
        [HttpGet]
        public decimal GetNetWorth(int profileId)
        {
            return _repository.GetNetWorth(profileId);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public IEnumerable<BankViewDTO> GetBankViewDetails()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            return
                 _repository.GetBankAccountViewInfo(userid);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetTopTenRichest()
        {
            string result = _repository.GetTopTenRichestJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public IEnumerable<CapitalTransactionDTO> GetBankStatement(DateTime? lastDateTime = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<CapitalTransactionDTO> capitalTrn =
                _repository.GetBankStatement(userid, lastDateTime);
            return capitalTrn;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public HttpResponseMessage GetMetalPrices()
        {
            string result = _repository.GetMetalPrices();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetFundTypes()
        {
            string result = _repository.GetAllFundTypes();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveSellMetal(BuySellMetalDTO
                buysellMetal)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            buysellMetal.UserId = userid;
            buysellMetal.OrderType = "S";
            Task taskA = Task.Factory.StartNew(() => manager.ProcessBuySellMetal(buysellMetal));
            return new PostResponseDTO
            {
                Message = "Sell Metal Cart Successfully Submitted",
                StatusCode = 200
            };
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveBuyMetal(BuySellMetalDTO buysellMetal)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            buysellMetal.OrderType = "B";
            buysellMetal.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessBuySellMetal(buysellMetal));

            return new PostResponseDTO
            {
                Message = "Buy Metal Cart Successfully Submitted",
                StatusCode = 200
            };
        }

    }
}
