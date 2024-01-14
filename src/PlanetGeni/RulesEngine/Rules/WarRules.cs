using Common;
using DAO.Models;
using DTO.Db;
using Newtonsoft.Json;
using PlanetGeni.HttpHelper;
using Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class WarRules : IRules
    {
        private int UserId;
        private string UserFullName;
        private string TargetCountryName;
        private string CountryId;
        private string TargetCountryId;


        private HttpClient client = new HttpClient();
        public WarRules()
        {

        }
        public WarRules(string tagertCountryId, string targetCountryName, int userId, string countryId)
        {
            CountryId = countryId;
            UserId = userId;
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            UserFullName = webRepo.GetFullName(userId);
            TargetCountryName = targetCountryName;
            TargetCountryId = tagertCountryId;

        }
        /// <summary>
        /// Request Approval for War Key from Seneators
        /// </summary>
        public void AddAppovalRequestTask(Guid taskId)
        {
            IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
            ICountryLeaderRepository countryRepo = new CountryLeaderRepository();
            string jsonleaders = countryRepo.GetActiveSeneatorJson(CountryId);
            List<CountryLeader> leaders =
                JsonConvert.DeserializeObject<List<CountryLeader>>(jsonleaders);
            DateTime dueDate = DateTime.UtcNow.AddHours(48);
            StringBuilder parm = new StringBuilder();
            string defaultResponse = "Approve";
            parm.AppendFormat("{0}|{1}|{2}|{3}|<strong>Date:{4}</strong>|{5}",
                UserId,
                UserFullName, TargetCountryName, TargetCountryId, dueDate, defaultResponse);
            TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
            taskRepo.SaveReminder(reminderTask);


            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            String notificationparmText = "";
            DateTime dateTime = DateTime.UtcNow;
            notificationparmText = string.Format("{0}|{1}|{2}|{3}|<strong>Date:{4}</strong>",
                  UserId,
                UserFullName, TargetCountryName, TargetCountryId,
                            dateTime.ToString());

            foreach (CountryLeader leader in leaders)
            {
                UserTask warkeyrequest = GetTask(leader, dueDate, parm,
                    (short)AppSettings.WarKeyApprovalChoiceId, taskId);

                taskRepo.SaveTask(warkeyrequest);
                userNotif.AddNotification(true, taskId.ToString(),
           AppSettings.WarRequestVotingRequestNotificationId,
           notificationparmText.ToString(), 10, leader.UserId);

            }

            client.PostAsync(AppSettings.ReminderServiceRunTaskByTypeUrl,
                new StringContent(Http.PutIntoQuotes(AppSettings.WarTaskType.ToString()),
                    Encoding.UTF8, "application/json"));
        }

        private UserTask GetTask(CountryLeader leader, DateTime dueDate,
            StringBuilder parm, short defaultResponse, Guid taskId)
        {

            UserTask warkeyrequest = new UserTask
            {
                TaskId = taskId,
                AssignerUserId = UserId,
                CompletionPercent = 0,
                CreatedAt = DateTime.UtcNow,
                DefaultResponse = defaultResponse,
                DueDate = dueDate,
                Flagged = true,
                Priority = 1,
                Parms = parm.ToString(),
                Status = "I",
                TaskTypeId = AppSettings.WarTaskType,
                UserId = leader.UserId
            };
            return warkeyrequest;
        }

        private TaskReminder GetTaskReminder(DateTime dueDate, Guid taskId)
        {
            TaskReminder keyRequestReminder = new TaskReminder
            {
                TaskId = taskId,
                EndDate = dueDate,
                StartDate = DateTime.UtcNow,
                ReminderTransPort = "MP",
                ReminderFrequency = 4
            };

            return keyRequestReminder;
        }


        public ValidationResult IsValid()
        {
            if (CountryId == TargetCountryId)
            {
                return new ValidationResult("cannot attack own country"); ;

            }

            if (HasPendingRequest())
            {
                return new ValidationResult("peding war request against this country already."); ;

            }
            return ValidationResult.Success;

        }
        /// <summary>
        /// Check to see if there are any pending request for that country
        /// </summary>
        /// <returns></returns>
        public bool HasPendingRequest()
        {
            IWarRequestRepository warRequest = new WarRequestRepository();
            if (warRequest.GetPendingWarRequest(CountryId, TargetCountryId).RequestingCountryId != CountryId)
                return false;
            return true;
        }

        public RequestWarKey GetWarRequestKey(Guid taskId)
        {
            return new RequestWarKey()
            {
                TaskId = taskId,
                ApprovalStatus = "W", //Waiting Approval
                RequestedAt = DateTime.UtcNow,
                RequestingCountryId = CountryId,
                RequestingUserId = UserId,
                TaregtCountryId = TargetCountryId,
                WarStatus = "N", // NOt Started
            };
        }
        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit PostComment then send 1 else 0.
            return result;
        }


    }
}
