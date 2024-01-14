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
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class CasinoServiceController : ApiController
    {
        ICasinoDTORepository _repository;
        CasinoManager manager;
        private Random rand = new Random();
        public CasinoServiceController(ICasinoDTORepository repo)
        {
            _repository = repo;
            manager = new CasinoManager(_repository);
        }

        public CasinoServiceController()
        {
            _repository = new CasinoDTORepository();
            manager = new CasinoManager(_repository);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public SlotNumber GetSpinSlotNumber(decimal betAmount)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryid = HttpContext.Current.Session["CountryId"].ToString();
            SlotNumber winNumber = new SlotNumber
            {
                Number1 = rand.Next(0, 300) % 7,
                Number2 = rand.Next(0, 200) % 7,
                Number3 = rand.Next(0, 100) % 7,
                BetAmount = betAmount,
                UserId = userid,
                CountryId = countryid

            };
            Task taskA = Task.Factory.StartNew(() => manager.ProcessSlotSpin(winNumber));
            return winNumber;
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public int GetRouletNumber(RouletteDTO roulette)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryid = HttpContext.Current.Session["CountryId"].ToString();
            roulette.WinNumber = rand.Next(0, 8);
            roulette.UserId = userid;
            roulette.CountryId = countryid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessRoulete(roulette));

            return roulette.WinNumber;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 0, MustRevalidate = true)]
        public HttpResponseMessage GetSlotMachineThreeList()
        {
            string result = _repository.GetSlotMachineThreeList();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }
    }
}
