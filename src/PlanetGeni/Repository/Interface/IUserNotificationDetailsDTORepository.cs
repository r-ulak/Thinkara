using DAO.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserNotificationDetailsDTORepository
    {
        string CompileEmailFromNotfications(IEnumerable<UserByNotification> notfications);
        IEnumerable<UserByNotification> GetNewNotificationThatNeedsToBeEmailed(DateTime today);
        void UpdateEmailSentByTime(bool emailSent, DateTime emailSentBy);
        void UpdateEmailSentStatus(bool emailSent, Guid[] taskId);
        int DeleteAllNotification(int userId);
        IQueryable<UserNotificationDetailsDTO> GetOldNotificationList
            (int userId, Guid? lastNotificationId = null, DateTime? lastUpdatedAt = null);
        IQueryable<UserNotificationDetailsDTO> GetNewNotificationList
            (int userId, Guid? recentNotificationId = null, DateTime? recentUpdatedAt = null);
        int DeleteReadNotification(string notificationId);
        void AddNotification(bool hasTask, string taskId,
        short notificationTypeId, string paramaters,
        sbyte priority, int userId);
        void SendBulkNotificationsAndPost(UserNotification notification, int[] userId, Post userPost);
    }
}
