using Common;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.ServiceController
{
    public class UserVoteManager
    {
        IUserVoteDTORepository _repository;
        private IRedisCacheProvider cache { get; set; }
        public UserVoteManager(IUserVoteDTORepository repo)
        {
            _repository = repo;
        }
        public UserVoteManager()
        {
            _repository = new UserVoteDTORepository();
        }

        public void ProcessVotingResponse(VoteResponseDTO voteResponse, int userid)
        {
            ITaskTypeRepository _taskrepository
                 = new TaskTypeRepository();

            IUserTaskDetailsDTORepository taskRepo
                  = new UserTaskDetailsDTORepository();

            try
            {
                TaskType voteTaskType = JsonConvert.DeserializeObject<TaskType>(
                   _taskrepository.GetTaskDescriptionByType(voteResponse.TaskTypeId));

                voteResponse.IsIncomepleteTask = taskRepo.IsIncomepleteTask(voteResponse.TaskId, userid);

                UserVoteRules userVoteRule = new UserVoteRules(
                    voteResponse,
                    voteTaskType,
                   _repository.GetVoteChoiceByTaskType(voteResponse.TaskTypeId).ToList()
                    );
                if (userVoteRule.IsValid() == ValidationResult.Success)
                {
                    _repository.SaveVoteResponse(voteResponse, userid);
                    ProcessTaskResponse(voteResponse, userid);

                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessVotingResponse");

            }
        }
        private void ProcessTaskResponse(VoteResponseDTO voteResponse, int userId)
        {
            if (voteResponse.TaskTypeId == AppSettings.LoanRequestTaskType)
            {
                ProcessUserLoanResponse(voteResponse);
            }
            else if (voteResponse.TaskTypeId == AppSettings.TaxTaskType)
            {
                ProcessCountryTaxResponse(voteResponse, userId);
            }
            else if (voteResponse.TaskTypeId == AppSettings.BudgetTaskType)
            {
                ProcessCountryBudgetResponse(voteResponse, userId);
            }
            else if (voteResponse.TaskTypeId == AppSettings.WarTaskType)
            {
                ProcessWarKeyResponse(voteResponse);
            }
            else if (voteResponse.TaskTypeId == AppSettings.JoinPartyRequestTaskType)
            {
                ProcessJoinPartyRequestResponse(voteResponse, userId);
            }

            else if (voteResponse.TaskTypeId == AppSettings.JoinPartyRequestInviteTaskType)
            {
                ProcessJoinPartyRequestInvitationResponse(voteResponse, userId);
            }

            else if (voteResponse.TaskTypeId == AppSettings.NominationNotifyPartyTaskType)
            {
                ProcessNotifyNominationRequestResponse(voteResponse);
            }
            else if (voteResponse.TaskTypeId == AppSettings.NominationPartyTaskType)
            {
                ProcessNominationElectionRequestResponse(voteResponse);
            }
            else if (voteResponse.TaskTypeId == AppSettings.EjectPartyTaskType)
            {
                ProcessPartyEjectionResponse(voteResponse, userId);
            }
            else if (voteResponse.TaskTypeId == AppSettings.JoinPartyTaskType)
            {
                ProcessInvitationResponse(voteResponse, userId);
            }
            else if (voteResponse.TaskTypeId == AppSettings.ClosePartyTaskType)
            {
                ProcessClosePartyElectionRequestResponse(voteResponse);
            }
            else if (voteResponse.TaskTypeId == AppSettings.RunForOfficeAsPartyTaskType)
            {
                ProcessRunForOfficePartyResponse(voteResponse);
            }
            else if (voteResponse.TaskTypeId == AppSettings.RunForOfficeAsIndividualTaskType)
            {
                ProcessRunForOfficeIndependentResponse(voteResponse);
            }
            else if (voteResponse.TaskTypeId == AppSettings.JobTaskType)
            {
                ProcessUserJobResponse(voteResponse);
            }

        }
        #region UserJob
        private void ProcessUserJobResponse(VoteResponseDTO voteResponse)
        {
            UserJobManager jobsManager = new UserJobManager();
            if (voteResponse.ChoiceRadioId == AppSettings.JobOfferAcceptChoiceId)
            {
                jobsManager.ProcessUserJobAcceptance(voteResponse.TaskId);
            }
            else if (voteResponse.ChoiceRadioId == AppSettings.JobOfferRejectChoiceId)
            {
                jobsManager.ProcessUserJobRejection(voteResponse.TaskId);

            }
        }
        #endregion UserJob

        #region UserLoan
        private void ProcessUserLoanResponse(VoteResponseDTO voteResponse)
        {
            IUserLoanDTORepository _repo = new UserLoanDTORepository();
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            IWebUserDTORepository webuserRepo = new WebUserDTORepository();
            UserLoan userLoan = _repo.GetLoanById(voteResponse.TaskId);

            string loandLendor = webuserRepo.GetFullName(userLoan.LendorId);
            int response = _repo.UpdateLoanVoteResponse(voteResponse);
            if (response != 1)
            {
                voteResponse.ChoiceRadioId = AppSettings.UserLoanDenialChoiceId;
            }
            string parmText = PrepareUserLoanNotificationMsg(userLoan, voteResponse, loandLendor);
            sbyte priority = 1;
            if (voteResponse.ChoiceRadioId == AppSettings.UserLoanDenialChoiceId)
            {
                priority = 7;
            }
            userNotif.AddNotification(false, string.Empty,
         AppSettings.LoanRequestVotingResultNotificationId, parmText, priority, userLoan.UserId);


        }
        private string PrepareUserLoanNotificationMsg(UserLoan userLoan, VoteResponseDTO voteResponse, string loandLendor)
        {
            String parmText = "";
            string result = voteResponse.ChoiceRadioId == AppSettings.UserLoanApprovalChoiceId ? "Approved" : "Denied";
            parmText = string.Format("{0}|{1}|{2}|{3}",
                userLoan.LoanAmount,
                userLoan.MonthlyInterestRate,
                loandLendor,
                result);
            return parmText.ToString();
        }
        #endregion UserLoan

        #region WarRequest
        private string PrepareWarVoteCountNotficationMsg
            (IEnumerable<ChoiceCountDTO> taskVoteCount, int voteNeeded, RequestWarKey warKey, int totalVoters)
        {
            String parmText = "";
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            CountryCode targetCountry =
            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(warKey.TaregtCountryId));
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.WarKeyApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.WarKeyDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            parmText = string.Format("<strong>{0}</strong>|<strong>{1}</strong>|<strong>{2}</strong> <span class='flagsprite flagsprite-{3} inline'></span>|{4}|{5}",
                     approvalVote, denialVote,
                       targetCountry.Code, targetCountry.CountryId,
                       voteNeeded, totalVoters);
            return parmText.ToString();
        }
        private string PrepareWarResultNotficationMsg
    (IEnumerable<ChoiceCountDTO> taskVoteCount, RequestWarKey warKey, string result)
        {
            String parmText = "";
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            CountryCode targetCountry =
            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(warKey.TaregtCountryId));
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.WarKeyApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.WarKeyDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            parmText = string.Format("<strong>{0}</strong> <span class='flagsprite flagsprite-{1} inline'></span>|<strong>{2}</strong>|{3}|{4}",
                       targetCountry.Code, targetCountry.CountryId, result,
                     approvalVote, denialVote);
            return parmText.ToString();
        }
        private void ProcessWarKeyResponse(VoteResponseDTO voteResponse)
        {
            IWarRequestRepository _repo = new WarRequestRepository();
            RequestWarKey warRequest = _repo.GetRequestWarKeyByTask(voteResponse.TaskId.ToString());
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

            ICountryLeaderRepository _countrRepo = new CountryLeaderRepository();
            int totalVoters = _countrRepo.GetTotalActiveSeneator(warRequest.RequestingCountryId);
            double voteNeeded = totalVoters * AppSettings.WarKeyApprovalVoteNeeded;

            IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);
            int result = _repository.HasEnoughVoteForThisTask(AppSettings.WarKeyApprovalChoiceId, AppSettings.WarKeyDenialChoiceId,
                                            AppSettings.WarKeyApprovalVoteNeeded, totalVoters, taskVoteCount);
            if (result == AppSettings.WarKeyApprovalChoiceId)
            {
                //TODO Start War PRocess

                //Notify the Requestor 
                string parmtext = PrepareWarResultNotficationMsg(taskVoteCount,
                     warRequest, "Approved");

                userNotif.AddNotification(false, string.Empty,
         AppSettings.WarRequestResultNotificationId, parmtext, 10, warRequest.RequestingUserId);
            }
            else if (result == AppSettings.WarKeyDenialChoiceId)
            {
                string parmtext = PrepareWarResultNotficationMsg(taskVoteCount,
               warRequest, "Denied");
                userNotif.AddNotification(false, string.Empty,
                        AppSettings.WarRequestResultNotificationId, parmtext, 10, warRequest.RequestingUserId);
            }
            else
            {
                string parmtext = PrepareWarVoteCountNotficationMsg(taskVoteCount,
                    Convert.ToInt32(Math.Ceiling(voteNeeded)), warRequest, totalVoters);

                userNotif.AddNotification(false, string.Empty,
         AppSettings.WarRequestVotingCountNotificationId, parmtext, 4, warRequest.RequestingUserId);

            }

        }
        #endregion WarRequest

        #region Tax
        private void ProcessCountryTaxResponse(VoteResponseDTO voteResponse, int userId)
        {
            ICountryTaxDetailsDTORepository _repo = new CountryTaxDetailsDTORepository();
            CountryTax newTax = _repo.GetCountryTaxByTaskId(voteResponse.TaskId.ToString());
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();


            ICountryLeaderRepository _countrRepo = new CountryLeaderRepository();
            int totalVoters = _countrRepo.GetTotalActiveSeneator(newTax.CountryId);
            double voteNeeded = totalVoters * AppSettings.TaxAmedApprovalVoteNeeded;

            IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

            int result = _repository.HasEnoughVoteForThisTask(AppSettings.TaxApprovalChoiceId, AppSettings.TaxDenialChoiceId,
                                        AppSettings.TaxAmedApprovalVoteNeeded, totalVoters, taskVoteCount);

            IUserTaskDetailsDTORepository userTaskRepo = new UserTaskDetailsDTORepository();
            UserTask usertask = userTaskRepo.GetTaskById(voteResponse.TaskId, userId);

            if (result == AppSettings.TaxApprovalChoiceId)
            {
                if (newTax.Status != "A")
                {
                    CountryTaxDetailsDTO currentTax = _repo.GetCountryTax(newTax.CountryId);
                    _repo.ApproveNewTaxAmendments(currentTax.TaskId.ToString(),
                            newTax.TaskId.ToString());
                    string parmtext = PrepareTaxResultNotficationMsg(taskVoteCount,
           newTax, "Approved");

                    userNotif.AddNotification(false, string.Empty,
             AppSettings.TaxAmendResultNotificationId, parmtext, 8, usertask.AssignerUserId);
                }
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else if (result == AppSettings.TaxDenialChoiceId)
            {
                string parmtext = PrepareTaxResultNotficationMsg(taskVoteCount,
               newTax, "Denied");
                userNotif.AddNotification(false, string.Empty,
                        AppSettings.TaxAmendResultNotificationId, parmtext, 8, usertask.AssignerUserId);
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else
            {
                string parmtext = PrepareTaxVoteCountNotficationMsg(taskVoteCount,
                    Convert.ToInt32(Math.Ceiling(voteNeeded)), newTax, totalVoters);

                userNotif.AddNotification(false, string.Empty,
         AppSettings.TaxAmendVotingCountNotificationId, parmtext, 4, usertask.AssignerUserId);

            }

        }
        private string PrepareTaxVoteCountNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, int voteNeeded, CountryTax countryTax, int totalVoters)
        {
            String parmText = "";
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            CountryCode targetCountry =
            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(countryTax.CountryId));
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.TaxApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.TaxDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            parmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}|{4}|{5}|{6}",
                     approvalVote, denialVote, countryTax.CreatedAt.ToString(),
                       targetCountry.Code, targetCountry.CountryId,
                       voteNeeded, totalVoters);
            return parmText.ToString();
        }
        private string PrepareTaxResultNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, CountryTax countryTax, string result)
        {
            String parmText = "";
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            CountryCode targetCountry =
            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(countryTax.CountryId));
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.TaxApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.TaxDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            parmText = string.Format("{0}|{1}|{2}|{3}|{4}|<strong>Date:{5}</strong>",
                result, approvalVote, denialVote,
                       targetCountry.Code, targetCountry.CountryId, countryTax.CreatedAt.ToString());
            return parmText.ToString();
        }
        #endregion Tax

        #region Budget

        private void ProcessCountryBudgetResponse(VoteResponseDTO voteResponse, int userId)
        {
            ICountryBudgetDetailsDTORepository _repo = new CountryBudgetDetailsDTORepository();
            CountryBudget newBudget = _repo.GetCountryBudgetByTaskId(voteResponse.TaskId);
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();


            ICountryLeaderRepository _countrRepo = new CountryLeaderRepository();
            int totalVoters = _countrRepo.GetTotalActiveSeneator(newBudget.CountryId);
            double voteNeeded = totalVoters * AppSettings.BudgetAmedApprovalVoteNeeded;

            IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

            int result = _repository.HasEnoughVoteForThisTask(AppSettings.BudgetApprovalChoiceId, AppSettings.BudgetDenialChoiceId,
                                        AppSettings.BudgetAmedApprovalVoteNeeded, totalVoters, taskVoteCount);

            IUserTaskDetailsDTORepository userTaskRepo = new UserTaskDetailsDTORepository();
            UserTask usertask = userTaskRepo.GetTaskById(voteResponse.TaskId, userId);

            if (result == AppSettings.BudgetApprovalChoiceId)
            {
                if (newBudget.Status != "A")
                {
                    CountryBudgetDetailsDTO currentBudget = JsonConvert.DeserializeObject<CountryBudgetDetailsDTO>(_repo.GetCountryBudget(newBudget.CountryId));
                    _repo.ApproveNewBudgetAmendments(currentBudget.TaskId.ToString(),
                            newBudget.TaskId.ToString());
                    string parmtext = PrepareBudgetResultNotficationMsg(taskVoteCount,
           newBudget, "Approved");

                    userNotif.AddNotification(false, string.Empty,
             AppSettings.BudgetAmendResultNotificationId, parmtext, 8, usertask.AssignerUserId);
                }
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
                //cache.HashRemove(AppSettings.RedisHashCountryBudget, newBudget.CountryId);
                //cache.HashRemove(AppSettings.RedisHashCountryProfile + newBudget.CountryId, "budget");
            }
            else if (result == AppSettings.BudgetDenialChoiceId)
            {
                string parmtext = PrepareBudgetResultNotficationMsg(taskVoteCount,
               newBudget, "Denied");
                userNotif.AddNotification(false, string.Empty,
                        AppSettings.BudgetAmendResultNotificationId, parmtext, 8, usertask.AssignerUserId);
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else
            {
                string parmtext = PrepareBudgetVoteCountNotficationMsg(taskVoteCount,
                    Convert.ToInt32(Math.Ceiling(voteNeeded)), newBudget, totalVoters);

                userNotif.AddNotification(false, string.Empty,
         AppSettings.BudgetAmendVotingCountNotificationId, parmtext, 4, usertask.AssignerUserId);

            }

        }
        private string PrepareBudgetVoteCountNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, int voteNeeded, CountryBudget countryBudget, int totalVoters)
        {
            String parmText = "";
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            CountryCode targetCountry =
            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(countryBudget.CountryId));
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.BudgetApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.BudgetDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            parmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}|{4}|{5}|{6}",
                     approvalVote, denialVote, countryBudget.CreatedAt.ToString(),
                       targetCountry.Code, targetCountry.CountryId,
                       voteNeeded, totalVoters);
            return parmText.ToString();
        }
        private string PrepareBudgetResultNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, CountryBudget countryBudget, string result)
        {
            String parmText = "";
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            CountryCode targetCountry =
            JsonConvert.DeserializeObject<CountryCode>(countryRepo.GetCountryCodeJson(countryBudget.CountryId));
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.BudgetApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.BudgetDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            parmText = string.Format("{0}|{1}|{2}|{3}|{4}|<strong>Date:{5}</strong>",
                result, approvalVote, denialVote,
                       targetCountry.Code, targetCountry.CountryId, countryBudget.CreatedAt.ToString());
            return parmText.ToString();
        }
        #endregion Budget

        #region Political Party

        #region Close Party Request
        private void ProcessClosePartyElectionRequestResponse(VoteResponseDTO voteResponse)
        {
            IPartyDTORepository _repo = new PartyDTORepository();
            PartyCloseRequest partyClose = _repo.GetPartyCloseRequest(voteResponse.TaskId.ToString());

            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

            PoliticalParty partyInfo = _repo.GetPartyById(partyClose.PartyId.ToString());
            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;
            double voteNeeded;
            double approvalPercent = AppSettings.PartyCloseApprovalVoteNeeded;


            voteNeeded = totalVoters * approvalPercent;

            IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

            int result = _repository.HasEnoughVoteForThisTask(AppSettings.PartyCloseApprovalChoiceId, AppSettings.PartyCloseDenialChoiceId,
                                        approvalPercent, totalVoters, taskVoteCount);

            if (result == AppSettings.PartyCloseApprovalChoiceId)
            {
                if (partyClose.Status == "P")
                {
                    partyClose.Status = "A";
                    _repo.CloseParty(partyClose);
                    string parmtext = PrepareClosePartyElectionResultNotficationMsg(taskVoteCount, partyClose, "Approved");

                    userNotif.AddNotification(false, string.Empty,
             AppSettings.PartyCloseResultNotificationId, parmtext, 4, partyClose.UserId);
                }
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else if (result == AppSettings.PartyCloseDenialChoiceId)
            {
                partyClose.Status = "D";
                _repo.CloseParty(partyClose);
                string parmtext = PrepareClosePartyElectionResultNotficationMsg(taskVoteCount, partyClose, "Denied");
                userNotif.AddNotification(false, string.Empty,
AppSettings.PartyCloseResultNotificationId, parmtext, 8, partyClose.UserId);
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else
            {
                string parmtext = PrepareClosePartyElectionVoteCountNotficationMsg(taskVoteCount,
     Convert.ToInt32(Math.Ceiling(voteNeeded)), partyClose, totalVoters);

                userNotif.AddNotification(false, string.Empty,
         AppSettings.PartyCloseVotingCountNotificationId, parmtext, 4, partyClose.UserId);
            }


        }

        private string PrepareClosePartyElectionResultNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, PartyCloseRequest partyClose, string result)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            string partyName = partyRepo.GetpartyName(partyClose.PartyId.ToString());


            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyCloseApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyCloseDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
    partyClose.PartyId.ToString(), partyName,
  result, approvalVote, denialVote, partyClose.RequestDate.ToString());

            return notificationparmText.ToString();
        }

        private string PrepareClosePartyElectionVoteCountNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, int voteNeeded, PartyCloseRequest partyClose, int totalVoters)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            string partyName = partyRepo.GetpartyName(partyClose.PartyId.ToString());


            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyCloseApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyCloseDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                 approvalVote, denialVote,
 partyClose.PartyId.ToString(), partyName, voteNeeded, totalVoters);

            return notificationparmText.ToString();
        }
        #endregion Close Party Request

        #region Nomination
        private void ProcessNotifyNominationRequestResponse(VoteResponseDTO voteResponse)
        {
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository _repo = new PartyDTORepository();
            PartyNomination partyNomination = _repo.GetPartyNomination(voteResponse.TaskId.ToString());

            if (voteResponse.ChoiceRadioId == AppSettings.NotifyNominationRequestApprovalChoiceId)
            {
                PartyNominationDTO nominationDTO = new PartyNominationDTO
                {
                    PartyName = _repo.GetpartyName(partyNomination.PartyId.ToString()),
                    InitatorPartyId = partyNomination.PartyId.ToString(),
                    InitatorId = partyNomination.InitatorId,
                    InitatorFullName = webRepo.GetFullName(partyNomination.InitatorId),
                    InitatorPicture = webRepo.GetUserPicture(partyNomination.InitatorId),
                    PartyId = partyNomination.PartyId.ToString(),
                    NominatingMemberType = partyNomination.NominatingMemberType,
                    NomineeId = partyNomination.NomineeId,
                    NomineeFullName = webRepo.GetFullName(partyNomination.NomineeId),
                    NomineePicture = webRepo.GetUserPicture(partyNomination.NomineeId),

                };

                _repo.RequestNominationPartyMember(nominationDTO, voteResponse.TaskId);
            }
            else if (voteResponse.ChoiceRadioId == AppSettings.NotifyNominationRequestDenialChoiceId)
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                StringBuilder notificationparmText = new StringBuilder();
                notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
 partyNomination.NomineeId, webRepo.GetFullName(partyNomination.NomineeId), partyNomination.GetPartyMemberType(), partyNomination.PartyId.ToString(), _repo.GetpartyName(partyNomination.PartyId.ToString()));

                userNotif.AddNotification(false, string.Empty,
       AppSettings.PartyNotifyRejectNominationNotificationId, notificationparmText.ToString(), 4, partyNomination.InitatorId);
                _repo.UpdateMemberStatus(partyNomination.NomineeId, partyNomination.PartyId.ToString(), string.Empty);
            }

        }
        private void ProcessNominationElectionRequestResponse(VoteResponseDTO voteResponse)
        {
            IPartyDTORepository _repo = new PartyDTORepository();
            PartyNomination partyNomination = _repo.GetPartyNomination(voteResponse.TaskId.ToString());

            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

            PoliticalParty partyInfo = _repo.GetPartyById(partyNomination.PartyId.ToString());
            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;
            double voteNeeded;
            double approvalPercent;
            if (partyNomination.NominatingMemberType == "F")
            {
                approvalPercent = AppSettings.PartyNominationFounderApprovalVoteNeeded;
            }
            else if (partyNomination.NominatingMemberType == "C")
            {
                approvalPercent = AppSettings.PartyNominationCoFounderApprovalVoteNeeded;
            }
            else
            {
                approvalPercent = AppSettings.PartyNominationMemberApprovalVoteNeeded;
            }

            voteNeeded = totalVoters * approvalPercent;

            IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

            int result = _repository.HasEnoughVoteForThisTask(AppSettings.PartyNominationElectionApprovalChoiceId, AppSettings.PartyNominationElectionDenialChoiceId,
                                        approvalPercent, totalVoters, taskVoteCount);

            if (result == AppSettings.PartyNominationElectionApprovalChoiceId)
            {
                if (partyNomination.Status == "P")
                {
                    _repo.ProcessNominationApproval(partyNomination);
                    string parmtext = PrepareNominationElectionResultNotficationMsg(taskVoteCount, partyNomination, "Approved");

                    userNotif.AddNotification(false, string.Empty,
             AppSettings.PartyNominationResultNotificationId, parmtext, 4, partyNomination.InitatorId);
                }
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else if (result == AppSettings.PartyNominationElectionDenialChoiceId)
            {
                _repo.UpdateNomination(partyNomination, "D");
                string parmtext = PrepareNominationElectionResultNotficationMsg(taskVoteCount, partyNomination, "Denied");
                userNotif.AddNotification(false, string.Empty,
         AppSettings.PartyNominationResultNotificationId, parmtext, 8, partyNomination.InitatorId);
                userNotif.AddNotification(false, string.Empty,
AppSettings.PartyNominationResultNotificationId, parmtext, 8, partyNomination.NomineeId);

                _repository.DeleteRemaningTaskNotComplete(voteResponse);
                _repo.UpdateMemberStatus(partyNomination.NomineeId, partyNomination.PartyId.ToString(), string.Empty);
            }
            else
            {
                string parmtext = PrepareNominationElectionVoteCountNotficationMsg(taskVoteCount,
     Convert.ToInt32(Math.Ceiling(voteNeeded)), partyNomination, totalVoters);

                userNotif.AddNotification(false, string.Empty,
         AppSettings.PartyNominationVotingCountNotificationId, parmtext, 4, partyNomination.InitatorId);
            }


        }

        private string PrepareNominationElectionResultNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, PartyNomination partyNomination, string result)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            string partyMemberType = partyNomination.GetPartyMemberType();
            string partyName = partyRepo.GetpartyName(partyNomination.PartyId.ToString());


            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyNominationElectionApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyNominationElectionDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}",
     partyNomination.NomineeId, webRepo.GetFullName(partyNomination.NomineeId), partyMemberType, partyNomination.PartyId.ToString(), partyName, result, approvalVote, denialVote, partyNomination.RequestDate.ToString());


            return notificationparmText.ToString();
        }

        private string PrepareNominationElectionVoteCountNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, int voteNeeded, PartyNomination partyNomination, int totalVoters)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            string partyMemberType = partyNomination.GetPartyMemberType();
            string partyName = partyRepo.GetpartyName(partyNomination.PartyId.ToString());


            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyNominationElectionApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyNominationElectionDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                 approvalVote, denialVote,
      partyNomination.NomineeId, webRepo.GetFullName(partyNomination.NomineeId), partyMemberType, partyNomination.PartyId.ToString(), partyName, voteNeeded, partyMemberType, totalVoters);

            return notificationparmText.ToString();
        }
        #endregion Nomination

        #region Political Party Join Request
        private void ProcessJoinPartyRequestResponse(VoteResponseDTO voteResponse, int userId)
        {
            IPartyDTORepository _repo = new PartyDTORepository();
            PartyJoinRequest joinRequest = _repo.GetPartyJoinRequest(voteResponse.TaskId.ToString());
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();


            PoliticalParty partyInfo = _repo.GetPartyById(joinRequest.PartyId.ToString());
            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;
            double voteNeeded = totalVoters * AppSettings.JoinPartyRequestApprovalVoteNeeded;

            IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

            int result = _repository.HasEnoughVoteForThisTask(AppSettings.JoinPartyRequestApprovalChoiceId, AppSettings.JoinPartyRequestDenialChoiceId,
                                        AppSettings.JoinPartyRequestApprovalVoteNeeded, totalVoters, taskVoteCount);


            if (result == AppSettings.JoinPartyRequestApprovalChoiceId)
            {
                if (joinRequest.Status == "P")
                {
                    _repo.SendApprovedMemeberPartyInvite(joinRequest);
                }
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else if (result == AppSettings.JoinPartyRequestDenialChoiceId)
            {
                IUserTaskDetailsDTORepository userTaskRepo = new UserTaskDetailsDTORepository();
                UserTask usertask = userTaskRepo.GetTaskById(voteResponse.TaskId, userId);
                StringBuilder parmtext = new StringBuilder();
                parmtext.AppendFormat("{0}|{1}", joinRequest.PartyId, _repo.GetpartyName(joinRequest.PartyId.ToString()));
                userNotif.AddNotification(false, string.Empty,
                        AppSettings.PartyJoinRequestInviteRejectNotificationId, parmtext.ToString(), 8, usertask.AssignerUserId);
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }


        }
        private void ProcessJoinPartyRequestInvitationResponse(VoteResponseDTO voteResponse, int userId)
        {
            IPartyDTORepository _repo = new PartyDTORepository();
            PartyInvite invite = _repo.GetPartyInviteById(voteResponse.TaskId.ToString());
            if (AppSettings.JoinPartyRequestInviteAcceptChoiceId == voteResponse.ChoiceRadioId)
            {
                if (invite.Status == "P")
                {
                    _repo.AddPartyMemberOnJoinRequest(invite, voteResponse.TaskId);
                }
            }
            else if (AppSettings.JoinPartyRequestInviteRejectChoiceId == voteResponse.ChoiceRadioId)
            {
                _repo.DecliedPartyMemberOnJoinRequest(invite, voteResponse.TaskId);

            }
        }
        #endregion  Political Party Join Request

        #region Party Invitation
        private void ProcessInvitationResponse(VoteResponseDTO voteResponse, int userId)
        {
            IPartyDTORepository _repo = new PartyDTORepository();
            PartyInvite invite = _repo.GetPartyInviteById(voteResponse.TaskId.ToString());
            if (AppSettings.PartyInviteAcceptChoiceId == voteResponse.ChoiceRadioId)
            {
                if (invite.Status == "P")
                {
                    _repo.AddPartyMemberOnJoinRequest(invite, voteResponse.TaskId, false);
                }
            }
            else if (AppSettings.PartyInviteRejectChoiceId == voteResponse.ChoiceRadioId)
            {
                _repo.DecliedPartyMemberOnJoinRequest(invite, voteResponse.TaskId, false);

            }
        }
        #endregion Party Invitation

        #region PartyEjection
        private void ProcessPartyEjectionResponse(VoteResponseDTO voteResponse, int userId)
        {
            IPartyDTORepository _repo = new PartyDTORepository();
            PartyEjection partyEjection = _repo.GetPartyEjection(voteResponse.TaskId.ToString());
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();

            PoliticalParty partyInfo = _repo.GetPartyById(partyEjection.PartyId.ToString());
            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;

            double voteNeeded;
            double approvalPercent;
            if (partyEjection.EjecteeMemberType == "F")
            {
                approvalPercent = AppSettings.PartyEjectionFounderApprovalVoteNeeded;
            }
            else if (partyEjection.EjecteeMemberType == "C")
            {
                approvalPercent = AppSettings.PartyEjectionCoFounderApprovalVoteNeeded;
            }
            else
            {
                approvalPercent = AppSettings.PartyEjectionMemberApprovalVoteNeeded;
            }

            voteNeeded = totalVoters * approvalPercent;

            IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

            int result = _repository.HasEnoughVoteForThisTask(AppSettings.PartyEjectionApprovalChoiceId, AppSettings.PartyEjectionDenialChoiceId,
                                        approvalPercent, totalVoters, taskVoteCount);

            if (result == AppSettings.PartyEjectionApprovalChoiceId)
            {
                if (partyEjection.Status != "A")
                {

                    string parmtext = PrepareEjectionResultNotficationMsg(taskVoteCount,
           partyEjection, "Approved");

                    _repo.ProcessEjectionApproval(partyEjection);


                    userNotif.AddNotification(false, string.Empty,
             AppSettings.PartyEjectResultNotificationId, parmtext, 8, partyEjection.InitatorId);
                    userNotif.AddNotification(false, string.Empty,
 AppSettings.PartyEjectResultNotificationId, parmtext, 8, partyEjection.EjecteeId);

                }

                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else if (result == AppSettings.PartyEjectionDenialChoiceId)
            {
                string parmtext = PrepareEjectionResultNotficationMsg(taskVoteCount,
               partyEjection, "Denied");
                _repo.ProcessEjectionDenial(partyEjection);

                userNotif.AddNotification(false, string.Empty,
                             AppSettings.PartyEjectResultNotificationId, parmtext, 8, partyEjection.InitatorId);
                userNotif.AddNotification(false, string.Empty,
             AppSettings.PartyEjectResultNotificationId, parmtext, 8, partyEjection.EjecteeId);
                _repository.DeleteRemaningTaskNotComplete(voteResponse);
            }
            else
            {
                string parmtext = PrepareEjectionVoteCountNotficationMsg(taskVoteCount,
                    Convert.ToInt32(Math.Ceiling(voteNeeded)), partyEjection, totalVoters);

                userNotif.AddNotification(false, string.Empty,
         AppSettings.PartyEjectVotingCountNotificationId, parmtext, 4, partyEjection.InitatorId);

                userNotif.AddNotification(false, string.Empty,
         AppSettings.PartyEjectVotingCountNotificationId, parmtext, 4, partyEjection.EjecteeId);

            }

        }
        private string PrepareEjectionResultNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, PartyEjection partyEjection, string result)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            string partyName = partyRepo.GetpartyName(partyEjection.PartyId.ToString());
            string ejecteefullName = webRepo.GetFullName(partyEjection.EjecteeId);

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyEjectionApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyEjectionDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
     partyEjection.EjecteeId, ejecteefullName, partyEjection.PartyId.ToString(), partyName, result, approvalVote, denialVote, partyEjection.RequestDate.ToString());


            return notificationparmText.ToString();
        }

        private string PrepareEjectionVoteCountNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, int voteNeeded, PartyEjection partyEjection, int totalVoters)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            string partyName = partyRepo.GetpartyName(partyEjection.PartyId.ToString());
            string ejecteefullName = webRepo.GetFullName(partyEjection.EjecteeId);


            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyEjectionApprovalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == AppSettings.PartyEjectionDenialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}",
                 approvalVote, denialVote,
      partyEjection.EjecteeId, ejecteefullName, partyEjection.PartyId.ToString(), partyName, voteNeeded, totalVoters);

            return notificationparmText.ToString();
        }
        #endregion PartyEjection

        #endregion Political Party

        #region Election
        #region RunForOffice
        private void ProcessRunForOfficeIndependentResponse(VoteResponseDTO voteResponse)
        {
            IElectionDTORepository _repo = new ElectionDTORepository();
            ElectionCandidate electionCandidate = _repo.GetElectionCandidate(voteResponse.TaskId);
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
            int totalVoters;
            double approvalPercent;
            if (electionCandidate.CandidateTypeId == "I")
            {
                totalVoters = taskRepo.GetTaskCountById(voteResponse.TaskId);
                approvalPercent = AppSettings.ElectionRunForOfficeIndividualApprovalVoteNeeded;



                IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

                int result = _repository.HasEnoughVoteForThisTask(AppSettings.RunForOfficeIndividualApprovalChoiceId, AppSettings.RunForOfficeIndividualDenialChoiceId,
                                            approvalPercent, totalVoters, taskVoteCount);

                if (result == AppSettings.RunForOfficeIndividualApprovalChoiceId)
                {
                    if (electionCandidate.Status != "A")
                    {

                        string parmtext = PrepareRunForOfficeResultNotficationMsg(taskVoteCount,
               electionCandidate, "Approved", AppSettings.RunForOfficeIndividualApprovalChoiceId, AppSettings.RunForOfficeIndividualDenialChoiceId);

                        _repo.ProcessApprovedRunforOffice(electionCandidate, parmtext);
                    }

                    _repository.DeleteRemaningTaskNotComplete(voteResponse);
                }
                else if (result == AppSettings.RunForOfficeIndividualDenialChoiceId)
                {
                    string parmtext = PrepareRunForOfficeResultNotficationMsg(taskVoteCount,
                   electionCandidate, "Denied", AppSettings.RunForOfficeIndividualApprovalChoiceId, AppSettings.RunForOfficeIndividualDenialChoiceId);
                    _repo.ProcessDeniedRunforOffice(electionCandidate, parmtext);

                    _repository.DeleteRemaningTaskNotComplete(voteResponse);
                }
                else
                {
                    double voteNeeded = approvalPercent;
                    string parmtext = PrepareRunForOfficeCountNotficationMsg(taskVoteCount,
                        Convert.ToInt32(Math.Ceiling(voteNeeded)), electionCandidate, totalVoters, AppSettings.RunForOfficeIndividualApprovalChoiceId, AppSettings.RunForOfficeIndividualDenialChoiceId);


                    userNotif.AddNotification(false, string.Empty,
             AppSettings.RunForOfficeVotingCountNotificationId, parmtext, 4, electionCandidate.UserId);

                }
            }

        }
        private void ProcessRunForOfficePartyResponse(VoteResponseDTO voteResponse)
        {
            IElectionDTORepository _repo = new ElectionDTORepository();
            IPartyDTORepository partyRepo = new PartyDTORepository();
            ElectionCandidate electionCandidate = _repo.GetElectionCandidate(voteResponse.TaskId);
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            int totalVoters;
            double approvalPercent;
            if (electionCandidate.CandidateTypeId == "P")
            {
                PoliticalParty partyInfo = partyRepo.GetPartyById(electionCandidate.PartyId.ToString());
                int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
                totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                    partyFounderSize * AppSettings.FounderVoteScore
                   ;
                approvalPercent = AppSettings.ElectionRunForOfficePartyApprovalVoteNeeded;

                IEnumerable<ChoiceCountDTO> taskVoteCount = _repository.GetVoteCountByChoiceForTask(voteResponse.TaskId);

                int result = _repository.HasEnoughVoteForThisTask(AppSettings.RunForOfficePartyApprovalChoiceId, AppSettings.RunForOfficePartyDenialChoiceId,
                                            approvalPercent, totalVoters, taskVoteCount);

                if (result == AppSettings.RunForOfficePartyApprovalChoiceId)
                {
                    if (electionCandidate.Status != "A")
                    {

                        string parmtext = PrepareRunForOfficeResultNotficationMsg(taskVoteCount,
               electionCandidate, "Approved", AppSettings.RunForOfficePartyApprovalChoiceId, AppSettings.RunForOfficePartyDenialChoiceId);

                        _repo.ProcessApprovedRunforOffice(electionCandidate, parmtext);

                    }

                    _repository.DeleteRemaningTaskNotComplete(voteResponse);
                }
                else if (result == AppSettings.RunForOfficePartyDenialChoiceId)
                {
                    string parmtext = PrepareRunForOfficeResultNotficationMsg(taskVoteCount,
                   electionCandidate, "Denied", AppSettings.RunForOfficePartyApprovalChoiceId, AppSettings.RunForOfficePartyDenialChoiceId);

                    _repo.ProcessDeniedRunforOffice(electionCandidate, parmtext);
                    _repository.DeleteRemaningTaskNotComplete(voteResponse);
                }
                else
                {
                    double voteNeeded = totalVoters * approvalPercent;
                    string parmtext = PrepareRunForOfficeCountNotficationMsg(taskVoteCount,
                        Convert.ToInt32(Math.Ceiling(voteNeeded)), electionCandidate, totalVoters, AppSettings.RunForOfficePartyApprovalChoiceId, AppSettings.RunForOfficePartyDenialChoiceId);


                    userNotif.AddNotification(false, string.Empty,
             AppSettings.RunForOfficeVotingCountNotificationId, parmtext, 4, electionCandidate.UserId);

                }
            }

        }
        private string PrepareRunForOfficeResultNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, ElectionCandidate electionCandidate, string result, int approvalChoiceId, int denialChoiceId)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IElectionDTORepository electionRepo = new ElectionDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();
            Election electionterm =
            electionRepo.GetCurrentElectionTerm(electionCandidate.CountryId);

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == approvalChoiceId)
              .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == denialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            notificationparmText.AppendFormat("<strong>Date:{0}</strong>|{1}|{2}|{3}|<strong>Date:{4}</strong>",
                electionterm.VotingStartDate,
result, approvalVote, denialVote, electionCandidate.RequestDate.ToString());


            return notificationparmText.ToString();
        }

        private string PrepareRunForOfficeCountNotficationMsg
(IEnumerable<ChoiceCountDTO> taskVoteCount, int voteNeeded, ElectionCandidate electionCandidate, int totalVoters, int approvalChoiceId, int denialChoiceId)
        {
            StringBuilder notificationparmText = new StringBuilder();
            IElectionDTORepository electionRepo = new ElectionDTORepository();
            ChoiceCountDTO defaultChoiceCount = new ChoiceCountDTO();

            Election electionterm =
   electionRepo.GetCurrentElectionTerm(electionCandidate.CountryId);

            Int64 approvalVote = taskVoteCount.Where(q => q.ChoiceId == approvalChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;
            Int64 denialVote = taskVoteCount.Where(q => q.ChoiceId == denialChoiceId)
                .DefaultIfEmpty(defaultChoiceCount).First().VoteCount;

            notificationparmText.AppendFormat("{0}|{1}|<strong>Date:{2}</strong>|{3}|{4}",
                 approvalVote, denialVote,
      electionterm.VotingStartDate,
     voteNeeded, totalVoters);

            return notificationparmText.ToString();
        }

        #endregion RunForOffice
        #endregion Election


    }
}
