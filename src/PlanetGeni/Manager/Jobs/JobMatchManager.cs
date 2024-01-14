using Common;
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
    public class JobMatchManager
    {
        private IJobDTORepository jobRepo = new JobDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
        IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
        IWebUserDTORepository webRepo = new WebUserDTORepository();
        IPostCommentDTORepository postRepo = new PostCommentDTORepository();
        private List<int> jobOfferedCandidates = new List<int>();
        private List<int> jobDecliendCandidates = new List<int>();
        private List<int> jobNotAvailableCandidates = new List<int>();
        public JobMatchManager()
        {

        }
        public void MatchJob(int runId)
        {
            IEnumerable<CountryCode> countries = jobRepo.GetAllCountryApplyingJob();

            foreach (var item in countries)
            {

                Console.WriteLine("Processing For CountryId {0}", item.CountryId);
                List<AppliedJobDTO> appliedJobs =
                jobRepo.GetAllApplyJobCodeByCountry(item.CountryId).ToList();
                IEnumerable<short> jobCodes = appliedJobs.Select(x => x.JobCodeId).Distinct();

                foreach (var jobcodeId in jobCodes)
                {
                    jobDecliendCandidates.Clear();
                    jobOfferedCandidates.Clear();
                    jobNotAvailableCandidates.Clear();
                    JobCode jobCode = jobRepo.GetJobCode(jobcodeId);
                    JobCountry jobCountry = jobRepo.GetJobCountry(jobcodeId, item.CountryId);
                    if (jobCountry.QuantityAvailable > 0 && jobCode.JobCodeId > 0)
                    {

                        List<int> alluserIds = appliedJobs.Where(f => f.JobCodeId == jobcodeId).Select(x => x.UserId).ToList();
                        List<JobMatchScoreDTO> jobScore = jobRepo.GetJobMatchScore(alluserIds.ToArray(), jobCode).ToList();
                        jobOfferedCandidates = jobScore.Where(f => f.MatchScore >= jobCode.MinimumMatchScore).Select(x => x.UserId).Take(jobCountry.QuantityAvailable).ToList();

                        foreach (var jobScoreItem in jobScore)
                        {
                            Console.WriteLine("JobMatch Score: {0} UserId: {1}  MinimumMatchScore : {2}", jobScoreItem.MatchScore, jobScoreItem.UserId, jobCode.MinimumMatchScore);
                        }

                        jobNotAvailableCandidates = jobScore.Where(f => f.MatchScore >= jobCode.MinimumMatchScore).Select(x => x.UserId).Skip(jobCountry.QuantityAvailable).ToList();


                        jobDecliendCandidates.AddRange(alluserIds.Except(jobScore.Where(f => f.MatchScore >= jobCode.MinimumMatchScore).Select(x => x.UserId)));


                        Guid[] approvedJobTaskId = appliedJobs.Where(jobs =>
                            jobOfferedCandidates.Any(offered => offered == jobs.UserId && jobs.JobCodeId == jobcodeId)).Select(x => x.TaskId).Distinct().ToArray();

                        Guid[] deniedJobTaskId = appliedJobs.Where(jobs =>
                            jobDecliendCandidates.Any(denied => denied == jobs.UserId && jobs.JobCodeId == jobcodeId)).Select(x => x.TaskId).Distinct().ToArray();

                        Console.WriteLine("Total Job Offering {0}", jobOfferedCandidates.Count);
                        Console.WriteLine("Total Job Decliened {0}", jobDecliendCandidates.Count);


                        Console.WriteLine("Updating JobCode Status for approvedJobTaskId. Total Job Updating {0}", approvedJobTaskId.Length);
                        if (approvedJobTaskId.Length > 0)
                        {
                            jobRepo.UpdateUserJobStatus("O", approvedJobTaskId);
                        }
                        Console.WriteLine("Updating JobCode Status for deniedTaskId. Total Job Updating {0}", deniedJobTaskId.Length);
                        if (deniedJobTaskId.Length > 0)
                        {
                            jobRepo.UpdateUserJobStatus("D", deniedJobTaskId);
                        }

                        Console.WriteLine("Adding Job Offer Task");
                        AddTaskForJobOffer(jobCode.Title, jobCountry.Salary, approvedJobTaskId);

                        Console.WriteLine("Sending Job Offer Notification");
                        SentNotfication(jobCode.Title, jobCountry.Salary, AppSettings.JobOfferNotificationId);
                        Console.WriteLine("Sending Job Denial Notification");
                        SentNotfication(jobCode.Title, jobCountry.Salary, AppSettings.JobDeclinedNotificationId);

                        Console.WriteLine("Sending Job Not Available Notification");
                        SentNotfication(jobCode.Title, jobCountry.Salary, AppSettings.JobNotAvailableNotificationId);

                        Console.WriteLine("Posting Job Offer Notification");
                        SendPost(jobCode.Title, jobCountry.Salary);

                        jobCountry.QuantityAvailable -= jobOfferedCandidates.Count;
                        Console.WriteLine("Updating JobCountry Count");
                        jobRepo.UpdateJobCountry(jobCountry);
                        Console.WriteLine("Job Successfully Completed");
                    }
                    else
                    {
                        Console.WriteLine("Job NOT avalaible for JobCodeId {0} Title {1} Qty avaiable is  {2}",
                       jobCode.JobCodeId, jobCode.Title, jobCountry.QuantityAvailable);
                        jobNotAvailableCandidates = appliedJobs.Where(f => f.JobCodeId == jobcodeId).Select(x => x.UserId).ToList();
                        Console.WriteLine("Sending Job Not Available Notification");
                        SentNotfication(jobCode.Title, jobCountry.Salary, AppSettings.JobNotAvailableNotificationId);

                    }
                }
            }
        }
        private void SentNotfication(string jobTitle, decimal salary, short notificationTypeId)
        {
            StringBuilder parm = new StringBuilder();
            int[] candidates;
            if (notificationTypeId == AppSettings.JobOfferNotificationId)
            {
                parm.AppendFormat("{0}|{1}", jobTitle, salary);
                candidates = jobOfferedCandidates.ToArray();
            }
            else if (notificationTypeId == AppSettings.JobDeclinedNotificationId)
            {
                parm.AppendFormat("{0}", jobTitle);
                candidates = jobDecliendCandidates.ToArray();
            }
            else if (notificationTypeId == AppSettings.JobNotAvailableNotificationId)
            {
                parm.AppendFormat("{0}", jobTitle);
                candidates = jobNotAvailableCandidates.ToArray();
            }
            else
            {
                candidates = new int[] { };
            }
            UserNotification approvedNotfication = new UserNotification
            {
                NotificationTypeId = notificationTypeId,
                Priority = 1,
                HasTask = false,
                Parms = parm.ToString()
            };
            if (candidates.Length > 0)
            {
                Console.WriteLine("Sending Notifcation to {0} Users of notificationType {1}", candidates.Length, notificationTypeId);
                userNotif.SendBulkNotificationsAndPost(approvedNotfication, candidates, new Post());

            }
        }
        private void AddTaskForJobOffer(string jobTitle, decimal jobSalary, Guid[] approvedJobTaskId)
        {
            if (jobOfferedCandidates.Count == 0)
            {
                return;
            }
            DateTime trnDate = DateTime.UtcNow;
            DateTime dueDate = trnDate.AddHours(72);
            StringBuilder parm = new StringBuilder();
            string defaultResponse = "Accept";
            parm.AppendFormat("{0}|{1}|<strong>Date:{2}</strong>|{3}", jobTitle,
              jobSalary,
              dueDate,
              defaultResponse);

            UserTask task = new UserTask
            {
                AssignerUserId = 0,
                CompletionPercent = 0,
                CreatedAt = trnDate,
                DefaultResponse = (short)AppSettings.JobOfferAcceptChoiceId,
                DueDate = dueDate,
                Flagged = false,
                Priority = 11,
                Parms = parm.ToString(),
                Status = "I",
                TaskTypeId = AppSettings.JobTaskType
            };

            TaskReminder taskreminder = new TaskReminder
            {
                EndDate = dueDate,
                StartDate = trnDate,
                ReminderTransPort = "MP",
                ReminderFrequency = 8
            };

            taskRepo.SendBulkTaskListAndReminder(task, jobOfferedCandidates.ToArray(), taskreminder, approvedJobTaskId);

        }
        private void SendPost(string jobTitle, decimal salary)
        {
            if (jobOfferedCandidates.Count == 0)
            {
                return;
            }
            StringBuilder parm = new StringBuilder();
            IEnumerable<WebUserDTO> webuserInfo = webRepo.GetWebUserList(jobOfferedCandidates.ToArray());

            foreach (var item in webuserInfo)
            {
                parm.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                        item.UserId,
                        item.Picture, item.FullName,
                        jobTitle,
                        salary
                        );

                Post post = new Post
                        {
                            Parms = parm.ToString(),
                            PostContentTypeId = AppSettings.JobOfferPostContentTypeId,
                            UserId = item.UserId
                        };
                postRepo.SavePost(post);
                parm.Clear();
            }
        }
    }
}
