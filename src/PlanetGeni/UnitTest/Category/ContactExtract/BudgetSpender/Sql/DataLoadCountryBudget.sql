SET SQL_SAFE_UPDATES =0;
Delete From WebUser;
Delete From UserNotification;
Delete From Post;
Delete From WebJobHistory;
Delete From JobCountry;
Delete From Education;
Delete From CountryBudgetByType;
Delete From CountryBudget;
Delete From UserBankAccount;
Delete From StockTrade;
Delete From StockTrade;
Delete From CountryLeader;



 LOAD DATA LOCAL INFILE '{0}CountryLeader.tsv' INTO TABLE CountryLeader FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}Education.tsv' INTO TABLE Education FIELDS TERMINATED BY '\t';
 LOAD DATA LOCAL INFILE '{0}JobCountry.tsv' INTO TABLE JobCountry FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}CountryBudget.tsv' INTO TABLE CountryBudget FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}CountryBudgetByType.tsv' INTO TABLE CountryBudgetByType FIELDS TERMINATED BY '\t' ;
 LOAD DATA LOCAL INFILE '{0}UserBankAccount.tsv' INTO TABLE UserBankAccount FIELDS TERMINATED BY '\t' ;
 
 Update CountryBudget
Set StartDate =date_format(curdate() - interval 1 month,'%Y-%m-01 00:00:00'),
EndDate = date_format(last_day(curdate()-interval 1 month),'%Y-%m-%d 23:59:59')
Where EndDate ='2015-04-30 00:00:00';


 Update CountryBudget
Set StartDate =date_format(curdate(),'%Y-%m-01 00:00:00'),
EndDate = date_format(last_day(curdate()),'%Y-%m-%d 23:59:59')
Where EndDate ='2025-05-30 00:00:00';

 Update Education
Set CreatedAT =date_format(curdate() - interval 1 month,'%Y-%m-05 00:00:00')
Where UserId<1000;

 Update CountryLeader
Set StartDate =date_format(curdate(),'%Y-%m-01 00:00:00'),
EndDate = date_format(last_day(curdate()),'%Y-%m-%d 23:59:59');

Update Stock
SET CurrentValue=10;
