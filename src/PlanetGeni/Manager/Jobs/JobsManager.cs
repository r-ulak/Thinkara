using Common;
using DAO;
using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;

namespace Manager.Jobs
{
    public class JobsManager
    {
        private WebJobHistory history;
        private WebJobRepository webJobRepo = new WebJobRepository();
        public JobsManager(sbyte jobId)
        {
            history = new WebJobHistory
           {
               JobId = jobId
           };
        }
        public int GetRunId()
        {
            history.CreatedAT = DateTime.UtcNow;
            return webJobRepo.AddjobHistory(history);

        }

        public DateTime GetLastJobRunTime()
        {

            return
                        webJobRepo.GetLastJobRun(history.JobId).CreatedAT;
        }
    }
}
