using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
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
    [Category("BankAc")]
    public class BankAcUnitTest
    {
        private static string Category = "BankAc";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.bankAc"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IPartyDTORepository partyRepo = new PartyDTORepository();
        private IAdvertisementDetailsDTORepository bankAcRepo = new AdvertisementDetailsDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        private UnitTestFixture setupFixture;
        public BankAcUnitTest()
        {
            string[] createtables = new string[] { "CountryTax", "Stock", "UserLoan", "Merchandise" };
            setupFixture = new UnitTestFixture(spContext, createtables);
        }
        [TestFixtureSetUp]
        public void Init()
        {
           // //return;

            setupFixture.BootStrapDb();
            setupFixture.LoadDataTable();


        }
        [SetUp]
        public void SetupTestData()
        {
            string dataLobankAcqlpath = @"\Sql\DataLoad" + Category + ".sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
                + dataLobankAcqlpath));

            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());
            cache.FlushAllDatabase();
        }

        #region BuyMetal
        [Category("BankAc")]
        [Test]
        [TestCase(1, 50.0, 10.0, 270000, "B", "Pass", "")]
        [TestCase(1, 0, 10.0, 20000, "B", "Pass", "")]
        [TestCase(1, 10, 0.0, 50000, "B", "Pass", "")]
        [TestCase(1, 0, 0.0, 0, "B", "Fail", "")]
        [TestCase(1, -1, -1, 0, "B", "Fail", "")]
        [TestCase(1, 10000, 5000, 0, "B", "Fail", "")]

        [TestCase(1, -50.0, -10.0, -270000, "S", "Pass", "")]
        [TestCase(1, -10, 0.0,-50000, "S", "Pass", "")]
        [TestCase(1, 0, -10.0, -20000, "S", "Pass", "")]
        [TestCase(1, 0, 0.0, 0, "S", "Fail", "")]
        [TestCase(1, 1, 1, 0, "S", "Fail", "")]
        [TestCase(1,-10000, -5000, 0, "S", "Fail", "")]

        public void BuySellMetal(int userId, decimal goldDelta, decimal silverDelta,
            decimal cashdiff, string orderType, string expectedResult, string failMsg)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            BuySellMetalDTO buysellMetal = new BuySellMetalDTO
            {
                UserId = userId,
                OrderType = orderType,
                GoldDelta = goldDelta,
                SilverDelta = silverDelta
            };

            BankAccountManager manager = new BankAccountManager();
            UserBankAccount oldbankac = bankRepo.GetUserBankDetails(buysellMetal.UserId);
            manager.ProcessBuySellMetal(buysellMetal);

            buysellMetal.Delta = cashdiff;
            if (expectedResult == "Pass")
            {
                CheckBankAcResult(oldbankac, buysellMetal);
            }
            else
            {
                CheckBankAcResultFail(buysellMetal, oldbankac, failMsg);
            }

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
        }

        public void CheckBankAcResult(UserBankAccount oldbankac, BuySellMetalDTO buysellMetal)
        {
            setupFixture.CheckUserBankAccount(oldbankac, -buysellMetal.Delta,
                buysellMetal.GoldDelta, buysellMetal.SilverDelta);
            string orderType = buysellMetal.OrderType == "B" ? "Buy" : "Sell";
            string msg = string.Format("{0}|{1}|{2}",
                     orderType, buysellMetal.GoldDelta, buysellMetal.SilverDelta);
            setupFixture.CheckUserNotification(new int[] { buysellMetal.UserId }, new string[] { msg },
     AppSettings.BuySellMetalSuccessNotificationId, false, 1, 1);

            if (buysellMetal.OrderType == "B")
            {
                setupFixture.CheckCapitalTransactionLog(buysellMetal.UserId, 0
               , Math.Abs(buysellMetal.Delta), 0, AppSettings.MetalFundType, 1);
            }
            else
            {
                setupFixture.CheckCapitalTransactionLog(0, buysellMetal.UserId
               , Math.Abs(buysellMetal.Delta), 0, AppSettings.MetalFundType, 1);
            }
        }
        public void CheckBankAcResultFail(BuySellMetalDTO buysellMetal, UserBankAccount oldbankac, string failMsg)
        {
            setupFixture.CheckUserBankAccount(oldbankac, 0, 0, 0);
            string orderType = buysellMetal.OrderType == "B" ? "Buy" : "Sell";
            string msg = string.Format("{0}|{1}|{2}|{3}",
                     orderType, buysellMetal.GoldDelta, buysellMetal.SilverDelta, failMsg);
            setupFixture.CheckUserNotification(new int[] { buysellMetal.UserId }, new string[] { msg },
                    AppSettings.BuySellMetalFailNotificationId, false, 1, 1);
            setupFixture.CheckCapitalTransactionLog(buysellMetal.UserId, 0
    , Math.Abs(buysellMetal.Delta), 0, AppSettings.MetalFundType, 0);

        }

        #endregion BuyMetal

    }
}
