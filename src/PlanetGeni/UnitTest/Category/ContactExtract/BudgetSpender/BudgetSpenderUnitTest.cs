using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
using Manager.BudgetSpender;
using Manager.Jobs;
using Manager.ServiceController;
using Newtonsoft.Json;
using NUnit.Framework;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Model;

namespace UnitTest.Category
{
    [TestFixture]
    [Category("BudgetSpender")]
    public class BudgetSpender
    {
        private static string Category = "CountryBudget";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["jobid.matchjob"]);
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.budgetSpender"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private UnitTestFixture setupFixture;
        public BudgetSpender()
        {
            string[] createtables = new string[] { Category, "WebJob", "Education", "Stock", "Job", "Education", "CountryTax", "PoliticalParty" };
            setupFixture = new UnitTestFixture(spContext, createtables);
        }
        [TestFixtureSetUp]
        public void Init()
        {
            //return;

            setupFixture.BootStrapDb();
            setupFixture.LoadDataTable();


        }
        [SetUp]
        public void SetupTestData()
        {
            setupFixture.LoadDataTable(Category, rootFolderCategory);
            cache.FlushAllDatabase();
        }

        [Test]
        [TestCase(new int[] { 97, 486, 815, 1, 8, 11, 12, 13, 15, 17, 246, 319, 547, 690, 63, 135, 176, 245, 272, 332, 339, 421, 445, 495, 571, 611, 691, 762, 819, 838, 940 }, 118, new string[] { "in", "se", "us" }, 22,23)]
        public void RunBudgetSpendor(int[] userIds, short notificationTypeId, string[] countryIds, sbyte postTypeId, sbyte notifyPostTypeId)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            JobsManager jobmanager = new JobsManager(jobId);
            BudgetSpenderManager budgetManager = new BudgetSpenderManager();
            int runId = jobmanager.GetRunId();
            budgetManager.RunBudgetSpendor(runId);

            CheckRunBudgetSpendor(userIds, notificationTypeId, countryIds, postTypeId, notifyPostTypeId);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 60 * 4, "Should take less than  4 minutes");
        }
        private void CheckRunBudgetSpendor(int[] userIds, short notificationTypeId, string[] countryIds, sbyte postTypeId, sbyte notifyPostTypeId)
        {
            List<CountryBudget> expectedcountryBudgets = new List<CountryBudget>();
            GetExpectedCountryBudget(expectedcountryBudgets);

            List<CountryBudgetByType> expectedcountryBudgetTypes = new List<CountryBudgetByType>();
            GetExpectedCountryBudgetByType(expectedcountryBudgetTypes);

            List<StockTrade> expectedstockTrades = new List<StockTrade>();
            GetExpectedStockTrade(expectedstockTrades);

            List<JobCountry> expectedjobCountries = new List<JobCountry>();
            GetExpectedJobCountry(expectedjobCountries);

            List<UserBankAccount> expecteduserbankAccounts = new List<UserBankAccount>();
            GetExpectedUserBankAccount(expecteduserbankAccounts);

            string getcountryBudgetsSql = "select TaskId, CountryId, TotalAmount From CountryBudget Where length(TaskId)>1 Order By  TaskId, CountryId, TotalAmount";
            List<CountryBudget> actualcountryBudgets = spContext.GetSqlData<CountryBudget>(getcountryBudgetsSql).ToList();

            string getcountryBudgetTypesSql = "Select TaskId, Amount, AmountLeft, BudgetType  from CountryBudgetByType  Where length(TaskId)>1 Order By TaskId, Amount, AmountLeft, BudgetType";
            List<CountryBudgetByType> actualcountryBudgetTypes = spContext.GetSqlData<CountryBudgetByType>(getcountryBudgetTypesSql).ToList();

            string getstockTradesSql = "Select UserId, StockId,LeftUnit, InitialUnit, OfferPrice, Status, OrderType, TradeType From StockTrade Order by UserId,StockId";
            List<StockTrade> actualstockTrades = spContext.GetSqlData<StockTrade>(getstockTradesSql).ToList();

            string getjobCountrySql = "Select * from JobCountry Order By JobCodeId, CountryId";
            List<JobCountry> actualJobCountry = spContext.GetSqlData<JobCountry>(getjobCountrySql).ToList();

            string getuserBankAcSql = "Select UserId, Cash, Gold, Silver From UserBankAccount Order By UserId, Cash, Gold, Silver";
            List<UserBankAccount> actualuserBankAccounts = spContext.GetSqlData<UserBankAccount>(getuserBankAcSql).ToList();

            Assert.AreEqual(expecteduserbankAccounts.Count, actualuserBankAccounts.Count);
            for (int i = 0; i < expecteduserbankAccounts.Count; i++)
            {
                Assert.AreEqual(expecteduserbankAccounts[i].UserId, actualuserBankAccounts[i].UserId);
                Assert.AreEqual(expecteduserbankAccounts[i].Cash, actualuserBankAccounts[i].Cash);
                Assert.AreEqual(expecteduserbankAccounts[i].Gold, actualuserBankAccounts[i].Gold);
                Assert.AreEqual(expecteduserbankAccounts[i].Silver, actualuserBankAccounts[i].Silver);
            }

            Assert.AreEqual(expectedjobCountries.Count, actualJobCountry.Count);
            for (int i = 0; i < expectedjobCountries.Count; i++)
            {
                Assert.AreEqual(expectedjobCountries[i].JobCodeId, actualJobCountry[i].JobCodeId);
                Assert.AreEqual(expectedjobCountries[i].CountryId, actualJobCountry[i].CountryId);
                Assert.AreEqual(expectedjobCountries[i].Salary, actualJobCountry[i].Salary);
                Assert.AreEqual(expectedjobCountries[i].QuantityAvailable, actualJobCountry[i].QuantityAvailable);
            }

            Assert.AreEqual(expectedstockTrades.Count, actualstockTrades.Count);
            for (int i = 0; i < expectedstockTrades.Count; i++)
            {
                Assert.AreEqual(expectedstockTrades[i].UserId, actualstockTrades[i].UserId);
                Assert.AreEqual(expectedstockTrades[i].StockId, actualstockTrades[i].StockId);
                Assert.AreEqual(expectedstockTrades[i].LeftUnit, actualstockTrades[i].LeftUnit);
                Assert.AreEqual(expectedstockTrades[i].InitialUnit, actualstockTrades[i].InitialUnit);
                Assert.AreEqual(expectedstockTrades[i].OfferPrice, actualstockTrades[i].OfferPrice);
                Assert.AreEqual(expectedstockTrades[i].Status, actualstockTrades[i].Status);
                Assert.AreEqual(expectedstockTrades[i].OrderType, actualstockTrades[i].OrderType);
                Assert.AreEqual(expectedstockTrades[i].TradeType, actualstockTrades[i].TradeType);
            }

            Assert.AreEqual(expectedcountryBudgetTypes.Count, actualcountryBudgetTypes.Count);
            for (int i = 0; i < expectedcountryBudgetTypes.Count; i++)
            {
                Assert.AreEqual(expectedcountryBudgetTypes[i].TaskId, actualcountryBudgetTypes[i].TaskId);
                Assert.AreEqual(expectedcountryBudgetTypes[i].Amount, actualcountryBudgetTypes[i].Amount);
                Assert.AreEqual(expectedcountryBudgetTypes[i].AmountLeft, actualcountryBudgetTypes[i].AmountLeft);
                Assert.AreEqual(expectedcountryBudgetTypes[i].BudgetType, actualcountryBudgetTypes[i].BudgetType);
            }

            Assert.AreEqual(expectedcountryBudgets.Count, actualcountryBudgets.Count);
            for (int i = 0; i < expectedcountryBudgets.Count; i++)
            {
                Assert.AreEqual(expectedcountryBudgets[i].TaskId, actualcountryBudgets[i].TaskId);
                Assert.AreEqual(expectedcountryBudgets[i].CountryId, actualcountryBudgets[i].CountryId);
                Assert.AreEqual(expectedcountryBudgets[i].TotalAmount, actualcountryBudgets[i].TotalAmount);
            }

            setupFixture.CheckUserNotification(userIds, new string[0], notificationTypeId, false, 0, userIds.Length);
            setupFixture.CheckPost(postTypeId, 1, 3, countryIds, Guid.Empty);
            setupFixture.CheckPost(notifyPostTypeId, 1, 5, countryIds, Guid.Empty);
        }
        private void GetExpectedCountryBudget(List<CountryBudget> countryBudgets)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            String file_name = dataPath + "CountryBudgetExpected.csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                CountryBudget budget = new CountryBudget();
                budget.TaskId = new Guid(tokens[0]);
                budget.CountryId = (tokens[1]);
                budget.TotalAmount = Convert.ToDecimal(tokens[2]);
                countryBudgets.Add(budget);
            }
            sr.Close();
        }
        private void GetExpectedCountryBudgetByType(List<CountryBudgetByType> countryBudgetTypes)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            String file_name = dataPath + "CountryBudgetByTypeExpected.csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                CountryBudgetByType budgetType = new CountryBudgetByType();
                budgetType.TaskId = new Guid(tokens[0]);
                budgetType.Amount = Convert.ToDecimal(tokens[1]);
                budgetType.AmountLeft = Convert.ToDecimal(tokens[2]);
                budgetType.BudgetType = Convert.ToSByte((tokens[3]));
                countryBudgetTypes.Add(budgetType);
            }
            sr.Close();
        }
        private void GetExpectedStockTrade(List<StockTrade> stockTrades)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            String file_name = dataPath + "StockTradeExpected.csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                StockTrade stockTrade = new StockTrade();
                stockTrade.UserId = Convert.ToInt32(tokens[0]);
                stockTrade.StockId = Convert.ToInt16(tokens[1]);
                stockTrade.LeftUnit = Convert.ToInt32(tokens[2]);
                stockTrade.InitialUnit = Convert.ToInt32(tokens[3]);
                stockTrade.OfferPrice = Convert.ToDecimal(tokens[4]);
                stockTrade.Status = (tokens[5]);
                stockTrade.OrderType = (tokens[6]);
                stockTrade.TradeType = (tokens[7]);
                stockTrades.Add(stockTrade);
            }
            sr.Close();
        }
        private void GetExpectedJobCountry(List<JobCountry> jobCountires)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            String file_name = dataPath + "JobCountryExpected.csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                JobCountry jobCountry = new JobCountry();
                jobCountry.JobCodeId = Convert.ToInt16(tokens[0]);
                jobCountry.CountryId = (tokens[1]);
                jobCountry.Salary = Convert.ToDecimal(tokens[2]);
                jobCountry.QuantityAvailable = Convert.ToInt32(tokens[3]);
                jobCountires.Add(jobCountry);
            }
            sr.Close();
        }
        private void GetExpectedUserBankAccount(List<UserBankAccount> userBankAccount)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            String file_name = dataPath + "UserBankAccountExpected.csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                UserBankAccount bankac = new UserBankAccount();
                bankac.UserId = Convert.ToInt32(tokens[0]);
                bankac.Cash = Convert.ToDecimal(tokens[1]);
                bankac.Gold = Convert.ToDecimal(tokens[2]);
                bankac.Silver = Convert.ToDecimal(tokens[3]);
                userBankAccount.Add(bankac);
            }
            sr.Close();
        }

    }
}
