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
    public class CountryBudgetDetailsDTORepository : ICountryBudgetDetailsDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public CountryBudgetDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public CountryBudgetDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public string GetCountryBudget(string countryid)
        {
            ///TODO check if they are eligible to pull in EDIT or read
            string countryBudgetData = cache.GetHash(AppSettings.RedisHashCountryBudget, countryid);

            if (countryBudgetData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryid);
                CountryBudgetDetailsDTO budgetypedetails =
                    spContext.GetSqlDataSignleRow<CountryBudgetDetailsDTO>(AppSettings.SPGetBudgetByCountry, dictionary);
                dictionary.Clear();
                if (budgetypedetails.TaskId != Guid.Empty)
                {
                    dictionary.Add("parmTaskId", budgetypedetails.TaskId);
                    IEnumerable<CountryBudgetTypeDTO> countryBudgetType =
                        spContext.GetSqlData<CountryBudgetTypeDTO>(AppSettings.SPGetBudgetTypeList, dictionary);

                    budgetypedetails.BudgetType = countryBudgetType.ToArray();
                    budgetypedetails.AmountLeft = budgetypedetails.TotalAmount - countryBudgetType.Sum(item => item.Amount);
                }

                countryBudgetData = JsonConvert.SerializeObject(budgetypedetails);

                cache.SetHash(AppSettings.RedisHashCountryBudget, countryid, countryBudgetData);
                cache.ExpireKey(AppSettings.RedisHashCountryBudget, AppSettings.CountryBudgetCacheLimit);

            }
            return countryBudgetData;
        }
        public CountryBudgetByType GetCountryBudgetByType(string countryid, int budgetType)
        {
            ///TODO check if they are eligible to pull in EDIT or read

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", countryid);
            dictionary.Add("parmBudgetType", budgetType);
            CountryBudgetByType countrybudgetType =
    spContext.GetSqlDataSignleRow<CountryBudgetByType>(AppSettings.SPGetCountryBudgetByType, dictionary);
            return countrybudgetType;
        }
        public bool SaveBudget(CountryBudgetDetailsDTO budgetDetails, string countryId, int requestoruserId,
       string fullName, Guid taskId)
        {
            bool result = false;

            try
            {
                if (ProcessBudgetAmendTask(requestoruserId,
                    budgetDetails, fullName, countryId, taskId) == false)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
                ;
                return result;


            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to SaveBudegt");
                result = false;
            }

            return result;
        }
        public CountryBudget GetCountryBudgetByTaskId(Guid taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            return spContext.GetByPrimaryKey<CountryBudget>(dictionary);
        }
        public CountryBudgetDetailsDTO GetCountryBudgetByTask(string taskId)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            CountryBudgetDetailsDTO
            budgetypedetails =
               spContext.GetSqlDataSignleRow<CountryBudgetDetailsDTO>(AppSettings.SPGetBudgetBracketByTaskId, dictionary);
            dictionary.Clear();
            dictionary.Add("parmTaskId", taskId);
            IEnumerable<CountryBudgetTypeDTO> countryBudgetType =
                spContext.GetSqlData<CountryBudgetTypeDTO>(AppSettings.SPGetBudgetTypeList, dictionary);


            budgetypedetails.BudgetType = countryBudgetType.ToArray();
            budgetypedetails.AmountLeft = budgetypedetails.TotalAmount - countryBudgetType.Sum(item => item.Amount);


            return budgetypedetails;
        }
        private TaskReminder GetTaskReminder(DateTime dueDate, Guid taskId)
        {
            TaskReminder budgetreminder = new TaskReminder
            {
                TaskId = taskId,
                EndDate = dueDate,
                StartDate = DateTime.UtcNow,
                ReminderTransPort = "MP",
                ReminderFrequency = 8
            };

            return budgetreminder;
        }
        private UserTask GetTask(Guid taskId, int requestoruserId, DateTime trnDate, short defaultResponse,
            DateTime dueDate, StringBuilder parm, int userId)
        {


            UserTask budgetAmendTask = new UserTask
            {
                TaskId = taskId,
                AssignerUserId = requestoruserId,
                CompletionPercent = 0,
                CreatedAt = trnDate,
                DefaultResponse = defaultResponse,
                DueDate = dueDate,
                Flagged = false,
                Priority = 11,
                Parms = parm.ToString(),
                Status = "I",
                TaskTypeId = AppSettings.BudgetTaskType,
                UserId = userId
            };
            return budgetAmendTask;
        }
        private bool AddAmendBudget(CountryBudgetDetailsDTO budgetDetails, Guid taskId)
        {
            bool result = false;
            try
            {
                CountryBudget countryBudget = new CountryBudget
                {
                    CountryId = budgetDetails.CountryId,
                    EndDate = DateTime.MaxValue.AddSeconds(-1),
                    StartDate = DateTime.UtcNow,
                    TotalAmount = budgetDetails.TotalAmount,
                    TaskId = taskId,
                    CreatedAt = DateTime.UtcNow,
                    Status = budgetDetails.Status
                };
                foreach (CountryBudgetTypeDTO item in budgetDetails.BudgetType)
                {
                    CountryBudgetByType budgetType = new CountryBudgetByType
                    {
                        Amount = item.Amount,
                        AmountLeft = item.Amount,
                        TaskId = taskId,
                        BudgetType = item.BudgetType
                    };
                    spContext.Add(budgetType);
                    result = true;
                }
                spContext.Add(countryBudget);

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to AddAmendBudget");
                result = false;
            }
            return result;
        }
        private bool ProcessBudgetAmendTask(int requestoruserId,
          CountryBudgetDetailsDTO budgetdetails, string fullName, string countryId, Guid taskId)
        {
            bool result = false;
            try
            {
                ICountryLeaderRepository countryRepo = new CountryLeaderRepository();
                string jsonleaders = countryRepo.GetActiveSeneatorJson(countryId);
                List<CountryLeader> leaders =
                    JsonConvert.DeserializeObject<List<CountryLeader>>(jsonleaders);
                DateTime trnDate = DateTime.UtcNow;
                IUserTaskDetailsDTORepository taskRepo = new UserTaskDetailsDTORepository();
                DateTime dueDate = trnDate.AddHours(72);
                TaskReminder reminderTask = GetTaskReminder(dueDate, taskId);
                taskRepo.SaveReminder(reminderTask);
                StringBuilder parm = new StringBuilder();
                string defaultResponse = "Denied";
                parm.AppendFormat(@"{0}|{1}|{2}|<strong>Date:{3}</strong>|{4}",
              requestoruserId, fullName, taskId, dueDate, defaultResponse);

                IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
                String notificationparmText = "";
                DateTime dateTime = DateTime.UtcNow;
                ICountryCodeRepository countrycodeRepo = new CountryCodeRepository();

                CountryCode targetCountry =
                           JsonConvert.DeserializeObject<CountryCode>(countrycodeRepo.GetCountryCodeJson(countryId));

                notificationparmText = string.Format("{0}|{1}|<strong>Date:{2}</strong>|{3}|{4}",
                      requestoruserId,
                    fullName, dateTime.ToString(),
                    targetCountry.Code, targetCountry.CountryId
                    );


                foreach (CountryLeader leader in leaders)
                {
                    UserTask userTask = GetTask(taskId, requestoruserId, trnDate,
                                                (short)AppSettings.BudgetDenialChoiceId, dueDate,
                                                        parm, leader.UserId);
                    taskRepo.SaveTask(userTask);

                    userNotif.AddNotification(true, taskId.ToString(),
                                               AppSettings.BudgetAmendVotingRequestNotificationId,
                                               notificationparmText.ToString(), 7, leader.UserId);

                }
                AddAmendBudget(budgetdetails, taskId);

                result = true;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save budget amend request");
                result = false;
            }
            return result;

        }
        public int ApproveNewBudgetAmendments(string currentTaskId, string newTaskId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskIdCurrent", currentTaskId);
                dictionary.Add("parmTaskIdNew", newTaskId);
                return spContext.ExecuteStoredProcedure(
                    AppSettings.SPUpdateNewCountryBudget, dictionary);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to ApproveNewTaxAmendments");
                return -1;

            }

        }
        private void PopulateCountryBudget()
        {
            IEnumerable<CountryBudgetDTO> countryPop = (spContext.GetSqlDataNoParms<CountryBudgetDTO>(AppSettings.SPGetCountryBudgetRank));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountryBudget, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.TotalAmount))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountryBudget, AppSettings.CountryProfileCacheLimit);
        }
        public decimal GetCountryBudgetValue(string countryCode)
        {
            double? countryBudget = cache.GetSortedSetScore(AppSettings.RedisSortedSetCountryBudget, countryCode.ToLower());
            if (countryBudget == null)
            {
                PopulateCountryBudget();

                countryBudget = cache.GetSortedSetScore(AppSettings.RedisSortedSetCountryBudget, countryCode.ToLower());

                if (countryBudget == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDecimal(countryBudget);
                }
            }
            else
            {
                return Convert.ToDecimal(countryBudget);
            }

        }
        public int GetCountryBudgetRank(string countryCode)
        {
            long? countryBudgetRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryBudget, countryCode.ToLower());
            if (countryBudgetRank == null)
            {
                PopulateCountryBudget();

                countryBudgetRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryBudget, countryCode.ToLower());

                if (countryBudgetRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countryBudgetRank);
                }
            }
            else
            {
                return Convert.ToInt32(countryBudgetRank);
            }
        }
        public string GetCountBudgetPercenTile(string countryId)
        {
            string redisKey = AppSettings.RedisHashCountryProfile + countryId;
            string countryBudgetPercentData = cache.GetHash(redisKey, "budget");
            if (countryBudgetPercentData == null)
            {
                CountryBudgetDetailsDTO budgetypedetails
                    = JsonConvert.DeserializeObject<CountryBudgetDetailsDTO>(GetCountryBudget(countryId));
                if (budgetypedetails.TaskId == Guid.Empty)
                {
                    return "[]";
                }
                countryBudgetPercentData =
                    JsonConvert.SerializeObject(
               budgetypedetails.BudgetType.Select(x => new CountryBudgetPercentDTO { BudgetPercent = x.Amount / budgetypedetails.TotalAmount * 100, Description = x.Description, ImageFont = x.ImageFont }).ToArray());
                cache.SetHash(redisKey, "budget", countryBudgetPercentData);
                cache.ExpireKey(redisKey, AppSettings.CountryProfileCacheLimit);

            }

            return countryBudgetPercentData;



        }
        public int GetCountrySafestRank(string countryCode)
        {
            long? countrySafestRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountrySafest, countryCode.ToLower());
            if (countrySafestRank == null)
            {
                PopulateCountrySafest();

                countrySafestRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountrySafest, countryCode.ToLower());

                if (countrySafestRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countrySafestRank);
                }
            }
            else
            {
                return Convert.ToInt32(countrySafestRank);
            }
        }
        private void PopulateCountrySafest()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmBudgetType", AppSettings.InternalSecurityBudgetType);
            IEnumerable<CountrySafestDTO> countryPop = (spContext.GetSqlData<CountrySafestDTO>(AppSettings.SPGetCountrySafestRank, dictionary));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountrySafest, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.SeurityBudget))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountrySafest, AppSettings.CountryProfileCacheLimit);

        }

        public int ApplyBudgetPopulationStimulator()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPopulationScale", RulesSettings.PopulationScale);
            dictionary.Add("parmBudgetSimulatorTaxType", AppSettings.TaxBudgetSimulartorCode);

            return
            spContext.ExecuteSPWithOutput(AppSettings.SPBudgetPopulationStimulator, dictionary, "parmResultCount");
        }
        public void ApplyBudgetWarStimulator()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmWarWinScale", RulesSettings.WarWinScale);
            dictionary.Add("parmWarLooseScale", RulesSettings.LooseWinScale);
            dictionary.Add("parmBudgetWarVictoryTaxType", AppSettings.TaxWarVictoryCode);
            dictionary.Add("parmBudgetWarLossTaxType", AppSettings.TaxWarLostCode);
            dictionary.Add("parmWinDaysCredit", RulesSettings.WinDaysCredit);
            dictionary.Add("parmLostDaysCredit", RulesSettings.LostDaysCredit);

            spContext.ExecuteStoredProcedure(AppSettings.SPBudgetWarStimulator, dictionary);
        }
        public int ApplyBudgetStimulator()
        {

            return
            spContext.ExecuteSPWithOutput(AppSettings.SPBudgetStimulator, new Dictionary<string, object>(), "parmResultCount");
        }
        public void ReCalculateBudget()
        {
            spContext.ExecuteStoredProcedure(AppSettings.SPReCalculateBudget, new Dictionary<string, object>());
        }
        public void ClearCache()
        {
            string[] keys = new string[]{
                AppSettings.RedisHashCountryBudget,
                AppSettings.RedisSortedSetCountryBudget,
                AppSettings.RedisHashCountryProfile,
                AppSettings.RedisSortedSetCountrySafest
            };
            cache.Invalidate(keys);
        }
    }

}
