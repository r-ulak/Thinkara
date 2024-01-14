SET SQL_SAFE_UPDATES =0;
Delete From WebUser;
Delete From UserVoteSelectedChoice;
Delete From UserTask;
Delete From UserJob;
Delete From UserNotification;
Delete From TaskReminder;
Delete From Post;
Delete From WebJobHistory;
Delete From JobCode;
Delete From JobCountry;
Delete From Education;
Delete From MajorCode;
Delete From IndustryCode;



 LOAD DATA LOCAL INFILE '{0}UserJob.tsv' INTO TABLE UserJob FIELDS TERMINATED BY '\t';
 LOAD DATA LOCAL INFILE '{0}WebUser.tsv' INTO TABLE WebUser FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE '{0}Education.tsv' INTO TABLE Education FIELDS TERMINATED BY '\t';
 LOAD DATA LOCAL INFILE '{2}JobCode.csv' INTO TABLE JobCode FIELDS TERMINATED BY ',' ;
 LOAD DATA LOCAL INFILE '{2}MajorCode.csv' INTO TABLE MajorCode FIELDS TERMINATED BY ',' ;
 LOAD DATA LOCAL INFILE '{2}IndustryCode.csv' INTO TABLE IndustryCode FIELDS TERMINATED BY ',' ;
 LOAD DATA LOCAL INFILE '{2}DegreeCode.csv' INTO TABLE DegreeCode FIELDS TERMINATED BY ',' ;
 LOAD DATA LOCAL INFILE '{0}JobCountry.tsv' INTO TABLE JobCountry FIELDS TERMINATED BY '\t' ;
