using Common;
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
    public class JobDTORepository : IJobDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();
        public JobDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public JobDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public string SearchJob(JobSearchDTO jobCriteria, string countryId, int UserId)
        {
            string parmJobType = string.Empty;
            string parmIndustries = string.Empty;
            string parmMajors = string.Empty;
            if (jobCriteria.JobType.Length > 0)
            {
                parmJobType = string.Join(",", jobCriteria.JobType);
            }
            if (jobCriteria.Industry.Length > 0)
            {
                parmIndustries = string.Join(",", jobCriteria.Industry);
            }
            if (jobCriteria.Major.Length > 0)
            {
                parmMajors = string.Join(",", jobCriteria.Major);
            }
            StringBuilder redisKey = new StringBuilder();
            redisKey.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                AppSettings.RedisKeyJobCodeSearch,
                countryId,
                jobCriteria.SalaryLowerRange,
                parmJobType,
                jobCriteria.SalaryHigherRange,
                "I",
                parmIndustries,
                "M",
                parmMajors,
                "L",
                jobCriteria.LastJobCodeId
            );
            string searchJobData = cache.GetStringKey(redisKey.ToString());
            if (searchJobData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmJobType", parmJobType);
                dictionary.Add("parmIndustryId", parmIndustries);
                dictionary.Add("parmMajorId", parmMajors);
                dictionary.Add("parmCountryId", countryId);
                dictionary.Add("parmSalaryRangeUp", jobCriteria.SalaryHigherRange);
                dictionary.Add("parmlastJobCodeId", jobCriteria.LastJobCodeId);
                dictionary.Add("parmLimit", AppSettings.SearchJobLimit);
                dictionary.Add("parmSalaryRangeDown", jobCriteria.SalaryLowerRange);
                dictionary.Add("parmOverTime", jobCriteria.OverTime);

                searchJobData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<JobCodeDTO>(
                AppSettings.SPSearchJob,
                dictionary));
                cache.SetStringKey(redisKey.ToString(), searchJobData);
            }
            return (searchJobData);
        }
        public JobOverTimeCheckInDTO JobOverTimeCheckIn(int userid, JobOverTimeCheckInDTO jobOverTimeCheckIn)
        {
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskId", jobOverTimeCheckIn.TaskId);
                dictionary.Add("parmUserId", userid);
                UserJob userJob =
                    spContext.GetSqlDataSignleRow<UserJob>(AppSettings.SPGetUserJob, dictionary);
                if (userJob.NextOverTimeCheckIn <= DateTime.UtcNow)
                {
                    userJob.NextOverTimeCheckIn = DateTime.UtcNow.AddHours(userJob.CheckInDuration);
                    userJob.OverTimeHours += userJob.CheckInDuration;
                }
                spContext.Update(userJob);
                jobOverTimeCheckIn.NextOverTimeCheckIn = userJob.NextOverTimeCheckIn;
                return jobOverTimeCheckIn;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to JobOverTimeCheckIn");
            }
            return new JobOverTimeCheckInDTO();
        }
        public IEnumerable<UserJobCodeDTO> GetCurrentJobs(int userid)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userid);
            IEnumerable<UserJobCodeDTO> userjob =
         spContext.GetSqlData<UserJobCodeDTO>
         (AppSettings.SPGetCurrentJobs, dictionary);
            return userjob;

        }
        public IEnumerable<UserJobCodeDTO> GetJobHistory
               (int userid, DateTime? parmlastDateTime = null)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userid);
            dictionary.Add("parmLastDateTime", parmlastDateTime);
            dictionary.Add("parmJobimit", AppSettings.JobHistoryLimit);
            IEnumerable<UserJobCodeDTO> userjob =
         spContext.GetSqlData<UserJobCodeDTO>
         (AppSettings.SPGetJobHistory, dictionary);
            return userjob;

        }
        public JobCode GetJobCode(short jobCodeId)
        {
            return JsonConvert.DeserializeObject<JobCode>(
                GetJobCodeJson(jobCodeId));
        }
        public string GetJobCodeJson(short jobCodeId)
        {
            string reidsKey = AppSettings.RedisKeyJobCode;
            string jobCodeData = cache.GetHash(reidsKey, jobCodeId.ToString());
            if (jobCodeData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmJobCodeId", jobCodeId);
                jobCodeData = JsonConvert.SerializeObject(
                         spContext.GetByPrimaryKey<JobCode>(dictionary));
                cache.SetHash(reidsKey, jobCodeId.ToString(), jobCodeData);
            }
            return jobCodeData;

        }
        public UserJob GetUserJob(Guid taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            return spContext.GetByPrimaryKey<UserJob>(dictionary);

        }
        public JobCountry GetJobCountry(short jobCodeId, string countryId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmJobCodeId", jobCodeId);
            dictionary.Add("parmCountryId", countryId);
            return spContext.GetByPrimaryKey<JobCountry>(dictionary);

        }
        public void UpdateJobCountry(JobCountry jobCountry)
        {
            spContext.Update(jobCountry);
        }

        public string GetJobProfile(int userId)
        {
            string reidsKey = AppSettings.RedisHashUserProfile + userId;
            string profileJobData = cache.GetHash(reidsKey, "job");
            if (profileJobData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                profileJobData = JsonConvert.SerializeObject(
                         spContext.GetSqlData<JobProfileDTO>
                      (AppSettings.SPGetJobProfile, dictionary));
                cache.SetHash(reidsKey, "job", profileJobData);
                cache.ExpireKey(reidsKey, AppSettings.UserProfileCacheLimit);
            }
            return profileJobData;

        }
        public bool WithDrawJob(string[] taskIds, int userid)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                foreach (string item in taskIds)
                {
                    dictionary.Add("parmTaskId", item);
                    dictionary.Add("parmUserId", userid);
                    spContext.ExecuteStoredProcedure(AppSettings.SPWithDrawJob, dictionary);
                    dictionary.Clear();
                }

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SPWithDrawJob");
                return false;
            }
        }
        public bool QuitJob(string[] taskIds, int userid)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                foreach (string item in taskIds)
                {
                    dictionary.Add("parmTaskId", item);
                    dictionary.Add("parmUserId", userid);
                    spContext.ExecuteStoredProcedure(AppSettings.SPQuitJob, dictionary);
                    dictionary.Clear();
                }

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to QuitJob");
                return false;
            }
        }
        public bool ApplyForJob(ApplyJobCodeDTO[] applyJobList, int userid, string countryId)
        {
            bool result = false;
            try
            {
                DateTime dateTime = DateTime.UtcNow;
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                foreach (ApplyJobCodeDTO item in applyJobList)
                {
                    dictionary.Add("parmJobCodeId", item.JobCodeId);
                    dictionary.Add("parmCountryId", countryId);
                    ApplyJobCodeDTO jobCode = spContext.GetSqlDataSignleRow<ApplyJobCodeDTO>
                        (AppSettings.SPGetJobCodeAndSalary, dictionary);
                    dictionary.Clear();
                    UserJob userJob = new UserJob
                    {
                        CheckInDuration = jobCode.CheckInDuration,
                        EndDate = dateTime.AddHours(jobCode.Duration),
                        IncomeYearToDate = 0,
                        JobCodeId = item.JobCodeId,
                        NextOverTimeCheckIn = DateTime.UtcNow,
                        OverTimeHours = 0,
                        Salary = jobCode.Salary,
                        StartDate = dateTime,
                        Status = "P",
                        TaskId = Guid.NewGuid(),
                        UpdatedAt = dateTime,
                        AppliedOn = dateTime,
                        UserId = userid

                    };

                    spContext.Add(userJob);
                }

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ApplyForJob");
                return false;
            }
        }
        public string GetTopTenIncomeSalary()
        {
            string incomeSalaryTopNData = cache.GetStringKey(AppSettings.RedisKeyJobIncomeSummaryTop10);
            if (incomeSalaryTopNData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", 10);
                incomeSalaryTopNData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<TopTenUserSalary>(
                AppSettings.SPGetTopNIncomeSalary,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyJobIncomeSummaryTop10, incomeSalaryTopNData,
                    AppSettings.JobIncomeSummaryTop10CacheLimit);
            }
            return (incomeSalaryTopNData);
        }
        public JobSummaryDTO GetJobSummary(int userid)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userid);
            IEnumerable<JobSummaryStatusDTO> userJobSummaryByStatus =
         spContext.GetSqlData<JobSummaryStatusDTO>
         (AppSettings.SPGetJobSummary, dictionary);


            JobSummaryDTO userJobSummary =
spContext.GetSqlDataSignleRow<JobSummaryDTO>
(AppSettings.SPGetJobSummaryTotal, dictionary);
            string[] acceptedStatus = new string[] { "A", "E", "Q" };
            userJobSummary.TotalJobsAccepted = (int)userJobSummaryByStatus.Where(x => acceptedStatus.Contains(x.Status)).Sum(p => p.Total);
            userJobSummary.TotalJobsRejcted = (int)userJobSummaryByStatus.Where(x => x.Status == "R").Sum(p => p.Total);
            userJobSummary.TotalJobsDenied = (int)userJobSummaryByStatus.Where(x => x.Status == "D").Sum(p => p.Total);
            userJobSummary.TotalJobsPending = (int)userJobSummaryByStatus.Where(x => x.Status == "P").Sum(p => p.Total);
            userJobSummary.TotalJobsOpenOffer = (int)userJobSummaryByStatus.Where(x => x.Status == "O").Sum(p => p.Total);


            return userJobSummary;
        }
        public IEnumerable<JobCode> HasPendingOrOpenOfferForSameJob(int userId, short[] items)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string statusList = "'P', 'O'";
            string itemList = String.Join(",", items);
            dictionary.Add("parmJobCodeList", itemList);
            dictionary.Add("parmJobCodeStatusList", statusList);
            dictionary.Add("parmUserId", userId);

            return spContext.GetSqlData<JobCode>(AppSettings.SPHasPendingOrOpenOfferForSameJob, dictionary);


        }
        public bool IsCurrentlyOnThisJobCode(int userId, short[] items)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string statusList = "'A'";
            string itemList = String.Join(",", items);
            dictionary.Add("parmJobCodeList", itemList);
            dictionary.Add("parmJobCodeStatusList", statusList);
            dictionary.Add("parmUserId", userId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPCurrentOrAppliedJob, dictionary, "cnt"));
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public bool AcceptedJobOffer(UserJob userJob, JobCode jobCode)
        {
            try
            {
                userJob.Status = "A";
                userJob.StartDate = DateTime.UtcNow;
                userJob.EndDate = DateTime.UtcNow.AddHours(jobCode.Duration);
                userJob.NextOverTimeCheckIn = DateTime.UtcNow;
                userJob.UpdatedAt = DateTime.UtcNow;

                spContext.Update(userJob);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to AcceptedJobOffer");

                return false;
            }
        }
        public bool RejectedJobOffer(UserJob userJob)
        {
            try
            {
                userJob.Status = "R";
                spContext.Update(userJob);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to RejectedJobOffer");

                return false;
            }
        }
        public decimal GetCurrentJobsTotalHPW(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            return Convert.ToDecimal(spContext.GetSqlDataSignleValue(AppSettings.SPGetCurrentJobsTotalHPW, dictionary, "TotalMaxHPW"));
        }
        public int GetCountrySalaryRank(string countryCode)
        {
            long? countrySalaryRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountrySalary, countryCode.ToLower());
            if (countrySalaryRank == null)
            {
                PopulateCountrySalary();

                countrySalaryRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountrySalary, countryCode.ToLower());

                if (countrySalaryRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countrySalaryRank);
                }
            }
            else
            {
                return Convert.ToInt32(countrySalaryRank);
            }
        }
        private void PopulateCountrySalary()
        {
            IEnumerable<CountrySalaryDTO> countryPop = (spContext.GetSqlDataNoParms<CountrySalaryDTO>(AppSettings.SPGetCountryAvgJob));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountrySalary, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.AvgSalary))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountrySalary, AppSettings.CountryProfileCacheLimit);

        }
        public IEnumerable<CountryCode> GetAllCountryApplyingJob()
        {
            return spContext.GetSqlDataNoParms<CountryCode>
           (AppSettings.SPGetAllCountryApplyingJob);
        }
        public IEnumerable<AppliedJobDTO> GetAllApplyJobCodeByCountry(string countryId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", countryId);
            return spContext.GetSqlData<AppliedJobDTO>
           (AppSettings.SPGetAllApplyJobCodeByCountry, dictionary);
        }
        public void UpdateUserJobStatus(string status, Guid[] taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskIdList", string.Join(",", taskId.Select(x => string.Format("'{0}'", x))));
            dictionary.Add("parmStatus", status);
            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateUserJobStatus, dictionary);
        }

        public IEnumerable<JobMatchScoreDTO> GetJobMatchScore(int[] userId, JobCode jobCode)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmMatchMajorFactor", RulesSettings.MatchMajorFactor);
            dictionary.Add("parmDurationJobCodeFactor", RulesSettings.DurationJobCodeFactor);
            dictionary.Add("parmDurationIndustryFactor", RulesSettings.DurationIndustryFactor);
            dictionary.Add("parmDurationAnyExperinceFactor", RulesSettings.DurationAnyExperinceFactor);
            dictionary.Add("parmJobCodeId", jobCode.JobCodeId);
            dictionary.Add("parmIndustryId", jobCode.IndustryId);
            dictionary.Add("parmMajorId", jobCode.ReqMajorId);
            dictionary.Add("parmDegreeId", jobCode.ReqDegreeId);
            dictionary.Add("parmUserIdList", string.Join(",", userId));

            return spContext.GetSqlData<JobMatchScoreDTO>
           (AppSettings.SPGetJobMatchScore, dictionary);
        }
        public void IncreaseLeadersSalary()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmLeadersJobBudgetType", AppSettings.LeaderSalaryBudgetType);
            dictionary.Add("parmSeneatorJobCode", AppSettings.SenatorJobCodeId);

            spContext.ExecuteStoredProcedure(AppSettings.SPIncreaseLeadersSalary, dictionary);
        }
        public void IncreaseSalaryBudget()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmSalaryBudgetType", AppSettings.JobsBudgetType);
            dictionary.Add("parmSeneatorJobCode", AppSettings.SenatorJobCodeId);

            spContext.ExecuteStoredProcedure(AppSettings.SPIncreaseSalaryBudget, dictionary);
        }
        public void IncreaseArmyJob()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmArmyBudgetType", AppSettings.ArmyJobBudgetType);
            dictionary.Add("parmArmyJobCode", AppSettings.ArmyJobCodeId);

            spContext.ExecuteStoredProcedure(AppSettings.SPIncreaseArmyJob, dictionary);
        }

        public void RunPayRoll(decimal workingdays, DateTime today, DateTime lastpayCheckDate)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmWorkingDays", workingdays);
            dictionary.Add("parmLastPayCheckDate", lastpayCheckDate);
            dictionary.Add("parmCurrentPayCheckDate", today);
            dictionary.Add("parmBankId", AppSettings.BankId);
            dictionary.Add("parmNotificationTypeId", AppSettings.PayCheckNotificationId);
            dictionary.Add("parmTaxCode", AppSettings.TaxIncomeCode);
            dictionary.Add("parmFundType", AppSettings.PayCheckFundType);

            spContext.ExecuteStoredProcedure(AppSettings.SPRunPayRoll, dictionary);
        }

        public void UpdateUserJobAfterPayRoll(decimal workingdays, DateTime today, DateTime lastpayCheckDate)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmWorkingDays", workingdays);
            dictionary.Add("parmLastPayCheckDate", lastpayCheckDate);
            dictionary.Add("parmCurrentPayCheckDate", today);
            dictionary.Add("parmTaxCode", AppSettings.TaxIncomeCode);

            spContext.ExecuteStoredProcedure(AppSettings.SPUpdateUserJobAfterPayRoll, dictionary);
        }
        public IEnumerable<ExpiringUserJobDTO> GetUserWithExpiringJobs()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmDayRange", RulesSettings.ExpireJobEmailDayInterval);
            return spContext.GetSqlData<ExpiringUserJobDTO>
         (AppSettings.SPGetUserWithExpiringJobs, dictionary);

        }
        public void UpdateExpireJobEmailSent(Guid[] taskIds)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            if (taskIds.Length > 0)
            {
                for (int i = 0; i < taskIds.Length; i += 1000)
                {
                    dictionary.Clear();
                    dictionary.Add("parmTaskId", string.Join(",", taskIds.Skip(i).Take(1000).Select(x => string.Format("'{0}'", x))));
                    spContext.ExecuteStoredProcedure(AppSettings.SPUpdateExpireJobEmailSent, dictionary);
                }
            }

        }
        public void ClearCache()
        {
            string[] keys = new string[]{
                AppSettings.RedisKeyJobCode
            };
            cache.Invalidate(keys);
            string rediskey = AppSettings.RedisKeyJobCodeSearch + "*";
            cache.Invalidate(cache.FindKeys(rediskey));
        }
    }
}
