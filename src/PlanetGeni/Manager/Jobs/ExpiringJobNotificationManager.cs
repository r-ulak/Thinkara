using Common;
using Common.SendMail;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Manager.Jobs
{
    public class ExpiringJobNotificationManager
    {
        IJobDTORepository jobRepo = new JobDTORepository();
        IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
        public ExpiringJobNotificationManager()
        {

        }
        public void SendEmailNotification(int runId)
        {

            Console.WriteLine("getting the Expring job... ");
            IEnumerable<ExpiringUserJobDTO> expringJobs = jobRepo.GetUserWithExpiringJobs();
            StringBuilder parmText = new StringBuilder();
            List<Guid> taskIds = new List<Guid>();
            foreach (var item in expringJobs)
            {
                parmText.AppendFormat("{0}|{1}|{2}|<strong>Date:{3}</strong>|{4}",
                    item.Title,
                    item.Salary,
                    item.EndDate <= DateTime.UtcNow ? "has expired" : "will expire",
                    item.EndDate,
                    item.IncomeYearToDate.ToString("N")
                    );
                userNotif.AddNotification(false, string.Empty,
                     AppSettings.TimeToFindNewJobNotificationId, parmText.ToString(), 5, item.UserId);
                parmText.Clear();
                taskIds.Add(item.TaskId);
            }

            jobRepo.UpdateExpireJobEmailSent(taskIds.ToArray());
            Console.WriteLine("Finished SendEmailNotification, total of {0}", expringJobs.Count());

        }
    }
}
