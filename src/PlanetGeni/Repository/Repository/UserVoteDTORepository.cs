using Common;
using Dao.Models;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserVoteDTORepository : IUserVoteDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public UserVoteDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public UserVoteDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public IEnumerable<UserVoteChoice> GetVoteChoiceByTaskType(int taskTypeId)
        {
            string voteChoicesData = cache.GetStringKey(
                AppSettings.RedisKeyVoteChoices + taskTypeId.ToString());
            IEnumerable<UserVoteChoice> userVoteChoice;
            if (voteChoicesData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskTypeId", taskTypeId);
                userVoteChoice = spContext.GetSqlData<UserVoteChoice>
                    (AppSettings.SPGetVoteChoiceByTaskType, dictionary);
                voteChoicesData = JsonConvert.SerializeObject(userVoteChoice);
                cache.SetStringKey(
                    AppSettings.RedisKeyVoteChoices + taskTypeId.ToString()
                    , voteChoicesData);
            }
            else
            {
                userVoteChoice = JsonConvert.DeserializeObject<IEnumerable<UserVoteChoice>>(voteChoicesData);
            }
            return userVoteChoice;

        }
        public UserVoteDTO GetVotingDetails(string taskId, int userid)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            dictionary.Add("parmUserId", userid);

            UserVoteDTO votedetailsByTask =
                spContext.GetSqlDataSignleRow<UserVoteDTO>
                (AppSettings.SPGetVotingDetailsByTaskId, dictionary);
            votedetailsByTask.Choices = GetVoteChoiceByTaskType(votedetailsByTask.TaskTypeId);
            return votedetailsByTask;

        }
        public IEnumerable<ChoiceCountDTO> GetVoteCountByChoiceForTask(Guid taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            IEnumerable<ChoiceCountDTO> voteCount =
                spContext.GetSqlData<ChoiceCountDTO>(AppSettings.SPGetCountByChoiceForTask, dictionary);
            return voteCount;
        }
        public int HasEnoughVoteForThisTask(int approvalChoiceId, int deinalChoiceId, double approvalPercent, int totalVoters
           , IEnumerable<ChoiceCountDTO> taskVoteCount)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == approvalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == deinalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;


            double approvalthresHold = approvalPercent * totalVoters;
            double denialthresHold = (1 - approvalPercent) * totalVoters;
            if (approvalPercent > 1)
            {
                approvalthresHold = approvalPercent;
                denialthresHold = totalVoters - approvalthresHold;
            }
            if (approvalVote >= approvalthresHold)
            {
                return approvalChoiceId;
            }
            else if (denialVote >= denialthresHold)
            {
                return deinalChoiceId;

            }
            return 0;
        }

        public bool SaveVoteResponse(VoteResponseDTO voteResponse, int userId)
        {
            bool result = false;
            try
            {
                sbyte score = 1;
                if (voteResponse.TaskTypeId >= 5 && voteResponse.TaskTypeId <= 30)
                {
                    // captured upto 30 for future tasks
                    IPartyDTORepository partyRepo = new PartyDTORepository();
                    string memberType = partyRepo.GetPartyMemberType(userId);

                    if (memberType == "F")
                    {
                        score = AppSettings.FounderVoteScore;
                    }
                    else if (memberType == "C")
                    {
                        score = AppSettings.CoFounderVoteScore;

                    }

                }
                for (int i = 0; i < voteResponse.ChoiceIds.Length; i++)
                {
                    UserVoteSelectedChoice selectedChoice = new UserVoteSelectedChoice()
                    {
                        ChoiceId = voteResponse.ChoiceIds[i],
                        TaskId = voteResponse.TaskId,
                        UserId = userId,
                        Score = score

                    };
                    spContext.Add(selectedChoice);
                }
                //update Task Status
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskId", voteResponse.TaskId);
                dictionary.Add("parmCompletionPercent", 100);
                dictionary.Add("parmUserId", userId);
                dictionary.Add("parmStatus", "C");

                spContext.ExecuteStoredProcedure(AppSettings.SPUpdateTaskStatus, dictionary);
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveVoteResponse");
                return false;
            }

        }

        public void DeleteRemaningTaskNotComplete(VoteResponseDTO voteResponse)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", voteResponse.TaskId);
            spContext.ExecuteStoredProcedure(AppSettings.SPDeleteTaskNotComplete, dictionary);

        }
    }
}
