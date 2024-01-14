using AzureServices;
using Common;
using DAO;
using DAO.Models;
using DTO.Db;
using Newtonsoft.Json;
using NUnit.Framework;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    [TestFixture]
    public class Persistence
    {
        private StoredProcedureExtender sp = new StoredProcedureExtender();
        private Random rand = new Random();
        //[Test]
        public void AddCountryBudget()
        {
            ICountryCodeRepository countryRepo = new CountryCodeRepository();

            List<CountryCode> countries = JsonConvert.DeserializeObject<List<CountryCode>>(countryRepo.GetCountryCodes());
            ICountryBudgetDetailsDTORepository budgetRepo = new CountryBudgetDetailsDTORepository();

            string sqlBudget = string.Format("Select * from BudgetCode;");

            List<BudgetCode> budgetCodes = sp.GetSqlData<BudgetCode>(sqlBudget).ToList();


            foreach (var item in countries)
            {
                Guid taskId = Guid.NewGuid();
                CountryBudget countryBudget = new CountryBudget
                {
                    CountryId = item.CountryId,
                    CreatedAt = DateTime.UtcNow,
                    StartDate = new DateTime(2015, 4, 1),
                    EndDate = new DateTime(2015, 4, 30),
                    Status = "A",
                    TotalAmount = 10000,
                    TaskId = taskId
                };
                sp.Add(countryBudget);
                foreach (var budget in budgetCodes)
                {
                    CountryBudgetByType budgetType = new CountryBudgetByType
                    {
                        TaskId = taskId,
                        BudgetType = budget.BudgetType,
                        AmountLeft = 100,
                        Amount = 1000
                    };
                    sp.Add(budgetType);
                }
            }
        }

        //[Test]
        public void AddCountryTax()
        {
            ICountryCodeRepository countryRepo = new CountryCodeRepository();

            List<CountryCode> countries = JsonConvert.DeserializeObject<List<CountryCode>>(countryRepo.GetCountryCodes());
            ICountryTaxDetailsDTORepository taxRepo = new CountryTaxDetailsDTORepository();

            string sqlTax = string.Format("Select * from TaxCode;");

            List<TaxCode> taxCodes = sp.GetSqlData<TaxCode>(sqlTax).ToList();


            foreach (var item in countries)
            {
                Guid taskId = Guid.NewGuid();
                CountryTax countryTax = new CountryTax
                {
                    CountryId = item.CountryId,
                    CreatedAt = DateTime.UtcNow,
                    StartDate = DateTime.UtcNow.AddDays(-10),
                    EndDate = DateTime.UtcNow.AddYears(2),
                    Status = "A",
                    TaskId = taskId
                };
                sp.Add(countryTax);
                foreach (var tax in taxCodes)
                {
                    CountryTaxByType taxType = new CountryTaxByType
                    {
                        TaskId = taskId,
                        TaxPercent = 10,
                        TaxType = tax.TaxType
                    };
                    sp.Add(taxType);
                }
            }
        }
        [Test]
        [Repeat(1)]
        public void ReadCodeTablesJSON()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            CodeTableDTO codeTables = new CodeTableDTO();
            codeTables.PostContentType = sp.GetSqlDataNoParms<PostContentType>(AppSettings.SPPostContentTypeList);
            codeTables.NotificationType = sp.GetSqlDataNoParms<NotificationType>(AppSettings.SPNotificationTypeList);
            codeTables.TaskType = sp.GetSqlDataNoParms<TaskType>(AppSettings.SPTaskTypeList);
            codeTables.ContactProvider = sp.GetSqlDataNoParms<ContactProvider>(AppSettings.SPGetAllContactProvider);

            string content = JsonConvert.SerializeObject(codeTables);
            System.IO.File.WriteAllText(@"C:\Home\CodeBack\PlanetGeni\PlanetWeb\Content\cdn\codetables.js", content);
        }
        //[Test]
        public void AddJobCountry()
        {
            ICountryCodeRepository countryRepo = new CountryCodeRepository();

            string getJobCode = "Select * from JobCode";
            List<JobCode> jobCodes = sp.GetSqlData<JobCode>(getJobCode).ToList();
            string deleteJobCode = "Delete from JobCountry";
            sp.ExecuteSql(deleteJobCode);
            List<CountryCode> countries = JsonConvert.DeserializeObject<List<CountryCode>>(countryRepo.GetCountryCodes());
            JobCountry jobCountry = new JobCountry();
            decimal salaryFactor = 0.1M;
            StringBuilder insertSql = new StringBuilder();
            foreach (var item in countries)
            {
                jobCountry.CountryId = item.CountryId;
                salaryFactor = 100 / (countryRepo.GetCountryPopulation(item.CountryId) + 1);
                if (salaryFactor < 0.1M)
                {
                    salaryFactor = 0.1M;
                }
                foreach (var jobcode in jobCodes)
                {
                    jobCountry.JobCodeId = jobcode.JobCodeId;
                    jobCountry.Salary = jobcode.MinimumMatchScore * salaryFactor;
                    jobCountry.QuantityAvailable = Convert.ToInt32(10 / salaryFactor);
                    insertSql.AppendFormat("{0},{1},{2},{3}\n"
                        , jobCountry.JobCodeId,
                        jobCountry.CountryId,
                        jobCountry.Salary,
                        jobCountry.QuantityAvailable
                        );
                }
            }
            System.IO.File.WriteAllText(@"C:\Home\CodeBack\PlanetGeni\DAL\DbScripts\Data\JobCountry.csv", insertSql.ToString());


        }
        ////[Test]
        public void AddMoreJobCode()
        {

            Random rnd = new Random();
            StoredProcedureExtender spContext = new StoredProcedureExtender();
            string getJobCode = "Select * from JobCode";
            List<JobCode> jobCodes = spContext.GetSqlData<JobCode>(getJobCode).ToList();
            string deleteJobCode = "Delete from JobCode";
            spContext.ExecuteSql(deleteJobCode);
            short count = 0;
            foreach (var item in jobCodes)
            {
                for (int k = 0; k <= 1; k++)
                {
                    if (k == 0)
                    {
                        item.JobType = "F";
                        item.MaxHPW = (sbyte)rnd.Next(32, 50);
                    }
                    else
                    {
                        item.JobType = "P";
                        item.MaxHPW = (sbyte)rnd.Next(16, 24);

                    }

                    for (sbyte j = 0; j <= 1; j++)
                    {
                        item.ReqDegreeId = (sbyte)rnd.Next(1, 5);
                        for (short i = 0; i <= 2; i++)
                        {
                            item.JobExperience = (short)rnd.Next(0, 10);
                            item.IndustryExperience = (short)rnd.Next(0, item.JobExperience);
                            item.JobCodeId = ++count;
                            item.MinimumMatchScore = (item.ReqMajorId * 10 + item.ReqDegreeId * 15 + (item.IndustryExperience + 1) * 5 + (item.JobExperience + 1) * 5 / 100);
                            item.Duration = (short)rnd.Next(40, item.MinimumMatchScore * 10 + 40);
                            item.CheckInDuration = (sbyte)rnd.Next(0, 2);
                            if (item.CheckInDuration > 0)
                            {
                                item.OverTimeRate = (decimal)rnd.Next(0, item.MinimumMatchScore / 10);
                            }
                            spContext.Add(item);

                        }
                    }
                }
            }

        }
        ////[Test]
        [Repeat(1)]
        public void UpdateCountryBudget()
        {
            CountryBudget budget = new CountryBudget();
            budget.CountryId = "us";
            budget.StartDate = DateTime.Now;
            budget.EndDate = DateTime.Now.AddDays(3);
            budget.TotalAmount = 200000000;
            budget.TaskId = Guid.NewGuid();

            Assert.AreEqual(sp.Update(budget), 1);


        }
        ////[Test]
        [Repeat(1000)]
        public void AddUpdateCountryBudget()
        {
            CountryBudget budget = new CountryBudget();
            budget.CountryId = "us";
            budget.StartDate = DateTime.Now;
            budget.EndDate = DateTime.Now.AddDays(3);
            budget.TotalAmount = 300000000;
            budget.TaskId = Guid.NewGuid();

            Assert.AreEqual(2, sp.AddUpdate(budget));


        }
        ////[Test]
        [Repeat(1)]
        public void AddWelcomePost()
        {
            IPartyDTORepository partyRepo = new PartyDTORepository();
            PartyInvite invite = new PartyInvite
            {
                TaskId = new Guid("3c2e78b9-2de7-4473-a479-595cbcf2e5c1"),
                PartyId = new Guid("51c2c32f-e010-4e74-bbe8-9282e2b76a8d"),
                UserId = 1,
                MemberType = "M",
                InvitationDate = Convert.ToDateTime("2014-10-12 04:50:36"),
                Status = "A"
            };
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmPartyId", "51c2c32f-e010-4e74-bbe8-9282e2b76a8d");
            PoliticalParty partyinfo =
                sp.GetByPrimaryKey<PoliticalParty>(dictionary);

            partyRepo.AddWelcomePost(invite, partyinfo);
        }
        ////[Test]
        [TestCase("C:\\Home\\CodeBack\\PlanetGeni\\PlanetWeb\\Content\\Images\\pic")]
        public void UploadallDummyProfilePic(string folderName)
        {
            AzureStorage.InitializeAccountPropeties();
            AzureStorage.UploadAllFodlerContent(folderName);
        }
        //[Test]
        [Repeat(1)]
        public void AddEmailNotfication()
        {
            IUserNotificationDetailsDTORepository usernotif = new UserNotificationDetailsDTORepository();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            WebUserDTO webUser = webRepo.GetUserPicFName(1);
            StringBuilder notificationparmText = new StringBuilder();

            usernotif.AddNotification(false, string.Empty, 111, "Rojan Ulak", 1, 1);

            usernotif.AddNotification(false, string.Empty, 125, "", 1, 1);

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
1, "Planet Geni",
                      Guid.NewGuid(), "FlashPoint", Guid.NewGuid()
                      );
            usernotif.AddNotification(true, Guid.NewGuid().ToString(),
                 AppSettings.RunForOfficePartyVotingRequestNotificationId,
                 notificationparmText.ToString(), 7, 1);
            notificationparmText.Clear();

            notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
           1,
           webUser.Picture,
           webUser.FullName,
           "Pick3",
           "50000",
          3
           );

            usernotif.AddNotification(false, string.Empty,
      AppSettings.LotteryWinNotificationId, notificationparmText.ToString(), 1, 1);
            notificationparmText.Clear();

            notificationparmText.AppendFormat("<strong>Date:{0}</strong>|{1}|{2}|{3}|<strong>Date:{4}</strong>",
             DateTime.Now,
"Approved", 10, 1, DateTime.Now);

            usernotif.AddNotification(false, string.Empty,
     AppSettings.RunForOfficeResultNotificationId, notificationparmText.ToString(), 8, 1);
            notificationparmText.Clear();

            System.Threading.Thread.Sleep(5000);
        }
    }
}
