SET SQL_SAFE_UPDATES =0;
Delete From WebUser;
Delete From Friend;
Delete From PartyMember;
Delete From UserVoteSelectedChoice;
Delete From PoliticalParty;
Delete From PartyInvite;
Delete From PartyNomination;
Delete From UserTask;
Delete From UserNotification;
Delete From TaskReminder;
Delete From PartyJoinRequest;
Delete From PartyEjection;
Delete From Post;
Delete From UserBankAccount;
Delete From CapitalTransactionLog;
Delete From WebUserContact;
Delete From PartyCloseRequest;
Delete From ElectionAgenda;
Delete From PartyAgenda;



LOAD DATA LOCAL INFILE '{0}PartyAgenda.tsv' INTO TABLE PartyAgenda FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}PartyCloseRequest.tsv' INTO TABLE PartyCloseRequest FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}ElectionAgenda.tsv' INTO TABLE ElectionAgenda FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}WebUserContact.tsv' INTO TABLE WebUserContact FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}UserBankAccount.tsv' INTO TABLE UserBankAccount FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}TaskReminder.tsv' INTO TABLE TaskReminder FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}Friend.tsv' INTO TABLE Friend FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}PartyMember.tsv' INTO TABLE PartyMember FIELDS TERMINATED BY '\t' IGNORE 1 LINES;

  LOAD DATA LOCAL INFILE '{0}UserVoteSelectedChoice.tsv' INTO TABLE UserVoteSelectedChoice FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
   LOAD DATA LOCAL INFILE '{0}PoliticalParty.tsv' INTO TABLE PoliticalParty FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
    LOAD DATA LOCAL INFILE '{0}PartyInvite.tsv' INTO TABLE PartyInvite FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
	 LOAD DATA LOCAL INFILE '{0}PartyNomination.tsv' INTO TABLE PartyNomination FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
	 LOAD DATA LOCAL INFILE '{0}UserTask.tsv' INTO TABLE UserTask FIELDS TERMINATED BY '\t' IGNORE 1 LINES;	 
	LOAD DATA LOCAL INFILE '{0}UserNotification.tsv' INTO
	TABLE UserNotification FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
		LOAD DATA LOCAL INFILE '{0}PartyEjection.tsv' INTO TABLE PartyEjection FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
SET SQL_SAFE_UPDATES=0;
Update PartyMember 
SET MemberEndDate =null
where MemberEndDate=0;