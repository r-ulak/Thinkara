using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Manager.Jobs;
using Newtonsoft.Json;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Manager.ServiceController
{
    public class WebUserManager
    {
        private IWebUserDTORepository _repository;
        private IUserNotificationDetailsDTORepository userNotif;
        private AutoCompleteIndexManager<WebUserIndexDTO> webUserIndex;
        public WebUserManager(IWebUserDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
            webUserIndex =
                 new AutoCompleteIndexManager<WebUserIndexDTO>
                 (
                 AppSettings.RedisHashIndexWebUser,
                 AppSettings.RedisSetIndexWebUser,
                 AppSettings.SPGetWebUserIndexList,
                 new Dictionary<string, object>(),
                 AppSettings.RedisAutocompleteDatabaseId,
                 "UserId",
                 new PropertyInfo[] { typeof(WebUserIndexDTO).GetProperty("EmailId") },
                 new string[] { "CountryId", "Picture", "UserId" }

                 );
        }
        public WebUserManager()
        {
            _repository = new WebUserDTORepository();
            userNotif = new UserNotificationDetailsDTORepository();
            webUserIndex =
                 new AutoCompleteIndexManager<WebUserIndexDTO>
                 (
                 AppSettings.RedisHashIndexWebUser,
                 AppSettings.RedisSetIndexWebUser,
                 AppSettings.SPGetWebUserIndexList,
                 new Dictionary<string, object>(),
                 AppSettings.RedisAutocompleteDatabaseId,
                 "UserId",
                 new PropertyInfo[] { typeof(WebUserIndexDTO).GetProperty("EmailId") },
                 new string[] { "CountryId", "Picture", "UserId" }

                 );
        }

        public void ProcessUpdateProfileName(WebUserInfoDTO webInfo)
        {
            sbyte priority = 0;
            short notificationTypeId = 0;
            StringBuilder parmText = new StringBuilder();
            WebUserIndexDTO currentUserInfo = _repository.GetWebUserIndexDTO(webInfo.UserId);

            if (_repository.UpdateProfileName(webInfo))
            {
                notificationTypeId = AppSettings.ProfileUpdateSuccessNotificationId;
                priority = 4;
                parmText.Append("Name");
                WebUserIndexDTO newUserInfo = currentUserInfo;
                newUserInfo.NameFirst = webInfo.NameFirst;
                newUserInfo.NameLast = webInfo.NameLast;
                UpdateWebUserIndex(currentUserInfo, newUserInfo);
            }
            else
            {
                notificationTypeId = AppSettings.ProfileUpdateFailNotificationId;
                priority = 7;
                parmText.AppendFormat("{0}|{1}", "Name", AppSettings.UnexpectedErrorMsg);
            }
            userNotif.AddNotification(false, string.Empty,
   notificationTypeId, parmText.ToString(), priority, webInfo.UserId);
        }
        public void ProcessUpdateProfilePic(WebUserInfoDTO webInfo)
        {
            sbyte priority = 0;
            short notificationTypeId = 0;
            StringBuilder parmText = new StringBuilder();
            WebUserIndexDTO currentUserInfo = _repository.GetWebUserIndexDTO(webInfo.UserId);
            if (_repository.UpdateProfilePic(webInfo))
            {
                notificationTypeId = AppSettings.ProfileUpdateSuccessNotificationId;
                priority = 4;
                parmText.Append("Picture");
                WebUserIndexDTO newUserInfo = currentUserInfo;
                newUserInfo.Picture = webInfo.Picture;
                UpdateWebUserIndex(currentUserInfo, newUserInfo);
            }
            else
            {
                notificationTypeId = AppSettings.ProfileUpdateFailNotificationId;
                priority = 7;
                parmText.AppendFormat("{0}|{1}", "Picture", AppSettings.UnexpectedErrorMsg);


            }
            userNotif.AddNotification(false, string.Empty,
   notificationTypeId, parmText.ToString(), priority, webInfo.UserId);
        }
        private void UpdateWebUserIndex(WebUserIndexDTO currentUserInfo, WebUserIndexDTO newUserInfo)
        {
            webUserIndex.UpdateIndexItem(currentUserInfo, newUserInfo);
        }

        public void AddWebUserIndex(WebUser userInfo)
        {
            webUserIndex.AddIndexItem(
                new WebUserIndexDTO()
                {
                    CountryId = userInfo.CountryId,
                    EmailId = userInfo.EmailId,
                    NameFirst = userInfo.NameFirst,
                    NameLast = userInfo.NameLast,
                    Picture = RulesSettings.InitialPicture,
                    UserId= userInfo.UserId,
                    FullName= userInfo.NameFirst + " "+ userInfo.NameLast
                });


        }

        public async Task<HttpResponseMessage> RegisterSpotImAsync(int userId, string spotName)
        {
            WebUserDTO webuserDTO = _repository.GetUserPicFName(userId);
            SpotIMDTO spotuserInfo = new SpotIMDTO(webuserDTO.FullName, userId, AppSettings.AzureProfilePicUrl + webuserDTO.Picture);
            SpotIMPostDTO spotPostDTO = new SpotIMPostDTO
            {
                spot_to_join = spotName,
                user_info = spotuserInfo
            };

            string postBody = JsonConvert.SerializeObject(spotPostDTO);

            HttpResponseMessage aResponse;

            using (HttpClient client = new HttpClient())
            {
                Uri theUri = new Uri(AppSettings.SpoImRegisterUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppSettings.SpoImAccessToken);
                client.DefaultRequestHeaders.Host = theUri.Host;
                aResponse = await client.PostAsync(theUri, new StringContent(postBody, Encoding.UTF8, "application/json"));

            }
            return aResponse;
        }
    }
}


