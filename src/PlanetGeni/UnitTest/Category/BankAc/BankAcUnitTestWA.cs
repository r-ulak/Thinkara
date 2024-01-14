using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Configuration;
using DAO;
using DataCache;
using System.IO;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using DAO.Models;
using System.Collections.Generic;
using System.Linq;
using Common;
using Repository;
using OpenQA.Selenium.Interactions;
using DTO.Db;

namespace UnitTest.Category
{
    [TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(ChromeDriver))]
    [Category("BankAc")]
    public class BankAcUnitTestWA<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;
        private readonly int waitTime = 10;
        private StringBuilder verificationErrors;
        private string baseURL = ConfigurationManager.AppSettings["baseurl"];
        private string partyPic = ConfigurationManager.AppSettings["pic.capture"];
        private static string Category = "BankAc";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.bankAc"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IRedisCacheProvider redisCache = new RedisCacheProvider(AppSettings.RedisDatabaseId);
        private IPartyDTORepository partyRepo = new PartyDTORepository();
        private IAdvertisementDetailsDTORepository adsRepo = new AdvertisementDetailsDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private ICountryTaxDetailsDTORepository countrytaxRepo = new CountryTaxDetailsDTORepository();
        private IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        private UnitTestFixture setupFixture;
        [TestFixtureSetUp]
        public void Init()
        {
            string[] createtables = new string[] { "CountryTax", "Stock", "UserLoan", "Merchandise" };
            setupFixture = new UnitTestFixture(spContext, createtables);
            //return;
            setupFixture.BootStrapDb();
            setupFixture.LoadDataTable();
        }
        [SetUp]
        public void SetupTestData()
        {

            setupFixture.LoadDataTable(Category, rootFolderCategory);
            cache.FlushAllDatabase();
            string driversPath = Environment.CurrentDirectory + @"\..\..\Selenium\WebDrivers\";
            if (typeof(TWebDriver) == typeof(FirefoxDriver))
            {
                driver = new TWebDriver();
            }
            else if (typeof(TWebDriver) == typeof(ChromeDriver))
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                driver = new ChromeDriver(driversPath, options);
            }
            else if (typeof(TWebDriver) == typeof(InternetExplorerDriver))
            {
                InternetExplorerOptions options = new InternetExplorerOptions { IgnoreZoomLevel = true };
                driver = new InternetExplorerDriver(driversPath, options);
            }
            else
            {
                driver = Activator.CreateInstance(typeof(TWebDriver), new object[] { driversPath }) as IWebDriver;
            }

            verificationErrors = new StringBuilder();
            if (typeof(TWebDriver) != typeof(ChromeDriver))
                driver.Manage().Window.Maximize();

        }
        [TearDown]
        public void FixtureTearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }
        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "2.60m", "10.00m", "655.00k", "92.00k", "45.40m", "2.65m", "1.69b", 10, 10, 70000, -33000, -4, -10, -5, -4, 107, 15, 16)]
        public void CheckBankAc(int userId, string emailId, string password, string stock, string cash, string gold, string silver, string loanlefttoPay, string loanlefttoCollect, string asset, decimal goldDelta, decimal silverDelta, decimal cashdiff, decimal sellcashdiff, decimal sellgoldDelta, decimal sellsilverDelta, decimal golddiff, decimal silverdiff, sbyte notficationTypeId, int stmtcount, int stmtcountplus)
        {
            setupFixture.Login(emailId, password, "bankac", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("Summary"), waitTime, driver);

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankview-box']/div[2]/div/div/div/div/div/div[3]/span"), waitTime, driver, stock.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankview-box']/div[2]/div/div/div/div[2]/div/div[3]/span"), waitTime, driver, loanlefttoPay.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankview-box']/div[2]/div/div/div/div[3]/div/div[3]/span"), waitTime, driver, loanlefttoCollect.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankview-box']/div[2]/div/div/div/div[4]/div/div[3]/span"), waitTime, driver, asset.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankview-box']/div[2]/div/div/div/div[5]/div/div[3]/span"), waitTime, driver, cash.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankview-box']/div[2]/div/div/div/div[6]/div/div[3]/span"), waitTime, driver, silver.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankview-box']/div[2]/div/div/div/div[7]/div/div[3]/span"), waitTime, driver, gold.ToString());

            BuyMetal(userId, goldDelta, silverDelta, cashdiff, notficationTypeId);
            SellMetal(userId, sellgoldDelta, sellsilverDelta, sellcashdiff, golddiff, silverdiff, notficationTypeId);
            StmtMetal(userId, stmtcount, stmtcountplus);
            RichMetal(userId);
        }

        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 10, 10, 70000, 107)]
        [TestCase(119, "mcunningham@dynabox.org", "UnitTest_24!", 10, 0, 50000, 107)]
        public void BuyMetalLogin(int userId, string emailId, string password, decimal goldDelta, decimal silverDelta, decimal cashdiff, sbyte notficationTypeId)
        {
            setupFixture.Login(emailId, password, "bankac", waitTime, driver);
            BuyMetal(userId, goldDelta, silverDelta, cashdiff, notficationTypeId);
        }
        private void BuyMetal(int userId,decimal goldDelta, decimal silverDelta, decimal cashdiff, sbyte notficationTypeId)
        {
            setupFixture.WaitForElementPreset(By.LinkText("Buy Metal"), waitTime, driver);
            driver.FindElement(By.LinkText("Buy Metal")).Click();
            UserBankAccount oldbankac = bankRepo.GetUserBankDetails(userId);
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='bankbuy-box']/div[2]/div/div/div/span"), waitTime, driver);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            if (goldDelta > 0)
            {
                js.ExecuteScript("$('#bankbuy-box').find('.capitalType').eq(0).simpleSlider('setValue'," + goldDelta / 100 + ")");
            }
            if (silverDelta > 0)
            {
                js.ExecuteScript("$('#bankbuy-box .panel-body').scrollTop(1000);");
                js.ExecuteScript("$('#bankbuy-box').find('.capitalType').eq(1).simpleSlider('setValue'," + silverDelta / 100 + ")");
            }
            driver.FindElement(By.Id("buymetal")).Click();
            setupFixture.WaitFortextPreset            (By.CssSelector("div.col-xs-12 > span"), waitTime, driver, "Buy Metal Cart Successfully Submitted");
            setupFixture.WaitForNotificationUpdate(notficationTypeId, userId, 1, waitTime);

            BankAcUnitTest banktest = new BankAcUnitTest();
            BuySellMetalDTO buysellMetal = new BuySellMetalDTO
            {
                UserId = userId,
                OrderType = "B",
                GoldDelta = goldDelta,
                SilverDelta = silverDelta,
                Delta = cashdiff
            };
            banktest.CheckBankAcResult(oldbankac, buysellMetal);
        }

        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", -4, -10, -33000, -5, -4, 107)]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", -4, 0, -25000, -5, 0, 107)]
        public void SellMetalLogin(int userId, string emailId, string password, decimal goldDelta, decimal silverDelta, decimal cashdiff, decimal golddiff, decimal silverdiff, sbyte notficationTypeId)
        {
            setupFixture.Login(emailId, password, "bankac", waitTime, driver);
            SellMetal(userId, goldDelta, silverDelta, cashdiff, golddiff, silverdiff, notficationTypeId);
        }
        private void SellMetal(int userId,  decimal goldDelta, decimal silverDelta, decimal cashdiff, decimal golddiff, decimal silverdiff, sbyte notficationTypeId)
        {
            string sqldelete = string.Format("Delete From UserNotification Where  UserId ={0}", userId);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            spContext.ExecuteSql(sqldelete);
            js.ExecuteScript("$('#main').scrollTop(0);");
            setupFixture.WaitForElementPreset(By.LinkText("Sell Metal"), waitTime, driver);

            driver.FindElement(By.LinkText("Sell Metal")).Click();
            UserBankAccount oldbankac = bankRepo.GetUserBankDetails(userId);
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='banksell-box']/div[2]/div/div/div/span"), waitTime, driver);
            if (goldDelta < 0)
            {
                js.ExecuteScript("$('#banksell-box').find('.capitalType').eq(0).simpleSlider('setValue'," + Math.Abs(goldDelta) / 100 + ")");
            }
            if (silverDelta < 0)
            {
                js.ExecuteScript("$('#banksell-box .panel-body').scrollTop(1000);");
                js.ExecuteScript("$('#banksell-box').find('.capitalType').eq(1).simpleSlider('setValue'," + Math.Abs(silverDelta) / 100 + ")");
            }
            driver.FindElement(By.Id("sellmetal")).Click();
            setupFixture.WaitFortextPreset
            (By.CssSelector("#banksell-submit > div.panel-body > div.col-xs-12 > span"), waitTime, driver, "Sell Metal Cart Successfully Submitted");
            setupFixture.WaitForNotificationUpdate(notficationTypeId, userId, 1, waitTime);


            BankAcUnitTest banktest = new BankAcUnitTest();
            BuySellMetalDTO sellsellMetal = new BuySellMetalDTO
            {
                UserId = userId,
                OrderType = "S",
                GoldDelta = golddiff,
                SilverDelta = silverdiff,
                Delta = cashdiff
            };

            banktest.CheckBankAcResult(oldbankac, sellsellMetal);
        }

        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 14, 14)]
        public void StmtMetalLogin(int userId, string emailId, string password, int count, int countplus)
        {
            setupFixture.Login(emailId, password, "bankac", waitTime, driver);
            StmtMetal(userId, count, countplus);
        }
        private void StmtMetal(int userId,int count, int countplus)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#main').scrollTop(0);");

            setupFixture.WaitForElementPreset(By.LinkText("Transaction"), waitTime, driver);
            driver.FindElement(By.LinkText("Transaction")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("span.label.label-info"), waitTime, driver, count.ToString());

            driver.FindElement(By.Id("bankstmtshowmore")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("span.label.label-info"), waitTime, driver, countplus.ToString());


        }
        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!")]
        public void RichMetalLogin(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "bankac", waitTime, driver);
            RichMetal(userId);
        }
        private void RichMetal(int userId)
        {
            setupFixture.WaitForElementPreset(By.LinkText("Richest"), waitTime, driver);
            driver.FindElement(By.LinkText("Richest")).Click();
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankrich-box']/div[2]/div/fieldset/div/div[2]/div/div[3]/a/span"), waitTime, driver, "Norma James");

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankrich-box']/div[2]/div/fieldset/div/div[2]/div/div[4]/span"), waitTime, driver, "14.87m");

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankrich-box']/div[2]/div/fieldset/div/div[12]/div/div[3]/a/span"), waitTime, driver, "Sarah Kennedy");

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='bankrich-box']/div[2]/div/fieldset/div/div[12]/div/div[4]/span"), waitTime, driver, "12.06m");
        }


    }
}
