using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UserLoanDTORepository : IUserLoanDTORepository
    {
        private StoredProcedure spContext = new StoredProcedure();
        private IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        public UserLoanDTORepository()
        {
        }

        public IQueryable<UserLoanDTO> GetLoanList
                       (int userId, DateTime? lastLendorUpdatedAt = null,
            DateTime? lastBorrowerUpdatedAt = null)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmlastLendorUpdatedAt", lastLendorUpdatedAt);
            dictionary.Add("parmlastBorrowerUpdatedAt", lastBorrowerUpdatedAt);
            dictionary.Add("parmLimit", AppSettings.LoanLimit);
            IEnumerable<UserLoanDTO> userLoanList =
                spContext.GetSqlData<UserLoanDTO>
                (AppSettings.SPGetLoanListByUserId, dictionary);
            return userLoanList.AsQueryable();
        }
        private TaskReminder GetTaskReminder(DateTime dueDate, Guid taskId)
        {
            TaskReminder keyRequestReminder = new TaskReminder
            {
                TaskId = taskId,
                EndDate = dueDate,
                StartDate = DateTime.UtcNow,
                ReminderTransPort = "MP",
                ReminderFrequency = 4
            };

            return keyRequestReminder;
        }

        private UserTask GetTask(Guid taskId, int
            requestoruserId, DateTime trnDate,
            short defaultResponse,
            DateTime dueDate,
            StringBuilder parm,
            int lendorId)
        {


            UserTask loanRequestTask = new UserTask
            {
                TaskId = taskId,
                AssignerUserId = requestoruserId,
                CompletionPercent = 0,
                CreatedAt = trnDate,
                DefaultResponse = defaultResponse,
                DueDate = dueDate,
                Flagged = false,
                Priority = 10,
                Parms = parm.ToString(),
                Status = "I",
                TaskTypeId = AppSettings.LoanRequestTaskType,
                UserId = lendorId
            };
            return loanRequestTask;
        }
        private int AddLoanRequestTask(Guid taskId, int requestoruserId, RequestLoanDTO requestLoan, string fullName)
        {
            try
            {
                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();

                string defaultResponse = "Denied";
                parm.AppendFormat(@"{0}|{1}|{2}|{3}|<strong>Date:{4}</strong>|{5}",
               requestoruserId, fullName, requestLoan.LoanAmount,
              requestLoan.MonthlyIntrestRate, dueDate, defaultResponse);
                UserTask loanrequestTask = GetTask(taskId, requestoruserId, trnDate, (short)
                                            AppSettings.UserLoanDenialChoiceId, dueDate, parm, requestLoan.LendorId);
                return spContext.Add(loanrequestTask);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save loan request");
                return -1;
            }
        }
        public bool SaveLoanRequest(RequestLoanDTO requestloan, Guid taskId, int requestoruserId, string fullName)
        {
            bool result = false;
            try
            {

                UserLoan loan = new UserLoan()
                {
                    CreatedAt = DateTime.UtcNow,
                    LeftAmount = requestloan.LoanAmount * (1 + requestloan.MonthlyIntrestRate / 100),
                    LendorId = requestloan.LendorId,
                    LoanAmount = requestloan.LoanAmount,
                    MonthlyInterestRate = requestloan.MonthlyIntrestRate,
                    PaidAmount = 0,
                    Status = "P", //Pending"
                    TaskId = taskId,
                    UpdatedAt = DateTime.UtcNow,
                    UserId = requestoruserId
                };
                if (requestloan.LendorId == AppSettings.BankId)
                {
                    if (requestloan.MonthlyIntrestRate < requestloan.MinMonthlyIntrestRate)
                    {
                        requestloan.MonthlyIntrestRate = requestloan.MinMonthlyIntrestRate;
                        loan.MonthlyInterestRate = requestloan.MonthlyIntrestRate;
                    }
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("parmUserId", loan.UserId);
                    dictionary.Add("parmLendorId", loan.LendorId);

                    decimal totalleftunpaidLoanamount = (decimal)spContext.GetSqlDataSignleValue
                            (AppSettings.SPGetPendingLoanPayment, dictionary, "TotalLeftAmount");
                    sbyte priority = 1;

                    if (totalleftunpaidLoanamount < requestloan.QualifiedAmount)
                    {
                        dictionary.Add("parmTaskId", taskId);
                        dictionary.Add("parmBankId", AppSettings.BankId);
                        dictionary.Add("parmFundType", AppSettings.LoanFundType);
                        dictionary.Add("parmLoanAmount", loan.LoanAmount);

                        int response = (int)spContext.GetSqlDataSignleValue
                                (AppSettings.SPApproveLoanRequest, dictionary, "result");
                        loan.Status = "A";
                    }
                    else
                    {
                        loan.Status = "D";
                        priority = 7;
                    }
                    string loanresult = loan.Status == "A" ? "Approved" : "Denied";
                    String parmText = "";
                    parmText = string.Format("{0}|{1}|{2}|{3}",
               loan.LoanAmount,
               loan.MonthlyInterestRate,
              webRepo.GetFullName(AppSettings.BankId),
               loanresult);

                    userNotif.AddNotification(false, string.Empty,
                               AppSettings.LoanRequestVotingResultNotificationId,
                                    parmText.ToString(), priority, loan.UserId);
                }
                else
                {
                    AddLoanRequestTask(taskId, requestoruserId, requestloan, fullName);
                }
                spContext.Add(loan);

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionLogging.LogError(ex, "Failed trying to save loan");
                return result;
            }
        }
        public RequestLoanDTO GetQuailfiedIntrestedRate(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            return
                spContext.GetSqlDataSignleRow<RequestLoanDTO>(AppSettings.SPGetQuailfiedIntrestedRate, dictionary);

        }
        private int UpdateLoanStatus(Guid taskId, char status)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            dictionary.Add("parmStatus", status);
            return spContext.ExecuteStoredProcedure(AppSettings.SPUpdateLoanStatus, dictionary);

        }

        public bool MakePayment(UserLoanDTO loanPayment, int userId)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskId", loanPayment.TaskId);
                dictionary.Add("parmSourceId", userId);
                dictionary.Add("parmLendorId", loanPayment.UserId);
                dictionary.Add("parmBankId", AppSettings.BankId);
                dictionary.Add("parmFundType", AppSettings.LoanFundType);
                dictionary.Add("parmPayingAmount", loanPayment.PayingAmount);

                int response = (int)spContext.GetSqlDataSignleValue
         (AppSettings.SPExecuteLoanPayment, dictionary, "result");

                if (response != 1)
                {
                    return false;
                }

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                result = false;
                ExceptionLogging.LogError(ex, "Failed trying MakePayment");
                return result;
            }

        }
        public UserLoan GetLoanById(Guid taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);

            return spContext.GetByPrimaryKey<UserLoan>(dictionary);
        }

        public int UpdateLoanVoteResponse(VoteResponseDTO voteResponse)
        {
            UserLoan loan = GetLoanById(voteResponse.TaskId);
            if (voteResponse.ChoiceIds[0] == AppSettings.UserLoanApprovalChoiceId)
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskId", loan.TaskId);
                dictionary.Add("parmUserId", loan.UserId);
                dictionary.Add("parmLendorId", loan.LendorId);
                dictionary.Add("parmBankId", AppSettings.BankId);
                dictionary.Add("parmFundType", AppSettings.LoanFundType);
                dictionary.Add("parmLoanAmount", loan.LoanAmount);

                int response = (int)spContext.GetSqlDataSignleValue
                        (AppSettings.SPApproveLoanRequest, dictionary, "result");

                return response;
            }
            else if (voteResponse.ChoiceIds[0] == AppSettings.UserLoanDenialChoiceId)
            {
                UpdateLoanStatus(loan.TaskId, 'D');
                return 0;
            }
            return 0;

        }
    }


}
