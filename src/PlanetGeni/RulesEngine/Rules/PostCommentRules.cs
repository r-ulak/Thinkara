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
using DTO.Custom;
namespace RulesEngine
{
    public class PostCommentRules : IRules
    {
        private ResourceManager
             resmgr = new ResourceManager("RulesEngine.ValidationMessage",
                               Assembly.GetExecutingAssembly());
        public PostCommentRules()
        {
        }
        public ValidationResult IsValid()
        {
            return ValidationResult.Success;
        }
        public ValidationResult IsValid(BuySpotDTO spotDetails)
        {
            if (spotDetails.Message.Length < 0 || spotDetails.Message.Length > 2500)
                return new ValidationResult("Spot Message length must be between 5 and 1000");

            return ValidationResult.Success;

        }

        public ValidationResult IsValidCost(BuySpotDTO spotDetails)
        {
            if ((spotDetails.CalculatedTotalCost > spotDetails.TotalCost))
                return new ValidationResult("cost calculated does not add up");

            return ValidationResult.Success;
        }

    }
}
