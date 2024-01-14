using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ElectionDTORepository : IElectionDTORepository
    {

        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        private IUserNotificationDetailsDTORepository userNotif;
        private IPartyDTORepository partyRepo;
        public ElectionDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
            userNotif = new UserNotificationDetailsDTORepository();
            partyRepo = new PartyDTORepository();
        }

        public ElectionDTORepository(IRedisCacheProvider cacheProvider)
        {
            userNotif = new UserNotificationDetailsDTORepository();
            this.cache = cacheProvider;
        }
        public IEnumerable<Election> GetLastNoVoteCountedElectionPeriod()
        {
            return
            spContext.GetSqlDataNoParms<Election>((AppSettings.SPGetLastNoVoteCountedElectionPeriod));
        }
        public Election GetCurrentElectionTerm(string countyrId)
        {
            return JsonConvert.DeserializeObject<Election>(
            GetCurrentElectionTermJson(countyrId));
        }
        public string GetCurrentElectionTermJson(string countyrId)
        {
            countyrId = countyrId.ToLower();
            string electiontermData = cache.GetStringKey(AppSettings.RedisKeyElectionTerm + countyrId);
            if (electiontermData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countyrId);
                electiontermData = JsonConvert.SerializeObject(
                   spContext.GetSqlDataSignleRow<Election>((AppSettings.SPGetCurrentElectionPeriod), dictionary));
                cache.SetStringKey(AppSettings.RedisKeyElectionTerm + countyrId, electiontermData);
            }
            return (electiontermData);
        }
        public string GetPoliticalPostionsJson()
        {
            string politicalPositionData = cache.GetStringKey(AppSettings.RedisKeyPoliticalPositions);
            if (politicalPositionData == null)
            {
                politicalPositionData = JsonConvert.SerializeObject
                    (spContext.GetSqlDataNoParms<ElectionPosition>(AppSettings.SPPoliticalPostionList));
                cache.SetStringKey(AppSettings.RedisKeyPoliticalPositions, politicalPositionData);
            }
            return (politicalPositionData);
        }
        public void UpdateElection(Election election)
        {
            spContext.Update(election);
        }
        public void AddElection(Election election)
        {
            spContext.Add(election);
        }

        #region Election Application
        public ElectionCandidate GetElectionCandidate(Guid taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            return spContext.GetByPrimaryKey<ElectionCandidate>(dictionary);
        }
        public bool HasPendingOrApprovedApplication(int userId, int electionId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmElectionId", electionId);
            dictionary.Add("parmUserId", userId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPGetPendingOrApprovedElectionApp, dictionary, "cnt"));
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public int NumberofApprovedPartyMembers(int electionId, string partyId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmElectionId", electionId);
            dictionary.Add("parmPartyId", partyId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPNumberofApprovedPartyMember, dictionary, "cnt"));
            return count;
        }
        public int GetConsecutiveTerm(int userId, int electionId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmElectionId", electionId);
            dictionary.Add("parmUserId", userId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPGetConsecutiveTerm, dictionary, "cnt"));
            return count;
        }
        public int NumberOfApprovedCandidate(int electionId, string countryId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmElectionId", electionId);
            dictionary.Add("parmCountryId", countryId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPNumberOfApprovedCandidate, dictionary, "cnt"));
            return count;
        }

        public bool ApplyForElection(RunForOfficeDTO runforOffice)
        {
            try
            {
                runforOffice.TaskId = Guid.NewGuid();
                if (AddElectionFund(runforOffice))
                {

                    AddElectionCandidate(runforOffice);
                    CandidateAgenda agenda = new CandidateAgenda
                    {
                        ElectionId = runforOffice.CurrentTerm.ElectionId,
                        UserId = runforOffice.UserId
                    };
                    AddElectionAgenda(agenda, runforOffice.Agendas);
                    IWebUserDTORepository webRepo = new WebUserDTORepository();
                    WebUserDTO webinfo = webRepo.GetUserPicFName(runforOffice.UserId);
                    runforOffice.Picture = webinfo.Picture;
                    runforOffice.FullName = webinfo.FullName;

                    SendTaskandNotificationElection(runforOffice);
                    AddElectionPost(runforOffice);
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to ApplyForElection");
                return false;
            }

        }

        private bool AddElectionFund(RunForOfficeDTO runforOffice)
        {
            try
            {
                ICountryCodeRepository countryRepo = new CountryCodeRepository();
                ICountryTaxDetailsDTORepository countrytaxRepo = new CountryTaxDetailsDTORepository();

                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                decimal totaltax = runforOffice.CurrentTerm.Fee * countrytaxRepo.GetCountryTaxByCode(runforOffice.CountryId, AppSettings.TaxElectionCode) / 100;


                dictionary.Add("parmUserId", runforOffice.UserId);
                dictionary.Add("parmCountryUserId", countryRepo.GetCountryCode(runforOffice.CountryId).CountryUserId);
                dictionary.Add("parmCountryId", runforOffice.CountryId);
                dictionary.Add("parmTaskId", runforOffice.TaskId);
                dictionary.Add("parmFundType", AppSettings.ElectionFeeFundType);
                dictionary.Add("parmTaxCode", AppSettings.TaxElectionCode);
                dictionary.Add("parmAmount", runforOffice.CurrentTerm.Fee);
                dictionary.Add("parmTaxAmount", totaltax);

                int response = (int)spContext.GetSqlDataSignleValue
              (AppSettings.SPExecutePayNation, dictionary, "result");
                if (response != 1)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to AddElectionFund");
                return false;
            }

        }

        private void AddElectionCandidate(RunForOfficeDTO runforOffice)
        {
            ElectionCandidate candidate = new ElectionCandidate
            {
                CandidateTypeId = runforOffice.CandidateTypeId,
                ElectionId = runforOffice.CurrentTerm.ElectionId,
                PositionTypeId = runforOffice.PositionTypeId,
                Status = "P",
                UserId = runforOffice.UserId,
                TaskId = runforOffice.TaskId,
                RequestDate = DateTime.UtcNow,
                CountryId = runforOffice.CountryId,
                LogoPictureId = runforOffice.CandidateTypeId == "I" ? runforOffice.LogoPictureId : string.Empty
            };

            if (string.IsNullOrEmpty(runforOffice.LogoPictureId) || runforOffice.LogoPictureId.IndexOf(".") == -1)
            {
                candidate.LogoPictureId = string.Empty;
            }

            if (runforOffice.CandidateTypeId == "P")
            {
                candidate.PartyId = new Guid(runforOffice.PartyId);
            }
            spContext.Add(candidate);
        }

        private void AddElectionAgenda(CandidateAgenda agenda, short[] agendas)
        {
            foreach (var item in agendas)
            {
                agenda.AgendaTypeId = item;
                spContext.Add(agenda);
            }

        }

        private UserTask GetTask(Guid taskId, int requestoruserId, DateTime trnDate, short defaultResponse,
DateTime dueDate, StringBuilder parm, int userId, short taskType)
        {
            UserTask startpartyTask = new UserTask
            {
                TaskId = taskId,
                AssignerUserId = requestoruserId,
                CompletionPercent = 0,
                CreatedAt = trnDate,
                DefaultResponse = defaultResponse,
                DueDate = dueDate,
                Flagged = false,
                Priority = 11,
                Parms = parm.ToString(),
                Status = "I",
                TaskTypeId = taskType,
                UserId = userId
            };
            return startpartyTask;
        }
        private void SendTaskandNotificationElection(RunForOfficeDTO runforOffice)
        {

            IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
            StringBuilder notificationparmText = new StringBuilder();
            StringBuilder taskParmtext = new StringBuilder();
            DateTime trnDate = DateTime.UtcNow;
            DateTime dueDate = trnDate.AddHours(72);
            TaskReminder reminderTask = taskRepo.GetTaskReminder(dueDate, runforOffice.TaskId);
            taskRepo.SaveReminder(reminderTask);
            string defaultResponse = "Approved";


            if (runforOffice.CandidateTypeId == "P")
            {
                runforOffice.PartyName = partyRepo.GetpartyName(runforOffice.PartyId);
                notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
runforOffice.UserId, runforOffice.FullName,
                         runforOffice.PartyId, runforOffice.PartyName, runforOffice.TaskId
                         );
                taskParmtext.AppendFormat("{0}|{1}|{2}|{3}|{4}|<strong>Date:{5}</strong>|{6}",
runforOffice.UserId, runforOffice.FullName,
                runforOffice.PartyId, runforOffice.PartyName, runforOffice.TaskId, dueDate, defaultResponse);

                int[] allmembers = Array.ConvertAll(partyRepo.GetAllPartyMember(runforOffice.PartyId), int.Parse);
                foreach (var item in allmembers)
                {
                    if (item == runforOffice.UserId)
                    {
                        continue;
                    }
                    UserTask userTask = GetTask(runforOffice.TaskId, runforOffice.UserId, trnDate, (short)AppSettings.RunForOfficePartyApprovalChoiceId, dueDate, taskParmtext, (item),
                                AppSettings.RunForOfficeAsPartyTaskType);
                    taskRepo.SaveTask(userTask);
                    userNotif.AddNotification(true, runforOffice.TaskId.ToString(),
                   AppSettings.RunForOfficePartyVotingRequestNotificationId,
                   notificationparmText.ToString(), 7, (item));
                }
            }
            else
            {
                notificationparmText.AppendFormat("{0}|{1}|{2}",
                              runforOffice.UserId, runforOffice.FullName,
                             runforOffice.TaskId
                              );

                taskParmtext.AppendFormat("{0}|{1}|{2}|<strong>Date:{3}</strong>|{4}",
   runforOffice.UserId, runforOffice.FullName,
                             runforOffice.TaskId, dueDate, defaultResponse);
                foreach (var item in runforOffice.FriendSelected)
                {
                    UserTask userTask = GetTask(runforOffice.TaskId, runforOffice.UserId, trnDate, (short)AppSettings.RunForOfficeIndividualApprovalChoiceId, dueDate, taskParmtext, Convert.ToInt32(item),
            AppSettings.RunForOfficeAsIndividualTaskType);
                    taskRepo.SaveTask(userTask);
                    userNotif.AddNotification(true, runforOffice.TaskId.ToString(),
                   AppSettings.RunForOfficeIndividualVotingRequestNotificationId,
                   notificationparmText.ToString(), 7, Convert.ToInt32(item));
                }
            }
        }

        private void AddElectionPost(RunForOfficeDTO runforOffice)
        {
            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            StringBuilder postParms = new StringBuilder();
            Post post = new Post();
            if (runforOffice.CandidateTypeId == "P")
            {
                postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                    runforOffice.UserId,
                    runforOffice.Picture
                    , runforOffice.FullName
                    , runforOffice.PartyId, runforOffice.PartyName,
                    runforOffice.TaskId);
                post.PostContentTypeId = AppSettings.RunForOfficePartyPostContentTypeId;
                post.PartyId = new Guid(runforOffice.PartyId);

            }
            else
            {
                postParms.AppendFormat("{0}|{1}|{2}|{3}",
                         runforOffice.UserId,
                        runforOffice.Picture
                        , runforOffice.FullName
                        , runforOffice.TaskId);
                post.PostContentTypeId = AppSettings.RunForOfficeIndividualPostContentTypeId;
                post.UserId = runforOffice.UserId;
            }
            post.Parms = postParms.ToString();
            post.PostId = runforOffice.TaskId;
            postRepo.SavePost(post);
        }

        public void ProcessDeniedRunforOffice(ElectionCandidate candidate, string parmtext)
        {
            UpdateCandidateStatus(candidate.TaskId, "D");
            userNotif.AddNotification(false, string.Empty,
         AppSettings.RunForOfficeResultNotificationId, parmtext, 8, candidate.UserId);
        }
        public void ProcessApprovedRunforOffice(ElectionCandidate candidate, string parmtext)
        {
            try
            {
                ValidationResult validationResult = CheckIfStillValid(candidate);
                if (validationResult != ValidationResult.Success)
                {
                    UpdateCandidateStatus(candidate.TaskId, "D");
                    AddEelectionCandidateNoLogerValidNotifcation(candidate, validationResult);
                    DeleteElectionTask(candidate.ElectionId);
                    AddElectionCandidateApplicationResultPost(candidate, "Denied");
                }
                else
                {
                    UpdateCandidateStatus(candidate.TaskId, "A");
                    AddElectionVotingEntry(candidate);
                    AddElectionCandidateApplicationResultPost(candidate, "Approved");
                    userNotif.AddNotification(false, string.Empty,
                 AppSettings.RunForOfficeResultNotificationId, parmtext, 8, candidate.UserId);
                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to UpdateCandidateStatus");
            }
        }

        private void AddEelectionCandidateNoLogerValidNotifcation(ElectionCandidate candidate, ValidationResult validationResult)
        {

            Election electionTerm = GetCurrentElectionTerm(candidate.CountryId);
            string parmText = string.Format("<strong>Date:{0}</strong>|{1}",
               electionTerm.VotingStartDate, validationResult.ErrorMessage);
            userNotif.AddNotification(false, string.Empty,
      AppSettings.RunForOfficeFailNotificationId, parmText.ToString(), 9, candidate.UserId);

        }

        private ValidationResult CheckIfStillValid(ElectionCandidate candidate)
        {
            int approvedusers = NumberOfApprovedCandidate(candidate.ElectionId, candidate.CountryId);
            if (approvedusers > RulesSettings.ElectionCandidateNumberHardCap)
            {
                return new ValidationResult(
             string.Format("{0} members have already been approved to run in this election term, which is the curret elction Cap", RulesSettings.ElectionCandidateNumberHardCap));
            }
            ICountryCodeRepository countryRepo = new CountryCodeRepository();
            int totalPopulation = countryRepo.GetCountryPopulation(candidate.CountryId);
            if (approvedusers > RulesSettings.ElectionCapPercent * totalPopulation)
            {
                return new ValidationResult(
                 string.Format("{0}% of total population has already been approved to run in this election term", RulesSettings.ElectionPartyCapPercent * 100));
            }


            if (candidate.CandidateTypeId == "P")
            {
                PoliticalParty partyInfo = partyRepo.GetPartyById(candidate.PartyId.ToString());
                int approvedpartyMember = NumberofApprovedPartyMembers(candidate.ElectionId, candidate.PartyId.ToString());

                if (approvedpartyMember > RulesSettings.ElectionPartyCapPercent * partyInfo.PartySize)
                {
                    return new ValidationResult(
          string.Format("you already have {0}% of your Party Members approved to run in this election term ", RulesSettings.ElectionPartyCapPercent * 100));
                }

                if (partyInfo.Status != "A")
                {
                    return new ValidationResult(("your Party is not in approved status"));
                }
            }
            return ValidationResult.Success;

        }

        private void DeleteElectionTask(int elctionId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmElectionId", elctionId);
            spContext.ExecuteStoredProcedure(AppSettings.SPDeleteTaskElectionCap, dictionary);
        }
        private void ProcessDeniedRunforOffice(ElectionCandidate candidate)
        {
            try
            {
                UpdateCandidateStatus(candidate.TaskId, "D");
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to UpdateCandidateStatus");
            }
        }
        private void UpdateCandidateStatus(Guid taskId, string status)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            dictionary.Add("parmStatus", status);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateCandidateStatus, dictionary);

        }

        private void AddElectionCandidateApplicationResultPost(ElectionCandidate candidate, string result)
        {

            IPostCommentDTORepository postRepo = new PostCommentDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            WebUserDTO webuser = webRepo.GetUserPicFName(candidate.UserId);
            StringBuilder postParms = new StringBuilder();
            Post post = new Post();
            if (candidate.CandidateTypeId == "P")
            {
                string partyName = partyRepo.GetpartyName(candidate.PartyId.ToString());
                postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                     candidate.UserId,
                    webuser.Picture
                    , webuser.FullName
                    , result
                    , candidate.PartyId, partyName,
                    candidate.TaskId);
                post.PostContentTypeId = AppSettings.RunForOfficeApplicationResultPartyPostContentTypeId;
                post.PartyId = candidate.PartyId;

            }
            else
            {
                postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                         candidate.UserId,
                        webuser.Picture
                        , webuser.FullName
                        , result
                        , candidate.TaskId);
                post.PostContentTypeId = AppSettings.RunForOfficeApplicationResultIndividualPostContentTypeId;
                post.UserId = candidate.UserId;
            }
            post.Parms = postParms.ToString();
            postRepo.SavePost(post);
        }

        private void AddElectionVotingEntry(ElectionCandidate candidate)
        {
            ElectionVoting voting = new ElectionVoting
            {
                CountryId = candidate.CountryId,
                ElectionId = candidate.ElectionId,
                Score = 0,
                UserId = candidate.UserId,
                ElectionResult = string.Empty

            };
            spContext.Add(voting);
        }

        #endregion Election Application

        #region Election Ticket
        public RunForOfficeTicketDTO GetRunForOfficeTicket(string taskId, int userId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskId", taskId);

                RunForOfficeTicketDTO result = spContext.GetSqlDataSignleRow<RunForOfficeTicketDTO>((AppSettings.SPGetRunForOfficeTicket), dictionary);
                if (result.CandidateTypeId == "P")
                {
                    result.PartyName = partyRepo.GetpartyName(result.PartyId.ToString());
                    result.LogoPictureId = partyRepo.GetlogoPictureId(result.PartyId.ToString());
                }
                result.CandidateAgendaId = GetCandidateAgenda(userId, result.ElectionId);


                return result;

            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Failed trying to GetRunForOfficeTicket"); return new RunForOfficeTicketDTO();
            }
        }

        public int[] GetCandidateAgenda(int userId, int electionId)
        {
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                dictionary.Add("parmElectionId", electionId);
                int[] agendas =
                    spContext.GetSqlData((AppSettings.SPGetCandidateAgenda), dictionary).ToArray();
                return agendas;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to GetCandidateAgenda");
                return new int[] { };
            }
        }
        #endregion Election Ticket

        #region ElectionCandidate
        public string GetElectionLast12Json(string countyrId)
        {
            string electionlast12Data = cache.GetStringKey(AppSettings.RedisKeyElectionLast12 + countyrId);
            if (electionlast12Data == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countyrId);
                electionlast12Data = JsonConvert.SerializeObject(
                   spContext.GetSqlData((AppSettings.SPGetElectionLast12), dictionary));
                cache.SetStringKey(AppSettings.RedisKeyElectionLast12 + countyrId, electionlast12Data);
            }
            return (electionlast12Data);
        }
        public string GetCandidateByElection(CandidateSearchDTO candidateSearch)
        {
            string redisKey = AppSettings.RedisKeyElectionCandidateSearch + candidateSearch.CountryId + candidateSearch.ElectionId + candidateSearch.LastPage;
            string electioncandidateData = cache.GetStringKey(redisKey);

            if (electioncandidateData == null)
            {
                Election currentElection = GetCurrentElectionTerm(candidateSearch.CountryId);
                if (candidateSearch.ElectionId == 0)
                {
                    candidateSearch.ElectionId = currentElection.ElectionId;
                }

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", candidateSearch.CountryId);
                dictionary.Add("parmElectionId", candidateSearch.ElectionId);
                dictionary.Add("parmLimitOffset", candidateSearch.LastPage);
                dictionary.Add("parmLimit", AppSettings.ElectionCandidateLimit);


                IEnumerable<ElectionCandidateDTO> electioncandidates =
                       spContext.GetSqlData<ElectionCandidateDTO>((AppSettings.SPGetCandidateByElection), dictionary);
                foreach (var item in electioncandidates)
                {
                    if (item.CandidateTypeId == "P")
                    {
                        item.PartyName = partyRepo.GetpartyName(item.PartyId.ToString());
                    }

                }
                electioncandidateData = JsonConvert.SerializeObject(electioncandidates);
                int cacheLimit = 0;
                if (candidateSearch.ElectionId == currentElection.ElectionId)
                {
                    cacheLimit = AppSettings.CurrentElectionResultCacheLimit;
                }
                cache.SetStringKey(redisKey, electioncandidateData, cacheLimit);
            }
            return (electioncandidateData);
        }

        #region Voting Candidate
        public string GetCurrentVotingInfo(CandidateVotingDTO candidateVoting)
        {
            string redisKey = AppSettings.RedisKeyElectionVoteCandidate + candidateVoting.CountryId;

            string electionvotingData = cache.GetStringKey(redisKey);


            if (electionvotingData == null)
            {
                Election currentElection = GetCurrentElectionTerm(candidateVoting.CountryId);
                if (!(DateTime.UtcNow >= currentElection.VotingStartDate && DateTime.UtcNow <= currentElection.EndDate))
                {
                    return
                        JsonConvert.SerializeObject(new string[] { });
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", candidateVoting.CountryId);
                dictionary.Add("parmElectionId", currentElection.ElectionId);
                dictionary.Add("parmPositionTypeId", candidateVoting.PositionTypeId);


                IEnumerable<VotingCandidateDTO> electioncandidates =
                       spContext.GetSqlData<VotingCandidateDTO>(AppSettings.SPGetCurrentVotingInfo, dictionary);
                foreach (var item in electioncandidates)
                {
                    if (item.CandidateTypeId == "P")
                    {
                        item.PartyName = partyRepo.GetpartyName(item.PartyId.ToString());
                    }
                }
                electionvotingData = JsonConvert.SerializeObject(electioncandidates);
                cache.SetStringKey(redisKey, electionvotingData, AppSettings.CurrentElectionResultCacheLimit);
            }
            return (electionvotingData);
        }
        #endregion Voting Candidate

        #region Election Donation
        public bool ExecuteDonateElection(PayWithTaxDTO donation)
        {
            try
            {
                ICountryTaxDetailsDTORepository countrytaxRepo = new CountryTaxDetailsDTORepository();

                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                decimal totaltax = donation.Amount * countrytaxRepo.GetCountryTaxByCode(donation.CountryId, AppSettings.TaxDonationCode) / 100;



                dictionary.Add("parmCountryId", donation.CountryId);
                dictionary.Add("parmUserId", donation.UserId);
                dictionary.Add("parmTaskId", Guid.NewGuid());
                dictionary.Add("parmSourceId", donation.SourceId);
                dictionary.Add("parmFundType", AppSettings.ElectionDonationFundType);
                dictionary.Add("parmTaxCode", AppSettings.TaxDonationCode);
                dictionary.Add("parmAmount", donation.Amount);
                dictionary.Add("parmTaxAmount", totaltax);

                int response = (int)spContext.GetSqlDataSignleValue
              (AppSettings.SPExecutePayWithTax, dictionary, "result");

                if (response != 1)
                {
                    return false;
                }
                AddElectionDonationPost(donation);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SPEjectPartyMember");
                return false;
            }
        }

        public void AddElectionDonationPost(PayWithTaxDTO donation)
        {
            try
            {
                IPostCommentDTORepository postRepo = new PostCommentDTORepository();
                StringBuilder postParms = new StringBuilder();
                Post post = new Post();
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                WebUserDTO sourceinfo = webRepo.GetUserPicFName(donation.SourceId);
                WebUserDTO userinfo = webRepo.GetUserPicFName(donation.UserId);

                postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                        donation.SourceId,
                        sourceinfo.Picture,
                        sourceinfo.FullName,
                         donation.Amount,
                        donation.UserId,
                        userinfo.Picture,
                        userinfo.FullName
                       );
                post.PostContentTypeId = AppSettings.ElectionDonationContentTypeId;
                post.UserId = donation.UserId;
                post.Parms = postParms.ToString();
                post.PostId = Guid.NewGuid();
                postRepo.SavePost(post);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to AddElectionDonationPost");
            }
        }

        #endregion Election Donation

        #region QuitElection
        public bool QuitElection(QuitElectionDTO quit)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("parmElectionId", quit.CandidateInfo.ElectionId);
                dictionary.Add("parmUserId", quit.UserId);
                spContext.ExecuteStoredProcedure(AppSettings.SPQuitElection, dictionary);
                AddQuitElectionPost(quit);

                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to QuitElection");
                return false;
            }
        }

        public void AddQuitElectionPost(QuitElectionDTO quit)
        {
            try
            {
                IPostCommentDTORepository postRepo = new PostCommentDTORepository();
                StringBuilder postParms = new StringBuilder();
                Post post = new Post();
                IWebUserDTORepository webRepo = new WebUserDTORepository();
                WebUserDTO userinfo = webRepo.GetUserPicFName(quit.UserId);

                postParms.AppendFormat("{0}|{1}|{2}|{3}",
                        quit.UserId,
                        userinfo.Picture,
                        userinfo.FullName,
                        quit.CandidateInfo.TaskId
                       );
                post.PostContentTypeId = AppSettings.ElectionWithdrawContentTypeId;
                post.UserId = userinfo.UserId;
                post.Parms = postParms.ToString();
                post.PostId = Guid.NewGuid();
                postRepo.SavePost(post);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to AddElectionDonationPost");
            }
        }

        #endregion QuitElection
        #region ElectionVoting
        public bool SaveVoting(CandidateVotingDTO votes)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (var item in votes.Candidates)
                {
                    dictionary.Add("parmUserId", item);
                    dictionary.Add("parmCountryId", votes.CountryId);
                    dictionary.Add("parmPoints", 1);
                    dictionary.Add("parmElectionId", votes.ElectionInfo.ElectionId);
                    spContext.ExecuteStoredProcedure(AppSettings.SPExecuteUpdateVotingScore, dictionary);
                    dictionary.Clear();
                }
                ElectionVoter elevoter = new ElectionVoter
                {
                    UserId = votes.UserId,
                    ElectionId = votes.ElectionInfo.ElectionId
                };

                spContext.Add(elevoter);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveVoting");
                return false;
            }
        }

        public bool HasVotedThisElection(CandidateVotingDTO voting)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmElectionId", voting.ElectionInfo.ElectionId);
            dictionary.Add("parmUserId", voting.UserId);
            return spContext.GetByPrimaryKey<ElectionVoter>(dictionary).UserId == voting.UserId;
        }

        public string GetElectionCandidateCountry(CandidateVotingDTO candidates)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserIdList", string.Join(",", candidates.Candidates));
            dictionary.Add("parmElectionId", candidates.ElectionInfo.ElectionId);
            try
            {
                string result
                    = spContext.GetSqlDataSignleValue(
                AppSettings.SPGetElectionCandidateCountry,
                dictionary, "CountryId").ToString();
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion ElectionVoting

        public IEnumerable<ElectionVotingDTO> GetElectionResult(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            IEnumerable<ElectionVotingDTO> resultlist
                = spContext.GetSqlData<ElectionVotingDTO>(AppSettings.SPGetVoteResultByElection, dictionary);
            return resultlist;
        }

        public ElectionCandidate GetElectionCandidate(int userId, int electionId, string countryId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmElectionId", electionId);
            dictionary.Add("parmCountryId", countryId);
            ElectionCandidate result
                = spContext.GetSqlDataSignleRow<ElectionCandidate>(AppSettings.SPGetElectionCandidate, dictionary);
            return result;
        }

        #endregion ElectionCandidate
        #region Election Voting
        public void AddNextElectionTerm()
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmNumberOfDaysofElection", RulesSettings.NumberOfDaysofElection);
            dictionary.Add("parmNumberOfDaysToElection", RulesSettings.NumberOfDaysToElection);
            spContext.ExecuteStoredProcedure(AppSettings.SPAddNextElectionTerm, dictionary);
        }

        public void NotifyStartOfElectionPeroid()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPostContentTypeId", AppSettings.NotifyStartOfElectionPeroidContentTypeId);
            spContext.ExecuteStoredProcedure(AppSettings.SPNotifyStartOfElectionPeroid, dictionary);
        }
        public void NotifyStartOfVotingPeroid()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPostContentTypeId", AppSettings.NotifyStartOfVotingPeroidContentTypeId);
            spContext.ExecuteStoredProcedure(AppSettings.SPNotifyStartOfVotingPeroid, dictionary);
        }
        public void NotifyLastDayOfVotingPeroid()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPostContentTypeId", AppSettings.NotifyLastDayOfVotingPeroidContentTypeId);
            spContext.ExecuteStoredProcedure(AppSettings.SPNotifyLastDayOfVotingPeroid, dictionary);
        }
        public bool ElectionVoteCounting(VoteCountingDTO votignDTO)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", votignDTO.CountryCode.CountryId);
            dictionary.Add("parmCountryName", votignDTO.CountryCode.Code);
            dictionary.Add("parmLimit", votignDTO.LeaderLimit);
            dictionary.Add("parmPostContentTypeId", AppSettings.ElectionVictoryContentTypeId);
            dictionary.Add("parmWinNotificationTypeId", AppSettings.ElectionVotingWinNotificationId);
            dictionary.Add("parmLossNotificationTypeId", AppSettings.ElectionVotingLostNotificationId);
            dictionary.Add("parmPriority", votignDTO.Priority);
            dictionary.Add("parmElectionId", votignDTO.ElectionId);
            int response = (int)spContext.GetSqlDataSignleValue(AppSettings.SPExecuteElectionVoteCounting, dictionary, "result");
            return response == 1 ? true : false;
        }

        public int GetElectionCandiate(string countryId, int electionId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", countryId);
            dictionary.Add("parmElectionId", electionId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPGetElectionCandidateCount
                , dictionary, "cnt"));
            return count;
        }

        #endregion Election Voting
        #region invalidate cache
        public void ClearElectionTermCache(string countyrId)
        {
            cache.Invalidate(AppSettings.RedisKeyElectionTerm + countyrId);
        }
        public void ClearAllElectionTermCache()
        {
            string rediskey = AppSettings.RedisKeyElectionTerm + "*";
            cache.Invalidate(cache.FindKeys(rediskey));
        }
        #endregion invalidate cache
    }
}
