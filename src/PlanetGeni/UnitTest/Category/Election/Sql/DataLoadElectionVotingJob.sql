SET SQL_SAFE_UPDATES =0;
Delete FROm Election;
Delete FROm ElectionCandidate;
Delete FROm ElectionVoting;
LOAD DATA LOCAL INFILE '{0}ElectionVoting-2.tsv' INTO TABLE ElectionVoting FIELDS TERMINATED BY '\t';


Insert into Election 
Select 1, CountryId, date_sub(utc_timestamp(), INTERVAL 20 Day),  date_sub(utc_timestamp(), INTERVAL 7 Day),
 date_sub(utc_timestamp(), INTERVAL 1 Hour), 50000,0,0,0
from CountryCode;
Update Election
SET ElectionId= 1;

Update ElectionVoting
SET ElectionResult = ' ',
ElectionId =1;

Insert into ElectionCandidate
Select uuid(), 1, UserId,null,CountryId, 'I', 1, 'A','','2015-08-02 01:32:33.226059'
FROM ElectionVoting;

Select * from Election order by EndDate desc;
select * from ElectionVoting;
Select * from ElectionCandidate;
Select * from CountryLeader;
Select * from Post Order by UpdatedAt desc;
delete  from PostComment;

Select * from UserNotification Where NotificationTypeId in (145, 146);


delete from Post;
delete from UserNotification;

