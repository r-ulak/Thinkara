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
    public class RobberyRules : IRules
    {

        public RobberyRules()
        {

        }
        public ValidationResult IsValid()
        {
            return ValidationResult.Success;
        }
        public ValidationResult IsValidPickPocketing(CrimeIncidentDTO incident)
        {
            if ((incident.MaxAllowedAmount + 1) < (incident.Amount))
            {
                return new ValidationResult(string.Format("you cannot snipe more than {0} % of their net cash", RulesSettings.MaxAllowedPickPocketPercent));
            }
            if (!incident.IsFriend)
            {
                return new ValidationResult("you can only snipe a friend.");
            }
            if (incident.RobbedRecently)
            {
                return new ValidationResult(string.Format("you cannot snipe same person in less than {0} days", RulesSettings.RobberyFrequencyonSamePersonCap));
            }

            return ValidationResult.Success;
        }

        public ValidationResult IsValidRobbery(CrimeIncidentDTO incident)
        {
            if (!incident.IsFriend)
            {
                return new ValidationResult("you can only rob a friend");
            }
            if (incident.RobbedRecently)
            {
                return new ValidationResult(string.Format("you cannot rob same person in less than {0} days", RulesSettings.RobberyFrequencyonSamePersonCap));
            }

            return ValidationResult.Success;
        }
        public ValidationResult IsValidSuspectReporting(CrimeIncidentDTO incident)
        {
            if (incident.VictimId == 0)
            {
                return new ValidationResult("invalid incident detected");
            }
            if (incident.SuspectAleadyNotified == true)
            {
                return new ValidationResult("you may only notify one suspect per incident");
            }
            return ValidationResult.Success;
        }


    }
}
