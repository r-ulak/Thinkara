using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class CountryBudgetRules : IRules
    {
        public CountryBudgetDetailsDTO NewBudgetDetails { get; set; }
        public CountryBudget OldBudget { get; set; }

        public CountryBudgetRules()
        {

        }
        public CountryBudgetRules(CountryBudgetDetailsDTO budgetDetails, CountryBudget budget)
        {
            NewBudgetDetails = budgetDetails;
            OldBudget = budget;
        }

        public ValidationResult IsValid()
        {
            if (NewBudgetDetails.AllowEdit == false)
            {
                return new ValidationResult("Access Denied"); ;
            } 
            decimal total = NewBudgetDetails.BudgetType.Sum(item => item.Amount);

            if (total > OldBudget.TotalAmount) // Check if the total Amount of budgetType is less than Total Budget from DB.
            {
                return new ValidationResult(string.Format("total amount {0} of budget is more than allocated total {1}", total, OldBudget.TotalAmount)); ;
            }
            decimal totalPercent = NewBudgetDetails.BudgetType.Sum(item => item.BudgetPercent);
            if (totalPercent > 100)
            {
                return new ValidationResult("total percent is more than 100"); ;

            }

            //if (OldBudget.Status == 1 || OldBudget.Status == 2 || OldBudget.Status == 4)
            //{
            //    return new ValidationResult("Invalid Budget Status"); ;

            //}
            return ValidationResult.Success;

        }

        public CountryBudgetDetailsDTO CheckBeforeSave()
        {

            NewBudgetDetails.Status = "P";

            //these fields should not change on client side, 
            //set it to what it is supposed to be from Db
            NewBudgetDetails.TotalAmount = OldBudget.TotalAmount;
            NewBudgetDetails.CountryId = OldBudget.CountryId;

            return NewBudgetDetails;
        }





    }
}
