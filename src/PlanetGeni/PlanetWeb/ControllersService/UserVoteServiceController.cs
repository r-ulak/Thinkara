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
    public class UserVoteServiceController : ApiController
    {
        IUserVoteDTORepository _repository;
        public UserVoteServiceController(IUserVoteDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public UserVoteServiceController()
        {
            _repository = new UserVoteDTORepository();
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public UserVoteDTO GetVotingDetails(string taskId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            UserVoteDTO getVotingDTO = _repository.GetVotingDetails(taskId, userid);
            return getVotingDTO;
        }

        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public PostResponseDTO SaveVoteResponse(VoteResponseDTO voteResponse)
        {
            if (voteResponse.ChoiceRadioId > 0)
            {
                voteResponse.ChoiceIds = new int[1];
                voteResponse.ChoiceIds[0] = voteResponse.ChoiceRadioId;
            }
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() =>
                ProcessVotingResponse(voteResponse, userid));
            return new PostResponseDTO
            {
                Message = "Vote Successfully Submitted",
                StatusCode = 200
            };
        }

        private void ProcessVotingResponse(VoteResponseDTO voteResponse, int userid)
        {
            UserVoteManager voteManager = new UserVoteManager();
            voteManager.ProcessVotingResponse(voteResponse, userid);
        }
    }
}
