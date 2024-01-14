using Common;
using DTO.Custom;
using Manager.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoCompleteIndexer
{
    class AutoCompleteIndexerJob
    {

        static void Main(string[] args)
        {
            sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["JobId"]);
            sbyte daysInterval = Convert.ToSByte(ConfigurationManager.AppSettings["DaysInterval"]);
            bool indexWebUser = Convert.ToBoolean(ConfigurationManager.AppSettings["redis.autoComplete.Index.Webuser"]);
            bool indexCity = Convert.ToBoolean(ConfigurationManager.AppSettings["redis.autoComplete.Index.City"]);

            JobsManager jobmanager = new JobsManager(jobId);
            DateTime lastRunTime = jobmanager.GetLastJobRunTime();
            if ((DateTime.UtcNow - lastRunTime).TotalDays > daysInterval)
            {
                int runId = jobmanager.GetRunId();
                if (indexWebUser)
                {
                    IndexWebUser(runId);
                }

            }
            else
            {
                Console.WriteLine("Only {0} days has past Need {1} days to pass before next run", (DateTime.UtcNow - lastRunTime).TotalDays, daysInterval);
            }
        }

        private static void IndexWebUser(int runId)
        {
            AutoCompleteIndexManager<WebUserIndexDTO> webUserIndex =
                new AutoCompleteIndexManager<WebUserIndexDTO>
                (
                AppSettings.RedisHashIndexWebUser,
                AppSettings.RedisSetIndexWebUser,
                AppSettings.SPGetWebUserIndexList,
                new Dictionary<string, object>(),
                AppSettings.RedisAutocompleteDatabaseId,
                "UserId",
                new PropertyInfo[] { typeof(WebUserIndexDTO).GetProperty("EmailId"), typeof(WebUserIndexDTO).GetProperty("FullName") },
                new string[] { "CountryId", "Picture", "UserId" }

                );

            webUserIndex.IndexAll();


        }
    }
}
