using Manager.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetSimulatorJob
{
    class PayRollJob
    {
        static void Main(string[] args)
        {
            sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["JobId"]);
            sbyte daysInterval = Convert.ToSByte(ConfigurationManager.AppSettings["DaysInterval"]);
            JobsManager jobmanager = new JobsManager(jobId);
            DateTime lastRunTime = jobmanager.GetLastJobRunTime();
            if ((DateTime.UtcNow - lastRunTime).TotalDays > daysInterval)
            {
                PayRollManager payRoll = new PayRollManager();
                int runId = jobmanager.GetRunId();
                payRoll.RunPayCheck(runId, jobId, lastRunTime);
            }
            else
            {
                Console.WriteLine("Only {0} days has past Need {1} days to pass before next run", (DateTime.UtcNow - lastRunTime).TotalDays, daysInterval);
            }
        }
    }
}
