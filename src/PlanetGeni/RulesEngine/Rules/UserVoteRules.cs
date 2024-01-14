using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Dao.Models;
namespace RulesEngine
{
    public class UserVoteRules : IRules
    {
        public VoteResponseDTO VoteDetails { get; set; }
        public List<UserVoteChoice> VoteChoices { get; set; }
        public TaskType VoteTaskType { get; set; }


        public UserVoteRules()
        {
        }
        public UserVoteRules(VoteResponseDTO voteDetails,
            TaskType voteTaskType,
            List<UserVoteChoice> voteChoices)
        {
            VoteDetails = voteDetails;
            VoteChoices = voteChoices;
            VoteTaskType = voteTaskType;
        }
        public ValidationResult IsValid()
        {
            if (VoteDetails .IsIncomepleteTask== false)
                return new ValidationResult("task already being completed");
            if (VoteTaskType.MaxChoiceCount < VoteDetails.ChoiceIds.Length)
            {
                return new ValidationResult("InvalidChoiceCount");
            }
            for (int i = 0; i < VoteDetails.ChoiceIds.Length; i++)
            {
                if (VoteChoices.FindIndex(item => item.ChoiceId == VoteDetails.ChoiceIds[i]) < 0)
                {
                    return new ValidationResult("InvalidChoiceId");
                }
            }
            return ValidationResult.Success;
        }


        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit Task then send 1 else 0.
            return result;
        }
        public void AddTaskTask()
        {

        }

        public void AddResubmitTaskTask(List<string> errorList)
        {
            throw new Exception(string.Join(",", errorList.ToArray()));

        }

    }
}
