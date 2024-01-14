using Common;
using DAO.Models;
using DTO.Db;
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
    public class ElectionManager
    {
        IPartyDTORepository partyRepo;
        IElectionDTORepository _repository;
        IWebUserDTORepository webRepo;

        private IUserNotificationDetailsDTORepository userNotif;
        public ElectionManager(IElectionDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
            partyRepo = new PartyDTORepository();
            webRepo = new WebUserDTORepository();
        }
        public ElectionManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            partyRepo = new PartyDTORepository();
            webRepo = new WebUserDTORepository();
            _repository = new ElectionDTORepository();
        }
        public void ProcessAppForOffice(RunForOfficeDTO runforOffice)
        {
            try
            {
                ICountryCodeRepository countryRepo = new CountryCodeRepository();
                IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
                runforOffice.CountryId = webRepo.GetCountryId(runforOffice.UserId);
                runforOffice.CurrentTerm = _repository.GetCurrentElectionTerm(runforOffice.CountryId);
                runforOffice.TotalPopulation = Convert.ToInt32(countryRepo.GetCountryPopulation(runforOffice.CountryId));

                runforOffice.HasPendingApplication = _repository.HasPendingOrApprovedApplication(runforOffice.UserId, runforOffice.CurrentTerm.ElectionId);


                runforOffice.TotalCash = bankRepo.GetUserBankDetails(runforOffice.UserId).Cash;
                runforOffice.ConsecutiveTerm = _repository.GetConsecutiveTerm(runforOffice.UserId, runforOffice.CurrentTerm.ElectionId);
                runforOffice.NumberOfApprovedCandidate = _repository.NumberOfApprovedCandidate(runforOffice.CurrentTerm.ElectionId, runforOffice.CountryId);
                if (runforOffice.CandidateTypeId == "P")
                {
                    runforOffice.PartyId = partyRepo.GetActivePartyId(runforOffice.UserId);
                    if (runforOffice.PartyId != Guid.Empty.ToString())
                    {

                        PoliticalParty partyInfo =
                        partyRepo.GetPartyById(runforOffice.PartyId);
                        runforOffice.PartySize = partyInfo.PartySize;
                        runforOffice.PartyStatus = partyInfo.Status;
                        runforOffice.NumberofApprovedPartyMembers = _repository.NumberofApprovedPartyMembers(runforOffice.CurrentTerm.ElectionId, runforOffice.PartyId);
                    }

                }


                ElectionRules electionRule =
                 new ElectionRules();
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = electionRule.IsValidElectionApplication(runforOffice);
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.ApplyForElection(runforOffice);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("<strong>Date:{0}</strong>|{1}",
                            runforOffice.CurrentTerm.VotingStartDate, AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.RunForOfficeFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("<strong>Date:{0}</strong>", runforOffice.CurrentTerm.VotingStartDate); notificationTypeId = AppSettings.RunForOfficeSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("<strong>Date:{0}</strong>|{1}",
                         runforOffice.CurrentTerm.VotingStartDate, validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.RunForOfficeFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, runforOffice.UserId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessAppForOffice");
            }
        }
        public void ProcessElectionDonation(PayWithTaxDTO donation)
        {
            try
            {
                String parmText = "";
                short notificationTypeId = 0;
                sbyte priority = 0;


                donation.CountryId = webRepo.GetCountryId(donation.UserId);
                int electionId = _repository.GetCurrentElectionTerm(donation.CountryId).ElectionId;
                ElectionRules electionRule =
                     new ElectionRules();
                ElectionCandidate candidate = _repository.GetElectionCandidate(donation.UserId, electionId, donation.CountryId);

                ValidationResult validationResult = electionRule.IsValidElectionDonation(candidate);
                donation.UserFullName = webRepo.GetFullName(donation.UserId);
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.ExecuteDonateElection(donation);
                    if (!result)
                    {
                        //Add a notification to resubmit 

                        parmText = string.Format("{0}|{1}|{2}|{3}",
                         donation.Amount,
                         donation.SourceId,
                          donation.UserFullName, AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.ElectionDonationFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}|{2}",
                                    donation.Amount,
                                    donation.SourceId,
                                     donation.UserFullName);
                        notificationTypeId = AppSettings.ElectionDonationSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}|{2}",
                          donation.Amount,
                          donation.SourceId,
                           donation.UserFullName,
                          validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.ElectionDonationFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, donation.UserId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessElectionDonation");
            }

        }

        public void ProcessQuitElection(QuitElectionDTO quit)
        {
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                quit.CountryId = webRepo.GetCountryId(quit.UserId);
                int electionId = _repository.GetCurrentElectionTerm(quit.CountryId).ElectionId;

                StringBuilder parmText = new StringBuilder();
                short notificationTypeId = 0;
                quit.CandidateInfo = _repository.GetElectionCandidate(quit.UserId, electionId, quit.CountryId);
                ElectionRules electionRule =
                    new ElectionRules();
                sbyte priority = 0;
                ValidationResult validationResult = electionRule.IsValidElectionDonation(quit.CandidateInfo);
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.QuitElection(quit);
                    if (!result)
                    {
                        //Add a notification to resubmit 

                        parmText.AppendFormat("{0}",
                          AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.ElectionQuitFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        notificationTypeId = AppSettings.ElectionQuitSuccessNotificationId;
                    }
                }
                else
                {
                    parmText.AppendFormat("{0}",
                          validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.ElectionQuitFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, quit.UserId);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessQuit");
            }
        }
        public void ProcessVoteElection(CandidateVotingDTO voting)
        {
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                voting.CountryId = webRepo.GetCountryId(voting.UserId);
                voting.ElectionInfo = _repository.GetCurrentElectionTerm(voting.CountryId);


                StringBuilder parmText = new StringBuilder();
                short notificationTypeId = 0;
                voting.UserCountryId = _repository.GetElectionCandidateCountry(voting);
                voting.HasVotedThisElection = _repository.HasVotedThisElection(voting);
                ElectionRules electionRule =
                    new ElectionRules();
                sbyte priority = 0;
                ValidationResult validationResult = electionRule.IsValidElectionVoting(voting);
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.SaveVoting(voting);
                    if (!result)
                    {
                        //Add a notification to resubmit 

                        parmText.AppendFormat("{0}",
                          AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.ElectionVotingFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        notificationTypeId = AppSettings.ElectionVotingSuccessNotificationId;
                    }
                }
                else
                {
                    parmText.AppendFormat("{0}",
                          validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.ElectionVotingFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, voting.UserId);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessVoteElection");
            }
        }
    }
}


