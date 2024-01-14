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
    public class UserJobManager
    {
        IJobDTORepository _repository;
        IWebUserDTORepository webRepo;

        private IUserNotificationDetailsDTORepository userNotif;
        public UserJobManager(IJobDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
        }
        public UserJobManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            _repository = new JobDTORepository();
        }
        public void ProcessUserJobRejection(Guid taskId)
        {
            UserJob userJob = _repository.GetUserJob(taskId);
            JobCode jobCode = _repository.GetJobCode(userJob.JobCodeId);
            bool result = _repository.RejectedJobOffer(userJob);
            String parmText = "";
            short notificationTypeId = 0;
            sbyte priority = 1;
            if (!result)
            {
                //Add a notification to resubmit 
                parmText = string.Format("{0}|{1}", jobCode.Title, AppSettings.UnexpectedErrorMsg);
                notificationTypeId = AppSettings.JobOfferRejectedFailedNotificationId;
                priority = 7;
            }
            else
            {
                parmText = string.Format("{0}", jobCode.Title); notificationTypeId = AppSettings.JobOfferRejectedSuccessNotificationId;
            }
            userNotif.AddNotification(false, string.Empty,
             notificationTypeId, parmText.ToString(), priority, userJob.UserId);
        }
        public void ProcessUserJobAcceptance(Guid taskId)
        {
            try
            {
                UserJob userJob = _repository.GetUserJob(taskId);
                if (userJob.Status == "O")
                {
                    JobRules userJobRule =
                     new JobRules();
                    String parmText = "";
                    short notificationTypeId = 0;
                    ValidationResult validationResult = userJobRule.IsValidJobAcceptance(
                        _repository.GetCurrentJobsTotalHPW(userJob.UserId), _repository.IsCurrentlyOnThisJobCode(userJob.UserId, new short[] { userJob.JobCodeId })
                        );
                    sbyte priority = 1;
                    JobCode jobCode = _repository.GetJobCode(userJob.JobCodeId);
                    if (validationResult == ValidationResult.Success)
                    {
                        bool result = _repository.AcceptedJobOffer(userJob, jobCode);
                        if (!result)
                        {
                            //Add a notification to resubmit 
                            parmText = string.Format("{0}|{1}", jobCode.Title, AppSettings.UnexpectedErrorMsg);
                            notificationTypeId = AppSettings.JobOfferAccepetedFailedNotificationId;
                            priority = 7;
                        }
                        else
                        {
                            parmText = string.Format("{0}", jobCode.Title); notificationTypeId = AppSettings.JobOfferAccepetedSuccessNotificationId;
                        }
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}", jobCode.Title,
    validationResult.ErrorMessage);
                        notificationTypeId = AppSettings.JobOfferAccepetedFailedNotificationId;
                        priority = 6;
                    }
                    userNotif.AddNotification(false, string.Empty,
               notificationTypeId, parmText.ToString(), priority, userJob.UserId);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessUserJob");
            }
        }
        public void ProcessSaveApplyJobs(ApplyJobCodeDTO[] applyJobList, int userid, string countryId)
        {
            try
            {
                short[] jobCodeIds = applyJobList.Select(x => x.JobCodeId).ToArray();
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                JobRules jobRules =
                 new JobRules();
                String parmText = "";
                short notificationTypeId = 0;
                ValidationResult validationResult = jobRules.IsValidJobApplication(_repository.HasPendingOrOpenOfferForSameJob(userid, jobCodeIds));
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.ApplyForJob(applyJobList, userid, countryId);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("<strong>Date:{0}</strong>|{1}",
                            dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.JobApplicationFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("<strong>Date:{0}</strong>", dateTime.ToString());
                        notificationTypeId = AppSettings.JobApplicationSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("<strong>Date:{0}</strong>|{1}",
                    dateTime.ToString(), validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.JobApplicationFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessSaveApplyJobs");
            }
        }
        public void ProcessQuitJobs(string[] taskIds, int userid)
        {
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = 0;
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                bool result = _repository.QuitJob(taskIds, userid);
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("<strong>Date:{0}</strong>|{1}",
                        dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.JobQuitFailNotificationId;
                    priority = 7;
                }
                else
                {
                    parmText = string.Format("<strong>Date:{0}</strong>", dateTime.ToString());
                    notificationTypeId = AppSettings.JobQuitSuccessNotificationId;
                }

                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessQuitJobs");
            }
        }
        public void ProcessWithDrawJobs(string[] taskIds, int userid)
        {
            try
            {
                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String parmText = "";
                short notificationTypeId = 0;
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                bool result = _repository.WithDrawJob(taskIds, userid);
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("<strong>Date:{0}</strong>|{1}",
                        dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.JobWithDrawFailNotificationId;
                    priority = 7;
                }
                else
                {
                    parmText = string.Format("<strong>Date:{0}</strong>", dateTime.ToString());
                    notificationTypeId = AppSettings.JobWithDrawSuccessNotificationId;
                }

                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, userid);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessWithDrawJobs");
            }
        }
    }
}


