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
Delete From UserStock;
Delete From StockTrade;
Delete From Stock;



LOAD DATA LOCAL INFILE '{0}UserBankAccount.tsv' INTO TABLE UserBankAccount FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}Friend.tsv' INTO TABLE Friend FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}UserStock.tsv' INTO TABLE UserStock FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}StockTrade.tsv' INTO TABLE StockTrade FIELDS TERMINATED BY '\t';
 LOAD DATA LOCAL INFILE '{0}Stock.tsv' INTO TABLE Stock FIELDS TERMINATED BY ',';
 Update WebUser
 SET CountryId ='se';
 
 Update CountryTaxByType
 SET TaxPercent =50
 Where TaxType =5;
 