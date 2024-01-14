using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Manager.ServiceController;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
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
    [Authorize]
    [RequireHttps]
    public class ElectionServiceController : ApiController
    {

        IElectionDTORepository _repository;
        ElectionManager manager;
        public ElectionServiceController(IElectionDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public ElectionServiceController()
        {
            _repository = new ElectionDTORepository();
            manager = new ElectionManager(_repository);
        }
        [ApiValidateAntiForgeryToken]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetCurrentElectionTerm(string countyrId)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetCurrentElectionTermJson(countyrId);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }

        [ApiValidateAntiForgeryToken]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetPoliticalPostions()
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetPoliticalPostionsJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public PostResponseDTO ApplyForOffice(RunForOfficeDTO runforOffice)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            runforOffice.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessApplyForOffice(runforOffice));
            return new PostResponseDTO
            {
                Message = "Run For Office Application Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessApplyForOffice(RunForOfficeDTO runforOffice)
        {
            manager.ProcessAppForOffice(runforOffice);
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public RunForOfficeTicketDTO GetRunForOfficeTicket(string taskId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            RunForOfficeTicketDTO runforOffice =
                _repository.GetRunForOfficeTicket(taskId, userid);
            return runforOffice;
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public int[] GetCandidateAgenda(CandidateAgendaDTO candidateagenda)
        {
            int[] result =
                _repository.GetCandidateAgenda(candidateagenda.UserId, candidateagenda.ElectionId);
            return result;
        }

        [ApiValidateAntiForgeryToken]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetElectionLast12(string countryId)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetElectionLast12Json(countryId);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage GetCandidateByElection(CandidateSearchDTO candidateSearch)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetCandidateByElection(candidateSearch);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage GetCurrentVotingInfo(CandidateVotingDTO candidateVoting)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetCurrentVotingInfo(candidateVoting);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO DonateElection(PayWithTaxDTO donation)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            donation.SourceId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessElectionDonation(donation));
            return new PostResponseDTO
            {
                Message = "Donation Successfully Submitted",
                StatusCode = 200
            };
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public IEnumerable<ElectionVotingDTO> GetElectionResult(int candidate)
        {
            IEnumerable<ElectionVotingDTO> voteresult =
                _repository.GetElectionResult(candidate);
            return voteresult;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO QuitElection()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            QuitElectionDTO quit = new QuitElectionDTO
            {
                UserId = userid
            };
            Task taskA = Task.Factory.StartNew(() => manager.ProcessQuitElection(quit));
            return new PostResponseDTO
            {
                Message = "Request To Withdraw Election Submitted",
                StatusCode = 200
            };
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO VoteElection(CandidateVotingDTO candidates)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            candidates.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessVoteElection(candidates));
            return new PostResponseDTO
            {
                Message = "Request To Vote For Election Submitted",
                StatusCode = 200
            };
        }

    }
}