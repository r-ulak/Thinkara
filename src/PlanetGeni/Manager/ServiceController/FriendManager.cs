using Common;
using DAO.Models;
using DTO.Db;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Manager.ServiceController
{
    public class FriendManager
    {
        IFriendDetailsDTORepository _repository;
        IUserNotificationDetailsDTORepository userNotif;

        IWebUserDTORepository webRepo;
        public FriendManager(IFriendDetailsDTORepository repo)
        {
            _repository = repo;
            webRepo = new WebUserDTORepository();
            userNotif = new UserNotificationDetailsDTORepository();
        }
        public FriendManager()
        {
            _repository = new FriendDetailsDTORepository();
            webRepo = new WebUserDTORepository();
        }
        public void ProcessSendFriendInvite(EmailInviteDTO inviteList)
        {
            try
            {
                if (AppSettings.SendEmailInvite == false)
                {
                    return;
                }
                MailMessage messsgae = new MailMessage();
                List<WebUserContact> newContacts = new List<WebUserContact>();
                WebUserDTO picName = webRepo.GetUserPicFName(inviteList.UserId);
                messsgae.Subject = "Thinkara Invite from " + picName.FullName;
                StringBuilder emailBody = new StringBuilder();
                emailBody.Append(AppSettings.InviteEmailTemplate);
                emailBody.Replace(":profilePic", AppSettings.AzureProfilePicUrl + picName.Picture);
                emailBody.Replace(":FullName", picName.FullName);
                emailBody.Replace(":Message", picName.FullName + ": " + inviteList.Message);
                Guid lastGuid = Guid.NewGuid();
                emailBody.Replace(":inviteId", lastGuid.ToString());

                foreach (var item in inviteList.InvitationList)
                {
                    messsgae.Bcc.Add(item.FriendEmailId);
                }
                messsgae.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailBody.ToString(), null, MediaTypeNames.Text.Plain));
                messsgae.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(emailBody.ToString(), null, MediaTypeNames.Text.Html));
                inviteList.InvitationId = lastGuid;

                SmtpClient smtp = new SmtpClient();
                smtp.SendMailAsync(messsgae);
                _repository.UpdateEmailInvite(inviteList);



            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessFriend");
            }
        }
        public void ProcessFriendRelation(FriendRelationDTO friendRelation)
        {
            sbyte priority = 0;
            short notificationTypeId = 0;
            StringBuilder parmText = new StringBuilder();
            bool result = false;
            string fullName =
             webRepo.GetFullName(friendRelation.FriendId);
            string action = "";
            switch (friendRelation.ActionType)
            {
                case "F":
                    result = _repository.FollowFriend(friendRelation);
                    action = "Follow";
                    break;
                case "U":
                    result = _repository.UnFollowFriend(friendRelation);
                    action = "UnFollow";
                    break;
                case "B":
                    result = _repository.BlockFollower(friendRelation);
                    action = "Block";
                    break;
            }

            if (result)
            {
                notificationTypeId = AppSettings.SoicalCircleSuccessNotificationId;
                priority = 4;
                parmText.AppendFormat("{0}|{1}|{2}",
                    action, friendRelation.FriendId, fullName);
            }
            else
            {
                notificationTypeId = AppSettings.SoicalCircleFailNotificationId;
                priority = 7;
                parmText.AppendFormat("{0}|{1}|{2}|{3}",
                 action, friendRelation.FriendId, fullName, AppSettings.UnexpectedErrorMsg);
            }
            userNotif.AddNotification(false, string.Empty,
   notificationTypeId, parmText.ToString(), priority, friendRelation.UserId);
        }

        public void ProcessFollowAllFriend(FollowAllDTO followFriends)
        {
            sbyte priority = 0;
            short notificationTypeId = 0;
            StringBuilder parmText = new StringBuilder();
            int result = _repository.FollowAllFriend(followFriends);

            if (result == followFriends.FriendId.Length)
            {
                notificationTypeId = AppSettings.SoicalCircleBulkFollowSuccessNotificationId;
                priority = 4;
                parmText.AppendFormat("{0}|{1}|{2}",
                    result, followFriends.FriendId.Length, "Follwoed");
            }
            else
            {
                notificationTypeId = AppSettings.SoicalCircleBulkFollowFailNotificationId;
                priority = 7;
                parmText.AppendFormat("{0}|{1}|{2}|{3}",
                  result, followFriends.FriendId.Length, "Follwoed", AppSettings.UnexpectedErrorMsg);
            }
            userNotif.AddNotification(false, string.Empty,
   notificationTypeId, parmText.ToString(), priority, followFriends.UserId);
        }

    }
}


