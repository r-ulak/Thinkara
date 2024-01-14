using Common;
using DAO.Models;
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
    public class RobberyManager
    {
        RobberyDTORepository _repository;
        IWebUserDTORepository webRepo;
        IMerchandiseDetailsDTORepository merRepo;
        IFriendDetailsDTORepository friendRepo;
        private IUserNotificationDetailsDTORepository userNotif;
        public RobberyManager(RobberyDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            merRepo = new MerchandiseDetailsDTORepository();
            friendRepo = new FriendDetailsDTORepository();
        }
        public RobberyManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            merRepo = new MerchandiseDetailsDTORepository();
        }
        public void ProcessRobberyProperty(CrimeIncidentDTO incident)
        {
            try
            {
                incident.IsFriend = friendRepo.IsFriend(incident.UserId, incident.VictimId);
                incident.RobbedRecently = _repository.NotRobbedInLastNDayByUser(incident);
                RobberyRules robberyRules = new RobberyRules();
                StringBuilder parmText = new StringBuilder();
                sbyte priority = 0;
                short notificationTypeId = 0;
                MerchandiseType merchandiseType = merRepo.GetMerchandiseType(incident.MerchandiseTypeId);

                WebUserDTO webUser = webRepo.GetUserPicFName(incident.VictimId);

                ValidationResult validationResult = robberyRules.IsValidRobbery(incident);
                parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
         merchandiseType.ImageFont, merchandiseType.Name,
         incident.VictimId,
         webUser.Picture, webUser.FullName);
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.ExecuteStealProperty(incident);

                    if (!result)
                    {
                        parmText.AppendFormat("|{0}",
                             AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.PropertyRobberyFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        notificationTypeId = AppSettings.PropertyRobberySuccessNotificationId;
                        priority = 2;
                        AddNotificationRobbery(incident, merchandiseType);
                    }
                }
                else
                {
                    parmText.AppendFormat("|{0}",
                              validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.PropertyRobberyFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
notificationTypeId, parmText.ToString(), priority, incident.UserId);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to ProcessPickPocketing");

            }
        }
        public void ProcessPickPocketing(CrimeIncidentDTO incident)
        {
            try
            {
                incident.MaxAllowedAmount = _repository.GetMaxAllowedPickPocketing(incident.VictimId);
                incident.IsFriend = friendRepo.IsFriend(incident.UserId, incident.VictimId);
                incident.RobbedRecently = _repository.NotRobbedInLastNDayByUser(incident);
                RobberyRules robberyRules = new RobberyRules();
                StringBuilder parmText = new StringBuilder();
                sbyte priority = 0;
                short notificationTypeId = 0;
                WebUserDTO webUser = webRepo.GetUserPicFName(incident.VictimId);

                ValidationResult validationResult = robberyRules.IsValidPickPocketing(incident);
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.ExecutePickPocket(incident);
                    if (!result)
                    {
                        parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                             incident.Amount, incident.VictimId,
                             webUser.Picture, webUser.FullName,
                             AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.CashSinperFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText.AppendFormat("{0}|{1}|{2}|{3}",
     incident.Amount, incident.VictimId,
     webUser.Picture, webUser.FullName);
                        notificationTypeId = AppSettings.CashSinperSuccessNotificationId;
                        priority = 2;
                        AddNotificationPickPocket(incident);
                    }
                }
                else
                {
                    parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                    incident.Amount, incident.VictimId,
                        webUser.Picture, webUser.FullName, validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.CashSinperFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
notificationTypeId, parmText.ToString(), priority, incident.UserId);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to ProcessPickPocketing");

            }
        }
        private void AddNotificationPickPocket(CrimeIncidentDTO incident)
        {
            StringBuilder parmText = new StringBuilder();
            parmText.AppendFormat("{0}|{1}",
                incident.Amount.ToString("N") + " fiat",
                incident.IncidentId
                );

            userNotif.AddNotification(false, string.Empty,
      AppSettings.CrimeAlertCashNotificationId, parmText.ToString(), 9, incident.VictimId);
        }
        private void AddNotificationRobbery(CrimeIncidentDTO incident, MerchandiseType merchandiseType)
        {
            StringBuilder parmText = new StringBuilder();
            parmText.AppendFormat("{0}|{1}|{2}",
                merchandiseType.ImageFont,
                merchandiseType.Name,
                incident.IncidentId
                );

            userNotif.AddNotification(false, string.Empty,
      AppSettings.CrimeAlertPropertyNotificationId, parmText.ToString(), 9, incident.VictimId);
        }

        public void ProcessSuspectReporting(CrimeIncidentDTO incident)
        {
            try
            {
                string incidentInfo = _repository.GetCrimeReportByIncident(incident.IncidentId);
                incident.SuspectAleadyNotified = _repository.IncidentAlreadyReported(incident);
                ReportIncidentDTO incidentDTO = JsonConvert.DeserializeObject<ReportIncidentDTO>(incidentInfo);
                incident.VictimId = incidentDTO.VictimId;
                incident.MerchandiseTypeId = incidentDTO.MerchandiseTypeId;
                incident.Amount = incidentDTO.Amount;


                RobberyRules robberyRules = new RobberyRules();
                StringBuilder parmText = new StringBuilder();
                sbyte priority = 0;
                short notificationTypeId = 0;
                WebUserDTO webUser = webRepo.GetUserPicFName(incident.SuspectId);

                ValidationResult validationResult = robberyRules.IsValidSuspectReporting(incident);
                parmText.AppendFormat("{0}|{1}|{2}",
                        incident.SuspectId,
                        webUser.Picture, webUser.FullName);
                if (validationResult == ValidationResult.Success)
                {
                    if (incident.MerchandiseTypeId > 1)
                    {
                        incident.StolenAsset = string.Format("<i class='fa fa-2x {0}'></i>{1}", incidentDTO.ImageFont, incidentDTO.Name);
                    }
                    else
                    {
                        incident.StolenAsset = "Cash";
                    }


                    bool result = _repository.ProcessRobberySuspect(incident);
                    if (!result)
                    {
                        parmText.AppendFormat("|{0}",
                             AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.CrimeSuspectFailNotificationId;
                        priority = 7;
                    }
                    else
                    {

                        notificationTypeId = AppSettings.CrimeSuspectSuccessNotificationId;
                        priority = 2;
                    }
                }
                else
                {
                    parmText.AppendFormat("|{0}",
                            validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.CrimeSuspectFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
notificationTypeId, parmText.ToString(), priority, incident.SuspectReportingUserId);
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to ProcessSuspectReporting");

            }
        }
    }
}


