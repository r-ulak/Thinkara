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
    // [TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(ChromeDriver))]
    [Category("Loan")]
    public class LoanUnitTestWA<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;
        private readonly int waitTime = 10;
        private StringBuilder verificationErrors;
        private string baseURL = ConfigurationManager.AppSettings["baseurl"];
        private string partyPic = ConfigurationManager.AppSettings["pic.capture"];
        private static string Category = "Loan";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.loan"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);

        private IEducationDTORepository educationRepo = new EducationDTORepository();
        private IJobDTORepository jobRepo = new JobDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private UnitTestFixture setupFixture;
        public LoanUnitTestWA()
        {
            string[] createtables = new string[] { "Stock", "Merchandise", "UserLoan", "CountryTax" };
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
        [TestCase("sjohnston@gigabox.com", "UnitTest_24!", "vmcdonald@eabox.com")]
        public void RequestLoan(string emailId, string password, string lenderemailId)
        
           
        {
            setupFixture.Login(emailId, password, "bank", waitTime, driver);
            setupFixture.WaitFortextPreset(By.LinkText("Request Loan"), waitTime, driver, "Request Loan");
            driver.FindElement(By.LinkText("Request Loan")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-6 > span.pull-right"), waitTime, driver, "19.00");
            driver.FindElement(By.Id("radiolendorFriend")).Click();
            driver.FindElement(By.Id("dduserloanFriends")).Click();
            driver.FindElement(By.XPath("//div[@id='dduserloanFriends']/ul/li[3]/a")).Click();
            driver.FindElement(By.Id("loanAmount")).Clear();
            driver.FindElement(By.Id("loanAmount")).SendKeys("10000");
            driver.FindElement(By.Id("loanPercent")).Clear();
            driver.FindElement(By.Id("loanPercent")).SendKeys("19.00");

            driver.FindElement(By.Id("saveLoanRequest")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.panel-body > div.col-xs-12 > span"), waitTime, driver, "Loan Request Successfully Submitted");

            driver.FindElement(By.CssSelector("i.fa.fa-caret-down")).Click();
            setupFixture.WaitFortextPreset(By.Id("logoff"), waitTime, driver, "Log off");
            driver.FindElement(By.Id("logoff")).Click();
                      
           setupFixture.Login(lenderemailId, password, "task", waitTime, driver);
           setupFixture.WaitFortextPreset(By.CssSelector("div.taskdetails > span"), waitTime, driver, "Loan Request");
           driver.FindElement(By.XPath("//div[@id='myuserTaskcontent-box']/div[2]/ul/li/div/a/i")).Click();
           setupFixture.WaitFortextPreset(By.XPath("//div[@id='myuserVotingcontent-box']/div[2]/fieldset[3]/ul/li[2]/span"), waitTime, driver,"Deny");
           driver.FindElement(By.Name("vote")).Click();
           driver.FindElement(By.Id("saveVoteResponse")).Click();
           setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-12 > span"), waitTime, driver, "Vote Successfully Submitted");
           driver.FindElement(By.CssSelector("i.fa.fa-caret-down")).Click();
           setupFixture.WaitFortextPreset(By.Id("logoff"), waitTime, driver, "Log off");
           driver.FindElement(By.Id("logoff")).Click();

           setupFixture.Login(emailId, password, "bank", waitTime, driver);
           setupFixture.WaitFortextPreset(By.XPath("//div[@id='loanviewcontent-wrapper']/div/div/div[2]/div/div/div[2]/span"), waitTime, driver, "11,900.00");
           driver.FindElement(By.CssSelector("i.fa.fa-caret-down")).Click();
           setupFixture.WaitFortextPreset(By.Id("logoff"), waitTime, driver, "Log off");
           driver.FindElement(By.Id("logoff")).Click();

        }
        
        
        [Test]
        [TestCase("sjohnston@gigabox.com", "UnitTest_24!", 10000)]
        [TestCase("vmcdonald@eabox.com", "UnitTest_24!", 1000)]
        public void RequestLoanBank(string emailId, string password, decimal loanAmount)
        {
            setupFixture.Login(emailId, password, "bank", waitTime, driver);
            setupFixture.WaitFortextPreset(By.LinkText("Request Loan"), waitTime, driver, "Request Loan");
            driver.FindElement(By.LinkText("Request Loan")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("span.input-group-addon.bootstrap-touchspin-prefix"), waitTime, driver, "Loan Me");
            driver.FindElement(By.Id("radiolendorBank")).Click();
            driver.FindElement(By.Id("loanAmount")).Clear();
            driver.FindElement(By.Id("loanAmount")).SendKeys(loanAmount.ToString());
             driver.FindElement(By.Id("saveLoanRequest")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.panel-body > div.col-xs-12 > span"),waitTime, driver, "Loan Request Successfully Submitted");
            driver.FindElement(By.Id("notifbadge")).Click();
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='myuserNotificationcontent-box']/div[2]/ul/li/div[2]/div/span[2]"), waitTime, driver, "Loan Request");
            driver.FindElement(By.Id("backbtn")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//a[@id='nav-bank']/i"), waitTime, driver);
            Thread.Sleep(2000);

            driver.FindElement(By.XPath("//a[@id='nav-bank']/i")).Click();
            setupFixture.WaitFortextPreset(By.LinkText("My Loans"), waitTime, driver, "My Loans");

     }
        [Test]
        [TestCase("sjohnston@gigabox.com", "UnitTest_24!", 10000, "//div[@id='loanviewcontent-wrapper']/div/div/div/div[2]/button")]
        [TestCase("sjohnston@gigabox.com", "UnitTest_24!", 10000, "//div[@id='loanviewcontent-wrapper']/div[2]/div/div/div[2]/button")]
      
        public void PayLoan(string emailId, string password, decimal payloanAmount, string payLoadselector)
        {
            setupFixture.Login(emailId, password, "bank", waitTime, driver);
            setupFixture.WaitFortextPreset(By.LinkText("My Loans"), waitTime, driver, "My Loans");
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='loanviewfooter']/div/div[2]/div[2]/div/span"), waitTime, driver, "10");

            driver.FindElement(By.XPath(payLoadselector)).Click();
            setupFixture.WaitForElementPreset(By.Id("radiopayOther"), waitTime, driver);
            Thread.Sleep(2000);
            driver.FindElement(By.Id("radiopayOther")).Click();
            driver.FindElement(By.Id("payloanAmount")).Clear();
            driver.FindElement(By.Id("payloanAmount")).Click();
            driver.FindElement(By.Id("payloanAmount")).SendKeys(payloanAmount.ToString());
            Thread.Sleep(2000);
            driver.FindElement(By.Id("payloan")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("#mypayLoancontent-submit > div.panel-body > div.col-xs-12 > span"), waitTime, driver, "Loan Payment Successfully Submitted");
            driver.FindElement(By.CssSelector("i.fa.fa-caret-down")).Click();
            setupFixture.WaitFortextPreset(By.Id("logoff"), waitTime, driver, "Log off");
            driver.FindElement(By.Id("logoff")).Click();

        }


        [Test]
        [TestCase("sjohnston@gigabox.com", "UnitTest_24!")]
        public void LoadMoreLoanTest (string emailId, string password)
        {
            setupFixture.Login(emailId, password, "bank", waitTime, driver);
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='loanviewfooter']/div/div[2]/div[2]/div/span"), waitTime, driver, "10");
            driver.FindElement(By.CssSelector("i.fa.fa-plus")).Click();
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='loanviewfooter']/div/div[2]/div[2]/div/span"), waitTime, driver, "20");

        }
    }
}
