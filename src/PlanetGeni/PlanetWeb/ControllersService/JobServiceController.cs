using Common;
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
    public class JobServiceController : ApiController
    {
        IJobDTORepository _repository;
        private UserJobManager manager;
        public JobServiceController(IJobDTORepository repo)
        {
            _repository = repo;
            manager = new UserJobManager(_repository);
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public JobServiceController()
        {
            _repository = new JobDTORepository();
            manager = new UserJobManager(_repository);
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public IEnumerable<UserJobCodeDTO> GetCurrentJobs()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<UserJobCodeDTO> myjobs =
                _repository.GetCurrentJobs(userid);
            return myjobs;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage SearchJob(JobSearchDTO jobCriteria)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.SearchJob(jobCriteria, countryId, userid);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;

            return resp;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public JobOverTimeCheckInDTO JobOverTimeCheckIn(JobOverTimeCheckInDTO jobOverTimeCheckIn)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            return _repository.JobOverTimeCheckIn(userid, jobOverTimeCheckIn);
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO QuitJob(string[] taskIds)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => manager.ProcessQuitJobs(taskIds, userid));

            return new PostResponseDTO
            {
                Message = "Quit Job Successfully Submitted",
                StatusCode = 200
            };
        }
        public PostResponseDTO WithDrawJob(string[] taskIds)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => manager.ProcessWithDrawJobs(taskIds, userid));

            return new PostResponseDTO
            {
                Message = "WithDraw Job Successfully Submitted",
                StatusCode = 200
            };
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public JobSummaryDTO GetJobSummaryDTO()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            JobSummaryDTO result = _repository.GetJobSummary(userid);
            return result;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetTopTenIncomeSalary()
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetTopTenIncomeSalary();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public IEnumerable<UserJobCodeDTO> GetJobHistory
               (DateTime? parmlastDateTime = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<UserJobCodeDTO> jobhistory =
                    _repository.GetJobHistory(userid, parmlastDateTime);
            return jobhistory;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public HttpResponseMessage GetJobProfile(int profileid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetJobProfile(profileid);
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
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveApplyJobs(ApplyJobCodeDTO[] applyJobList)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string countryId = (HttpContext.Current.Session["CountryId"].ToString());
            Task taskA = Task.Factory.StartNew(() => manager.ProcessSaveApplyJobs(applyJobList, userid, countryId));
            return new PostResponseDTO
            {
                Message = "Job Application Successfully Submitted",
                StatusCode = 200
            };
        }

    }
}
