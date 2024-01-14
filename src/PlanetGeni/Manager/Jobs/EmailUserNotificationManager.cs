using Common;
using Common.SendMail;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class EmailUserNotificationManager
    {
        IUserNotificationDetailsDTORepository notifRepo = new UserNotificationDetailsDTORepository();
        public EmailUserNotificationManager()
        {

        }
        public void SendNotification(int runId)
        {

            if (!AppSettings.SendEmailNotfication)
            {
                Console.WriteLine("SendEmailNotfication currently is {0}", AppSettings.SendEmailNotfication);
                return;
            }
            DateTime today = DateTime.UtcNow;
            Console.WriteLine("getting the GetNewNotificationThatNeedsToBeEmailed... ");
            IEnumerable<UserByNotification> newNotifications = notifRepo.GetNewNotificationThatNeedsToBeEmailed(today);
            IEnumerable<IGrouping<UserEmailDTO, UserByNotification>> groupByEmail =
                            newNotifications.GroupBy(
                            notif =>
                                new UserEmailDTO
                                {
                                    EmailId = notif.EmailId,
                                    NameFirst = notif.NameFirst,
                                    UserId = notif.UserId
                                },
                            notif =>
                                        new UserByNotification
                                        {
                                            NotificationTypeId = notif.NotificationTypeId,
                                            Parms = notif.Parms,
                                            NameFirst = notif.NameFirst,
                                            UserId = notif.UserId
                                        }
                            );

            SendEmail mailservice = new SendEmail();
            EmailMessage message = new EmailMessage();
            StringBuilder emailBody = new StringBuilder();
            message.Subject = "New Notification(s)";
            Console.WriteLine("got {0} NewNotificationByUser... ", newNotifications.Count());
            foreach (var groupItem in groupByEmail)
            {

                emailBody.Clear();
                emailBody.Append(AppSettings.OfflineNotficationEmailtemplate);
                emailBody.Replace(":Message", notifRepo.CompileEmailFromNotfications(groupItem));
                emailBody.Replace(":FirstName", groupItem.Key.NameFirst);
                message.Destination = groupItem.Key.EmailId;
                message.Body = emailBody.ToString();
                Console.WriteLine("Emailing {0} has {1} message size ", message.Destination, emailBody.Length);
                mailservice.SendGridasync(message);

            }

            Console.WriteLine("calling UpdateEmailSentByTime...");
            notifRepo.UpdateEmailSentByTime(true, today);
            Console.WriteLine("Finished EmailUserNotificationManager, total of {0}", newNotifications.Count());

        }
    }
}
