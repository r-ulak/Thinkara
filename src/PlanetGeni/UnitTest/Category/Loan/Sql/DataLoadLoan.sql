SET SQL_SAFE_UPDATES =0;
Delete From WebUser;
Delete From CreditScore;
Delete From Friend;
Delete From UserBankAccount;
Delete From UserLoan;
Delete From CreditInterest;



 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t';
 LOAD DATA LOCAL INFILE '{0}CreditScore.tsv' INTO TABLE CreditScore FIELDS TERMINATED BY '\t';
 LOAD DATA LOCAL INFILE '{0}Friend.tsv' INTO TABLE Friend FIELDS TERMINATED BY '\t';
 LOAD DATA LOCAL INFILE '{0}UserBankAccount.tsv' INTO TABLE UserBankAccount FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}UserLoan.tsv' INTO TABLE UserLoan FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}CreditInterest.tsv' INTO TABLE CreditInterest FIELDS TERMINATED BY '\t' ;
