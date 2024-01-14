SET SQL_SAFE_UPDATES =0;
Delete From ElectionCandidate;
Delete From ElectionVoting;


LOAD DATA LOCAL INFILE '{0}ElectionVoting-1.tsv' INTO TABLE ElectionVoting FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}ElectionCandidate-1.tsv' INTO TABLE ElectionCandidate FIELDS TERMINATED BY '\t';
Update WebUser 
SET CountryId ='se';
Update WebUser 
SET CountryId ='us' Where UserId =6;