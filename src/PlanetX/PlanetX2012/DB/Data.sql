use PlanetX;

SET foreign_key_checks = 0;

LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\Webuser.csv' INTO TABLE PlanetX.WebUser FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\Friend.csv' INTO TABLE PlanetX.Friend FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp, UpdatedAt = current_timestamp;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\BusinessTypes.csv' INTO TABLE PlanetX.BusinessCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\BusinessSubTypes.csv' INTO TABLE PlanetX.BusinessSubCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\country_list.csv' INTO TABLE PlanetX.CountryCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\city_list.csv' INTO TABLE PlanetX.CityCode FIELDS TERMINATED BY ',' (CountryId, City) SET CityId=NUll;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\Business.csv' INTO TABLE PlanetX.Business FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp, UpdatedAt = current_timestamp;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\DegreeType.csv' INTO TABLE PlanetX.DegreeCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\MajorCode.csv' INTO TABLE PlanetX.MajorCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\University.csv' INTO TABLE PlanetX.UniversityCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\Post.csv' INTO TABLE PlanetX.Post FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp, UpdatedAt = current_timestamp;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\PostComment.csv' INTO TABLE PlanetX.PostComment FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp, CommentDate = current_timestamp, UpdatedAt = current_timestamp;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\PostUserACL.csv' INTO TABLE PlanetX.PostUserACL FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp,UpdatedAt = current_timestamp;

INSERT INTO `PlanetX`.`Card` (`CardId`,`UserId`,`CardType`,`Amount`,`ExpireDate`,`CreatedAt`) VALUES ('1', 1, '1', '250', CURDATE() , CURDATE());
INSERT INTO `PlanetX`.`Card` (`CardId`,`UserId`,`CardType`,`Amount`,`ExpireDate`,`CreatedAt`) VALUES ('2', 1, '2', '250', CURDATE() , CURDATE());
INSERT INTO `PlanetX`.`ProvinceCode` (`ProvinceId`, `Province`) VALUES ('1', 'Oklahoma');
INSERT INTO `PlanetX`.`LoanCode` (`LoanType`, `Code`) VALUES ('1', 'Credit');
INSERT INTO `PlanetX`.`LoanFromBusiness` (`UserId`,`BusinessId`,`LoanType`,`LoanAmount`,`MonthlyInterestRate`,`CreatedAt`,`UpdatedAt`) VALUES ( 1, '1', '1', '200', '2.5', CURDATE(), CURDATE());
INSERT INTO `PlanetX`.`LoanFromPerson` (`UserId`,`SourceId`,`LoanType`,`LoanAmount`,`MonthlyInterestRate`,`CreatedAt`,`UpdatedAt`) VALUES ( 1, '2', '1', '250', '2.5', CURDATE(), CURDATE());
update PlanetX.PostComment set CommentDate= Current_timestamp where postid<100;

SET foreign_key_checks = 1;
