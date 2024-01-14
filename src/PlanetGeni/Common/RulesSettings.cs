using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class RulesSettings
    {
        #region Election
        static public double ElectionCapPercent { get; set; }
        static public double ElectionPartyCapPercent { get; set; }
        static public sbyte ElectionCandidateNumberHardCap { get; set; }
        static public double SenetaorSeatCapPercent { get; set; }
        static public int SeneateSeatHardCap { get; set; }

        static public sbyte MaxConsecutiveTerm { get; set; }
        static public sbyte MinmumFriendsEndorsement { get; set; }
        static public sbyte MinmumAgenda { get; set; }
        static public sbyte MaximumAgenda { get; set; }
        static public sbyte MaximumElectionVotingCandidate { get; set; }
        static public sbyte NumberOfDaysToElection { get; set; }
        static public sbyte NumberOfDaysofElection { get; set; }
        static public decimal EelctionFee { get; set; }
        #endregion Election
        #region Stock
        static public sbyte StockThresholdForStockPrice { get; set; }
        static public decimal StockDividentRate { get; set; }
        static public decimal StockDividentCap { get; set; }
        static public int StockDaysHistory { get; set; }

        #endregion
        #region InitializeUser
        static public decimal InitialGold { get; set; }
        static public decimal InitialCash { get; set; }
        static public decimal InitialSilver { get; set; }
        static public decimal InitializeCreditScore { get; set; }
        static public string InitialPicture { get; set; }
        #endregion InitializeUser
        #region Rent Job
        static public decimal WeeklyRent { get; set; }
        static public decimal NorentPenalty { get; set; }
        static public decimal ColleRentalCapForPost { get; set; }

        #endregion Rent Job
        #region Lottery
        static public decimal LotteryCapForPost { get; set; }
        static public decimal LotteryPick5Match1 { get; set; }
        static public decimal LotteryPick5Match2 { get; set; }
        static public decimal LotteryPick5Match3 { get; set; }
        static public decimal LotteryPick5Match4 { get; set; }
        static public decimal LotteryPick5Match5 { get; set; }
        static public decimal LotteryPick3Match1 { get; set; }
        static public decimal LotteryPick3Match2 { get; set; }
        static public decimal LotteryPick3Match3 { get; set; }
        static public sbyte Pick5Frequency { get; set; }
        static public sbyte Pick3Frequency { get; set; }
        #endregion Lottery
        #region BudgetStimulator
        static public decimal PopulationScale { get; set; }
        static public decimal WarWinScale { get; set; }
        static public decimal LooseWinScale { get; set; }
        static public int WinDaysCredit { get; set; }
        static public int LostDaysCredit { get; set; }

        #endregion BudgetStimulator
        #region JobMatch
        static public short MatchMajorFactor { get; set; }
        static public short DurationJobCodeFactor { get; set; }
        static public short DurationIndustryFactor { get; set; }
        static public short DurationAnyExperinceFactor { get; set; }
        #endregion JobMatch
        #region UserJob
        static public decimal MaxHPW { get; set; }
        static public sbyte ExpireJobEmailDayInterval { get; set; } //When to send emails about expired/expring jobs

        #endregion UserJob
        #region PostComment
        static public decimal SpotCostFactor { get; set; }
        static public decimal SpotImageCostFactor { get; set; }
        #endregion PostComment
        #region Social Assets
        static public decimal InviteCredit { get; set; }
        static public decimal InviteAcceptedCredit { get; set; }
        #endregion Social
        #region Casino
        static public decimal SlotMachineAll3MatchAwardFactor { get; set; }
        static public decimal RouleteMatchAwardFactor { get; set; }

        static public decimal SlotMachine2MatchAwardFactor { get; set; }
        #endregion Casino

        #region Robbery
        static public short RobberyFrequencyonSamePersonCap { get; set; }
        static public decimal RobberyIncidentReportWantedFactor { get; set; }
        static public decimal RobberySuspectReportWantedFactor { get; set; }
        static public sbyte RobberyMaxQuantity { get; set; }
        static public decimal RobberyMaxWantedLevel { get; set; }
        static public decimal IncidentPerDayRate { get; set; }
        static public decimal DeductionPerDayRate { get; set; }
        static public decimal MaxAllowedPickPocketPercent { get; set; }
        static public sbyte MaxAllowedSuspectReportPerIncident { get; set; }
        static public decimal RobberyAssetSeizePercent { get; set; }



        #endregion Robbery

        #region EmailNotifcation Grow Users
        static public short MinimumInviteExpectedToBeSent { get; set; }
        static public short MinimumNumberOfFriendsExpected { get; set; }

        #endregion EmailNotifcation Grow Users

        #region UserNotification
        static public sbyte TimeLimitLastLoginForEmailNotificationinHours { get; set; }

        #endregion UserNotification
        static RulesSettings()
        {
            #region Election
            ElectionCapPercent = 0.5;
            ElectionCandidateNumberHardCap = 100;
            SenetaorSeatCapPercent = ElectionCapPercent / 2;
            SeneateSeatHardCap = ElectionCandidateNumberHardCap / 2;
            MaxConsecutiveTerm = 4;
            ElectionPartyCapPercent = 0.49;
            MinmumAgenda = 3;
            MaximumAgenda = 6;
            MinmumFriendsEndorsement = 10;
            MaximumElectionVotingCandidate = 5;
            NumberOfDaysToElection = 5;
            NumberOfDaysofElection = 2;
            EelctionFee = 50000;
            #endregion Election

            #region Stock
            StockThresholdForStockPrice = 10;
            StockDividentRate = 4;
            StockDividentCap = 2;
            StockDaysHistory = -2;
            #endregion Stock

            #region InitializeUser
            InitialGold = 5;
            InitialCash = 50000;
            InitialSilver = 5;
            InitialPicture = "0.png";
            InitializeCreditScore = 100;
            #endregion InitializeUser

            #region Rent Job
            WeeklyRent = 120;
            NorentPenalty = -100;
            ColleRentalCapForPost = 1500;

            #endregion Rent Job

            #region Lottery
            LotteryCapForPost = 1000;

            LotteryPick5Match1 = 500;
            LotteryPick5Match2 = 5000;
            LotteryPick5Match3 = 50000;
            LotteryPick5Match4 = 500000;
            LotteryPick5Match5 = 50000000;

            LotteryPick3Match1 = 100;
            LotteryPick3Match2 = 5000;
            LotteryPick3Match3 = 50000;
            Pick5Frequency = 1;
            Pick3Frequency = 1;
            #endregion Lottery

            #region BudgetStimulator
            PopulationScale = 500;
            WarWinScale = 10;
            LooseWinScale = 10;
            WinDaysCredit = 45;
            LostDaysCredit = 45;
            #endregion BudgetStimulator

            #region JobMatch
            MatchMajorFactor = 96;
            DurationJobCodeFactor = 10;
            DurationIndustryFactor = 50;
            DurationAnyExperinceFactor = 100;
            #endregion JobMatch

            #region UserJob
            MaxHPW = 80;
            ExpireJobEmailDayInterval = 1;
            #endregion UserJob
            #region PostComment
            SpotCostFactor = 0.05M;
            SpotImageCostFactor = 0.05M;
            #endregion PostComment

            #region Social Assets
            InviteCredit = 500;
            InviteAcceptedCredit = 5000;
            #endregion Social Assets

            #region UserNotification
            TimeLimitLastLoginForEmailNotificationinHours = 2;
            #endregion UserNotification

            #region Casino
            SlotMachineAll3MatchAwardFactor = 2;
            RouleteMatchAwardFactor = 10;
            SlotMachine2MatchAwardFactor = 0.5M;
            #endregion Casino

            #region Robbery
            RobberyFrequencyonSamePersonCap = 1;
            RobberyIncidentReportWantedFactor = 0.5M;
            RobberySuspectReportWantedFactor = 1;
            RobberyMaxQuantity = 1;
            RobberyMaxWantedLevel = 5;
            IncidentPerDayRate = 0.25M;
            DeductionPerDayRate = 0.50M;
            MaxAllowedPickPocketPercent = 10;
            MaxAllowedSuspectReportPerIncident = 1;
            RobberyAssetSeizePercent = 90;
            #endregion Robbery

            #region EmailNotifcation Grow Users
            MinimumInviteExpectedToBeSent = 10;
            MinimumNumberOfFriendsExpected = 20;
            #endregion EmailNotifcation Grow Users

        }
    }
}
