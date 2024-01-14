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
    [Category("Profile")]
    public class ProfileUnitTestWA<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;
        private readonly int waitTime = 10;
        private StringBuilder verificationErrors;
        private string baseURL = ConfigurationManager.AppSettings["baseurl"];
        private string partyPic = ConfigurationManager.AppSettings["pic.capture"];
        private static string Category = "Profile";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.profile"];
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
            string[] createtables = new string[] { "PoliticalParty", "Stock", "UserLoan", "Merchandise", "Job", "Education", "CountryBudget", "CountryTax", "Security" };
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
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 3, 5, 17, 5, 2, "-24,581,554.68", 107, 12, 10, 5303)]
        [TestCase(1013, "kgardner@mydo.name", "UnitTest_24!", 0, 0, 0, 0, 0, "1,564,286.99", 0, 0, 10, 1690)]
        public void CheckUserProfile(int userId, string emailId, string password, int partyCount, int jobCount, int merchandisecount, int degreeCount, int friendsCount, string netWorth, int totalproperty, int totalDegrees, int totalawards, int userLevel)
        {
            setupFixture.Login(emailId, password, "profile", waitTime, driver);
            setupFixture.WaitFortextPreset(By.CssSelector("h4 > strong"), waitTime, driver, netWorth);
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='profilestat']/div[2]/h4/strong"), waitTime, driver, totalproperty.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='profilestat']/div[3]/h4/strong"), waitTime, driver, totalDegrees.ToString());

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='profilestat']/div[4]/h4/strong"), waitTime, driver, totalawards.ToString());
            setupFixture.WaitFortextPreset(By.CssSelector("span.fontsize90"), waitTime, driver, userLevel.ToString());

            setupFixture.WaitForElementPreset(By.XPath("(//button[@type='button'])[2]"), waitTime, driver);

            setupFixture.WaitForElementPreset(By.LinkText("Party MemberShip"), waitTime, driver);
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Party MemberShip")).Click();
            if (partyCount > 0)
            {
                setupFixture.WaitFortextPreset(By.XPath("//div[@id='profileAccordion']/div[2]/div/div/span"), waitTime, driver, partyCount.ToString());
            }

            setupFixture.WaitForElementPreset(By.LinkText("Employment"), waitTime, driver);
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Employment")).Click();
            if (jobCount > 0)
            {
                setupFixture.WaitFortextPreset(By.XPath("//div[@id='profileAccordion']/div[3]/div/div/span"), waitTime, driver, jobCount.ToString());

            }

            setupFixture.WaitForElementPreset(By.LinkText("Property"), waitTime, driver);
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Property")).Click();
            if (merchandisecount > 0)
            {
                setupFixture.WaitFortextPreset(By.XPath("//div[@id='profileAccordion']/div[4]/div/div/span"), waitTime, driver, merchandisecount.ToString());
            }



            setupFixture.WaitForElementPreset(By.LinkText("Education"), waitTime, driver);
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Education")).Click();
            if (degreeCount > 0)
            {
                setupFixture.WaitFortextPreset(By.XPath("//div[@id='profileAccordion']/div[5]/div/div/span"), waitTime, driver, degreeCount.ToString());
            }


            setupFixture.WaitForElementPreset(By.LinkText("Friends"), waitTime, driver);
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Friends")).Click();
            if (friendsCount > 0)
            {
                setupFixture.WaitFortextPreset(By.XPath("//div[@id='profileAccordion']/div[6]/div/div/span"), waitTime, driver, friendsCount.ToString());
            }


        }

        [Test]
        [TestCase(63, "nparker@thoughtworks.name", "UnitTest_24!", 8, 8, 7, 16, 14, "100,000.00", "2nd", 853, "1st")]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 0, 0, 7, 0, 0, "0.00", "1st", 0, "0th")]
        public void CheckCountryProfile(int userId, string emailId, string password, int budgetCount, int revenueCount, int rankingcount, int securityAssetCount, int leadersCount, string budgetWorth, string populationRank, int assetCount, string richest)
        {
            setupFixture.Login(emailId, password, "country", waitTime, driver);
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='countryProfileview-box']/div[2]/div/div[2]/div/div[2]/h4/strong"), waitTime, driver, populationRank.ToString());
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='countryProfileview-box']/div[2]/div/div[2]/div/div[3]/h4/strong"), waitTime, driver, assetCount.ToString());
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='countryProfileview-box']/div[2]/div/div[2]/div/div[4]/h4/strong"), waitTime, driver, richest);
            if (budgetCount > 0)
            {
                setupFixture.WaitFortextPreset(By.CssSelector("div.pull-right > span.text-right"), waitTime, driver, budgetCount.ToString());
            }



            setupFixture.WaitForElementPreset(By.LinkText("Revenue Sources"), waitTime, driver);
            driver.FindElement(By.LinkText("Revenue Sources")).Click();
            if (revenueCount > 0)
            {
                setupFixture.WaitFortextPreset(By.CssSelector("div.panel-heading.active > div.pull-right > span.text-right"), waitTime, driver, revenueCount.ToString());
            }


            Thread.Sleep(1000);
            setupFixture.WaitForElementPreset(By.LinkText("Ranking"), waitTime, driver);
            driver.FindElement(By.LinkText("Ranking")).Click();
            if (rankingcount > 0)
            {
                setupFixture.WaitFortextPreset(By.CssSelector("div.panel-heading.active > div.pull-right > span.text-right"), waitTime, driver, rankingcount.ToString());
            }


            setupFixture.WaitForElementPreset(By.LinkText("Security Asset"), waitTime, driver);
            driver.FindElement(By.LinkText("Security Asset")).Click();
            if (securityAssetCount > 0)
            {
                setupFixture.WaitFortextPreset(By.CssSelector("div.panel-heading.active > div.pull-right > span.text-right"), waitTime, driver, securityAssetCount.ToString());
            }


            setupFixture.WaitForElementPreset(By.LinkText("Leaders"), waitTime, driver);
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Leaders")).Click();
            if (leadersCount > 0)
            {
                setupFixture.WaitFortextPreset(By.CssSelector("div.panel-heading.active > div.pull-right > span.text-right"), waitTime, driver, leadersCount.ToString());
            }


        }


    }
}
