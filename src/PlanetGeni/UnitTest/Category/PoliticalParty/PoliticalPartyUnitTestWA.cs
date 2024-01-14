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

namespace UnitTest.Category
{
    [TestFixture(typeof(FirefoxDriver))]
    //[TestFixture(typeof(InternetExplorerDriver))]
    [TestFixture(typeof(ChromeDriver))]
    [Category("PolitcalParty")]
    public class PoliticalPartyUnitTestWA<TWebDriver> where TWebDriver : IWebDriver, new()
    {
        private IWebDriver driver;
        private readonly int waitTime = 10;
        private StringBuilder verificationErrors;
        private string baseURL = ConfigurationManager.AppSettings["baseurl"];
        private static string Category = "PoliticalParty";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IPartyDTORepository partyRepo = new PartyDTORepository(); private string rootFolderCategory = ConfigurationManager.AppSettings["db.politicalparty"];
        private string partyPic = ConfigurationManager.AppSettings["pic.capture"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private UnitTestFixture setupFixture;
        [TestFixtureSetUp]
        public void Init()
        {
            string[] createtables = new string[] { Category, "Stock", "Merchandise", "UserLoan", "CountryTax" };
            setupFixture = new UnitTestFixture(spContext, createtables);
            //return;
            setupFixture.BootStrapDb();
            setupFixture.LoadDataTable();


        }
        [SetUp]
        public void SetupTestData()
        {
            string dataLoadsqlpath = @"\Sql\DataLoad" + Category + ".sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
                + dataLoadsqlpath));

            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());
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

        #region Start party
        public void StartPartyCheckNotification(int userId, string emailId, string password)
        {

            driver.FindElement(By.XPath("//a[@id='notifbadge']/span")).Click();
            setupFixture.WaitForElementPreset(By.Id("notificationsrefresh"), waitTime, driver);
            setupFixture.WaitForElementPreset(By.Id("notificationshowmore"), waitTime, driver);
            driver.FindElement(By.Id("notificationsrefresh")).Click();


            Thread.Sleep(6000);
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='myuserNotificationcontent-box']/div[2]/ul/li[10]"), waitTime, driver);

            for (int second = 0; ; second++)
            {
                if (second >= 30) Assert.Fail("timeout");
                try
                {
                    if (setupFixture.IsElementPresent(By.XPath("//div[@id='myuserNotificationcontent-box']/div[2]/ul/li[13]"), driver)) break;
                    driver.FindElement(By.Id("notificationsrefresh")).Click();
                    Thread.Sleep(4000);
                    driver.FindElement(By.Id("notificationshowmore")).Click();

                }
                catch (Exception)
                { }
                Thread.Sleep(2000);
            }
            Assert.AreEqual("14", driver.FindElement(By.CssSelector("span.label.label-info")).Text);
            driver.Quit();
        }

        [Test]
        [TestCase(3, "fowens@wikizz.gov", "UnitTest_24!")]
        public void StartParty(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "party", waitTime, driver);

            setupFixture.WaitForElementPreset(By.Id("party-search"), waitTime, driver);

            driver.FindElement(By.LinkText("Start Party")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("div.recipient.col-xs-4"), waitTime, driver);
            setupFixture.WaitForElementPreset(By.Name("partyName"), waitTime, driver);


            Thread.Sleep(5000);
            driver.FindElement(By.Name("partyName")).Clear();
            driver.FindElement(By.Name("partyName")).SendKeys("Pelicans");
            driver.FindElement(By.Name("partyMotto")).Clear();
            driver.FindElement(By.Name("partyMotto")).SendKeys("Hello motto");
            driver.FindElement(By.Id("partymembershipFee")).Clear();
            driver.FindElement(By.Id("partymembershipFee")).SendKeys("30000");
            driver.FindElement(By.CssSelector("div.col-xs-12.input-group > ul.checkbox.list-unstyled > li > input[name=\"partyAgenda\"]")).Click();
            driver.FindElement(By.XPath("(//input[@name='partyAgenda'])[10]")).Click();
            setupFixture.WaitForElementPreset(By.Id("partyfileupload"), waitTime, driver);
            setupFixture.WaitFortextPreset(By.XPath("//div[@id='partyinvitefriends']/div[6]/span"), waitTime, driver, "Elizabeth Patterson");

            driver.FindElement(By.XPath("(//button[@type='button'])[6]")).Click();
            if (typeof(TWebDriver) != typeof(InternetExplorerDriver))
            {
                string picpath = (rootFolder + partyPic).Replace(@"\", @"\\");
                driver.FindElement(By.Id("partyfileupload")).SendKeys(picpath);
            }

            driver.FindElement(By.XPath("//div[@id='startpartyinviteemailcontacts']/div/div/div[2]/div[2]/i")).Click();
            driver.FindElement(By.XPath("//div[@id='startpartyinviteemailcontacts']/div/div/div[2]/div[4]/i")).Click();
            driver.FindElement(By.XPath("//div[@id='startpartyinviteemailcontacts']/div/div/div[2]/div[6]/i")).Click();
            driver.FindElement(By.Id("startPartysubmit")).Click();

            Assert.IsTrue(CheckforNotificationUntilPresent(AppSettings.PartyInvitationNotifySuccessNotificationId, userId, 1));
            setupFixture.WaitFortextPreset(By.CssSelector("#startPartycontent-submit > div.panel-body > div.col-xs-12 > span"), waitTime, driver, "Party Application Successfully Submitted");
            Assert.AreEqual("Party Application Successfully Submitted", driver.FindElement(By.CssSelector("#startPartycontent-submit > div.panel-body > div.col-xs-12 > span")).Text);
            CheckPartyCreated("Pelicans");
            StartPartyCheckNotification(userId, emailId, password);
        }
        private void CheckPartyCreated(string partyName)
        {
            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.Id("partyrefresh"), waitTime, driver);
            Thread.Sleep(5000);
            driver.FindElement(By.Id("partyrefresh")).Click();
            Thread.Sleep(2000);

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(0);");
            setupFixture.WaitForElementPreset(By.CssSelector("img[title=" + partyName + "]"), waitTime, driver);

        }
        [Test]
        [TestCase(3, "fowens@wikizz.gov", "UnitTest_24!")]
        public void CheckStartPartyApproved(int userId, string emailId, string password)
        {
            PoliticalPartyUnitTest partytest = new PoliticalPartyUnitTest();
            partytest.StartaNewParty(3, "Pleicans", "Mottop", new int[] { 991, 992, 993, 994, 995, 996, 997, 98304, 88311, 83072, 71713, 63053, 1 }, new short[] { 1, 2 }, "Pass");
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("My"), waitTime, driver);

            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/div/div[3]/div/div[2]/u/span"), waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("Members"), waitTime, driver);

            driver.FindElement(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/ul/li[2]/a/span")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/div/div[3]/div/div[2]/u/span"), waitTime, driver);

            Assert.AreEqual("3,000.00", driver.FindElement(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/div/div[3]/div/div[2]/u/span")).Text);

            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/div/div[2]/div[2]/div[3]/div/div/div/span[2]"), waitTime, driver);

            Assert.AreEqual("30,000.00", driver.FindElement(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/div/div[2]/div[2]/div[3]/div/div/div/span[2]")).Text);
            Assert.AreEqual("Pleicans", driver.FindElement(By.CssSelector("h6 > label")).Text);
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymembersmy0']/div/div/fieldset[3]/div/div/ul/li[10]/p"), waitTime, driver);


            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            Assert.AreEqual("Benjamin Scott", driver.FindElement(By.XPath("//div[@id='partymembersmy0']/div/div/fieldset[3]/div/div/ul/li[10]/p")).Text);
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymembersmy0']/div/div/fieldset[3]/div[2]/div/h6/span"), waitTime, driver);

            Thread.Sleep(2000);
            Assert.AreEqual("10", driver.FindElement(By.XPath("//div[@id='partymembersmy0']/div/div/fieldset[3]/div[2]/div/h6/span")).Text);
            setupFixture.WaitForElementPreset(By.CssSelector("p.text-ellipsis.fontsize80"), waitTime, driver);
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(100);");
            Assert.AreEqual("Frances Owens", driver.FindElement(By.CssSelector("p.text-ellipsis.fontsize80")).Text);
            driver.Quit();
        }
        [Test]
        [TestCase(3, "fowens@wikizz.gov", "UnitTest_24!")]
        public void StartPartyJavascriptValidation(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.Id("party-search"), waitTime, driver);

            driver.FindElement(By.LinkText("Start Party")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("div.recipient.col-xs-4"), waitTime, driver);
            setupFixture.WaitForElementPreset(By.Id("startPartysubmit"), waitTime, driver);
            Thread.Sleep(4000);

            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#startPartysubmit').focus();");
            driver.FindElement(By.Id("startPartysubmit")).Click();
            Assert.AreEqual("Enter a Party Name", driver.FindElement(By.CssSelector("span.help-block.fontsize90")).Text);
            Assert.AreEqual("Enter a Party Motto", driver.FindElement(By.XPath("//form[@id='startpartyform']/div/div[2]/span")).Text);

            js.ExecuteScript("$('input[name=partyName]').focus();");
            driver.FindElement(By.Name("partyName")).Clear();
            driver.FindElement(By.Name("partyName")).SendKeys("Jabberstorm"); //this need to change if it fails becase right now us is hardcoded country instead of pulling user countyr.
            driver.FindElement(By.Name("partyMotto")).Clear();
            driver.FindElement(By.Name("partyMotto")).SendKeys("What");
            js.ExecuteScript("$('#startPartysubmit').focus();");
            driver.FindElement(By.Id("startPartysubmit")).Click();
            js.ExecuteScript("$('input[name=partyName]').focus();");
            setupFixture.WaitFortextPreset(By.XPath("//form[@id='startpartyform']/div/div/span"), waitTime, driver, "Party Name already taken");
            Assert.AreEqual("Party Name already taken", driver.FindElement(By.XPath("//form[@id='startpartyform']/div/div/span")).Text);
            Assert.AreEqual("Enter at least 5 characters", driver.FindElement(By.XPath("//form[@id='startpartyform']/div/div[2]/span")).Text);
            driver.Quit();
        }

        #endregion Start party

        #region Quit party

        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!")]
        public void Quitparty(int userId, string emailId, string password)
        {
            IPartyDTORepository partyRepo = new PartyDTORepository();
            string partyId = partyRepo.GetActivePartyId(userId);
            PoliticalParty partyInfo = partyRepo.GetPartyById(partyId);
            PoliticalPartyUnitTest partytest = new PoliticalPartyUnitTest();
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.Id("party-search"), waitTime, driver);

            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/div/div[2]/div[2]/div[3]/div/div[2]/span"), waitTime, driver);


            driver.FindElement(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/div/div[2]/div[2]/div[3]/div/div[2]/span")).Click();
            setupFixture.WaitFortextPreset(By.CssSelector("#partyMycontent-submit > div.panel-body > div.col-xs-12 > span"), waitTime, driver, "Request To Quit Party Submitted");

            Assert.AreEqual("Request To Quit Party Submitted", driver.FindElement(By.CssSelector("#partyMycontent-submit > div.panel-body > div.col-xs-12 > span")).Text);
            driver.Quit();
            CheckforNotificationUntilPresent(AppSettings.PartyLeaveRequestSuccessNotificationId, userId, 1);
            partytest.CheckQuitParty(userId, partyInfo, "P");
        }
        #endregion Quit party

        #region Manage Party
        [Test]
        [TestCase(9930, "mwelch@aimbu.org", "UnitTest_24!")]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!")]
        public void ManagePartyTest(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("My"), waitTime, driver);
            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Manage"), waitTime, driver);
            driver.FindElement(By.LinkText("Manage")).Click();

            if (userId == 1)
            {
                setupFixture.WaitFortextPreset(By.CssSelector("#partymanagemy0 > span.fontsize90"), waitTime, driver, "Access Denied");
                Assert.AreEqual("Access Denied", driver.FindElement(By.CssSelector("#partymanagemy0 > span.fontsize90")).Text);
                driver.Quit();
                return;
            }

            setupFixture.WaitForElementPreset(By.Id("managepartymembershipFee"), waitTime, driver);

            Thread.Sleep(2000);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('input[name=partyName]').focus();");
            driver.FindElement(By.Name("partyName")).Clear();
            driver.FindElement(By.Name("partyName")).SendKeys("FlashPoint Mob");
            js.ExecuteScript("$('input[name=partyMotto]').focus();");
            driver.FindElement(By.Name("partyMotto")).Clear();
            driver.FindElement(By.Name("partyMotto")).SendKeys("WhatUp");
            js.ExecuteScript("$('#managepartymembershipFee').focus();");

            driver.FindElement(By.Id("managepartymembershipFee")).Clear();
            driver.FindElement(By.Id("managepartymembershipFee")).SendKeys("500");
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(500);");

            setupFixture.WaitForElementPreset(By.CssSelector("div.col-xs-12.input-group > ul.checkbox.list-unstyled > li > input[name='partyAgenda']"), waitTime, driver);
            js.ExecuteScript("$('input[name^=partyAgenda]').first().focus();");

            Thread.Sleep(1000);
            driver.FindElement(By.XPath("(//input[@name='partyAgenda'])[10]")).Click();
            driver.FindElement(By.XPath("(//input[@name='partyAgenda'])[16]")).Click();
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");

            driver.FindElement(By.Id("managepartysave")).Click();
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            setupFixture.WaitForElementPreset(By.CssSelector("div.alert.alert-success"), waitTime, driver);

            driver.FindElement(By.Id("partyrefresh")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.Id("partyrefresh")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("h6 > label"), waitTime, driver);
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(0);");
            Assert.AreEqual("FlashPoint Mob", driver.FindElement(By.CssSelector("h6 > label")).Text);
            Assert.AreEqual("WhatUp", driver.FindElement(By.CssSelector("div.col-xs-10 > span.fontsize80")).Text);
            driver.FindElement(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/ul/li/a/span")).Click();
            Thread.Sleep(3000);
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            if (!(driver.GetType().ToString().Contains("ChromeDriver")))
            {

                setupFixture.WaitFortextPreset(By.XPath("//div[@id='partymyagenda0']/div/div/div/div[2]/ul/li[3]/span"), waitTime, driver, "Taking War to other nation is best policy than to have to defend one from unfriendly nations.");
                Assert.AreEqual("Taking War to other nation is best policy than to have to defend one from unfriendly nations.", driver.FindElement(By.XPath("//div[@id='partymyagenda0']/div/div/div/div[2]/ul/li[3]/span")).Text);
                Assert.AreEqual("Establish strong repository of arms and ammunition.", driver.FindElement(By.XPath("//div[@id='partymyagenda0']/div/div/div/div[2]/ul/li[2]/span")).Text);
                Assert.AreEqual("Our National Defence must be very strong to deter and foil any unfriendly nations.", driver.FindElement(By.XPath("//div[@id='partymyagenda0']/div/div/div/div[2]/ul/li[1]/span")).Text);
            }

            PutOldNameBack();

            driver.Quit();
        }

        private void PutOldNameBack()
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            driver.FindElement(By.LinkText("Manage")).Click();
            js.ExecuteScript("$('input[name=partyName]').focus();");
            driver.FindElement(By.Name("partyName")).Clear();
            driver.FindElement(By.Name("partyName")).SendKeys("FlashPoint");
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");

            driver.FindElement(By.Id("managepartysave")).Click();
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            setupFixture.WaitForElementPreset(By.CssSelector("div.alert.alert-success"), waitTime, driver);
            driver.FindElement(By.Id("partyrefresh")).Click();
            Thread.Sleep(4000);
            driver.FindElement(By.Id("partyrefresh")).Click();
            Thread.Sleep(10000);
            driver.FindElement(By.Id("partyrefresh")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("h6 > label"), waitTime, driver);
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(0);");
            Assert.AreEqual("FlashPoint", driver.FindElement(By.CssSelector("h6 > label")).Text);
        }
        [Test]
        [TestCase(9930, "mwelch@aimbu.org", "UnitTest_24!")]
        public void ManagePartyTestFileUpload(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("My"), waitTime, driver);
            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Manage"), waitTime, driver);
            driver.FindElement(By.LinkText("Manage")).Click();
            setupFixture.WaitForElementPreset(By.Id("managepartymembershipFee"), waitTime, driver);


            Thread.Sleep(2000);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");

            if (typeof(TWebDriver) != typeof(InternetExplorerDriver))
            {
                string picpath = (rootFolder + partyPic).Replace(@"\", @"\\");
                driver.FindElement(By.Id("managepartyfileupload")).SendKeys(picpath);
            }

            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            driver.FindElement(By.Id("managepartyupload")).Click();
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            setupFixture.WaitForElementPreset(By.CssSelector("div.alert.alert-success"), waitTime, driver);
            setupFixture.WaitFortextPreset(By.CssSelector("p.fontsize90 > span"), waitTime, driver, "Upload Submitted Successfully");
            Assert.AreEqual("Upload Submitted Successfully", driver.FindElement(By.CssSelector("p.fontsize90 > span")).Text);

            CheckPartyCreated(partyRepo.GetUserParties(userId).ToList()[0].PartyName);
            driver.Quit();
        }
        [Test]
        [TestCase(9930, "mwelch@aimbu.org", "UnitTest_24!")]
        public void ManagePartyValidationsTest(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("My"), waitTime, driver);
            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Manage"), waitTime, driver);
            driver.FindElement(By.LinkText("Manage")).Click();
            setupFixture.WaitForElementPreset(By.Id("managepartymembershipFee"), waitTime, driver);


            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            js.ExecuteScript("$('input[name=partyName]').focus();");
            driver.FindElement(By.Name("partyName")).Clear();
            driver.FindElement(By.Name("partyName")).SendKeys("");
            js.ExecuteScript("$('input[name=partyMotto]').focus();");
            driver.FindElement(By.Name("partyMotto")).Clear();
            driver.FindElement(By.Name("partyMotto")).SendKeys("");
            js.ExecuteScript("$('#managepartymembershipFee').focus();");
            driver.FindElement(By.Id("managepartymembershipFee")).Clear();
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(2000);");
            driver.FindElement(By.Id("managepartysave")).Click();
            js.ExecuteScript("$('input[name=partyName]').focus();");
            Assert.AreEqual("Enter a Party Name", driver.FindElement(By.CssSelector("span.help-block.fontsize90")).Text);
            Assert.AreEqual("Enter a Party Motto", driver.FindElement(By.XPath("//form[@id='managepartyform']/div/div[2]/span")).Text);
            Assert.AreEqual("Enter a MemberShip Fee", driver.FindElement(By.XPath("//form[@id='managepartyform']/div/div[3]/span")).Text);

            js.ExecuteScript("$('input[name=partyName]').focus();");
            driver.FindElement(By.Name("partyName")).Clear();
            driver.FindElement(By.Name("partyName")).SendKeys("ainyx");
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(2000);");
            driver.FindElement(By.Id("managepartysave")).Click();
            js.ExecuteScript("$('input[name=partyName]').focus();");
            setupFixture.WaitFortextPreset(By.CssSelector("span.help-block.fontsize90"), waitTime, driver, "Party Name already taken");
            Assert.AreEqual("Party Name already taken", driver.FindElement(By.CssSelector("span.help-block.fontsize90")).Text);
            driver.Quit();
        }
        #endregion Manage Party

        #region Close Party
        [Test]
        [TestCase(9930, "mwelch@aimbu.org", "UnitTest_24!")]
        public void ClosePartyTest(int userId, string emailId, string password)
        {
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.LinkText("My"), waitTime, driver);
            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Manage"), waitTime, driver);
            driver.FindElement(By.LinkText("Manage")).Click();
            setupFixture.WaitForElementPreset(By.Id("managepartymembershipFee"), waitTime, driver);

            if (userId == 1)
            {
                Assert.AreEqual("Access Denied", driver.FindElement(By.CssSelector("#partymanagemy0 > span")).Text);
                return;
            }
            Thread.Sleep(4000);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(10000);");
            setupFixture.WaitForElementPreset(By.Id("managepartyclose"), waitTime, driver);

            driver.FindElement(By.Id("managepartyclose")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("div.alert.alert-success"), waitTime, driver);

            Assert.AreEqual("Close Request Submitted Successfully", driver.FindElement(By.CssSelector("p.fontsize90 > span")).Text);
            driver.Quit();

        }
        #endregion Close Party

        #region Check Party Info
        [Test]
        [TestCase(312, "kflores@wikido.info", "UnitTest_24!")]
        public void CheckPartyInfo(int userId, string emailId, string password)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            AddNotification(userId);
            setupFixture.Login(emailId, password, "", waitTime, driver);
            driver.FindElement(By.XPath("//a[@id='notifbadge']/span")).Click();
            setupFixture.WaitForElementPreset(By.Id("notificationsrefresh"), waitTime, driver);
            setupFixture.WaitFortextPreset(By.CssSelector("a.btn-link > strong"), waitTime, driver, "Jabberstorm");
            Thread.Sleep(4000);
            driver.FindElement(By.LinkText("Jabberstorm")).Click();

            setupFixture.WaitForElementPreset(By.LinkText("Members"), waitTime, driver);
            driver.FindElement(By.LinkText("Members")).Click();
            setupFixture.WaitForElementPreset(By.LinkText("Agenda"), waitTime, driver);
            driver.FindElement(By.LinkText("Agenda")).Click();

            driver.FindElement(By.LinkText("Members")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymembersview0']/div/div/fieldset[3]/div/div/div[9]/p"), waitTime, driver);
            driver.FindElement(By.XPath("//div[@id='partymembersview0']/div/div/fieldset[3]/div[2]/div[2]/span")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymembersview0']/div/div/fieldset[3]/div/div/div[11]/p"), waitTime, driver);
            Assert.AreEqual("Tina Edwards", driver.FindElement(By.XPath("//div[@id='partymembersview0']/div/div/fieldset[3]/div/div/div[11]/p")).Text);
            Assert.AreEqual("11", driver.FindElement(By.XPath("//div[@id='partymembersview0']/div/div/fieldset[3]/div[2]/div/h6/span")).Text);

            js.ExecuteScript("$('#partymembersview0').focus();");
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Agenda")).Click();
            Assert.AreEqual("War is also a means to defend the nation at the same time strengthen the economy of your country.", driver.FindElement(By.XPath("//div[@id='partyviewagenda0']/div/div/div/div[2]/ul/li[6]/span")).Text);

            driver.Quit();

        }
        private void AddNotification(int userId)
        {
            IUserNotificationDetailsDTORepository notify = new UserNotificationDetailsDTORepository();

            notify.AddNotification(false, string.Empty, AppSettings.PartyApplyJoinVotingRequestNotificationId, "8|Russell Williamson|b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|<strong>Date:12/18/2014 2:35:28 AM</strong>|onclick: GoToTask()", 6, userId);

        }
        #endregion Check Party Info

        #region Search Party
        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 1, 5, 2, 3, 1, "1", false)]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 5, 5, 5, 5, 1, "5", false)]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 5, 5, 4, 5, 1, "0", false)]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 1, 4, 1, 4, 2, "2", true)]
        public void SearchParty1(int userId, string emailId, string password, int partysize, int electionvictory, int partyworth, int membershipFee, int partyAgenda, string expectedresult, bool checksearchItem)
        {
            SearchParty(userId, emailId, password, partysize, electionvictory, partyworth, membershipFee, partyAgenda, expectedresult, checksearchItem);
            string selector = "";
            if (Convert.ToInt32(expectedresult) > 0)
            {
                setupFixture.WaitForElementPreset(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div/div"), waitTime, driver);
                selector = "div.col-xs-6 > h6 > span.label.label-info";
            }
            else
            {
                selector = "span.label.label-info";
            }
            setupFixture.WaitFortextPreset(By.CssSelector(selector), waitTime, driver, expectedresult);

            Assert.AreEqual(expectedresult, driver.FindElement(By.CssSelector(selector)).Text);
            driver.FindElement(By.Id("partyCodesshowmore")).Click();
            Thread.Sleep(5000);
            Assert.AreEqual(expectedresult, driver.FindElement(By.CssSelector(selector)).Text);
            if (checksearchItem)
            {
                CheckSearchItems(expectedresult);
            }
            driver.Quit();

        }

        private void CheckSearchItems(string expectedresult)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            driver.FindElement(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div/ul/li[2]/a/span")).Click();
            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(500);");
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymemberssearch0']/div/div/fieldset[3]/div/div/div[9]/p"), waitTime, driver);
            driver.FindElement(By.XPath("//div[@id='partymemberssearch0']/div/div/fieldset[3]/div[2]/div[2]/span")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymemberssearch0']/div/div/fieldset[3]/div/div/div[11]/p"), waitTime, driver);
            Thread.Sleep(4000);
            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(900);");
            Assert.AreEqual("11", driver.FindElement(By.XPath("//div[@id='partymemberssearch0']/div/div/fieldset[3]/div[2]/div/h6/span")).Text);
            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(0);");
            driver.FindElement(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div/ul/li/a/span")).Click();

            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partysearchagenda0']/div/div/div/div[2]/ul/li[6]/span"), waitTime, driver);

            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(400);");
            Assert.AreEqual("War is also a means to defend the nation at the same time strengthen the economy of your country.", driver.FindElement(By.XPath("//div[@id='partysearchagenda0']/div/div/div/div[2]/ul/li[6]/span")).Text);

            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(10000);");
            driver.FindElement(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div[2]/ul/li[2]/a/span")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymemberssearch1']/div/div/fieldset/div/div/div/p"), waitTime, driver);

            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(10000);");
            Assert.AreEqual("Victor Hayes", driver.FindElement(By.XPath("//div[@id='partymemberssearch1']/div/div/fieldset/div/div/div/p")).Text);
            Assert.AreEqual("0", driver.FindElement(By.XPath("//div[@id='partymemberssearch1']/div/div/fieldset[3]/div[2]/div/h6/span")).Text);
            driver.FindElement(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div[2]/ul/li/a/span")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partysearchagenda1']/div/div/div/div[2]/ul/li[2]/span"), waitTime, driver);
            Thread.Sleep(2000);
            Assert.AreEqual("Establish strong repository of arms and ammunition.", driver.FindElement(By.XPath("//div[@id='partysearchagenda1']/div/div/div/div[2]/ul/li[2]/span")).Text);


        }

        private void SearchParty(int userId, string emailId, string password, int partysize, int electionvictory, int partyworth, int membershipFee, int partyAgenda, string expectedresult, bool checksearchItem)
        {
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            setupFixture.Login(emailId, password, "party", waitTime, driver);
            setupFixture.WaitForElementPreset(By.CssSelector("#electionvictory > fieldset > legend.text-warning.fontsize120"), waitTime, driver);


            driver.FindElement(By.XPath("(//input[@name='partysize'])[" + partysize + "]")).Click();
            driver.FindElement(By.XPath("(//input[@name='electionvictory'])[" + electionvictory + "]")).Click();
            js.ExecuteScript("$('input[name=partyworth]').focus();");
            driver.FindElement(By.XPath("(//input[@name='partyworth'])[" + partyworth + "]")).Click();
            js.ExecuteScript("$('input[name=membershipFee]').focus();");
            driver.FindElement(By.XPath("(//input[@name='membershipFee'])[" + membershipFee + "]")).Click();
            js.ExecuteScript("$('ul.checkbox.list-unstyled > li').focus();");
            js.ExecuteScript("$('#partySearch .panel-body').scrollTop(10000);");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("(//input[@name='partyAgenda'])[" + partyAgenda + "]")).Click();
            driver.FindElement(By.Id("searchParty")).Click();
        }
        #endregion Search Party

        #region Check Party Agenda
        [Test]
        [TestCase(1, "sjohnston@gigabox.com", "UnitTest_24!", 5, 5, 5, 5, 1, "5", false)]
        public void CheckPartyAgenda(int userId, string emailId, string password, int partysize, int electionvictory, int partyworth, int membershipFee, int partyAgenda, string expectedresult, bool checksearchItem)
        {
            SearchParty(userId, emailId, password, partysize, electionvictory, partyworth, membershipFee, partyAgenda, expectedresult, checksearchItem);
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div/div"), waitTime, driver);
            setupFixture.WaitFortextPreset(By.CssSelector("div.col-xs-6 > h6 > span.label.label-info"), waitTime, driver, "5");
            Assert.AreEqual("5", driver.FindElement(By.CssSelector("div.col-xs-6 > h6 > span.label.label-info")).Text);
            driver.FindElement(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div/ul/li/a/span")).Click();
            setupFixture.WaitForElementPreset(By.CssSelector("ul.list-styled > li"), waitTime, driver);
            Assert.AreEqual("Our National Defence must be very strong to deter and foil any unfriendly nations.", driver.FindElement(By.CssSelector("li > span.fontsize80")).Text);

            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(900);");
            driver.FindElement(By.XPath("//div[@id='partySearchResultcontent-wrapper']/div[3]/ul/li/a/span")).Click();

            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partysearchagenda2']/div/div/div/div[2]/ul/li[5]/span"), waitTime, driver);
            js.ExecuteScript("$('#partySearchResultItemcontent-box').scrollTop(1000);");
            Thread.Sleep(2000);
            Assert.AreEqual("We believe that best defended of the nation is to have aggressive foreign policy.", driver.FindElement(By.XPath("//div[@id='partysearchagenda2']/div/div/div/div[2]/ul/li[5]/span")).Text);


            driver.FindElement(By.LinkText("My")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/ul/li/a/span"), waitTime, driver);
            driver.FindElement(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/ul/li/a/span")).Click();

            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partyMyItemcontent-wrapper']/div/ul/li/a/span"), waitTime, driver);


            setupFixture.WaitForElementPreset(By.CssSelector("#partymyagenda0 > div.row.padding3centt > div.col-xs-12 > div.row > div.col-xs-11 > ul.list-styled > li > span.fontsize80"), waitTime, driver);
            Thread.Sleep(3000);
            js.ExecuteScript("$('#partyMyItemcontent-box').scrollTop(1000);");

            setupFixture.WaitFortextPreset(By.CssSelector("#partymyagenda0 > div.row.padding3centt > div.col-xs-12 > div.row > div.col-xs-11 > ul.list-styled > li > span.fontsize80"), waitTime, driver, "Our National Defence must be very strong to deter and foil any unfriendly nations.");


            Assert.AreEqual("Our National Defence must be very strong to deter and foil any unfriendly nations.", driver.FindElement(By.CssSelector("#partymyagenda0 > div.row.padding3centt > div.col-xs-12 > div.row > div.col-xs-11 > ul.list-styled > li > span.fontsize80")).Text);
            driver.FindElement(By.LinkText("History")).Click();

            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partyMyHistoryItemcontent-wrapper']/div/ul/li/a/span"), waitTime, driver);
            driver.FindElement(By.XPath("//div[@id='partyMyHistoryItemcontent-wrapper']/div/ul/li/a/span")).Click();
            setupFixture.WaitForElementPreset(By.XPath("//div[@id='partymyHistoryagenda0']/div/div/div/div[2]/ul/li[2]/span"), waitTime, driver);

            js.ExecuteScript("$('#partyMyHistoryItemcontent-box').scrollTop(1000);");

            Assert.AreEqual("Taking War to other nation is best policy than to have to defend one from unfriendly nations.", driver.FindElement(By.XPath("//div[@id='partymyHistoryagenda0']/div/div/div/div[2]/ul/li[2]/span")).Text);

            driver.Quit();

        }
        #endregion Check Party Agenda

        private bool CheckforNotificationUntilPresent(short notificationId, int userId, int notifycount)
        {

            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId ={0} And NotificationTypeId={1}", userId, notificationId);
            for (int second = 0; ; second++)
            {
                if (second >= waitTime) Assert.Fail("timeout");
                try
                {

                    List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
                    if (userNotifications.Count() >= notifycount)
                    {
                        Thread.Sleep(6000);
                        return true;
                    }
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }

        }
    }
}
