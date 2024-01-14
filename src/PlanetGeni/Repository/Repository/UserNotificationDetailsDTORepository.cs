using Common;
using Common.SendMail;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserNotificationDetailsDTORepository : IUserNotificationDetailsDTORepository
    {
        private StoredProcedure spContext = new StoredProcedure();
        private IRedisCacheProvider cache { get; set; }

        public UserNotificationDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public UserNotificationDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public IQueryable<UserNotificationDetailsDTO> GetOldNotificationList(int userId, Guid? lastNotificationId = null,
            DateTime? lastUpdatedAt = null)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmLastNotificationId", lastNotificationId);
            dictionary.Add("parmLastUpdatedAt", lastUpdatedAt);
            dictionary.Add("parmLimit", AppSettings.NotificationLimit);

            IEnumerable<UserNotificationDetailsDTO> userNotificationList =
                spContext.GetSqlData<UserNotificationDetailsDTO>(AppSettings.SPGetOldUserNotificationList, dictionary);

            return userNotificationList.AsQueryable();
        }
        public IQueryable<UserNotificationDetailsDTO> GetNewNotificationList(int userId, Guid? recentNotificationId = null,
    DateTime? recentUpdatedAt = null)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmRecentNotificationId", recentNotificationId);
            dictionary.Add("parmRecentUpdatedAt", recentUpdatedAt);

            IEnumerable<UserNotificationDetailsDTO> userNotificationList =
                spContext.GetSqlData<UserNotificationDetailsDTO>(AppSettings.SPGetNewUserNotificationList, dictionary);


            return userNotificationList.AsQueryable();
        }

        public int DeleteAllNotification(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            return spContext.ExecuteStoredProcedure(AppSettings.SPDeleteAllNotification, dictionary);

        }
        public int DeleteReadNotification(string notificationId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmNotificationId", notificationId);
            return spContext.ExecuteStoredProcedure(AppSettings.SPDeleteReadNotification, dictionary);

        }
        private UserNotification GetUserNotificationInstance(bool hasTask, string taskId,
            short notificationTypeId, string paramaters,
            sbyte priority, int userId)
        {
            if (hasTask)
            {
                paramaters =
                 String.Format("{0}|onclick: GoToTask()", paramaters);
            }
            UserNotification userNotification = new UserNotification
            {
                HasTask = hasTask,
                NotificationId = taskId == string.Empty ? Guid.NewGuid() : new Guid(taskId),
                NotificationTypeId = notificationTypeId,
                Parms = paramaters,
                Priority = priority,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId

            };
            return userNotification;
        }
        public void AddNotification(bool hasTask, string taskId,
            short notificationTypeId, string paramaters,
            sbyte priority, int userId)
        {
            try
            {
                if (userId == AppSettings.BankId)
                {
                    return;
                }
                UserNotification newnotifcation = GetUserNotificationInstance(hasTask, taskId,
             notificationTypeId, paramaters,
             priority, userId);
                spContext.Add(newnotifcation);
                if (AppSettings.SendEmailNotfication)
                {
                    Task taskA = Task.Factory.StartNew(() => SendEmailNotfication(newnotifcation));

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to Add notification");
            }
        }
        private NotificationType GetOfflineNotficationById(short notificationTypeId)
        {
            string reidsKey = AppSettings.RedisHashNotificationType;

            string notficationData = cache.GetHash(reidsKey, notificationTypeId.ToString());
            if (notficationData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmNotificationTypeId", notificationTypeId);
                var notificationType = spContext.GetSqlDataSignleRow<NotificationType>(AppSettings.SPGetOfflineNotificationById, dictionary);
                if (notificationType.NotificationTypeId == notificationTypeId)
                {
                    notficationData = JsonConvert.SerializeObject(notificationType);
                    cache.SetHash(reidsKey, notificationTypeId.ToString(), notficationData);
                    return notificationType;
                }
                return new NotificationType();
            }

            return JsonConvert.DeserializeObject<NotificationType>(notficationData);
        }

        private void SendEmailNotfication(UserNotification newnotifcation)
        {
            try
            {

                NotificationType noticationType =
                GetOfflineNotficationById(newnotifcation.NotificationTypeId);
                if (noticationType.NotificationTypeId == newnotifcation.NotificationTypeId)
                {
                    IWebUserDTORepository webRepo = new WebUserDTORepository();
                    EmailMessage message = new EmailMessage();
                    message.Destination = webRepo.GetEmailByUserId(newnotifcation.UserId);
                    message.Subject = noticationType.ShortDescription;
                    StringBuilder emailBody = new StringBuilder();
                    emailBody.Append(AppSettings.OfflineNotficationEmailtemplate);
                    PrepareEmailMessage(newnotifcation);
                    emailBody.Replace(":FirstName", webRepo.GetFirstName(newnotifcation.UserId));
                    emailBody.Replace(":Message", string.Format(noticationType.Description, newnotifcation.Parms.Split('|')));
                    emailBody.Replace(":::p-pic", AppSettings.ProfilePicUrl);

                    message.Body = emailBody.ToString();

                    SendEmail mailservice = new SendEmail();
                    mailservice.SendGridasync(message);
                    UpdateEmailSentStatus(true, new Guid[] { newnotifcation.NotificationId });
                }

            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Failed trying to  SendEmailNotfication");
            }
        }

        public string CompileEmailFromNotfications(IEnumerable<UserByNotification> notfications)
        {
            StringBuilder message = new StringBuilder();
            foreach (var item in notfications)
            {
                  NotificationType noticationType =
                GetOfflineNotficationById(item.NotificationTypeId);
                  item.Parms = item.Parms.Replace("Date:", "(UTC) ");
                  item.Parms = string.Format(noticationType.Description, item.Parms.Split('|'));
                  item.Parms.Replace(":::p-pic", AppSettings.ProfilePicUrl);
                  message.Append(item.Parms.Truncate(50));
                  message.Append("<br/>");
                  message.Append("<br/>");
                  message.Append('-', 100);
                  message.Append("<br/>");
                  message.Append("<br/>");
            }
            return message.ToString();
        }

        public void UpdateEmailSentStatus(bool emailSent, Guid[] taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskIdList", string.Join(",", taskId.Select(x => string.Format("'{0}'", x))));
            dictionary.Add("parmEmailSent", emailSent);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateEmailSentStatus, dictionary);
        }

        public void UpdateEmailSentByTime(bool emailSent, DateTime emailSentBy)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmEmailSentTime", emailSentBy);
            dictionary.Add("parmEmailSent", emailSent);
            dictionary.Add("parmTimeLimit", RulesSettings.TimeLimitLastLoginForEmailNotificationinHours);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateEmailSentByTime, dictionary);
        }

        public IEnumerable<UserByNotification> GetNewNotificationThatNeedsToBeEmailed(DateTime today)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTimeLimit", RulesSettings.TimeLimitLastLoginForEmailNotificationinHours);
            dictionary.Add("parmEmailSentTime", today);
            return spContext.GetSqlData<UserByNotification>(AppSettings.SPGetNewNotificationThatNeedsToBeEmailed, dictionary);
        }



        private void PrepareEmailMessage(UserNotification newnotifcation)
        {
            newnotifcation.Parms = newnotifcation.Parms.Replace("Date:", "(UTC) ");
        }

        public void SendBulkNotificationsAndPost(UserNotification notification, int[] userId, Post userPost)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmNotificationTypeId", notification.NotificationTypeId);
            dictionary.Add("parmPriority", notification.Priority);
            dictionary.Add("parmHasTask", notification.HasTask);
            dictionary.Add("parmNotficationParms", notification.Parms);
            dictionary.Add("parmCountryId", userPost.CountryId);
            dictionary.Add("parmPartyId", userPost.PartyId);
            dictionary.Add("parmPostParms", userPost.Parms);
            dictionary.Add("parmPostContentTypeId", userPost.PostContentTypeId);
            dictionary.Add("parmUserIdList", string.Join(",", userId));
            spContext.ExecuteStoredProcedure(AppSettings.SPSendBulkNotificationsAndPost, dictionary);
        }
    }
}
