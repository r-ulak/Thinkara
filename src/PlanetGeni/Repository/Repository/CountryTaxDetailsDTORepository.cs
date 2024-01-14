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
    public class CountryTaxDetailsDTORepository : ICountryTaxDetailsDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public CountryTaxDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public CountryTaxDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public decimal GetCountryTaxByCode(string countryid, int taxcode)
        {
            string currentTaxData = cache.GetStringKey(
             AppSettings.RedisKeyCurrentTaxCode + countryid + taxcode);
            decimal taxPercent;
            if (currentTaxData == null)
            {
                CountryTaxDetailsDTO currentTax = GetCountryTax(countryid);
                taxPercent = currentTax.TaxType.
                                    First(m => m.TaxType == taxcode).TaxPercent;
                cache.SetStringKey(
                  AppSettings.RedisKeyCurrentTaxCode + countryid + taxcode
                  , taxPercent.ToString());
            }
            else
            {
                taxPercent = Convert.ToDecimal(currentTaxData);
            }
            return taxPercent;
        }

        public CountryTax GetCountryTaxByTaskId(string taskId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            return spContext.GetByPrimaryKey<CountryTax>(dictionary);
        }
        public CountryTaxDetailsDTO GetCountryTax(string countryid)
        {
            ///TODO check if they are eligible to pull in EDIT or read
            string currentTaxData = cache.GetStringKey(
             AppSettings.RedisKeyCurrentTax + countryid);
            CountryTaxDetailsDTO taxtypedetails;
            if (currentTaxData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryid);
                taxtypedetails =
                   spContext.GetSqlDataSignleRow<CountryTaxDetailsDTO>(AppSettings.SPGetCurrentCountryTax, dictionary);
                dictionary.Clear();
                dictionary.Add("parmTaskId", taxtypedetails.TaskId);
                IEnumerable<CountryTaxTypeDTO> countryTaxType =
                    spContext.GetSqlData<CountryTaxTypeDTO>(AppSettings.SPGetCountryTaxTypeById, dictionary);
                taxtypedetails.TaxType = countryTaxType.ToArray();
                taxtypedetails.Total = countryTaxType.Sum(item => item.TaxPercent);

                currentTaxData = JsonConvert.SerializeObject(taxtypedetails);
                cache.SetStringKey(
                  AppSettings.SPGetCurrentCountryTax + countryid
                  , currentTaxData);

            }
            else
            {
                taxtypedetails = JsonConvert.DeserializeObject<CountryTaxDetailsDTO>(currentTaxData);

            }
            return taxtypedetails;
        }
        public bool SaveTax(CountryTaxDetailsDTO taxDetails, string countryId, int requestoruserId,
       string fullName, Guid taskId)
        {
            bool result = false;
            try
            {
                if (ProcessTaxAmendTask(requestoruserId,
               taxDetails, fullName, countryId, taskId) == false)
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
                ExceptionLogging.LogError(ex, "Error to SaveTax");
                result = false;
            }
            return result;
        }

        public CountryTaxDetailsDTO GetCountryTaxByTask(string taskId)
        {
            ///TODO check if they are eligible to pull in EDIT or read

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTaskId", taskId);
            CountryTaxDetailsDTO
            taxtypedetails =
               spContext.GetSqlDataSignleRow<CountryTaxDetailsDTO>(AppSettings.SPGetTaxBracketByTaskId, dictionary);
            dictionary.Clear();
            dictionary.Add("parmTaskId", taskId);
            IEnumerable<CountryTaxTypeDTO> countryTaxType =
                spContext.GetSqlData<CountryTaxTypeDTO>(AppSettings.SPGetCountryTaxTypeById, dictionary);
            taxtypedetails.TaxType = countryTaxType.ToArray();
            taxtypedetails.Total = countryTaxType.Sum(item => item.TaxPercent);

            return taxtypedetails;
        }
        private TaskReminder GetTaskReminder(DateTime dueDate, Guid taskId)
        {
            TaskReminder taxreminder = new TaskReminder
            {
                TaskId = taskId,
                EndDate = dueDate,
                StartDate = DateTime.UtcNow,
                ReminderTransPort = "MP",
                ReminderFrequency = 8
            };

            return taxreminder;
        }
        private UserTask GetTask(Guid taskId, int requestoruserId, DateTime trnDate, short defaultResponse,
            DateTime dueDate, StringBuilder parm, int userId)
        {



            UserTask taxAmendTask = new UserTask
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
                TaskTypeId = AppSettings.TaxTaskType,
                UserId = userId
            };
            return taxAmendTask;
        }
        private bool ProcessTaxAmendTask(int requestoruserId,
            CountryTaxDetailsDTO taxdetails, string fullName, string countryId, Guid taskId)
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
                                                (short)AppSettings.TaxDenialChoiceId, dueDate,
                                                        parm, leader.UserId);
                    taskRepo.SaveTask(userTask);

                    userNotif.AddNotification(true, taskId.ToString(),
                                               AppSettings.TaxAmendVotingRequestNotificationId,
                                               notificationparmText.ToString(), 7, leader.UserId);

                }
                if (AddAmendTax(taxdetails, taskId) == false)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
                result = true;

            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to save tax amend request");
                result = false;
            }
            return result;

        }
        private bool AddAmendTax(CountryTaxDetailsDTO taxdetails, Guid taskId)
        {
            bool result = false;
            CountryTax cTax = new CountryTax()
            {
                CountryId = taxdetails.CountryId,
                EndDate = DateTime.MaxValue.AddSeconds(-1),
                StartDate = DateTime.UtcNow,
                Status = "P",
                TaskId = taskId,
                CreatedAt = DateTime.UtcNow
            };
            try
            {
                spContext.Add(cTax);
                foreach (var item in taxdetails.TaxType)
                {
                    CountryTaxByType taxByType = new CountryTaxByType()
                    {
                        TaskId = taskId,
                        TaxPercent = item.TaxPercent,
                        TaxType = item.TaxType
                    };
                    spContext.Add(taxByType);
                }
            }
            catch (Exception ex)
            {

                ExceptionLogging.LogError(ex, "Error to AddAmendTax");
                result = false;
            }
            return result;

        }
        public int ApproveNewTaxAmendments(string currentTaskId, string newTaskId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskIdCurrent", currentTaskId);
                dictionary.Add("parmTaskIdNew", newTaskId);
                return spContext.ExecuteStoredProcedure(
                    AppSettings.SPUpdateNewCountryTax, dictionary);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Failed trying to ApproveNewTaxAmendments");
                return -1;

            }

        }

        public string GetRevenueByCountry(string countryId)
        {
            string redisKey = AppSettings.RedisHashCountryProfile + countryId;
            string countryRevenuePercentData = cache.GetHash(redisKey, "revenue");
            if (countryRevenuePercentData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryId);
                countryRevenuePercentData = JsonConvert.SerializeObject(
                        spContext.GetSqlData<CountryRevenuePercentDTO>(AppSettings.SPGetRevenueByCountry, dictionary));

                cache.SetHash(redisKey, "revenue", countryRevenuePercentData);
                cache.ExpireKey(redisKey, AppSettings.CountryProfileCacheLimit);

            }
            return countryRevenuePercentData;

        }

    }
}
