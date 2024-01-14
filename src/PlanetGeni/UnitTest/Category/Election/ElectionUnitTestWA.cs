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

namespace UnitTest.Category
{
    [TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(ChromeDriver))]
    [Category("Election")]
    public class ElectionUnitTestWA<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;
        private readonly int waitTime = 10;
        private StringBuilder verificationErrors;
        private string baseURL = ConfigurationManager.AppSettings["baseurl"];
        private string partyPic = ConfigurationManager.AppSettings["pic.capture"];
        private static string Category = "Election";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.election"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IRedisCacheProvider redisCache = new RedisCacheProvider(AppSettings.RedisDatabaseId);
        private IPartyDTORepository partyRepo = new PartyDTORepository();
        private IElectionDTORepository eleRepo = new ElectionDTORepository();
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
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "I", "Seneator", new int[] { 2, 3, 4 })]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "P", "Seneator", new int[] { 2, 3, 4 })]
        public void RunforOffice(int userId, string emailId, string password, string candidateType, string postionType, int[] agendas)
        {
            setupFixture.Login(emailId, password, "election", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("Run For Office"), waitTime, driver);
            driver.FindElement(By.LinkText("Run For Office")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("div.recipient.col-xs-6 > span"), waitTime, driver);
            setupFixture.WaitForElementPreset(By.Id("radioIndependet"), waitTime, driver);
            driver.FindElement(By.Id("radioIndependet")).Click();
            setupFixture.WaitForElementPreset(By.XPath("(//button[@type='button'])[4]"), waitTime, driver);
            driver.FindElement(By.XPath("(//button[@type='button'])[4]")).Click();
            driver.FindElement(By.XPath("(//input[@name='partyAgenda'])[2]")).Click();
            driver.FindElement(By.XPath("(//input[@name='partyAgenda'])[3]")).Click();
            driver.FindElement(By.XPath("(//input[@name='partyAgenda'])[4]")).Click();

            setupFixture.WaitForElementPreset(By.Id("electionPositions"), waitTime, driver);
            driver.FindElement(By.Id("electionPositions")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Seneator"), waitTime, driver);

            driver.FindElement(By.LinkText("Seneator")).Click();

            if (typeof(TWebDriver) != typeof(InternetExplorerDriver))
            {
                string picpath = (rootFolder + partyPic).Replace(@"\", @"\\");
                driver.FindElement(By.Id("runofficefileupload")).SendKeys(picpath);
            }
            if (candidateType == "P")
            {
                // setupFixture.WaitFortextPreset(By.Id("candidateParty"), waitTime, driver, "FlashPoint");
                driver.FindElement(By.Id("radioParty")).Click();

            }

            driver.FindElement(By.Id("applyelection")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("#electionapply-submit > div.panel-body > div.col-xs-12 > span"), waitTime, driver, "Run For Office Application Successfully Submitted");
            Assert.AreEqual("Run For Office Application Successfully Submitted", driver.FindElement(By.CssSelector("#electionapply-submit > div.panel-body > div.col-xs-12 > span")).Text);
        }

        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!")]
        public void RunforOfficeValidation(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "election", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("Run For Office"), waitTime, driver);
            driver.FindElement(By.LinkText("Run For Office")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("div.recipient.col-xs-6 > span"), waitTime, driver);
            setupFixture.WaitForElementPreset(By.Id("radioIndependet"), waitTime, driver);
            driver.FindElement(By.Id("radioIndependet")).Click();
            setupFixture.WaitForElementPreset(By.XPath("(//button[@type='button'])[2]"), waitTime, driver);


            setupFixture.WaitForElementPreset(By.Id("electionPositions"), waitTime, driver);


            driver.FindElement(By.Id("applyelection")).Click();

            Assert.AreEqual("10 Nomaination Requried", driver.FindElement(By.XPath("//div[@id='electionfriendsFooter']/span[3]")).Text);
            Assert.AreEqual("3 Agenda Requried", driver.FindElement(By.XPath("//div[@id='electionAgendaFooter']/span[3]")).Text);
            Assert.AreEqual("Select Office to Run For", driver.FindElement(By.XPath("//div[@id='electionapply-box']/div[2]/div/div/fieldset/div/div[2]/span")).Text);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#main').animate({ scrollTop: 0}, 'slow');");

            setupFixture.WaitFortextPreset(By.Id("candidateParty"), waitTime, driver, "Flashpoint");
            if (typeof(TWebDriver) == typeof(FirefoxDriver))
            {
                driver.FindElement(By.Id("radioParty")).Click();
                if (driver.FindElement(By.Id("eleFriends")).GetCssValue("display") != "none")
                {
                    throw new Exception("Element Must be hiden");
                }
            }
        }

        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "P", new int[] { })]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531, 98304 })]

        public void RunforOfficeElectionTicket(int userId, string emailId, string password, string candidatetype, int[] friendSelected)
        {
            ElectionUnitTest unittest = new ElectionUnitTest();
            unittest.RunForOffice(userId, 1, "", candidatetype, friendSelected, new short[] { 2, 3, 4 }, "Pass", 0, "", 0, 0, false, true);
            setupFixture.Login(emailId, password, "election", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("Agendas"), waitTime, driver);
            Thread.Sleep(2000);
            driver.FindElement(By.LinkText("Agendas")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-12 > h5"), waitTime, driver, "Steve Johnston");
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='partyViewcontent-box']/div[2]/div/div/div/div/div/div[2]/div/span"), waitTime, driver, "Seneator");
            if (candidatetype == "P")
            {

                Assert.AreEqual(
                  driver.FindElement(By.LinkText("Flashpoint")).Text, "Flashpoint");
            }
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#main').animate({ scrollTop: 0}, 'slow');");
            Assert.AreEqual(
            driver.FindElement(By.XPath("//div[@id='partyViewcontent-box']/div[2]/div/div/div/div[2]/div/fieldset/ul/li[3]/span")).Text, "We favour investment in stock and commodity exchanges.");
            Assert.AreEqual(
         driver.FindElement(By.XPath("//div[@id='partyViewcontent-box']/div[2]/div/div/div/fieldset/div[3]/div[2]/div[2]/span")).Text, "07/01/2020");
        }

        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "se")]
        public void RunForOfficeCandidate(int userId, string emailId, string password, string countryId)
        {
            setupFixture.LoadDataTable("ElectionCandidate-1", rootFolderCategory);
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            setupFixture.Login(emailId, password, "election", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("Candidate"), waitTime, driver);
            driver.FindElement(By.LinkText("Candidate")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-6 > span.label.label-info"), waitTime, driver, "25");

            CheckDropDownElectionCandidate("Johnny Fuller", "2nd", "38", "38", 4, "Won", "98");
            DonateMoneyElectionCandidate(5000, 1620, countryId, userId, AppSettings.ElectionDonationSuccessNotificationId);
            CheckElectionResult(98, "Lost");
            CheckElectionAgenda("Establish strong repository of arms and ammunition.");


            CheckDropDownElectionCandidate("Jacqueline Carroll", "2nd", "21", "21", 3, "Won", "96");
            DonateMoneyElectionCandidate(6000, 7347, countryId, userId, AppSettings.ElectionDonationFailNotificationId);
            CheckElectionResult(12, "Won");
            CheckElectionAgenda("Our National Defence must be very strong to deter and foil any unfriendly nations.");



            CheckPartyCandidate();
            CheckDropDownElectionCandidate("Norma Parker", "2nd", "12", "12", 2, "Lost", "88");
            DonateMoneyElectionCandidate(7000, 1017, countryId, userId, AppSettings.ElectionDonationSuccessNotificationId);
            CheckElectionResult(87, "Lost");
            CheckElectionAgenda("Our National Defence must be very strong to deter and foil any unfriendly nations.");


            CheckDropDownElectionCandidate("Elizabeth Patterson", "2nd", "5", "5", 1, "Withdrawn", "92");
            CheckElectionResult(26, "Withdrawn");
            CheckElectionAgenda("We favour investment in stock and commodity exchanges.");
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);

        }

        private void CheckDropDownElectionCandidate(string fullName, string rank, string initalLength, string finallength, int index, string result, string votes)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            driver.FindElement(By.Id("electioncandidatedd")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Election " + index), waitTime, driver);
            driver.FindElement(By.LinkText("Election " + index)).Click();




            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-6 > span.label.label-info"), waitTime, driver, initalLength);
            js.ExecuteScript("$('#elecandidate-box .candidates').scrollTop(300);");
            Assert.AreEqual(rank, driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[2]/div/div[2]/div/div[3]/h4/strong")).Text);
            Assert.AreEqual(fullName, driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[2]/div/div/div/div/div/h5")).Text);

            Assert.AreEqual(result, driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[2]/div/div[2]/div/div/h4/strong")).Text);

            Assert.AreEqual(votes, driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[2]/div/div[2]/div/div[2]/h4/strong")).Text);

            driver.FindElement(By.CssSelector("i.fa.fa-plus")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-6 > span.label.label-info"), waitTime, driver, finallength);
            if (index == 4)
            {
                js.ExecuteScript("$('#elecandidate-box .candidates').scrollTop(30000);");
                Assert.AreEqual("38th", driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[38]/div/div[2]/div/div[3]/h4/strong")).Text);
                Assert.AreEqual("Teresa Stevens", driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[38]/div/div/div/div/div/h5")).Text);
            }

        }

        private void CheckPartyCandidate()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#elecandidate-box .candidates').scrollTop(300);");
            driver.FindElement(By.LinkText("Flashpoint")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("h6 > label"), waitTime, driver, "Flashpoint");
            driver.FindElement(By.XPath("//a[@id='backbtn']/i")).Click();

            setupFixture.WaitForElementPreset(By.Id("nav-election"), waitTime, driver);
            Thread.Sleep(3000);
            driver.FindElement((By.Id("nav-election"))).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Candidate"), waitTime, driver);
            driver.FindElement(By.LinkText("Candidate")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-6 > span.label.label-info"), waitTime, driver, "25");

        }

        private void DonateMoneyElectionCandidate(int amount, int userid, string countryId, int sourceid, short notificationId)
        {
            UserBankAccount mybankac = bankRepo.GetUserBankDetails(sourceid);
            UserBankAccount oldbankac;
            oldbankac = bankRepo.GetUserBankDetails(userid);

            decimal totaltax = amount * countrytaxRepo.GetCountryTaxByCode(countryId, AppSettings.TaxDonationCode) / 100;
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#elecandidate-box .candidates').scrollTop(360);");


            driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[2]/ul/li/a")).Click();
            setupFixture.WaitForElementPreset(By.XPath("(//input[@name='donateCash'])[2]"), waitTime, driver);
            driver.FindElement(By.XPath("(//input[@name='donateCash'])[2]")).Clear(); driver.FindElement(By.XPath("(//input[@name='donateCash'])[2]")).SendKeys(amount.ToString());

            driver.FindElement(By.XPath("(//button[@type='submit'])[3]")).Click();

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='eledonate1']/div/div[2]/div/div/div/p/span[2]"), waitTime, driver, amount.ToString());

            setupFixture.WaitForNotificationUpdate(notificationId, userid, 1, waitTime);

            if (notificationId == AppSettings.ElectionDonationFailNotificationId)
            {
                setupFixture.CheckUserBankAccount(oldbankac, 0, 0, 0);
                setupFixture.CheckUserBankAccount(mybankac, 0, 0, 0);
            }
            else
            {
                setupFixture.CheckUserBankAccount(oldbankac, amount, 0, 0);
                setupFixture.CheckUserBankAccount(mybankac, -(amount + totaltax), 0, 0);
            }



        }

        private void CheckElectionResult(int votes, string result)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#elecandidate-box .candidates').scrollTop(450);");

            driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[2]/ul/li[2]/a")).Click();
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='eleresult1']/div/div[2]/div[2]/div[2]/h4/strong"), waitTime, driver, votes.ToString());
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='eleresult1']/div/div/div[2]/div/h4/strong"), waitTime, driver, result);
        }
        private void CheckElectionAgenda(string result)
        {
            driver.FindElement(By.XPath("//div[@id='elecandidate-box']/div[2]/div[2]/div/fieldset/div/div[2]/ul/li[3]/a/span")).Click();
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='eleagenda1']/ul/li/span"), waitTime, driver, result);
        }

        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "se", "38")]
        public void ElectionVoting(int userId, string emailId, string password, string countryId, string totalCandidate)
        {
            setupFixture.LoadDataTable("ElectionCandidate-1", rootFolderCategory);
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            setupFixture.Login(emailId, password, "election", waitTime, driver);
            setupFixture.WaitFortextPreset(By.CssSelector("span.label.label-info"), waitTime, driver, totalCandidate);
            Thread.Sleep(1000);

            setupFixture.WaitForElementPreset(By.XPath("//div[@id='elevoting-box']/div[2]/div/div/fieldset/div/div"), waitTime, driver);
            driver.FindElement(By.XPath("//div[@id='elevoting-box']/div[2]/div/div/fieldset/div/div")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Seneator"), waitTime, driver);
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Seneator")).Click();
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;

            driver.FindElement(By.CssSelector("input[type=\"checkbox\"]")).Click();
            driver.FindElement(By.XPath("//input[@value='1620']")).Click();
            js.ExecuteScript("$('#elevoting-box .candidates').scrollTop(300);");
            driver.FindElement(By.XPath("//input[@value='1818']")).Click();
            js.ExecuteScript("$('#elevoting-box .candidates').scrollTop(400);");
            driver.FindElement(By.XPath("//input[@value='312']")).Click();
            js.ExecuteScript("$('#elevoting-box .candidates').scrollTop(700);");
            driver.FindElement(By.XPath("//input[@value='3084']")).Click();
            js.ExecuteScript("$('#elevoting-box .candidates').scrollTop(1100);");
            driver.FindElement(By.XPath("//input[@value='1017']")).Click();
            js.ExecuteScript("$('#elevoting-box .candidates').scrollTop(1300);");
            driver.FindElement(By.XPath("//input[@value='1975']")).Click();
            driver.FindElement(By.Id("elevote")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-12 > span"), waitTime, driver, "Request To Vote For Election Submitted");

            setupFixture.WaitForNotificationUpdate(AppSettings.ElectionVotingSuccessNotificationId, userId, 1, waitTime);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);

        }

        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", "se", "0")]
        public void ElectionVotingNoCandidate(int userId, string emailId, string password, string countryId, string totalCandidate)
        {
            setupFixture.LoadDataTable("ElectionCandidate-1", rootFolderCategory);
            string electionPeroidsql = "Update Election SET VotingStartDate ='2020-07-01 16:25:00' Where ElectionId =4";
            spContext.ExecuteSql(electionPeroidsql);

            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            setupFixture.Login(emailId, password, "election", waitTime, driver);
            setupFixture.WaitFortextPreset(By.CssSelector("span.label.label-info"), waitTime, driver, totalCandidate);
            Thread.Sleep(1000);
            setupFixture.WaitFortextPreset(By.CssSelector("h5"), waitTime, driver, "Voting Peroid has not started");

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);

        }



    }
}
