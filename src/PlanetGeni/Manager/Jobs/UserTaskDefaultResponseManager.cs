using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Manager.ServiceController;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class UserTaskDefaultResponseManager
    {
        public UserTaskDefaultResponseManager()
        {

        }
        public int StartTaskVoting(int runId)
        {
            IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
            IUserVoteDTORepository voteRepo = new UserVoteDTORepository();
            IEnumerable<UserTaskDTO> inCompleteTaskList = taskRepo.GetIncompletePastDueTask().ToList();
            UserVoteManager votemanager = new UserVoteManager(); VoteResponseDTO userVote = new VoteResponseDTO();
            foreach (var item in inCompleteTaskList)
            {
                userVote.TaskId = item.TaskId;
                userVote.TaskTypeId = item.TaskTypeId;
                userVote.ChoiceIds = new int[1] { item.DefaultResponse };
                userVote.ChoiceRadioId = item.DefaultResponse;

                votemanager.ProcessVotingResponse(userVote, item.UserId);

            }

            var result = inCompleteTaskList
                            .GroupBy(l => l.TaskTypeId);
            foreach (var grp in result)
            {
                Console.WriteLine("TaskId {0} Count {1}",
                   grp.Key,
                   grp.Count());
            }

            Console.WriteLine("Total Number of Task voted with Default Response {0}", inCompleteTaskList.Count());
            return inCompleteTaskList.Count();
        }
    }
}
