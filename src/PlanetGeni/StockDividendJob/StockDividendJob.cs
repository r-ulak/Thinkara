using Manager.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDividendJob
{
    class StockDividendJob
    {
        static void Main(string[] args)
        {
            sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["JobId"]);
            sbyte daysInterval = Convert.ToSByte(ConfigurationManager.AppSettings["DaysInterval"]);
            JobsManager jobmanager = new JobsManager(jobId);
            DateTime lastRunTime = jobmanager.GetLastJobRunTime();
            if ((DateTime.UtcNow - lastRunTime).TotalDays > daysInterval)
            {
                StockDividendManager dividenManager = new StockDividendManager();
                int runId = jobmanager.GetRunId();
                dividenManager.PayStockDividend(runId);
            }
            else
            {
                Console.WriteLine("Only {0} days has past Need {1} days to pass before next run", (DateTime.UtcNow - lastRunTime).TotalDays, daysInterval);
            }
        }
    }
}
