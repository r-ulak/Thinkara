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
namespace RulesEngine
{
    public class UserTaskRules : IRules
    {
        public UserTask TaskDetails { get; set; }
        private ResourceManager
             resmgr = new ResourceManager("RulesEngine.ValidationMessage",
                               Assembly.GetExecutingAssembly());
        public UserTaskRules()
        {
        }
        public UserTaskRules(UserTask taskDetails)
        {
            TaskDetails = taskDetails;

        }
        public ValidationResult IsValid()
        {
            if (AllowUpdateInsert() == false)
                return new ValidationResult(resmgr.GetString("AccessDenied"));
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
