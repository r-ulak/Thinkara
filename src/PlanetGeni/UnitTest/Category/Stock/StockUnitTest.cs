using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
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
    [Category("Stock")]
    public class StockUnitTest
    {
        private static string Category = "Stock";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.stock"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IRedisCacheProvider redisCache = new RedisCacheProvider(AppSettings.RedisDatabaseId);
        private IPartyDTORepository partyRepo = new PartyDTORepository();
        private IAdvertisementDetailsDTORepository stockRepo = new AdvertisementDetailsDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        private UnitTestFixture setupFixture;
        public StockUnitTest()
        {
            string[] createtables = new string[] { "CountryTax", Category, "PoliticalParty" };
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
            string dataLostockqlpath = @"\Sql\DataLoad" + Category + ".sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
                + dataLostockqlpath));

            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());
            cache.FlushAllDatabase();
        }

        [Test]
        //[Repeat(5)]
        [TestCase("1", new int[] { 1, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21 }, 105, 106, 3, 18
            , 41.05, 20, 41.05, 20.05, 95.48, "2015-08-07")]
        [TestCase("2", new int[] { 1, 6, 19, 23, 5, 18, 9, 21, 7, 8, 3, 4, 20 }, 105, 106, 3, 18
            , 41.14, 20, 41.14, 20.14, 95.91, "2015-08-07"
            )]
        public void StockBrokerTest(string fileId, int[] userids, sbyte successnotificationTypeId, sbyte failnotificationTypeId, int failnotifCount, int successnotifCount, decimal historyprice, decimal prevPrice, decimal curPrice, decimal dayChange, decimal daychangePercent, string date)
        {

            setupFixture.LoadDataTable("Stock-x", rootFolderCategory, fileId);
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            AppSettings.MaxCountryId = 0;
            StockBrokerManager manager = new StockBrokerManager();
            manager.ProcessAll(date).ToString();

            CheckStockResults(fileId, userids, successnotificationTypeId, failnotificationTypeId, failnotifCount, successnotifCount,
                 historyprice, prevPrice, curPrice, dayChange, daychangePercent
                );

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 45, "Should take less than  45 seconds");
        }

        private void CheckStockResults(string fileId, int[] userids, sbyte successnotificationTypeId, sbyte failnotificationTypeId, int failnotifCount, int successnotifCount,
         decimal historyprice, decimal prevPrice, decimal curPrice, decimal dayChange, decimal daychangePercent
            )
        {
            CheckStock(prevPrice, curPrice, dayChange, daychangePercent);
            CheckStockHistory(historyprice);
            List<UserStock> expecteduserStocks = new List<UserStock>();
            GetExpectedUserStock(expecteduserStocks, fileId);
            List<StockTrade> expectedStockTrade = new List<StockTrade>();
            GetExpectedStockTrade(expectedStockTrade, fileId);
            List<UserBankAccount> expectedBankAcs = new List<UserBankAccount>();
            GetExpecteduserBankAccount(expectedBankAcs, fileId);
            List<StockTradeHistory> expectedtradehistory = new List<StockTradeHistory>();
            GetExpectedStockTradeHistory(expectedtradehistory, fileId);

            string getUserStockSql = "Select * from UserStock order by UserId desc, PurchasedUnit, PurchasedPrice";
            List<UserStock> actualuserStock = spContext.GetSqlData<UserStock>(getUserStockSql).ToList();

            string getStockTradeSql = "Select * from StockTrade order by UpdatedAt desc, UserId, LeftUnit,InitialUnit, TradeType, Status";
            List<StockTrade> actualStockTrade = spContext.GetSqlData<StockTrade>(getStockTradeSql).ToList();


            string getbankAcSql = "Select * from UserBankAccount order by UserId ";
            List<UserBankAccount> actualBankAcs = spContext.GetSqlData<UserBankAccount>(getbankAcSql).ToList();


            string gettradehistorySql = "Select * from StockTradeHistory Order by BuyerId, SellerId, StockId, Unit, DealPrice";
            List<StockTradeHistory> actualTradeHistory = spContext.GetSqlData<StockTradeHistory>(gettradehistorySql).ToList();



            Assert.AreEqual(expectedtradehistory.Count, actualTradeHistory.Count);
            for (int i = 0; i < expectedtradehistory.Count; i++)
            {
                Assert.AreEqual(expectedtradehistory[i].BuyerId, actualTradeHistory[i].BuyerId);
                Assert.AreEqual(expectedtradehistory[i].SellerId, actualTradeHistory[i].SellerId);
                Assert.AreEqual(expectedtradehistory[i].StockId, actualTradeHistory[i].StockId);
                Assert.AreEqual(expectedtradehistory[i].Unit, actualTradeHistory[i].Unit);
                Assert.AreEqual(expectedtradehistory[i].DealPrice, actualTradeHistory[i].DealPrice);
            }


            Assert.AreEqual(expecteduserStocks.Count, actualuserStock.Count);
            for (int i = 0; i < expecteduserStocks.Count; i++)
            {
                Assert.AreEqual(expecteduserStocks[i].UserId, actualuserStock[i].UserId);
                Assert.AreEqual(expecteduserStocks[i].StockId, actualuserStock[i].StockId);
                Assert.AreEqual(expecteduserStocks[i].PurchasedUnit, actualuserStock[i].PurchasedUnit);
                Assert.AreEqual(expecteduserStocks[i].PurchasedPrice, actualuserStock[i].PurchasedPrice);
            }

            Assert.AreEqual(expectedStockTrade.Count, actualStockTrade.Count);
            for (int i = 0; i < expectedStockTrade.Count; i++)
            {
                Assert.AreEqual(expectedStockTrade[i].TradeId, actualStockTrade[i].TradeId);
                Assert.AreEqual(expectedStockTrade[i].UserId, actualStockTrade[i].UserId);
                Assert.AreEqual(expectedStockTrade[i].StockId, actualStockTrade[i].StockId);
                Assert.AreEqual(expectedStockTrade[i].LeftUnit, actualStockTrade[i].LeftUnit);
                Assert.AreEqual(expectedStockTrade[i].InitialUnit, actualStockTrade[i].InitialUnit);
                Assert.AreEqual(expectedStockTrade[i].Status, actualStockTrade[i].Status);
                Assert.AreEqual(expectedStockTrade[i].OrderType, actualStockTrade[i].OrderType);
                Assert.AreEqual(expectedStockTrade[i].TradeType, actualStockTrade[i].TradeType);
            }

            Assert.AreEqual(expectedBankAcs.Count, actualBankAcs.Count);
            for (int i = 0; i < expectedBankAcs.Count; i++)
            {
                Assert.AreEqual(expectedBankAcs[i].UserId, actualBankAcs[i].UserId);
                Assert.AreEqual(expectedBankAcs[i].Cash, actualBankAcs[i].Cash);
            }
            setupFixture.CheckUserNotification(userids, new string[0], successnotificationTypeId, false, 0, successnotifCount);
            setupFixture.CheckUserNotification(userids, new string[0], failnotificationTypeId, false, 0, failnotifCount);
        }

        private void GetExpectedUserStock(List<UserStock> userStocks, string fileId)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\"; String file_name = dataPath + "ExpectedUserStock-" + fileId + ".csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                UserStock stock = new UserStock();
                stock.UserId = Convert.ToInt32(tokens[0]);
                stock.StockId = Convert.ToInt16(tokens[1]);
                stock.PurchasedUnit = Convert.ToInt32(tokens[2]);
                stock.PurchasedPrice = Convert.ToInt32(tokens[3]);
                userStocks.Add(stock);
            }
            sr.Close();
        }
        private void GetExpectedStockTrade(List<StockTrade> stocktrades, string fileId)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\"; String file_name = dataPath + "ExpectedStockTrade-" + fileId + ".csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                StockTrade stock = new StockTrade();
                stock.TradeId = new Guid(tokens[0]);
                stock.UserId = Convert.ToInt32(tokens[2]);
                stock.StockId = Convert.ToInt16(tokens[3]);
                stock.LeftUnit = Convert.ToInt32(tokens[4]);
                stock.InitialUnit = Convert.ToInt32(tokens[5]);
                stock.Status = (tokens[8]);
                stock.OrderType = (tokens[9]);
                stock.TradeType = (tokens[10]);

                stocktrades.Add(stock);
            }
            sr.Close();
        }

        private void GetExpecteduserBankAccount(List<UserBankAccount> bankacs, string fileId)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\"; String file_name = dataPath + "ExpectedUserBankAccount-" + fileId + ".csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                UserBankAccount bankac = new UserBankAccount();
                bankac.UserId = Convert.ToInt32(tokens[0]);
                bankac.Cash = Convert.ToDecimal(tokens[1]);
                bankacs.Add(bankac);
            }
            sr.Close();
        }
        private void GetExpectedStockTradeHistory(List<StockTradeHistory> tradehistory, string fileId)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\"; String file_name = dataPath + "ExpectedStockTradeHistory-" + fileId + ".csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                StockTradeHistory trades = new StockTradeHistory();
                trades.BuyerId = Convert.ToInt32(tokens[0]);
                trades.SellerId = Convert.ToInt32(tokens[1]);
                trades.StockId = Convert.ToInt16(tokens[2]);
                trades.Unit = Convert.ToInt32(tokens[3]);
                trades.DealPrice = Convert.ToDecimal(tokens[4]);
                tradehistory.Add(trades);
            }
            sr.Close();
        }

        private void CheckStock(decimal prevPrice, decimal curPrice, decimal dayChange, decimal daychangePercent)
        {
            string getStockSql = "Select * from Stock Where StockId =1";
            List<Stock> stockPrice = spContext.GetSqlData<Stock>(getStockSql).ToList();
            Assert.AreEqual(stockPrice[0].PreviousDayValue, prevPrice);
            Assert.AreEqual(stockPrice[0].CurrentValue, curPrice);
            Assert.AreEqual(stockPrice[0].DayChange, dayChange);
            Assert.AreEqual(stockPrice[0].DayChangePercent, daychangePercent);
        }

        private void CheckStockHistory(decimal curPrice)
        {
            string getStockhistorySql = "Select * from StockHistory Where StockId =1";
            List<StockHistory> stockHistory = spContext.GetSqlData<StockHistory>(getStockhistorySql).ToList();
            Assert.AreEqual(stockHistory[0].CurrentValue, curPrice);
        }
    }
}
