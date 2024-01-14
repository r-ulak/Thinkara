using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class JobRules : IRules
    {

        public JobRules()
        {

        }
        public ValidationResult IsValid()
        {
            return ValidationResult.Success;
        }
        public ValidationResult IsValidJobApplication(IEnumerable<JobCode> jobCodes)
        {
            if (jobCodes.Count() > 0)
            {
                StringBuilder jobTitle = new StringBuilder();
                foreach (var item in jobCodes)
                {
                    switch (item.JobType)
                    {
                        case "F":
                            jobTitle.Append("FullTime on ");
                            break;
                        case "P":
                            jobTitle.Append("PartTime on ");
                            break;
                        case "C":
                            jobTitle.Append("Contract on ");
                            break;
                    }
                    jobTitle.Append(item.Title);
                    jobTitle.Append("/");
                }
                jobTitle.Remove(jobTitle.Length - 1, 1);
                return new ValidationResult(string.Format("you already have pending/offered job for same job codes i.e {0}",jobTitle.ToString()));
            }

            return ValidationResult.Success;
        }

        public ValidationResult IsValidJobAcceptance(decimal totalMaxHPW, bool currentlyHasSameJob)
        {
            if (totalMaxHPW >= RulesSettings.MaxHPW)
            {
                return new ValidationResult(string.Format("you already have {0} hours of Job", RulesSettings.MaxHPW));

            }
            if (currentlyHasSameJob == true)
            {
                return new ValidationResult(("you cannot accept same job code that you currently are working on."));
            }

            return ValidationResult.Success;
        }

        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit PostComment then send 1 else 0.
            return result;
        }



    }
}
