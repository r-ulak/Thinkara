using AzureServices;
using Common;
using Dao.Models;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Manager.ServiceController;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
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

    public class WebUserServiceController : ApiController
    {

        IWebUserDTORepository _repository;
        AutoCompleteRepository autoRepo;
        WebUserManager manager;
        public WebUserServiceController(IWebUserDTORepository repo)
        {
            _repository = repo;
            autoRepo = new AutoCompleteRepository();
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public WebUserServiceController()
        {
            _repository = new WebUserDTORepository();
            manager = new WebUserManager(_repository);
            autoRepo = new AutoCompleteRepository();
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]

        public WebUserInfo GetWebUserInfo()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            string provider = (HttpContext.Current.Session["loginProvider"]).ToString();
            UserActivityLog activityLog = new UserActivityLog
            {
                UserId = userid,
                IPAddress = HttpContext.Current.Request.UserHostAddress,
                LastLogin = DateTime.UtcNow,
                FirstLogin = DateTime.UtcNow,
                Hit = 1
            };
            Task taskA = Task.Factory.StartNew(() =>
            _repository.TrackUserActivity(activityLog));
            return _repository.GetWebUserInfo(userid, provider);

        }
        [HttpGet]
        [ApiValidateAntiForgeryToken]

        public bool IsThisFirstLogin()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            return _repository.IsThisFirstLogin(userid);
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]

        [CacheOutput(ClientTimeSpan = 900, MustRevalidate = true)]
        public UserProfileDTO GetProfileStat(int profileId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            if (userid > 0)
            {

                return _repository.GetProfileStat(profileId);
            }
            else
            {
                return new UserProfileDTO();
            }
        }


        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public WebUserInfo GetWebUserInfo(int profileId)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            if (userid > 0)
            {

                return _repository.GetWebUserInfo(profileId, string.Empty);
            }
            else
            {
                return new WebUserInfo();
            }

        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public BlobSasUrlDTO GetBlobSasUrl(SasUrlDTO sasurl)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            if (sasurl.SourceType == "ads")
            {
                sasurl.BlobName =
                    Guid.NewGuid().ToString() + userid.ToString() + "." + sasurl.FileType;
            }
            else if (sasurl.SourceType == "partynew")
            {
                sasurl.BlobName =
                    Guid.NewGuid().ToString() + "." + sasurl.FileType;
            }
            else if (sasurl.SourceType == "profile")
            {
                if (sasurl.BlobName == RulesSettings.InitialPicture)
                {
                    sasurl.BlobName =
                        Guid.NewGuid().ToString() + userid.ToString() + "." + sasurl.FileType;
                }
            }
            //TODO: files extesnisons are not accurate becasue when they upload it we use old ext, below code can fix that , but we need to delete old file from blob and update tables
            //int fileExtPos = sasurl.BlobName.LastIndexOf(".");
            //if (fileExtPos >= 0)
            //    sasurl.BlobName = sasurl.BlobName.Substring(0, fileExtPos) + "." + sasurl.FileType;
            if (string.IsNullOrEmpty(sasurl.BlobName))
            {
                sasurl.BlobName =
                Guid.NewGuid().ToString() + userid.ToString() + "." + sasurl.FileType;
            }
            BlobSasUrlDTO blobSasUrl = new BlobSasUrlDTO
            {
                FileName = sasurl.BlobName,
                Url = string.Empty
            };
            blobSasUrl.Url = AzureStorage.GetSasForBlob(sasurl);
            return blobSasUrl;
        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public PostResponseDTO SaveWebUserInfo(WebUserInfoDTO webInfo)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            webInfo.UserId = userid;
            Task taskA = Task.Factory.StartNew(() =>
                manager.ProcessUpdateProfileName(webInfo));
            return new PostResponseDTO
            {
                Message = "Updates Submitted Successfully",
                StatusCode = 200
            };

        }
        public PostResponseDTO SaveWebUserPic(WebUserInfoDTO webInfo)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            webInfo.UserId = userid;
            Task taskA = Task.Factory.StartNew(() =>
            manager.ProcessUpdateProfilePic(webInfo));
            return new PostResponseDTO
            {
                Message = "Updates Submitted Successfully",
                StatusCode = 200
            };

        }

        [HttpPost]
        [ApiValidateAntiForgeryToken]
        public HttpResponseMessage AutoCompleteUserSearch(AutoCompleteDTO autoComplete)
        {
            if (autoComplete.Skip % 10 != 0)
            {
                autoComplete.Skip =
                    ((int)Math.Round(autoComplete.Skip / 10.0)) * 10;
            }
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = autoRepo.AutoCompleteUserSearch(autoComplete);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;

            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        [CacheOutput(ClientTimeSpan = 10, MustRevalidate = true)]
        public HttpResponseMessage AutoCompleteUserSearch(string term)
        {
            AutoCompleteDTO autoComplete = new AutoCompleteDTO
            {
                QueryString = term,
                Skip = 0
            };
            HttpResponseMessage resp = new HttpResponseMessage();
            string result = autoRepo.AutoCompleteUserSearch(autoComplete);
            StringContent sc = new StringContent(result);
            sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            resp.Content = sc;

            return resp;
        }

        [HttpGet]
        [ApiValidateAntiForgeryToken]
        public async Task<HttpResponseMessage> SpotImSSO(string spotName)
        {

            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            return await manager.RegisterSpotImAsync(userid, spotName);
        }

    }
}
