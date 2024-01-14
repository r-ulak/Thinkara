using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class UserLoanServiceController : ApiController
    {
        IUserLoanDTORepository _repository;
        IWebUserDTORepository webRepo = new WebUserDTORepository();

        public UserLoanServiceController(IUserLoanDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public UserLoanServiceController()
        {
            _repository = new UserLoanDTORepository();
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        public IEnumerable<UserLoanDTO> GetLoanList
            (DateTime? lastLendorUpdatedAt = null,
            DateTime? lastBorrowerUpdatedAt = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<UserLoanDTO> UserLoanDTOs =
                _repository.GetLoanList(userid, lastLendorUpdatedAt, lastBorrowerUpdatedAt);
            return UserLoanDTOs;
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        public RequestLoanDTO GetQuailfiedIntrestedRate()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            return _repository.GetQuailfiedIntrestedRate(userid);
        }
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public PostResponseDTO MakePayment(UserLoanDTO loanPayment)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);

            Task taskA = Task.Factory.StartNew(() =>
                ProcessPayment(loanPayment, userid));
            return new PostResponseDTO
            {
                Message = "Loan Payment Successfully Submitted",
                StatusCode = 200
            };
        }

        private void ProcessPayment(UserLoanDTO loanPayment, int userid)
        {
            string fullName = webRepo.GetFullName(userid);
            UserLoanRules loanRules = new UserLoanRules(
         _repository.GetLoanById(loanPayment.TaskId), loanPayment, userid);
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            String parmText = "";
            short notificationTypeId = AppSettings.LoanPaymentFailNotificationId;
            ValidationResult validationResult = loanRules.IsValidLoanPayment();
            sbyte priority = 0;
            DateTime dateTime = DateTime.UtcNow;


            if (validationResult == ValidationResult.Success)
            {
                bool result = _repository.MakePayment(loanPayment, userid);
                if (!result)
                {
                    parmText = string.Format("{0}|{1}|{2}",
                                loanPayment.PayingAmount,
                                loanPayment.FullName,
                                AppSettings.UnexpectedErrorMsg);
                    priority = 6;
                }
                else
                {
                    parmText = string.Format("{0}|{1}",
                         loanPayment.PayingAmount,
                         loanPayment.FullName
                         );
                    notificationTypeId = AppSettings.LoanPaymentSuccessNotificationId;
                    priority = 1;

                    string parmNotifyLendor = String.Format("{0}|{1}|{2}|<strong>Date:{3}</strong>",
                        fullName, loanPayment.PayingAmount, loanPayment.MonthlyInterestRate, loanPayment.CreatedAt.ToString());
                    userNotif.AddNotification(false, string.Empty, AppSettings.LoanPaymentNotificationId
      , parmNotifyLendor, priority, loanPayment.NextPartyId);
                }
            }
            else
            {
                parmText = string.Format("{0}|{1}|{2}",
               loanPayment.PayingAmount,
               loanPayment.FullName,
               validationResult.ErrorMessage);
                priority = 6;
            }
            userNotif.AddNotification(false, string.Empty,
       notificationTypeId, parmText.ToString(), priority, userid);
        }

        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public PostResponseDTO RequestLoan(RequestLoanDTO loanRequest)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() =>
                ProcessLoanRequest(loanRequest, userid));
            return new PostResponseDTO
            {
                Message = "Loan Request Successfully Submitted",
                StatusCode = 200
            };
        }

        private void ProcessLoanRequest(RequestLoanDTO loanRequest, int userid)
        {
            try
            {
                string fullName = webRepo.GetFullName(userid);
                UserLoanRules loanRules = new UserLoanRules(
      _repository.GetQuailfiedIntrestedRate(userid), loanRequest, userid);

                IWebUserDTORepository webuserRepo = new WebUserDTORepository();
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = AppSettings.LoanRequestFailNotificationId;
                if (loanRequest.LendorId == AppSettings.BankId)
                {
                    RequestLoanDTO qualifiedloan =
                    _repository.GetQuailfiedIntrestedRate(userid);
                    loanRequest.MinMonthlyIntrestRate = qualifiedloan.MinMonthlyIntrestRate;
                    loanRequest.QualifiedAmount = qualifiedloan.QualifiedAmount;
                }
                ValidationResult validationResult = loanRules.IsValidRequestLoan();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;

                WebUser loanLendor = webuserRepo.GetWebUser(loanRequest.LendorId);

                if (validationResult == ValidationResult.Success)
                {
                    Guid taskid = Guid.NewGuid();
                    bool result = _repository.SaveLoanRequest(loanRequest, taskid, userid, fullName);
                    if (!result)
                    {
                        parmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}|{4}",
                  loanRequest.LoanAmount,
                  loanRequest.MonthlyIntrestRate,
                  dateTime.ToString(),
                  loanLendor.NameFirst + " " + loanLendor.NameLast,
                 AppSettings.UnexpectedErrorMsg);
                        priority = 6;
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}",
                              loanRequest.LoanAmount,
                              loanRequest.MonthlyIntrestRate,
                              dateTime.ToString(),
                              loanLendor.NameFirst + " " + loanLendor.NameLast);
                        notificationTypeId = AppSettings.LoanRequestSuccessNotificationId;
                        priority = 2;

                        string parmNotifyLendor = String.Format("{0}|{1}|{2}",
                            fullName, loanRequest.LoanAmount, loanRequest.MonthlyIntrestRate);
                        userNotif.AddNotification(true, taskid.ToString(), AppSettings.LoanRequestTaskNotificationId
          , parmNotifyLendor, priority, loanRequest.LendorId);
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}|{4}",
                   loanRequest.LoanAmount,
                   loanRequest.MonthlyIntrestRate,
                   dateTime.ToString(),
                   loanLendor.NameFirst + " " + loanLendor.NameLast,
                   validationResult.ErrorMessage);
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, userid);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessLoanRequest");
            }
        }

    }
}
