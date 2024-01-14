using Manager.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserTaskDefaultResponseJob
{
    class UserTaskDefaultResponseJob
    {
        static void Main(string[] args)
        {
            sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["JobId"]);
            double daysInterval = Convert.ToDouble(ConfigurationManager.AppSettings["DaysInterval"]);
            JobsManager jobmanager = new JobsManager(jobId);
            DateTime lastRunTime = jobmanager.GetLastJobRunTime();
            if ((DateTime.UtcNow - lastRunTime).TotalDays > daysInterval)
            {
                UserTaskDefaultResponseManager userTaskResponseManager = new UserTaskDefaultResponseManager();
                int runId = jobmanager.GetRunId();
                userTaskResponseManager.StartTaskVoting(runId);
                Console.WriteLine("Job Successfully Completed");
            }
            else
            {
                Console.WriteLine("Only {0} days has past Need {1} days to pass before next run", (DateTime.UtcNow - lastRunTime).TotalDays, daysInterval);
            }
        }
    }
}
