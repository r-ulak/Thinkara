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
    public class RobberyServiceController : ApiController
    {
        RobberyDTORepository _repository;
        RobberyManager manager;
        public RobberyServiceController(RobberyDTORepository repo)
        {
            _repository = repo;
            manager = new RobberyManager(_repository);
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public RobberyServiceController()
        {
            _repository = new RobberyDTORepository();
            manager = new RobberyManager(_repository);
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SnipeCash(CrimeIncidentDTO incident)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            incident.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessPickPocketing(incident));
            return new PostResponseDTO
            {
                Message = "Sniping of Cash Successfully Submitted",
                StatusCode = 200
            };
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO RobProperty(CrimeIncidentDTO incident)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            incident.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessRobberyProperty(incident));
            return new PostResponseDTO
            {
                Message = "Robbing Property Successfully Submitted",
                StatusCode = 200
            };
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public decimal AllowedMaxCashSnipe(int friendId)
        {
            return _repository.GetMaxAllowedPickPocketing(friendId);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 1000, MustRevalidate = true)]
        public HttpResponseMessage GetCrimeReportByIncident(Guid incident)
        {
            string result = _repository.GetCrimeReportByIncident(incident);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetCrimeReportByUser()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string result = _repository.GetCrimeReportByUser(userid);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage resp = new HttpResponseMessage();
            resp.Content = sc;
            return resp;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO ReportSuspect(CrimeIncidentDTO incident)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            incident.SuspectReportingUserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessSuspectReporting(incident));
            return new PostResponseDTO
            {
                Message = "Suspect Reporting Successfully Submitted",
                StatusCode = 200
            };
        }

    }
}
