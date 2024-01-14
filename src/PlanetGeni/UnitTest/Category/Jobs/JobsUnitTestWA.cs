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
using Manager.Jobs;

namespace UnitTest.Category
{
    [TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(ChromeDriver))]
    [Category("Jobs")]
    public class JobsUnitTestWA<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;
        private readonly int waitTime = 15;
        private StringBuilder verificationErrors;
        private string partyPic = ConfigurationManager.AppSettings["pic.capture"];
        private string Category = "Jobs";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.jobs"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["jobid.matchjob"]);

        private IEducationDTORepository educationRepo = new EducationDTORepository();
        private IJobDTORepository jobRepo = new JobDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private UnitTestFixture setupFixture;
        public JobsUnitTestWA()
        {
            string[] createtables = new string[] { "PoliticalParty", "Stock", "UserLoan", "Merchandise", "Job", "Education", "CountryBudget", "WebJob", "CountryTax", "Security" };
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
        [TestCase("planetgeni2@gmail.com", "UnitTest_24!", "0.00", "5", "434", "435", 20, 40, 2, 37, 1, 39, 112, 113, 2, 0, 0, 0, 21, 0, 0, 114, 0, 116, 0)]
        [TestCase("planetgeni@gmail.com", "UnitTest_24!", "0.00", "3", "317", "318", 20, 40, 2, 37, 1, 39, 112, 113, 0, 2, 0, 2, 21, 2, 2, 114, 2, 116, 0)]
        [TestCase("planetgeni1@gmail.com", "UnitTest_24!", "0.00", "3", "317", "318", 20, 40, 2, 37, 1, 39, 112, 113, 0, 2, 0, 2, 21, 2, 2, 114, 1, 116, 1)]
        public void ApplyJobTest(string emailId, string password, string netWorth,
            string jobmajor, string jobId1, string jobId2,
            int firstCount, int secondCount, int jobcount, short notificationTypeId, int notificationCount, sbyte successnotificationTypeId, sbyte failnotificationTypeId, sbyte notAvailablenotificationTypeId, int failnotifCount, int successnotifCount, int notAvailablenCount, int taskCount, sbyte postContentTypeid, int postCount, int taskReminderCount, sbyte acceptednotificationTypeId, int acceptCount, sbyte rejectednotificationTypeId, int rejectedCount)
        {
            string deletsql = string.Format("Delete from UserJob"); ;
            spContext.ExecuteSql(deletsql);

            setupFixture.Login(emailId, password, "job", waitTime, driver);
            setupFixture.WaitFortextPreset(By.CssSelector("#salaryRange > fieldset > legend.text-warning.fontsize120"), waitTime, driver, "Salary Range");


            driver.FindElement(By.Name("searchjobtype")).Click();
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#jobSearch .panel-body').scrollTop(10000);");
            driver.FindElement(By.XPath("(//input[@name='jobMajor'])[" + jobmajor + "]")).Click();
            driver.FindElement(By.Id("searchJob")).Click();

            setupFixture.WaitFortextPreset(By.CssSelector("div.well.well-sm > div.row.padding3centt > div.col-xs-12 > fieldset > legend.text-warning.fontsize120"), waitTime, driver, "Qualification");

            setupFixture.WaitFortextPreset(By.CssSelector("span.label.label-info"), waitTime, driver, firstCount.ToString());
            driver.FindElement(By.Id("jobCodesshowmore")).Click();

            setupFixture.WaitFortextPreset(By.CssSelector("span.label.label-info"), waitTime, driver, secondCount.ToString());
            js.ExecuteScript("$('#jobSearchResultItemcontent-box').scrollTop(450);");
            driver.FindElement(By.XPath("//input[@value='" + jobId1 + "']")).Click();
            js.ExecuteScript("$('#jobSearchResultItemcontent-box').scrollTop(900);");
            driver.FindElement(By.XPath("//input[@value='" + jobId2 + "']")).Click();
            driver.FindElement(By.Id("applyjobsearch")).Click();
            int userId = webRepo.GetUserIdByEmail(emailId);

            CheckApplyJob(userId, jobcount, notificationTypeId, notificationCount);

            JobsManager jobmanager = new JobsManager(jobId);
            JobMatchManager jobMatchManager = new JobMatchManager();
            int runId = jobmanager.GetRunId();
            jobMatchManager.MatchJob(runId);
            CheckRunMatchJob(userId, successnotificationTypeId, failnotificationTypeId, notAvailablenotificationTypeId, failnotifCount, successnotifCount, notAvailablenCount, taskCount, postContentTypeid, postCount, taskReminderCount);
            if (acceptCount > 0)
            {
                for (int i = 0; i < acceptCount; i++)
                {
                    AcceptJob();
                }
                for (int i = 0; i < rejectedCount; i++)
                {
                    RejectJob();
                }
                if (acceptCount > 0)
                {
                    CheckAcceptRejectJob(userId, acceptednotificationTypeId, acceptCount, successnotifCount);
                }
                if (rejectedCount > 0)
                {
                    CheckAcceptRejectJob(userId, rejectednotificationTypeId, rejectedCount, successnotifCount);
                }
            }
        }
        private void CheckApplyJob(int userId, int jobcount, short notificationTypeId, int notificationCount)
        {
            string userJobsql = string.Format("Select * from UserJob Where UserId={0} ", userId);
            for (int second = 0; ; second++)
            {
                if (second >= waitTime) Assert.Fail("timeout");
                try
                {

                    List<UserJob> userJobs = spContext.GetSqlData<UserJob>(userJobsql).ToList();

                    if (userJobs.Count() == jobcount)
                    {
                        Thread.Sleep(6000);
                        break;
                    }
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            setupFixture.CheckUserNotification(new int[] { userId }, new string[] { }, notificationTypeId, false, notificationCount, notificationCount);

        }
        private void CheckRunMatchJob(int userId, sbyte successnotificationTypeId, sbyte failnotificationTypeId, sbyte notAvailablenotificationTypeId, int failnotifCount, int successnotifCount, int notAvailablenCount, int taskCount, sbyte postContentTypeid, int postCount, int taskReminderCount)
        {
            setupFixture.CheckUserNotification(new int[] { userId }, new string[0], notAvailablenotificationTypeId, false, 0, notAvailablenCount);
            setupFixture.CheckUserNotification(new int[] { userId }, new string[0], successnotificationTypeId, false, 0, successnotifCount);
            setupFixture.CheckUserNotification(new int[] { userId }, new string[0], failnotificationTypeId, false, 0, failnotifCount);
            setupFixture.CheckUserTaskNoParmCheck(new int[] { userId }, new string[0], AppSettings.JobTaskType, taskCount);
            setupFixture.CheckPost(postContentTypeid, postCount, string.Empty, Guid.Empty);
            setupFixture.CheckTaskReminder(taskReminderCount);
        }
        private void RejectJob()
        {
            driver.FindElement(By.ClassName("backbtn")).Click();
            Thread.Sleep(2000);
            setupFixture.WaitForElementPreset(By.XPath("//a[@id='nav-task']/i"), waitTime, driver);
            driver.FindElement(By.XPath("//a[@id='nav-task']/i")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.taskdetails > span"), waitTime, driver, "Job Offer Request");
            driver.FindElement(By.XPath("//div[@id='myuserTaskcontent-box']/div[2]/ul/li/div/a/i")).Click();

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='myuserVotingcontent-box']/div[2]/fieldset[3]/legend"), waitTime, driver, "Voting");
            driver.FindElement(By.XPath("(//input[@name='vote'])[2]")).Click();
            driver.FindElement(By.Id("saveVoteResponse")).Click();


        }
        private void AcceptJob()
        {
            driver.FindElement(By.ClassName("backbtn")).Click();
            Thread.Sleep(5000);
            setupFixture.WaitForElementPreset(By.XPath("//a[@id='nav-task']/i"), waitTime, driver);
            driver.FindElement(By.XPath("//a[@id='nav-task']/i")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.taskdetails > span"), waitTime * 2, driver, "Job Offer Request");
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='myuserTaskcontent-box']/div[2]/ul/li/div/a/i"), waitTime, driver);
            driver.FindElement(By.XPath("//div[@id='myuserTaskcontent-box']/div[2]/ul/li/div/a/i")).Click();

            setupFixture.WaitFortextPreset(By.XPath("//div[@id='myuserVotingcontent-box']/div[2]/fieldset[3]/legend"), waitTime, driver, "Voting");
            driver.FindElement(By.Name("vote")).Click();
            driver.FindElement(By.Id("saveVoteResponse")).Click();


        }

        private void CheckAcceptRejectJob(int userId, sbyte acceptedrejectednotificationTypeId, int acceptrejectCount, int successnotifCount)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            driver.FindElement(By.ClassName("backbtn")).Click();
            Thread.Sleep(2000);
            setupFixture.WaitForElementPreset(By.XPath("//a[@id='nav-job']/i"), waitTime, driver);
            driver.FindElement(By.XPath("//a[@id='nav-job']/i")).Click();
            js.ExecuteScript("$('#main').scrollTop(0);");
            setupFixture.WaitForElementPreset(By.LinkText("My"), waitTime, driver);
            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-6 > span.label.label-info"), waitTime, driver, acceptrejectCount.ToString());

            driver.FindElement(By.LinkText("History")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("div.row.padding3centt > div.col-xs-6 > h6 > span.label.label-info"), waitTime, driver, successnotifCount.ToString());

            driver.FindElement(By.LinkText("Summary")).Click();
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='jobSummarycontent-box']/div/div/div[2]/span[2]"), waitTime, driver, acceptrejectCount.ToString());
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-2 > span.fa-1x"), waitTime, driver, "1st");

            setupFixture.CheckUserNotification(new int[] { userId }, new string[0], acceptedrejectednotificationTypeId, false, 0, acceptrejectCount);


        }
    }
}
