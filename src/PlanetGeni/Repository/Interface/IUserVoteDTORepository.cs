using Dao.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserVoteDTORepository
    {
        UserVoteDTO GetVotingDetails(string taskId, int userid);
        bool SaveVoteResponse(VoteResponseDTO voteResponse, int userid);
        IEnumerable<UserVoteChoice> GetVoteChoiceByTaskType(int taskTypeId);
        int HasEnoughVoteForThisTask(int approvalChoiceId, int deinalChoiceId, double approvalPercent, int totalVoters
          , IEnumerable<ChoiceCountDTO> taskVoteCount);
        IEnumerable<ChoiceCountDTO> GetVoteCountByChoiceForTask(Guid taskId);
        void DeleteRemaningTaskNotComplete(VoteResponseDTO voteResponse);
    }
}
