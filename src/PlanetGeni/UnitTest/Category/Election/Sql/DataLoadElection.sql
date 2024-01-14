SET SQL_SAFE_UPDATES =0;
Delete From WebUser;
Delete From Friend;
Delete From PartyMember;
Delete From UserVoteSelectedChoice;
Delete From PoliticalParty;
Delete From UserTask;
Delete From UserNotification;
Delete From TaskReminder;
Delete From Post;
Delete From UserBankAccount;
Delete From CapitalTransactionLog;
Delete From ElectionAgenda;
Delete From PartyAgenda;
Delete From Election;
Delete From ElectionCandidate;
Delete From CandidateAgenda;
Delete From ElectionVoting;
Delete From ElectionVoter;


LOAD DATA LOCAL INFILE '{0}ElectionVoter.tsv' INTO TABLE ElectionVoter FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}CandidateAgenda.tsv' INTO TABLE CandidateAgenda FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}ElectionVoting.tsv' INTO TABLE ElectionVoting FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}Election.tsv' INTO TABLE Election FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}ElectionCandidate.tsv' INTO TABLE ElectionCandidate FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}PartyAgenda.tsv' INTO TABLE PartyAgenda FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}ElectionAgenda.tsv' INTO TABLE ElectionAgenda FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}UserBankAccount.tsv' INTO TABLE UserBankAccount FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}Friend.tsv' INTO TABLE Friend FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}PartyMember.tsv' INTO TABLE PartyMember FIELDS TERMINATED BY '\t' IGNORE 1 LINES;

  LOAD DATA LOCAL INFILE '{0}PoliticalParty.tsv' INTO TABLE PoliticalParty FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
SET SQL_SAFE_UPDATES=0;
Update PartyMember 
SET MemberEndDate =null
where MemberEndDate=0;