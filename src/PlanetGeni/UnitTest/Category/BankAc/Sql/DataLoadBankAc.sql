SET SQL_SAFE_UPDATES =0;
Delete From WebUser;
Delete From UserNotification;
Delete From TaskReminder;
Delete From Post;
Delete From UserBankAccount;
Delete From CapitalTransactionLog;
Delete From UserLoan;
Delete From UserStock;
Delete From Friend;
Delete From CapitalTransactionLog;


LOAD DATA LOCAL INFILE '{0}UserBankAccount.tsv' INTO TABLE UserBankAccount FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}UserMerchandise.tsv' INTO TABLE UserMerchandise FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}UserLoan.tsv' INTO TABLE UserLoan FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}UserStock.tsv' INTO TABLE UserStock FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}Friend.tsv' INTO TABLE Friend FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}CapitalTransactionLog.tsv' INTO TABLE CapitalTransactionLog FIELDS TERMINATED BY '\t' ;
 