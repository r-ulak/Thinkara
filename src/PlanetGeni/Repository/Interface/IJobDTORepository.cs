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
    public interface IJobDTORepository
    {
         void UpdateExpireJobEmailSent(Guid[] taskIds);
        IEnumerable<ExpiringUserJobDTO> GetUserWithExpiringJobs();

        IEnumerable<CountryCode> GetAllCountryApplyingJob();
        void RunPayRoll(decimal workingdays, DateTime today, DateTime lastpayCheckDate);
        void UpdateUserJobAfterPayRoll(decimal workingdays, DateTime today, DateTime lastpayCheckDate);

        void ClearCache();
        void IncreaseLeadersSalary();
        void IncreaseArmyJob();
        void IncreaseSalaryBudget();
        bool IsCurrentlyOnThisJobCode(int userId, short[] items);
        IEnumerable<JobCode> HasPendingOrOpenOfferForSameJob(int userId, short[] items);
        UserJob GetUserJob(Guid taskId);
        bool RejectedJobOffer(UserJob userJob);
        bool AcceptedJobOffer(UserJob userJob, JobCode jobCode);
        string GetJobProfile(int userid);
        string SearchJob(JobSearchDTO jobCriteria, string countyrId, int UserId);
        JobOverTimeCheckInDTO JobOverTimeCheckIn(int userid, JobOverTimeCheckInDTO jobOverTimeCheckIn);
        IEnumerable<UserJobCodeDTO> GetCurrentJobs(int userid);
        bool WithDrawJob(string[] taskIds, int userid);
        bool QuitJob(string[] taskIds, int userid);
        bool ApplyForJob(ApplyJobCodeDTO[] applyJobList, int userid, string countryId);
        string GetTopTenIncomeSalary();
        JobSummaryDTO GetJobSummary(int userid);
        IEnumerable<UserJobCodeDTO> GetJobHistory
              (int userid, DateTime? parmlastDateTime = null);
        int GetCountrySalaryRank(string countryCode);
        IEnumerable<JobMatchScoreDTO> GetJobMatchScore(int[] userId, JobCode jobCode);
        void UpdateUserJobStatus(string status, Guid[] taskId);
        IEnumerable<AppliedJobDTO> GetAllApplyJobCodeByCountry(string countryId);
        JobCode GetJobCode(short jobCodeId);
        JobCountry GetJobCountry(short jobCodeId, string countryId);
        void UpdateJobCountry(JobCountry jobCountry);
        decimal GetCurrentJobsTotalHPW(int userId);


    }
}
