using Manager.Jobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DegreeChecker
{
    class DegreeChecker
    {
        static void Main(string[] args)
        {
            sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["JobId"]);
            JobsManager jobmanager = new JobsManager(jobId);
            DegreeCheckManager degreemanager = new DegreeCheckManager();
            int runId = jobmanager.GetRunId();
            int count = degreemanager.StartDegreeCheck(runId);
            Console.WriteLine("RunId {0} \n Completed Graduations {1}", runId, count);
        }


    }
}
