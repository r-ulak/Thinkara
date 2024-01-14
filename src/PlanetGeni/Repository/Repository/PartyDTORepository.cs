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
    public class PartyDTORepository : IPartyDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        public PartyDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public PartyDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public PartyMember GetActiveUserParty(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            PartyMember resultlist
                  = spContext.GetSqlDataSignleRow<PartyMember>(AppSettings.SPGetActiveUserParty, dictionary);
            return resultlist;
        }
        public PartyEjection GetPartyEjection(string taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            PartyEjection result
                  = spContext.GetByPrimaryKey<PartyEjection>(dictionary);
            return result;
        }
        public PartyCloseRequest GetPartyCloseRequest(string taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            PartyCloseRequest result
                  = spContext.GetByPrimaryKey<PartyCloseRequest>(dictionary);
            return result;
        }
        public PartyNomination GetPartyNomination(string taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            PartyNomination result
                  = spContext.GetByPrimaryKey<PartyNomination>(dictionary);
            return result;
        }
        public string GetMyParties(int userId)
        {
            string partyId = GetActivePartyId(userId);
            if (new Guid(partyId) == Guid.Empty)
            {
                return string.Empty;
            }
            return GetpartyName(partyId);
        }
        public string GetActivePartyId(int userId)
        {
            string partyId =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "PartyId");
            if (partyId == null)
            {
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                partyId = webRepo.GetWebUserDictionary(userId)["PartyId"];
            }

            return partyId;
        }
        public string GetMemberStatus(int userId)
        {
            string partyId =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "MemberStatus");
            if (partyId == null)
            {
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                partyId = webRepo.GetWebUserDictionary(userId)["MemberStatus"];
            }

            return partyId;
        }
        public string GetPartyMemberType(int userId)
        {
            string partyMemberType =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "MemberType");
            if (partyMemberType == null)
            {
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                partyMemberType = webRepo.GetWebUserDictionary(userId)["MemberType"];
            }
            return partyMemberType;
        }
        public void SetActivePartyId(string partyId, int userId)
        {
            cache.SetHash(AppSettings.RedisHashWebUser + userId, "PartyId", partyId);
        }
        private Dictionary<string, string> GetPartyDictionary(string partyId)
        {
            Type type = typeof(WebUserDTO);
            int numberofProperty = type.GetProperties().Length;
            Dictionary<string, string> partydata =
                cache.GetHashSet(AppSettings.RedisHashPoliticalParty + partyId);
            if (partydata.Count < numberofProperty)
            {
                partydata.Clear();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyId);
                PoliticalParty party =
              GetPartyById(partyId);

                string field = string.Empty;
                string fieldValue = string.Empty;
                Dictionary<string, string> partyCachedata = new Dictionary<string, string>();

                foreach (var propertyName in party.GetType().GetProperties())
                {
                    field = propertyName.Name;
                    fieldValue = party.GetType().GetProperty(field).GetValue(party, null).ToString();
                    partyCachedata.Add(field, fieldValue);
                    partydata.Add(field, fieldValue);
                }
                cache.SetHashDictionary(AppSettings.RedisHashPoliticalParty + partyId, partyCachedata);
            }
            return partydata;
        }
        public string GetpartyName(string partyId)
        {
            if (partyId == Guid.Empty.ToString())
            {
                return String.Empty;
            }
            string partyName =
                cache.GetHash(AppSettings.RedisHashPoliticalParty + partyId, "PartyName");
            if (partyName == null)
            {
                PoliticalParty party =
                   GetPartyById(partyId);
                partyName = party.PartyName;
                cache.SetHash(AppSettings.RedisHashPoliticalParty + partyId, "PartyName", partyName);
            }

            return partyName;
        }
        public string GetlogoPictureId(string partyId)
        {
            if (partyId == Guid.Empty.ToString())
            {
                return String.Empty;
            }
            string logoPictureId =
                cache.GetHash(AppSettings.RedisHashPoliticalParty + partyId, "LogoPictureId");
            if (logoPictureId == null)
            {
                PoliticalParty party =
                   GetPartyById(partyId);
                logoPictureId = party.LogoPictureId;
                cache.SetHash(AppSettings.RedisHashPoliticalParty + partyId, "LogoPictureId", logoPictureId);
            }

            return logoPictureId;
        }
        public string GetpartyStatus(string partyId)
        {
            string partyStatus =
                cache.GetHash(AppSettings.RedisHashPoliticalParty + partyId, "Status");
            if (partyStatus == null)
            {
                PoliticalParty party =
                    GetPartyById(partyId);
                partyStatus = party.Status;
                cache.SetHash(AppSettings.RedisHashPoliticalParty + partyId, "Status", partyStatus);
            }
            return partyStatus;
        }
        public int GetpartySize(string partyId)
        {
            string partySize =
                cache.GetHash(AppSettings.RedisHashPoliticalParty + partyId, "PartySize");
            if (partySize == null)
            {
                PoliticalParty party =
                    GetPartyById(partyId);
                partySize = party.Status;
                cache.SetHash(AppSettings.RedisHashPoliticalParty + partyId, "PartySize", partySize);
            }
            return Convert.ToInt32(partySize);
        }
        public PoliticalParty GetPartyById(string partyId)
        {
            return JsonConvert.DeserializeObject<PoliticalParty>(
            GetPartyByIdJson(partyId));
        }

        public string GetPartyByIdJson(string partyId)
        {
            string redisKey = AppSettings.RedisKeyParty + partyId;
            string partyById = cache.GetStringKey(redisKey);
            if (partyById == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyId);
                partyById = JsonConvert.SerializeObject(
                   spContext.GetByPrimaryKey<PoliticalParty>(dictionary));
                cache.SetStringKey(redisKey, partyById);
            }
            return (partyById);
        }
        public PartyInvite GetPartyInviteById(string taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            PartyInvite result
                = spContext.GetByPrimaryKey<PartyInvite>(dictionary);
            return result;
        }
        public IEnumerable<MyPoliticalPartyDTO> GetUserParties(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            IEnumerable<MyPoliticalPartyDTO> resultlist
                = spContext.GetSqlData<MyPoliticalPartyDTO>(AppSettings.SPGetCurrentParty, dictionary);
            return resultlist;

        }
        public IEnumerable<MyPoliticalPartyDTO> GetPastUserParties(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            IEnumerable<MyPoliticalPartyDTO> resultlist
                = spContext.GetSqlData<MyPoliticalPartyDTO>(AppSettings.SPGetPastParty, dictionary);
            return resultlist;
        }
        public string GetAllUserParty(int userId)
        {
            string reidsKey = AppSettings.RedisHashUserProfile + userId;
            string profilePartyData = cache.GetHash(reidsKey, "party");
            if (profilePartyData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);

                profilePartyData = JsonConvert.SerializeObject(
          spContext.GetSqlData<PartyProfileDTO>(AppSettings.SPGetAllUserParty, dictionary));

                cache.SetHash(reidsKey, "party", profilePartyData);
                cache.ExpireKey(reidsKey, AppSettings.UserProfileCacheLimit);
            }

            return profilePartyData;
        }
        public string GetPartyAgendasJson(string partyId)
        {
            string electionAgendaData = cache.GetStringKey(AppSettings.RedisKeyElectionAgendas + partyId);
            if (electionAgendaData == null)
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyId);
                electionAgendaData = JsonConvert.SerializeObject
                    (spContext.GetSqlData<PartyAgendaDTO>(AppSettings.SPGetPartyAgenda, dictionary));
                cache.SetStringKey(AppSettings.RedisKeyElectionAgendas + partyId, electionAgendaData);
            }
            return (electionAgendaData);
        }
        public string GetAllPoliticalAgendaJson()
        {
            string electionAgendaData = cache.GetStringKey(AppSettings.RedisKeyPoliticalAgenda);
            if (electionAgendaData == null)
            {

                electionAgendaData = JsonConvert.SerializeObject
                    (spContext.GetSqlDataNoParms<ElectionAgenda>(AppSettings.SPGetAllPoliticalAgenda));
                cache.SetStringKey(AppSettings.RedisKeyPoliticalAgenda, electionAgendaData);
            }
            return (electionAgendaData);
        }
        public string GetPartyFounders(string partyId)
        {
            string partyFoundersData = cache.GetStringKey(AppSettings.RedisKeyPartyFounders + partyId);
            if (partyFoundersData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyId);
                dictionary.Add("parmMemberType", "F");
                partyFoundersData = JsonConvert.SerializeObject
                   (spContext.GetSqlData<PartyMemberDTO>(AppSettings.SPGetPartyByMemberType, dictionary));
                cache.SetStringKey(AppSettings.RedisKeyPartyFounders + partyId, partyFoundersData);

            }
            return partyFoundersData;
        }
        public string GetPartyCoFounders(string partyId)
        {
            string partyCoFoundersData = cache.GetStringKey(AppSettings.RedisKeyPartyCoFounders + partyId);
            if (partyCoFoundersData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyId);
                dictionary.Add("parmMemberType", "C");
                partyCoFoundersData = JsonConvert.SerializeObject
                   (spContext.GetSqlData<PartyMemberDTO>(AppSettings.SPGetPartyByMemberType, dictionary));
                cache.SetStringKey(AppSettings.RedisKeyPartyCoFounders + partyId, partyCoFoundersData);

            }
            return partyCoFoundersData;
        }
        public string GetPartyMembers(GetPartyMemberTypeDTO partyMemberType)
        {
            string rediskey = AppSettings.RedisKeyPartyMembers + partyMemberType.PartyId + partyMemberType.LastStartDate.ToString() + partyMemberType.MemberType;
            string partyMembersData = cache.GetStringKey(rediskey);
            if (partyMembersData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyMemberType.PartyId);
                dictionary.Add("parmLimit", AppSettings.PartyMemberLimit);
                dictionary.Add("parmMemberType", partyMemberType.MemberType);
                dictionary.Add("parmLastStartDate", partyMemberType.LastStartDate);
                partyMembersData = JsonConvert.SerializeObject
                    (spContext.GetSqlData<PartyMemberDTO>(AppSettings.SPGetPartyMember, dictionary));
                cache.SetStringKey(rediskey, partyMembersData);
            }
            return partyMembersData;
        }
        public bool RequestEjectPartyMember(EjectPartyDTO ejectionList)
        {
            bool result = false;
            try
            {
                if (ProcessEjectMember(Guid.NewGuid(), ejectionList))
                {
                    result = true;

                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to RequestEjectPartyMember");
                return false;
            }
        }
        public PartySummaryDTO GetUserPartySummary(int userid)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userid);
            PartySummaryDTO userPartySummary =
         spContext.GetSqlDataSignleRow<PartySummaryDTO>
         (AppSettings.SPGetUserPartyMemberSummary, dictionary);

            IEnumerable<PartInviteTypeDTO> userPartyInvite =
            spContext.GetSqlData<PartInviteTypeDTO>
            (AppSettings.SPGetUserPartyInviteSummary, dictionary);
            userPartySummary.PartyInvites = userPartyInvite.ToArray();

            return userPartySummary;
        }


        public IEnumerable<WebUserContactDTO> GetEmailInvitationList(int userid, string lastemailId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userid);
            dictionary.Add("parmLimit", AppSettings.EmailInvitationLimit);
            dictionary.Add("parmLastFriendEmailId", lastemailId);
            IEnumerable<WebUserContactDTO> emailInvitationList =
             spContext.GetSqlData<WebUserContactDTO>
             (AppSettings.SPGetEmailInvitationList, dictionary);
            return emailInvitationList;
        }
        public string GetTopTenPartyByMember()
        {
            string partyByMemberTopNData = cache.GetStringKey(AppSettings.RedisKeyPartyByMemberTop10);
            if (partyByMemberTopNData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", 10);
                partyByMemberTopNData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<PoliticalPartyDTO>(
                AppSettings.SPGetTopNPartyByMember,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyPartyByMemberTop10, partyByMemberTopNData,
                    AppSettings.PartySummaryTop10CacheLimit);
            }
            return (partyByMemberTopNData);
        }
        public string SearchParty(PartySearchDTO searchCriteria)
        {
            string parmAgendaType = string.Empty;
            if (searchCriteria.AgendaType.Length > 0)
            {
                parmAgendaType = string.Join(",", searchCriteria.AgendaType);
            }

            string reidsKey = AppSettings.RedisKeyPartySearch +
                searchCriteria.PartySizeRangeDown
                + parmAgendaType + searchCriteria.PartySizeRangeUp
               + searchCriteria.PartyVictoryRangeDown
                + "L" + searchCriteria.PartyVictoryRangeUp
                + "L" + searchCriteria.PartyWorthRangeUp
                + "L" + searchCriteria.PartyWorthRangeDown
                + "L" + searchCriteria.PartyFeeRangeDown
                + searchCriteria.LastStartDate
                + "L" + searchCriteria.PartyFeeRangeUp;


            string searchPartyData = cache.GetStringKey(reidsKey);
            if (searchPartyData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmAgendaType", parmAgendaType);
                dictionary.Add("parmPartySizeRangeUp", searchCriteria.PartySizeRangeUp);
                dictionary.Add("parmPartySizeRangeDown", searchCriteria.PartySizeRangeDown);
                dictionary.Add("parmLimit", AppSettings.SearchPartyLimit);
                dictionary.Add("parmPartyVictoryRangeUp", searchCriteria.PartyVictoryRangeUp);
                dictionary.Add("parmPartyVictoryRangeDown", searchCriteria.PartyVictoryRangeDown);
                dictionary.Add("parmPartyFeeRangeUp", searchCriteria.PartyFeeRangeUp);
                dictionary.Add("parmPartyFeeRangeDown", searchCriteria.PartyFeeRangeDown);
                dictionary.Add("parmPartyWorthRangeUp", searchCriteria.PartyWorthRangeUp * 1000000);
                dictionary.Add("parmPartyWorthRangeDown", searchCriteria.PartyWorthRangeDown * 1000000);
                dictionary.Add("parmLastStartDate", searchCriteria.LastStartDate);

                IEnumerable<PoliticalPartyDTO> parties =
                      spContext.GetSqlData<PoliticalPartyDTO>(
                AppSettings.SPSearchParty,
                dictionary);

                searchPartyData = JsonConvert.SerializeObject(parties);
                cache.SetStringKey(reidsKey, searchPartyData);
            }
            return (searchPartyData);
        }
        public string[] GetAllPartyMember(string partyId)
        {
            string rediskey = AppSettings.RedisSetKeyPartyAllMembers + partyId;
            if (cache.KeyExists(rediskey) == false)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyId);
                IEnumerable<UserDTO> partyMembers =
                    (spContext.GetSqlData<UserDTO>(AppSettings.SPGetAllPartyMember, dictionary));
                foreach (var item in partyMembers)
                {
                    cache.SetAdd(rediskey, item.UserId.ToString());
                }
            }
            return cache.GetAllSet(rediskey);
        }
        public int GetPartyMemberTotal(string partyId)
        {
            int count = 0;
            count = Convert.ToInt32(GetPartyDictionary(partyId)["PartySize"]);
            return count;
        }
        public string GetAllPartyMemberJson(string partyId)
        {
            string rediskey = AppSettings.RedisKeyPartyAllMembers + partyId;
            string allMembers = cache.GetStringKey(rediskey);
            if (allMembers == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", partyId);
                allMembers = JsonConvert.SerializeObject
                    (spContext.GetSqlData<PartyMemberDTO>(AppSettings.SPGetAllPartyMemberWithNames, dictionary));
                cache.SetStringKey(rediskey, allMembers);
            }
            return allMembers;

        }
        public bool IsUniquePartyName(string partyName, string countryId)
        {
            string rediskey = AppSettings.RedisSetKeyPartyNames + countryId;
            if (cache.KeyExists(rediskey) == false)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryId);
                IEnumerable<PartyNameDTO> partyNames =
                    (spContext.GetSqlData<PartyNameDTO>(AppSettings.SPGetPartyNames, dictionary));
                foreach (var item in partyNames)
                {
                    cache.SetAdd(rediskey, item.PartyName.ToUpper());
                }
            }
            return !cache.IsSetMember(rediskey, partyName.ToUpper());
        }
        public bool IsActiveMemberOfDiffrentParty(int userId)
        {
            if (GetActiveUserParty(userId).UserId > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsActiveMemberOfParty(string partyId, int userId)
        {
            if (GetActivePartyId(userId) == partyId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsPartyFounderOrCoFounder(int userId)
        {
            string memberType =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "MemberType");
            if (memberType == null)
            {
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                memberType = webRepo.GetWebUserDictionary(userId)["MemberType"];
            }

            return (memberType == "C" || memberType == "F" ? true : false);
        }
        public bool IsPartyCoFounder(int userId)
        {
            string memberType =
                cache.GetHash(AppSettings.RedisHashWebUser + userId, "MemberType");
            if (memberType == null)
            {
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                memberType = webRepo.GetWebUserDictionary(userId)["MemberType"];
            }

            return (memberType == "C" ? true : false);
        }
        public bool IsPartyFounder(int userId)
        {
            string memberType =
         cache.GetHash(AppSettings.RedisHashWebUser + userId, "MemberType");
            if (memberType == null)
            {
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                memberType = webRepo.GetWebUserDictionary(userId)["MemberType"];
            }

            return (memberType == "F" ? true : false);
        }

        private TaskReminder GetTaskReminder(DateTime dueDate, Guid taskId)
        {
            TaskReminder startpartyreminder = new TaskReminder
            {
                TaskId = taskId,
                EndDate = dueDate,
                StartDate = DateTime.UtcNow,
                ReminderTransPort = "MP",
                ReminderFrequency = 8
            };

            return startpartyreminder;
        }
        private UserTask GetTask(Guid taskId, int requestoruserId, DateTime trnDate, short defaultResponse,
            DateTime dueDate, StringBuilder parm, int userId, short taskType)
        {


            UserTask startpartyTask = new UserTask
            {
                TaskId = taskId,
                AssignerUserId = requestoruserId,
                CompletionPercent = 0,
                CreatedAt = trnDate,
                DefaultResponse = defaultResponse,
                DueDate = dueDate,
                Flagged = false,
                Priority = 11,
                Parms = parm.ToString(),
                Status = "I",
                TaskTypeId = taskType,
                UserId = userId
            };
            return startpartyTask;
        }
        private bool UpdateBankAcPartyPayment(decimal amount, int userId, string partyId, int fundType)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                dictionary.Add("parmAmount", amount);
                dictionary.Add("parmFundType", fundType);
                dictionary.Add("parmPartyId", partyId);
                int result = (int)spContext.GetSqlDataSignleValue
                    (AppSettings.SPExecutePartyPayment, dictionary, "result");
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to UpdateBankAcPartyPayment");
                return false;
            }
        }
        #region Close party
        public bool CloseParty(PartyCloseRequest partyClose)
        {
            try
            {
                PoliticalParty partyInfo = GetPartyById(partyClose.PartyId.ToString());

                if (partyClose.Status == "A")
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("parmPartyId", partyClose.PartyId);
                    int response = spContext.ExecuteStoredProcedure(AppSettings.SPCloseParty, dictionary);
                    if (response == 1)
                    {

                        AddClosePartyPost(partyClose);
                        UpdateClosePartyStatus(partyClose);
                        List<PartyMemberDTO> coFounders =
             JsonConvert.DeserializeObject<List<PartyMemberDTO>>(GetPartyCoFounders(partyInfo.PartyId.ToString()));
                        string[] allmembers = FireAllMembers(partyClose);
                        DistributeMoney(allmembers, partyInfo, coFounders);
                        UpdatePartyStatus(partyClose.PartyId.ToString(), "C");
                    }

                }
                else
                {
                    string partystatus = "A";
                    if (partyInfo.PartySize < AppSettings.InitialPartySize)
                    {
                        partystatus = "P";
                    }
                    UpdatePartyStatus(partyClose.PartyId.ToString(), partystatus);
                    UpdateClosePartyStatus(partyClose);
                    AddClosePartyPost(partyClose);

                }
                ExpireCachePoliticalParty(partyClose.PartyId.ToString(), partyClose.UserId);

                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to CloseParty");
                return false;
            }
        }
        private void AddClosePartyPost(PartyCloseRequest partyClose)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            string partyName = partyRepo.GetpartyName(partyClose.PartyId.ToString());
            StringBuilder message = new StringBuilder();
            WebUserDTO webUserInfo = webRepo.GetUserPicFName(partyClose.UserId);

            string result = partyClose.Status == "A" ? "Approved" : "Denied";
            message.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                 partyClose.UserId,
                webUserInfo.Picture, webUserInfo.FullName,
                partyClose.PartyId, partyName,
                result
                );

            Post post = new Post
            {
                Parms = message.ToString(),
                PostContentTypeId = AppSettings.PartyClosePostContentTypeId,
                PartyId = partyClose.PartyId

            };
            postRepo.SavePost(post);
        }
        private string[] FireAllMembers(PartyCloseRequest partyClose)
        {
            string[] allmembers = GetAllPartyMember(partyClose.PartyId.ToString());
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (string member in allmembers)
            {
                dictionary.Add("parmPartyId", partyClose.PartyId);
                dictionary.Add("parmUserId", member);
                dictionary.Add("parmStatus", "C");
                spContext.ExecuteStoredProcedure(AppSettings.SPEjectPartyMember, dictionary);
                dictionary.Clear();
            }
            return allmembers;
        }
        public void UpdateClosePartyStatus(PartyCloseRequest partyClose)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", partyClose.TaskId);
            dictionary.Add("parmPartyStatus", partyClose.Status);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateClosePartyStatus, dictionary);
            if (partyClose.Status == "A")
            {
                ExpireCachePoliticalParty(partyClose.PartyId.ToString(), partyClose.UserId);

            }
        }
        public bool RequestCloseParty(ClosePartyDTO closeParty)
        {
            bool result = false;
            try
            {
                if (ProcessClosParty(Guid.NewGuid(), closeParty))
                {
                    result = true;

                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to RequestCloseParty");
                return false;
            }
        }
        private bool ProcessClosParty(Guid taskId, ClosePartyDTO closeParty)
        {
            bool result = false;
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                IWebUserDTORepository webUserRepo = new WebUserDTORepository();
                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Approved";
                String notificationparmText = "";

                parm.AppendFormat("{0}|{1}|{2}|{3}|<strong>Date:{4}</strong>|{5}",
              closeParty.InitatorId, closeParty.InitatorFullName,
              closeParty.PartyId, closeParty.PartyName, dueDate, defaultResponse);

                notificationparmText = "";

                notificationparmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}",
               closeParty.InitatorId, closeParty.InitatorFullName,
                     trnDate.ToString(), closeParty.PartyName
                    );

                string[] allmembers = GetAllPartyMember(closeParty.PartyId);
                foreach (string member in allmembers)
                {
                    UserTask userTask = GetTask(taskId, closeParty.InitatorId, trnDate,
                                                (short)AppSettings.PartyCloseApprovalChoiceId, dueDate,
                                                        parm, Convert.ToInt32(member), AppSettings.ClosePartyTaskType);
                    taskRepo.SaveTask(userTask);
                    userNotif.AddNotification(true, taskId.ToString(),
                                               AppSettings.PartyCloseVotingRequestNotificationId,
                                               notificationparmText.ToString(), 7, Convert.ToInt32(member));
                }
                AddPartyCloseRequest(closeParty, taskId);
                AddClosePartyRequestPost(closeParty);
                UpdatePartyStatus(closeParty.PartyId, "H");
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save ProcessCloseMember");
                result = false;
            }
            return result;

        }
        private void AddClosePartyRequestPost(ClosePartyDTO partyClose)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            StringBuilder message = new StringBuilder();
            string picture = webRepo.GetUserPicture(partyClose.InitatorId);

            message.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                partyClose.InitatorId,
                picture, partyClose.InitatorFullName,
                partyClose.PartyId, partyClose.PartyName
                );

            Post post = new Post
            {
                Parms = message.ToString(),
                PostContentTypeId = AppSettings.PartyCloseRequestPostContentTypeId,
                PartyId = new Guid(partyClose.PartyId)

            };
            postRepo.SavePost(post);
        }

        private void AddPartyCloseRequest(ClosePartyDTO closeParty, Guid taskId)
        {
            PartyCloseRequest partyClose = new PartyCloseRequest
            {
                PartyId = new Guid(closeParty.PartyId),
                RequestDate = DateTime.UtcNow,
                Status = "P",
                TaskId = taskId,
                UserId = closeParty.InitatorId
            };
            spContext.Add(partyClose);

        }
        public void UpdatePartyStatus(string partyId, string status)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPartyId", partyId);
            dictionary.Add("parmPartyStatus", status);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdatePartyStatus, dictionary);
        }

        private void DistributeMoney(string[] allmembers, PoliticalParty partyInfo, List<PartyMemberDTO> coFounders)
        {
            try
            {


                int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
                int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                     partyFounderSize * AppSettings.FounderVoteScore
                    ;

                decimal share = (partyInfo.TotalValue / totalVoters);
                decimal finalShare = 0;

                foreach (var item in allmembers)
                {
                    finalShare = share;
                    if (Convert.ToInt32(item) == partyInfo.PartyFounder)
                    {
                        finalShare = share * AppSettings.FounderVoteScore;
                    }
                    else if (coFounders.Count(f => f.UserId == Convert.ToInt32(item)) == 1)
                    {
                        finalShare = share * AppSettings.CoFounderVoteScore;
                    }

                    UpdateBankAcPartyPayment(finalShare, Convert.ToInt32(item), partyInfo.PartyId.ToString(), AppSettings.PartyCloseFundType);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to DistributeMoney");
            }
        }


        #endregion Close party

        #region PartyEjection
        private bool ProcessEjectMember(Guid taskId, EjectPartyDTO ejectionDto)
        {
            bool result = false;
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                IWebUserDTORepository webUserRepo = new WebUserDTORepository();
                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Approved";
                String notificationparmText = "";

                ejectionDto.InitiatorFullName = webUserRepo.GetFullName(ejectionDto.InitatorId);
                ejectionDto.EjecteeFullName = webUserRepo.GetFullName(ejectionDto.EjecteeId);
                ejectionDto.PartyName = GetpartyName(ejectionDto.PartyId);

                parm.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|<strong>Date:{6}</strong>|{7}",
                     ejectionDto.InitatorId, ejectionDto.InitiatorFullName,
                   ejectionDto.EjecteeId, ejectionDto.EjecteeFullName,
               ejectionDto.PartyId, ejectionDto.PartyName,
                dueDate, defaultResponse);

                notificationparmText = "";

                notificationparmText = string.Format("{0}|{1}|{2}|{3}|<strong>Date:{4}</strong>|{5}>|{6}",
                   ejectionDto.EjecteeId, ejectionDto.EjecteeFullName,
                   ejectionDto.InitatorId, ejectionDto.InitiatorFullName,
                     trnDate.ToString(), ejectionDto.PartyId, ejectionDto.PartyName);

                string[] allmembers = GetAllPartyMember(ejectionDto.EjecteePartyId);

                string fullName = webUserRepo.GetFullName(ejectionDto.EjecteeId);
                string taskParm = String.Format("{0}|{1}|{2}",
                  ejectionDto.EjecteeId, fullName,
                    parm.ToString());
                string notificationParm = String.Format("{0}|{1}|{2}",
               ejectionDto.EjecteeId, fullName,
                 notificationparmText);
                foreach (string member in allmembers)
                {
                    if (ejectionDto.EjecteeId == Convert.ToInt32(member))
                    {
                        continue;
                    }
                    UserTask userTask = GetTask(taskId, ejectionDto.InitatorId, trnDate,
                                                (short)AppSettings.PartyEjectionApprovalChoiceId, dueDate,
                                                        parm, Convert.ToInt32(member), AppSettings.EjectPartyTaskType);
                    taskRepo.SaveTask(userTask);
                    userNotif.AddNotification(true, taskId.ToString(),
                                               AppSettings.PartyEjectVotingRequestNotificationId,
                                               notificationparmText.ToString(), 7, Convert.ToInt32(member));
                }
                AddPartyEjection(ejectionDto, taskId);
                UpdateMemberStatus(ejectionDto.EjecteeId, ejectionDto.EjecteePartyId, AppSettings.PartyMemberStatusEjection);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save ProcessEjectMember");
                result = false;
            }
            return result;

        }
        private bool AddPartyEjection(EjectPartyDTO ejectionDto, Guid taskId)
        {
            PartyEjection partyEjection = new PartyEjection
            {
                TaskId = taskId,
                PartyId = new Guid(ejectionDto.PartyId),
                InitatorId = ejectionDto.InitatorId,
                EjecteeId = ejectionDto.EjecteeId,
                EjecteeMemberType = ejectionDto.EjecteeMemberType,
                RequestDate = DateTime.UtcNow,
                Status = "P"
            };
            try
            {
                spContext.Add(partyEjection);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to AddPartyEjection");
                return false;
            }
        }

        public void ProcessEjectionApproval(PartyEjection partyEjection)
        {
            UpdateEjectMemberEndDate(partyEjection);
            UpdatePartySize(partyEjection);
            SendPostContentEjectionResult(partyEjection, "Approved");
            UpdateEjection(partyEjection, "A");
            ExpireCachePoliticalParty(partyEjection.PartyId.ToString(), partyEjection.EjecteeId);
        }
        private void UpdatePartySize(PartyEjection partyEjection)
        {
            PoliticalParty partyinfo = GetPartyById(partyEjection.PartyId.ToString());

            partyinfo.PartySize--;
            if (partyEjection.EjecteeMemberType == "F")
            {
                partyinfo.PartyFounder = 0;

            }
            else if (partyEjection.EjecteeMemberType == "C")
            {
                partyinfo.CoFounderSize--;
            }
            spContext.Update(partyinfo);
        }

        public void ProcessEjectionDenial(PartyEjection partyEjection)
        {
            SendPostContentEjectionResult(partyEjection, "Denied");
            UpdateEjection(partyEjection, "D");
            UpdateMemberStatus(partyEjection.EjecteeId, partyEjection.PartyId.ToString(), string.Empty);

        }

        private void UpdateEjectMemberEndDate(PartyEjection partyEjection)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPartyId", partyEjection.PartyId);
            dictionary.Add("parmUserId", partyEjection.EjecteeId);
            dictionary.Add("parmStatus", "F");
            spContext.ExecuteStoredProcedure(AppSettings.SPEjectPartyMember, dictionary);
        }
        private void SendPostContentEjectionResult(PartyEjection partyEjection, string approvalStatus)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();

            StringBuilder postParms = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            WebUserDTO webData = webRepo.GetUserPicFName(partyEjection.EjecteeId);

            postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
        partyEjection.EjecteeId,
        webData.Picture
        , webData.FullName
        , partyEjection.PartyId, GetpartyName(partyEjection.PartyId.ToString()), approvalStatus);

            Post post = new Post
            {
                Parms = postParms.ToString(),
                PostContentTypeId = AppSettings.PartyEjectionPostContentTypeId,
                PartyId = partyEjection.PartyId
            };
            postRepo.SavePost(post);
        }

        public void UpdateEjection(PartyEjection ejection, string status)
        {
            ejection.Status = status;
            spContext.Update(ejection);
        }

        #endregion PartyEjection

        #region Party Nomination
        public bool RequestNominationPartyMember(PartyNominationDTO nominationParty, Guid taskId)
        {
            bool result = false;
            try
            {
                if (ProcessNominationMember(taskId, nominationParty))
                {
                    result = true;

                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to RequestNominationPartyMember");
                return false;
            }
        }
        private bool ProcessNominationMember(Guid taskId, PartyNominationDTO nominationParty)
        {
            bool result = false;
            try
            {
                IPostCommentDTORepository postRepo = new PostCommentDTORepository();
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder postParms = new StringBuilder();
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Approved";
                String notificationparmText = "";


                nominationParty.GetPartyMemberType();

                parm.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|<strong>Date:{7}</strong>|{8}",
              nominationParty.InitatorId, nominationParty.InitatorFullName,
               nominationParty.NomineeId, nominationParty.NomineeFullName,
              nominationParty.PartyId, nominationParty.PartyName, nominationParty.PartyMemberType,
              dueDate, defaultResponse);

                notificationparmText = "";

                notificationparmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}|{4}|{5}|{6}|{7}",
               nominationParty.InitatorId, nominationParty.InitatorFullName,
                     trnDate.ToString(),
               nominationParty.NomineeId, nominationParty.NomineeFullName,
                   nominationParty.PartyMemberType, nominationParty.PartyId, nominationParty.PartyName
                    );

                string[] allmembers = GetAllPartyMember(nominationParty.PartyId);
                foreach (string member in allmembers)
                {
                    if (Convert.ToInt32(member) == nominationParty.NomineeId)
                    {
                        continue;
                    }
                    UserTask userTask = GetTask(taskId, nominationParty.InitatorId, trnDate,
                                                (short)AppSettings.PartyNominationElectionApprovalChoiceId, dueDate,
                                                        parm, Convert.ToInt32(member), AppSettings.NominationPartyTaskType);
                    taskRepo.SaveTask(userTask);
                    userNotif.AddNotification(true, taskId.ToString(),
                                               AppSettings.PartyNominationVotingRequestNotificationId,
                                               notificationparmText.ToString(), 7, Convert.ToInt32(member));
                }


                postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
   nominationParty.InitatorId, nominationParty.InitatorPicture, nominationParty.InitatorFullName,
    nominationParty.NomineeId, nominationParty.NomineePicture, nominationParty.NomineeFullName, nominationParty.PartyMemberType
    , nominationParty.PartyId, nominationParty.PartyName);
                Post post = new Post
                {
                    Parms = postParms.ToString(),
                    PostContentTypeId = AppSettings.PartyNominationPostContentTypeId,
                    PartyId = new Guid(nominationParty.PartyId)
                };
                postRepo.SavePost(post);

                result = true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save ProcessNominationMember");
                result = false;
            }
            return result;

        }
        public bool NotifyNominationPartyMember(PartyNominationDTO nominationParty)
        {
            bool result = false;
            try
            {
                if (ProcessNotifyNominationPartyMember(Guid.NewGuid(), nominationParty))
                {
                    result = true;

                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to NotifyNominationPartyMember");
                return false;
            }
        }
        private bool ProcessNotifyNominationPartyMember(Guid taskId, PartyNominationDTO nominationParty)
        {
            bool result = false;
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                IWebUserDTORepository webUserRepo = new WebUserDTORepository();
                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Approved";
                String notificationparmText = "";

                parm.AppendFormat("{0}|{1}|{2}|{3}|{4}|<strong>Date:{5}</strong>|{6}",
              nominationParty.InitatorId, nominationParty.InitatorFullName, nominationParty.PartyMemberType,
              nominationParty.PartyId, nominationParty.PartyName, dueDate, defaultResponse);

                notificationparmText = "";

                notificationparmText = string.Format("{0}|{1}|{2}|{3}|{4}",
               nominationParty.InitatorId, nominationParty.InitatorFullName, nominationParty.PartyMemberType, nominationParty.PartyId, nominationParty.PartyName
                    );


                UserTask userTask = GetTask(taskId, nominationParty.InitatorId, trnDate, (short)AppSettings.NotifyNominationRequestApprovalChoiceId, dueDate, parm, nominationParty.NomineeId, AppSettings.NominationNotifyPartyTaskType);
                taskRepo.SaveTask(userTask);
                userNotif.AddNotification(true, taskId.ToString(),
                                           AppSettings.PartyNotifyNominationNotificationId,
                                           notificationparmText.ToString(), 7, nominationParty.NomineeId);
                AddPartyNomination(nominationParty, taskId);
                UpdateMemberStatus(nominationParty.NomineeId, nominationParty.PartyId, AppSettings.PartyMemberStatusNomination);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save ProcessNotifyNominationPartyMember");
                result = false;
            }
            return result;

        }

        private bool AddPartyNomination(PartyNominationDTO partyNomination, Guid taskId)
        {
            PartyNomination nomination = new PartyNomination
            {
                TaskId = taskId,
                PartyId = new Guid(partyNomination.PartyId),
                InitatorId = partyNomination.InitatorId,
                NomineeId = partyNomination.NomineeId,
                NomineeIdMemberType = partyNomination.NomineeIdMemberType,
                NominatingMemberType = partyNomination.NominatingMemberType,
                RequestDate = DateTime.UtcNow,
                Status = "P"
            };
            try
            {
                spContext.Add(nomination);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to AddPartyNomination");
                return false;
            }
        }
        public bool HasPendingNomination(string partyId, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPartyId", partyId);
            dictionary.Add("parmUserId", userId);
            IEnumerable<PartyNomination> partyNomination =
                (spContext.GetSqlData<PartyNomination>(AppSettings.SPGetPendingNomination, dictionary));

            if (partyNomination.Count() > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsCurrentOrPastParty(int userId, string partyId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPartyId", partyId);
            dictionary.Add("parmUserId", userId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPIsCurrentOrPastParty, dictionary, "cnt"));
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public bool ProcessNominationApproval(PartyNomination nomination)
        {
            try
            {
                PartyMember nominee =
                             GetActiveUserParty(nomination.NomineeId);
                nominee.MemberEndDate = DateTime.UtcNow;
                spContext.Update(nominee);

                PoliticalParty party = GetPartyById(nomination.PartyId.ToString());

                if (nomination.NominatingMemberType == "F")
                {
                    int oldFounder = party.PartyFounder;
                    party.PartyFounder = nomination.NomineeId;
                    spContext.Update(party);
                    DemoteFounderToMember(nomination);
                    SendDemontionNotification(nomination, oldFounder);
                }
                else if (nomination.NominatingMemberType == "C")
                {
                    party.CoFounderSize++;
                }
                else if (nomination.NominatingMemberType == "M" && nominee.MemberType == "C")
                {
                    party.CoFounderSize--;
                }
                AddPartyMemberNomination(nominee, nomination);
                SendNominationCongratulations(nomination);
                spContext.Update(party);
                UpdateNomination(nomination, "A");
                ExpireCachePoliticalParty(nominee.PartyId.ToString(), nominee.UserId);
                return true;
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Failed trying to save ProcessNominationApproval");
                return false;
            }
        }

        public void UpdateNomination(PartyNomination nomination, string status)
        {
            nomination.Status = status;
            spContext.Update(nomination);
        }

        private void SendDemontionNotification(PartyNomination nominationParty, int oldFounder)
        {
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            StringBuilder notificationparmText = new StringBuilder();
            string partyName = GetpartyName(nominationParty.PartyId.ToString());

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}",
                nominationParty.PartyId, partyName,
  nominationParty.NomineeId,
 webRepo.GetFullName(nominationParty.NomineeId));

            userNotif.AddNotification(false, string.Empty,
                            AppSettings.PartyNotifyDemotionNominationNotificationId,
                            notificationparmText.ToString(), 8, oldFounder);
        }
        private void SendNominationCongratulations(PartyNomination nominationParty)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            string partyMemberType = nominationParty.GetPartyMemberType();

            StringBuilder postParms = new StringBuilder();
            StringBuilder notificationparmText = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            string partyName = GetpartyName(nominationParty.PartyId.ToString());
            WebUserDTO webData = webRepo.GetUserPicFName(nominationParty.NomineeId);
            postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
        nominationParty.NomineeId,
       webData.Picture
        , webData.FullName
        , partyMemberType, nominationParty.PartyId, partyName);

            notificationparmText.AppendFormat("{0}|{1}|{2}",
          partyMemberType, nominationParty.PartyId, partyName);

            Post post = new Post
            {
                Parms = postParms.ToString(),
                PostContentTypeId = AppSettings.PartyNominationElectionPostContentTypeId,
                PartyId = nominationParty.PartyId
            };
            userNotif.AddNotification(false, string.Empty,
                                   AppSettings.PartyNotifyCongratulationNominationNotificationId,
                                   notificationparmText.ToString(), 4, nominationParty.NomineeId);

            postRepo.SavePost(post);
        }

        private void AddPartyMemberNomination(PartyMember nominee, PartyNomination nomination)
        {
            nominee.MemberEndDate = null;
            nominee.MemberStartDate = DateTime.UtcNow;
            nominee.MemberType = nomination.NominatingMemberType;
            nominee.MemberStatus = string.Empty;
            spContext.Add(nominee);

        }
        private void DemoteFounderToMember(PartyNomination nomination)
        {

            PartyMember founder =
                 JsonConvert.DeserializeObject<List<PartyMember>>(GetPartyFounders(nomination.PartyId.ToString()))[0]; //There can only be on Founder
            founder.MemberEndDate = DateTime.UtcNow;
            founder.MemberStatus = "D";
            founder.PartyId = nomination.PartyId;
            spContext.Update(founder);


            founder.MemberEndDate = null;
            founder.MemberStartDate = DateTime.UtcNow;
            founder.MemberType = "M";
            founder.MemberStatus = string.Empty;
            spContext.Add(founder);

        }
        #endregion Party Nomination

        #region Leave Party
        public bool LeaveParty(QuitPartyDTO quit, PoliticalParty partyInfo)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("parmPartyId", quit.PartyId);
                dictionary.Add("parmUserId", quit.UserId);
                dictionary.Add("parmStatus", "Q");
                spContext.ExecuteStoredProcedure(AppSettings.SPEjectPartyMember, dictionary);
                partyInfo.PartySize--;
                if (partyInfo.PartyFounder == quit.UserId)
                {
                    partyInfo.PartyFounder = 0;
                }
                UpdatePartSizeAndStatus(partyInfo);
                PartyQuitPost(quit);
                ExpireCachePoliticalParty(partyInfo.PartyId.ToString(), quit.UserId);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to LeaveParty");
                return false;
            }
        }
        private void UpdatePartSizeAndStatus(PoliticalParty partyInfo)
        {

            if (partyInfo.PartySize <= 0)
            {
                partyInfo.Status = "C";
            }
            else if (partyInfo.PartySize < AppSettings.InitialPartySize)
            {
                partyInfo.Status = "P";
            }
            else if (partyInfo.PartyFounder == 0)
            {
                partyInfo.Status = "P";
            }


            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            dictionary.Add("parmPartyId", partyInfo.PartyId);
            dictionary.Add("parmPartyStatus", partyInfo.Status);
            dictionary.Add("parmPartySize", partyInfo.PartySize);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdatePartySizeStatus, dictionary);


        }
        public void PartyQuitPost(QuitPartyDTO quit)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            StringBuilder message = new StringBuilder();
            WebUserDTO webUserInfo = webRepo.GetUserPicFName(quit.UserId);
            message.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                quit.UserId,
                webUserInfo.Picture,
                webUserInfo.FullName,
                quit.PartyId, quit.PartyName
                );

            Post post = new Post
            {
                Parms = message.ToString(),
                PostContentTypeId = AppSettings.PartyDumpedPostContentTypeId,
                PartyId = new Guid(quit.PartyId)

            };
            postRepo.SavePost(post);


        }

        #endregion Leave Party

        # region Party Invite
        public bool SendPartyInvite(InviteeDTO partyInviteInfo, string partyId, int InitatorId, string fullName)
        {
            try
            {
                // THis logic is check in InviteRules as well as here and so is redundant
                if (partyInviteInfo.FriendId > 0)
                {
                    if (HasPendingPartyInivite(partyId, partyInviteInfo.FriendId))
                    {
                        return false;
                    }

                }

                Guid taskId = Guid.NewGuid();
                PartyInvite partyInvite = new PartyInvite
                {
                    InvitationDate = DateTime.UtcNow,
                    MemberType = "M",
                    PartyId = new Guid(partyId),
                    Status = "P",
                    TaskId = taskId,
                    UserId = partyInviteInfo.FriendId > 0 ? partyInviteInfo.FriendId : 0,
                    EmailId = String.IsNullOrEmpty(partyInviteInfo.EmailId) == false ? partyInviteInfo.EmailId : string.Empty
                };
                spContext.Add(partyInvite);
                if (partyInviteInfo.FriendId > 0)
                {
                    if (ProcessSendInvitationTask(partyInviteInfo, partyId, taskId, InitatorId, fullName) == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SendPartyInvite");
                return false;
            }


        }
        public bool HasPendingPartyInivite(string partyId, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmPartyId", partyId);
            try
            {
                IList<PartyInvite> results =
                  spContext.GetSqlData<PartyInvite>(AppSettings.SPHasPendingPartyInvite, dictionary).ToList();

                if (results.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }
        private bool ProcessSendInvitationTask(InviteeDTO partyInviteInfo, string partyId, Guid taskId, int InitatorId, string fullName)
        {
            bool result = false;
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Declined";
                StringBuilder notificationparmText = new StringBuilder();
                PoliticalParty partyInfo = GetPartyById(partyId);

                parm.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|<strong>Date:{6}</strong>|{7}",
              InitatorId, fullName, "Member", partyId, partyInfo.PartyName, partyInfo.MembershipFee, dueDate, defaultResponse);

                notificationparmText.AppendFormat("{0}|{1}",
                            partyInfo.PartyId, partyInfo.PartyName
                    );

                UserTask userTask = GetTask(taskId, InitatorId, trnDate,
                                            (short)AppSettings.PartyInviteRejectChoiceId, dueDate,
                                                    parm, partyInviteInfo.FriendId, AppSettings.JoinPartyTaskType);
                if (taskRepo.SaveTask(userTask) == -1)
                {
                    result = false;
                    return result;
                }
                userNotif.AddNotification(true, taskId.ToString(),
                                           AppSettings.PartyJoinRequestInviteNotificationId,
                                           notificationparmText.ToString(), 7, partyInviteInfo.FriendId);

                result = true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save ProcessInvitationTask");
                result = false;
            }
            return result;
        }

        # endregion Party Invite

        #region PartyJoin Request
        public bool RequestJoinParty(JoinRequestPartyDTO joinParty)
        {
            bool result = false;
            try
            {
                if (ProcessRequestJoinParty(Guid.NewGuid(), joinParty))
                {
                    result = true;

                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to RequestJoinParty");
                return false;
            }
        }
        public bool HasPendingJoinRequest(string partyId, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmPartyId", partyId);
            try
            {
                IList<PartyJoinRequest> results =
                  spContext.GetSqlData<PartyJoinRequest>(AppSettings.SPHasPendingJoinRequest, dictionary).ToList();

                if (results.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }
        public bool HasPendingPartyInivite(string partyId, string emailId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmEmailid", emailId);
            dictionary.Add("parmPartyId", partyId);
            try
            {
                IList<PartyJoinRequest> results =
                  spContext.GetSqlData<PartyJoinRequest>(AppSettings.SPHasPendingPartyInvite, dictionary).ToList();

                if (results.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }


        }
        private bool ProcessRequestJoinParty(Guid taskId, JoinRequestPartyDTO joinParty)
        {
            bool result = false;
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                IWebUserDTORepository webUserRepo = new WebUserDTORepository();
                DateTime trnDate = joinParty.RequestDateTime;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Yea";
                StringBuilder notificationparmText = new StringBuilder();


                parm.AppendFormat("{0}|{1}|{2}|{3}|<Strong>Date:{4}</Strong>|{5}",
              joinParty.UserId, joinParty.FullName,
              joinParty.PartyId, joinParty.PartyName, dueDate, defaultResponse);


                notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|<strong>Date:{4}</strong>",
                            joinParty.UserId, joinParty.FullName,
              joinParty.PartyId, joinParty.PartyName,
                     trnDate.ToString()
                    );

                string[] allmembers = GetAllPartyMember(joinParty.PartyId);
                foreach (string member in allmembers)
                {
                    UserTask userTask = GetTask(taskId, joinParty.UserId, trnDate,
                                                (short)AppSettings.JoinPartyRequestApprovalChoiceId, dueDate,
                                                        parm, Convert.ToInt32(member), AppSettings.JoinPartyRequestTaskType);
                    taskRepo.SaveTask(userTask);
                    userNotif.AddNotification(true, taskId.ToString(),
                                               AppSettings.PartyApplyJoinVotingRequestNotificationId,
                                               notificationparmText.ToString(), 7, Convert.ToInt32(member));
                }
                AddNewPartyJoinRequest(joinParty, taskId);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save ProcessRequestJoinParty");
                result = false;
            }
            return result;

        }
        private bool AddNewPartyJoinRequest(JoinRequestPartyDTO joinParty, Guid taskId)
        {
            bool result = false;
            try
            {
                PartyJoinRequest joinPartyRequest = new PartyJoinRequest
                {
                    PartyId = new Guid(joinParty.PartyId),
                    RequestDate = joinParty.RequestDateTime,
                    Status = "P",
                    UserId = joinParty.UserId,
                    TaskId = taskId
                };
                spContext.Add(joinPartyRequest);
                result = true;
                return result;
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to StartParty");
                return false;
            }
        }
        public PartyJoinRequest GetPartyJoinRequest(string taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            return spContext.GetByPrimaryKey<PartyJoinRequest>(dictionary);

        }
        public bool SendApprovedMemeberPartyInvite(PartyJoinRequest joinRequest)
        {
            try
            {
                Guid taskId = Guid.NewGuid();
                ProcessInvitationTask(joinRequest.PartyId.ToString(), taskId, joinRequest.UserId);
                PartyInvite partyInvite = new PartyInvite
                {
                    InvitationDate = DateTime.UtcNow,
                    MemberType = "M",
                    PartyId = joinRequest.PartyId,
                    Status = "P",
                    TaskId = taskId,
                    UserId = joinRequest.UserId
                };
                spContext.Add(partyInvite);
                return true;


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SendMemeberPartyInvite");
                return false;
            }
        }
        private bool ProcessInvitationTask(string partyId, Guid taskId, int userId)
        {
            bool result = false;
            try
            {
                PoliticalParty politicalParty = GetPartyById(partyId);
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Accept";
                StringBuilder notificationparmText = new StringBuilder();


                parm.AppendFormat("{0}|{1}|{2}|<Strong>Date:{3}</Strong>|{4}",
          politicalParty.PartyId, politicalParty.PartyName, politicalParty.MembershipFee
              , dueDate, defaultResponse);


                notificationparmText.AppendFormat("{0}|{1}",
                            politicalParty.PartyId, politicalParty.PartyName
                    );

                UserTask userTask = GetTask(taskId, userId, trnDate,
                                            (short)AppSettings.JoinPartyRequestInviteAcceptChoiceId, dueDate,
                                                    parm, userId, AppSettings.JoinPartyRequestInviteTaskType);
                taskRepo.SaveTask(userTask);
                userNotif.AddNotification(true, taskId.ToString(),
                                           AppSettings.PartyJoinRequestInviteNotificationId,
                                           notificationparmText.ToString(), 7, userId);
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save ProcessInvitationTask");
                result = false;
            }
            return result;
        }
        public bool AddPartyMemberOnJoinRequest(PartyInvite invite, Guid taskId, bool hasJoinRequest = true)
        {
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                StringBuilder notificationparmText = new StringBuilder();
                string joinstatus = "";
                notificationparmText.AppendFormat("{0}|{1}",
             invite.PartyId, GetpartyName(invite.PartyId.ToString())
                 );
                if (IsActiveMemberOfDiffrentParty((int)invite.UserId) == false)
                {
                    PoliticalParty partyInfo = GetPartyById(invite.PartyId.ToString());
                    if (partyInfo.Status != "C")
                    {
                        if (AddPartyFund(partyInfo.MembershipFee, invite.PartyId, (int)invite.UserId) == true)
                        {
                            PartyMember member = new PartyMember
                          {
                              MemberType = "M",
                              PartyId = invite.PartyId,
                              MemberStartDate = DateTime.UtcNow,
                              UserId = (int)invite.UserId

                          };
                            spContext.Add(member);
                            CheckandUpdatePartyStatus(partyInfo);

                            invite.Status = "A";
                            spContext.Update(invite);
                            userNotif.AddNotification(false, invite.TaskId.ToString(),
                              AppSettings.PartyWelcomeNotificationId,
                              notificationparmText.ToString(), 7, (int)invite.UserId);
                            AddWelcomePost(invite, partyInfo);
                            SetActivePartyId(invite.PartyId.ToString(), (int)invite.UserId);
                            ExpireCachePoliticalParty(invite.PartyId.ToString(), (int)invite.UserId);
                            joinstatus = "A";

                        }
                        else
                        {
                            notificationparmText.Append(
                               "|Not Enough Cash for membership fee.");
                            invite.Status = "N";
                            spContext.Update(invite);
                            userNotif.AddNotification(false, invite.TaskId.ToString(),
                              AppSettings.PartyNotWelcomeNotificationId,
                              notificationparmText.ToString(), 7, (int)invite.UserId);
                            joinstatus = "D";
                        }

                    }
                    else
                    {
                        notificationparmText.Append(
                           "| party is already closed");
                        invite.Status = "M";
                        spContext.Update(invite);
                        userNotif.AddNotification(false, invite.TaskId.ToString(),
                    AppSettings.PartyNotWelcomeNotificationId,
                    notificationparmText.ToString(), 7, (int)invite.UserId);
                        joinstatus = "D";
                    }
                }
                else
                {
                    notificationparmText.Append(
                       "|Since you are active member of different party. Please leave that and request to rejoin again");
                    invite.Status = "M";
                    spContext.Update(invite);
                    userNotif.AddNotification(false, invite.TaskId.ToString(),
                AppSettings.PartyNotWelcomeNotificationId,
                notificationparmText.ToString(), 7, (int)invite.UserId);
                    joinstatus = "D";
                }
                if (hasJoinRequest == true)
                {
                    PartyJoinRequestUpdate(joinstatus, invite);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SPEjectPartyMember");
                return false;
            }
        }
        public void AddWelcomePost(PartyInvite invite, PoliticalParty partyInfo)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            StringBuilder message = new StringBuilder();
            WebUserDTO webUserInfo = webRepo.GetUserPicFName((int)invite.UserId);
            string position = string.Empty;

            if (invite.MemberType == "M")
            {
                position = "Member";
            }
            else if (invite.MemberType == "C")
            {
                position = "Co-Founder";

            }
            else if (invite.MemberType == "F")
            {
                position = "Founder";
            }

            message.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                invite.PartyId, partyInfo.PartyName,
                invite.UserId,
                webUserInfo.Picture,
                webUserInfo.FullName,
                position
                );

            Post post = new Post
            {
                Parms = message.ToString(),
                PostContentTypeId = AppSettings.WelcomePartyPostContentTypeId,
                PartyId = invite.PartyId

            };
            postRepo.SavePost(post);
        }

        private void PartyJoinRequestUpdate(string status, PartyInvite invite)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPartyId", invite.PartyId);
            dictionary.Add("parmUserId", invite.UserId);
            dictionary.Add("parmStatus", status);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdatePartyJoinRequest, dictionary);

        }
        public void DecliedPartyMemberOnJoinRequest(PartyInvite invite, Guid taskId, bool hasJoinRequest = true)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            StringBuilder message = new StringBuilder();
            WebUserDTO webUserInfo = webRepo.GetUserPicFName((int)invite.UserId);
            message.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                 invite.UserId,
                webUserInfo.Picture,
                webUserInfo.FullName,
                invite.PartyId, GetpartyName(invite.PartyId.ToString())
                );

            Post post = new Post
            {
                Parms = message.ToString(),
                PostContentTypeId = AppSettings.PartyJoinRequestInvitationDumpedPostContentTypeId,
                PartyId = invite.PartyId

            };
            invite.Status = "D";
            spContext.Update(invite);
            postRepo.SavePost(post);
            if (hasJoinRequest == true)
            {
                PartyJoinRequestUpdate("D", invite);
            }

        }
        public bool AddPartyFund(decimal amount, Guid partyId, int userId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("parmPartyId", partyId);
                dictionary.Add("parmUserId", userId);
                dictionary.Add("parmAmount", amount);
                dictionary.Add("parmFundType", AppSettings.PartyMembershipFeeFundType);

                int response = (int)spContext.GetSqlDataSignleValue
              (AppSettings.SPExecuteJoinPartyFee, dictionary, "result");
                if (response != 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to AddPartyFund");
                return false;
            }

        }
        #endregion PartyJoin Request

        #region Party Donation

        public bool ExecuteDonateParty(DonatePartyDTO donation)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("parmPartyId", donation.PartyId);
                dictionary.Add("parmUserId", donation.UserId);
                dictionary.Add("parmAmount", donation.Amount);
                dictionary.Add("parmFundType", AppSettings.PartyDonationFundType);

                int response = (int)spContext.GetSqlDataSignleValue
              (AppSettings.SPExecuteDonateParty, dictionary, "result");
                if (response != 1)
                {
                    return false;
                }
                AddDonationPost(donation);
                ExpireCachePoliticalParty(donation.PartyId, donation.UserId);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SPEjectPartyMember");
                return false;
            }
        }
        private void AddDonationPost(DonatePartyDTO donation)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();

            StringBuilder postParms = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            WebUserDTO webData = webRepo.GetUserPicFName(donation.UserId);

            postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                donation.UserId,
                webData.Picture,
                webData.FullName,
                donation.Amount,
                donation.PartyId,
                donation.PartyName);

            Post post = new Post
            {
                Parms = postParms.ToString(),
                PostContentTypeId = AppSettings.PartyDonationContentTypeId,
                PartyId = new Guid(donation.PartyId)
            };
            postRepo.SavePost(post);
        }

        #endregion Party Donation

        #region Start New Party
        public bool StartParty(StartPartyDTO startParty)
        {
            return AddNewParty(startParty);
        }
        private bool AddNewParty(StartPartyDTO startParty)
        {
            bool result = false;
            try
            {
                PoliticalParty politicalParty = new PoliticalParty
                {
                    CoFounderSize = 0,
                    CountryId = startParty.CountryId,
                    ElectionVictory = 0,
                    LogoPictureId = startParty.LogoPictureId,
                    MembershipFee = startParty.MembershipFee,
                    Motto = startParty.Motto,
                    PartyFounder = startParty.InitatorId,
                    PartyName = startParty.PartyName,
                    PartySize = 1,
                    StartDate = DateTime.UtcNow,
                    Status = "P",
                    TotalValue = 0,
                    PartyId = startParty.PartyId

                };
                spContext.Add(politicalParty);

                PartyMember member = new PartyMember
                {
                    MemberType = "F",
                    PartyId = startParty.PartyId,
                    MemberStartDate = DateTime.UtcNow,
                    UserId = startParty.InitatorId

                };
                spContext.Add(member);
                AddPartyAgenda(startParty);
                ExpireCachePoliticalParty(startParty.PartyId.ToString(), startParty.InitatorId);
                AddCachePoliticalPartyNames(startParty.CountryId, startParty.PartyName);
                result = true;
                return result;
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to StartParty");
                return false;
            }
        }
        private void CheckandUpdatePartyStatus(PoliticalParty partyInfo)
        {
            if (partyInfo.Status == "P")
            {
                if (partyInfo.PartySize >= AppSettings.InitialPartySize)
                {
                    UpdatePartyStatus(partyInfo.PartyId.ToString(), "A");
                }
            }


        }

        private void AddPartyAgenda(StartPartyDTO startParty)
        {
            PartyAgenda agenda = new PartyAgenda
            {
                PartyId = startParty.PartyId
            };
            foreach (var item in startParty.AgendaType)
            {
                agenda.AgendaTypeId = item;
                spContext.Add(agenda);
            }
        }

        # endregion Start New Party

        #region Manage Party
        public bool ManageParty(StartPartyDTO startParty)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", startParty.PartyId);
                dictionary.Add("parmMotto", startParty.Motto);
                dictionary.Add("parmMembershipFee", startParty.MembershipFee);
                dictionary.Add("parmPartyName", startParty.PartyName);
                dictionary.Add("parmPartyFounder", startParty.InitatorId);
                spContext.ExecuteStoredProcedure(AppSettings.SPUpdateManageParty, dictionary);
                AddUpdateAgenda(startParty);
                ExpireCachePoliticalParty(startParty.PartyId.ToString(), startParty.InitatorId);
                if (startParty.PartyNameChanged)
                {
                    RemoveCachePoliticalPartyNames(startParty.CountryId, startParty.OriginalPartyName);
                    AddCachePoliticalPartyNames(startParty.CountryId, startParty.PartyName);
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ManageParty");
                return false;
            }
        }

        private void AddUpdateAgenda(StartPartyDTO startParty)
        {
            List<PartyAgendaDTO> partyAgenda = JsonConvert.DeserializeObject<List<PartyAgendaDTO>>(GetPartyAgendasJson(startParty.PartyId.ToString()));

            short[] oldAgendas = partyAgenda.Select(x => x.AgendaTypeId).ToArray();
            IEnumerable<short> insertAgendas = startParty.AgendaType.Except(oldAgendas);
            IEnumerable<short> deleteAgendas = oldAgendas.Except(startParty.AgendaType);

            PartyAgenda agenda = new PartyAgenda
            {
                PartyId = startParty.PartyId
            };


            foreach (var item in insertAgendas)
            {
                agenda.AgendaTypeId = item;
                spContext.Add(agenda);
            }


            foreach (var item in deleteAgendas)
            {
                agenda.AgendaTypeId = item;

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", startParty.PartyId);
                dictionary.Add("parmAgendaTypeId", item);
                spContext.ExecuteStoredProcedure(AppSettings.SPDeleteAgenda, dictionary);
            }
            if (deleteAgendas.Count() > 0 || insertAgendas.Count() > 0)
            {
                ExpireCachePoliticalPartyAgenda(startParty.PartyId.ToString());
            }
        }

        public bool ManagePartyUploadLogo(StartPartyDTO startParty)
        {
            try
            {
                if (string.IsNullOrEmpty(startParty.LogoPictureId) || startParty.LogoPictureId.IndexOf(".") == -1)
                {
                    startParty.LogoPictureId = string.Empty;
                }

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmPartyId", startParty.PartyId);
                dictionary.Add("parmPartyFounder", startParty.InitatorId);
                dictionary.Add("parmLogoPictureId", startParty.LogoPictureId);
                spContext.ExecuteStoredProcedure(AppSettings.SPUpdateManagePartyLogo, dictionary);
                ExpireCachePoliticalParty(startParty.PartyId.ToString(), startParty.InitatorId);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ManageParty");
                return false;
            }
        }
        #endregion Manage Party

        public void ExpireCachePoliticalPartyAgenda(string partyId)
        {
            string key =
              AppSettings.RedisKeyElectionAgendas + partyId;
            cache.Invalidate(key);

        }
        public void RemoveCachePoliticalPartyNames(string countryId, string partyName)
        {
            cache.SetRemove(AppSettings.RedisSetKeyPartyNames + countryId, partyName.ToUpper().Trim());
        }
        public void AddCachePoliticalPartyNames(string countryId, string partyName)
        {
            cache.SetAdd(AppSettings.RedisSetKeyPartyNames + countryId, partyName.ToUpper().Trim());
        }
        public void ExpireCachePoliticalParty(string partyId, int userId)
        {


            string[] keys = new string[]{
                AppSettings.RedisHashWebUser + userId,
                AppSettings.RedisSetKeyPartyAllMembers + partyId,
                AppSettings.RedisKeyPartyAllMembers + partyId,
                AppSettings.RedisKeyPartyCoFounders + partyId,
                AppSettings.RedisKeyPartyFounders + partyId,
                AppSettings.RedisHashPoliticalParty + partyId,
                AppSettings.RedisKeyParty + partyId
                
            };

            cache.Invalidate(keys);
            string rediskey = AppSettings.RedisKeyPartyMembers + partyId + "*";
            cache.Invalidate(cache.FindKeys(rediskey));

        }
        public void UpdateMemberStatus(int userId, string partyId, string status)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPartyId", partyId);
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmMemberStatus", status);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateMemberNomination, dictionary);
            ExpireCachePoliticalParty(partyId, userId);
        }

    }

}
