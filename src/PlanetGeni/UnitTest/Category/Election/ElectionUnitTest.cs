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
    [Category("Election")]
    public class ElectionUnitTest
    {
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
        private IUserBankAccountDTORepository bankRepo = new UserBankAccountDTORepository();
        private ICountryCodeRepository countryRepo = new CountryCodeRepository();
        private UnitTestFixture setupFixture;
        public ElectionUnitTest()
        {
            string[] createtables = new string[] { "CountryTax", Category };
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
            string dataLoadsqlpath = @"\Sql\DataLoad" + Category + ".sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
                + dataLoadsqlpath));

            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());
            cache.FlushAllDatabase();


        }
        #region RunForOffice
        [Category("RunForOffice")]
        [Test]
        [TestCase(1, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531, 98304 }, new short[] { 2, 3, 4 }, "Pass", 0, "", 0, 0, false, true)]
        [TestCase(1, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531, 98304 }, new short[] { 2, 3, 4 }, "Pass", 0, "", 0, 0, false, false)]
        [TestCase(1, 1, "", "I", new int[] { 1920, 5855, 3518, 8723 }, new short[] { 2, 3, 4 }, "Fail", 92, "number of Friends endorsement must be at least 10", 0, 0, false, false)] //Not enough Friends
        [TestCase(1, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531, 98340 }, new short[] { 2 }, "Fail", 92, "number of Election Agenda must be between 3 and 6", 0, 0, false, false)] //Not enough agendas
        [TestCase(8, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531 }, new short[] { 2 }, "Fail", 92, "you do not have enough Cash to pay for elction fee.", 0, 0, false, false)] //Not enough money
        [TestCase(1920, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531 }, new short[] { 2, 3, 4 }, "Fail", 92, "you currently have a pending or Approved application to run for this election term", 1, 0, false, false)]
        [TestCase(1017, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531 }, new short[] { 2, 3, 4 }, "Fail", 92, "you cannot run for election after being elected in last 4 consecutive terms", 0, 0, false, false)]
        [TestCase(1, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531 }, new short[] { 2, 3, 4 }, "Fail", 92, "members have already been approved to run in this election term, which is the curret elction Cap", 0, 2, false, false)]
        [TestCase(1, 1, "", "I", new int[] { 1920, 5855, 3518, 8723, 119, 5375, 5, 6, 8, 432, 531 }, new short[] { 2, 3, 4 }, "Fail", 92, "of total population has already been approved to run in this election term", 0, 10, false, false)]
        [TestCase(1039, 1, "", "P", new int[] { }, new short[] { 2, 3, 4 }, "Fail", 92, "you are not current member of the selcted party", 0, 0, false, false)]
        [TestCase(1137, 1, "", "P", new int[] { }, new short[] { 2, 3, 4 }, "Fail", 92, "your Party is not in approved status", 0, 0, false, false)]
        [TestCase(1137, 1, "", "P", new int[] { }, new short[] { 2, 3, 4 }, "Fail", 92, "you already have 49% of your Party Members approved to run in this election term", 0, 0, true, false)]
        [TestCase(1, 1, "", "P", new int[0], new short[] { 2, 3, 4 }, "Pass", 0, "", 0, 0, false, false)]
        [TestCase(1, 1, "", "P", new int[0], new short[] { 2, 3, 4 }, "Pass", 0, "", 0, 0, false, true)]
        public void RunForOffice(int userid, sbyte positionTypeId, string logoPictureId,
           string candidateTypeId, int[] friendSelected, short[] agendas, string expectedResult, sbyte notificationType, string notificationMsg, int candidateCount, sbyte electionhardCap, bool addpartyMember, bool approveTask)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            RunForOfficeDTO runforOffice = new RunForOfficeDTO
            {
                PositionTypeId = positionTypeId,
                LogoPictureId = logoPictureId,
                CandidateTypeId = candidateTypeId,
                Agendas = agendas,
                FriendSelected = friendSelected,
                UserId = userid
            };
            sbyte oldElectionCap = RulesSettings.ElectionCandidateNumberHardCap;

            runforOffice.CountryId = webRepo.GetCountryId(runforOffice.UserId);
            runforOffice.CurrentTerm = eleRepo.GetCurrentElectionTerm(runforOffice.CountryId);
            if (electionhardCap > 0)
            {
                AddAprpovedCandidate(electionhardCap, runforOffice);
                RulesSettings.ElectionCandidateNumberHardCap = electionhardCap;
            }
            if (runforOffice.CandidateTypeId == "P" && addpartyMember)
            {
                runforOffice.PartyId = partyRepo.GetActivePartyId(runforOffice.UserId);
                AddPartyMember(runforOffice);
            }
            ElectionManager manager = new ElectionManager();
            UserBankAccount oldbankac = bankRepo.GetUserBankDetails(runforOffice.UserId);
            manager.ProcessAppForOffice(runforOffice);
            if (runforOffice.CandidateTypeId == "P")
            {
                runforOffice.FriendSelected =
                Array.ConvertAll(partyRepo.GetAllPartyMember(runforOffice.PartyId), int.Parse);

                runforOffice.FriendSelected =
                   runforOffice.FriendSelected.Where(val => val != runforOffice.UserId).ToArray();
            }

            if (expectedResult == "Pass")
            {
                CheckRunforOfficeResult(runforOffice, oldbankac);
                oldbankac = bankRepo.GetUserBankDetails(runforOffice.UserId);
                VoteRunForOffice(runforOffice, approveTask);
                if (!approveTask)
                {
                    CheckAfterVotingDenied(runforOffice, oldbankac);
                }
                else
                {
                    CheckAfterVotingApproved(runforOffice, oldbankac);
                }

            }
            else
            {
                CheckRunforOfficeResultFail(runforOffice, oldbankac, notificationMsg, candidateCount, notificationType);
            }
            RulesSettings.ElectionCandidateNumberHardCap = oldElectionCap;



            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than  25 seconds");
        }

        private void CheckRunforOfficeResult(RunForOfficeDTO runforOffice, UserBankAccount oldbankac)
        {
            CheckElectionAgenda(runforOffice, runforOffice.Agendas.Length);
            runforOffice.TaskId = CheckElectionCandidate(runforOffice, 1, "P");
            ICountryTaxDetailsDTORepository countrytaxRepo = new CountryTaxDetailsDTORepository();
            decimal totaltax = runforOffice.CurrentTerm.Fee * countrytaxRepo.GetCountryTaxByCode(runforOffice.CountryId, AppSettings.TaxElectionCode) / 100;
            decimal total = runforOffice.CurrentTerm.Fee;
            CountryCode countryCode =
            countryRepo.GetCountryCode(runforOffice.CountryId);
            setupFixture.CheckCapitalTransactionLog(runforOffice.UserId, countryCode.CountryUserId, total, totaltax, AppSettings.ElectionFeeFundType, 1);
            setupFixture.CheckUserBankAccount(oldbankac, -(total + totaltax), 0, 0);
            WebUserDTO webinfo = webRepo.GetUserPicFName(runforOffice.UserId);
            runforOffice.Picture = webinfo.Picture;
            runforOffice.FullName = webinfo.FullName;

            short votereuqestNotifyType = AppSettings.RunForOfficeIndividualVotingRequestNotificationId;
            sbyte postType = AppSettings.RunForOfficeIndividualPostContentTypeId;
            short taskType = AppSettings.RunForOfficeAsIndividualTaskType;

            if (runforOffice.CandidateTypeId == "P")
            {
                votereuqestNotifyType = AppSettings.RunForOfficePartyVotingRequestNotificationId;
                postType = AppSettings.RunForOfficePartyPostContentTypeId;
                taskType = AppSettings.RunForOfficeAsPartyTaskType;

            }
            setupFixture.CheckUserNotification(runforOffice.FriendSelected, PrepareNotifcationParm(runforOffice, votereuqestNotifyType),
                votereuqestNotifyType, true, runforOffice.FriendSelected.Length, runforOffice.FriendSelected.Length);
            setupFixture.CheckUserNotification(new int[] { runforOffice.UserId }, new string[0],
              AppSettings.RunForOfficeSuccessNotificationId, false, 1, 1);

            if (runforOffice.CandidateTypeId == "P")
            {
                setupFixture.CheckPost(new string[] { runforOffice.TaskId.ToString() }, PreparePostConetentParm(runforOffice, postType), postType, 1, 0, string.Empty, new Guid(runforOffice.PartyId));
            }
            else
            {
                setupFixture.CheckPost(new string[] { runforOffice.TaskId.ToString() }, PreparePostConetentParm(runforOffice, postType), postType, 1, runforOffice.UserId, string.Empty, new Guid());
            }


            setupFixture.CheckUserTask(runforOffice.FriendSelected, PrepareNotifcationParm(runforOffice, votereuqestNotifyType), taskType, runforOffice.FriendSelected.Length);
        }
        private void CheckRunforOfficeResultFail(RunForOfficeDTO runforOffice, UserBankAccount oldbankac, string notificationMsg, int candidateCount, sbyte notificationType)
        {

            CheckElectionAgenda(runforOffice, 0);
            runforOffice.TaskId = CheckElectionCandidate(runforOffice, candidateCount, "P");
            ICountryTaxDetailsDTORepository countrytaxRepo = new CountryTaxDetailsDTORepository();

            CountryCode countryCode =
            countryRepo.GetCountryCode(runforOffice.CountryId);
            setupFixture.CheckCapitalTransactionLog(runforOffice.UserId, countryCode.CountryUserId, 0, 0, AppSettings.ElectionFeeFundType, 0);
            setupFixture.CheckUserBankAccount(oldbankac, 0, 0, 0);

            setupFixture.CheckUserNotification(new int[] { runforOffice.UserId }, new string[] { notificationMsg },
              notificationType, false, 1, 1);
            setupFixture.CheckPost(new string[] { runforOffice.TaskId.ToString() }, new string[0], 0, 0, runforOffice.UserId, string.Empty, new Guid());
            setupFixture.CheckUserTask(runforOffice.FriendSelected, new string[0], 0, 0);
        }

        private void CheckAfterVotingDenied(RunForOfficeDTO runforOffice, UserBankAccount oldbankac)
        {
            CheckElectionCandidate(runforOffice, 1, "D");
            setupFixture.CheckUserBankAccount(runforOffice.UserId, oldbankac.Cash, oldbankac.Gold, oldbankac.Silver);

            int totalVoters;
            double voteNeeded;
            int approvalChoiceId;
            int denialChoiceId;
            totalVoters = runforOffice.FriendSelected.Length;
            int denyVoters = 0;
            int[] voters;
            int score;
            if (runforOffice.CandidateTypeId == "I")
            {

                voteNeeded = AppSettings.ElectionRunForOfficeIndividualApprovalVoteNeeded;
                approvalChoiceId = AppSettings.RunForOfficeIndividualApprovalChoiceId
;
                denialChoiceId = AppSettings.RunForOfficeIndividualDenialChoiceId;
                denyVoters = 2;
                voters = new int[] { runforOffice.FriendSelected[1], runforOffice.FriendSelected[2] };
                score = denyVoters;
            }
            else
            {
                PoliticalParty partyInfo = partyRepo.GetPartyById(runforOffice.PartyId.ToString());
                int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0; totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
               partyFounderSize * AppSettings.FounderVoteScore
              ;
                voteNeeded = totalVoters * AppSettings.ElectionRunForOfficePartyApprovalVoteNeeded;
                approvalChoiceId = AppSettings.RunForOfficePartyApprovalChoiceId;
                denialChoiceId = AppSettings.RunForOfficePartyDenialChoiceId;
                denyVoters = 8;
                voters = new int[denyVoters];
                for (int i = 0; i < denyVoters; i++)
                {
                    voters[i] = runforOffice.FriendSelected[i + 1];
                }
                score = 26;
            }

            setupFixture.CheckUserNotification(new int[] { runforOffice.UserId }, PrepareRunForOfficeCountNotficationMsg(runforOffice, Convert.ToInt32(voteNeeded), totalVoters),
                96, false, 1, denyVoters);

            setupFixture.CheckUserNotification(new int[] { runforOffice.UserId }, PrepareRunForOfficeResultNotficationMsg(runforOffice, "Denied"),
          95, false, 1, 1);
            setupFixture.CheckUserVoteSelectedChoice(runforOffice.TaskId, denialChoiceId,
                 score, voters);

            setupFixture.CheckUserVoteSelectedChoice(runforOffice.TaskId, approvalChoiceId,
      1, new int[] { runforOffice.FriendSelected[0] });



        }
        private void CheckAfterVotingApproved(RunForOfficeDTO runforOffice, UserBankAccount oldbankac)
        {
            CheckElectionCandidate(runforOffice, 1, "A");
            setupFixture.CheckUserBankAccount(runforOffice.UserId, oldbankac.Cash, oldbankac.Gold, oldbankac.Silver);

            int totalVoters;
            double voteNeeded;
            int approvalChoiceId;
            int denialChoiceId;
            totalVoters = runforOffice.FriendSelected.Length;
            int approveVoters = 0;
            int[] voters;
            int score;
            if (runforOffice.CandidateTypeId == "I")
            {
                voteNeeded = AppSettings.ElectionRunForOfficeIndividualApprovalVoteNeeded;
                approvalChoiceId = AppSettings.RunForOfficeIndividualApprovalChoiceId
;
                denialChoiceId = AppSettings.RunForOfficeIndividualDenialChoiceId;
                approveVoters = 10;
                voters = new int[approveVoters];
                for (int i = 0; i < approveVoters; i++)
                {
                    voters[i] = runforOffice.FriendSelected[i + 1];
                }
                score = approveVoters;
            }
            else
            {
                PoliticalParty partyInfo = partyRepo.GetPartyById(runforOffice.PartyId.ToString());
                int partyFounderSize = partyInfo.PartyFounder > 0 ? 1 : 0;
                totalVoters = (partyInfo.PartySize - partyInfo.CoFounderSize) - partyFounderSize + partyInfo.CoFounderSize * AppSettings.CoFounderVoteScore +
                    partyFounderSize * AppSettings.FounderVoteScore
                   ;
                voteNeeded = totalVoters * AppSettings.ElectionRunForOfficePartyApprovalVoteNeeded;
                approvalChoiceId = AppSettings.RunForOfficePartyApprovalChoiceId;
                denialChoiceId = AppSettings.RunForOfficePartyDenialChoiceId;
                approveVoters = 6;
                voters = new int[approveVoters];
                for (int i = 0; i < approveVoters; i++)
                {
                    voters[i] = runforOffice.FriendSelected[i + 1];
                }
                score = 10;
            }

            setupFixture.CheckUserNotification(new int[] { runforOffice.UserId }, PrepareRunForOfficeCountNotficationMsg(runforOffice, Convert.ToInt32(voteNeeded), totalVoters),
              96, false, 1, approveVoters);

            setupFixture.CheckUserNotification(new int[] { runforOffice.UserId }, PrepareRunForOfficeResultNotficationMsg(runforOffice, "Approved"),
        95, false, 1, 1);
            setupFixture.CheckUserVoteSelectedChoice(runforOffice.TaskId, approvalChoiceId,
               score, voters);

            setupFixture.CheckUserVoteSelectedChoice(runforOffice.TaskId, denialChoiceId,
    1, new int[] { runforOffice.FriendSelected[0] });



        }
        private void AddAprpovedCandidate(int count, RunForOfficeDTO runforOffice)
        {
            if (count > 3)
            {
                count = count - 3;
            }
            ElectionCandidate candidate = new ElectionCandidate
            {
                CandidateTypeId = runforOffice.CandidateTypeId,
                ElectionId = runforOffice.CurrentTerm.ElectionId,
                PositionTypeId = runforOffice.PositionTypeId,
                Status = "A",
                RequestDate = DateTime.UtcNow,
                CountryId = runforOffice.CountryId,
                LogoPictureId = runforOffice.LogoPictureId
            };
            count++;

            for (int i = 0; i <= count; i++)
            {
                candidate.TaskId = Guid.NewGuid();
                candidate.UserId = runforOffice.UserId + 1;
                spContext.Add(candidate);
            }

        }
        private void AddPartyMember(RunForOfficeDTO runforOffice)
        {

            PartyMember member = new PartyMember
            {
                MemberStartDate = DateTime.UtcNow.AddDays(-10),
                MemberStatus = "A",
                MemberType = "M",
                PartyId = new Guid(runforOffice.PartyId),
                UserId = 20
            };
            spContext.Add(member);
        }
        private string[] PrepareRunForOfficeCountNotficationMsg(RunForOfficeDTO runforOffice, int voteNeeded, int totalVoters)
        {
            StringBuilder notificationparmText = new StringBuilder();
            notificationparmText.AppendFormat("|<strong>Date:{0}</strong>|{1}|{2}",
      runforOffice.CurrentTerm.VotingStartDate,
     voteNeeded, totalVoters);

            string[] notifcationtext = new string[runforOffice.FriendSelected.Length];
            for (int i = 0; i < runforOffice.FriendSelected.Length; i++)
            {
                notifcationtext[i] = notificationparmText.ToString();
            }
            return notifcationtext;

        }
        private string[] PrepareRunForOfficeResultNotficationMsg(RunForOfficeDTO runforOffice, string result)
        {
            StringBuilder notificationparmText = new StringBuilder();
            notificationparmText.AppendFormat("<strong>Date:{0}</strong>|{1}|",
                runforOffice.CurrentTerm.VotingStartDate, result);

            string[] notifcationtext = new string[runforOffice.FriendSelected.Length];
            for (int i = 0; i < runforOffice.FriendSelected.Length; i++)
            {
                notifcationtext[i] = notificationparmText.ToString();
            }
            return notifcationtext;

        }
        private string[] PrepareNotifcationParm(RunForOfficeDTO runforOffice, short msgType)
        {

            StringBuilder notificationparmText = new StringBuilder();
            if (msgType == AppSettings.RunForOfficeIndividualVotingRequestNotificationId)
            {
                notificationparmText.AppendFormat("{0}|{1}|{2}",
                          runforOffice.UserId, runforOffice.FullName,
                      runforOffice.TaskId
                         );
            }
            else if (msgType == AppSettings.RunForOfficePartyVotingRequestNotificationId)
            {
                notificationparmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                         runforOffice.UserId, runforOffice.FullName,
                        runforOffice.PartyId, runforOffice.PartyName, runforOffice.TaskId
                        );
            }

            string[] notifcationtext = new string[runforOffice.FriendSelected.Length];
            for (int i = 0; i < runforOffice.FriendSelected.Length; i++)
            {
                notifcationtext[i] = notificationparmText.ToString();
            }
            return notifcationtext;

        }
        private string[] PreparePostConetentParm(RunForOfficeDTO runforOffice, sbyte postType)
        {

            StringBuilder postParms = new StringBuilder();
            if (postType == AppSettings.RunForOfficeIndividualPostContentTypeId)
            {
                postParms.AppendFormat("{0}|{1}|{2}|{3}",
                           runforOffice.UserId,
                          runforOffice.Picture
                          , runforOffice.FullName
                          , runforOffice.TaskId);
            }
            else if (postType == AppSettings.RunForOfficePartyPostContentTypeId)
            {
                postParms.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}",
                runforOffice.UserId,
               runforOffice.Picture
               , runforOffice.FullName
               , runforOffice.PartyId, runforOffice.PartyName,
               runforOffice.TaskId);
            }
            string[] posttext = new string[1];
            for (int i = 0; i < 1; i++)
            {
                posttext[i] = postParms.ToString();
            }
            return posttext;

        }

        private void VoteRunForOffice(RunForOfficeDTO runForOffice, bool approve)
        {

            int approvalChoiceId = AppSettings.RunForOfficePartyApprovalChoiceId;
            int denialChoiceId = AppSettings.RunForOfficePartyDenialChoiceId;
            short taskType = AppSettings.RunForOfficeAsPartyTaskType;
            if (runForOffice.CandidateTypeId == "I")
            {
                approvalChoiceId = AppSettings.RunForOfficeIndividualApprovalChoiceId;
                denialChoiceId = AppSettings.RunForOfficeIndividualDenialChoiceId;
                taskType = AppSettings.RunForOfficeAsIndividualTaskType;

            }
            if (!approve)
            {
                int temp = approvalChoiceId;
                approvalChoiceId = denialChoiceId;
                denialChoiceId = temp;
            }

            List<Voters> voters = new List<Voters>();
            int deniyer = runForOffice.FriendSelected[0];
            voters.Add(new Voters
            {
                ChoiceId = denialChoiceId,
                TaskVoters = new List<int> { deniyer }
            });
            voters.Add(new Voters
            {
                ChoiceId = approvalChoiceId,
                TaskVoters = runForOffice.FriendSelected.Where(val => val != deniyer).ToList()
            });

            setupFixture.VoteonTask(voters, runForOffice.TaskId, taskType);
        }
        #endregion RunForOffice

        #region ElectionVoting
        [Category("ElectionVoting")]
        [Test]
        [TestCase(new int[] { 1, 1620, 1818, 312 }, 1, "Pass", 1, "")]
        [TestCase(new int[] { 1, 1620, 1818, 312 }, 3689, "Fail", 1, "you can only vote once every election sesion")]
        [TestCase(new int[] { 1, 1620, 1818, 312 }, 6, "Fail", 0, "candidate was not found")]
        [TestCase(new int[] { 1, 1620, 1818, 312 }, 11, "Fail", 0, "candidate was not found")]
        [TestCase(new int[] { 1, 1620, 1818, 312, 3084, 4815 }, 11, "Fail", 0, "you cannot vote for more than 5 candidates")]
        public void ElectionVoting(int[] candidates, int userid, string expectedResult, int electioVoteCount, string parmmsg)
        {
            setupFixture.LoadDataTable("ElectionCandidate-1", rootFolderCategory);
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);

            CandidateVotingDTO candidateVoting = new CandidateVotingDTO
            {
                Candidates = candidates,
                UserId = userid
            };
            ElectionManager manager = new ElectionManager();
            candidateVoting.CountryId = webRepo.GetCountryId(candidateVoting.UserId);
            candidateVoting.ElectionInfo = eleRepo.GetCurrentElectionTerm(candidateVoting.CountryId);
            string getelectionVotersql = string.Format("Select * from ElectionVoting Where UserId in  ({0}) and ElectionId ={1} and CountryId ='{2}'", string.Join(",", candidates),
                candidateVoting.ElectionInfo.ElectionId, candidateVoting.CountryId);
            List<ElectionVoting> eleVoter = spContext.GetSqlData<ElectionVoting>(getelectionVotersql).ToList();


            manager.ProcessVoteElection(candidateVoting);

            if (expectedResult == "Pass")
            {
                CheckElectionVotingPassed(candidateVoting, eleVoter, electioVoteCount);
            }
            else
            {
                CheckElectionVotingFailed(candidateVoting, eleVoter, electioVoteCount, parmmsg);

            }
            int newCount = UnitUtility.ElmahErrorCount(spContext);
            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than  25 seconds");
        }

        private void CheckElectionVotingPassed(CandidateVotingDTO candidateVoting, List<ElectionVoting> eleVoter, int electioVoteCount)
        {
            CheckElectionVoter(candidateVoting, electioVoteCount);
            setupFixture.CheckUserNotification(new int[] { candidateVoting.UserId }, new string[] { }, AppSettings.ElectionVotingSuccessNotificationId, false, 1, 1);
            foreach (var item in eleVoter)
            {
                CheckElectionVoting(candidateVoting.ElectionInfo.ElectionId,
                  item.UserId, item.Score + 1
              , candidateVoting.UserCountryId, item.ElectionResult);
            }

        }

        private void CheckElectionVotingFailed(CandidateVotingDTO candidateVoting, List<ElectionVoting> eleVoter, int electioVoteCount, string parmmsg)
        {
            CheckElectionVoter(candidateVoting, electioVoteCount);
            setupFixture.CheckUserNotification(new int[] { candidateVoting.UserId }, new string[] { parmmsg }, AppSettings.ElectionVotingFailNotificationId, false, 1, 1);
            foreach (var item in eleVoter)
            {
                CheckElectionVoting(candidateVoting.ElectionInfo.ElectionId,
                  item.UserId, item.Score
              , candidateVoting.UserCountryId, item.ElectionResult);
            }

        }

        private void CheckElectionVoting(int electionId, int userId, int score, string countryId, string result)
        {
            string getelectionVotersql = string.Format("Select * from ElectionVoting Where UserId in  ({0}) and ElectionId ={1} and CountryId ='{2}'", userId, electionId, countryId);
            List<ElectionVoting> eleVoter = spContext.GetSqlData<ElectionVoting>(getelectionVotersql).ToList();
            Assert.AreEqual(eleVoter.Count(), 1);
            Assert.AreEqual(eleVoter[0].Score, score);
            Assert.AreEqual(eleVoter[0].ElectionResult, result);

        }
        private void CheckElectionVoter(CandidateVotingDTO candidateVoting, int count)
        {
            string getelectionVotersql = string.Format("Select * from ElectionVoter Where UserId in  ({0}) and ElectionId ={1}", candidateVoting.UserId, candidateVoting.ElectionInfo.ElectionId);
            List<ElectionVoter> eleVoter = spContext.GetSqlData<ElectionVoter>(getelectionVotersql).ToList();
            Assert.AreEqual(eleVoter.Count(), count);
        }
        #endregion ElectionVOting

        #region ElectionVotingJob

        [Test]

        public void ElectionVotingJob()
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            int oldCount = UnitUtility.ElmahErrorCount(spContext);
            IWebUserDTORepository webRepo = new WebUserDTORepository();
            ElectionVotingManager eleManager = new ElectionVotingManager();
            SetupDataElectionVotingCount();
            RulesSettings.SeneateSeatHardCap = 2;
            eleManager.StatVoteCount(0);
            CheckElectionVotingCountResult();
            int newCount = UnitUtility.ElmahErrorCount(spContext);

            Assert.AreEqual(oldCount, newCount);
            timer.Stop();
            int elapsedSeconds = timer.Elapsed.Seconds;
            Assert.IsTrue(elapsedSeconds < 25, "Should take less than 25 seconds");
        }
        private void SetupDataElectionVotingCount()
        {
            string dataLostockqlpath = @"\Sql\DataLoadElectionVotingJob.sql";

            StringBuilder boostrapSql = new StringBuilder();
            boostrapSql.Append(File.ReadAllText(rootFolder + rootFolderCategory
                + dataLostockqlpath));

            string dataPath = rootFolder + rootFolderCategory + @"Sql\Data\";
            boostrapSql.Replace("{0}", dataPath.Replace(@"\", @"\\"));
            spContext.ExecuteSql(boostrapSql.ToString());
        }
        private void CheckElectionVotingCountResult() {
            CheckElectionResult();
            CheckElectionVoting();
            CheckCountryLeader();
            CheckNotfication();
        }

        private void CheckNotfication()
        {
            setupFixture.CheckUserNotification(new int[] { 246, 1035, 351, 1192367 }, new string[] { }, AppSettings.ElectionVotingWinNotificationId, false, 0, 4);
        setupFixture.CheckUserNotification(new int[] { 27774, 339, 12188, 14576, 19236, 1, 11, 13, 14, 15, 80 }, new string[] { }, AppSettings.ElectionVotingLostNotificationId, false, 0, 11);
        setupFixture.CheckPost(AppSettings.ElectionVictoryContentTypeId, 1, "np");
        setupFixture.CheckPost(AppSettings.ElectionVictoryContentTypeId, 2, "se");
        setupFixture.CheckPost(AppSettings.ElectionVictoryContentTypeId, 1, "us");
        setupFixture.CheckPost(AppSettings.NotifyStartOfElectionPeroidContentTypeId, 240, "", new Guid());
        }

        private void CheckElectionResult()
        {
            string getElectionSql = "Select * from Election";
            List<Election> election = spContext.GetSqlData<Election>(getElectionSql).ToList();
            Assert.AreEqual(election.Count, 480);
            Assert.AreEqual(election.Count(f => f.ElectionId == 2
                && f.EndDate > f.VotingStartDate
                && f.VotingStartDate > f.StartDate
                && f.StartDate.Date == DateTime.UtcNow.Date
                && f.EndDate.Date == DateTime.UtcNow.AddDays(RulesSettings.NumberOfDaysofElection + RulesSettings.NumberOfDaysToElection).Date
                ), 240);
            Assert.AreEqual(election.Count(f => f.ElectionId == 2 && f.CountryId == "np" && f.Fee == 51000), 1);
            Assert.AreEqual(election.Count(f => f.ElectionId == 2 && f.CountryId == "us" && f.Fee == 51000), 1);
            Assert.AreEqual(election.Count(f => f.ElectionId == 2 && f.CountryId == "se" && f.Fee == 51000), 1);
            Assert.AreEqual(election.Count(f => f.ElectionId == 2 &&  f.Fee == 50000), 237);
            Assert.AreEqual(election.Count(f => f.ElectionId == 1 && f.EndDate.Date == DateTime.UtcNow.Date), 240);
            Assert.AreEqual(election.Count(f => f.ElectionId == 2 && f.StartTermNotified), 240);
        }
        private void CheckElectionVoting()
        {
            string getElectionVotingSql = "Select * from ElectionVoting";
            List<ElectionVoting> ekectionVoting = spContext.GetSqlData<ElectionVoting>(getElectionVotingSql).ToList();
            Assert.AreEqual(ekectionVoting.Count, 15);
            Assert.AreEqual(ekectionVoting.Count(f => f.ElectionId == 1
              && f.ElectionResult == "W"), 4);
            Assert.AreEqual(ekectionVoting.Count(f => f.ElectionId == 1
                        && f.ElectionResult == "L"), 11);
            Assert.AreEqual(ekectionVoting.Count(f => f.ElectionId == 1 && f.CountryId == "np" && f.ElectionResult == "W"), 1);
            Assert.AreEqual(ekectionVoting.Count(f => f.ElectionId == 1 && f.CountryId == "us" && f.ElectionResult == "W"), 1);
            Assert.AreEqual(ekectionVoting.Count(f => f.ElectionId == 1 && f.CountryId == "se" && f.ElectionResult == "W"), 2);
        }

        private void CheckCountryLeader()
        {
            string getCountryLeaderSql = "Select * from CountryLeader";
            List<CountryLeader> countryLeader = spContext.GetSqlData<CountryLeader>(getCountryLeaderSql).ToList();
            Assert.AreEqual(countryLeader.Count, 4);
            Assert.AreEqual(countryLeader.Count(f =>
                f.StartDate.Date == DateTime.UtcNow.Date
                && f.EndDate.Date == DateTime.UtcNow.AddDays(365).Date
                ), 4);
            Assert.AreEqual(countryLeader.Count(f => f.EndDate.Date == DateTime.UtcNow.AddDays(365).Date && f.CountryId == "np"), 1);
            Assert.AreEqual(countryLeader.Count(f => f.EndDate.Date == DateTime.UtcNow.AddDays(365).Date && f.CountryId == "us"), 1);
            Assert.AreEqual(countryLeader.Count(f => f.EndDate.Date == DateTime.UtcNow.AddDays(365).Date && f.CountryId == "se"), 2);
        }
        #endregion ElectionVotingJob

        private void CheckElectionAgenda(RunForOfficeDTO runforOffice, int count)
        {

            string getCandidateAgendasql = string.Format("Select * from CandidateAgenda Where ElectionId = {0} and UserId={1} ", runforOffice.CurrentTerm.ElectionId, runforOffice.UserId);
            List<CandidateAgenda> electionCandidate = spContext.GetSqlData<CandidateAgenda>(getCandidateAgendasql).ToList();

            Assert.AreEqual(electionCandidate.Count(), count);
            if (count > 0)
            {

                foreach (var item in runforOffice.Agendas)
                {
                    Assert.AreEqual(electionCandidate.Count(f => f.AgendaTypeId == item), 1);
                }
            }
        }
        private Guid CheckElectionCandidate(RunForOfficeDTO runforOffice, int count, string status)
        {

            string getElectionAgendasql = string.Format("Select * from ElectionCandidate Where ElectionId = {0} and UserId={1} ", runforOffice.CurrentTerm.ElectionId, runforOffice.UserId);
            List<ElectionCandidate> electionCandidate = spContext.GetSqlData<ElectionCandidate>(getElectionAgendasql).ToList();

            Assert.AreEqual(electionCandidate.Count(), count);

            Assert.AreEqual(electionCandidate.Count(f => f.CandidateTypeId == runforOffice.CandidateTypeId &&
                f.CountryId == runforOffice.CountryId &&
                f.ElectionId == runforOffice.CurrentTerm.ElectionId &&
                f.PositionTypeId == runforOffice.PositionTypeId &&
               f.LogoPictureId == runforOffice.LogoPictureId &&
               f.Status == status), count);
            if (count == 0)
            {
                return new Guid();
            }
            return electionCandidate[0].TaskId;

        }

    }
}
