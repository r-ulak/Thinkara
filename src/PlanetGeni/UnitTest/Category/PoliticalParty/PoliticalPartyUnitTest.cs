using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
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

namespace UnitTest.Category
{
    [TestFixture]
    [Category("PolitcalParty")]
    public class PoliticalPartyUnitTest
    {
        private static string Category = "PoliticalParty";
        private string rootFolder = ConfigurationManager.AppSettings["db.rootfolder"];
        private string database = ConfigurationManager.AppSettings["redis.database"];
        private string rootFolderCategory = ConfigurationManager.AppSettings["db.politicalparty"];
        private StoredProcedureExtender spContext = new StoredProcedureExtender();
        private RedisCacheProviderExtender cache = new RedisCacheProviderExtender(true, AppSettings.RedisDatabaseId);
        private IRedisCacheProvider redisCache = new RedisCacheProvider(AppSettings.RedisDatabaseId);
        private IPartyDTORepository partyRepo = new PartyDTORepository();
        private IWebUserDTORepository webRepo = new WebUserDTORepository();

        [TestFixtureSetUp]
        public void Init()
        {
            //return;
            string[] createtables = new string[] { Category, "CountryTax" };
            UnitTestFixture setupFixture = new UnitTestFixture(spContext, createtables);
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


        }

        #region Party Nomination
        #region Nominate CoFounder
        [Category("Party Nominate")]
        [Test]
        public void TestPartyNominationCoFounderApprovalRequest()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            Guid taskId = new Guid();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            PartyNominationDTO partyDTO = RequestNomination("C");
            taskId = TestPartyNominationRequest_CheckUserTask(partyDTO);
            TestPartyNominationRequest_CheckUserNotifcation(partyDTO);

            AcceptNomination(partyDTO, taskId);

            PoliticalParty partyInfo = partyRepo.GetPartyById(partyDTO.PartyId);
            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;
            VoteApprovalonNomination(taskId, partyDTO);

            double voteNeeded = totalVoters * AppSettings.PartyNominationCoFounderApprovalVoteNeeded;
            VerifyApprovalAfterCoFounderNomination(taskId, partyDTO, Convert.ToInt32(Math.Ceiling(voteNeeded)));
            CheckCacheRequestNomination(partyDTO, "C");
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");

        }

        private PartyNominationDTO RequestNomination(string nominationType)
        {
            PartyNominationDTO partyDTO = new PartyNominationDTO
            {
                InitatorId = 1,
                PartyId = "7b80127f-8b05-41cc-bd54-9c26baacea2c",
                NomineeId = 4878,
                NominatingMemberType = nominationType,
                NomineeIdMemberType = "M"

            };
            PoliticalPartyManager partymanager = new PoliticalPartyManager();
            partymanager.ProcessRequestNominationParty(partyDTO);




            return partyDTO;
        }
        private void AcceptNomination(PartyNominationDTO partyDTO, Guid taskId)
        {
            VoteResponseDTO userVote = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.NotifyNominationRequestApprovalChoiceId },
                ChoiceRadioId = AppSettings.NotifyNominationRequestApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.NominationNotifyPartyTaskType
            };
            UserVoteManager votemanager = new UserVoteManager();
            votemanager.ProcessVotingResponse(userVote, partyDTO.NomineeId);
            TestPartyNominationRequest_NomineeResponse_CheckUserNotifcation(partyDTO, userVote);
            TestPartyNominationRequest_NomineeResponse_CheckUserTask(partyDTO);

        }
        private Guid TestPartyNominationRequest_CheckUserTask(PartyNominationDTO partyDTO)
        {

            string getUserTasksql = string.Format("Select * from UserTask Where AssignerUserId = {0} and UserId ={1} and TaskTypeId ={2} and Status='{3}'",
                partyDTO.InitatorId,
                partyDTO.NomineeId,
                AppSettings.NominationNotifyPartyTaskType,
                "I"
                );

            string parmTextTask = @"1|Steve Johnston|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTasksql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userTask.Count(), 1);
            Assert.AreEqual(userTask.Count(), 1);
            Assert.IsFalse(userTask[0].Parms.Contains("||"));
            Assert.IsTrue(userTask[0].Parms.Contains(parmTextTask));
            Assert.AreEqual(userTask[0].Parms.Count(f => f == '|'), 6);
            return userTask[0].TaskId;

        }
        private void TestPartyNominationRequest_CheckUserNotifcation(PartyNominationDTO partyDTO)
        {
            string getUserNotificationSql = "Select * from UserNotification Order By NotificationTypeId";

            string parmTextNotifyNomination = @"1|Steve Johnston|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|onclick: GoToTask()";

            string parmTextNominationSuccess = @"1|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";


            List<UserNotification> userNotifcation = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();

            //1 Notification Must be created with the follwoing parm
            Assert.AreEqual(userNotifcation.Count(), 2);
            Assert.AreEqual(userNotifcation[0].Parms, parmTextNominationSuccess);
            Assert.AreEqual(userNotifcation[1].Parms, parmTextNotifyNomination);
            Assert.AreEqual(userNotifcation[0].Parms.Count(f => f == '|'), 4);
            Assert.AreEqual(userNotifcation[1].Parms.Count(f => f == '|'), 5);

            Assert.AreEqual(userNotifcation[0].NotificationTypeId, AppSettings.PartyNominationRequestSuccessNotificationId);
            Assert.AreEqual(userNotifcation[1].NotificationTypeId, AppSettings.PartyNotifyNominationNotificationId);


        }
        private void TestPartyNominationRequest_NomineeResponse_CheckUserNotifcation(PartyNominationDTO partyDTO, VoteResponseDTO userVote)
        {
            string getUserNotificationSql = string.Format("Select * from UserNotification Where NotificationTypeId ={0}", AppSettings.PartyNominationVotingRequestNotificationId);
            List<UserNotification> userNotifcation = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();

            string parmTextNotifyNomination = @"1|Steve Johnston|<strong>Date:11/19/2014 5:03:56 AM</strong>|4878|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|onclick: GoToTask()";

            Assert.AreEqual(userNotifcation[0].Parms.Count(f => f == '|'), 8);
            //Task for voting for nomination should not be sent to the user themselve.
            Assert.AreEqual(userNotifcation.Count(f => f.UserId == partyDTO.NomineeId), 0);


            string[] users = partyRepo.GetAllPartyMember(partyDTO.PartyId);
            Assert.AreEqual(userNotifcation.Count(), users.Length - 1);

            foreach (var item in users)
            {
                int checkUseId = Convert.ToInt32(item);
                if (checkUseId != partyDTO.NomineeId)
                {

                    Assert.AreEqual(userNotifcation.Count(f => f.UserId == checkUseId), 1);
                }
            }
        }
        private Guid TestPartyNominationRequest_NomineeResponse_CheckUserTask(PartyNominationDTO partyDTO)
        {

            string getUserTasksql = string.Format("Select * from UserTask Where AssignerUserId = {0} and TaskTypeId ={1} and Status='{2}'",
                partyDTO.InitatorId,
                AppSettings.NominationPartyTaskType,
                "I"
                );

            string parmTextTask = @"1|Steve Johnston|4878|Randy Wheeler|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|" + partyDTO.GetPartyMemberType() + "|";


            string[] users = partyRepo.GetAllPartyMember(partyDTO.PartyId);

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTasksql).ToList();
            //1 Task Must be created for each party member
            Assert.AreEqual(userTask.Count(), users.Length - 1);
            Assert.IsTrue(userTask[0].Parms.Contains(parmTextTask));
            Assert.IsFalse(userTask[0].Parms.Contains("||"));
            Assert.AreEqual(userTask[0].Parms.Count(f => f == '|'), 8);

            foreach (var item in users)
            {
                int checkUseId = Convert.ToInt32(item);
                if (checkUseId != partyDTO.NomineeId)
                {
                    Assert.AreEqual(userTask.Count(f => f.UserId == checkUseId), 1);
                }
            }

            return userTask[0].TaskId;

        }
        private void VoteApprovalonNomination(Guid taskId, PartyNominationDTO partyDTO)
        {
            UserVoteManager votemanager = new UserVoteManager();

            string[] users = partyRepo.GetAllPartyMember(partyDTO.PartyId);
            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyNominationElectionApprovalChoiceId },
                ChoiceRadioId = AppSettings.PartyNominationElectionApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.NominationPartyTaskType
            };

            VoteResponseDTO userVoteDeny = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyNominationElectionDenialChoiceId },
                ChoiceRadioId = AppSettings.PartyNominationElectionDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.NominationPartyTaskType
            };

            //This order is important
            votemanager.ProcessVotingResponse(userVoteApprove, 1294);
            votemanager.ProcessVotingResponse(userVoteDeny, 1); //Cofounder
            votemanager.ProcessVotingResponse(userVoteDeny, 2475);
            votemanager.ProcessVotingResponse(userVoteDeny, 2847);
            votemanager.ProcessVotingResponse(userVoteApprove, 4237);  //Cofounder
            votemanager.ProcessVotingResponse(userVoteApprove, 9930);  //Founder


        }
        private void VerifyApprovalAfterCoFounderNomination(Guid taskId, PartyNominationDTO partyDTO, int voteNeeded)
        {
            string getUserVoteSelectedsql = string.Format("Select * from UserVoteSelectedChoice Where TaskId = '{0}'", taskId);

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 7);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyNominationElectionApprovalChoiceId), 3);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyNominationElectionDenialChoiceId), 3);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.NotifyNominationRequestApprovalChoiceId), 1);


            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 4237).Score, AppSettings.CoFounderVoteScore);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 2475).Score, 1);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 9930).Score, AppSettings.FounderVoteScore);

            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId = {0} or UserId ={1}", partyDTO.NomineeId, partyDTO.InitatorId);

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 10);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == partyDTO.InitatorId), 8);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == partyDTO.NomineeId), 2);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId), 5);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId), 1);



            string notifyCountParmText = "4878|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|" + voteNeeded + "|" + partyDTO.GetPartyMemberType() + "|32";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Contains(notifyCountParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Count(f => f == '|'), 9);


            string notifyNomineeParmText = partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId).Parms.Contains(notifyNomineeParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId).Parms.Count(f => f == '|'), 2);

            string notifyNomineeResultParmText = "4878|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Approved";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Contains(notifyNomineeResultParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Count(f => f == '|'), 8);


            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}'", partyDTO.InitatorId, "C");

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 7);

            ///Check PartyMember
            string gertPartyMemberSql = "Select * from PartyMember";

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Find(f => f.MemberStatus == string.Empty && f.UserId == partyDTO.NomineeId).MemberType, "C");

            Assert.AreEqual(partyMember.Find(f => f.MemberStatus == "N" && f.UserId == partyDTO.NomineeId).MemberType, "M");


            ///Check Politicalparty
            string gertPartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", partyDTO.PartyId);

            List<DAO.Models.PoliticalParty> polticalParty = spContext.GetSqlData<DAO.Models.PoliticalParty>(gertPartySql).ToList();

            Assert.AreEqual(polticalParty[0].CoFounderSize, 3);

            ///Check Post
            string getPostSql = "Select * from Post";
            string postContentResult = @"1|6.jpg|Steve Johnston|4878|7.jpg|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            string partyNominationtext = @"4878|7.jpg|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 2);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationPostContentTypeId).Parms.Contains(postContentResult));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationPostContentTypeId).Parms.Contains("||"));
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationElectionPostContentTypeId).Parms.Contains(partyNominationtext));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationElectionPostContentTypeId).Parms.Contains("||"));
        }

        #endregion Nominate CoFounder

        #region Nominate CoFounder Decliend
        [Category("Party Nominate")]
        [Test]
        public void TestPartyNominationCoFounderDenialRequest()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            Guid taskId = new Guid();
            PartyNominationDTO partyDTO = RequestNomination("C");
            taskId = TestPartyNominationRequest_CheckUserTask(partyDTO);
            TestPartyNominationRequest_CheckUserNotifcation(partyDTO);
            AcceptNomination(partyDTO, taskId);

            PoliticalParty partyInfo = partyRepo.GetPartyById(partyDTO.PartyId);
            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;
            VoteDenialNomination(taskId, partyDTO);

            double voteNeeded = totalVoters * AppSettings.PartyNominationCoFounderApprovalVoteNeeded;
            VerifyDenialAfterCoFounderNomination(taskId, partyDTO, Convert.ToInt32(Math.Ceiling(voteNeeded)));
            CheckCacheRequestNomination(partyDTO, string.Empty);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");

        }
        private void VoteDenialNomination(Guid taskId, PartyNominationDTO partyDTO)
        {
            UserVoteManager votemanager = new UserVoteManager();


            string[] users = partyRepo.GetAllPartyMember(partyDTO.PartyId);

            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyNominationElectionApprovalChoiceId },
                ChoiceRadioId = AppSettings.PartyNominationElectionApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.NominationPartyTaskType
            };

            VoteResponseDTO userVoteDeny = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyNominationElectionDenialChoiceId },
                ChoiceRadioId = AppSettings.PartyNominationElectionDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.NominationPartyTaskType
            };

            //This order is important
            votemanager.ProcessVotingResponse(userVoteApprove, 1294);
            votemanager.ProcessVotingResponse(userVoteDeny, 1); //Cofounder
            votemanager.ProcessVotingResponse(userVoteDeny, 2475);
            votemanager.ProcessVotingResponse(userVoteDeny, 2847);
            votemanager.ProcessVotingResponse(userVoteDeny, 4237);  //Cofounder
            votemanager.ProcessVotingResponse(userVoteDeny, 9930);  //Founder
        }

        private void VerifyDenialAfterCoFounderNomination(Guid taskId, PartyNominationDTO partyDTO, int voteNeeded)
        {
            string getUserVoteSelectedsql = string.Format("Select * from UserVoteSelectedChoice Where TaskId = '{0}'", taskId);

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 7);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyNominationElectionApprovalChoiceId), 1);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyNominationElectionDenialChoiceId), 5);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.NotifyNominationRequestApprovalChoiceId), 1);


            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 4237).Score, AppSettings.CoFounderVoteScore);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 2475).Score, 1);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 9930).Score, AppSettings.FounderVoteScore);

            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId = {0} or UserId ={1}", partyDTO.NomineeId, partyDTO.InitatorId);

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 10);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == partyDTO.InitatorId), 8);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == partyDTO.NomineeId), 2);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId), 5);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId), 2);



            string notifyCountParmText = "4878|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|" + voteNeeded + "|" + partyDTO.GetPartyMemberType() + "|32";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Contains(notifyCountParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Count(f => f == '|'), 9);



            string notifyNomineeResultParmText = "4878|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Denied";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Contains(notifyNomineeResultParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Count(f => f == '|'), 8);


            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}'", partyDTO.InitatorId, "C");

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 7);

            ///Check PartyMember
            string gertPartyMemberSql = "Select * from PartyMember";

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Find(f => f.MemberStatus == string.Empty && f.UserId == partyDTO.NomineeId).MemberType, "M");


            ///Check PartyMember
            string gertPartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", partyDTO.PartyId);

            List<DAO.Models.PoliticalParty> polticalParty = spContext.GetSqlData<DAO.Models.PoliticalParty>(gertPartySql).ToList();

            Assert.AreEqual(polticalParty[0].CoFounderSize, 2);
            ///Check Post
            string getPostSql = "Select * from Post";
            string postContentResult = @"1|6.jpg|Steve Johnston|4878|7.jpg|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 1);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationPostContentTypeId).Parms.Contains(postContentResult));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationPostContentTypeId).Parms.Contains("||"));
        }
        private void CheckCacheRequestNomination(PartyNominationDTO partyDTO, string newMemberType)
        {


            Assert.AreEqual(partyRepo.GetMemberStatus(partyDTO.NomineeId), string.Empty);
            GetPartyMemberTypeDTO member = new GetPartyMemberTypeDTO
            {
                MemberType = "C",
                PartyId = new Guid(partyDTO.PartyId)
            };

            List<PartyMemberDTO> partyMembers = JsonConvert.DeserializeObject<List<PartyMemberDTO>>(partyRepo.GetPartyMembers(member));

            if (newMemberType == string.Empty)
            {
                Assert.AreEqual(partyMembers.Count(f => f.MemberType == "C"), 2);
                Assert.AreEqual(partyRepo.GetPartyMemberType(partyDTO.NomineeId), "M");

            }
            else if (newMemberType == "C")
            {
                Assert.AreEqual(partyMembers.Count(f => f.MemberType == "C"), 3);
                Assert.AreEqual(partyRepo.GetPartyMemberType(partyDTO.NomineeId), "C");

            }


        }

        #endregion Nominate CoFounder Decliend

        #region Nominate Founder
        [Category("Party Nominate")]
        [Test]
        public void TestPartyNominationFounderApprovalRequest()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            Guid taskId = new Guid();
            PartyNominationDTO partyDTO = RequestNomination("F");
            taskId = TestPartyNominationRequest_CheckUserTask(partyDTO);
            TestPartyNominationRequest_CheckUserNotifcation(partyDTO);
            AcceptNomination(partyDTO, taskId);

            PoliticalParty partyInfo = partyRepo.GetPartyById(partyDTO.PartyId);
            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;
            VoteApprovalonNomination(taskId, partyDTO);

            double voteNeeded = totalVoters * AppSettings.PartyNominationFounderApprovalVoteNeeded;

            VerifyApprovalAfterFounderNomination(taskId, partyDTO, Convert.ToInt32(Math.Ceiling(voteNeeded)), partyInfo.PartyFounder);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");

        }
        private void VerifyApprovalAfterFounderNomination(Guid taskId, PartyNominationDTO partyDTO, int voteNeeded, int oldFounder)
        {
            string getUserVoteSelectedsql = string.Format("Select * from UserVoteSelectedChoice Where TaskId = '{0}'", taskId);

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 7);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyNominationElectionApprovalChoiceId), 3);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyNominationElectionDenialChoiceId), 3);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.NotifyNominationRequestApprovalChoiceId), 1);


            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 4237).Score, AppSettings.CoFounderVoteScore);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 2475).Score, 1);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 9930).Score, AppSettings.FounderVoteScore);

            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId = {0} or UserId ={1} or Userid ={2}", partyDTO.NomineeId, partyDTO.InitatorId, oldFounder);
            string parmDemotion = @"7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|4878|Randy Wheeler";
            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 12);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == partyDTO.InitatorId), 8);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == partyDTO.NomineeId), 2);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == oldFounder), 2);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId), 5);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNotifyDemotionNominationNotificationId), 1);

            string notifyCountParmText = "4878|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|" + voteNeeded + "|" + partyDTO.GetPartyMemberType() + "|32";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Contains(notifyCountParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationVotingCountNotificationId).Parms.Count(f => f == '|'), 9);

            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyDemotionNominationNotificationId).Parms.Contains(parmDemotion));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyDemotionNominationNotificationId).Parms.Contains("||"));

            string notifyNomineeParmText = "" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId).Parms.Contains(notifyNomineeParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyCongratulationNominationNotificationId).Parms.Count(f => f == '|'), 2);

            string notifyNomineeResultParmText = "4878|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Approved";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Contains(notifyNomineeResultParmText));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Contains("||"));
            Assert.AreEqual(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNominationResultNotificationId).Parms.Count(f => f == '|'), 8);


            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}'", partyDTO.InitatorId, "C");

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 7);

            ///Check PartyMember
            string gertPartyMemberSql = "Select * from PartyMember";

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Find(f => f.MemberStatus == string.Empty && f.UserId == partyDTO.NomineeId).MemberType, "F");

            Assert.AreEqual(partyMember.Find(f => f.MemberStatus == "N" && f.UserId == partyDTO.NomineeId).MemberType, "M");




            ///Check PoliticalParty
            string gertPartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", partyDTO.PartyId);

            List<DAO.Models.PoliticalParty> polticalParty = spContext.GetSqlData<DAO.Models.PoliticalParty>(gertPartySql).ToList();

            Assert.AreEqual(polticalParty[0].CoFounderSize, 2);
            Assert.AreEqual(partyMember.Find(f => f.MemberStatus == "" && f.PartyId.ToString() == partyDTO.PartyId && f.MemberType == "F").UserId, partyDTO.NomineeId);

            ///Check Post
            string getPostSql = "Select * from Post";
            string postContentResult = @"1|6.jpg|Steve Johnston|4878|7.jpg|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            string partyNominationtext = @"4878|7.jpg|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 2);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationPostContentTypeId).Parms.Contains(postContentResult));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationPostContentTypeId).Parms.Contains("||"));
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationElectionPostContentTypeId).Parms.Contains(partyNominationtext));

            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyNominationElectionPostContentTypeId).Parms.Contains("||"));
        }

        #endregion Nominate Founder

        #region RejectNomination
        [Test]
        [Category("Party Nominate")]
        public void TestPartyNominationCoFounderRequestDenialByNominee()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            Guid taskId = new Guid();
            PartyNominationDTO partyDTO = RequestNomination("C");
            taskId = TestPartyNominationRequest_CheckUserTask(partyDTO);
            TestPartyNominationRequest_CheckUserNotifcation(partyDTO);
            DeclineNomination(partyDTO, taskId);
            CheckDeclineNomination(partyDTO);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");

        }
        private void DeclineNomination(PartyNominationDTO partyDTO, Guid taskId)
        {
            VoteResponseDTO userVote = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.NotifyNominationRequestDenialChoiceId },
                ChoiceRadioId = AppSettings.NotifyNominationRequestDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.NominationNotifyPartyTaskType
            };
            UserVoteManager votemanager = new UserVoteManager();
            votemanager.ProcessVotingResponse(userVote, partyDTO.NomineeId);

        }
        private void CheckDeclineNomination(PartyNominationDTO partyDTO)
        {
            string getUserTasksql = "Select * from UserTask ";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTasksql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userTask.Count(f => f.Status == "C"), 1);

            string getUserNotificationsql = "Select * from UserNotification ";
            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyNotifyRejectNominationNotificationId), 1);

            string parmText = @"4878|Randy Wheeler|CoFounder|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyRejectNominationNotificationId).Parms.Contains(parmText));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyNotifyRejectNominationNotificationId).Parms.Contains("||"));
        }

        #endregion RejectNomination

        #region Co Founder Size >20 or 20%
        [Category("Party Nominate")]
        [Test]
        public void RequestNominationWithCoFounderMoreThanCap()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            AddMoreMembers("7b80127f-8b05-41cc-bd54-9c26baacea2c");
            PartyNominationDTO partyDTO = RequestNomination("C");
            TestRequestNominationWithCoFounderMoreThanCap(partyDTO);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");

        }

        private void AddMoreMembers(string partyId)
        {

            string[] users = partyRepo.GetAllPartyMember(partyId);

            for (int i = 10000; i < 10050; i++)
            {

                PartyMember member = new PartyMember
                {
                    UserId = i,
                    PartyId = new Guid(partyId),
                    MemberType = "C",
                    MemberStatus = string.Empty,
                    MemberStartDate = DateTime.UtcNow,
                    MemberEndDate = null,
                    DonationAmount = 0
                };

                spContext.Add(member);
            }
            PoliticalParty partyInfo = partyRepo.GetPartyById(partyId);
            partyInfo.PartySize += 49;
            partyInfo.CoFounderSize += 49;
            spContext.Update(partyInfo);
            cache.FlushAllDatabase();


        }
        private void TestRequestNominationWithCoFounderMoreThanCap(PartyNominationDTO partyDTO)
        {
            string getUserNotifcationsql = "Select * from UserNotification";

            string parmTextTask = @"1|Randy Wheeler|" + partyDTO.GetPartyMemberType() + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotifcationsql).ToList();

            Assert.AreEqual(userNotification.Count(), 1);
            Assert.IsTrue(userNotification[0].Parms.Contains(parmTextTask));
            Assert.IsFalse(userNotification[0].Parms.Contains("||"));

        }
        #endregion Co Founder Size >20 or 20%

        #endregion Party Nomination

        #region Ejection Flow
        #region Approved Ejection

        #region Approved Ejection Member
        [Category("EjectionFlow")]
        [Test]
        public void EjectMemberApproved()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            EjectPartyDTO ejectionDto = new EjectPartyDTO
            {
                EjecteeId = 4878,
                InitatorId = 1,
                PartyId = "7b80127f-8b05-41cc-bd54-9c26baacea2c",
                InitiatorFullName = webRepo.GetFullName(9930),
                EjecteeFullName = webRepo.GetFullName(1),
            };
            string ejecteeIdPicture = webRepo.GetUserPicture(ejectionDto.EjecteeId);
            string InitatorIdPicture = webRepo.GetUserPicture(ejectionDto.InitatorId);

            Guid taskId = RequestEjection(ejectionDto);
            VoteEjectionApproval(ejectionDto, taskId);
            CheckVoteEjectionApproval(ejectionDto);
            CheckEjectionApproval(ejectionDto, ejecteeIdPicture, InitatorIdPicture);
            CheckPartyAndMemberAfterMemberEjectionApproval(ejectionDto);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private Guid RequestEjection(EjectPartyDTO ejectionDto)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessEjectMember(ejectionDto);

            string getUserNotificationSql = "Select * from UserNotification";

            string parmSuccessNotification = @"7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint";
            string parmEjectNotification = @"" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|" + ejectionDto.InitatorId + "|" + ejectionDto.InitiatorFullName + "|<strong>";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 10);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectVotingRequestNotificationId), 9);

            Assert.AreEqual(userNotification.Count(f => f.UserId == ejectionDto.EjecteeId), 0);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectSuccessNotificationId), 1);


            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingRequestNotificationId).Parms.Contains(parmEjectNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingRequestNotificationId).Parms.Contains("||"));
            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectSuccessNotificationId).Parms.Contains(parmSuccessNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectSuccessNotificationId).Parms.Contains("||"));

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingRequestNotificationId).Parms.Count(f => f == '|'), 7);


            string getUserTaskSql = "Select * from UserTask";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();

            Assert.AreEqual(userTask.Count(), 9);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.EjectPartyTaskType), 9);
            Assert.AreEqual(userTask.Count(f => f.UserId == ejectionDto.EjecteeId), 0);

            Assert.AreEqual(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Count(f => f == '|'), 7);
            string parmTaskEjection = @"" + ejectionDto.InitatorId + "|" + ejectionDto.InitiatorFullName + "|" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";
            Assert.IsTrue(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains(parmTaskEjection));
            Assert.IsFalse(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains("||"));
            Guid taskId = userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).TaskId;

            string getpartymemberSql = "Select * from PartyMember";
            List<PartyMember> partymember = spContext.GetSqlData<PartyMember>(getpartymemberSql).ToList();

            Assert.IsTrue(partymember.Find(f => f.UserId == ejectionDto.EjecteeId && f.PartyId.ToString() == ejectionDto.PartyId).MemberStatus == "E");
            return taskId;
        }
        private void VoteEjectionApproval(EjectPartyDTO ejectionDto, Guid taskId)
        {

            UserVoteManager votemanager = new UserVoteManager();
            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyEjectionApprovalChoiceId },
                ChoiceRadioId = AppSettings.PartyEjectionApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.EjectPartyTaskType
            };

            VoteResponseDTO userVoteDeny = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyEjectionDenialChoiceId },
                ChoiceRadioId = AppSettings.PartyEjectionDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.EjectPartyTaskType
            };

            //This order is important
            votemanager.ProcessVotingResponse(userVoteApprove, 1294);
            votemanager.ProcessVotingResponse(userVoteDeny, 2475);
            votemanager.ProcessVotingResponse(userVoteDeny, 2847);
            votemanager.ProcessVotingResponse(userVoteApprove, 4237);  //Cofounder
            votemanager.ProcessVotingResponse(userVoteApprove, 9930);  //Founder
            votemanager.ProcessVotingResponse(userVoteApprove, 7347);
            votemanager.ProcessVotingResponse(userVoteApprove, 9256);
        }
        private void CheckVoteEjectionApproval(EjectPartyDTO ejectionDto)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmCountNotification = @"" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";
            string parmEjectNotificationResult = @"" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Approved";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 20);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId), 8);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId), 2);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Count(f => f == '|'), 7);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Count(f => f == '|'), 7);

            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Contains(parmEjectNotificationResult));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Contains("||"));

            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Contains(parmCountNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Contains("||"));

            string getUserTaskSql = "Select * from UserTask";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();

            Assert.AreEqual(userTask.Count(), 5);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.EjectPartyTaskType), 5);
            Assert.AreEqual(userTask.Count(f => f.UserId == ejectionDto.EjecteeId), 0);
            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 5);

            string parmText = @"" + ejectionDto.InitatorId + "|" + ejectionDto.InitiatorFullName + "|" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";
            Assert.IsTrue(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains(parmText));
            Assert.IsFalse(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains("||"));
            Assert.AreEqual(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Count(f => f == '|'), 7);
        }
        private void CheckEjectionApproval(EjectPartyDTO ejectionDto, string ejecteeIdPicture, string InitatorIdPicture)
        {
            string getPostSql = "Select * from Post";
            string parmPost = @"" + ejectionDto.EjecteeId + "|" + ejecteeIdPicture + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Approved";

            List<Post> userPost = spContext.GetSqlData<Post>(getPostSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userPost.Count(), 1);
            Assert.AreEqual(userPost.Count(f => f.PostContentTypeId == AppSettings.PartyEjectionPostContentTypeId), 1);
            Assert.IsTrue(userPost.Find(f => f.PostContentTypeId == AppSettings.PartyEjectionPostContentTypeId).Parms.Contains(parmPost));
            Assert.IsFalse(userPost.Find(f => f.PostContentTypeId == AppSettings.PartyEjectionPostContentTypeId).Parms.Contains("||"));
            string getUserVoteSql = "Select * from UserVoteSelectedChoice";
            List<UserVoteSelectedChoice> userVote = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSql).ToList();
            Assert.AreEqual(userVote.Count(), 5);

            Assert.AreEqual(userVote.Count(f => f.ChoiceId == AppSettings.PartyEjectionApprovalChoiceId), 3);
            Assert.AreEqual(userVote.Count(f => f.ChoiceId == AppSettings.PartyEjectionDenialChoiceId), 2);



        }

        private void CheckPartyAndMemberAfterMemberEjectionApproval(EjectPartyDTO ejectionDto)
        {


            PoliticalParty partyInfo =
            partyRepo.GetPartyById(ejectionDto.PartyId);

            Assert.AreEqual(partyInfo.PartySize, 9);
            Assert.AreEqual(partyInfo.CoFounderSize, 2);

            PartyMember partymemberActiveParty = partyRepo.GetActiveUserParty(ejectionDto.EjecteeId);
            Assert.AreEqual(partymemberActiveParty.MemberType, null);


            string getPartyMemberSql = string.Format("Select * from PartyMember Where PartyId='{0}'", ejectionDto.PartyId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(getPartyMemberSql).ToList();
            Assert.AreEqual(partyMember.Count(), 32);
            Assert.IsTrue(partyMember.Find(f => f.UserId == ejectionDto.EjecteeId).MemberStatus == "F");
            Assert.IsTrue(partyMember.Find(f => f.UserId == ejectionDto.EjecteeId).MemberEndDate != null);

            string getPartyEjection = string.Format("Select * from PartyEjection", ejectionDto.PartyId);

            List<PartyEjection> partyejection = spContext.GetSqlData<PartyEjection>(getPartyEjection).ToList();
            Assert.AreEqual(partyejection.Count(), 1);
            Assert.AreEqual(partyejection.Find(f => f.Status == "A").EjecteeId, ejectionDto.EjecteeId);

        }

        #endregion Approved Ejection Member

        #region Approved Ejection Founder
        [Category("EjectionFlow")]
        [Test]
        public void EjectFounderApproved()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            EjectPartyDTO ejectionDto = new EjectPartyDTO
            {
                EjecteeId = 9930,
                InitatorId = 1,
                PartyId = "7b80127f-8b05-41cc-bd54-9c26baacea2c",
                InitiatorFullName = webRepo.GetFullName(9930),
                EjecteeFullName = webRepo.GetFullName(1),
            };
            string ejecteeIdPicture = webRepo.GetUserPicture(ejectionDto.EjecteeId);
            string InitatorIdPicture = webRepo.GetUserPicture(ejectionDto.InitatorId);

            Guid taskId = RequestEjection(ejectionDto);
            VoteEjectionFounderApproval(ejectionDto, taskId);
            CheckVoteEjectionFounderApproval(ejectionDto);
            CheckEjectionFounderApproval(ejectionDto, ejecteeIdPicture, InitatorIdPicture);
            CheckPartyAndMemberAfterFounderEjectionApproval(ejectionDto);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void CheckEjectionFounderApproval(EjectPartyDTO ejectionDto, string ejecteeIdPicture, string InitatorIdPicture)
        {
            string getPostSql = "Select * from Post";
            string parmPost = @"" + ejectionDto.EjecteeId + "|" + ejecteeIdPicture + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Approved";

            List<Post> userPost = spContext.GetSqlData<Post>(getPostSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userPost.Count(), 1);
            Assert.AreEqual(userPost.Count(f => f.PostContentTypeId == AppSettings.PartyEjectionPostContentTypeId), 1);
            Assert.IsTrue(userPost.Find(f => f.PostContentTypeId == AppSettings.PartyEjectionPostContentTypeId).Parms.Contains(parmPost));
            Assert.IsFalse(userPost.Find(f => f.PostContentTypeId == AppSettings.PartyEjectionPostContentTypeId).Parms.Contains("||"));
            string getUserVoteSql = "Select * from UserVoteSelectedChoice";
            List<UserVoteSelectedChoice> userVote = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSql).ToList();
            Assert.AreEqual(userVote.Count(), 9);

            Assert.AreEqual(userVote.Count(f => f.ChoiceId == AppSettings.PartyEjectionApprovalChoiceId), 8);
            Assert.AreEqual(userVote.Count(f => f.ChoiceId == AppSettings.PartyEjectionDenialChoiceId), 1);



        }

        private void CheckPartyAndMemberAfterFounderEjectionApproval(EjectPartyDTO ejectionDto)
        {


            PoliticalParty partyInfo =
            partyRepo.GetPartyById(ejectionDto.PartyId);

            Assert.AreEqual(partyInfo.PartySize, 9);
            Assert.AreEqual(partyInfo.CoFounderSize, 2);
            Assert.AreEqual(partyInfo.PartyFounder, 0);

            PartyMember partymemberActiveParty = partyRepo.GetActiveUserParty(ejectionDto.EjecteeId);
            Assert.AreEqual(partymemberActiveParty.MemberType, null);


            string getPartyMemberSql = string.Format("Select * from PartyMember Where PartyId='{0}'", ejectionDto.PartyId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(getPartyMemberSql).ToList();
            Assert.AreEqual(partyMember.Count(), 32);
            Assert.IsTrue(partyMember.Find(f => f.UserId == ejectionDto.EjecteeId).MemberStatus == "F");
            Assert.IsTrue(partyMember.Find(f => f.UserId == ejectionDto.EjecteeId).MemberEndDate <= DateTime.UtcNow.AddMinutes(1));

            string getPartyEjection = string.Format("Select * from PartyEjection", ejectionDto.PartyId);

            List<PartyEjection> partyejection = spContext.GetSqlData<PartyEjection>(getPartyEjection).ToList();
            Assert.AreEqual(partyejection.Count(), 1);
            Assert.AreEqual(partyejection.Find(f => f.Status == "A").EjecteeId, ejectionDto.EjecteeId);

        }
        private void VoteEjectionFounderApproval(EjectPartyDTO ejectionDto, Guid taskId)
        {

            UserVoteManager votemanager = new UserVoteManager();
            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyEjectionApprovalChoiceId },
                ChoiceRadioId = AppSettings.PartyEjectionApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.EjectPartyTaskType
            };

            VoteResponseDTO userVoteDeny = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyEjectionDenialChoiceId },
                ChoiceRadioId = AppSettings.PartyEjectionDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.EjectPartyTaskType
            };

            //This order is important
            votemanager.ProcessVotingResponse(userVoteApprove, 1294);
            votemanager.ProcessVotingResponse(userVoteApprove, 2475);
            votemanager.ProcessVotingResponse(userVoteDeny, 2847);
            votemanager.ProcessVotingResponse(userVoteApprove, 4237);  //Cofounder
            votemanager.ProcessVotingResponse(userVoteApprove, 1);  //CoFounder
            votemanager.ProcessVotingResponse(userVoteApprove, 7347);
            votemanager.ProcessVotingResponse(userVoteApprove, 9256);
            votemanager.ProcessVotingResponse(userVoteApprove, 4878);
            votemanager.ProcessVotingResponse(userVoteApprove, 9288);
        }
        private void CheckVoteEjectionFounderApproval(EjectPartyDTO ejectionDto)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmCountNotification = @"" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";
            string parmEjectNotificationResult = @"" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Approved";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 28);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId), 16);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId), 2);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Count(f => f == '|'), 7);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Count(f => f == '|'), 7);

            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Contains(parmEjectNotificationResult));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Contains("||"));
            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Contains(parmCountNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Contains("||"));

            string getUserTaskSql = "Select * from UserTask";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();

            Assert.AreEqual(userTask.Count(), 9);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.EjectPartyTaskType), 9);
            Assert.AreEqual(userTask.Count(f => f.UserId == ejectionDto.EjecteeId), 0);
            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 9);

            string parmText = @"" + ejectionDto.InitatorId + "|" + ejectionDto.InitiatorFullName + "|" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";
            Assert.IsTrue(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains(parmText));
            Assert.IsFalse(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains("||"));
            Assert.AreEqual(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Count(f => f == '|'), 7);
        }

        #endregion Approved Ejection Founder

        #region Approved Ejection CoFounder

        [Category("EjectionFlow")]
        [Test]
        public void EjectCoFounderApproved()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            EjectPartyDTO ejectionDto = new EjectPartyDTO
            {
                EjecteeId = 1,
                InitatorId = 9930,
                PartyId = "7b80127f-8b05-41cc-bd54-9c26baacea2c",
                InitiatorFullName = webRepo.GetFullName(9930),
                EjecteeFullName = webRepo.GetFullName(1),
            };
            string ejecteeIdPicture = webRepo.GetUserPicture(ejectionDto.EjecteeId);
            string InitatorIdPicture = webRepo.GetUserPicture(ejectionDto.InitatorId);

            Guid taskId = RequestEjection(ejectionDto);
            VoteEjectionApproval(ejectionDto, taskId);
            CheckVoteEjectionApproval(ejectionDto);
            CheckEjectionApproval(ejectionDto, ejecteeIdPicture, InitatorIdPicture);
            CheckPartyAndMemberAfterCoFounderEjectionApproval(ejectionDto);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void CheckPartyAndMemberAfterCoFounderEjectionApproval(EjectPartyDTO ejectionDto)
        {


            PoliticalParty partyInfo =
            partyRepo.GetPartyById(ejectionDto.PartyId);

            Assert.AreEqual(partyInfo.PartySize, 9);
            Assert.AreEqual(partyInfo.CoFounderSize, 1);

            PartyMember partymemberActiveParty = partyRepo.GetActiveUserParty(ejectionDto.EjecteeId);
            Assert.AreEqual(partymemberActiveParty.MemberType, null);


            string getPartyMemberSql = string.Format("Select * from PartyMember Where PartyId='{0}'", ejectionDto.PartyId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(getPartyMemberSql).ToList();
            Assert.AreEqual(partyMember.Count(), 32);
            Assert.IsTrue(partyMember.Find(f => f.UserId == ejectionDto.EjecteeId).MemberStatus == "F");
            Assert.IsTrue(partyMember.Find(f => f.UserId == ejectionDto.EjecteeId).MemberEndDate != null);

            string getPartyEjection = string.Format("Select * from PartyEjection", ejectionDto.PartyId);

            List<PartyEjection> partyejection = spContext.GetSqlData<PartyEjection>(getPartyEjection).ToList();
            Assert.AreEqual(partyejection.Count(), 1);
            Assert.AreEqual(partyejection.Find(f => f.Status == "A").EjecteeId, ejectionDto.EjecteeId);

        }
        #endregion Approved Ejection CoFounder

        #endregion Approved Ejection

        #region Denied Ejection
        [Category("DeniedEjection")]
        [Test]
        public void EjectMemberDenied()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            EjectPartyDTO ejectionDto = new EjectPartyDTO
            {
                EjecteeId = 4878,
                InitatorId = 1,
                PartyId = "7b80127f-8b05-41cc-bd54-9c26baacea2c",
                InitiatorFullName = webRepo.GetFullName(9930),
                EjecteeFullName = webRepo.GetFullName(1),
            };
            string ejecteeIdPicture = webRepo.GetUserPicture(ejectionDto.EjecteeId);
            string InitatorIdPicture = webRepo.GetUserPicture(ejectionDto.InitatorId);

            Guid taskId = RequestEjection(ejectionDto);
            VoteEjectionDenial(ejectionDto, taskId);
            CheckVoteEjectionDenial(ejectionDto);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }

        private void VoteEjectionDenial(EjectPartyDTO ejectionDto, Guid taskId)
        {
            UserVoteManager votemanager = new UserVoteManager();
            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyEjectionApprovalChoiceId },
                ChoiceRadioId = AppSettings.PartyEjectionApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.EjectPartyTaskType
            };

            VoteResponseDTO userVoteDeny = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyEjectionDenialChoiceId },
                ChoiceRadioId = AppSettings.PartyEjectionDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.EjectPartyTaskType
            };

            //This order is important
            votemanager.ProcessVotingResponse(userVoteDeny, 1294);
            votemanager.ProcessVotingResponse(userVoteApprove, 2475);
            votemanager.ProcessVotingResponse(userVoteDeny, 2847);
            votemanager.ProcessVotingResponse(userVoteDeny, 4237);  //Cofounder
            votemanager.ProcessVotingResponse(userVoteDeny, 9930);  //Founder
            votemanager.ProcessVotingResponse(userVoteDeny, 7347);
            votemanager.ProcessVotingResponse(userVoteApprove, 9256);
        }

        private void CheckVoteEjectionDenial(EjectPartyDTO ejectionDto)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmCountNotification = @"" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";
            string parmEjectNotificationResult = @"" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Denied";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 22);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId), 10);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId), 2);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Count(f => f == '|'), 7);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Count(f => f == '|'), 7);

            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Contains(parmEjectNotificationResult));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectResultNotificationId).Parms.Contains("||"));

            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Contains(parmCountNotification));

            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyEjectVotingCountNotificationId).Parms.Contains("||"));

            string getUserTaskSql = "Select * from UserTask";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();

            Assert.AreEqual(userTask.Count(), 6);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.EjectPartyTaskType), 6);
            Assert.AreEqual(userTask.Count(f => f.UserId == ejectionDto.EjecteeId), 0);
            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 6);

            string parmText = @"" + ejectionDto.InitatorId + "|" + ejectionDto.InitiatorFullName + "|" + ejectionDto.EjecteeId + "|" + ejectionDto.EjecteeFullName + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|";
            Assert.IsTrue(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains(parmText));
            Assert.IsFalse(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Contains("||"));
            Assert.AreEqual(userTask.Find(f => f.TaskTypeId == AppSettings.EjectPartyTaskType).Parms.Count(f => f == '|'), 7);
            string getpartymemberSql = "Select * from PartyMember";
            List<PartyMember> partymember = spContext.GetSqlData<PartyMember>(getpartymemberSql).ToList();

            Assert.IsTrue(partymember.Find(f => f.UserId == ejectionDto.EjecteeId).MemberStatus == string.Empty);
        }

        #endregion Denied Ejection


        #endregion Ejection Flow

        #region Party Join Request

        #region Party Join Request Approved

        [Category("PartyJoinRequest")]
        [Test]
        [TestCase(8, "b813ef5d-1f80-47f9-912f-05a68ca176d5", "Pass")]
        [TestCase(8, "b813ef5d-1f80-47f9-912f-05a68ca176d5", "Fail")] // Party in Close Status
        public void PartyJoinRequestApproved(int userId, string partyId, string expetcedResult)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            JoinRequestPartyDTO joinPartyDTO = new JoinRequestPartyDTO
            {
                PartyId = partyId,
                FullName = webRepo.GetFullName(userId),
                UserId = userId
            };
            string userPicture = webRepo.GetUserPicture(userId);

            JoinRequestPartyDTO[] joinPartyDTOs = new JoinRequestPartyDTO[1] { joinPartyDTO };

            Guid taskId = RequestJoinParty(joinPartyDTOs, userId);

            VotePartyJoinApproval(joinPartyDTO, taskId);

            Guid invitetaskId = CheckVotePartyJoinApproval(joinPartyDTO);
            if (expetcedResult == "Fail")
            {
                partyRepo.UpdatePartyStatus(partyId, "C");
                partyRepo.ExpireCachePoliticalParty(partyId, userId);
            }
            VotePartyJoinInvite(joinPartyDTO, invitetaskId, AppSettings.JoinPartyRequestInviteAcceptChoiceId);
            if (expetcedResult == "Fail")
            {
                CheckPartyJoinRequestFailed(userId);
            }
            else
            {
                CheckVotePartyJoinInviteAccept(joinPartyDTO, taskId);
            }

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void CheckPartyJoinRequestFailed(int userId)
        {
            //party not in Approved Status
            ///Check UserNotification

            string getUserNotificationsql = string.Format("Select * from UserNotification Where NotificationTypeId ={0}", AppSettings.PartyNotWelcomeNotificationId);

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            Assert.AreEqual(userNotifications.Count(), 1);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNotWelcomeNotificationId), 1);
            string parmtext = "party is already closed";
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(parmtext)), 1);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);


        }

        private Guid RequestJoinParty(JoinRequestPartyDTO[] joinPartyDTOs, int userId)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessRequestJoinParty(joinPartyDTOs, userId);

            string getUserNotificationSql = "Select * from UserNotification";

            string parmSuccessNotification = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|";
            string parmEjectNotification = @"" + userId + "|" + joinPartyDTOs[0].FullName + "|b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 18);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyApplyJoinVotingRequestNotificationId), 17);

            Assert.AreEqual(userNotification.Count(f => f.UserId == userId), 1);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId), 1);


            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinVotingRequestNotificationId).Parms.Contains(parmEjectNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinVotingRequestNotificationId).Parms.Contains("||"));
            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId).Parms.Contains(parmSuccessNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId).Parms.Contains("||"));

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinVotingRequestNotificationId).Parms.Count(f => f == '|'), 5);


            string getUserTaskSql = "Select * from UserTask";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();

            Assert.AreEqual(userTask.Count(), 17);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.JoinPartyRequestTaskType), 17);
            Assert.AreEqual(userTask.Count(f => f.UserId == userId), 0);

            Assert.AreEqual(userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestTaskType).Parms.Count(f => f == '|'), 5);
            string parmTaskEjection = @"" + userId + "|" + joinPartyDTOs[0].FullName + "|b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|";
            Assert.IsTrue(userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestTaskType).Parms.Contains(parmTaskEjection));
            Assert.IsFalse(userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestTaskType).Parms.Contains("||"));
            Guid taskId = userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestTaskType).TaskId;


            return taskId;
        }
        private void VotePartyJoinApproval(JoinRequestPartyDTO joinPartyDTO, Guid taskId)
        {

            UserVoteManager votemanager = new UserVoteManager();
            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.JoinPartyRequestApprovalChoiceId },
                ChoiceRadioId = AppSettings.JoinPartyRequestApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.JoinPartyRequestTaskType
            };

            VoteResponseDTO userVoteDeny = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.JoinPartyRequestDenialChoiceId },
                ChoiceRadioId = AppSettings.JoinPartyRequestDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.JoinPartyRequestTaskType
            };

            //This order is important
            votemanager.ProcessVotingResponse(userVoteApprove, 312);
            votemanager.ProcessVotingResponse(userVoteDeny, 399);
            votemanager.ProcessVotingResponse(userVoteDeny, 581);//Cofounder
            votemanager.ProcessVotingResponse(userVoteApprove, 1068);
            votemanager.ProcessVotingResponse(userVoteApprove, 1461);  //CoFounder
            votemanager.ProcessVotingResponse(userVoteApprove, 9856);  //Founder
        }
        private Guid CheckVotePartyJoinApproval(JoinRequestPartyDTO joinPartyDTO)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmJoinSuccessNotification = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|onclick: GoToTask()";
            string parmJoinInviteNotificationResult = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 19);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteNotificationId), 1);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId), 1);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId).Parms.Count(f => f == '|'), 2);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteNotificationId).Parms.Count(f => f == '|'), 2);

            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId).Parms.Contains(parmJoinInviteNotificationResult));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId).Parms.Contains("||"));
            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteNotificationId).Parms.Contains(parmJoinSuccessNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteNotificationId).Parms.Contains("||"));

            string getUserTaskSql = "Select * from UserTask";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();

            Assert.AreEqual(userTask.Count(), 7);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.JoinPartyRequestTaskType), 6);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.JoinPartyRequestInviteTaskType), 1);
            Assert.AreEqual(userTask.Count(f => f.UserId == joinPartyDTO.UserId), 1);
            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 6);

            string parmText = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|1842.99|";
            Assert.IsTrue(userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestInviteTaskType).Parms.Contains(parmText));
            Assert.IsFalse(userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestInviteTaskType).Parms.Contains("||"));
            Assert.AreEqual(userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestInviteTaskType).Parms.Count(f => f == '|'), 4);
            return userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyRequestInviteTaskType).TaskId;
        }

        private void VotePartyJoinInvite(JoinRequestPartyDTO joinPartyDTO, Guid invitetaskId, int choiceId)
        {

            UserVoteManager votemanager = new UserVoteManager();
            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { choiceId },
                ChoiceRadioId = choiceId,
                TaskId = invitetaskId,
                TaskTypeId = AppSettings.JoinPartyRequestInviteTaskType
            };


            //This order is important
            votemanager.ProcessVotingResponse(userVoteApprove, joinPartyDTO.UserId);
        }
        private void CheckVotePartyJoinInviteAccept(JoinRequestPartyDTO joinPartyDTO, Guid taskId)
        {
            string getUserVoteSelectedsql = "Select * from UserVoteSelectedChoice";

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 7);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestApprovalChoiceId), 4);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestDenialChoiceId), 2);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestInviteAcceptChoiceId), 1);


            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId = {0}", joinPartyDTO.UserId);

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 3);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyWelcomeNotificationId), 1);

            string welcomeparm = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyWelcomeNotificationId).Parms.Contains(welcomeparm));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyWelcomeNotificationId).Parms.Contains("||"));


            ///Check UserTask
            string gertUserTaskSql = "Select * from UserTask ";

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 7);
            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 7);

            ///Check PartyMember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where UserId ={0}", joinPartyDTO.UserId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Find(f => f.MemberStatus == null && f.UserId == joinPartyDTO.UserId).MemberType, "M");

            ///Check Politcialpary
            string gertPartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", joinPartyDTO.PartyId);

            List<DAO.Models.PoliticalParty> polticalParty = spContext.GetSqlData<DAO.Models.PoliticalParty>(gertPartySql).ToList();

            Assert.AreEqual(polticalParty[0].PartySize, 18);

            ///Check Post
            string getPostSql = "Select * from Post";
            string postContentResult = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm|8|138.jpg|Russell Williamson|Member";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 1);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.WelcomePartyPostContentTypeId).Parms.Contains(postContentResult));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.WelcomePartyPostContentTypeId).Parms.Contains("||"));

            //Check partyInvite
            string getPartyInviteSql = string.Format("Select * from PartyInvite Where UserId= {0}", joinPartyDTO.UserId);
            List<PartyInvite> partyInvite = spContext.GetSqlData<PartyInvite>(getPartyInviteSql).ToList();
            Assert.AreEqual(partyInvite.Count(f => f.Status == "A"), 1);

            //Check PartyJoinRequest
            string getPartyJoinRequestSql = string.Format("Select * from PartyJoinRequest Where UserId= {0}", joinPartyDTO.UserId);
            List<PartyJoinRequest> PartyJoinRequest = spContext.GetSqlData<PartyJoinRequest>(getPartyJoinRequestSql).ToList();
            Assert.AreEqual(PartyJoinRequest.Count(f => f.Status == "A"), 1);


        }
        #endregion Party Join Request Approved

        #region Party Join Request Denied

        [Category("PartyJoinRequest")]
        [Test]
        public void PartyJoinRequestDenied()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            int userId = 8;
            JoinRequestPartyDTO joinPartyDTO = new JoinRequestPartyDTO
            {
                PartyId = "b813ef5d-1f80-47f9-912f-05a68ca176d5",
                FullName = webRepo.GetFullName(userId),
                UserId = userId
            };
            string userPicture = webRepo.GetUserPicture(userId);

            JoinRequestPartyDTO[] joinPartyDTOs = new JoinRequestPartyDTO[1] { joinPartyDTO };

            Guid taskId = RequestJoinParty(joinPartyDTOs, userId);
            VotePartyJoinDenial(joinPartyDTO, taskId);
            CheckVotePartyJoinDenial(joinPartyDTO);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void VotePartyJoinDenial(JoinRequestPartyDTO joinPartyDTO, Guid taskId)
        {

            UserVoteManager votemanager = new UserVoteManager();
            VoteResponseDTO userVoteApprove = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.JoinPartyRequestApprovalChoiceId },
                ChoiceRadioId = AppSettings.JoinPartyRequestApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.JoinPartyRequestTaskType
            };

            VoteResponseDTO userVoteDeny = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.JoinPartyRequestDenialChoiceId },
                ChoiceRadioId = AppSettings.JoinPartyRequestDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.JoinPartyRequestTaskType
            };

            //This order is important
            votemanager.ProcessVotingResponse(userVoteApprove, 312);
            votemanager.ProcessVotingResponse(userVoteDeny, 399);
            votemanager.ProcessVotingResponse(userVoteDeny, 581);//Cofounder
            votemanager.ProcessVotingResponse(userVoteApprove, 1068);
            votemanager.ProcessVotingResponse(userVoteDeny, 1461);  //CoFounder
            votemanager.ProcessVotingResponse(userVoteDeny, 7225);  //CoFounder
            votemanager.ProcessVotingResponse(userVoteDeny, 5885);  //CoFounder
            votemanager.ProcessVotingResponse(userVoteDeny, 7279);
            votemanager.ProcessVotingResponse(userVoteDeny, 5375);
            votemanager.ProcessVotingResponse(userVoteDeny, 4763);
            votemanager.ProcessVotingResponse(userVoteDeny, 4640);
            votemanager.ProcessVotingResponse(userVoteDeny, 9856);  //Founder
        }
        private void CheckVotePartyJoinDenial(JoinRequestPartyDTO joinPartyDTO)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmJoinDenyNotification = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm";


            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 19);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteRejectNotificationId), 1);

            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId), 1);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestSuccessNotificationId).Parms.Count(f => f == '|'), 2);

            Assert.AreEqual(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteRejectNotificationId).Parms.Count(f => f == '|'), 1);



            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteRejectNotificationId).Parms.Contains(parmJoinDenyNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteRejectNotificationId).Parms.Contains("||"));
            string getUserTaskSql = "Select * from UserTask";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(getUserTaskSql).ToList();

            Assert.AreEqual(userTask.Count(), 12);
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.JoinPartyRequestTaskType), 12);

            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 12);
        }

        #endregion Party Join Request Denied

        #region Party Join Request Invitation Reject
        [Test]
        [Category("PartyJoinRequest")]
        public void PartyJoinRequestApprovedInvitationReject()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            int userId = 8;
            JoinRequestPartyDTO joinPartyDTO = new JoinRequestPartyDTO
            {
                PartyId = "b813ef5d-1f80-47f9-912f-05a68ca176d5",
                FullName = webRepo.GetFullName(userId),
                UserId = userId
            };
            string userPicture = webRepo.GetUserPicture(userId);

            JoinRequestPartyDTO[] joinPartyDTOs = new JoinRequestPartyDTO[1] { joinPartyDTO };

            Guid taskId = RequestJoinParty(joinPartyDTOs, userId);
            VotePartyJoinApproval(joinPartyDTO, taskId);
            Guid invitetaskId = CheckVotePartyJoinApproval(joinPartyDTO);
            VotePartyJoinInvite(joinPartyDTO, invitetaskId, AppSettings.JoinPartyRequestInviteRejectChoiceId);
            CheckVotePartyJoinInviteReject(joinPartyDTO, taskId);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }

        private void CheckVotePartyJoinInviteReject(JoinRequestPartyDTO joinPartyDTO, Guid taskId)
        {
            string getUserVoteSelectedsql = "Select * from UserVoteSelectedChoice";

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 7);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestApprovalChoiceId), 4);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestDenialChoiceId), 2);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestInviteRejectChoiceId), 1);


            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId = {0}", joinPartyDTO.UserId);

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 2);



            ///Check UserTask
            string gertUserTaskSql = "Select * from UserTask ";

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 7);
            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 7);

            ///Check PartyMember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where UserId ={0}", joinPartyDTO.UserId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Count(f => f.MemberStatus == null && f.UserId == joinPartyDTO.UserId), 0);

            ///Check Politcialpary
            string gertPartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", joinPartyDTO.PartyId);

            List<DAO.Models.PoliticalParty> polticalParty = spContext.GetSqlData<DAO.Models.PoliticalParty>(gertPartySql).ToList();

            Assert.AreEqual(polticalParty[0].PartySize, 17);

            ///Check Post
            string getPostSql = "Select * from Post";
            string postContentResult = @"8|138.jpg|Russell Williamson|b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 1);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyJoinRequestInvitationDumpedPostContentTypeId).Parms.Contains(postContentResult));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyJoinRequestInvitationDumpedPostContentTypeId).Parms.Contains("||"));
            //Check partyInvite
            string getPartyInviteSql = string.Format("Select * from PartyInvite Where UserId= {0}", joinPartyDTO.UserId);
            List<PartyInvite> partyInvite = spContext.GetSqlData<PartyInvite>(getPartyInviteSql).ToList();
            Assert.AreEqual(partyInvite.Count(f => f.Status == "D"), 1);

            //Check PartyJoinRequest
            string getPartyJoinRequestSql = string.Format("Select * from PartyJoinRequest Where UserId= {0}", joinPartyDTO.UserId);
            List<PartyJoinRequest> PartyJoinRequest = spContext.GetSqlData<PartyJoinRequest>(getPartyJoinRequestSql).ToList();
            Assert.AreEqual(PartyJoinRequest.Count(f => f.Status == "D"), 1);


        }
        #endregion Party Join Request Invitation Reject

        #region Party Join Request Another Party Affilications
        [Category("PartyJoinRequest")]
        [Test]
        public void PartyJoinRequestAnotherPartyAffiliations()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            int userId = 1294;
            JoinRequestPartyDTO joinPartyDTO = new JoinRequestPartyDTO
            {
                PartyId = "b813ef5d-1f80-47f9-912f-05a68ca176d5",
                FullName = webRepo.GetFullName(userId),
                UserId = userId
            };
            string userPicture = webRepo.GetUserPicture(userId);

            JoinRequestPartyDTO[] joinPartyDTOs = new JoinRequestPartyDTO[1] { joinPartyDTO };

            Guid taskId = RequestJoinParty(joinPartyDTOs, userId);
            VotePartyJoinApproval(joinPartyDTO, taskId);
            Guid invitetaskId = CheckVotePartyJoinApproval(joinPartyDTO);
            VotePartyJoinInvite(joinPartyDTO, invitetaskId, AppSettings.JoinPartyRequestInviteAcceptChoiceId);
            CheckVotePartyJoinInviteAcceptAnotherPartyAffiliationOrNoCash(joinPartyDTO, taskId, "M");

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void CheckVotePartyJoinInviteAcceptAnotherPartyAffiliationOrNoCash(JoinRequestPartyDTO joinPartyDTO, Guid taskId, string ejectionStaus)
        {
            string getUserVoteSelectedsql = "Select * from UserVoteSelectedChoice";

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 7);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestApprovalChoiceId), 4);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestDenialChoiceId), 2);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.JoinPartyRequestInviteAcceptChoiceId), 1);

            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId = {0}", joinPartyDTO.UserId);

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 3);

            string welcomeparm = @"b813ef5d-1f80-47f9-912f-05a68ca176d5|Jabberstorm";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotWelcomeNotificationId).Parms.Contains(welcomeparm));
            Assert.IsFalse(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyNotWelcomeNotificationId).Parms.Contains("||"));


            ///Check UserTask
            string gertUserTaskSql = "Select * from UserTask ";

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 7);
            Assert.AreEqual(userTask.Count(f => f.CompletionPercent == 100), 7);

            ///Check PartyMember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where UserId ={0}", joinPartyDTO.UserId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Count(f => f.PartyId.ToString() == joinPartyDTO.PartyId && f.UserId == joinPartyDTO.UserId), 0);

            ///Check Politcialpary
            string gertPartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", joinPartyDTO.PartyId);

            List<DAO.Models.PoliticalParty> polticalParty = spContext.GetSqlData<DAO.Models.PoliticalParty>(gertPartySql).ToList();

            Assert.AreEqual(polticalParty[0].PartySize, 17);

            //Check partyInvite
            string getPartyInviteSql = string.Format("Select * from PartyInvite Where UserId= {0}", joinPartyDTO.UserId);
            List<PartyInvite> partyInvite = spContext.GetSqlData<PartyInvite>(getPartyInviteSql).ToList();
            Assert.AreEqual(partyInvite.Count(f => f.Status == ejectionStaus), 1);


            //Check PartyJoinRequest
            string getPartyJoinRequestSql = string.Format("Select * from PartyJoinRequest Where UserId= {0}", joinPartyDTO.UserId);
            List<PartyJoinRequest> PartyJoinRequest = spContext.GetSqlData<PartyJoinRequest>(getPartyJoinRequestSql).ToList();
            Assert.AreEqual(PartyJoinRequest.Count(f => f.Status == "D"), 1);

        }
        #endregion Party Join Request Another Party Affilications

        #region Party Join Request no Cash
        [Category("PartyJoinRequest")]
        [Test]
        public void PartyJoinRequestNoCash()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            int userId = 8;
            JoinRequestPartyDTO joinPartyDTO = new JoinRequestPartyDTO
            {
                PartyId = "b813ef5d-1f80-47f9-912f-05a68ca176d5",
                FullName = webRepo.GetFullName(userId),
                UserId = userId
            };
            string userPicture = webRepo.GetUserPicture(userId);

            JoinRequestPartyDTO[] joinPartyDTOs = new JoinRequestPartyDTO[1] { joinPartyDTO };

            Guid taskId = RequestJoinParty(joinPartyDTOs, userId);
            ClearOutCash(userId);
            VotePartyJoinApproval(joinPartyDTO, taskId);
            Guid invitetaskId = CheckVotePartyJoinApproval(joinPartyDTO);
            VotePartyJoinInvite(joinPartyDTO, invitetaskId, AppSettings.JoinPartyRequestInviteAcceptChoiceId);
            CheckVotePartyJoinInviteAcceptAnotherPartyAffiliationOrNoCash(joinPartyDTO, taskId, "N");

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void ClearOutCash(int userId)
        {
            IUserBankAccountDTORepository bankac = new UserBankAccountDTORepository();
            decimal cash = -bankac.GetUserBankDetails(userId).Cash;
            bankac.UpdateBankAc(cash, userId);
        }

        #endregion Party Join Request no Cash

        #region Party Join Request Already a Current party Member
        [Category("PartyJoinRequest")]
        [Test]
        public void PartyJoinRequestAlreadyaCurrentPartyMember()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            int userId = 543;
            JoinRequestPartyDTO joinPartyDTO = new JoinRequestPartyDTO
            {
                PartyId = "b813ef5d-1f80-47f9-912f-05a68ca176d5",
                FullName = webRepo.GetFullName(userId),
                UserId = userId
            };
            string userPicture = webRepo.GetUserPicture(userId);

            JoinRequestPartyDTO[] joinPartyDTOs = new JoinRequestPartyDTO[1] { joinPartyDTO };

            RequestJoinPartyAlreadyamember(joinPartyDTOs, userId);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void RequestJoinPartyAlreadyamember(JoinRequestPartyDTO[] joinPartyDTOs, int userId)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessRequestJoinParty(joinPartyDTOs, userId);

            string getUserNotificationSql = "Select * from UserNotification";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 1);
            Assert.AreEqual(userNotification.Count(f => f.UserId == userId), 1);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyApplyJoinRequestFailNotificationId), 1);


        }
        #endregion Party Join Request Already a Current party Member



        #endregion Party Join Request

        #region PartyDonation

        #region PartyDonation Approved
        [Category("PartyDonation")]
        [Test]
        [TestCase("7b80127f-8b05-41cc-bd54-9c26baacea2c")]
        [TestCase("0dca8472-0998-414c-aae6-1ad2e3148197")]
        public void PartyDonationApproved(string partyId)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            int userId = 1;
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            IUserBankAccountDTORepository bankrepo = new UserBankAccountDTORepository();

            DonatePartyDTO donationDTO = new DonatePartyDTO
            {
                PartyId = partyId,
                UserId = userId,
                Amount = 1000,
            };

            //Check partymember
            string getPartyMemberSql = string.Format("Select * from PartyMember Where UserId ={0} and PartyId ='{1}'", donationDTO.UserId, donationDTO.PartyId);
            List<PartyMember> oldPartymember = spContext.GetSqlData<PartyMember>(getPartyMemberSql).ToList();


            PartyMember partyMemberInfo = oldPartymember[0];
            PoliticalParty partyinfo = partyRepo.GetPartyById(partyId);
            WebUserDTO webData = webRepo.GetUserPicFName(userId);
            UserBankAccount bacnkAccount = bankrepo.GetUserBankDetails(userId);


            SendDonationAmount(donationDTO);
            CheckSendDonationAmount(donationDTO, partyinfo, bacnkAccount, partyMemberInfo);



            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void SendDonationAmount(DonatePartyDTO donationDTO)
        {
            PoliticalPartyManager manager = new PoliticalPartyManager();
            manager.ProcessDonation(donationDTO);
        }

        private void CheckSendDonationAmount(DonatePartyDTO donationDTO, PoliticalParty partyInfo, UserBankAccount bankac, PartyMember partyMemberInfo)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmDonationSuccessNotification = donationDTO.Amount + "|" + donationDTO.PartyId + "|" + partyInfo.PartyName;

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 1);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyDonateSuccessNotificationId), 1);
            Assert.AreEqual(userNotification[0].Parms.Count(f => f == '|'), 2);


            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyDonateSuccessNotificationId).Parms.Contains(parmDonationSuccessNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyDonateSuccessNotificationId).Parms.Contains("||"));
            //Check userb bankAccount
            string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId ={0}", donationDTO.UserId);
            List<UserBankAccount> newBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();

            Assert.AreEqual(newBankAccount[0].Cash + donationDTO.Amount, bankac.Cash);

            //Check userb partyAccount

            string getPoliticalpartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", donationDTO.PartyId);
            List<PoliticalParty> newpartyInfo = spContext.GetSqlData<PoliticalParty>(getPoliticalpartySql).ToList();

            Assert.AreEqual(newpartyInfo[0].TotalValue - donationDTO.Amount, partyInfo.TotalValue);

            //Check partymember
            string getPartyMemberSql = string.Format("Select * from PartyMember Where UserId ={0} and PartyId ='{1}'", donationDTO.UserId, donationDTO.PartyId);
            List<PartyMember> newPartymember = spContext.GetSqlData<PartyMember>(getPartyMemberSql).ToList();

            Assert.AreEqual(newPartymember[0].DonationAmount - donationDTO.Amount, partyMemberInfo.DonationAmount);

            //Check Capitaltransaction
            string getCapitalTransactionLogSql = "Select * from CapitalTransactionLog";
            List<CapitalTransactionLog> capitalTransactionLog = spContext.GetSqlData<CapitalTransactionLog>(getCapitalTransactionLogSql).ToList();
            Assert.AreEqual(capitalTransactionLog.Count(), 1);
            Assert.AreEqual(capitalTransactionLog[0].Amount, donationDTO.Amount);
            Assert.AreEqual(capitalTransactionLog[0].SourceId, donationDTO.UserId);
            Assert.AreEqual(capitalTransactionLog[0].RecipentGuid.ToString(), donationDTO.PartyId);

            ///Check Post
            ///
            string getPostSql = "Select * from Post";
            string postContentResult = @"";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 1);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyDonationContentTypeId).Parms.Contains(postContentResult));

            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyDonationContentTypeId).Parms.Contains("||"));

        }


        #endregion PartyDonation Approved

        #region PartyDonation NoCash
        [Category("PartyDonation")]
        [Test]
        public void PartyDonationNoCash()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            int userId = 1;
            string partyId = "7b80127f-8b05-41cc-bd54-9c26baacea2c";
            ClearOutCash(userId);
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            IUserBankAccountDTORepository bankrepo = new UserBankAccountDTORepository();

            PartyMember partyMemberInfo = partyRepo.GetActiveUserParty(userId);
            PoliticalParty partyinfo = partyRepo.GetPartyById(partyId);
            WebUserDTO webData = webRepo.GetUserPicFName(userId);
            UserBankAccount bacnkAccount = bankrepo.GetUserBankDetails(userId);

            DonatePartyDTO donationDTO = new DonatePartyDTO
            {
                PartyId = partyId,
                UserId = userId,
                Amount = 1000
            };
            SendDonationAmount(donationDTO);
            CheckSendDonationAmountNoCash(donationDTO, partyinfo, bacnkAccount, partyMemberInfo);



            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void CheckSendDonationAmountNoCash(DonatePartyDTO donationDTO, PoliticalParty partyInfo, UserBankAccount bankac, PartyMember partyMemberInfo)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmDonationFailNotification = donationDTO.Amount + "|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|unexpected reasons, please try again";

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 1);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyDonateFailNotificationId), 1);
            Assert.AreEqual(userNotification[0].Parms.Count(f => f == '|'), 3);


            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyDonateFailNotificationId).Parms.Contains(parmDonationFailNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyDonateFailNotificationId).Parms.Contains("||"));

            //Check userb bankAccount
            string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId ={0}", donationDTO.UserId);
            List<UserBankAccount> newBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();

            Assert.AreEqual(newBankAccount[0].Cash, bankac.Cash);

            //Check userb partyAccount

            string getPoliticalpartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", donationDTO.PartyId);
            List<PoliticalParty> newpartyInfo = spContext.GetSqlData<PoliticalParty>(getPoliticalpartySql).ToList();

            Assert.AreEqual(newpartyInfo[0].TotalValue, partyInfo.TotalValue);

            //Check partymember
            string getPartyMemberSql = string.Format("Select * from PartyMember Where UserId ={0} and PartyId ='{1}'", donationDTO.UserId, donationDTO.PartyId);
            List<PartyMember> newPartymember = spContext.GetSqlData<PartyMember>(getPartyMemberSql).ToList();

            Assert.AreEqual(newPartymember[0].DonationAmount, partyMemberInfo.DonationAmount);

            //Check Capitaltransaction
            string getCapitalTransactionLogSql = "Select * from CapitalTransactionLog";
            List<CapitalTransactionLog> capitalTransactionLog = spContext.GetSqlData<CapitalTransactionLog>(getCapitalTransactionLogSql).ToList();
            Assert.AreEqual(capitalTransactionLog.Count(), 0);

            ///Check Post
            ///
            string getPostSql = "Select * from Post";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 0);


        }

        #endregion PartyDonation NoCash

        #region PartyDonation Diffrent PartyMember
        [Category("PartyDonation")]
        [Test]
        [TestCase("7b80127f-8b05-41cc-bd54-9c26baacea2c", 33, "you not being current or past Party member")] //DiffrentPartyMember
        [TestCase("f1aa3425-3d76-4248-84d2-5c7402e2a5a2", 9837, "party not being in active status")] //party in Pending Status
        public void PartyDonationFailed(string partyId, int userId, string expectedmsg)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();

            IUserBankAccountDTORepository bankrepo = new UserBankAccountDTORepository();

            PartyMember partyMemberInfo = partyRepo.GetActiveUserParty(userId);
            PoliticalParty partyinfo = partyRepo.GetPartyById(partyId);
            WebUserDTO webData = webRepo.GetUserPicFName(userId);
            UserBankAccount bacnkAccount = bankrepo.GetUserBankDetails(userId);

            DonatePartyDTO donationDTO = new DonatePartyDTO
            {
                PartyId = partyId,
                UserId = userId,
                Amount = 1000
            };
            SendDonationAmount(donationDTO);
            CheckSendDonationAmountFailed(donationDTO, partyinfo, bacnkAccount, partyMemberInfo, expectedmsg);



            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than five seconds");
        }
        private void CheckSendDonationAmountFailed(DonatePartyDTO donationDTO, PoliticalParty partyInfo, UserBankAccount bankac, PartyMember partyMemberInfo, string expectedmsg)
        {
            string getUserNotificationSql = "Select * from UserNotification";

            string parmDonationFailNotification = donationDTO.Amount + "|" + partyInfo.PartyId.ToString() + "|" + partyInfo.PartyName + "|" + expectedmsg;

            List<UserNotification> userNotification = spContext.GetSqlData<UserNotification>(getUserNotificationSql).ToList();
            //1 Task Must be created
            Assert.AreEqual(userNotification.Count(), 1);
            Assert.AreEqual(userNotification.Count(f => f.NotificationTypeId == AppSettings.PartyDonateFailNotificationId), 1);
            Assert.AreEqual(userNotification[0].Parms.Count(f => f == '|'), 3);


            Assert.IsTrue(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyDonateFailNotificationId).Parms.Contains(parmDonationFailNotification));
            Assert.IsFalse(userNotification.Find(f => f.NotificationTypeId == AppSettings.PartyDonateFailNotificationId).Parms.Contains("||"));

            //Check userb bankAccount
            string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId ={0}", donationDTO.UserId);
            List<UserBankAccount> newBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();

            Assert.AreEqual(newBankAccount[0].Cash, bankac.Cash);

            //Check userb partyAccount

            string getPoliticalpartySql = string.Format("Select * from PoliticalParty Where PartyId ='{0}'", donationDTO.PartyId);
            List<PoliticalParty> newpartyInfo = spContext.GetSqlData<PoliticalParty>(getPoliticalpartySql).ToList();

            Assert.AreEqual(newpartyInfo[0].TotalValue, partyInfo.TotalValue);



            //Check Capitaltransaction
            string getCapitalTransactionLogSql = "Select * from CapitalTransactionLog";
            List<CapitalTransactionLog> capitalTransactionLog = spContext.GetSqlData<CapitalTransactionLog>(getCapitalTransactionLogSql).ToList();
            Assert.AreEqual(capitalTransactionLog.Count(), 0);

            ///Check Post
            ///
            string getPostSql = "Select * from Post";
            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 0);


        }

        #endregion PartyDonation Diffrent PartyMember

        #endregion PartyDonation

        #region Party Invite

        #region Party Invite both email and Friends

        [Category("PartyInvite")]
        [Test]
        public void SendPartyInvites()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            int userId = 1;
            string partyId = "7b80127f-8b05-41cc-bd54-9c26baacea2c";




            InviteeDTO[] invitees = new InviteeDTO[] {
            new InviteeDTO{FriendId =1920},
            new InviteeDTO{FriendId =5855},
            new InviteeDTO{FriendId =3518},
            new InviteeDTO{FriendId =8723},
            new InviteeDTO{FriendId =5},
            new InviteeDTO{EmailId ="abanks1u9r@yahoo.co.jp"},
            new InviteeDTO{EmailId ="aelliott1dmm@dailymotion.com"},
            new InviteeDTO{EmailId ="awalkersyn@desdev.cn"},
            new InviteeDTO{EmailId ="dmontgomery1x9h@archive.org"},
            new InviteeDTO{EmailId ="abrown@zoonoodle.com"}, //existing email
            new InviteeDTO{EmailId ="agriffin@voonyx.info"}, //existing email id
            new InviteeDTO{EmailId ="mwelch@aimbu.org"}, //is existing email id with same party
            new InviteeDTO{EmailId ="fmoore@dynava.net"}, //is existing email id with same party
            };


            PartyInviteDTO partyInviteDto = new PartyInviteDTO
            {
                PartyId = partyId,
                UserId = userId,
                PartyInvites = invitees
            };

            PoliticalParty oldpartyInfo = partyRepo.GetPartyById(partyId);

            string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId in ({0},{1},{2},{3})", invitees[0].FriendId, invitees[1].FriendId, invitees[2].FriendId, invitees[3].FriendId);

            List<UserBankAccount> oldBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();

            SendPartyInvites(partyInviteDto);
            CheckSendPartyInvites(partyInviteDto);
            VotePartyInvite();
            CheckPartyInviteVote(partyInviteDto, oldBankAccount, oldpartyInfo);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 60, string.Format("Should take less than 25 seconds but took {0}", elapsedSeconds));
        }

        private void SendPartyInvites(
            PartyInviteDTO partyInviteDto)
        {
            PoliticalPartyManager partyManager = new PoliticalPartyManager();
            partyManager.ProcessPartyInvite(partyInviteDto);
        }
        private void CheckSendPartyInvites(
            PartyInviteDTO partyInviteDto)
        {
            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}' and DefaultResponse ='{2}'", partyInviteDto.UserId, "I", 10);
            string parmTextTask = @"1|Steve Johnston|Member|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|7717.33|";
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 6);
            Assert.AreEqual(userTask.Count(f => f.Parms.Contains("||")), 0);
            Assert.AreEqual(userTask.Count(f => f.Parms.Contains(parmTextTask)), 6);
            Assert.AreEqual(userTask.Find(f => f.TaskTypeId == AppSettings.JoinPartyTaskType).Parms.Count(f => f == '|'), 7);

            ///Check PartyInvite

            string getPartyInvitesql = "Select * from PartyInvite";

            List<PartyInvite> partyInviteInfo = spContext.GetSqlData<PartyInvite>(getPartyInvitesql).ToList();
            Assert.AreEqual(partyInviteInfo.Count(), 10);
            Assert.AreEqual(partyInviteInfo.Count(f => f.UserId == 0), 4);
            Assert.AreEqual(partyInviteInfo.Count(f => String.IsNullOrEmpty(f.EmailId)), 5);
            Assert.AreEqual(partyInviteInfo.Count(f => f.Status == "P"), 10);
            Assert.AreEqual(partyInviteInfo.Count(f => f.MemberType == "M"), 10);
            Assert.AreEqual(partyInviteInfo.Count(f => f.PartyId.ToString() == partyInviteDto.PartyId), 10);


            ///Check UserNotification

            string getUserNotificationsql = "Select * from UserNotification Order by Parms";

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 19);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == partyInviteDto.UserId), partyInviteDto.PartyInvites.Length);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteNotificationId), 6);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifySuccessNotificationId), 6);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifyFailNotificationId), 3);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifyEmailFreindsSuccessNotificationId), 4);

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);



            string notifyParmText = "7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|onclick: GoToTask()";
            Assert.IsTrue(userNotifications.Find(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteNotificationId).Parms.Contains(notifyParmText));


            string[] parmtextPartyInvitationNotifySuccessNotificationId = new string[] { 
            "1920|Ruby Wagner|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
"3518|Stephen Carr|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
"5855|Henry Robinson|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
"5|Anna Brown|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
"6|Anne Griffin|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
"8723|David Bailey|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
            };

            List<UserNotification> notifcationList = userNotifications.
         FindAll(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifySuccessNotificationId);

            for (int i = 0; i < parmtextPartyInvitationNotifySuccessNotificationId.Length; i++)
            {
                Assert.AreEqual(notifcationList.Count(f => f.Parms.Contains(parmtextPartyInvitationNotifySuccessNotificationId[i])), 1);
            }



            string[] parmtextPartyInvitationNotifyFailNotificationId = new string[] { 
            "4237|Fred Moore|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint| invitee being current party member",
"9930|Mildred Welch|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint| invitee being current party member"
            };
            notifcationList.Clear();
            notifcationList = userNotifications.
                    FindAll(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifyFailNotificationId);

            for (int i = 0; i < parmtextPartyInvitationNotifyFailNotificationId.Length; i++)
            {
                Assert.AreEqual(notifcationList.Count(f => f.Parms
                    .Contains(parmtextPartyInvitationNotifyFailNotificationId[i])), 1);
            }


            string[] parmtextPartyInvitationNotifyEmailFreindsSuccessNotificationId = new string[]{"abanks1u9r@yahoo.co.jp|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
                "aelliott1dmm@dailymotion.com|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
                "awalkersyn@desdev.cn|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
                "dmontgomery1x9h@archive.org|7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint"
            };
            notifcationList.Clear();
            notifcationList = userNotifications.
                    FindAll(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifyEmailFreindsSuccessNotificationId);

            for (int i = 0; i < parmtextPartyInvitationNotifyEmailFreindsSuccessNotificationId.Length; i++)
            {
                Assert.AreEqual(notifcationList.Count(f => f.Parms
                    .Contains(parmtextPartyInvitationNotifyEmailFreindsSuccessNotificationId[i])), 1);
            }
        }
        private void VotePartyInvite()
        {

            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask order by UserId");
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            for (int i = 0; i < userTask.Count; i++)
            {
                if (i % 2 == 0)
                {
                    AcceptPartyInvites(userTask[i].UserId, userTask[i].TaskId);

                }
                else
                {
                    DeclinePartyInvites(userTask[i].UserId, userTask[i].TaskId);
                }
            }

        }

        private void AcceptPartyInvites(int userId, Guid taskId)
        {
            VoteResponseDTO userVote = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyInviteAcceptChoiceId },
                ChoiceRadioId = AppSettings.PartyInviteAcceptChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.JoinPartyTaskType
            };
            UserVoteManager votemanager = new UserVoteManager();
            votemanager.ProcessVotingResponse(userVote, userId);
        }
        private void DeclinePartyInvites(int userId, Guid taskId)
        {
            VoteResponseDTO userVote = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyInviteRejectChoiceId },
                ChoiceRadioId = AppSettings.PartyInviteRejectChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.JoinPartyTaskType
            };
            UserVoteManager votemanager = new UserVoteManager();
            votemanager.ProcessVotingResponse(userVote, userId);
        }
        private void CheckPartyInviteVote(PartyInviteDTO partyInviteDto, List<UserBankAccount> oldBankAccount, PoliticalParty oldpartyInfo)
        {
            //Check partyInvite
            string getPartyInviteSql = string.Format("Select * from PartyInvite Where PartyId= '{0}'", partyInviteDto.PartyId);
            List<PartyInvite> partyInvite = spContext.GetSqlData<PartyInvite>(getPartyInviteSql).ToList();
            Assert.AreEqual(partyInvite.Count(f => f.Status == "A" && (f.UserId == partyInviteDto.PartyInvites[0].FriendId || f.UserId == partyInviteDto.PartyInvites[1].FriendId)), 2);
            Assert.AreEqual(partyInvite.Count(f => f.Status == "D" && (f.UserId == 6 || f.UserId == partyInviteDto.PartyInvites[2].FriendId || f.UserId == partyInviteDto.PartyInvites[3].FriendId)), 3);
            Assert.AreEqual(partyInvite.Count(f => f.Status == "N" && (f.UserId == 5)), 1);
            Assert.AreEqual(partyInvite.Count(f => f.Status == "P"), 4);
            Assert.AreEqual(partyInvite.Count(), 10);


            //Check partymember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where PartyId= '{0}' and MemberEndDate is null", partyInviteDto.PartyId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Count(f => string.IsNullOrEmpty(f.MemberStatus) && (f.UserId == partyInviteDto.PartyInvites[1].FriendId || f.UserId == partyInviteDto.PartyInvites[0].FriendId) && f.MemberType == "M"), 2);


            //Check PoliticalParty
            string gertPoliticalPartySql = string.Format("Select * from PoliticalParty Where PartyId= '{0}'", partyInviteDto.PartyId);
            List<PoliticalParty> politicalParty = spContext.GetSqlData<PoliticalParty>(gertPoliticalPartySql).ToList();
            Assert.AreEqual(politicalParty.Count(f => f.PartySize == 12), 1);
            Assert.AreEqual(politicalParty[0].TotalValue, oldpartyInfo.TotalValue + (2 * oldpartyInfo.MembershipFee));


            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}'", partyInviteDto.UserId, "C");

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(), 6);

            //Check userBankAccount
            string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId in ({0},{1},{2},{3})", partyInviteDto.PartyInvites[0].FriendId, partyInviteDto.PartyInvites[1].FriendId, partyInviteDto.PartyInvites[2].FriendId, partyInviteDto.PartyInvites[3].FriendId);

            List<UserBankAccount> newBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();

            Assert.AreEqual(newBankAccount.Find(f => f.UserId == partyInviteDto.PartyInvites[0].FriendId).Cash,
                oldBankAccount.Find(f => f.UserId == partyInviteDto.PartyInvites[0].FriendId).Cash - oldpartyInfo.MembershipFee);

            Assert.AreEqual(newBankAccount.Find(f => f.UserId == partyInviteDto.PartyInvites[1].FriendId).Cash,
                oldBankAccount.Find(f => f.UserId == partyInviteDto.PartyInvites[1].FriendId).Cash - oldpartyInfo.MembershipFee);

            //Check UserNotification

            string getUserNotificationsql = "Select * from UserNotification Order by Parms";

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            string[] parmtextNotifications = new string[] { 
          "7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint",
"7b80127f-8b05-41cc-bd54-9c26baacea2c|Flashpoint|Not Enough Cash for membership fee."
            };

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyWelcomeNotificationId && f.Parms.Contains(parmtextNotifications[0])), 2);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyNotWelcomeNotificationId && f.Parms.Contains(parmtextNotifications[1])), 1);

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);

        }

        #endregion Party Invite both email and Friends
        #endregion Party Invite

        #region Start Party
        #region Start Party Approved
        [Category("StartaNewParty")]
        [Test]
        [TestCase(1, "Pleicans", "Motto", new int[] { 991, 992, 993, 994, 995, 996, 997 }, new short[] { 1, 2 }, "Fail")]
        [TestCase(3, "JABBERSTORM", "Motto", new int[] { 991, 992, 993, 994, 995, 996, 997 }, new short[] { 1, 2 }, "Fail")]
        [TestCase(5, "NotEnoughInvite", "Motto", new int[] { 991, 992, 993, 994, 995, 996 }, new short[] { 1, 2 }, "Fail")]
        [TestCase(3, "Pleicans", "Motto", new int[] { 991, 992, 993, 994, 995, 996, 997, 98304, 88311, 83072, 71713, 63053, 1 }, new short[] { 1, 2 }, "Pass")]
        public void StartaNewParty(int userId, string partyName, string motto, int[] friendList, short[] agendaType, string expectedResult)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            StartPartyDTO startparty = new StartPartyDTO
            {
                PartyName = partyName,
                LogoPictureId = "00e502c5-81df-4296-b1f5-9c24190bf004",
                MembershipFee = 3000,
                Motto = motto,
                ContactInvitationList = new String[] { "aalexanderhj4@gnu.org", "ajacobs9vw@google.de", "aalexanderenb@soup.io" },
                FriendInvitationList = friendList,
                InitatorId = userId,
                AgendaType = agendaType

            };


            StartpartyRequest(startparty);
            if (expectedResult == "Fail")
            {
                CheckStartPartyFailMsg(startparty);
            }
            else if (expectedResult == "Pass")
            {
                PoliticalParty oldpartyInfo = CheckStartPartySuccessMsg(startparty);

                string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId in ({0})", string.Join(",", startparty.FriendInvitationList));

                List<UserBankAccount> oldBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();
                VoteStartPartyInvite();
                CheckStartPartyInviteVote(startparty, oldBankAccount, oldpartyInfo);
            }

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 60, "Should take less than  60 seconds");
        }
        private void StartpartyRequest(
       StartPartyDTO startPartyDTO)
        {
            PoliticalPartyManager partyManager = new PoliticalPartyManager();
            partyManager.ProcessStartParty(startPartyDTO);
        }
        private void CheckStartPartyFailMsg(StartPartyDTO startPartyDTO)
        {

            ///Check UserNotification

            string getUserNotificationsql = string.Format("Select * from UserNotification Where UserId ={0}", startPartyDTO.InitatorId);

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            string parmText = string.Empty;

            if (startPartyDTO.InitatorId == 1)
            {
                parmText = "|you being member of a diffrent party. You need to leave the current party membership before creating a new Party";
            }
            else if (startPartyDTO.InitatorId == 3)
            {
                parmText = "|Party Name was already taken";
            }
            else if (startPartyDTO.InitatorId == 5)
            {
                parmText = "or more CoFounders when starting a new Party";
            }

            Assert.AreEqual(userNotifications.Count(), 1);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyOpenFailNotificationId), 1);

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);
            Assert.AreEqual(1, userNotifications.Count(f => f.Parms.Contains(parmText)));
        }
        private PoliticalParty CheckStartPartySuccessMsg(StartPartyDTO startPartyDTO)
        {


            //Check PoliticalParty
            string gertPoliticalPartySql = string.Format("Select * from PoliticalParty Where PartyFounder= {0} And Status ='P' And PartyName ='{1}'", startPartyDTO.InitatorId, startPartyDTO.PartyName);
            List<PoliticalParty> politicalParty = spContext.GetSqlData<PoliticalParty>(gertPoliticalPartySql).ToList();

            Assert.AreEqual(1, politicalParty.Count());
            Assert.AreEqual(1, politicalParty.Count(f => f.TotalValue == 0));
            Assert.AreEqual(1, politicalParty.Count(f => f.PartySize == 1));
            Assert.AreEqual(1, politicalParty.Count(f => f.MembershipFee == 3000));
            Assert.AreEqual(1, politicalParty.Count(f => f.CoFounderSize == 0));

            ///Check UserNotification

            string getUserNotificationsql = "Select * from UserNotification";

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            string parmText = string.Empty;



            Assert.AreEqual(userNotifications.Count(), 30);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == startPartyDTO.InitatorId
                && f.NotificationTypeId == AppSettings.PartyInvitationNotifySuccessNotificationId), 13);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == startPartyDTO.InitatorId
        && f.NotificationTypeId == AppSettings.PartyInvitationNotifyEmailFreindsSuccessNotificationId), 3);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == startPartyDTO.InitatorId
 && f.NotificationTypeId == AppSettings.PartyOpenSuccessNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyJoinRequestInviteNotificationId), 13);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);

            parmText = politicalParty[0].PartyId + "|Pleicans";
            Assert.AreEqual(1, userNotifications.Count(f => f.Parms.Contains(parmText) && f.NotificationTypeId == AppSettings.PartyOpenSuccessNotificationId));
            parmText = "" + politicalParty[0].PartyId + "|Pleicans|onclick: GoToTask()";
            Assert.AreEqual(13, userNotifications.Count(f => f.Parms.Contains(parmText)));

            string[] parmtextPartyInvitationNotifySuccessNotificationId = new string[]{
"995|Sean Stewart|"+politicalParty[0].PartyId+"|Pleicans",
"996|Jeffrey Perez|"+politicalParty[0].PartyId+"|Pleicans",
"997|Rose Castillo|"+politicalParty[0].PartyId+"|Pleicans",
"991|Patrick Hughes|"+politicalParty[0].PartyId+"|Pleicans",
"63053|Kathryn Perkins|"+politicalParty[0].PartyId+"|Pleicans",
"88311|Benjamin Scott|"+politicalParty[0].PartyId+"|Pleicans",
"992|Jesse Watson|"+politicalParty[0].PartyId+"|Pleicans",
"1|Steve Johnston|"+politicalParty[0].PartyId+"|Pleicans",
"83072|Jose Patterson|"+politicalParty[0].PartyId+"|Pleicans",
"994|Paul Lewis|"+politicalParty[0].PartyId+"|Pleicans",
"98304|Harry Wells|"+politicalParty[0].PartyId+"|Pleicans",
"71713|Gregory Duncan|"+politicalParty[0].PartyId+"|Pleicans",
"993|Gerald Gardner|"+politicalParty[0].PartyId+"|Pleicans"
            };

            List<UserNotification> notifcationList = userNotifications.
           FindAll(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifySuccessNotificationId);
            notifcationList = userNotifications.
                    FindAll(f => f.NotificationTypeId == AppSettings.PartyInvitationNotifySuccessNotificationId);

            for (int i = 0; i < parmtextPartyInvitationNotifySuccessNotificationId.Length; i++)
            {
                Assert.AreEqual(notifcationList.Count(f => f.Parms
                    .Contains(parmtextPartyInvitationNotifySuccessNotificationId[i])), 1);
            }

            CheckCachedPartyNames(startPartyDTO.CountryId);

            return politicalParty[0];
        }

        private void VoteStartPartyInvite()
        {

            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask order by UserId");
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            for (int i = 0; i < userTask.Count; i++)
            {
                if (i % 6 == 0)
                {
                    DeclinePartyInvites(userTask[i].UserId, userTask[i].TaskId);

                }
                else
                {
                    AcceptPartyInvites(userTask[i].UserId, userTask[i].TaskId);
                }
            }

        }
        private void CheckStartPartyInviteVote(StartPartyDTO startparty, List<UserBankAccount> oldBankAccount, PoliticalParty oldpartyInfo)
        {
            //Check partyInvite
            string getFriendInvitationListql = string.Format("Select * from PartyInvite Where PartyId= '{0}'", oldpartyInfo.PartyId);
            List<PartyInvite> partyInvite = spContext.GetSqlData<PartyInvite>(getFriendInvitationListql).ToList();

            Assert.AreEqual(partyInvite.Count(f => f.Status == "A"), 10);

            Assert.AreEqual(partyInvite.Count(f => f.Status == "D"), 3);

            Assert.AreEqual(partyInvite.Count(f => f.Status == "P"), 3);

            Assert.AreEqual(partyInvite.Count(), 16);


            //Check partymember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where PartyId= '{0}' and MemberEndDate is null", oldpartyInfo.PartyId);

            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();

            Assert.AreEqual(partyMember.Count(f => string.IsNullOrEmpty(f.MemberStatus) && (f.UserId == startparty.FriendInvitationList[1] || f.UserId == startparty.FriendInvitationList[0]) && f.MemberType == "M"), 2);


            //Check PoliticalParty
            string gertPoliticalPartySql = string.Format("Select * from PoliticalParty Where PartyId= '{0}'", oldpartyInfo.PartyId);
            List<PoliticalParty> politicalParty = spContext.GetSqlData<PoliticalParty>(gertPoliticalPartySql).ToList();
            Assert.AreEqual(politicalParty.Count(f => f.PartySize == 11), 1);
            Assert.AreEqual(politicalParty.Count(f => f.Status == "A"), 1);
            Assert.AreEqual(politicalParty[0].TotalValue, oldpartyInfo.TotalValue + (10 * oldpartyInfo.MembershipFee));

            //Check party Agenda
            string gertpartyAgendaSql = string.Format("Select * from PartyAgenda Where PartyId= '{0}'", oldpartyInfo.PartyId);
            List<PartyAgenda> partyAgendas = spContext.GetSqlData<PartyAgenda>(gertpartyAgendaSql).ToList();
            Assert.AreEqual(2, partyAgendas.Count());
            Assert.AreEqual(1, partyAgendas.Count(f => f.AgendaTypeId == 1));
            Assert.AreEqual(1, partyAgendas.Count(f => f.AgendaTypeId == 2));



            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}'", startparty.InitatorId, "C");

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.JoinPartyTaskType), 13);

            //Check userBankAccount
            string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId in ({0})", string.Join(",", startparty.FriendInvitationList));

            List<UserBankAccount> newBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();

            foreach (UserBankAccount item in newBankAccount)
            {
                if (partyInvite.Count(f => f.Status == "A" && f.UserId == item.UserId) == 1)
                {
                    Assert.AreEqual(item.Cash, oldBankAccount.Find(f => f.UserId == item.UserId).Cash - oldpartyInfo.MembershipFee);
                }
                else
                {
                    Assert.AreEqual(item.Cash, oldBankAccount.Find(f => f.UserId == item.UserId).Cash);
                }
            }
        }
        #endregion Start Party Approved

        #endregion Start Party

        #region Quit Party

        [Category("QuitParty")]
        [Test]
        [TestCase(80, "C")] //Last Party Member Quit Party Closed
        [TestCase(9856, "P")] //Founder Quit Party on P
        [TestCase(1, "P")] //Founder Quit Party on P
        public void QuitParty(int userId, string expectedStatus)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            QuitPartyDTO quitParty = new QuitPartyDTO
            {
                UserId = userId
            };

            quitParty.PartyId = partyRepo.GetActivePartyId(quitParty.UserId);
            PoliticalParty partyInfo = partyRepo.GetPartyById(quitParty.PartyId);

            QuitPartyRequest(quitParty);
            CheckQuitParty(userId, partyInfo, expectedStatus);
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 40, "Should take less than  40 seconds");
        }
        private void QuitPartyRequest(QuitPartyDTO quitParty)
        {
            PoliticalPartyManager partyManager = new PoliticalPartyManager();
            partyManager.ProcessQuitParty(quitParty);
        }

        [Category("QuitParty")]
        public void CheckQuitParty(int userId, PoliticalParty partyInfo, string expectedStatus)
        {
            //Check PoliticalParty
            string gertPoliticalPartySql = string.Format("Select * from PoliticalParty Where PartyId= '{0}'", partyInfo.PartyId);
            List<PoliticalParty> politicalParty = spContext.GetSqlData<PoliticalParty>(gertPoliticalPartySql).ToList();
            Assert.AreEqual(politicalParty.Count(f => f.PartySize == (partyInfo.PartySize - 1)), 1);
            Assert.AreEqual(politicalParty.Count(f => f.Status == expectedStatus), 1);


            ///Check PartyMember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where UserId ={0}", userId);

            List<PartyMember> partyMembers = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();
            Assert.IsTrue(partyMembers.Find(f => f.MemberStatus == "Q" && f.UserId == userId).MemberEndDate > DateTime.MinValue);

            ///Check UserNotification

            string getUserNotificationsql = "Select * from UserNotification Order by Parms";

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 1);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyLeaveRequestSuccessNotificationId), 1);

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);

            ///Check Post
            ///
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            string getPostSql = "Select * from Post";

            WebUserDTO webUserInfo = webRepo.GetUserPicFName(userId); string postContentResult = string.Format("{0}|{1}|{2}|{3}|{4}",
                 userId,
                webUserInfo.Picture,
                webUserInfo.FullName,
                partyInfo.PartyId, partyInfo.PartyName
                );


            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 1);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyDumpedPostContentTypeId).Parms.Contains(postContentResult));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyDumpedPostContentTypeId).Parms.Contains("||"));
        }


        #endregion Quit Party

        #region Manage Party
        [Category("ManageParty")]
        [Test]
        [TestCase(9930, "FlashPoint Mob", "ba98548c-8a2c-4501-80aa-ba8701b5dedb", 10, "new Motoo", 89, new short[] { 4, 6, 8 }, "", "Pass")]
        [TestCase(9930, "MyRoom", "ba98548c-8a2c-4501-80aa-ba8701b5dedb", 10, "new Motoo", 89, new short[] { 4, 6, 8 }, "", "Pass")]
        [TestCase(9930, "Flashpoint", "ba98548c-8a2c-4501-80aa-ba8701b5dedb", 10, "new Motoo", 89, new short[] { 4, 6, 8 }, "", "Pass")] //sanme partyName
        [TestCase(1, "MyRoom", "ba98548c-8a2c-4501-80aa-ba8701b5dedb", 10, "new Motoo", 89, new short[] { 1, 2, 3, 4, 6, 7 }, "", "Fail")] // Not Founder
        [TestCase(1, "M", "", 10, "n", 90, new short[] { 1 }, "Party name not being at least 5 character", "Fail")] // Party Name not long
        [TestCase(1, "Monkey", "", 10, "n", 90, new short[] { }, "Must select at least 1 agenda", "Fail")] // No Agenda
        public void ManageParty(int userId, string partyName, string logoId, decimal fee, string motto, short notificationId, short[] agendaType, string parmmsg, string expectedresult)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            StartPartyDTO manageparty = new StartPartyDTO
            {
                InitatorId = userId,
                PartyName = partyName,
                LogoPictureId = logoId,
                MembershipFee = fee,
                Motto = motto,
                AgendaType = agendaType
            };

            string partyId = partyRepo.GetActivePartyId(manageparty.InitatorId);
            PoliticalParty partyInfo = partyRepo.GetPartyById(partyId);
            ManagePartyRequest(manageparty);
            CheckManageParty(manageparty, partyInfo, notificationId, agendaType, parmmsg, expectedresult);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 10, "Should take less than  10 seconds");
        }
        private void ManagePartyRequest(StartPartyDTO manageparty)
        {
            PoliticalPartyManager partyManager = new PoliticalPartyManager();
            partyManager.ProcessManageParty(manageparty);
        }
        private void CheckManageParty(StartPartyDTO manageParty, PoliticalParty oldpartyInfo, short notificationId, short[] agendas, string parmmsg, string expectedResult)
        {
            //Check PoliticalParty
            string gertPoliticalPartySql = string.Format("Select * from PoliticalParty Where PartyId= '{0}'", oldpartyInfo.PartyId);
            List<PoliticalParty> politicalParty = spContext.GetSqlData<PoliticalParty>(gertPoliticalPartySql).ToList();
            if (expectedResult == "Pass")
            {
                Assert.AreEqual(politicalParty.Count(f => f.PartyName == manageParty.PartyName
                                && f.MembershipFee == manageParty.MembershipFee
                                && f.Motto == manageParty.Motto
                                ), 1);
            }
            else if (expectedResult == "Fail")
            {
                Assert.AreEqual(politicalParty.Count(f => f.PartyName != manageParty.PartyName
                                               && f.MembershipFee != manageParty.MembershipFee
                                               && f.LogoPictureId != manageParty.LogoPictureId), 1);
            }

            //Check party Agenda
            if (agendas.Length > 0)
            {
                string gertpartyAgendaSql = string.Format("Select * from PartyAgenda Where PartyId= '{0}'", oldpartyInfo.PartyId);
                List<PartyAgenda> partyAgendas = spContext.GetSqlData<PartyAgenda>(gertpartyAgendaSql).ToList();


                Assert.AreEqual(agendas.Length, partyAgendas.Count());
                foreach (var item in agendas)
                {
                    Assert.AreEqual(1, partyAgendas.Count(f => f.AgendaTypeId == item));
                }
            }

            ///Check UserNotification

            string getUserNotificationsql = "Select * from UserNotification Order by Parms";

            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();


            Assert.AreEqual(userNotifications.Count(), 1);
            Assert.AreEqual(userNotifications.Count(f => f.UserId == manageParty.InitatorId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == notificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(parmmsg)), 1);

            if (expectedResult == "Pass")
            {
                CheckCachedPartyNames(oldpartyInfo.CountryId);
            }
        }
        private void CheckCachedPartyNames(string countryId)
        {
            //Check PartyName Cache
            string gertPoliticalPartySql = string.Format("Select * from PoliticalParty Where CountryId= '{0}'", countryId);
            List<PoliticalParty> politicalParty = spContext.GetSqlData<PoliticalParty>(gertPoliticalPartySql).ToList();

            string[] cahcedpartyList = redisCache.GetAllSet(AppSettings.RedisSetKeyPartyNames + countryId);
            if (cahcedpartyList.Length > 0)
            {
                Assert.AreEqual(politicalParty.Count(), cahcedpartyList.Length);

            }
            foreach (var item in cahcedpartyList)
            {
                Assert.AreEqual(politicalParty.Count(f => f.PartyName.ToUpper() == item), 1);
            }
        }

        #endregion Manage Party

        #region Close Party
        [Category("ManageParty")]
        [Test]
        [TestCase(1, "7b80127f-8b05-41cc-bd54-9c26baacea2c", "only party founder can request a closure", "", false, "Fail")] //Not Founder
        [TestCase(9856, "b813ef5d-1f80-47f9-912f-05a68ca176d5", "party already has a requset for closure", "H", false, "Fail")] // Status H
        [TestCase(9856, "b813ef5d-1f80-47f9-912f-05a68ca176d5", "party is already closed", "C", false, "Fail")]//Status C
        [TestCase(9930, "7b80127f-8b05-41cc-bd54-9c26baacea2c", "", "", true, "Pass")]
        [TestCase(9930, "7b80127f-8b05-41cc-bd54-9c26baacea2c", "", "", false, "Pass")]
        public void CloseParty(int userId, string partyId, string parmText, string closeStatus, bool approve, string expectedresult)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            ClosePartyDTO closeParty = new ClosePartyDTO
            {
                InitatorId = userId,
            };
            closeParty.PartyId = partyId;
            if (closeStatus != string.Empty)
            {
                AddPartyCloseRequest(closeParty, closeStatus);
            }
            ClosePartyRequest(closeParty);
            PoliticalParty partyInfo = partyRepo.GetPartyById(partyId);
            closeParty.InitatorFullName = webRepo.GetFullName(closeParty.InitatorId);
            if (expectedresult == "Fail")
            {
                CheckClosePartyRequestFail(closeParty, parmText, closeStatus);
            }
            else
            {
                CheckClosePartyRequestSuccess(closeParty, parmText, closeStatus);
                string[] allmembers = partyRepo.GetAllPartyMember(closeParty.PartyId);
                List<UserBankAccount> oldbankac =
                    GetUserBankAccounts(allmembers);
                List<PartyMemberDTO> coFounders =
                   JsonConvert.DeserializeObject<List<PartyMemberDTO>>(partyRepo.GetPartyCoFounders(partyInfo.PartyId.ToString()));
                VotePartyClose(approve);
                if (approve)
                {
                    CheckClosePartyRequestApprove(closeParty, parmText, closeStatus);
                    CheckBankAccountafterpartyClose(oldbankac, allmembers, partyInfo, coFounders);
                }
                else
                {
                    CheckClosePartyRequestDenied(closeParty, parmText, closeStatus);
                }
            }

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 30, "Should take less than  30 seconds");
        }
        private List<UserBankAccount> GetUserBankAccounts(string[] allmembers)
        {
            //Check userb bankAccount
            string getUserBankAccountSql = string.Format("Select * from UserBankAccount Where UserId in ({0})", String.Join(",", allmembers));
            List<UserBankAccount> oldBankAccount = spContext.GetSqlData<UserBankAccount>(getUserBankAccountSql).ToList();
            return oldBankAccount;
        }
        private void AddPartyCloseRequest(ClosePartyDTO closeParty, string closeStatus)
        {
            PartyCloseRequest closeRequest = new PartyCloseRequest
            {
                PartyId = new Guid(closeParty.PartyId),
                RequestDate = DateTime.UtcNow,
                Status = "P",
                TaskId = Guid.NewGuid(),
                UserId = closeParty.InitatorId
            };
            spContext.Add(closeRequest);
            partyRepo.UpdatePartyStatus(closeParty.PartyId, closeStatus);
        }
        private void ClosePartyRequest(ClosePartyDTO closeParty)
        {
            PoliticalPartyManager partyManager = new PoliticalPartyManager();
            partyManager.ProcessRequestCloseParty(closeParty);
        }

        private void CheckClosePartyRequestSuccess(ClosePartyDTO closeParty, string parmText, string closeStatus)
        {
            PoliticalParty partyInfo = partyRepo.GetPartyById(closeParty.PartyId);

            ///Check UserNotification
            string getUserNotificationsql = "Select * from UserNotification Order by Parms";
            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            Assert.AreEqual(userNotifications.Count(), partyInfo.PartySize + 1);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseVotingRequestNotificationId), partyInfo.PartySize);

            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseRequestSuccessNotificationId), 1);

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);

            string notifparmtext = string.Format("{0}|{1}",
                closeParty.PartyId,
                closeParty.PartyName
                );
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(notifparmtext) && f.NotificationTypeId == AppSettings.PartyCloseRequestSuccessNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);
            notifparmtext = string.Format("{0}|{1}",

      closeParty.InitatorId,
      closeParty.InitatorFullName);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(notifparmtext) && f.NotificationTypeId == AppSettings.PartyCloseVotingRequestNotificationId), partyInfo.PartySize);



            ///Check ClosePartyRequest

            string getClosePartyRequestsql = string.Format("Select * from PartyCloseRequest Where PartyId='{0}'", closeParty.PartyId);
            List<PartyCloseRequest> userPartyCloseRequest = spContext.GetSqlData<PartyCloseRequest>(getClosePartyRequestsql).ToList();
            Assert.AreEqual(userPartyCloseRequest.Count(), 1);
            Assert.AreEqual(userPartyCloseRequest.Count(f => f.Status == "P"), 1);

            ///Check PoliticalParty

            string getPoliticalPartyRequestsql = string.Format("Select * from PoliticalParty Where PartyId='{0}'", closeParty.PartyId);
            List<PoliticalParty> userPartyPoliticalParty = spContext.GetSqlData<PoliticalParty>(getPoliticalPartyRequestsql).ToList();
            Assert.AreEqual(userPartyPoliticalParty.Count(), 1);
            Assert.AreEqual(userPartyPoliticalParty.Count(f => f.Status == "H"), 1);
            Assert.AreEqual(userPartyPoliticalParty.Count(f => f.EndDate == null), 1);

            ///Check Post
            string getPostSql = "Select * from Post";
            string picture = webRepo.GetUserPicture(closeParty.InitatorId);
            StringBuilder message = new StringBuilder();

            message.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                 closeParty.InitatorId,
                picture, closeParty.InitatorFullName,
                closeParty.PartyId, closeParty.PartyName
                );


            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 1);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyCloseRequestPostContentTypeId).Parms.Contains(message.ToString()));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyCloseRequestPostContentTypeId).Parms.Contains("||"));
        }
        private void CheckClosePartyRequestFail(ClosePartyDTO closeParty, string parmText, string closeStatus)
        {
            ///Check UserNotification
            string getUserNotificationsql = "Select * from UserNotification Order by Parms";
            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            Assert.AreEqual(userNotifications.Count(), 1);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseRequestFailNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);

            string notifparmtext = string.Format("{0}|{1}|{2}",
                closeParty.PartyId,
                closeParty.PartyName,
                parmText);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(notifparmtext)), 0);

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseRequestFailNotificationId), 1);

            ///Check ClosePartyRequest

            string getClosePartyRequestsql = string.Format("Select * from PartyCloseRequest Where PartyId='{0}'", closeParty.PartyId);
            List<PartyCloseRequest> userPartyCloseRequest = spContext.GetSqlData<PartyCloseRequest>(getClosePartyRequestsql).ToList();
            if (closeStatus != string.Empty)
            {
                Assert.AreEqual(userPartyCloseRequest.Count(f => f.Status == "P"), 1);

            }


            ///Check PoliticalParty

            string getPoliticalPartyRequestsql = string.Format("Select * from PoliticalParty Where PartyId='{0}'", closeParty.PartyId);
            List<PoliticalParty> userPartyPoliticalParty = spContext.GetSqlData<PoliticalParty>(getPoliticalPartyRequestsql).ToList();
            Assert.AreEqual(userPartyPoliticalParty.Count(), 1);
            if (closeStatus != string.Empty)
            {
                Assert.AreEqual(userPartyPoliticalParty.Count(f => f.Status == closeStatus), 1);
            }
            Assert.AreEqual(userPartyPoliticalParty.Count(f => f.EndDate == null), 1);

        }

        private void VotePartyClose(bool approve)
        {
            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask order by UserId desc");
            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            if (approve)
            {
                DeclinePartyClose(1294, userTask[0].TaskId);
                DeclinePartyClose(2475, userTask[0].TaskId);
                AcceptPartyClose(2847, userTask[0].TaskId);
                AcceptPartyClose(4878, userTask[0].TaskId);
                AcceptPartyClose(7347, userTask[0].TaskId);
                AcceptPartyClose(9256, userTask[0].TaskId);
                AcceptPartyClose(9288, userTask[0].TaskId);
                AcceptPartyClose(4237, userTask[0].TaskId);
                AcceptPartyClose(1, userTask[0].TaskId);
                AcceptPartyClose(9930, userTask[0].TaskId);
            }
            else
            {
                AcceptPartyClose(1294, userTask[0].TaskId);
                DeclinePartyClose(2475, userTask[0].TaskId);
                AcceptPartyClose(2847, userTask[0].TaskId);
                AcceptPartyClose(4878, userTask[0].TaskId);
                AcceptPartyClose(7347, userTask[0].TaskId);
                AcceptPartyClose(9256, userTask[0].TaskId);
                AcceptPartyClose(9288, userTask[0].TaskId);
                AcceptPartyClose(4237, userTask[0].TaskId);
                AcceptPartyClose(1, userTask[0].TaskId);
                DeclinePartyClose(9930, userTask[0].TaskId);
            }
        }
        private void AcceptPartyClose(int userId, Guid taskId)
        {
            VoteResponseDTO userVote = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyCloseApprovalChoiceId },
                ChoiceRadioId = AppSettings.PartyCloseApprovalChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.ClosePartyTaskType
            };
            UserVoteManager votemanager = new UserVoteManager();
            votemanager.ProcessVotingResponse(userVote, userId);
        }
        private void DeclinePartyClose(int userId, Guid taskId)
        {
            VoteResponseDTO userVote = new VoteResponseDTO
            {
                ChoiceIds = new int[1] { AppSettings.PartyCloseDenialChoiceId },
                ChoiceRadioId = AppSettings.PartyCloseDenialChoiceId,
                TaskId = taskId,
                TaskTypeId = AppSettings.ClosePartyTaskType
            };
            UserVoteManager votemanager = new UserVoteManager();
            votemanager.ProcessVotingResponse(userVote, userId);
        }

        private void CheckClosePartyRequestApprove(ClosePartyDTO closeParty, string parmText, string closeStatus)
        {
            PoliticalParty partyInfo = partyRepo.GetPartyById(closeParty.PartyId);
            Assert.AreEqual(partyInfo.Status, "C");

            ///Check UserNotification
            string getUserNotificationsql = "Select * from UserNotification Order by Parms";
            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            Assert.AreEqual(userNotifications.Count(), 21);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseVotingCountNotificationId), 9);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseResultNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);


            string notifparmtext = string.Format("{0}|{1}",
                closeParty.PartyId,
                closeParty.PartyName
                );
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(notifparmtext) && f.NotificationTypeId == AppSettings.PartyCloseVotingCountNotificationId), 9);

            notifparmtext = string.Format("{0}|{1}|{2}",
                  closeParty.PartyId,
                closeParty.PartyName,
                "Approved"
                );

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(notifparmtext) && f.NotificationTypeId == AppSettings.PartyCloseResultNotificationId), 1);



            ///Check ClosePartyRequest

            string getClosePartyRequestsql = string.Format("Select * from PartyCloseRequest Where PartyId='{0}'", closeParty.PartyId);
            List<PartyCloseRequest> userPartyCloseRequest = spContext.GetSqlData<PartyCloseRequest>(getClosePartyRequestsql).ToList();
            Assert.AreEqual(userPartyCloseRequest.Count(), 1);
            Assert.AreEqual(userPartyCloseRequest.Count(f => f.Status == "A"), 1);

            ///Check PoliticalParty

            string getPoliticalPartyRequestsql = string.Format("Select * from PoliticalParty Where PartyId='{0}'", closeParty.PartyId);
            List<PoliticalParty> userPartyPoliticalParty = spContext.GetSqlData<PoliticalParty>(getPoliticalPartyRequestsql).ToList();
            Assert.AreEqual(userPartyPoliticalParty.Count(), 1);
            Assert.AreEqual(userPartyPoliticalParty.Count(f => f.Status == "C"), 1);
            Assert.AreEqual(userPartyPoliticalParty.Count(f => f.EndDate > DateTime.MinValue), 1);

            ///Check Post
            string getPostSql = "Select * from Post";
            string picture = webRepo.GetUserPicture(closeParty.InitatorId);
            StringBuilder message = new StringBuilder();

            message.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                 closeParty.InitatorId,
                picture, closeParty.InitatorFullName,
                closeParty.PartyId, closeParty.PartyName,
                "Approved"
                );


            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 2);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyClosePostContentTypeId).Parms.Contains(message.ToString()));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyClosePostContentTypeId).Parms.Contains("||"));

            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}'", partyInfo.PartyFounder, "C");

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.ClosePartyTaskType), partyInfo.PartySize);

            ///Check PartyMember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where PartyId='{0}'", partyInfo.PartyId);
            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();
            Assert.AreEqual(partyMember.Count(f => f.MemberStatus == "C" && f.MemberEndDate > DateTime.MinValue), partyInfo.PartySize);

            ///Check UserVoteSelectedChoice

            string getUserVoteSelectedsql = string.Format("Select * from UserVoteSelectedChoice Where TaskId = '{0}'", userTask[0].TaskId);

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 10);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyCloseApprovalChoiceId), 8);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyCloseDenialChoiceId), 2);


            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 1).Score, AppSettings.CoFounderVoteScore);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 2475).Score, 1);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 9930).Score, AppSettings.FounderVoteScore);

        }
        private void CheckClosePartyRequestDenied(ClosePartyDTO closeParty, string parmText, string closeStatus)
        {
            PoliticalParty partyInfo = partyRepo.GetPartyById(closeParty.PartyId);
            Assert.AreEqual(partyInfo.Status, "A");

            ///Check UserNotification
            string getUserNotificationsql = "Select * from UserNotification Order by Parms";
            List<UserNotification> userNotifications = spContext.GetSqlData<UserNotification>(getUserNotificationsql).ToList();
            Assert.AreEqual(userNotifications.Count(), 21);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseVotingCountNotificationId), 9);
            Assert.AreEqual(userNotifications.Count(f => f.NotificationTypeId == AppSettings.PartyCloseResultNotificationId), 1);
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains("||")), 0);


            string notifparmtext = string.Format("{0}|{1}",
                closeParty.PartyId,
                closeParty.PartyName
                );
            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(notifparmtext) && f.NotificationTypeId == AppSettings.PartyCloseVotingCountNotificationId), 9);

            notifparmtext = string.Format("{0}|{1}|{2}",
                  closeParty.PartyId,
                closeParty.PartyName,
                "Denied"
                );

            Assert.AreEqual(userNotifications.Count(f => f.Parms.Contains(notifparmtext) && f.NotificationTypeId == AppSettings.PartyCloseResultNotificationId), 1);



            ///Check ClosePartyRequest

            string getClosePartyRequestsql = string.Format("Select * from PartyCloseRequest Where PartyId='{0}'", closeParty.PartyId);
            List<PartyCloseRequest> userPartyCloseRequest = spContext.GetSqlData<PartyCloseRequest>(getClosePartyRequestsql).ToList();
            Assert.AreEqual(userPartyCloseRequest.Count(), 1);
            Assert.AreEqual(userPartyCloseRequest.Count(f => f.Status == "D"), 1);

            ///Check PoliticalParty

            string getPoliticalPartyRequestsql = string.Format("Select * from PoliticalParty Where PartyId='{0}'", closeParty.PartyId);
            List<PoliticalParty> userPartyPoliticalParty = spContext.GetSqlData<PoliticalParty>(getPoliticalPartyRequestsql).ToList();
            Assert.AreEqual(userPartyPoliticalParty.Count(), 1);
            Assert.AreEqual(userPartyPoliticalParty.Count(f => f.Status == "A"), 1);
            Assert.AreEqual(userPartyPoliticalParty.Count(f => f.EndDate == null), 1);

            ///Check Post
            string getPostSql = "Select * from Post";
            string picture = webRepo.GetUserPicture(closeParty.InitatorId);
            StringBuilder message = new StringBuilder();

            message.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                 closeParty.InitatorId,
                picture, closeParty.InitatorFullName,
                closeParty.PartyId, closeParty.PartyName,
                "Denied"
                );


            List<Post> post = spContext.GetSqlData<Post>(getPostSql).ToList();
            Assert.AreEqual(post.Count(), 2);
            Assert.IsTrue(post.Find(f => f.PostContentTypeId == AppSettings.PartyClosePostContentTypeId).Parms.Contains(message.ToString()));
            Assert.IsFalse(post.Find(f => f.PostContentTypeId == AppSettings.PartyClosePostContentTypeId).Parms.Contains("||"));

            ///Check UserTask
            string gertUserTaskSql = string.Format("Select * from UserTask Where AssignerUserId={0} and Status ='{1}'", partyInfo.PartyFounder, "C");

            List<UserTask> userTask = spContext.GetSqlData<UserTask>(gertUserTaskSql).ToList();
            Assert.AreEqual(userTask.Count(f => f.TaskTypeId == AppSettings.ClosePartyTaskType), partyInfo.PartySize);

            ///Check PartyMember
            string gertPartyMemberSql = string.Format("Select * from PartyMember Where PartyId='{0}'", partyInfo.PartyId);
            List<PartyMember> partyMember = spContext.GetSqlData<PartyMember>(gertPartyMemberSql).ToList();
            Assert.AreEqual(partyMember.Count(f => string.IsNullOrEmpty(f.MemberStatus) && f.MemberEndDate == null), partyInfo.PartySize - 1);

            ///Check UserVoteSelectedChoice

            string getUserVoteSelectedsql = string.Format("Select * from UserVoteSelectedChoice Where TaskId = '{0}'", userTask[0].TaskId);

            List<UserVoteSelectedChoice> userVoteSelectedChoice = spContext.GetSqlData<UserVoteSelectedChoice>(getUserVoteSelectedsql).ToList();
            Assert.AreEqual(userVoteSelectedChoice.Count(), 10);

            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyCloseApprovalChoiceId), 8);
            Assert.AreEqual(userVoteSelectedChoice.Count(f => f.ChoiceId == AppSettings.PartyCloseDenialChoiceId), 2);


            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 1).Score, AppSettings.CoFounderVoteScore);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 2475).Score, 1);
            Assert.AreEqual(userVoteSelectedChoice.Find(f => f.UserId == 9930).Score, AppSettings.FounderVoteScore);

        }
        private void CheckBankAccountafterpartyClose(List<UserBankAccount> oldbankinfo, string[] allpartymemebrs, PoliticalParty partyInfo,
              List<PartyMemberDTO> coFounders
            )
        {
            List<UserBankAccount> newBankAccount = GetUserBankAccounts(allpartymemebrs);

            int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
            int totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                 partyFounderSize * AppSettings.FounderVoteScore
                ;

            decimal share = (partyInfo.TotalValue / totalVoters);
            decimal finalShare = 0;

            foreach (var item in newBankAccount)
            {
                finalShare = share;
                if (Convert.ToInt32(item.UserId) == partyInfo.PartyFounder)
                {
                    finalShare = share * AppSettings.FounderVoteScore;
                }
                else if (coFounders.Count(f => f.UserId == Convert.ToInt32(item.UserId)) == 1)
                {
                    finalShare = share * AppSettings.CoFounderVoteScore;
                }
                Assert.AreEqual(Math.Round(item.Cash - finalShare, 2), oldbankinfo.Find(f => f.UserId == item.UserId).Cash);
            }

        }

        #endregion Close Party

        #region Search Party

        [Category("SearchParty")]
        [Test]
        [TestCase(new string[] { "1" }, 100, 1, 0, 0, 1000, 301, 10, 6, "1/1/0001 12:00:00 AM", 1)]
        [TestCase(new string[] { "1" }, 100, 1, 0, 0, 1000, 301, 10, 6, "2013-04-06T16:17:25", 0)] //LastStartDate from first search above
        public void SearchParty(string[] agendaType, int partySizeRangeUp, int partySizeRangeDown
           , int partyVictoryRangeUp, int partyVictoryRangeDown, decimal partyFeeRangeUp,
            decimal partyFeeRangeDown, decimal partyWorthRangeUp, decimal partyWorthRangeDown, string lastStartDate, int expectedcount
            )
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            PartySearchDTO searchCriteria = new PartySearchDTO
            {
                AgendaType = agendaType,
                PartySizeRangeUp = partySizeRangeUp,
                PartySizeRangeDown = partySizeRangeDown,
                PartyVictoryRangeUp = partyVictoryRangeUp,
                PartyVictoryRangeDown = partyVictoryRangeDown,
                PartyFeeRangeUp = partyFeeRangeUp,
                PartyFeeRangeDown = partyFeeRangeDown,
                PartyWorthRangeUp = partyWorthRangeUp,
                PartyWorthRangeDown = partyWorthRangeDown,
                LastStartDate = Convert.ToDateTime(lastStartDate)
            };

            string partyResult =
            partyRepo.SearchParty(searchCriteria);
            CheckPartySearchResult(partyResult, expectedcount);

            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 40, "Should take less than  40 seconds");
        }

        private void CheckPartySearchResult(string result, int expectedcount)
        {
            List<PoliticalPartyDTO> partylist = JsonConvert.DeserializeObject<List<PoliticalPartyDTO>>(result);
            Assert.AreEqual(expectedcount, partylist.Count());
        }

        #endregion Search Party

    }
}
