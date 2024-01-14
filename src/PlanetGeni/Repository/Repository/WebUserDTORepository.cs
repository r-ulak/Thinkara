using Common;
using ContactsManager;
using Dao.Models;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class WebUserDTORepository : IWebUserDTORepository
    {
        private IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
        private StoredProcedure spContext = new StoredProcedure();
        private IRedisCacheProvider cache { get; set; }
        public WebUserDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public WebUserDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public WebUser GetWebUser(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            WebUser user =
                spContext.GetByPrimaryKey<WebUser>(dictionary);
            return user;
        }
        public WebUserIndexDTO GetWebUserIndexDTO(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            WebUserIndexDTO user =
                spContext.GetSqlDataSignleRow<WebUserIndexDTO>(AppSettings.SPGetWebUserIndexDTO, dictionary);
            return user;
        }
        public Dictionary<string, string> GetWebUserDictionary(int userId)
        {
            Dictionary<string, string> webUserdata = new Dictionary<string, string>();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            WebUserCache userCache =
             spContext.GetSqlDataSignleRow<WebUserCache>(AppSettings.SPGetWebUserCache, dictionary);
            IPartyDTORepository partyRepo = new PartyDTORepository();
            PartyMember partyMember = partyRepo.GetActiveUserParty(userId);


            PopulateDictionary(webUserdata, userCache);
            PopulateDictionary(webUserdata, partyMember);


            cache.SetHashDictionary(AppSettings.RedisHashWebUser + userId, webUserdata);
            cache.ExpireKey(AppSettings.RedisHashWebUser + userId, AppSettings.UserProfileCacheLimit);

            return webUserdata;
        }

        private void PopulateDictionary(Dictionary<string, string> webUserdata, object obj)
        {
            string field = string.Empty;
            string fieldValue = string.Empty;
            foreach (var propertyName in obj.GetType().GetProperties())
            {
                field = propertyName.Name;
                var propertyvalue = obj.GetType().GetProperty(field).GetValue(obj, null);
                if (propertyvalue == null)
                {
                    fieldValue = "";
                }
                else
                {
                    fieldValue = propertyvalue.ToString();
                }
                try
                {
                    webUserdata.Add(field, fieldValue);
                }
                catch (Exception)
                {

                }

            }


        }
        public string GetFirstName(int userId)
        {
            string userFirstName =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "NameFirst");
            if (userFirstName == null)
            {
                userFirstName = GetWebUserDictionary(userId)["NameFirst"];
            }

            return userFirstName;
        }
        public string GetFullName(int userId)
        {
            string userFullName =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "FullName");
            if (userFullName == null)
            {
                userFullName = GetWebUserDictionary(userId)["FullName"];
            }

            return userFullName;
        }
        public string GetCountryId(int userId)
        {
            string countryId =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "CountryId");
            if (countryId == null)
            {
                countryId = GetWebUserDictionary(userId)["CountryId"];
            }

            return countryId;
        }
        public string isLeader(int userId)
        {
            string leader =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "IsLeader");
            if (leader == null)
            {
                return (GetWebUserDictionary(userId)["IsLeader"]);
            }

            return (leader);
        }
        public string GetUserPicture(int userId)
        {
            string picture =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "Picture");
            if (picture == null)
            {
                picture = GetWebUserDictionary(userId)["Picture"];
            }

            return picture;
        }
        public WebUserDTO GetUserPicFName(int userId)
        {
            string[] fields = new string[] { "Picture", "FullName" };
            WebUserDTO webdata = new WebUserDTO();

            string[] webuserData =
                cache.GetMultipleHash(AppSettings.RedisHashWebUser + userId, fields);
            if (Array.FindIndex(webuserData, i => i == null || i == string.Empty) > -1)
            {
                Dictionary<string, string> webuserdict = GetWebUserDictionary(userId);
                webdata.Picture = webuserdict["Picture"];
                webdata.FullName = webuserdict["FullName"];
                return webdata;
            }
            webdata.Picture = webuserData[0];
            webdata.FullName = webuserData[1];

            return webdata;
        }

        public WebUserInfo GetWebUserInfo(int userId, string providerInfo)
        {
            string[] fields = new string[] { "Picture", "FullName", "OnlineStatus", "CountryId", "UserLevel", "UserId", "NameLast", "NameFirst", "IsLeader" };
            IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
            Task<decimal> taskbankAc =
                 Task<decimal>.Factory.StartNew(() => bankRepo.GetNetWorth(userId));
            WebUserInfo webdata = new WebUserInfo();
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            string[] webuserData =
                cache.GetMultipleHash(AppSettings.RedisHashWebUser + userId, fields);
            webdata.UserId = userId;

            if (Array.FindIndex(webuserData, i => i == null || i == string.Empty) > -1 || Convert.ToInt32(webuserData[5]) == 0)
            {
                Dictionary<string, string> webuserdict = GetWebUserDictionary(userId);
                webdata.Picture = webuserdict["Picture"];
                webdata.FullName = webuserdict["FullName"];
                webdata.NameFirst = webuserdict["NameFirst"];
                webdata.NameLast = webuserdict["NameLast"];
                webdata.CountryId = webuserdict["CountryId"];
                webdata.UserLevel = Convert.ToInt32(webuserdict["UserLevel"]);
                webdata.OnlineStatus = Convert.ToSByte(webuserdict["OnlineStatus"]);
                webdata.IsLeader = (webuserdict["IsLeader"]);
                webdata.CountryName = countryRepo.GetCountryName(webdata.CountryId);
            }
            else
            {
                webdata.Picture = webuserData[0];
                webdata.FullName = webuserData[1];
                webdata.OnlineStatus = Convert.ToSByte(webuserData[2]);
                webdata.CountryId = webuserData[3];
                webdata.UserLevel = Convert.ToInt32(webuserData[4]);
                webdata.NameFirst = webuserData[6];
                webdata.NameLast = webuserData[7];
                webdata.IsLeader = (webuserData[8]);


                webdata.CountryName = countryRepo.GetCountryName(webdata.CountryId);
            }
            taskbankAc.Wait();
            webdata.NetWorth = Convert.ToDecimal(taskbankAc.Result);
            webdata.LoginProvider = providerInfo;
            return webdata;
        }

        public IEnumerable<WebUser> GetRandomWebUsers(int count, string countryCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmLimit", count);
            dictionary.Add("parmCountryId", countryCode);
            IEnumerable<WebUser> randomUsers = spContext.GetSqlData<WebUser>(AppSettings.SPGetRandomWebUsers, dictionary);
            return randomUsers;
        }
        public UserProfileDTO GetProfileStat(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            UserProfileDTO userprofilestat = spContext.GetSqlDataSignleRow<UserProfileDTO>(AppSettings.SPGetProfileStat, dictionary);
            return userprofilestat;
        }
        public int GetUserIdByEmail(string emailId)
        {
            string reidsKey = AppSettings.RedisHashIdEmail;

            string userEmailData = cache.GetHash(reidsKey, emailId);
            if (userEmailData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmEmailid", emailId.ToLower());
                try
                {
                    var emailIdObj = spContext.GetSqlDataSignleValue(AppSettings.SPGetUserIdByEmail, dictionary, "UserId");
                    if (emailIdObj != null)
                    {
                        cache.SetHash(reidsKey, emailId, Convert.ToInt32(emailIdObj).ToString());
                        return Convert.ToInt32(emailIdObj);
                    }
                    return 0;

                }
                catch (Exception)
                {
                    return 0;
                }

            }
            return Convert.ToInt32(userEmailData);
        }
        public string GetEmailByUserId(int userId)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            string emailIdObj = spContext.GetSqlDataSignleValue(AppSettings.SPGetEmailByUserId, dictionary, "EmailId").ToString();

            return emailIdObj;
        }

        public int InitializeUser(InitializeWebUser webInfo)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            webInfo.UserInfo.Picture = RulesSettings.InitialPicture;
            webInfo.UserInfo.NameFirst = myTI.ToTitleCase(webInfo.UserInfo.NameFirst);
            webInfo.UserInfo.NameLast = myTI.ToTitleCase(webInfo.UserInfo.NameLast);
            webInfo.UserInfo.CreatedAt = DateTime.UtcNow;
            webInfo.UserInfo.Active = 1;
            webInfo.UserInfo.UserId = spContext.AddUpdate(webInfo.UserInfo); // TODO : make it Add
            if (webInfo.UserInfo.UserId == 0)
            {
                webInfo.UserInfo.UserId = GetUserIdByEmail(webInfo.UserInfo.EmailId);
            }
            Task taskA = Task.Factory.StartNew(() => SaveWebUserContacts(webInfo));

            UserBankAccount bankAc = new UserBankAccount
            {
                Cash = RulesSettings.InitialCash,
                Gold = RulesSettings.InitialGold,
                Silver = RulesSettings.InitialSilver,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = webInfo.UserInfo.UserId
            };

            spContext.AddUpdate(bankAc);
            CreditScore creditScore = new CreditScore
            {
                Score = RulesSettings.InitializeCreditScore,
                UserId = webInfo.UserInfo.UserId
            };
            spContext.AddUpdate(creditScore);

            cache.RemoveSortedSetMember(AppSettings.RedisSortedSetCountryPopulation, webInfo.UserInfo.CountryId.ToLower());
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", webInfo.UserInfo.UserId);
            dictionary.Add("parmFriendEmailId", webInfo.UserInfo.EmailId);

            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateWebUserContactUserId, dictionary);
            return webInfo.UserInfo.UserId;
        }

        public void SaveWebUserContacts(InitializeWebUser webInfo)
        {
            sbyte providerId = 0;
            if (webInfo.AccessToken != string.Empty)
            {

                List<WebUserContact> conatcts = new List<WebUserContact>();
                if (webInfo.LoginProvider == "google")
                {
                    conatcts = GmailExtractor.ExtractEmail(webInfo.AccessToken);
                    providerId = AppSettings.GoogleProviderId;
                }
                else if (webInfo.LoginProvider == "yahoo")
                {
                    conatcts = YahooExtractor.ExtractEmail(
                        webInfo.YahooUserGuid,
                        webInfo.AccessToken,
                        webInfo.AccessTokenSeceret);
                    providerId = AppSettings.YahooProviderId;
                }
                else if (webInfo.LoginProvider == "microsoft")
                {
                    providerId = AppSettings.MicroSoftProviderId;
                    conatcts = MicrosoftExtractor.ExtractEmail(webInfo.AccessToken);
                }
                if (conatcts.Count > 0)
                {
                    conatcts.ForEach(u => u.UserId = webInfo.UserInfo.UserId);
                    spContext.AddUpdateList(conatcts);
                    ContactSource cSource = new ContactSource();
                    cSource.Total = conatcts.Count;
                    cSource.UpdatedAt = DateTime.UtcNow;
                    cSource.UserId = webInfo.UserInfo.UserId;
                    cSource.ProviderId = providerId;
                    spContext.AddUpdate(cSource);
                    AddFriendSuggestionByEmail(webInfo.UserInfo.UserId);
                }
            }
        }
        public void AddFriendSuggestionByEmail(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmNotificationTypeId", AppSettings.FoundNewSocialContactNotificationId);
            spContext.ExecuteStoredProcedure
                          (AppSettings.SPAddFriendSuggestionByEmail, dictionary);
        }
        public void ApplyCreidtScore()
        {
            spContext.ExecuteStoredProcedure(AppSettings.SPApplyCreidtScore, new Dictionary<string, object>());
        }
        public void UserChangingLevel(short notificationId, sbyte postConetentId)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmNotificationTypeId", notificationId);
            dictionary.Add("parmPostContentTypeId", postConetentId);
            spContext.ExecuteStoredProcedure(AppSettings.SPUserChangingLevel, dictionary);
        }
        public bool IsAllowedUsers(string emailId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmEmailId", emailId.ToLower());
            AllowedWebUser webUser =
            spContext.GetByPrimaryKey<AllowedWebUser>(dictionary);
            if (string.IsNullOrEmpty(webUser.EmailId))
            {
                return false;
            }
            else if (webUser.EmailId.ToLower() == emailId.ToLower())
            {
                return true;
            }
            return false;
        }
        public IEnumerable<WebUserDTO> GetWebUserList(int[] userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserIdList", string.Join(",", userId));
            return spContext.GetSqlData<WebUserDTO>
           (AppSettings.SPGetWebUserList, dictionary);
        }
        public bool UpdateProfileName(WebUserInfoDTO webInfo)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", webInfo.UserId);
                dictionary.Add("parmNameFirst", webInfo.NameFirst);
                dictionary.Add("parmNameLast", webInfo.NameLast);
                spContext.ExecuteStoredProcedure(AppSettings.SPUpdateProfileName, dictionary);
                Dictionary<string, string> webUserdata = new Dictionary<string, string>();

                webUserdata.Add("NameFirst", webInfo.NameFirst);
                webUserdata.Add("NameLast", webInfo.NameLast);
                webUserdata.Add("FullName", webInfo.NameFirst + " " + webInfo.NameLast);
                cache.SetHashDictionary(AppSettings.RedisHashWebUser + webInfo.UserId, webUserdata);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to UpdateProfileName");
                return false;
            }
        }
        public bool UpdateProfilePic(WebUserInfoDTO webInfo)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPicture", webInfo.Picture);
                dictionary.Add("parmUserId", webInfo.UserId);
                spContext.ExecuteStoredProcedure(AppSettings.SPUpdateProfilePic, dictionary);
                cache.SetHash(AppSettings.RedisHashWebUser + webInfo.UserId,
                    "Picture", webInfo.Picture);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to UpdateProfilePic");
                return false;
            }
        }
        public void ClearCacheProfileUpdate(int userId)
        {
            cache.Invalidate(AppSettings.RedisHashWebUser + userId);
        }
        public bool IsThisFirstLogin(int userId)
        {
            string reidsKey = AppSettings.RedisHashFirstLogin;
            string firstlogindata = cache.GetHash(reidsKey, userId.ToString());
            if (firstlogindata == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);

                firstlogindata = Convert.ToString(spContext.GetSqlDataSignleValue(AppSettings.SPIsThisFirstLogin, dictionary, "IsFirstLogin"));
                if (firstlogindata == "N")
                {
                    cache.SetHash(reidsKey, userId.ToString(), firstlogindata);
                }
            }
            return firstlogindata == "Y" ? true : false;
        }

        public void TrackUserActivity(UserActivityLog activity)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", activity.UserId);
                dictionary.Add("parmIPAddress", activity.IPAddress);
                dictionary.Add("parmLastLogin", activity.LastLogin);
                dictionary.Add("parmFirstLogin", activity.FirstLogin);
                dictionary.Add("parmHit", activity.Hit);
                spContext.ExecuteStoredProcedure(AppSettings.SPSaveUserActivityLog, dictionary);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to TrackUserActivity");

            }
        }

        public WebUserContact GetInvitationSender(string invitationId, string emailId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmInvitationId", invitationId);
            dictionary.Add("parmEmailId", emailId.ToLower());
            return spContext.GetSqlDataSignleRow<WebUserContact>(AppSettings.SPGetInvitationSender, dictionary);

        }
        public void GiveCreditForAcceptedInvitationandFollowInvitee(WebUserContact webcontact)
        {
            if (webcontact.UserId == 0)
            {
                return;
            }


            IFriendDetailsDTORepository friendRepo = new FriendDetailsDTORepository();
            FriendRelationDTO friendRelation = new FriendRelationDTO
            {
                ActionType = "F",
                FriendId = webcontact.UserId,
                UserId = webcontact.FriendUserId
            };
            friendRepo.FollowFriend(friendRelation);

            friendRelation.FriendId = webcontact.FriendUserId;
            friendRelation.UserId = webcontact.UserId;
            friendRepo.FollowFriend(friendRelation);

            IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();

            PayMeDTO payMe = new PayMeDTO
            {
                ReciepentId = webcontact.UserId,
                SourceUserId = AppSettings.BankId,
                CountryId = string.Empty,
                TaskId = Guid.NewGuid(),
                FundType = AppSettings.SocialAssetFundType,
                Amount = RulesSettings.InviteAcceptedCredit,
                Tax = 0
            };

            if (bankRepo.PayMe(payMe) == 1)
            {
                StringBuilder parmText = new StringBuilder();
                parmText.AppendFormat("{0}|{1}|{2}",
                          webcontact.FriendUserId, GetFullName(webcontact.FriendUserId),
                          payMe.Amount);
                userNotif.AddNotification(false, string.Empty,
                  AppSettings.InviteAcceptedSocialContactNotificationId, parmText.ToString(), 2, webcontact.UserId);
            }
        }

    }
}
