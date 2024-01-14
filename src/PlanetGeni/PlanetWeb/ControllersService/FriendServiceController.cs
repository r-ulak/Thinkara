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
    public class FriendServiceController : ApiController
    {
        IFriendDetailsDTORepository _repository;
        FriendManager manager;
        public FriendServiceController(IFriendDetailsDTORepository repo)
        {
            _repository = repo;
            manager = new FriendManager(_repository);
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public FriendServiceController()
        {
            _repository = new FriendDetailsDTORepository();
            manager = new FriendManager(_repository);
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public HttpResponseMessage GetFriendsDTOs()
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string result = _repository.GetFriendList(userid);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;

            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public HttpResponseMessage GetFriendsProfile(int profileid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetFriendList(profileid);
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
        public PostResponseDTO SendFriendInvite(EmailInviteDTO inviteList)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            inviteList.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessSendFriendInvite(inviteList));
            return new PostResponseDTO
            {
                Message = "Invite Request Successfully Submitted",
                StatusCode = 200
            };
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public IEnumerable<ContactSourceDTO> GetContactSource()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<ContactSourceDTO> contactsource =
                _repository.GetContactSource(userid);
            return contactsource;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public IEnumerable<WebUserDTO> GetFriendSuggestion(FriendSuggestDTO suggestDTO)
        {

            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            suggestDTO.UserId = userid;
            IEnumerable<WebUserDTO> friendsuggestion =
                _repository.GetFriendSuggestion(suggestDTO);
            return friendsuggestion;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO UpdateFriendRelation(FriendRelationDTO friendRelation)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            friendRelation.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessFriendRelation(friendRelation));
            return new PostResponseDTO
            {
                Message = "Soical Circle Update Successfully Submitted",
                StatusCode = 200
            };
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public void IgnoreSuggestion(FriendSuggestDTO friendDTO)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            friendDTO.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => _repository.IgnoreSuggestion(friendDTO));
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO FollwoAllFriend(FollowAllDTO followFriends)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            followFriends.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => manager.ProcessFollowAllFriend(followFriends));
            return new PostResponseDTO
            {
                Message = "Soical Circle Update Successfully Submitted",
                StatusCode = 200
            };
        }
    }
}
