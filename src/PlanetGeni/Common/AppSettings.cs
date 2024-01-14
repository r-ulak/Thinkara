using System;
using System.Configuration;

namespace Common
{
    public static class AppSettings
    {
        static public string ImageUploadPath { get; set; }
        static public string FileUploadPath { get; set; }
        static public string FileUploadPathThumb { get; set; }


        static public bool OnlyAllowedUsers { get; set; }
        static public bool SendEmailInvite { get; set; }
        static public bool RegisterInviteOnly { get; set; }
        static public bool SendEmailNotfication { get; set; }
        static public bool RedisSSL { get; set; }
        static public string RedisPassword { get; set; }
        static public int RedisDatabaseId { get; set; }
        static public int RedisAutocompleteDatabaseId { get; set; }
        static public string RedisServer { get; set; }
        static public string RedisSessionStateServer { get; set; }
        static public int RedisPort { get; set; }
        static public int RedisSyncTimeOut { get; set; }
        static public string RedisKeyDegreeCodes { get; set; }
        static public string RedisKeyTaskType { get; set; }
        static public string RedisKeyElectionAgendas { get; set; }
        static public string RedisKeyPartyCoFounders { get; set; }
        static public string RedisKeyPartyFounders { get; set; }
        static public string RedisKeyPartyMembers { get; set; }
        static public string RedisKeyAdsDTO { get; set; }
        static public string RedisKeyMajorCodes { get; set; }
        static public string RedisKeyCountryCodes { get; set; }
        static public string RedisHashCountryName { get; set; }
        static public string RedisKeyCountryName { get; set; }
        static public string RedisKeyParties { get; set; }
        static public string RedisKeyActiveSeneator { get; set; }
        static public string RedisKeyWeaponCodes { get; set; }
        static public string RedisKeyWeaponSummary { get; set; }
        static public string RedisKeyMerchandiseCodes { get; set; }
        static public string RedisHashMerchandiseCodes { get; set; }
        static public string RedisKeyMerchandiseSummary { get; set; }
        static public string RedisKeyMerchandiseSummaryTop10 { get; set; }
        static public string RedisKeyWeaponSummaryTop10 { get; set; }
        static public string RedisKeyVoteChoices { get; set; }
        static public string RedisKeyCurrentTax { get; set; }
        static public string RedisKeyActiveSeneatorCount { get; set; }
        static public string RedisKeyCurrentTaxCode { get; set; }
        static public string RedisKeyStockCode { get; set; }
        static public string RedisKeyCurrentStockPrice { get; set; }
        static public string RedisKeyStockSummaryTop10 { get; set; }
        static public string RedisKeyNotificationType { get; set; }
        static public string RedisKeyEducationSummaryTop10 { get; set; }
        static public string RedisKeyJobIncomeSummaryTop10 { get; set; }
        static public string RedisKeyJobCodeSearch { get; set; }
        static public string RedisKeyIndustryCodes { get; set; }
        static public string RedisKeyPartyByMemberTop10 { get; set; }
        static public string RedisKeyPartySearch { get; set; }
        static public string RedisSetKeyPartyNames { get; set; }
        static public string RedisHashWebUser { get; set; }
        static public string RedisHashPoliticalParty { get; set; }
        static public string RedisSetKeyPartyAllMembers { get; set; }
        static public string RedisKeyPartyAllMembers { get; set; }
        static public string RedisKeyPoliticalAgenda { get; set; }
        static public string RedisKeyParty { get; set; }
        static public string RedisKeySlotMachineList { get; set; }
        static public string RedisKeyPick3WinNumbers { get; set; }
        static public string RedisKeyPick5WinNumbers { get; set; }
        static public string RedisKeyLotteryNextDrawing { get; set; }
        static public string RedisKeyPoliticalPositions { get; set; }
        static public string RedisKeyElectionTerm { get; set; }
        static public string RedisKeyElectionLast12 { get; set; }
        static public string RedisKeyElectionCandidateSearch { get; set; }
        static public string RedisKeyElectionVoteCandidate { get; set; }
        static public string RedisKeyRichestSummaryTop10 { get; set; }
        static public string RedisKeyMetalPrices { get; set; }
        static public string RedisKeyFundTypes { get; set; }
        static public string RedisHashUserProfile { get; set; }
        static public string RedisHashCountryProfile { get; set; }
        static public string RedisSortedSetCountryPopulation { get; set; }
        static public string RedisSortedSetCountryBudget { get; set; }
        static public string RedisKeyCountryAsset { get; set; }
        static public string RedisHashCountryBudget { get; set; }
        static public string RedisSortedSetCountryCitizenWealth { get; set; }
        static public string RedisSortedSetCountryLiteracy { get; set; }
        static public string RedisSortedSetCountrySalary { get; set; }
        static public string RedisSortedSetCountrySafest { get; set; }
        static public string RedisSortedSetCountryDefenseAsset { get; set; }
        static public string RedisSortedSetCountryOffenseAsset { get; set; }
        static public string RedisHashIdEmail { get; set; }
        static public string RedisKeyContactSource { get; set; }
        static public string RedisKeyJobCode { get; set; }
        static public string RedisSetIndexWebUser { get; set; }
        static public string RedisHashIndexWebUser { get; set; }
        static public string RedisHashFirstLogin { get; set; }
        static public string RedisHashNotificationType { get; set; }
        static public string RedisKeyStockHistory { get; set; }
        static public string RedisKeyStockForecast { get; set; }
        static public string RedisHashMerchadiseType { get; set; }
        static public string RedisKeyCrimeIncident { get; set; }
        static public string RedisKeyCrimeUserReport { get; set; }






        static public string SPGetMyThreePicks { get; set; }
        static public string SPGetMyFivePicks { get; set; }
        static public string SPDeleteReadNotification { get; set; }
        static public string SPGetNewUserNotificationList { get; set; }
        static public string SPGetOldUserNotificationList { get; set; }
        static public string SPDegreeCodesList { get; set; }
        static public string SPMajorCodesList { get; set; }
        static public string SPPoliticalPostionList { get; set; }
        static public string SPElectionAgendaList { get; set; }
        static public string SPJobCodesList { get; set; }
        static public string SPFriendsList { get; set; }
        static public string SPUserPartyList { get; set; }
        static public string SPPartyMemberList { get; set; }
        static public string SPMyPartyList { get; set; }
        static public string SPMyPartyDetails { get; set; }
        static public string SPGetActiveLeaders { get; set; }
        static public string SPPartiesList { get; set; }
        static public string SPUserTaskList { get; set; }
        static public string SPGetRandomWebUsers { get; set; }
        static public string SPGetBudgetByCountry { get; set; }
        static public string SPGetBudgetTypeList { get; set; }
        static public string SPGetCountryBudgetByType { get; set; }
        static public string SPGetWeaponListByCountry { get; set; }
        static public string SPWeaponCodesList { get; set; }
        static public string SPgetWeaponSummary { get; set; }
        static public string SPMerchandiseCodesList { get; set; }
        static public string SPGetMerchandiseListByUser { get; set; }
        static public string SPGetCountMerchandiseByQty { get; set; }
        static public string SPgetMerchandiseSummary { get; set; }
        static public string SPHasThisMerchandise { get; set; }
        static public string SPGetTopNPropertyOwner { get; set; }
        static public string SPGetTopNWeaponStackCountry { get; set; }
        static public string SPGetAdsTypeList { get; set; }
        static public string SPGetVotingDetailsByTaskId { get; set; }
        static public string SPGetPendingWarRequest { get; set; }
        static public string SPGetAdsFrequencyTypeList { get; set; }
        static public string SPGetPostForUserWithLimit { get; set; }
        static public string SPGetNewUserPostWithLimit { get; set; }
        static public string SPGetCommentList { get; set; }
        static public string SPGetCountryCodeList { get; set; }
        static public string SPGetCommentsForPosts { get; set; }
        static public string SPGetChildCommentsForParent { get; set; }
        static public string SPUpdatePostCommentCount { get; set; }
        static public string SPUpdateUserDigAdd { get; set; }
        static public string SPUpdateUserDigMinus { get; set; }
        static public string SPUpdateCommentCount { get; set; }
        static public string SPUpdateTaskStatus { get; set; }
        static public string SPGetVoteChoiceByTaskType { get; set; }
        static public string SPGetLoanListByUserId { get; set; }
        static public string SPExecuteLoanPayment { get; set; }
        static public string SPApproveLoanRequest { get; set; }
        static public string SPUpdateLoanStatus { get; set; }
        static public string SPGetQuailfiedIntrestedRate { get; set; }
        static public string SPGetLoanByTaskId { get; set; }
        static public string SPGetCurrentCountryTax { get; set; }
        static public string SPGetTaxBracketByTaskId { get; set; }
        static public string SPGetCountryTaxTypeById { get; set; }
        static public string SPUpdateNewCountryTax { get; set; }
        static public string SPGetCountByChoiceForTask { get; set; }
        static public string SPGetMerchandiseTotal { get; set; }
        static public string SPGetAllMerchandiseTypeList { get; set; }
        static public string SPGetCurrentStock { get; set; }
        static public string SPGetStockTradeByUser { get; set; }
        static public string SPGetTopNStockOwner { get; set; }
        static public string SPGetStockSummary { get; set; }
        static public string SPGetStockByUser { get; set; }
        static public string SPGetCountStockByQty { get; set; }
        static public string SPTryCancelStockOrder { get; set; }
        static public string SPHasThisStock { get; set; }
        static public string SPHasThisStockonPendingTrade { get; set; }
        static public string SPExecuteTradeOrder { get; set; }
        static public string SPGetPendingStockTrade { get; set; }
        static public string SPUpdateNewCountryBudget { get; set; }
        static public string SPGetBudgetBracketByTaskId { get; set; }
        static public string SPGetEducationByUserId { get; set; }
        static public string SPGetTopNDegreeHolder { get; set; }
        static public string SPGetEducationSummary { get; set; }
        static public string SPExecuteUpdateUserBankAc { get; set; }
        static public string SPSearchJob { get; set; }
        static public string SPGetCurrentJobs { get; set; }
        static public string SPQuitJob { get; set; }
        static public string SPGetJobHistory { get; set; }
        static public string SPGetTopNIncomeSalary { get; set; }
        static public string SPGetJobSummary { get; set; }
        static public string SPGetUserPartyInviteSummary { get; set; }
        static public string SPGetUserPartyMemberSummary { get; set; }
        static public string SPGetJobSummaryTotal { get; set; }
        static public string SPGetUserJob { get; set; }
        static public string SPCurrentOrAppliedJob { get; set; }
        static public string SPGetCurrentJobsTotalHPW { get; set; }
        static public string SPHasPendingOrOpenOfferForSameJob { get; set; }
        static public string SPGetIndustryCodeList { get; set; }
        static public string SPGetJobCodeAndSalary { get; set; }
        static public string SPWithDrawJob { get; set; }
        static public string SPSearchParty { get; set; }
        static public string SPGetCurrentParty { get; set; }
        static public string SPGetPastParty { get; set; }
        static public string SPGetActiveUserParty { get; set; }
        static public string SPGetPartyByMemberType { get; set; }
        static public string SPEjectPartyMember { get; set; }
        static public string SPExecuteDonateParty { get; set; }
        static public string SPExecuteJoinPartyFee { get; set; }
        static public string SPGetUserPartySummary { get; set; }
        static public string SPGetTopNPartyByMember { get; set; }
        static public string SPCloseParty { get; set; }
        static public string SPGetEmailInvitationList { get; set; }
        static public string SPGetPartyAgenda { get; set; }
        static public string SPGetAllPoliticalAgenda { get; set; }
        static public string SPHasPendingJoinRequest { get; set; }
        static public string SPHasPendingPartyInvite { get; set; }
        static public string SPHasPendingPartyInviteByEmail { get; set; }
        static public string SPGetPartyMember { get; set; }
        static public string SPGetPartyNames { get; set; }
        static public string SPGetWebUserDTO { get; set; }
        static public string SPGetAllPartyMember { get; set; }
        static public string SPGetAllPartyMemberWithNames { get; set; }
        static public string SPGetCountryPopulation { get; set; }
        static public string SPPostContentTypeList { get; set; }
        static public string SPNotificationTypeList { get; set; }
        static public string SPTaskTypeList { get; set; }
        static public string SPGetNextLotteryDrawingDate { get; set; }
        static public string SPGetPickFiveWinNumber { get; set; }
        static public string SPGetPickThreeWinNumber { get; set; }
        static public string SPExecutePick3LotteryOrder { get; set; }
        static public string SPExecutePick5LotteryOrder { get; set; }
        static public string SPGetMatchLottoPick5 { get; set; }
        static public string SPGetMatchLottoPick3 { get; set; }

        static public string SPGetPendingNomination { get; set; }
        static public string SPGetWebUserCache { get; set; }
        static public string SPDeleteTaskNotComplete { get; set; }
        static public string SPUpdateMemberNomination { get; set; }
        static public string SPCheckIncompleteTask { get; set; }
        static public string GetTaskCountById { get; set; }
        static public string SPUpdatePartyJoinRequest { get; set; }
        static public string SPIsCurrentOrPastParty { get; set; }
        static public string SPGetUserIdByEmail { get; set; }
        static public string SPUpdatePartyStatus { get; set; }
        static public string SPUpdatePartySizeStatus { get; set; }
        static public string SPUpdateManageParty { get; set; }
        static public string SPUpdateManagePartyLogo { get; set; }
        static public string SPUpdateClosePartyStatus { get; set; }
        static public string SPExecutePartyPayment { get; set; }
        static public string SPDeleteAgenda { get; set; }
        static public string SPGetCurrentElectionPeriod { get; set; }
        static public string SPGetPendingOrApprovedElectionApp { get; set; }
        static public string SPNumberofApprovedPartyMember { get; set; }
        static public string SPNumberOfApprovedCandidate { get; set; }
        static public string SPGetConsecutiveTerm { get; set; }
        static public string SPExecutePayNation { get; set; }
        static public string SPUpdateCandidateStatus { get; set; }
        static public string SPDeleteTaskElectionCap { get; set; }
        static public string SPGetRunForOfficeTicket { get; set; }
        static public string SPGetCandidateAgenda { get; set; }
        static public string SPGetElectionLast12 { get; set; }
        static public string SPGetCandidateByElection { get; set; }
        static public string SPExecutePayWithTax { get; set; }
        static public string SPQuitElection { get; set; }
        static public string SPGetVoteResultByElection { get; set; }
        static public string SPGetElectionCandidate { get; set; }
        static public string SPGetCurrentVotingInfo { get; set; }
        static public string SPGetElectionCandidateCountry { get; set; }
        static public string SPExecuteUpdateVotingScore { get; set; }
        static public string SPDeleteStockTrade { get; set; }
        static public string SPGetTodaysStockPrice { get; set; }
        static public string SPGetFinanceContent { get; set; }
        static public string SPExecuteBuySellGoldAndSilver { get; set; }
        static public string SPGetCapitalTransactionLogById { get; set; }
        static public string SPGetAllFundType { get; set; }
        static public string SPGetAllCapitalType { get; set; }
        static public string SPGetTopNRichest { get; set; }
        static public string SPGetProfileStat { get; set; }
        static public string SPGetAllUserParty { get; set; }
        static public string SPGetJobProfile { get; set; }
        static public string SPGetMerchandiseProfile { get; set; }
        static public string SPGetEducationProfile { get; set; }
        static public string SPGetCountryPopulationRank { get; set; }
        static public string SPGetCountryBudgetRank { get; set; }
        static public string SPGetWeaponStatByCountry { get; set; }
        static public string SPGetRevenueByCountry { get; set; }
        static public string SPGetCountryOffenseAssetRank { get; set; }
        static public string SPGetCountryDefenseAssetRank { get; set; }
        static public string SPGetCountrySafestRank { get; set; }
        static public string SPGetCountryAvgJob { get; set; }
        static public string SPGetCountryLiteracyScore { get; set; }
        static public string SPGetCountryCitizenWealthLevel { get; set; }
        static public string SPGetSecurityProfile { get; set; }
        static public string SPGetActiveLeadersProfile { get; set; }
        static public string SPUpdateWebUserContactUserId { get; set; }
        static public string SPUpdateWebUserContactSendInvite { get; set; }
        static public string SPWebUserContactInviteAddUpdate { get; set; }
        static public string SPGetAllContactProvider { get; set; }
        static public string SPGetContactSource { get; set; }
        static public string SPExecutePayWithTaxBank { get; set; }
        static public string SPDegreeCheck { get; set; }
        static public string SPProcessUserWithRentalProperty { get; set; }
        static public string SPProcessUserWithoutHouse { get; set; }
        static public string SPGetLastWebJobRunTime { get; set; }
        static public string SPExecutePayMe { get; set; }
        static public string SPUserChangingLevel { get; set; }
        static public string SPApplyCreidtScore { get; set; }
        static public string SPMerchandiseCondition { get; set; }
        static public string SPWeaponCondition { get; set; }
        static public string SPBudgetStimulator { get; set; }
        static public string SPBudgetWarStimulator { get; set; }
        static public string SPBudgetPopulationStimulator { get; set; }
        static public string SPGetAllApplyJobCodeByCountry { get; set; }
        static public string SPGetJobMatchScore { get; set; }
        static public string SPSendBulkNotificationsAndPost { get; set; }
        static public string SPUpdateUserJobStatus { get; set; }
        static public string SPGetWebUserList { get; set; }
        static public string SPGetWebUserIndexList { get; set; }
        static public string SPSendBulkTaskAndReminder { get; set; }
        static public string SPSendBulkTaskListAndReminder { get; set; }
        static public string SPGetIncompletePastDueTask { get; set; }
        static public string SPGiveEducationCreditForCountry { get; set; }
        static public string SPReCalculateBudget { get; set; }
        static public string SPCancelStockOrderForBudget { get; set; }
        static public string SPBuyStockOrderForBudget { get; set; }
        static public string SPIncreaseSalaryBudget { get; set; }
        static public string SPIncreaseLeadersSalary { get; set; }
        static public string SPIncreaseArmyJob { get; set; }
        static public string SPSendBudgetImpNotify { get; set; }
        static public string SPUpdateProfileName { get; set; }
        static public string SPUpdateProfilePic { get; set; }
        static public string SPUnFollowFriend { get; set; }
        static public string SPBlockFollower { get; set; }
        static public string SPGetWebUserIndexDTO { get; set; }
        static public string SPFollowAllFriend { get; set; }
        static public string SPAddFriendSuggestionByEmail { get; set; }
        static public string SPAddFriendOfMyFriendSuggestion { get; set; }
        static public string SPGetFriendSuggestion { get; set; }
        static public string SPRemoveFriendSuggestion { get; set; }
        static public string SPIgnoreFriendSuggestion { get; set; }
        static public string SPUpdateMutalFriend { get; set; }
        static public string SPDeleteAllNotification { get; set; }
        static public string SPIsThisFirstLogin { get; set; }
        static public string SPSaveUserActivityLog { get; set; }
        static public string SPGetOfflineNotificationById { get; set; }
        static public string SPGetEmailByUserId { get; set; }
        static public string SPGetPendingLoanPayment { get; set; }
        static public string SPGetInvitationSender { get; set; }
        static public string SPPayStockDividend { get; set; }
        static public string SPRunPayRoll { get; set; }
        static public string SPUpdateUserJobAfterPayRoll { get; set; }
        static public string SPUpdateEmailSentStatus { get; set; }
        static public string SPUpdateEmailSentByTime { get; set; }
        static public string SPGetNewNotificationThatNeedsToBeEmailed { get; set; }
        static public string SPGetSlotMachineThreeList { get; set; }
        static public string SPSendStcokForecastNotification { get; set; }
        static public string SPGetAllCountryApplyingJob { get; set; }
        static public string SPExecuteStealProperty { get; set; }
        static public string SPExecutePickPocket { get; set; }
        static public string SPReportSuspectToAuthority { get; set; }
        static public string SPNotRobbedInLastNDayByUser { get; set; }
        static public string SPCrimeWatchWantedJob { get; set; }
        static public string SPIsMyFriend { get; set; }
        static public string SPGetCrimeReportByIncident { get; set; }
        static public string SPGetCrimeReportByUser { get; set; }
        static public string SPArrestUser { get; set; }
        static public string SPGetAllUserInJail { get; set; }
        static public string SPGetUserThatHasLowSocialAsset { get; set; }
        static public string SPGetUserWithExpiringJobs { get; set; }
        static public string SPUpdateExpireJobEmailSent { get; set; }
        static public string SPExecuteElectionVoteCounting { get; set; }
        static public string SPGetElectionCandidateCount { get; set; }
        static public string SPGetLastNoVoteCountedElectionPeriod { get; set; }
        static public string SPAddNextElectionTerm { get; set; }
        static public string SPNotifyLastDayOfVotingPeroid { get; set; }
        static public string SPNotifyStartOfVotingPeroid { get; set; }
        static public string SPNotifyStartOfElectionPeroid { get; set; }
























        static public int PostLimit { get; set; }
        static public int CommentLimit { get; set; }
        static public int TaskLimit { get; set; }
        static public int LoanLimit { get; set; }
        static public int NotificationLimit { get; set; }
        static public int LotteryWinningNumberLimit { get; set; }
        static public int EmailInvitationLimit { get; set; }
        static public int ElectionCandidateLimit { get; set; }
        static public int AutoCompleteWebUserSearchLimit { get; set; }
        static public int FriendSuggestionLimit { get; set; }


        static public int BankId { get; set; }
        static public int OutLawId { get; set; }


        static public int MaxCountryId { get; set; }

        static public double NextBoostTime { get; set; }
        static public double BoostTime { get; set; }
        static public int InitialPartySize { get; set; }
        static public double PartyCofounderSize { get; set; }
        static public double PartyCofounderSizeMaxPercent { get; set; }



        static public int TaxApprovalChoiceId { get; set; }
        static public int TaxDenialChoiceId { get; set; }
        static public int BudgetApprovalChoiceId { get; set; }
        static public int BudgetDenialChoiceId { get; set; }
        static public int PartyInviteAcceptChoiceId { get; set; }
        static public int PartyInviteRejectChoiceId { get; set; }
        static public double BudgetAmedApprovalVoteNeeded { get; set; }
        static public double TaxAmedApprovalVoteNeeded { get; set; }
        static public double WarKeyApprovalVoteNeeded { get; set; }
        static public double JoinPartyRequestApprovalVoteNeeded { get; set; }
        static public double PartyNominationFounderApprovalVoteNeeded { get; set; }
        static public double PartyNominationCoFounderApprovalVoteNeeded { get; set; }
        static public double PartyNominationMemberApprovalVoteNeeded { get; set; }
        static public double PartyEjectionFounderApprovalVoteNeeded { get; set; }
        static public double PartyEjectionCoFounderApprovalVoteNeeded { get; set; }
        static public double PartyEjectionMemberApprovalVoteNeeded { get; set; }
        static public double PartyCloseApprovalVoteNeeded { get; set; }
        static public double ElectionRunForOfficeIndividualApprovalVoteNeeded { get; set; }
        static public double ElectionRunForOfficePartyApprovalVoteNeeded { get; set; }


        static public sbyte FounderVoteScore { get; set; }
        static public sbyte CoFounderVoteScore { get; set; }



        static public int WarKeyApprovalChoiceId { get; set; }
        static public int WarKeyDenialChoiceId { get; set; }
        static public int PartyCloseApprovalChoiceId { get; set; }
        static public int PartyCloseDenialChoiceId { get; set; }
        static public int UserLoanApprovalChoiceId { get; set; }
        static public int UserLoanDenialChoiceId { get; set; }
        static public int JoinPartyRequestApprovalChoiceId { get; set; }
        static public int PartyEjectionApprovalChoiceId { get; set; }
        static public int PartyEjectionDenialChoiceId { get; set; }
        static public int PartyNominationElectionApprovalChoiceId { get; set; }
        static public int PartyNominationElectionDenialChoiceId { get; set; }
        static public int JoinPartyRequestDenialChoiceId { get; set; }
        static public int JoinPartyRequestInviteAcceptChoiceId { get; set; }
        static public int JoinPartyRequestInviteRejectChoiceId { get; set; }
        static public int NotifyNominationRequestDenialChoiceId { get; set; }
        static public int NotifyNominationRequestApprovalChoiceId { get; set; }
        static public int RunForOfficePartyApprovalChoiceId { get; set; }
        static public int RunForOfficePartyDenialChoiceId { get; set; }
        static public int RunForOfficeIndividualApprovalChoiceId { get; set; }
        static public int RunForOfficeIndividualDenialChoiceId { get; set; }
        static public int JobOfferAcceptChoiceId { get; set; }
        static public int JobOfferRejectChoiceId { get; set; }


        static public short WarTaskType { get; set; }
        static public short TaxTaskType { get; set; }
        static public short JoinPartyTaskType { get; set; }
        static public short EjectPartyTaskType { get; set; }
        static public short BudgetTaskType { get; set; }
        static public short LoanRequestTaskType { get; set; }
        static public short JobTaskType { get; set; }
        static public short ClosePartyTaskType { get; set; }
        static public short NominationPartyTaskType { get; set; }
        static public short JoinPartyRequestTaskType { get; set; }
        static public short JoinPartyRequestInviteTaskType { get; set; }
        static public short NominationNotifyPartyTaskType { get; set; }
        static public short RunForOfficeAsPartyTaskType { get; set; }
        static public short RunForOfficeAsIndividualTaskType { get; set; }


        static public string PartyMemberStatusNomination { get; set; }
        static public string PartyMemberStatusEjection { get; set; }


        static public byte TaxGiftCode { get; set; }
        static public byte TaxIncomeCode { get; set; }
        static public byte TaxStockCode { get; set; }
        static public int TaxMerchandiseCode { get; set; }
        static public int TaxEducationCode { get; set; }
        static public int TaxElectionCode { get; set; }
        static public int TaxDonationCode { get; set; }
        static public int TaxAdsCode { get; set; }
        static public int TaxLotteryCode { get; set; }
        static public int TaxBudgetSimulartorCode { get; set; }
        static public int TaxWarVictoryCode { get; set; }
        static public int TaxWarLostCode { get; set; }
        static public int TaxStockDividendCode { get; set; }





        static public sbyte GoogleProviderId { get; set; }
        static public sbyte YahooProviderId { get; set; }
        static public sbyte MicroSoftProviderId { get; set; }



        static public byte GoldStockType { get; set; }
        static public byte SilverStockType { get; set; }

        static public int PartyMemberLimit { get; set; }
        static public int InitialCommentLimit { get; set; }
        static public int MerchandiseCodeLimit { get; set; }


        static public int MerchandiseSummaryTop10CacheLimit { get; set; }
        static public int WeaponSummaryTop10CacheLimit { get; set; }
        static public int MerchandiseInventoryLimit { get; set; }
        static public int WeaponInventoryLimit { get; set; }
        static public int StockLimit { get; set; }
        static public int CapitalTrnLimit { get; set; }
        static public int JobHistoryLimit { get; set; }
        static public int WeaponSummaryCacheLimit { get; set; }
        static public int RichestSummaryTop10CacheLimit { get; set; }
        static public int StockSummaryTop10CacheLimit { get; set; }
        static public int EducationSummaryTop10CacheLimit { get; set; }
        static public int JobIncomeSummaryTop10CacheLimit { get; set; }
        static public int PartySummaryTop10CacheLimit { get; set; }
        static public int NextLotteryDrawCacheLimit { get; set; }
        static public int CurrentElectionResultCacheLimit { get; set; }
        static public int UserProfileCacheLimit { get; set; }
        static public int CountryProfileCacheLimit { get; set; }
        static public int CountryBudgetCacheLimit { get; set; }
        static public int StockHistoryDataCacheLimit { get; set; }
        static public int CrimeReportDataCacheLimit { get; set; }



        static public int SearchJobLimit { get; set; }
        static public int SearchPartyLimit { get; set; }

        static public int NationalBudgetType { get; set; }
        static public int InternalSecurityBudgetType { get; set; }
        static public int EducationBudgetType { get; set; }
        static public int LeaderSalaryBudgetType { get; set; }
        static public int JobsBudgetType { get; set; }
        static public int ArmyJobBudgetType { get; set; }
        static public int StockBudgetTypeStart { get; set; }
        static public int StockBudgetTypeEnd { get; set; }



        static public short SenatorPositionType { get; set; }
        static public short SenatorJobCodeId { get; set; }
        static public short ArmyJobCodeId { get; set; }


        static public string ReminderServiceRunTaskByTypeUrl { get; set; }
        static public string ProfilePicUrl { get; set; }

        static public sbyte LotteryFundType { get; set; }
        static public sbyte PartyDonationFundType { get; set; }
        static public sbyte PartyMembershipFeeFundType { get; set; }
        static public sbyte ElectionFeeFundType { get; set; }
        static public sbyte PartyCloseFundType { get; set; }
        static public sbyte ElectionDonationFundType { get; set; }
        static public sbyte AdsFundType { get; set; }
        static public sbyte StocksFundType { get; set; }
        static public sbyte MetalFundType { get; set; }
        static public sbyte PropertyFundType { get; set; }
        static public sbyte EducationFundType { get; set; }
        static public sbyte LoanFundType { get; set; }
        static public sbyte GiftFundType { get; set; }
        static public sbyte RentFundType { get; set; }
        static public sbyte SocialAssetFundType { get; set; }
        static public sbyte StockDividendFundType { get; set; }
        static public sbyte PayCheckFundType { get; set; }
        static public sbyte RobberyFundType { get; set; }




        static public short BuySellSuccessNotificationId { get; set; }
        static public short BuySellFailNotificationId { get; set; }
        static public short StockTradeSuccessNotificationId { get; set; }
        static public short StockTradeFailNotificationId { get; set; }
        static public short StockTradeCancelOrderNotificationId { get; set; }
        static public short StockTradeCancelFailOrderNotificationId { get; set; }

        static public short WeaponSuccessNotificationId { get; set; }
        static public short WeaponFailNotificationId { get; set; }
        static public short WarRequestSuccessNotificationId { get; set; }
        static public short WarRequestFailNotificationId { get; set; }
        static public short WarRequestVotingRequestNotificationId { get; set; }
        static public short WarRequestResultNotificationId { get; set; }
        static public short WarRequestVotingCountNotificationId { get; set; }
        static public short LoanRequestVotingResultNotificationId { get; set; }
        static public short LoanRequestSuccessNotificationId { get; set; }
        static public short LoanRequestFailNotificationId { get; set; }
        static public short LoanPaymentFailNotificationId { get; set; }
        static public short LoanPaymentSuccessNotificationId { get; set; }
        static public short LoanPaymentNotificationId { get; set; }
        static public short LoanRequestTaskNotificationId { get; set; }
        static public short TaxAmendSuccessNotificationId { get; set; }
        static public short TaxAmendFailNotificationId { get; set; }
        static public short TaxAmendVotingRequestNotificationId { get; set; }
        static public short TaxAmendResultNotificationId { get; set; }
        static public short TaxAmendVotingCountNotificationId { get; set; }
        static public short BudgetAmendSuccessNotificationId { get; set; }
        static public short BudgetAmendFailNotificationId { get; set; }
        static public short BudgetAmendVotingRequestNotificationId { get; set; }
        static public short BudgetAmendResultNotificationId { get; set; }
        static public short BudgetAmendVotingCountNotificationId { get; set; }
        static public short SendGiftSuccessNotificationId { get; set; }
        static public short SendGiftFailNotificationId { get; set; }
        static public short RecivedGiftFailNotificationId { get; set; }
        static public short EducationSuccessNotificationId { get; set; }
        static public short EducationFailNotificationId { get; set; }
        static public short EducationGraduationNotificationId;
        static public short JobApplicationSuccessNotificationId { get; set; }
        static public short JobApplicationFailNotificationId { get; set; }
        static public short JobOfferNotificationId { get; set; }
        static public short JobDeclinedNotificationId { get; set; }
        static public short JobNotAvailableNotificationId { get; set; }
        static public short JobOfferAccepetedSuccessNotificationId { get; set; }
        static public short JobOfferAccepetedFailedNotificationId { get; set; }
        static public short JobOfferRejectedSuccessNotificationId { get; set; }
        static public short JobOfferRejectedFailedNotificationId { get; set; }
        static public short JobTaskSuccessNotificationId { get; set; }
        static public short JobTaskFailNotificationId { get; set; }
        static public short JobQuitSuccessNotificationId { get; set; }
        static public short JobQuitFailNotificationId { get; set; }
        static public short JobWithDrawSuccessNotificationId { get; set; }
        static public short JobWithDrawFailNotificationId { get; set; }
        static public short PartyOpenSuccessNotificationId { get; set; }
        static public short PartyOpenFailNotificationId { get; set; }
        static public short PartyEjectSuccessNotificationId { get; set; }
        static public short PartyEjectFailNotificationId { get; set; }
        static public short PartyDonateSuccessNotificationId { get; set; }
        static public short PartyDonateFailNotificationId { get; set; }
        static public short PartyJoinRequestSuccessNotificationId { get; set; }
        static public short PartyJoinRequestFailNotificationId { get; set; }
        static public short PartyLeaveRequestSuccessNotificationId { get; set; }
        static public short PartyLeaveRequestFailNotificationId { get; set; }
        static public short PartyCloseRequestSuccessNotificationId { get; set; }
        static public short PartyCloseRequestFailNotificationId { get; set; }
        static public short PartyCloseVotingRequestNotificationId { get; set; }
        static public short PartyCloseResultNotificationId { get; set; }
        static public short PartyCloseVotingCountNotificationId { get; set; }
        static public short PartyEjectVotingRequestNotificationId { get; set; }
        static public short PartyEjectResultNotificationId { get; set; }
        static public short PartyEjectVotingCountNotificationId { get; set; }
        static public short PartyNominationVotingRequestNotificationId { get; set; }
        static public short PartyNominationResultNotificationId { get; set; }
        static public short PartyNominationVotingCountNotificationId { get; set; }
        static public short PartyJoinVotingRequestNotificationId { get; set; }
        static public short PartyNominationRequestSuccessNotificationId { get; set; }
        static public short PartyNominationRequestFailNotificationId { get; set; }
        static public short SecurityNotification { get; set; }
        static public short PartyApplyJoinRequestSuccessNotificationId { get; set; }
        static public short PartyApplyJoinRequestFailNotificationId { get; set; }
        static public short PartyApplyJoinVotingRequestNotificationId { get; set; }
        static public short PartyJoinRequestInviteNotificationId { get; set; }
        static public short PartyJoinRequestInviteRejectNotificationId { get; set; }
        static public short PartyWelcomeNotificationId { get; set; }
        static public short PartyNotWelcomeNotificationId { get; set; }
        static public short PartyNotifyNominationNotificationId { get; set; }
        static public short PartyNotifyDemotionNominationNotificationId { get; set; }
        static public short PartyNotifyCongratulationNominationNotificationId { get; set; }
        static public short PartyNotifyRejectNominationNotificationId { get; set; }
        static public short PartyInvitationNotifySuccessNotificationId { get; set; }
        static public short PartyInvitationNotifyFailNotificationId { get; set; }
        static public short PartyInvitationNotifyEmailFreindsSuccessNotificationId { get; set; }
        static public short PartyInvitationNotifyEmailFreindsFailNotificationId { get; set; }
        static public short PartyManageSuccessNotificationId { get; set; }
        static public short PartyManageFailNotificationId { get; set; }
        static public short RunForOfficeResultNotificationId { get; set; }
        static public short RunForOfficeVotingCountNotificationId { get; set; }
        static public short RunForOfficePartyVotingRequestNotificationId { get; set; }
        static public short RunForOfficeIndividualVotingRequestNotificationId { get; set; }
        static public short RunForOfficeSuccessNotificationId { get; set; }
        static public short RunForOfficeFailNotificationId { get; set; }
        static public short ElectionDonationSuccessNotificationId { get; set; }
        static public short ElectionDonationFailNotificationId { get; set; }
        static public short ElectionQuitSuccessNotificationId { get; set; }
        static public short ElectionQuitFailNotificationId { get; set; }
        static public short ElectionVotingSuccessNotificationId { get; set; }
        static public short ElectionVotingFailNotificationId { get; set; }
        static public short AdsSuccessNotificationId { get; set; }
        static public short AdsFailNotificationId { get; set; }
        static public short BuySellMetalSuccessNotificationId { get; set; }
        static public short BuySellMetalFailNotificationId { get; set; }
        static public short EducationCreditNotificationId { get; set; }




        static public short LotteryWinNotificationId { get; set; }
        static public short LotteryBuySuccessNotificationId { get; set; }
        static public short LotteryBuyFailNotificationId { get; set; }
        static public short StockBrokerTradeSuccessNotificationId { get; set; }
        static public short StockBrokerTradeFailNotificationId { get; set; }
        static public short RentalPaymentNotificationId { get; set; }
        static public short RentalCollectionNotificationId { get; set; }
        static public short ExperincePlusNotificationId { get; set; }
        static public short ProfileUpdateSuccessNotificationId { get; set; }
        static public short ProfileUpdateFailNotificationId { get; set; }
        static public short SoicalCircleSuccessNotificationId { get; set; }
        static public short SoicalCircleFailNotificationId { get; set; }
        static public short SoicalCircleBulkFollowSuccessNotificationId { get; set; }
        static public short SoicalCircleBulkFollowFailNotificationId { get; set; }
        static public short FoundNewSocialContactNotificationId { get; set; }
        static public short InviteSocialContactNotificationId { get; set; }
        static public short InviteAcceptedSocialContactNotificationId { get; set; }
        static public short StockDividenNotificationId { get; set; }
        static public short PayCheckNotificationId { get; set; }
        static public short SlotMachineWinNotificationId { get; set; }
        static public short RouleteWinNotificationId { get; set; }
        static public short StockForeCastNotificationId { get; set; }
        static public short CrimeAlertCashNotificationId { get; set; }
        static public short CrimeAlertPropertyNotificationId { get; set; }
        static public short JailTimeNotificationId { get; set; }
        static public short InJailNotificationId { get; set; }
        static public short CashSinperSuccessNotificationId { get; set; }
        static public short CashSinperFailNotificationId { get; set; }
        static public short PropertyRobberySuccessNotificationId { get; set; }
        static public short PropertyRobberyFailNotificationId { get; set; }
        static public short CrimeSuspectSuccessNotificationId { get; set; }
        static public short CrimeSuspectFailNotificationId { get; set; }
        static public short SuspectReportingNotificationId { get; set; }
        static public short TimeToFindNewJobNotificationId { get; set; }
        static public short ElectionVotingWinNotificationId { get; set; }
        static public short ElectionVotingLostNotificationId { get; set; }


        static public string UnexpectedErrorMsg { get; set; }



        static public sbyte RobberyPostContentTypeId { get; set; }
        static public sbyte PartyClosePostContentTypeId { get; set; }
        static public sbyte WelcomePartyPostContentTypeId { get; set; }
        static public sbyte LotteryWinPostContentTypeId { get; set; }

        static public sbyte PartyNominationPostContentTypeId { get; set; }
        static public sbyte PartyNominationElectionPostContentTypeId { get; set; }
        static public sbyte PartyEjectionPostContentTypeId { get; set; }
        static public sbyte PartyJoinRequestInvitationDumpedPostContentTypeId { get; set; }
        static public sbyte PartyDumpedPostContentTypeId { get; set; }
        static public sbyte PartyCloseRequestPostContentTypeId { get; set; }
        static public sbyte RunForOfficePartyPostContentTypeId { get; set; }
        static public sbyte RunForOfficeIndividualPostContentTypeId { get; set; }
        static public sbyte RunForOfficeApplicationResultIndividualPostContentTypeId { get; set; }
        static public sbyte RunForOfficeApplicationResultPartyPostContentTypeId { get; set; }
        static public sbyte PartyDonationContentTypeId { get; set; }
        static public sbyte ElectionDonationContentTypeId { get; set; }
        static public sbyte ElectionWithdrawContentTypeId { get; set; }
        static public sbyte GraduationPostContentTypeId { get; set; }
        static public sbyte RentalPostContentTypeId { get; set; }
        static public sbyte ExperienceLevelPostContentTypeId { get; set; }
        static public sbyte JobOfferPostContentTypeId { get; set; }
        static public sbyte EducationCreditContentTypeId { get; set; }
        static public sbyte BudgetImplentationContentTypeId { get; set; }
        static public sbyte StockForecastContentTypeId { get; set; }
        static public sbyte FriendRobberyContentTypeId { get; set; }
        static public sbyte JailTimeContentTypeId { get; set; }
        static public sbyte ElectionVictoryContentTypeId { get; set; }
        static public sbyte NotifyLastDayOfVotingPeroidContentTypeId { get; set; }
        static public sbyte NotifyStartOfVotingPeroidContentTypeId { get; set; }
        static public sbyte NotifyStartOfElectionPeroidContentTypeId { get; set; }


        static public string ConformEmailTemplate { get; set; }
        static public string ResetPasswordEmailTemplate { get; set; }
        static public string InviteEmailTemplate { get; set; }
        static public string OfflineNotficationEmailtemplate { get; set; }
        static public string AzureProfilePicUrl { get; set; }
        static public string SpoImRegisterUrl { get; set; }
        static public string YQLFinanceHistoryDataURL { get; set; }
        static public string SpoImAccessToken { get; set; }











        static AppSettings()
        {
            ConformEmailTemplate = Properties.Resources.emailconfirm;
            ResetPasswordEmailTemplate = Properties.Resources.passwordreset;
            InviteEmailTemplate = Properties.Resources.invite;
            OfflineNotficationEmailtemplate = Properties.Resources.offlinenotification;
            SpoImRegisterUrl = ConfigurationManager.AppSettings["spotIM:url"];
            YQLFinanceHistoryDataURL = ConfigurationManager.AppSettings["yql.finance.historydata"];
            SpoImAccessToken = ConfigurationManager.AppSettings["spotIM:accessToken"];
            AzureProfilePicUrl = ConfigurationManager.AppSettings["azure.profilepicURL"];
            OnlyAllowedUsers = Convert.ToBoolean(ConfigurationManager.AppSettings["login.onlyallowedusers"]);
            SendEmailInvite = Convert.ToBoolean(ConfigurationManager.AppSettings["sendemail.invite"]);
            RegisterInviteOnly = Convert.ToBoolean(ConfigurationManager.AppSettings["register.inviteonly"]);
            SendEmailNotfication = Convert.ToBoolean(ConfigurationManager.AppSettings["sendemail.notification"]);
            RedisServer = ConfigurationManager.AppSettings["redis.server"];
            RedisDatabaseId = Convert.ToInt32(ConfigurationManager.AppSettings["redis.database"]);
            RedisAutocompleteDatabaseId = 3;

            RedisPort = Convert.ToInt32(ConfigurationManager.AppSettings["redis.port"]);
            RedisSyncTimeOut = Convert.ToInt32(ConfigurationManager.AppSettings["redis.syncTimeout"]);
            RedisPassword = ConfigurationManager.AppSettings["redis.password"];
            RedisSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["redis.ssl"]);
            ImageUploadPath = ConfigurationManager.AppSettings["ImageUploadPath"];
            FileUploadPath = ConfigurationManager.AppSettings["FileUploadPath"];
            FileUploadPathThumb = ConfigurationManager.AppSettings["FileUploadPathThumb"];
            RedisKeyDegreeCodes = "DGC";
            RedisKeyElectionAgendas = "EAG";
            RedisKeyMajorCodes = "MJC";
            RedisKeyParties = "PL";
            RedisKeyActiveSeneator = "AS"; //+countryId
            RedisKeyActiveSeneatorCount = "ASC"; //+countryId
            RedisKeyAdsDTO = "ADC";
            RedisKeyWeaponCodes = "WC";
            RedisKeyWeaponSummary = "WS";
            RedisKeyMerchandiseCodes = "MC";
            RedisHashMerchandiseCodes = "HMC";
            RedisKeyMerchandiseSummary = "MS";
            RedisKeyMerchandiseSummaryTop10 = "MSTT";
            RedisKeyWeaponSummaryTop10 = "WSTT";
            RedisKeyCountryCodes = "CC";
            RedisHashCountryName = "CHH";
            RedisKeyCountryName = "CN";
            RedisKeyTaskType = "TT"; //+typeId
            RedisKeyVoteChoices = "VC"; //+taskType
            RedisKeyCurrentTax = "CT"; //+countryId
            RedisKeyCurrentTaxCode = "CTC"; //+countryId+taxCode
            RedisKeyStockCode = "SC"; //+stockId
            RedisKeyCurrentStockPrice = "CSP";
            RedisKeyStockSummaryTop10 = "STT";
            RedisKeyNotificationType = "NT";
            RedisKeyEducationSummaryTop10 = "ETT";
            RedisKeyJobIncomeSummaryTop10 = "JTT";
            RedisKeyPartyByMemberTop10 = "PTT";
            RedisKeyJobCodeSearch = "JCS";  //+countryId+criteriaId
            RedisKeyPartySearch = "PS"; //+countryId+criteriaId			
            RedisKeyIndustryCodes = "IC";
            RedisKeyPartyCoFounders = "PCF";//+partyId
            RedisKeyPartyFounders = "PF";//+partyId
            RedisKeyPartyMembers = "PM";//+partyId
            RedisKeyParty = "PK";
            RedisSetKeyPartyNames = "SPN";  //+countryId
            RedisHashWebUser = "WU"; //+UserId
            RedisHashPoliticalParty = "PP"; //+partyId
            RedisSetKeyPartyAllMembers = "SPM";//+parytId
            RedisKeyPartyAllMembers = "APM";//+parytId
            RedisKeyPoliticalAgenda = "PA";
            RedisKeySlotMachineList = "SMT";
            RedisKeyPick3WinNumbers = "LP3";
            RedisKeyPick5WinNumbers = "LP5";
            RedisKeyLotteryNextDrawing = "LND";
            RedisKeyPoliticalPositions = "EPP";
            RedisKeyElectionTerm = "CET";
            RedisKeyElectionLast12 = "EL12";
            RedisKeyElectionCandidateSearch = "ECS"; // + countryid , electionid and page
            RedisKeyElectionVoteCandidate = "ECV"; // + countyrid
            RedisKeyRichestSummaryTop10 = "RTT";
            RedisKeyMetalPrices = "CAT";
            RedisKeyFundTypes = "FT";
            RedisHashUserProfile = "HKP";
            RedisHashCountryProfile = "HKCP";// + countyrid
            RedisSortedSetCountryPopulation = "SSCP";
            RedisSortedSetCountryBudget = "SSCB";
            RedisKeyCountryAsset = "CAS"; // + countryId
            RedisHashCountryBudget = "HKCB";
            RedisSortedSetCountryCitizenWealth = "HKCW";
            RedisSortedSetCountryLiteracy = "HKCL";
            RedisSortedSetCountrySalary = "HKCS";
            RedisSortedSetCountrySafest = "HKCA";
            RedisSortedSetCountryDefenseAsset = "HKCD";
            RedisSortedSetCountryOffenseAsset = "HKCA";
            RedisHashIdEmail = "HKEI";
            RedisKeyContactSource = "CPS";
            RedisKeyJobCode = "JCI";
            RedisSetIndexWebUser = "IWU";
            RedisHashIndexWebUser = "HKWU";
            RedisHashFirstLogin = "HKFL";
            RedisHashNotificationType = "HKNT";
            RedisKeyStockHistory = "YSH";
            RedisKeyStockForecast = "SFC";
            RedisHashMerchadiseType = "HKMT";
            RedisKeyCrimeIncident = "CIR"; // + incidentId
            RedisKeyCrimeUserReport = "CUR"; // + userId


            SPGetMyThreePicks = "GetMyThreePicks";
            SPGetMyFivePicks = "GetMyFivePicks";
            SPDegreeCodesList = "GetDegreeCodeList";
            SPPoliticalPostionList = "GetPoliticalPostionsList";
            SPElectionAgendaList = "GetElectionAgendaList";
            SPMajorCodesList = "GetMajorCodeList";
            SPJobCodesList = "GetJobCodeList";
            SPUserPartyList = "GetUserPartyList";
            SPFriendsList = "GetFriendsInfo";
            SPPartyMemberList = "GetPartyMemberList";
            SPMyPartyList = "GetMyPartyList";
            SPMyPartyDetails = "GetMyPartyDetails";
            SPPartiesList = "GetPartyList";
            SPGetActiveLeaders = "GetActiveLeaders";
            SPUserTaskList = "GetUserTaskList";
            SPGetBudgetByCountry = "GetCurrentCountryBudget";
            SPGetBudgetTypeList = "GetCountryBudgetByIdList";
            SPGetAdsTypeList = "GetAdsTypeList";
            SPGetAdsFrequencyTypeList = "GetAdsFrequencyTypeList";
            SPGetPostForUserWithLimit = "GetPostForUserWithLimit";
            SPGetNewUserPostWithLimit = "GetNewUserPostWithLimit";
            SPGetCommentList = "GetCommentListByPostId";
            SPGetCommentsForPosts = "GetCommentsForPost";
            SPGetChildCommentsForParent = "GetCommentListByParentId";
            SPUpdatePostCommentCount = "UpdatePostCommentCount";
            SPUpdateUserDigAdd = "UpdateUserDigAdd";
            SPUpdateUserDigMinus = "UpdateUserDigMinus";
            SPUpdateCommentCount = "UpdateCommentCount";
            SPGetCountryBudgetByType = "GetCountryBudgetByType";
            SPGetWeaponListByCountry = "GetWeaponListByCountryId";
            SPgetWeaponSummary = "GetWeaponSummary";
            SPWeaponCodesList = "GetWeaponTypeList";
            SPGetMerchandiseListByUser = "GetMerchandiseListByUserId";
            SPHasThisMerchandise = "HasThisMerchandise";
            SPGetTopNPropertyOwner = "GetTopNPropertyOwner";
            SPGetTopNWeaponStackCountry = "GetTopNWeaponStackCountry";
            SPgetMerchandiseSummary = "GetMerchandiseSummary";
            SPGetCountMerchandiseByQty = "GetCountMerchandiseByQty";
            SPMerchandiseCodesList = "GetMerchandiseTypeList";
            SPGetPendingWarRequest = "GetPendingWarRequestByCountryId";
            SPGetCountryCodeList = "GetCountryCodes";
            SPGetVotingDetailsByTaskId = "GetVotingDetailsByTaskId";
            SPGetVoteChoiceByTaskType = "GetVoteChoiceByTaskType";
            SPUpdateTaskStatus = "UpdateTaskStatus";
            SPGetLoanListByUserId = "GetLoanListByUserId";
            SPExecuteLoanPayment = "ExecuteLoanPayment";
            SPApproveLoanRequest = "ApproveLoanRequest";
            SPUpdateLoanStatus = "UpdateLoanStatus";
            SPGetQuailfiedIntrestedRate = "GetQuailfiedIntrestedRate";
            SPGetLoanByTaskId = "GetLoanByTaskId";
            SPGetTaxBracketByTaskId = "GetTaxbracketByTaskId";
            SPGetCurrentCountryTax = "GetCurrentCountryTax";
            SPGetCountryTaxTypeById = "GetCountryTaxTypeById";
            SPUpdateNewCountryTax = "UpdateNewCountryTax";
            SPGetCountByChoiceForTask = "GetCountByChoiceForTask";
            SPGetMerchandiseTotal = "GetMerchandiseTotal";
            SPGetAllMerchandiseTypeList = "GetAllMerchandiseTypeList";
            SPGetCurrentStock = "GetCurrentStock";
            SPGetStockTradeByUser = "GetStockTradeByUser";
            SPGetStockByUser = "GetStockByUser";
            SPGetCountStockByQty = "GetCountStockByQty";
            SPTryCancelStockOrder = "TryCancelStockOrder";
            SPHasThisStock = "HasThisStock";
            SPHasThisStockonPendingTrade = "HasThisStockonPendingTrade";
            SPGetStockSummary = "GetStockSummary";
            SPGetTopNStockOwner = "GetTopNStockOwner";
            SPExecuteTradeOrder = "ExecuteTradeOrder";
            SPGetPendingStockTrade = "GetPendingStockTrade";
            SPGetNewUserNotificationList = "GetNewUserNotificationList";
            SPGetOldUserNotificationList = "GetOldUserNotificationList";
            SPDeleteReadNotification = "DeleteReadNotification";
            SPUpdateNewCountryBudget = "UpdateNewCountryBudget";
            SPGetBudgetBracketByTaskId = "GetBudgetBracketByTaskId";
            SPGetEducationByUserId = "GetEducationByUserId";
            SPGetEducationSummary = "GetEducationSummary";
            SPGetTopNDegreeHolder = "GetTopNDegreeHolder";
            SPExecuteUpdateUserBankAc = "ExecuteUpdateUserBankAc";
            SPSearchJob = "SearchJob";
            SPGetCurrentJobs = "GetCurrentJobs";
            SPQuitJob = "QuitJob";
            SPGetJobHistory = "GetJobHistory";
            SPGetTopNIncomeSalary = "GetTopNIncomeSalary";
            SPGetJobSummary = "GetJobSummary";
            SPGetUserPartyInviteSummary = "GetUserPartyInviteSummary";
            SPGetUserPartyMemberSummary = "GetUserPartyMemberSummary";
            SPGetJobSummaryTotal = "GetJobSummaryTotal";
            SPGetUserJob = "GetUserJob";
            SPCurrentOrAppliedJob = "CurrentOrAppliedJob";
            SPGetCurrentJobsTotalHPW = "GetCurrentJobsTotalHPW";
            SPHasPendingOrOpenOfferForSameJob = "HasPendingOrOpenOfferForSameJob";
            SPGetIndustryCodeList = "GetIndustryCodeList";
            SPGetJobCodeAndSalary = "GetJobCodeAndSalary";
            SPWithDrawJob = "WithDrawJob";
            SPSearchParty = "SearchParty";
            SPGetCurrentParty = "GetCurrentParty";
            SPGetPastParty = "GetPastParty";
            SPGetActiveUserParty = "GetActiveUserParty";
            SPGetPartyByMemberType = "GetPartyByMemberType";
            SPEjectPartyMember = "EjectPartyMember";
            SPExecuteDonateParty = "ExecuteDonateParty";
            SPExecuteJoinPartyFee = "ExecuteJoinPartyFee";
            SPGetUserPartySummary = "GetUserPartySummary";
            SPGetTopNPartyByMember = "GetTopNPartyByMember";
            SPCloseParty = "CloseParty";
            SPGetEmailInvitationList = "GetEmailInvitationList";
            SPGetPartyAgenda = "GetPartyAgenda";
            SPGetAllPoliticalAgenda = "GetAllPoliticalAgenda";
            SPGetPartyMember = "GetPartyMember";
            SPGetPartyNames = "GetPartyNames";
            SPGetCountryPopulation = "GetCountryPopulation";
            SPGetRandomWebUsers = "GetRandomWebUsers";
            SPPostContentTypeList = "PostContentTypeList";
            SPNotificationTypeList = "NotificationTypeList";
            SPTaskTypeList = "TaskTypeList";
            SPGetNextLotteryDrawingDate = "GetNextLotteryDrawingDate";
            SPGetWebUserDTO = "GetWebUserDTO";
            SPGetAllPartyMember = "GetAllPartyMember";
            SPGetAllPartyMemberWithNames = "GetAllPartyMemberWithNames";
            SPHasPendingJoinRequest = "HasPendingJoinRequest";
            SPHasPendingPartyInvite = "HasPendingPartyInvite";
            SPHasPendingPartyInviteByEmail = "HasPendingPartyInviteByEmail";
            SPGetPickFiveWinNumber = "GetPickFiveWinNumber";
            SPGetPickThreeWinNumber = "GetPickThreeWinNumber";
            SPExecutePick3LotteryOrder = "ExecutePick3LotteryOrder";
            SPExecutePick5LotteryOrder = "ExecutePick5LotteryOrder";
            SPGetMatchLottoPick5 = "GetMatchLottoPick5";
            SPGetMatchLottoPick3 = "GetMatchLottoPick3";
            SPGetPendingNomination = "GetPendingNomination";
            SPGetWebUserCache = "GetWebUserCache";
            SPDeleteTaskNotComplete = "DeleteTaskNotComplete";
            SPUpdateMemberNomination = "UpdateMemberNomination";
            SPCheckIncompleteTask = "CheckIncompleteTask";
            GetTaskCountById = "GetTaskCountById";
            SPUpdatePartyJoinRequest = "UpdatePartyJoinRequest";
            SPIsCurrentOrPastParty = "IsCurrentOrPastParty";
            SPGetUserIdByEmail = "GetUserIdByEmail";
            SPUpdatePartyStatus = "UpdatePartyStatus";
            SPUpdatePartySizeStatus = "UpdatePartySizeStatus";
            SPUpdateManageParty = "UpdateManageParty";
            SPUpdateManagePartyLogo = "UpdateManagePartyLogo";
            SPUpdateClosePartyStatus = "UpdateClosePartyStatus";
            SPExecutePartyPayment = "ExecutePartyPayment";
            SPDeleteAgenda = "DeleteAgenda";
            SPGetCurrentElectionPeriod = "GetCurrentElectionPeriod";
            SPGetPendingOrApprovedElectionApp = "GetPendingOrApprovedElectionApp";
            SPNumberofApprovedPartyMember = "NumberofApprovedPartyMember";
            SPNumberOfApprovedCandidate = "NumberOfApprovedCandidate";
            SPGetConsecutiveTerm = "GetConsecutiveTerm";
            SPExecutePayNation = "ExecutePayNation";
            SPUpdateCandidateStatus = "UpdateCandidateStatus";
            SPDeleteTaskElectionCap = "DeleteTaskElectionCap";
            SPGetRunForOfficeTicket = "GetRunForOfficeTicket";
            SPGetCandidateAgenda = "GetCandidateAgenda";
            SPGetElectionLast12 = "GetElectionLast12";
            SPGetCandidateByElection = "GetCandidateByElection";
            SPExecutePayWithTax = "ExecutePayWithTax";
            SPQuitElection = "QuitElection";
            SPGetVoteResultByElection = "GetVoteResultByElection";
            SPGetElectionCandidate = "GetElectionCandidate";
            SPGetCurrentVotingInfo = "GetCurrentVotingInfo";
            SPGetElectionCandidateCountry = "GetElectionCandidateCountry";
            SPExecuteUpdateVotingScore = "ExecuteUpdateVotingScore";
            SPDeleteStockTrade = "DeleteStockTrade";
            SPGetTodaysStockPrice = "GetTodaysStockPrice";
            SPGetFinanceContent = "GetFinanceContent";
            SPExecuteBuySellGoldAndSilver = "ExecuteBuySellGoldAndSilver";
            SPGetCapitalTransactionLogById = "GetCapitalTransactionLogById";
            SPGetAllFundType = "GetAllFundType";
            SPGetAllCapitalType = "GetAllCapitalType";
            SPGetTopNRichest = "GetTopNRichest";
            SPGetProfileStat = "GetProfileStat";
            SPGetAllUserParty = "GetAllUserParty";
            SPGetJobProfile = "GetJobProfile";
            SPGetMerchandiseProfile = "GetMerchandiseProfile";
            SPGetEducationProfile = "GetEducationProfile";
            SPGetCountryPopulationRank = "GetCountryPopulationRank";
            SPGetCountryBudgetRank = "GetCountryBudgetRank";
            SPGetWeaponStatByCountry = "GetWeaponStatByCountry";
            SPGetRevenueByCountry = "GetRevenueByCountry";
            SPGetCountryCitizenWealthLevel = "GetCountryCitizenWealthLevel";
            SPGetCountryLiteracyScore = "GetCountryLiteracyScore";
            SPGetCountryAvgJob = "GetCountryAvgJob";
            SPGetCountrySafestRank = "GetCountrySafestRank";
            SPGetCountryDefenseAssetRank = "GetCountryDefenseAssetRank";
            SPGetCountryOffenseAssetRank = "GetCountryOffenseAssetRank";
            SPGetSecurityProfile = "GetSecurityProfile";
            SPGetActiveLeadersProfile = "GetActiveLeadersProfile";
            SPUpdateWebUserContactUserId = "UpdateWebUserContactUserId";
            SPUpdateWebUserContactSendInvite = "UpdateWebUserContactSendInvite";
            SPWebUserContactInviteAddUpdate = "WebUserContactInviteAddUpdate";
            SPGetAllContactProvider = "GetAllContactProvider";
            SPGetContactSource = "GetContactSource";
            SPExecutePayWithTaxBank = "ExecutePayWithTaxBank";
            SPDegreeCheck = "DegreeCheck";
            SPProcessUserWithRentalProperty = "ProcessUserWithRentalProperty";
            SPProcessUserWithoutHouse = "ProcessUserWithoutHouse";
            SPGetLastWebJobRunTime = "GetLastWebJobRunTime";
            SPExecutePayMe = "ExecutePayMe";
            SPUserChangingLevel = "UserChangingLevel";
            SPApplyCreidtScore = "ApplyCreidtScore";
            SPMerchandiseCondition = "MerchandiseCondition";
            SPWeaponCondition = "WeaponCondition";
            SPBudgetStimulator = "BudgetStimulator";
            SPBudgetWarStimulator = "BudgetWarStimulator";
            SPBudgetPopulationStimulator = "BudgetPopulationStimulator";
            SPGetAllApplyJobCodeByCountry = "GetAllApplyJobCodeByCountry";
            SPGetJobMatchScore = "GetJobMatchScore";
            SPSendBulkNotificationsAndPost = "SendBulkNotificationsAndPost";
            SPUpdateUserJobStatus = "UpdateUserJobStatus";
            SPGetWebUserList = "GetWebUserList";
            SPGetWebUserIndexList = "GetWebUserIndexList";
            SPSendBulkTaskAndReminder = "SendBulkTaskAndReminder";
            SPSendBulkTaskListAndReminder = "SendBulkTaskListAndReminder";
            SPGetIncompletePastDueTask = "GetIncompletePastDueTask";
            SPGiveEducationCreditForCountry = "GiveEducationCreditForCountry";
            SPReCalculateBudget = "ReCalculateBudget";
            SPCancelStockOrderForBudget = "CancelStockOrderForBudget";
            SPBuyStockOrderForBudget = "BuyStockOrderForBudget";
            SPIncreaseSalaryBudget = "IncreaseSalaryBudget";
            SPIncreaseLeadersSalary = "IncreaseLeadersSalary";
            SPIncreaseArmyJob = "IncreaseArmyJob";
            SPSendBudgetImpNotify = "SendBudgetImpNotify";
            SPUpdateProfileName = "UpdateProfileName";
            SPUpdateProfilePic = "UpdateProfilePic";
            SPUnFollowFriend = "UnFollowFriend";
            SPBlockFollower = "BlockFollower";
            SPGetWebUserIndexDTO = "GetWebUserIndexDTO";
            SPFollowAllFriend = "FollowAllFriend";
            SPAddFriendSuggestionByEmail = "AddFriendSuggestionByEmail";
            SPAddFriendOfMyFriendSuggestion = "AddFriendOfMyFriendSuggestion";
            SPGetFriendSuggestion = "GetFriendSuggestion";
            SPRemoveFriendSuggestion = "RemoveFriendSuggestion";
            SPIgnoreFriendSuggestion = "IgnoreFriendSuggestion";
            SPUpdateMutalFriend = "UpdateMutalFriend";
            SPDeleteAllNotification = "DeleteAllNotification";
            SPIsThisFirstLogin = "IsThisFirstLogin";
            SPSaveUserActivityLog = "SaveUserActivityLog";
            SPGetOfflineNotificationById = "GetOfflineNotificationById";
            SPGetEmailByUserId = "GetEmailByUserId";
            SPGetPendingLoanPayment = "GetPendingLoanPayment";
            SPGetInvitationSender = "GetInvitationSender";
            SPPayStockDividend = "PayStockDividend";
            SPRunPayRoll = "RunPayRoll";
            SPUpdateUserJobAfterPayRoll = "UpdateUserJobAfterPayRoll";
            SPUpdateEmailSentStatus = "UpdateEmailSentStatus";
            SPUpdateEmailSentByTime = "UpdateEmailSentByTime";
            SPGetNewNotificationThatNeedsToBeEmailed = "GetNewNotificationThatNeedsToBeEmailed";
            SPGetSlotMachineThreeList = "GetSlotMachineThreeList";
            SPSendStcokForecastNotification = "SendStcokForecastNotification";
            SPGetAllCountryApplyingJob = "GetAllCountryApplyingJob";
            SPExecuteStealProperty = "ExecuteStealProperty";
            SPExecutePickPocket = "ExecutePickPocket";
            SPReportSuspectToAuthority = "ReportSuspectToAuthority";
            SPNotRobbedInLastNDayByUser = "NotRobbedInLastNDayByUser";
            SPCrimeWatchWantedJob = "CrimeWatchWantedJob";
            SPIsMyFriend = "IsMyFriend";
            SPGetCrimeReportByIncident = "GetCrimeReportByIncident";
            SPGetCrimeReportByUser = "GetCrimeReportByUser";
            SPArrestUser = "ArrestUser";
            SPGetAllUserInJail = "GetAllUserInJail";
            SPGetUserThatHasLowSocialAsset = "GetUserThatHasLowSocialAsset";
            SPGetUserWithExpiringJobs = "GetUserWithExpiringJobs";
            SPUpdateExpireJobEmailSent = "UpdateExpireJobEmailSent";
            SPExecuteElectionVoteCounting = "ExecuteElectionVoteCounting";
            SPGetElectionCandidateCount = "GetElectionCandidateCount";
            SPGetLastNoVoteCountedElectionPeriod = "GetLastNoVoteCountedElectionPeriod";
            SPAddNextElectionTerm = "AddNextElectionTerm";
            SPNotifyLastDayOfVotingPeroid = "NotifyLastDayOfVotingPeroid";
            SPNotifyStartOfVotingPeroid = "NotifyStartOfVotingPeroid";
            SPNotifyStartOfElectionPeroid = "NotifyStartOfElectionPeroid";

            //UserProfileUrl = "/Election/GetElections/";
            ReminderServiceRunTaskByTypeUrl = "https://web.planetgeni.com/ReminderService/ReminderService.svc/RunTaskReminderByType";
            PostLimit = 25;
            CommentLimit = 10;
            PartyMemberLimit = 10;
            InitialCommentLimit = 10;
            MerchandiseCodeLimit = 5;

            NationalBudgetType = 1;
            InternalSecurityBudgetType = 2;
            JobsBudgetType = 3;
            EducationBudgetType = 4;
            LeaderSalaryBudgetType = 5;
            ArmyJobBudgetType = 6;
            StockBudgetTypeStart = 7;
            StockBudgetTypeEnd = 17;

            WeaponSummaryCacheLimit = 6000; //6000sesc 
            MerchandiseSummaryTop10CacheLimit = 6000; //6000sesc 
            WeaponSummaryTop10CacheLimit = 6000; //6000sesc 
            WeaponInventoryLimit = 15;
            MerchandiseInventoryLimit = 15;

            SenatorPositionType = 1;
            SenatorJobCodeId = 3061;
            ArmyJobCodeId = 3062;


            StockSummaryTop10CacheLimit = 6000;
            RichestSummaryTop10CacheLimit = 6000;
            EducationSummaryTop10CacheLimit = 6000;
            JobIncomeSummaryTop10CacheLimit = 6000;
            PartySummaryTop10CacheLimit = 6000;
            NextLotteryDrawCacheLimit = 43200;
            CurrentElectionResultCacheLimit = 600;
            CountryProfileCacheLimit = 3600 * 12;
            CountryBudgetCacheLimit = 3600 * 12;
            UserProfileCacheLimit = 3600;
            StockHistoryDataCacheLimit = 86400;
            CrimeReportDataCacheLimit = 86400;


            LotteryFundType = 1;
            PartyDonationFundType = 2;
            PartyMembershipFeeFundType = 3;
            PartyCloseFundType = 4;
            ElectionFeeFundType = 5;
            ElectionDonationFundType = 6;
            AdsFundType = 7;
            StocksFundType = 8;
            MetalFundType = 9;
            PropertyFundType = 10;
            EducationFundType = 11;
            LoanFundType = 12;
            GiftFundType = 13;
            RentFundType = 14;
            SocialAssetFundType = 15;
            StockDividendFundType = 16;
            PayCheckFundType = 17;
            RobberyFundType = 18;


            GoldStockType = 1;
            SilverStockType = 2;

            NotificationLimit = 10;
            TaskLimit = 10;
            LoanLimit = 5;
            StockLimit = 10;
            CapitalTrnLimit = 15;
            JobHistoryLimit = 10;
            SearchJobLimit = 20;
            SearchPartyLimit = 20;
            LotteryWinningNumberLimit = 10;
            EmailInvitationLimit = 150;
            ElectionCandidateLimit = 25;
            AutoCompleteWebUserSearchLimit = 10;
            FriendSuggestionLimit = 25;


            TaxGiftCode = 1;
            TaxElectionCode = 2;
            TaxMerchandiseCode = 3;
            TaxEducationCode = 4;
            TaxStockCode = 5;
            TaxIncomeCode = 6;
            TaxDonationCode = 7;
            TaxAdsCode = 8;
            TaxLotteryCode = 9;
            TaxBudgetSimulartorCode = 10;
            TaxWarVictoryCode = 11;
            TaxWarLostCode = 12;
            TaxStockDividendCode = 13;

            GoogleProviderId = 1;
            YahooProviderId = 2;
            MicroSoftProviderId = 3;

            BankId = 10001;
            OutLawId = 10000;
            MaxCountryId = 10244;
            NextBoostTime = 0.5;
            BoostTime = -5;
            InitialPartySize = 5;
            PartyCofounderSize = 25;  //Party Cofounder can have max of 20% or 25 members which ever is higher .
            PartyCofounderSizeMaxPercent = .2;


            BudgetAmedApprovalVoteNeeded = 0.75;
            TaxAmedApprovalVoteNeeded = 0.25; //3/4 of the total senators
            WarKeyApprovalVoteNeeded = 0.75;
            JoinPartyRequestApprovalVoteNeeded = 0.15;
            PartyNominationFounderApprovalVoteNeeded = 0.5;
            PartyNominationCoFounderApprovalVoteNeeded = 0.25;
            PartyNominationMemberApprovalVoteNeeded = 0.05;
            PartyEjectionFounderApprovalVoteNeeded = 0.5;
            PartyEjectionCoFounderApprovalVoteNeeded = 0.4;
            PartyEjectionMemberApprovalVoteNeeded = 0.3;
            PartyCloseApprovalVoteNeeded = 0.85;
            ElectionRunForOfficeIndividualApprovalVoteNeeded = 10;
            ElectionRunForOfficePartyApprovalVoteNeeded = 0.10;


            FounderVoteScore = 15;
            CoFounderVoteScore = 5;
            PartyMemberStatusNomination = "N";
            PartyMemberStatusEjection = "E";


            WarTaskType = 1;
            LoanRequestTaskType = 2;
            TaxTaskType = 3;
            BudgetTaskType = 4;
            JobTaskType = 5;
            JoinPartyTaskType = 6;
            ClosePartyTaskType = 7;
            EjectPartyTaskType = 8;
            NominationPartyTaskType = 9;
            JoinPartyRequestTaskType = 10;
            JoinPartyRequestInviteTaskType = 11;
            NominationNotifyPartyTaskType = 12;
            RunForOfficeAsPartyTaskType = 13;
            RunForOfficeAsIndividualTaskType = 14;

            WarKeyApprovalChoiceId = 1;
            WarKeyDenialChoiceId = 2;
            UserLoanApprovalChoiceId = 3;
            UserLoanDenialChoiceId = 4;
            TaxApprovalChoiceId = 5;
            TaxDenialChoiceId = 6;
            BudgetApprovalChoiceId = 7;
            BudgetDenialChoiceId = 8;
            PartyInviteAcceptChoiceId = 9;
            PartyInviteRejectChoiceId = 10;
            PartyCloseApprovalChoiceId = 11;
            PartyCloseDenialChoiceId = 12;
            PartyEjectionApprovalChoiceId = 13;
            PartyEjectionDenialChoiceId = 14;
            PartyNominationElectionApprovalChoiceId = 15;
            PartyNominationElectionDenialChoiceId = 16;
            JoinPartyRequestApprovalChoiceId = 17;
            JoinPartyRequestDenialChoiceId = 18;
            JoinPartyRequestInviteAcceptChoiceId = 19;
            JoinPartyRequestInviteRejectChoiceId = 20;
            NotifyNominationRequestApprovalChoiceId = 21;
            NotifyNominationRequestDenialChoiceId = 22;
            RunForOfficePartyApprovalChoiceId = 23;
            RunForOfficePartyDenialChoiceId = 24;
            RunForOfficeIndividualApprovalChoiceId = 25;
            RunForOfficeIndividualDenialChoiceId = 26;
            JobOfferAcceptChoiceId = 27;
            JobOfferRejectChoiceId = 28;


            BuySellSuccessNotificationId = 1;
            BuySellFailNotificationId = 2;
            StockTradeSuccessNotificationId = 3;
            StockTradeFailNotificationId = 4;
            StockTradeCancelOrderNotificationId = 5;
            StockTradeCancelFailOrderNotificationId = 6;
            StockBrokerTradeSuccessNotificationId = 105;
            StockBrokerTradeFailNotificationId = 106;
            UnexpectedErrorMsg = "unexpected reasons, please try again";
            WeaponSuccessNotificationId = 7;
            WeaponFailNotificationId = 8;
            WarRequestSuccessNotificationId = 9;
            WarRequestFailNotificationId = 10;
            WarRequestVotingRequestNotificationId = 11;
            WarRequestResultNotificationId = 12;
            WarRequestVotingCountNotificationId = 13;
            LoanRequestVotingResultNotificationId = 14;
            LoanRequestSuccessNotificationId = 15;
            LoanRequestFailNotificationId = 16;
            LoanPaymentSuccessNotificationId = 17;
            LoanPaymentFailNotificationId = 18;
            LoanPaymentNotificationId = 19;
            LoanRequestTaskNotificationId = 20;
            TaxAmendSuccessNotificationId = 21;
            TaxAmendFailNotificationId = 22;
            TaxAmendVotingRequestNotificationId = 23;
            TaxAmendResultNotificationId = 24;
            TaxAmendVotingCountNotificationId = 25;
            BudgetAmendSuccessNotificationId = 26;
            BudgetAmendFailNotificationId = 27;
            BudgetAmendVotingRequestNotificationId = 28;
            BudgetAmendResultNotificationId = 29;
            BudgetAmendVotingCountNotificationId = 30;
            SendGiftSuccessNotificationId = 31;
            SendGiftFailNotificationId = 32;
            RecivedGiftFailNotificationId = 33;
            EducationSuccessNotificationId = 34;
            EducationFailNotificationId = 35;
            EducationGraduationNotificationId = 36;
            EducationCreditNotificationId = 118;
            JobApplicationSuccessNotificationId = 37;
            JobApplicationFailNotificationId = 38;
            JobOfferNotificationId = 39;
            JobDeclinedNotificationId = 112;
            JobNotAvailableNotificationId = 113;
            JobOfferAccepetedSuccessNotificationId = 114;
            JobOfferAccepetedFailedNotificationId = 115;
            JobOfferRejectedSuccessNotificationId = 116;
            JobOfferRejectedFailedNotificationId = 117;
            JobTaskSuccessNotificationId = 40;
            JobTaskFailNotificationId = 41;
            JobQuitSuccessNotificationId = 42;
            JobQuitFailNotificationId = 43;
            JobWithDrawSuccessNotificationId = 44;
            JobWithDrawFailNotificationId = 45;
            PartyOpenSuccessNotificationId = 46;
            PartyOpenFailNotificationId = 47;
            PartyEjectSuccessNotificationId = 48;
            PartyEjectFailNotificationId = 49;
            PartyDonateSuccessNotificationId = 50;
            PartyDonateFailNotificationId = 51;
            PartyJoinRequestSuccessNotificationId = 52;
            PartyJoinRequestFailNotificationId = 53;
            PartyLeaveRequestSuccessNotificationId = 54;
            PartyLeaveRequestFailNotificationId = 55;
            PartyCloseRequestSuccessNotificationId = 56;
            PartyCloseRequestFailNotificationId = 57;
            PartyCloseVotingRequestNotificationId = 58;
            PartyCloseResultNotificationId = 59;
            PartyCloseVotingCountNotificationId = 60;
            PartyEjectVotingRequestNotificationId = 61;
            PartyEjectResultNotificationId = 62;
            PartyEjectVotingCountNotificationId = 63;
            PartyNominationVotingRequestNotificationId = 64;
            PartyNominationResultNotificationId = 65;
            PartyNominationVotingCountNotificationId = 66;
            PartyJoinVotingRequestNotificationId = 67;
            PartyNominationRequestSuccessNotificationId = 68;
            PartyNominationRequestFailNotificationId = 69;
            SecurityNotification = 70;
            PartyApplyJoinRequestSuccessNotificationId = 71;
            PartyApplyJoinRequestFailNotificationId = 72;
            PartyApplyJoinVotingRequestNotificationId = 73;
            PartyJoinRequestInviteNotificationId = 74;
            PartyJoinRequestInviteRejectNotificationId = 75;
            PartyWelcomeNotificationId = 76;
            PartyNotWelcomeNotificationId = 77;
            LotteryWinNotificationId = 78;
            LotteryBuySuccessNotificationId = 79;
            LotteryBuyFailNotificationId = 80;
            PartyNotifyNominationNotificationId = 81;
            PartyNotifyDemotionNominationNotificationId = 82;
            PartyNotifyCongratulationNominationNotificationId = 83;
            PartyNotifyRejectNominationNotificationId = 84;
            PartyInvitationNotifySuccessNotificationId = 85;
            PartyInvitationNotifyFailNotificationId = 86;
            PartyInvitationNotifyEmailFreindsSuccessNotificationId = 87;
            PartyInvitationNotifyEmailFreindsFailNotificationId = 88;
            PartyManageSuccessNotificationId = 89;
            PartyManageFailNotificationId = 90;
            RunForOfficeSuccessNotificationId = 91;
            RunForOfficeFailNotificationId = 92;
            RunForOfficeIndividualVotingRequestNotificationId = 93;
            RunForOfficePartyVotingRequestNotificationId = 94;
            RunForOfficeResultNotificationId = 95;
            RunForOfficeVotingCountNotificationId = 96;
            ElectionDonationSuccessNotificationId = 97;
            ElectionDonationFailNotificationId = 98;
            ElectionQuitSuccessNotificationId = 99;
            ElectionQuitFailNotificationId = 100;
            ElectionVotingSuccessNotificationId = 101;
            ElectionVotingFailNotificationId = 102;
            AdsSuccessNotificationId = 103;
            AdsFailNotificationId = 104;
            BuySellMetalSuccessNotificationId = 107;
            BuySellMetalFailNotificationId = 108;
            RentalPaymentNotificationId = 109;
            RentalCollectionNotificationId = 110;
            ExperincePlusNotificationId = 111;
            ProfileUpdateSuccessNotificationId = 119;
            ProfileUpdateFailNotificationId = 120;
            SoicalCircleSuccessNotificationId = 121;
            SoicalCircleFailNotificationId = 122;
            SoicalCircleBulkFollowSuccessNotificationId = 123;
            SoicalCircleBulkFollowFailNotificationId = 124;
            FoundNewSocialContactNotificationId = 125;
            InviteSocialContactNotificationId = 126;
            InviteAcceptedSocialContactNotificationId = 127;
            StockDividenNotificationId = 128;
            PayCheckNotificationId = 129;
            SlotMachineWinNotificationId = 130;
            RouleteWinNotificationId = 131;
            StockForeCastNotificationId = 132;
            CrimeAlertCashNotificationId = 133;
            CrimeAlertPropertyNotificationId = 134;
            JailTimeNotificationId = 135;
            InJailNotificationId = 136;
            CashSinperSuccessNotificationId = 137;
            CashSinperFailNotificationId = 138;
            PropertyRobberySuccessNotificationId = 139;
            PropertyRobberyFailNotificationId = 140;
            CrimeSuspectSuccessNotificationId = 141;
            CrimeSuspectFailNotificationId = 142;
            SuspectReportingNotificationId = 143;
            TimeToFindNewJobNotificationId = 144;
            ElectionVotingWinNotificationId = 145;
            ElectionVotingLostNotificationId = 146;


            RobberyPostContentTypeId = 1;
            PartyClosePostContentTypeId = 2;
            WelcomePartyPostContentTypeId = 3;
            LotteryWinPostContentTypeId = 4;
            PartyNominationPostContentTypeId = 5;
            PartyNominationElectionPostContentTypeId = 6;
            PartyEjectionPostContentTypeId = 7;
            PartyJoinRequestInvitationDumpedPostContentTypeId = 8;
            PartyDonationContentTypeId = 9;
            PartyDumpedPostContentTypeId = 10;
            PartyCloseRequestPostContentTypeId = 11;
            RunForOfficePartyPostContentTypeId = 12;
            RunForOfficeIndividualPostContentTypeId = 13;
            RunForOfficeApplicationResultIndividualPostContentTypeId = 14;
            RunForOfficeApplicationResultPartyPostContentTypeId = 15;
            ElectionDonationContentTypeId = 16;
            ElectionWithdrawContentTypeId = 17;
            GraduationPostContentTypeId = 18;
            RentalPostContentTypeId = 19;
            ExperienceLevelPostContentTypeId = 20;
            JobOfferPostContentTypeId = 21;
            EducationCreditContentTypeId = 22;
            BudgetImplentationContentTypeId = 23;
            StockForecastContentTypeId = 24;
            FriendRobberyContentTypeId = 25;
            JailTimeContentTypeId = 26;
            ElectionVictoryContentTypeId = 27;
            NotifyLastDayOfVotingPeroidContentTypeId = 28;
            NotifyStartOfVotingPeroidContentTypeId = 29;
            NotifyStartOfElectionPeroidContentTypeId = 30;
        }

    }
}
