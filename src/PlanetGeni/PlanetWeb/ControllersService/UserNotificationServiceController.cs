using DTO.Custom;
using DTO.Db;
using PlanetWeb.ControllersService.RequireHttps;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PlanetWeb.Controllers
{
    [Authorize]
    [RequireHttps]
    public class UserNotificationServiceController : ApiController
    {
        IUserNotificationDetailsDTORepository _repository;
        public UserNotificationServiceController(IUserNotificationDetailsDTORepository repo)
        {
            _repository = repo;
        }

        /// <summary>
        /// Delete this if you are using an IoC controller
        /// </summary>
        public UserNotificationServiceController()
        {
            _repository = new UserNotificationDetailsDTORepository();
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        public IEnumerable<UserNotificationDetailsDTO> GetNewNotificationList(Guid? recentNotificationId = null, DateTime? recentUpdatedAt = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<UserNotificationDetailsDTO> UserNotificationDetailsDTOs =
                _repository.GetNewNotificationList(userid, recentNotificationId, recentUpdatedAt);
            return UserNotificationDetailsDTOs;
        }


        [ApiValidateAntiForgeryToken]
        [HttpGet]
        public IEnumerable<UserNotificationDetailsDTO> GetOldNotificationList(Guid? lastNotificationId = null, DateTime? lastUpdatedAt = null)
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            IEnumerable<UserNotificationDetailsDTO> UserNotificationDetailsDTOs =
                _repository.GetOldNotificationList(userid, lastNotificationId, lastUpdatedAt);
            return UserNotificationDetailsDTOs;
        }

        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public void MarkReadNotification([FromBody]string notificationId)
        {
            Task taskA = Task.Factory.StartNew(() =>
           ProcessDeleteNotification(notificationId));
        }
        [ApiValidateAntiForgeryToken]
        [HttpPost]
        public void ClearAllNotification()
        {
            int userid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            Task taskA = Task.Factory.StartNew(() =>
           ProcessClearAllNotification(userid));
        }
        private void ProcessDeleteNotification(string notificationId)
        {
            try
            {
                _repository.DeleteReadNotification(notificationId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessDeleteNotification");
            }
        }

        private void ProcessClearAllNotification(int userId)
        {
            try
            {
                _repository.DeleteAllNotification(userId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessClearAllNotification");
            }
        }


    }
}
