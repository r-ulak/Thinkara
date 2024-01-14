using DAO;
using DAO.Models;
using DTO.Db;
using Manager.ServiceController;
using NUnit.Framework;
using OpenQA.Selenium;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnitTest.Model;

namespace UnitTest
{

    public class UnitTestFixture
    {

        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private StoredProcedureExtender spContext;
        private StoredProcedureExtender spContextIdenity;
        private string[] CreateTables;

        public UnitTestFixture(StoredProcedureExtender _spContext, string[] createtables)
        {
            CreateTables = createtables;
            spContext = _spContext;
            spContextIdenity = new StoredProcedureExtender("DefaultConnection");
        }
        public void BootStrapDb()
        {
            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + ConfigurationManager.AppSettings["db.schemasql"]));
            foreach (var item in CreateTables)
            {
                boostrapSql.Append(File.ReadAllText(rootFolder + ConfigurationManager.AppSettings["db." + item.ToLower() + ".createsql"]));
            }


            boostrapSql.Append(File.ReadAllText(rootFolder + ConfigurationManager.AppSettings["db.base.createsql"]));
            boostrapSql.Append(File.ReadAllText(rootFolder + ConfigurationManager.AppSettings["db.curdsql"]));
            boostrapSql.Append(File.ReadAllText(rootFolder + ConfigurationManager.AppSettings["db.spsql"]));

            boostrapSql.Replace("DELIMITER $$", string.Empty);
            boostrapSql.Replace("DELIMITER ;", string.Empty);
            boostrapSql.Replace("$$", ";");

            spContext.ExecuteSql(boostrapSql.ToString());
            boostrapSql.Clear();

            boostrapSql.Append(File.ReadAllText(rootFolder + ConfigurationManager.AppSettings["db.identity.createsql"]));
            spContextIdenity.ExecuteSql(boostrapSql.ToString());



        }

        public void LoadDataTable()
        {
            string dataLoadsqlpath = @"\UnitTest\Category\Sql\DataLoadGlobal.sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + dataLoadsqlpath));

            string dataPath = rootFolder + @"DAL\DbScripts\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            dataPath = rootFolder + @"UnitTest\Category\Sql\Data\";
            boostrapSql.Replace("{1}", dataPath.Replace(@"\", @"\\"));
            boostrapSql.Replace("{1}", dataPath.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());

            boostrapSql.Clear();
            boostrapSql.Append(File.ReadAllText(rootFolder + ConfigurationManager.AppSettings["db.identity.data"]));
            dataPath = rootFolder + @"UnitTest\Category\Sql\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            spContextIdenity.ExecuteSql(boostrapSql.ToString());
        }
        public void LoadDataTable(string dataLoadsql, string rootFolderCategory, string fileId = "")
        {
            string dataLoadsqlpath = @"\Sql\DataLoad" + dataLoadsql + ".sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
                + dataLoadsqlpath));

            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            string dataPath2 = rootFolder + @"DAL\DbScripts\Data\";

            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            boostrapSql.Replace("{1}", fileId);
            boostrapSql.Replace("{2}", dataPath2.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());

        }
        public void CheckUserBankAccount(int userid, decimal cash, decimal gold, decimal silver)
        {

            string getuserbankacsql = string.Format("Select * from UserBankAccount Where UserId = {0}", userid);
            List<UserBankAccount> bankac = spContext.GetSqlData<UserBankAccount>(getuserbankacsql).ToList();
            Assert.AreEqual(bankac.Count(), 1);
            Assert.AreEqual(bankac.Count(f => f.Cash == cash
                && f.Gold == gold && f.Silver == silver), 1);
        }
        public void CheckUserBankAccount(UserBankAccount oldbankac, decimal cashdiff, decimal golddiff, decimal silverdiff)
        {

            string getuserbankacsql = string.Format("Select * from UserBankAccount Where UserId = {0}", oldbankac.UserId);
            List<UserBankAccount> bankac = spContext.GetSqlData<UserBankAccount>(getuserbankacsql).ToList();
            Assert.AreEqual(bankac.Count(), 1);
            Assert.AreEqual(bankac[0].Cash - oldbankac.Cash, Math.Round(cashdiff, 2));
            Assert.AreEqual(bankac[0].Silver - oldbankac.Silver, Math.Round(silverdiff, 2));
            Assert.AreEqual(bankac[0].Gold - oldbankac.Gold, Math.Round(golddiff, 2));
        }
        public void WaitForNotificationUpdate(short notificationId, int userId, int notifycount, int waitTime)
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
                        return;
                    }
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }

        }
        public void CheckCapitalTransactionLog(int sourceId, int recipentId, decimal amount, decimal tax, int fundtype, int count)
        {

            string getcpttrnsql = string.Format("Select * from CapitalTransactionLog Where SourceId = {0} and RecipentId={1} ", sourceId, recipentId);
            List<CapitalTransactionLog> trnLog = spContext.GetSqlData<CapitalTransactionLog>(getcpttrnsql).ToList();
            Assert.AreEqual(trnLog.Count(), count);
            if (count > 0)
            {

                Assert.AreEqual(trnLog[0].Amount, Math.Round(amount, 2));
                Assert.AreEqual(trnLog[0].TaxAmount, Math.Round(tax, 2));
                Assert.AreEqual(trnLog.Count(f => f.FundType == fundtype), count);
            }
        }

        public void CheckUserNotification(int[] userids, string[] parmtext, short notificationTypeId, bool hasTask, int count, int notifCount)
        {
            string getusernotifsql = string.Format("Select * from UserNotification Where UserId in  ({0}) and NotificationTypeId ={1}", String.Join(",", userids), notificationTypeId);
            List<UserNotification> userNotif = spContext.GetSqlData<UserNotification>(getusernotifsql).ToList();
            Assert.AreEqual(userNotif.Count(), notifCount);
            if (notifCount > 0)
            {

                Assert.AreEqual(userNotif.Count(f => f.HasTask == hasTask), notifCount);
                Assert.IsFalse(userNotif.Find(f => f.NotificationTypeId == notificationTypeId).Parms.Contains("||"));
            }
            if (count > 0)
            {

                foreach (var item in userNotif)
                {
                    Assert.AreEqual(userNotif.Count(f => f.Parms.Contains(item.Parms)), count);
                }
            }
            foreach (var item in parmtext)
            {
                Assert.IsTrue(userNotif.Count(f => f.Parms.Contains(item)) >= count);

            }
        }

        public void CheckPost(string[] postId, string[] parmtext, sbyte postTypeId, int count, int userId, string countryId, Guid partyId)
        {
            string parm = String.Join("','", postId.Select(i => i.Replace("'", "''")).ToArray());
            parm = string.Join(",", postId
                                            .Select(x => string.Format("'{0}'", x)));
            string getpostsql = string.Format("Select * from Post Where PostId in ({0}) and PostContentTypeId ={1}", parm, postTypeId);
            List<Post> userPost = spContext.GetSqlData<Post>(getpostsql).ToList();
            Assert.AreEqual(userPost.Count(), count);
            if (count > 0)
            {
                Assert.IsFalse(userPost.Find(f => f.PostContentTypeId == postTypeId).Parms.Contains("||"));
                foreach (var item in userPost)
                {
                    Assert.AreEqual(userPost.Count(f => f.Parms.Contains(item.Parms)), count);
                }
                if (userId > 0)
                {
                    Assert.AreEqual(userPost.Count(f => f.UserId == userId), count);

                }

                if (!string.IsNullOrEmpty(countryId))
                {
                    Assert.AreEqual(userPost.Count(f => f.CountryId == countryId), count);
                }
                if (partyId != Guid.Empty)
                {
                    Assert.AreEqual(userPost.Count(f => f.PartyId == partyId), count);
                }
            }
        }
        public void CheckPost(sbyte postTypeId, int count, string countryId)
        {

            string getpostsql = string.Format("Select * from Post Where PostContentTypeId ={0} and CountryId ='{1}'", postTypeId, countryId);
            List<Post> userPost = spContext.GetSqlData<Post>(getpostsql).ToList();
            Assert.AreEqual(userPost.Count(), count);
            if (count > 0)
            {
                Assert.IsFalse(userPost.Find(f => f.PostContentTypeId == postTypeId).Parms.Contains("||"));
                foreach (var item in userPost)
                {
                    Assert.IsTrue(userPost.Count(f => f.Parms.Contains(item.Parms)) >= 1);
                }

                if (!string.IsNullOrEmpty(countryId))
                {
                    Assert.AreEqual(userPost.Count(f => f.CountryId == countryId), count);
                }
            }
        }
        public void CheckPost(sbyte postTypeId, int count, string countryId, Guid partyId)
        {

            string getpostsql = string.Format("Select * from Post Where PostContentTypeId ={0}", postTypeId);
            List<Post> userPost = spContext.GetSqlData<Post>(getpostsql).ToList();
            Assert.AreEqual(userPost.Count(), count);
            if (count > 0)
            {
                Assert.IsFalse(userPost.Find(f => f.PostContentTypeId == postTypeId).Parms.Contains("||"));
                foreach (var item in userPost)
                {
                    Assert.IsTrue(userPost.Count(f => f.Parms.Contains(item.Parms)) >= 1);
                }


                if (!string.IsNullOrEmpty(countryId))
                {
                    Assert.AreEqual(userPost.Count(f => f.CountryId == countryId), count);
                }
                if (partyId != Guid.Empty)
                {
                    Assert.AreEqual(userPost.Count(f => f.PartyId == partyId), count);
                }
            }
        }
        public void CheckPost(sbyte postTypeId, int countbyCountry, int count, string[] countryId, Guid partyId)
        {

            string getpostsql = string.Format("Select * from Post Where PostContentTypeId ={0}", postTypeId);
            List<Post> userPost = spContext.GetSqlData<Post>(getpostsql).ToList();
            Assert.AreEqual(userPost.Count(), count);
            if (count > 0)
            {
                Assert.IsFalse(userPost.Find(f => f.PostContentTypeId == postTypeId).Parms.Contains("||"));
                foreach (var item in userPost)
                {
                    Assert.IsTrue(userPost.Count(f => f.Parms.Contains(item.Parms)) >= 1);
                }


                if (countryId.Length > 0)
                {
                    foreach (var item in countryId)
                    {
                        Assert.AreEqual(userPost.Count(f => f.CountryId == item), countbyCountry);
                    }
                }
                if (partyId != Guid.Empty)
                {
                    Assert.AreEqual(userPost.Count(f => f.PartyId == partyId), count);
                }
            }
        }

        public void CheckUserTask(int[] userids, string[] parmtext, short taskTypeId, int count)
        {
            string getusernotifsql = "";
            if (userids.Length == 0)
            {
                getusernotifsql = "Select * from UserTask";
            }
            else
            {
                getusernotifsql = string.Format("Select * from UserTask Where UserId in  ({0}) and TaskTypeId", String.Join(",", userids), taskTypeId);
            }

            List<UserTask> usertask = spContext.GetSqlData<UserTask>(getusernotifsql).ToList();
            Assert.AreEqual(usertask.Count(), count);
            if (count > 0)
            {

                Assert.IsFalse(usertask.Find(f => f.TaskTypeId == taskTypeId).Parms.Contains("||"));
                foreach (var item in usertask)
                {
                    Assert.AreEqual(usertask.Count(f => f.Parms.Contains(item.Parms)), count);
                }
            }
        }
        public void CheckUserTaskNoParmCheck(int[] userids, string[] parmtext, short taskTypeId, int count)
        {
            string getusernotifsql = "";
            if (userids.Length == 0)
            {
                getusernotifsql = "Select * from UserTask";
            }
            else
            {
                getusernotifsql = string.Format("Select * from UserTask Where UserId in  ({0}) and TaskTypeId = {1}", String.Join(",", userids), taskTypeId);
            }

            List<UserTask> usertask = spContext.GetSqlData<UserTask>(getusernotifsql).ToList();
            Assert.AreEqual(usertask.Count(), count);
            if (count > 0)
            {

                Assert.IsFalse(usertask.Find(f => f.TaskTypeId == taskTypeId).Parms.Contains("||"));
            }
        }
        public void CheckUserVoteSelectedChoice(Guid taskId, int choiceId, int totalscore, int[] userIds)
        {
            string getuservotesql = string.Format("Select * from UserVoteSelectedChoice Where TaskId =  '{0}' and ChoiceId ={1}", taskId, choiceId);
            List<UserVoteSelectedChoice> userVote = spContext.GetSqlData<UserVoteSelectedChoice>(getuservotesql).ToList();
            Assert.AreEqual(userVote.Count(), userIds.Length);
            Assert.AreEqual(userVote.Sum(f => f.Score), totalscore);
            foreach (var item in userIds)
            {
                Assert.AreEqual(userVote.Count(f => f.UserId == item), 1);
            }
        }


        public void VoteonTask(List<Voters> voters, Guid taskId, short taskType)
        {
            UserVoteManager votemanager = new UserVoteManager();
            VoteResponseDTO userVote = new VoteResponseDTO
            {
                TaskId = taskId,
                TaskTypeId = taskType
            };
            foreach (var item in voters)
            {
                userVote.ChoiceIds = new int[1] { item.ChoiceId };
                userVote.ChoiceRadioId = item.ChoiceId;
                foreach (var voterId in item.TaskVoters)
                {
                    votemanager.ProcessVotingResponse(userVote, voterId);
                }
            }

        }
        public void CheckTaskReminder(int count)
        {
            string gettaskReminder = string.Format("Select * from TaskReminder");
            List<TaskReminder> taskReminders = spContext.GetSqlData<TaskReminder>(gettaskReminder).ToList();
            Assert.AreEqual(taskReminders.Count(), count);
        }
        public void Login(string emailId, string password, string navid, int waitTime, IWebDriver driver)
        {
            string baseURL = ConfigurationManager.AppSettings["baseurl"];

            driver.Navigate().GoToUrl(baseURL);
            for (int second = 0; ; second++)
            {
                if (second >= waitTime) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("Email"), driver)) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys(emailId);
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys(password);

            driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            int userId = webRepo.GetUserIdByEmail(emailId);
            string countryCode = webRepo.GetCountryId(userId);
            ICountryCodeRepository countryRepo = new CountryCodeRepository();

            WaitFortextPreset(By.CssSelector("span.text-right"), waitTime * 3, driver, countryRepo.GetCountryName(countryCode));
            if (navid != string.Empty)
            {
                WaitForElementPreset(By.Id("nav-" + navid), waitTime, driver);
                driver.FindElement(By.Id("nav-" + navid)).Click();
            }

        }
        public bool IsElementPresent(By by, IWebDriver driver)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public void WaitForElementPreset(By by, int waitFor, IWebDriver driver)
        {
            for (int second = 0; ; second++)
            {
                if (second >= waitFor) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(by, driver)) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }

        public void WaitFortextPreset(By by, int waitFor, IWebDriver driver, string text)
        {
            for (int second = 0; ; second++)
            {
                if (second >= waitFor) Assert.Fail("timeout");
                try
                {
                    if (text.Trim() == driver.FindElement(by).Text.Trim()) break;

                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }
        public void TyrUntilElementPreset(By by, int waitFor, IWebDriver driver, By clickItem)
        {
            for (int second = 0; ; second++)
            {
                if (second >= waitFor) Assert.Fail("timeout");
                try
                {
                    driver.FindElement(clickItem).Click();
                    if (IsElementPresent(by, driver)) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
        }
    }


}
