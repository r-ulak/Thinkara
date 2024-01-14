using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FriendDetailsDTORepository : IFriendDetailsDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
        public FriendDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public FriendDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetFriendList(int userId)
        {
            string firendsData =
            cache.GetHash(AppSettings.RedisHashWebUser + userId, "Friends");
            if (firendsData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                firendsData = JsonConvert.SerializeObject(spContext.GetSqlData<FriendDetailsDTO>(AppSettings.SPFriendsList, dictionary));
                cache.SetHash(AppSettings.RedisHashWebUser + userId, "Friends", firendsData);
            }
            return firendsData;
        }

        public void UpdateEmailInvite(EmailInviteDTO inviteList)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (var item in inviteList.InvitationList)
            {
                if (item.NewEmail)
                {
                    dictionary.Add("parmUserId", inviteList.UserId);
                    dictionary.Add("parmFriendEmailId", item.FriendEmailId);
                    int userId = webRepo.GetUserIdByEmail(item.FriendEmailId);
                    dictionary.Add("parmFriendUserId", userId);
                    dictionary.Add("parmNameFirst", string.Empty);
                    dictionary.Add("parmInvitationId", inviteList.InvitationId);
                    dictionary.Add("parmNameLast", string.Empty);
                    dictionary.Add("parmPartyInvite", 0);
                    if (userId > 0)
                    {
                        dictionary.Add("parmJoinInvite", 0);
                    }
                    else
                    {
                        dictionary.Add("parmJoinInvite", 1);
                    }
                    spContext.ExecuteStoredProcedure(AppSettings.SPWebUserContactInviteAddUpdate, dictionary);
                    dictionary.Clear();
                }

            }
            string emailparm = string.Join(",", inviteList.InvitationList.Where(y => y.NewEmail == false).
                Select(x => string.Format("'{0}'", x.FriendEmailId)));
            if (emailparm.Length > 0)
            {
                dictionary.Add("parmEmailId", emailparm);
                dictionary.Add("parmUserId", inviteList.UserId);
                dictionary.Add("parmInvitationId", inviteList.InvitationId);
                spContext.ExecuteStoredProcedure(AppSettings.SPUpdateWebUserContactSendInvite, dictionary);
            }
            GiveCreditForInvitation(inviteList);
        }
        private void GiveCreditForInvitation(EmailInviteDTO inviteList)
        {
            if (inviteList.InvitationList.Length == 0)
            {
                return;
            }
            IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();

            PayMeDTO payMe = new PayMeDTO
            {
                ReciepentId = inviteList.UserId,
                SourceUserId = AppSettings.BankId,
                CountryId = string.Empty,
                TaskId = Guid.NewGuid(),
                FundType = AppSettings.SocialAssetFundType,
                Amount = RulesSettings.InviteCredit * inviteList.InvitationList.Length,
                Tax = 0
            };

            if (bankRepo.PayMe(payMe) == 1)
            {
                StringBuilder parmText = new StringBuilder();
                parmText.AppendFormat("{0}|{1}",
                          inviteList.InvitationList.Length, payMe.Amount);
                userNotif.AddNotification(false, string.Empty,
                  AppSettings.InviteSocialContactNotificationId, parmText.ToString(), 2, inviteList.UserId);
            }
        }
        public IEnumerable<ContactSourceDTO> GetContactSource(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            IEnumerable<ContactSourceDTO> resultlist
                = spContext.GetSqlData<ContactSourceDTO>(AppSettings.SPGetContactSource, dictionary);
            return resultlist;
        }
        public IEnumerable<WebUserDTO> GetFriendSuggestion(FriendSuggestDTO suggestDTO)
        {
            //Potenial to be added to Cache but not doing it now becasue it could be costly and friend suggestion might be stale.
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", suggestDTO.UserId);
            dictionary.Add("parmLimit", AppSettings.FriendSuggestionLimit);
            dictionary.Add("parmLastSuggestionUserId", suggestDTO.SuggestionUserId);

            IEnumerable<WebUserDTO> resultlist
                = spContext.GetSqlData<WebUserDTO>(AppSettings.SPGetFriendSuggestion, dictionary);
            return resultlist;
        }

        public bool UnFollowFriend(FriendRelationDTO friendDTO)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", friendDTO.UserId);
                dictionary.Add("parmFriendId", friendDTO.FriendId);
                spContext.ExecuteStoredProcedure(AppSettings.SPUnFollowFriend, dictionary);
                InvalidateCacheFriend(friendDTO);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to UnFollowFriend");
                return false;
            }
        }
        public bool IgnoreSuggestion(FriendSuggestDTO friendDTO)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", friendDTO.UserId);
                dictionary.Add("parmSuggestionUserId", friendDTO.SuggestionUserId);
                spContext.ExecuteStoredProcedure(AppSettings.SPIgnoreFriendSuggestion, dictionary);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SPBlockFollower");
                return false;
            }
        }
        public bool BlockFollower(FriendRelationDTO friendDTO)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", friendDTO.UserId);
                dictionary.Add("parmFriendId", friendDTO.FriendId);
                spContext.ExecuteStoredProcedure(AppSettings.SPBlockFollower, dictionary);
                InvalidateCacheFriend(friendDTO);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SPBlockFollower");
                return false;
            }
        }
        public bool FollowFriend(FriendRelationDTO friendDTO)
        {
            try
            {
                if (friendDTO.UserId == friendDTO.FriendId)
                {
                    return false;
                }
                Friend follow = new Friend
                {
                    CreatedAt = DateTime.UtcNow,
                    FollowerUserId = friendDTO.UserId,
                    FollowingUserId = friendDTO.FriendId
                };
                spContext.Add(follow);
                InvalidateCacheFriend(friendDTO);
                AddFriendOfMyFriendSuggestion(friendDTO.UserId);
                RemoveAnyFriendSuggestion(new FollowAllDTO
                {
                    FriendId = new int[] { friendDTO.FriendId },
                    UserId = friendDTO.UserId
                });
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to FollowFriend");
                return false;
            }
        }
        public void RemoveAnyFriendSuggestion(FollowAllDTO removesugession)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", removesugession.UserId);
            dictionary.Add("parmSuggestionUserId", String.Join(",", removesugession.FriendId));
            spContext.ExecuteStoredProcedure
                          (AppSettings.SPRemoveFriendSuggestion, dictionary);
        }
        public void UpdateMutalFriendSuggestion(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmNotificationTypeId", AppSettings.FoundNewSocialContactNotificationId);
            spContext.ExecuteStoredProcedure
                          (AppSettings.SPUpdateMutalFriend, dictionary);
        }
        public void AddFriendOfMyFriendSuggestion(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmNotificationTypeId", AppSettings.FoundNewSocialContactNotificationId);
            spContext.ExecuteStoredProcedure
                          (AppSettings.SPAddFriendOfMyFriendSuggestion, dictionary);
            Task taskA = Task.Factory.StartNew(() => UpdateMutalFriendSuggestion(userId));
        }
        public bool IsFriend(int userId, int friendId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmFriendId", friendId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPIsMyFriend, dictionary, "Cnt"));
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public int FollowAllFriend(FollowAllDTO followFriends)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserIdList", string.Join(",", followFriends.FriendId));
                dictionary.Add("parmUserId", followFriends.UserId);
                int response = (int)spContext.GetSqlDataSignleValue
                             (AppSettings.SPFollowAllFriend, dictionary, "result");
                InvalidateCacheFriend(followFriends.FriendId, followFriends.UserId);
                AddFriendOfMyFriendSuggestion(followFriends.UserId);
                RemoveAnyFriendSuggestion(followFriends);
                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to FollowAllFriend");
                return 0;
            }
        }

        public IEnumerable<UserEmailDTO> GetUserThatHasLowSocialAsset()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmMinimumInviteSent", RulesSettings.MinimumInviteExpectedToBeSent);
            dictionary.Add("parmMinimumNumberOfFriends", RulesSettings.MinimumNumberOfFriendsExpected);

            return spContext.GetSqlData<UserEmailDTO>(AppSettings.SPGetUserThatHasLowSocialAsset, dictionary);
        }


        public void InvalidateCacheFriend(int[] friendId, int userId)
        {
            Array.Resize(ref friendId, friendId.Length + 1);
            friendId[friendId.Length - 1] = userId;
            cache.Invalidate(friendId.Select(x => AppSettings.RedisHashWebUser + x.ToString()).ToArray());
        }
        public void InvalidateCacheFriend(FriendRelationDTO friend)
        {
            string[] keys = new string[] { 
            AppSettings.RedisHashWebUser + friend.FriendId,
            AppSettings.RedisHashWebUser + friend.UserId
            };
            cache.Invalidate(keys);
        }
    }
}
