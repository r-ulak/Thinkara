SET SQL_SAFE_UPDATES =0;
Delete From WebUser;
Delete From Friend;
Delete From UserVoteSelectedChoice;
Delete From UserTask;
Delete From UserNotification;
Delete From TaskReminder;
Delete From Post;
Delete From UserBankAccount;
Delete From CapitalTransactionLog;
Delete From Advertisement;


LOAD DATA LOCAL INFILE '{0}UserBankAccount.tsv' INTO TABLE UserBankAccount FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}Friend.tsv' INTO TABLE Friend FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 