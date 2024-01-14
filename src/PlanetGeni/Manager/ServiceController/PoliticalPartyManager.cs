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
    public class PoliticalPartyManager
    {
        IUserNotificationDetailsDTORepository userNotif; IPartyDTORepository _repository;
        public PoliticalPartyManager(IPartyDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
        }
        public PoliticalPartyManager()
        {
            _repository = new PartyDTORepository();
            userNotif = new UserNotificationDetailsDTORepository();
        }
        public void ProcessRequestNominationParty(PartyNominationDTO nominationParty)
        {
            try
            {
                Task<string> taskGetPartyMemberType = Task<string>.Factory.StartNew(() => _repository.GetPartyMemberType(nominationParty.NomineeId));

                Task<string> taskGetActiveParty = Task<string>.Factory.StartNew(() => _repository.GetActivePartyId(nominationParty.NomineeId));

                Task<bool> taskHasPendingNomination = Task<bool>.Factory.StartNew(() => _repository.HasPendingNomination(nominationParty.PartyId, nominationParty.NomineeId));

                nominationParty.InitatorPartyId = _repository.GetActivePartyId(nominationParty.InitatorId);

                taskGetActiveParty.Wait();
                taskHasPendingNomination.Wait();
                taskGetPartyMemberType.Wait();

                nominationParty.NomineeIdPartyId = taskGetActiveParty.Result;
                nominationParty.HasPendingNomination = taskHasPendingNomination.Result;
                nominationParty.NomineeIdMemberStatus = _repository.GetMemberStatus(nominationParty.NomineeId);
                PartyRules partyRules =
                 new PartyRules(nominationParty, _repository.GetPartyById(nominationParty.PartyId));
                StringBuilder parmText = new StringBuilder();
                short notificationTypeId = 0;
                ValidationResult validationResult = partyRules.IsValidNominationParty();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;

                IWebUserDTORepository userRepo = new WebUserDTORepository();

                nominationParty.InitatorFullName = userRepo.GetFullName(nominationParty.InitatorId);
                nominationParty.InitatorPicture = userRepo.GetUserPicture(nominationParty.InitatorId);

                nominationParty.NomineeFullName = userRepo.GetFullName(nominationParty.NomineeId);
                nominationParty.NomineePicture = userRepo.GetUserPicture(nominationParty.NomineeId);

                nominationParty.PartyName = _repository.GetpartyName(nominationParty.PartyId);

                nominationParty.GetPartyMemberType();

                if (validationResult == ValidationResult.Success)
                {

                    bool result = _repository.NotifyNominationPartyMember(nominationParty);
                    if (!result)
                    {
                        parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|<Strong>Date:{5}</Strong>|{6}",
                     nominationParty.InitatorId, nominationParty.NomineeFullName, nominationParty.PartyMemberType,
             nominationParty.PartyId, nominationParty.PartyName,
                   dateTime.ToString(), AppSettings.UnexpectedErrorMsg);

                        priority = 7;
                    }
                    else
                    {
                        parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                    nominationParty.InitatorId, nominationParty.NomineeFullName, nominationParty.PartyMemberType,
         nominationParty.PartyId, nominationParty.PartyName);
                        notificationTypeId = AppSettings.PartyNominationRequestSuccessNotificationId;
                    }
                }
                else
                {
                    parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|<Strong>Date:{5}</Strong>|{6}",
                    nominationParty.InitatorId, nominationParty.NomineeFullName, nominationParty.PartyMemberType,
              nominationParty.PartyId, nominationParty.PartyName,
                    dateTime.ToString(), validationResult.ErrorMessage);

                    notificationTypeId = AppSettings.PartyNominationRequestFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, nominationParty.InitatorId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessNominationParty");
            }
        }
        public void ProcessEjectMember(EjectPartyDTO ejectionDto)
        {
            try
            {

                IWebUserDTORepository webUserRepo = new WebUserDTORepository();

                String parmText = "";
                short notificationTypeId = 0;
                ejectionDto.IsEjectorFounderorCoFounder = _repository.IsPartyFounderOrCoFounder(ejectionDto.InitatorId);
                ejectionDto.EjecteePartyId = _repository.GetActivePartyId(ejectionDto.EjecteeId);
                ejectionDto.EjectorPartyId = _repository.GetActivePartyId(ejectionDto.InitatorId);
                ejectionDto.EjecteeMemberStatus = _repository.GetMemberStatus(ejectionDto.EjecteeId);
                ejectionDto.EjecteeMemberType = _repository.GetPartyMemberType(ejectionDto.EjecteeId);
                ejectionDto.PartyName = _repository.GetpartyName(ejectionDto.PartyId);
                PartyRules partyRules =
               new PartyRules(ejectionDto);
                ValidationResult validationResult = partyRules.IsValidPartyEjection();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;

                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.RequestEjectPartyMember(ejectionDto);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}|{1}|{2}", ejectionDto.PartyId,
                            ejectionDto.PartyName, AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.PartyEjectFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}", ejectionDto.PartyId,
                                ejectionDto.PartyName);
                        notificationTypeId = AppSettings.PartyEjectSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}|{2}", ejectionDto.PartyId,
                           ejectionDto.PartyName, validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.PartyEjectFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, ejectionDto.InitatorId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessEjectMember");
            }
        }

        public void ProcessRequestJoinParty(JoinRequestPartyDTO[] joinParty, int userId)
        {
            try
            {

                DateTime dateTime = DateTime.UtcNow;
                IWebUserDTORepository userRepo = new WebUserDTORepository();

                string fullname = userRepo.GetFullName(userId);

                foreach (var item in joinParty)
                {
                    item.IsAlreadyCurrentPartyMember = _repository.IsActiveMemberOfParty(item.PartyId, userId);
                    item.HasPendingJoinRequest = _repository.HasPendingJoinRequest(item.PartyId, userId);
                    item.UserId = userId;
                    item.FullName = fullname;
                    item.RequestDateTime = dateTime;
                }
                PartyRules partyRules =
                 new PartyRules(joinParty);
                partyRules.IsValidJoinPartyRequest();

                String parmText = "";
                short notificationTypeId = 0;
                sbyte priority = 0;
                foreach (var item in partyRules.JoinParty)
                {
                    if (item.IsValid == ValidationResult.Success)
                    {
                        item.PartyName = _repository.GetpartyName(item.PartyId);
                        bool result = _repository.RequestJoinParty(item);

                        if (!result)
                        {
                            //Add a notification to resubmit 

                            parmText = string.Format("{0}|{1}|<Strong>Date:{2}</Strong>|{3}", item.PartyId,
                                item.PartyName, dateTime, AppSettings.UnexpectedErrorMsg);
                            notificationTypeId = AppSettings.PartyApplyJoinRequestFailNotificationId;
                            priority = 7;
                        }
                        else
                        {
                            parmText = string.Format("{0}|{1}|<Strong>Date:{2}</Strong>", item.PartyId,
                                item.PartyName, dateTime);
                            notificationTypeId = AppSettings.PartyApplyJoinRequestSuccessNotificationId;
                        }
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}|<Strong>Date:{2}</Strong>|{3}", item.PartyId,
                                 item.PartyName, dateTime, item.IsValid.ErrorMessage);
                        notificationTypeId = AppSettings.PartyApplyJoinRequestFailNotificationId;
                        priority = 6;
                    }
                    userNotif.AddNotification(false, string.Empty,
               notificationTypeId, parmText.ToString(), priority, item.UserId);

                }


            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessNominationParty");
            }
        }
        public void ProcessDonation(DonatePartyDTO donation)
        {
            try
            {


                StringBuilder parmText = new StringBuilder();
                short notificationTypeId = 0;

                donation.IsCurrentOrPastParty = _repository.IsCurrentOrPastParty(donation.UserId, donation.PartyId);
                donation.PartyName = _repository.GetpartyName(donation.PartyId);
                donation.PartyStatus = _repository.GetpartyStatus(donation.PartyId);
                PartyRules partyRules =
            new PartyRules(donation);
                ValidationResult validationResult = partyRules.IsValidPartyDonation();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;

                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.ExecuteDonateParty(donation);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText.AppendFormat("{0}|{1}|{2}|{3}",
                            donation.Amount,
                            donation.PartyId,
                            donation.PartyName,
                            AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.PartyDonateFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        //Add a notification to resubmit 
                        parmText.AppendFormat("{0}|{1}|{2}",
                          donation.Amount,
                          donation.PartyId,
                          donation.PartyName);
                        notificationTypeId = AppSettings.PartyDonateSuccessNotificationId;
                    }
                }
                else
                {
                    parmText.AppendFormat("{0}|{1}|{2}|{3}",
                   donation.Amount,
                   donation.PartyId,
                   donation.PartyName,
                    validationResult.ErrorMessage
                   );
                    notificationTypeId = AppSettings.PartyDonateFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, donation.UserId);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessDonation");
            }
        }
        public void ProcessPartyInvite(PartyInviteDTO partyInvite)
        {
            try
            {

                IWebUserDTORepository webRepo = new WebUserDTORepository();
                StringBuilder parmText = new StringBuilder();
                short notificationTypeId = 0;
                partyInvite.PartyId = _repository.GetActivePartyId(partyInvite.UserId);

                foreach (InviteeDTO item in partyInvite.PartyInvites)
                {

                    if (!String.IsNullOrEmpty(item.EmailId))
                    {
                        item.FriendId = webRepo.GetUserIdByEmail(item.EmailId);

                    }

                    if (item.FriendId > 0)
                    {
                        item.AlreadyCurrentPartyMember = _repository.IsActiveMemberOfParty(partyInvite.PartyId, item.FriendId);
                        item.HasPendingPartyInviteForCurrentParty = _repository.HasPendingPartyInivite(partyInvite.PartyId, item.FriendId);
                    }
                    else if (!String.IsNullOrEmpty(item.EmailId))
                    {
                        item.HasPendingPartyInviteForCurrentParty = _repository.HasPendingPartyInivite(partyInvite.PartyId, item.EmailId);
                    }

                }

                partyInvite.PartyName = _repository.GetpartyName(partyInvite.PartyId);
                PartyRules partyRules =
            new PartyRules(partyInvite);
                partyRules.IsValidPartyInviteRequest();
                sbyte priority = 0;
                short failNotifyId = 0;
                short successNotifyId = 0;
                string InitiatorfullName = webRepo.GetFullName(partyInvite.UserId);

                foreach (InviteeDTO item in partyRules.PartyInviteInfo.PartyInvites)
                {
                    parmText.Clear();
                    if (item.FriendId > 0)
                    {
                        item.FullName = webRepo.GetFullName(item.FriendId);
                        failNotifyId = AppSettings.PartyInvitationNotifyFailNotificationId;
                        successNotifyId = AppSettings.PartyInvitationNotifySuccessNotificationId;
                        parmText.AppendFormat("{0}|{1}|",
                            item.FriendId,
                            item.FullName
                            );
                    }
                    else if (!String.IsNullOrEmpty(item.EmailId))
                    {
                        failNotifyId = AppSettings.PartyInvitationNotifyEmailFreindsFailNotificationId;
                        successNotifyId = AppSettings.PartyInvitationNotifyEmailFreindsSuccessNotificationId;
                        parmText.AppendFormat("{0}|",
                        item.EmailId
                        );
                    }
                    else
                    {
                        continue;
                    }

                    if (item.IsValid == ValidationResult.Success)
                    {
                        bool result = _repository.SendPartyInvite(item, partyRules.PartyInviteInfo.PartyId, partyRules.PartyInviteInfo.UserId, InitiatorfullName);

                        if (!result)
                        {
                            //Add a notification to resubmit 

                            parmText.AppendFormat("{0}|{1}|{2}", partyInvite.PartyId,
                                partyInvite.PartyName, AppSettings.UnexpectedErrorMsg);
                            notificationTypeId = failNotifyId;
                            priority = 7;
                        }
                        else
                        {
                            parmText.AppendFormat("{0}|{1}", partyInvite.PartyId,
                                partyInvite.PartyName);
                            notificationTypeId = successNotifyId;
                        }
                    }
                    else
                    {
                        parmText.AppendFormat("{0}|{1}|{2}", partyInvite.PartyId,
                                 partyInvite.PartyName, item.IsValid.ErrorMessage);
                        notificationTypeId = failNotifyId;
                        priority = 6;
                    }
                    userNotif.AddNotification(false, string.Empty,
               notificationTypeId, parmText.ToString(), priority, partyInvite.UserId);

                }

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessPartyInvite");
            }
        }
        public void ProcessStartParty(StartPartyDTO startParty)
        {
            try
            {

                IWebUserDTORepository userRepo = new WebUserDTORepository();
                startParty.CountryId = userRepo.GetCountryId(startParty.InitatorId);

                startParty.IsActiveMemberOfDiffrentParty = _repository.IsActiveMemberOfDiffrentParty(startParty.InitatorId);
                startParty.IsUniquePartyName = _repository.IsUniquePartyName(startParty.PartyName,
                    startParty.CountryId);
                PartyRules partyRules =
                 new PartyRules(startParty);
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = partyRules.IsValidCreateParty();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    startParty.FullName = userRepo.GetFullName(startParty.InitatorId);
                    startParty.PartyId = Guid.NewGuid();
                    bool result = _repository.StartParty(startParty);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}|{1}",
                            startParty.PartyName, AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.PartyOpenFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}", startParty.PartyId,
                            startParty.PartyName);
                        notificationTypeId = AppSettings.PartyOpenSuccessNotificationId;
                        StartPartyInvites(startParty);
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}",
                     startParty.PartyName, validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.PartyOpenFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, startParty.InitatorId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessStartParty");
            }
        }
        private void StartPartyInvites(StartPartyDTO startParty)
        {
            InviteeDTO[] invitees = new InviteeDTO[startParty.FriendInvitationList.Length + startParty.ContactInvitationList.Length];
            int index = 0;
            foreach (var item in startParty.FriendInvitationList)
            {
                invitees[index] = new InviteeDTO();
                invitees[index].FriendId = item;
                index++;
            }
            foreach (var item in startParty.ContactInvitationList)
            {
                invitees[index] = new InviteeDTO();
                invitees[index].EmailId = item;
                index++;
            }
            PartyInviteDTO newPartyInvite = new PartyInviteDTO();
            newPartyInvite.UserId = startParty.InitatorId;
            newPartyInvite.PartyInvites = invitees;


            ProcessPartyInvite(newPartyInvite);
        }
        public void ProcessQuitParty(QuitPartyDTO quit)
        {
            try
            {


                StringBuilder parmText = new StringBuilder();
                short notificationTypeId = 0;

                quit.PartyId = _repository.GetActivePartyId(quit.UserId);
                PoliticalParty partyInfo = _repository.GetPartyById(quit.PartyId);

                quit.PartyName = partyInfo.PartyName;
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;

                bool result = _repository.LeaveParty(quit, partyInfo);
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText.AppendFormat("{0}|{1}|{2}",
                        quit.PartyId,
                        quit.PartyName,
                        AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.PartyLeaveRequestFailNotificationId;
                    priority = 7;
                }
                else
                {
                    //Add a notification to resubmit 
                    parmText.AppendFormat("{0}|{1}",
                      quit.PartyId,
                      quit.PartyName);
                    notificationTypeId = AppSettings.PartyLeaveRequestSuccessNotificationId;
                }

                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, quit.UserId);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessQuit");
            }
        }
        public void ProcessManagePartyUpload(StartPartyDTO startParty)
        {
            try
            {

                IWebUserDTORepository userRepo = new WebUserDTORepository();
                startParty.CountryId = userRepo.GetCountryId(startParty.InitatorId);

                String parmText = "";
                short notificationTypeId = 0;
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                startParty.PartyId = new Guid(_repository.GetActivePartyId(startParty.InitatorId));
                bool result = _repository.ManagePartyUploadLogo(startParty);
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("{0}|{1}",
                        startParty.PartyName, AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.PartyManageFailNotificationId;
                    priority = 7;
                }
                else
                {
                    parmText = string.Format("{0}|{1}", startParty.PartyId,
                        startParty.PartyName);
                    notificationTypeId = AppSettings.PartyManageSuccessNotificationId;
                }

                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, startParty.InitatorId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessManagePartyUpload");
            }
        }
        public void ProcessManageParty(StartPartyDTO startParty)
        {
            try
            {




                startParty.PartyId = new Guid(_repository.GetActivePartyId(startParty.InitatorId));
                PoliticalParty partyinfo = _repository.GetPartyById(startParty.PartyId.ToString());
                startParty.CountryId = partyinfo.CountryId;
                if (partyinfo.PartyName == startParty.PartyName)
                {
                    startParty.IsUniquePartyName = true;
                    startParty.PartyNameChanged = false;

                }
                else
                {
                    startParty.PartyNameChanged = true;
                    startParty.OriginalPartyName = partyinfo.PartyName;
                    startParty.IsUniquePartyName = _repository.IsUniquePartyName(startParty.PartyName,
                        partyinfo.CountryId);
                }
                PartyRules partyRules =
                 new PartyRules(startParty);
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = partyRules.IsValidManageParty();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {

                    bool result = _repository.ManageParty(startParty);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}|{1}",
                            startParty.PartyName, AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.PartyManageFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}", startParty.PartyId,
                            startParty.PartyName);
                        notificationTypeId = AppSettings.PartyManageSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}",
                     startParty.PartyName, validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.PartyManageFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, startParty.InitatorId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessManageParty");
            }
        }
        public void ProcessRequestCloseParty(ClosePartyDTO closeParty)
        {
            try
            {

                IWebUserDTORepository userRepo = new WebUserDTORepository();

                ;
                closeParty.PartyId = _repository.GetActivePartyId(closeParty.InitatorId);

                if (closeParty.PartyId == new Guid().ToString())
                {
                    return;
                }
                PoliticalParty partyinfo = _repository.GetPartyById(closeParty.PartyId);
                PartyRules partyRules =
                 new PartyRules(partyinfo, closeParty);
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = partyRules.IsValidCloseParty();
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                closeParty.PartyName = partyinfo.PartyName;

                if (validationResult == ValidationResult.Success)
                {
                    closeParty.InitatorFullName = userRepo.GetFullName(closeParty.InitatorId);
                    bool result = _repository.RequestCloseParty(closeParty);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}|{1}|{2}",
                            closeParty.PartyId,
                            closeParty.PartyName, AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.PartyCloseRequestFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}",
                            closeParty.PartyId,
                            closeParty.PartyName);
                        notificationTypeId = AppSettings.PartyCloseRequestSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}|{2}",
                        closeParty.PartyId,
                        closeParty.PartyName, validationResult.ErrorMessage); notificationTypeId = AppSettings.PartyCloseRequestFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, closeParty.InitatorId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessCloseParty");
            }
        }
    }
}


