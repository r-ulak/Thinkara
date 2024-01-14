using DAO.Models;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Common;
namespace RulesEngine
{
    public class UserLoanRules : IRules
    {
        private UserLoan Loan { get; set; }
        private RequestLoanDTO BaseLoanRequest { get; set; }
        private RequestLoanDTO UserLoanRequest { get; set; }
        private UserLoanDTO UserLoanPayment { get; set; }
        private int UserId { get; set; }

        public UserLoanRules()
        {
        }
        public UserLoanRules(UserLoan loan, UserLoanDTO userLoanPayment, int userId)
        {
            Loan = loan;
            UserLoanPayment = userLoanPayment;
            UserId = userId;

        }

        public UserLoanRules(RequestLoanDTO baseLoanRequest,
            RequestLoanDTO userLoanRequest, int userId)
        {
            BaseLoanRequest = baseLoanRequest;
            UserLoanRequest = userLoanRequest;
            UserId = userId;
        }
        public ValidationResult IsValid()
        {
            if (AllowUpdateInsert() == false)
                return new ValidationResult("Access Denied");
            if (Loan.LoanAmount <= 0 || Loan.LeftAmount < 0 || Loan.PaidAmount < 0)
            {
                return new ValidationResult("Amount is not valid.");
            }
            if (!(Loan.Status == "P" ||
                Loan.Status == "A" ||
                Loan.Status == "D"
                ))
            {
                return new ValidationResult("Invalid Status");
            }
            return ValidationResult.Success;

        }

        public ValidationResult IsValidRequestLoan()
        {
            if (UserLoanRequest.LendorId == UserId)
            {
                return new ValidationResult("Invalid LendorId");

            }
            if (UserLoanRequest.LoanAmount <= 0)
            {
                return new ValidationResult("Amount is not valid.");
            }

            if (UserLoanRequest.MinMonthlyIntrestRate != BaseLoanRequest.MinMonthlyIntrestRate)
            {
                return new ValidationResult("Invalid intrest rate.");

            }

            return ValidationResult.Success;
        }
        public ValidationResult IsValidLoanPayment()
        {
            if (Loan.UserId != UserId)
            {
                return new ValidationResult("Invalid user");
            }
            if (UserLoanPayment.PayingAmount > Loan.LeftAmount)
            {
                return new ValidationResult("Amount is not valid.");
            }
            if (Loan.Status != "A")
            {
                return new ValidationResult("Invalid status");
            }
            return ValidationResult.Success;
        }

        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit Loan then send 1 else 0.
            return result;
        }
        public void AddLoanTask()
        {

        }

        public void AddResubmitLoanTask(List<string> errorList)
        {
            throw new Exception(string.Join(",", errorList.ToArray()));

        }

    }
}
