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
    [Category("Jobs")]
    public class JobsUnitTest
    {
        private static string Category = "Jobs";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private sbyte jobId = Convert.ToSByte(ConfigurationManager.AppSettings["jobid.matchjob"]);
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.jobs"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IWebUserDTORepository webRepo = new WebUserDTORepository();
        private UnitTestFixture setupFixture;
        public JobsUnitTest()
        {
            string[] createtables = new string[] { Category, "WebJob", "Education", "CountryTax" };
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
        }

        [Test]
        [TestCase("1", new int[] { 1, 4, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 5, 6, 7, 8, 9 }, 39, 112, 113, 39, 16, 18, 16, 21, 16, 16)]
        public void JobsMatchTest(string fileId, int[] userids, sbyte successnotificationTypeId, sbyte failnotificationTypeId, sbyte notAvailablenotificationTypeId, int failnotifCount, int successnotifCount, int notAvailablenCount, int taskCount, sbyte postContentTypeid, int postCount, int taskReminderCount)
        {

            setupFixture.LoadDataTable("Jobs-x", rootFolderCategory, fileId);
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            JobsManager jobmanager = new JobsManager(jobId);
            JobMatchManager jobMatchManager = new JobMatchManager();
            int runId = jobmanager.GetRunId();
            jobMatchManager.MatchJob(runId);

            CheckJobMatchResult(fileId, userids, successnotificationTypeId, failnotificationTypeId, notAvailablenotificationTypeId, failnotifCount, successnotifCount, notAvailablenCount, taskCount, postContentTypeid, postCount, taskReminderCount);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 60, "Should take less than  1 minutes");
        }

        private void CheckJobMatchResult(string fileId, int[] userids, sbyte successnotificationTypeId, sbyte failnotificationTypeId, sbyte notAvailablenotificationTypeId, int failnotifCount, int successnotifCount, int notAvailablenCount, int taskCount, sbyte postContentTypeid, int postCount, int taskReminderCount)
        {
            List<UserJob> expecteduserJobs = new List<UserJob>();
            GetExpectedUserJob(expecteduserJobs, fileId);

            string getUserJobSql = "Select UserId, JobCodeId, Status from UserJob Order By UserId, JobCodeId, Status";
            List<UserJob> actualuserJob = spContext.GetSqlData<UserJob>(getUserJobSql).ToList();

            Assert.AreEqual(expecteduserJobs.Count, actualuserJob.Count);
            for (int i = 0; i < expecteduserJobs.Count; i++)
            {
                Assert.AreEqual(expecteduserJobs[i].UserId, actualuserJob[i].UserId);
                Assert.AreEqual(expecteduserJobs[i].JobCodeId, actualuserJob[i].JobCodeId);
                Assert.AreEqual(expecteduserJobs[i].Status, actualuserJob[i].Status);
            }
            setupFixture.CheckUserNotification(userids, new string[0], successnotificationTypeId, false, 0, successnotifCount);
            setupFixture.CheckUserNotification(userids, new string[0], failnotificationTypeId, false, 0, failnotifCount);
            setupFixture.CheckUserNotification(userids, new string[0], notAvailablenotificationTypeId, false, 0, notAvailablenCount);
            setupFixture.CheckUserTaskNoParmCheck(userids, new string[0], AppSettings.JobTaskType, taskCount);
            setupFixture.CheckPost(postContentTypeid, postCount, string.Empty, Guid.Empty);
            setupFixture.CheckTaskReminder(taskReminderCount);

        }
        private void GetExpectedUserJob(List<UserJob> userJobs, string fileId)
        {
            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\"; String file_name = dataPath + "ExpectedUserJob-" + fileId + ".csv";
            StreamReader sr = new StreamReader(file_name);
            String line;

            while ((line = sr.ReadLine()) != null)
            {
                String[] tokens = line.Split(',');
                UserJob userJob = new UserJob();
                userJob.UserId = Convert.ToInt32(tokens[0]);
                userJob.JobCodeId = Convert.ToInt16(tokens[1]);
                userJob.Status = (tokens[2]);
                userJobs.Add(userJob);
            }
            sr.Close();
        }

        [Test]
        [TestCase(4, new short[] { 317, 318 }, 38, 1, "Meteorologist", 0, 1)]
        [TestCase(10, new short[] { 317, 318 }, 37, 1, "Meteorologist", 0, 2)]

        public void ApplyJobTestWithPendingJobCode(int userId, short[] jocodesId, short notificationId, int notifCount, string jobTitle, int taskCount, int userJobCount)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            string countryId = webRepo.GetCountryId(userId);
            UserJobManager manager = new UserJobManager();
            ApplyJobCodeDTO[] applyJobList = new ApplyJobCodeDTO[2];
            for (int i = 0; i < jocodesId.Length; i++)
            {
                applyJobList[i] = new ApplyJobCodeDTO()
                {
                    JobCodeId = jocodesId[i],
                };
            }
            manager.ProcessSaveApplyJobs(applyJobList, userId, countryId);
            CheckApplyJobTestWithPendingJobCode(userId, notificationId, notifCount, jobTitle, taskCount, jocodesId, userJobCount);
            int newCount = UnitUtility.ElmahErrorCount(spContext);

            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");

        }

        private void CheckApplyJobTestWithPendingJobCode(int userId, short notificationId, int notifCount, string jobTitle, int taskCount, short[] jocodesId, int userJobCount)
        {
            setupFixture.CheckUserNotification(new int[] { userId }, new string[] { jobTitle }, notificationId, false, 0, notifCount);
            setupFixture.CheckUserTaskNoParmCheck(new int[] { userId }, new string[0], AppSettings.JobTaskType, taskCount);

            string getUserJobSql = string.Format("Select UserId, JobCodeId, Status from UserJob Where UserId in ({0}) And JobCodeId in ({1}) And Status ='P' Order By UserId, JobCodeId, Status", userId, string.Join(",", jocodesId));
            List<UserJob> actualuserJob = spContext.GetSqlData<UserJob>(getUserJobSql).ToList();
            Assert.AreEqual(userJobCount, actualuserJob.Count);

        }

        [Test]
        [TestCase(new int[] { 4, 11 }, "Meteorologist")]
        public void AcceptJobTest(int[] userIds, string jobTitle)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            JobsManager jobmanager = new JobsManager(jobId);
            JobMatchManager jobMatchManager = new JobMatchManager();
            int runId = jobmanager.GetRunId();
            jobMatchManager.MatchJob(runId);
            AcceptJobRequest(userIds);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            CheckAcceptJobRequest(userIds, jobTitle);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");

        }

        private void AcceptJobRequest(int[] userIds)
        {
            string getUserTaskSql = string.Format("Select * from UserTask Where UserId in ({0})", string.Join(",", userIds));
            List<UserTask> actualusertask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();
            List<Voters> voters = new List<Voters>();
            foreach (var item in actualusertask)
            {
                voters.Add(new Voters
           {
               ChoiceId = AppSettings.JobOfferAcceptChoiceId,
               TaskVoters = new List<int> { item.UserId }
           });
                setupFixture.VoteonTask(voters, item.TaskId, AppSettings.JobTaskType);
                voters.Clear();
            }


        }
        private void CheckAcceptJobRequest(int[] userIds, string jobTitle)
        {
            setupFixture.CheckUserNotification(
                userIds, new string[] { jobTitle }, AppSettings.JobOfferAccepetedFailedNotificationId, false, 1, 1);

            setupFixture.CheckUserNotification(
               userIds, new string[] { jobTitle }, AppSettings.JobOfferNotificationId, false, 2, 2);

            setupFixture.CheckUserTaskNoParmCheck(
                userIds, new string[0], AppSettings.JobTaskType, 2
                );

            string getUserJobSql = string.Format("Select UserId, JobCodeId, Status from UserJob Where UserId in ({0})AND Status ='O' ", string.Join(",", userIds));
            List<UserJob> actualuserJob = spContext.GetSqlData<UserJob>(getUserJobSql).ToList();
            Assert.AreEqual(1, actualuserJob.Count);

        }


        [Test]
        [TestCase("1", new int[] { 1, 4, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 5, 6, 7, 8, 9 }, 39, 112, 113, 39, 16, 18, 16, 21, 16, 16)]
        public void JobsMatchWithDefaultResponseTest(string fileId, int[] userids, sbyte successnotificationTypeId, sbyte failnotificationTypeId, sbyte notAvailablenotificationTypeId, int failnotifCount, int successnotifCount, int notAvailablenCount, int taskCount, sbyte postContentTypeid, int postCount, int taskReminderCount)
        {

            setupFixture.LoadDataTable("Jobs-x", rootFolderCategory, fileId);
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            JobsManager jobmanager = new JobsManager(jobId);
            JobMatchManager jobMatchManager = new JobMatchManager();
            int runId = jobmanager.GetRunId();
            jobMatchManager.MatchJob(runId);

            string updateSql = "Update UserTask SET DueDate = UTC_TIMESTAMP()";
            spContext.ExecuteSql(updateSql);
            System.Threading.Thread.Sleep(1000);

            UserTaskDefaultResponseManager taskManager = new UserTaskDefaultResponseManager();
            taskManager.StartTaskVoting(runId);

            CheckUserTaskComplete();

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 60, "Should take less than  1 minutes");
        }


        private void CheckUserTaskComplete()
        {

            string getUserTaskSql = string.Format("Select * from UserTask ");
            List<UserTask> actualuserTask= spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();
            Assert.AreEqual(actualuserTask.Count(f=>f.Status=="C"), actualuserTask.Count);
        }
    }
}
