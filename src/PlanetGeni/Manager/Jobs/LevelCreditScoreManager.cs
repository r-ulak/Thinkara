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
    public class LevelCreditScoreManager
    {
        IWebUserDTORepository webRepo = new WebUserDTORepository();
        public LevelCreditScoreManager()
        {

        }
        public void ChangeUserLevelAndCreditScore(int runId)
        {
            Console.WriteLine("Applying Credit Score...");
            webRepo.ApplyCreidtScore();
            Console.WriteLine("Changing User Level");

            webRepo.UserChangingLevel(
                AppSettings.ExperincePlusNotificationId,
                AppSettings.ExperienceLevelPostContentTypeId
                );
        }


    }
}
