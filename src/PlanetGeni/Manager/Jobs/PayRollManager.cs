using Common;
using DAO;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{
    public class PayRollManager
    {
        IJobDTORepository jobRepo = new JobDTORepository();
        public PayRollManager()
        {

        }
        public void RunPayCheck(int runId, sbyte jobId, DateTime lastRunDate)
        {

            DateTime today = DateTime.UtcNow;
            if (lastRunDate == null || lastRunDate == DateTime.MinValue)
            {
                lastRunDate = today.AddDays(-7);
            }
            decimal workingDays = Convert.ToDecimal((today - lastRunDate).TotalDays);

            Console.WriteLine("Paying pay checks... with paycheckrundate of {0}, lastpayCheckDate was {1} current workingDays {2}", today, lastRunDate, workingDays);
            jobRepo.RunPayRoll(workingDays, today, lastpayCheckDate: lastRunDate);
            Console.WriteLine("Finished Paying Paycheck");

            Console.WriteLine("Updating userJob");
            jobRepo.UpdateUserJobAfterPayRoll(workingDays, today, lastpayCheckDate: lastRunDate);
            Console.WriteLine("Finished userJob");

        }
    }
}
