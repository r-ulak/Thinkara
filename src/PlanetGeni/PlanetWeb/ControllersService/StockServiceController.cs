using Common;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class StockServiceController : ApiController
    {
        IStockDTORepository _repository;
        StockManager manager;
        public StockServiceController(IStockDTORepository repo)
        {
            _repository = repo;
            manager = new StockManager(_repository);
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public StockServiceController()
        {
            _repository = new StockDTORepository();
            manager = new StockManager(_repository);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetCurrentStock()
        {
            string result = _repository.GetCurrentStockJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 7200, MustRevalidate = true)]
        public HttpResponseMessage GetStockForeCast()
        {
            string result = _repository.GetStockForecast();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetTopTenStockOwner()
        {
            string result = _repository.GetTopTenStockOwnerJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [ApiValidateAntiForgeryToken]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public IEnumerable<UserStockDTO> GetStockByUser
            (DateTime? lastDateTime = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<UserStockDTO> userStockDTOs =
                _repository.GetStockByUser(userid, lastDateTime);

            return userStockDTOs;
        }

        [ApiValidateAntiForgeryToken]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public IEnumerable<StockSummaryDTO> GetStockSummary()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<StockSummaryDTO> userStockSummary =
                _repository.GetStockSummary(userid);

            return userStockSummary;
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public IEnumerable<StockTradeDTO> GetStockTradeByUser
            (DateTime? lastDateTime = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<StockTradeDTO> stockTradeDTOs =
                _repository.GetStockTradeByUser(userid, lastDateTime);

            return stockTradeDTOs;
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveSellStockCart(BuySellStockDTO[]
      stockCartList)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => manager.ProcessSellStockCart(stockCartList, userid));
            return new PostResponseDTO
            {
                Message = "Sell Property Cart Successfully Submitted",
                StatusCode = 200
            };
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveBuyStockCart(BuySellStockDTO[] stockList)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() => manager.ProcessBuyStockCart(stockList, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Buy Stock Cart Successfully Submitted",
                StatusCode = 200
            };
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveOrderStockCart(Guid[] stockList)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => manager.ProcessOrderStockCart(stockList, userid));
            return new PostResponseDTO
            {
                Message = "Buy Stock Cart Successfully Submitted",
                StatusCode = 200
            };
        }

    }
}
