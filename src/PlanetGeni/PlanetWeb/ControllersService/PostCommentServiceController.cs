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

namespace PlanetWeb.Controllers
{
    [ApiValidateAntiForgeryToken]
    [Authorize]
    [RequireHttps]
    public class PostCommentServiceController : ApiController
    {
        IPostCommentDTORepository _repository;
        IWebUserDTORepository _webuserrepository = new WebUserDTORepository();
        IPartyDTORepository _partyrepository = new PartyDTORepository();
        PostCommentManager manager;


        public PostCommentServiceController(IPostCommentDTORepository repo)
        {
            _repository = repo;
            manager = new PostCommentManager(_repository);
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public PostCommentServiceController()
        {
            _repository = new PostCommentDTORepository();
            manager = new PostCommentManager(_repository);
        }


        [HttpPost]
        public PostCommentDTO GetPostCommentList(GetPostDTO postDto)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            postDto.UserId = userid;
            postDto.CountryId = _webuserrepository.GetCountryId(userid);
            postDto.PartyId = _partyrepository.GetActivePartyId(userid);

            PostCommentDTO postCommentDTOs =
                _repository.GetPostCommentList(postDto);
            return postCommentDTOs;
        }

        [HttpGet]
        public IEnumerable<CommentDTO> GetMoreCommentList(string postId, DateTime? lastDateTime = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]); IEnumerable<CommentDTO> commentDTOs =
                _repository.GetMoreCommentList(postId, lastDateTime, userid);
            return commentDTOs;
        }

        [HttpGet]
        public IEnumerable<CommentDTO> GetMoreChildCommentList(string parentCommentId, DateTime? lastDateTime = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]); IQueryable<CommentDTO> childCommentDTOs =
                _repository.GetMoreChildCommentList(userid, parentCommentId, lastDateTime);
            return childCommentDTOs;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public CommentDTO SavePostComment(CommentDTO postCommentDetails)
        {

            postCommentDetails.PostCommentId = Guid.NewGuid();
            postCommentDetails.UserId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            postCommentDetails.CreatedAt = DateTime.UtcNow;
            Task taskA = Task.Factory.StartNew(() => ProcessCommentPost(postCommentDetails));

            return postCommentDetails;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SavePostVote(UserDigDTO userDig)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            userDig.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => _repository.SaveUserDig(userDig));
            return new PostResponseDTO
            {
                StatusCode = 200
            };
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveSpot(BuySpotDTO spotDetails)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            spotDetails.UserId = userid;
            manager.ProcessBuySpot(spotDetails);
            return new PostResponseDTO
            {
                StatusCode = 200
            };
        }
        private void ProcessCommentPost(CommentDTO postCommentDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = _repository.SavePostComment(postCommentDetails);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessPost");
            }
        }

    }
}
