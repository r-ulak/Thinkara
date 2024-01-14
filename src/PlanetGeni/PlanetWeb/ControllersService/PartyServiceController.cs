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
    public class PartyServiceController : ApiController
    {

        IPartyDTORepository _repository;
        public PartyServiceController(IPartyDTORepository repo)
        {
            _repository = repo;
        }
        public PartyServiceController()
        {
            _repository = new PartyDTORepository();
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public IEnumerable<MyPoliticalPartyDTO> GetUserParties()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<MyPoliticalPartyDTO> myParties =
                _repository.GetUserParties(userid);
            return myParties;
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public string GetUserPartyName()
        {
            int userId = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            return _repository.GetMyParties(userId);
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public PoliticalParty GetUserPartById(Guid partyId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            PoliticalParty party;
            if (userid > 0 && partyId != Guid.Empty)
            {
                party =
                   _repository.GetPartyById(partyId.ToString());
                return party;
            }
            return null;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public IEnumerable<MyPoliticalPartyDTO> GetPastUserParties()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<MyPoliticalPartyDTO> myParties =
                _repository.GetPastUserParties(userid);
            return myParties;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]

        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public HttpResponseMessage GetAllUserParty(int profileid)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            HttpResponseMessage resp = new HttpResponseMessage();
            if (userid > 0)
            {
                string result = _repository.GetAllUserParty(profileid);
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
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage GetPartyAgendasJson(Guid partyId)
        {
            if (partyId == Guid.Empty)
            {
                return null;
            }
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetPartyAgendasJson(partyId.ToString());
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetAllPartyMemberJson(string partyid)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetAllPartyMemberJson(partyid);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetAllPoliticalAgendaJson()
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetAllPoliticalAgendaJson();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public HttpResponseMessage GetPartyCoFounders(string partyId)
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetPartyCoFounders(partyId);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage GetPartyMemberByPage(GetPartyMemberTypeDTO partyMemberType)
        {
            partyMemberType.MemberType = "M";
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetPartyMembers(partyMemberType);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public bool IsUniquePartyName(UniquePartyDTO partycheck)
        {
            if (partycheck.partyName.ToLower().Trim() == partycheck.originalName.ToLower().Trim())
            {
                return true;
            }
            return _repository.IsUniquePartyName(partycheck.partyName, partycheck.countryId);
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public bool IsUniquePartyName(string partyName, string countryId, string originalName)
        {
            if (originalName != null)
            {
                if (partyName.ToLower().Trim() == originalName.ToLower().Trim())
                {
                    return true;
                }
            }
            return _repository.IsUniquePartyName(partyName, countryId);
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage GetPartyCoFounderByPage(GetPartyMemberTypeDTO partyMemberType)
        {
            partyMemberType.MemberType = "C";
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetPartyMembers(partyMemberType);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage GetPartyFounderByPage(GetPartyMemberTypeDTO partyMemberType)
        {
            partyMemberType.MemberType = "F";
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetPartyMembers(partyMemberType);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public PartySummaryDTO GetUserPartySummary()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            PartySummaryDTO result = _repository.GetUserPartySummary(userid);
            return result;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 100, MustRevalidate = true)]
        public IEnumerable<WebUserContactDTO> GetEmailInvitationList(string lastEmailId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<WebUserContactDTO> result = _repository.GetEmailInvitationList(userid, lastEmailId);
            return result;
        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 3600, MustRevalidate = true)]
        public HttpResponseMessage GetTopTenPartyByMember()
        {
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.GetTopTenPartyByMember();
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO StartParty(StartPartyDTO startParty)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            startParty.InitatorId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessStartParty(startParty));
            return new PostResponseDTO
            {
                Message = "Party Application Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessStartParty(StartPartyDTO startParty)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessStartParty(startParty);
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO ManageParty(StartPartyDTO manageParty)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            manageParty.InitatorId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessManageParty(manageParty));
            return new PostResponseDTO
            {
                Message = "Party Application Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessManageParty(StartPartyDTO manageParty)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessManageParty(manageParty);
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO ManagePartyUploadPartyLogo(StartPartyDTO manageParty)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            manageParty.InitatorId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessManageUploadPartyLogo(manageParty));
            return new PostResponseDTO
            {
                Message = "Party Application Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessManageUploadPartyLogo(StartPartyDTO manageParty)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessManagePartyUpload(manageParty);
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO RequestEjectMember(EjectPartyDTO ejectionList)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => ProcessEjectMember(ejectionList));
            return new PostResponseDTO
            {
                Message = "Party Application Successfully Submitted",
                StatusCode = 200
            };
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SendPartyInvite(PartyInviteDTO partyInvite)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            partyInvite.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessPartyInvite(partyInvite));
            return new PostResponseDTO
            {
                Message = "Party Invitation Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessPartyInvite(PartyInviteDTO partyInvite)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessPartyInvite(partyInvite);
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO DonateParty(DonatePartyDTO donation)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            donation.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessDonation(donation));
            return new PostResponseDTO
            {
                Message = "Donation Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessDonation(DonatePartyDTO donation)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessDonation(donation);
        }
        private void ProcessEjectMember(EjectPartyDTO ejectionDto)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessEjectMember(ejectionDto);

        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage SearchParty(PartySearchDTO searchCriteria)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            if (userid == 0)
            {
                return new HttpResponseMessage();
            }
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = _repository.SearchParty(searchCriteria);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;
            return resp;
        }
        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO RequestCloseParty()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => ProcessRequestCloseParty(userid));
            return new PostResponseDTO
            {
                Message = "Party Application Successfully Close",
                StatusCode = 200
            };
        }
        private void ProcessRequestCloseParty(int userId)
        {
            ClosePartyDTO closeParty = new ClosePartyDTO
            {
                InitatorId = userId
            };
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessRequestCloseParty(closeParty);
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO RequestNominationParty(PartyNominationDTO nominationParty)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            nominationParty.InitatorId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessRequestNominationParty(nominationParty));
            return new PostResponseDTO
            {
                Message = "Party Nomination Successfully Submitted",
                StatusCode = 200
            };
        }
        private void ProcessRequestNominationParty(PartyNominationDTO nominationParty)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessRequestNominationParty(nominationParty);

        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO RequestJoinParty(JoinRequestPartyDTO[] joinParty)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() => ProcessRequestJoinParty(joinParty, userid));
            return new PostResponseDTO
            {
                Message = "Request To Join Party Submitted",
                StatusCode = 200
            };
        }
        private void ProcessRequestJoinParty(JoinRequestPartyDTO[] joinParty, int userId)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessRequestJoinParty(joinParty, userId);

        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO QuitParty()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            QuitPartyDTO quitParty = new QuitPartyDTO();
            quitParty.UserId = userid;
            Task taskA = Task.Factory.StartNew(() => ProcessQuitParty(quitParty));
            return new PostResponseDTO
            {
                Message = "Request To Quit Party Submitted",
                StatusCode = 200
            };
        }
        private void ProcessQuitParty(QuitPartyDTO quitParty)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessQuitParty(quitParty);

        }
    }
}