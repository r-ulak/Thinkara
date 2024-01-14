

use planetgeni;

SET foreign_key_checks = 0;

LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/AllowedWebUser.csv' INTO TABLE AllowedWebUser FIELDS TERMINATED BY ',';
UPDATE AllowedWebUser SET EmailId = REPLACE(REPLACE(EmailId, '\r', ''), '\n', '');
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/WebUser.csv' INTO TABLE WebUser FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp;
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/Friend.csv' INTO TABLE Friend FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp;
-- LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/BusinessTypes.csv' INTO TABLE BusinessCode FIELDS TERMINATED BY ',';
-- LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/BusinessSubTypes.csv' INTO TABLE BusinessSubCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/country_list.csv' INTO TABLE CountryCode FIELDS TERMINATED BY ',';
-- LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/citylist.csv' INTO TABLE CityCode FIELDS TERMINATED BY ',' LINES TERMINATED BY '\r\n' (CountryId, City) SET CityId=NUll  ;
-- LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Business.csv' INTO TABLE Business FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp, UpdatedAt = current_timestamp;
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/DegreeCode.csv' INTO TABLE DegreeCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/Education.csv' INTO TABLE Education FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CapitalType.csv' INTO TABLE CapitalType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/MajorCode.csv' INTO TABLE MajorCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/AdsType.csv' INTO TABLE AdsType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/AdsFrequencyType.csv' INTO TABLE AdsFrequencyType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/BudgetCode.csv' INTO TABLE BudgetCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/WeaponType.csv' INTO TABLE WeaponType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/JobCode.csv' INTO TABLE JobCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CountryRevenue.csv' INTO TABLE CountryRevenue FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/WarResult.csv' INTO TABLE WarResult FIELDS TERMINATED BY ',';


LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CountryBudgetByType.csv' INTO TABLE CountryBudgetByType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/UserVoteChoice.csv' INTO TABLE UserVoteChoice FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/TaskType.csv' INTO TABLE TaskType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/Post.csv' INTO TABLE Post FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PostComment.csv' INTO TABLE PostComment FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CountryWeapon.csv' INTO TABLE CountryWeapon FIELDS TERMINATED BY ',' SET PurchasedAt = ADDDATE(current_timestamp , INTERVAL - (FLOOR( 1 + RAND( ) *60 )) DAY);
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CountryLeader.csv' INTO TABLE CountryLeader FIELDS TERMINATED BY ',' SET StartDate = ADDDATE(current_timestamp , INTERVAL -31 DAY), EndDate = ADDDATE(current_timestamp , INTERVAL 31 DAY);
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CountryBudget.csv' INTO TABLE CountryBudget FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/UserLoan.csv' INTO TABLE UserLoan FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/ElectionPosition.csv' INTO TABLE ElectionPosition FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CreditInterest.csv' INTO TABLE CreditInterest FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CapitalTransactionLog.csv' INTO TABLE CapitalTransactionLog FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CreditScore.csv' INTO TABLE CreditScore FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CountryTax.csv' INTO TABLE CountryTax FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/TaxCode.csv' INTO TABLE TaxCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/CountryTaxByType.csv' INTO TABLE CountryTaxByType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/UserMerchandise.csv' INTO TABLE UserMerchandise FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/MerchandiseType.csv' INTO TABLE MerchandiseType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/UserBankAccount.csv' INTO TABLE UserBankAccount FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/Stock.csv' INTO TABLE Stock FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/StockTrade.csv' INTO TABLE StockTrade FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/UserStock.csv' INTO TABLE UserStock FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/UserNotification.csv' INTO TABLE UserNotification FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/NotificationType.csv' INTO TABLE NotificationType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/IndustryCode.csv' INTO TABLE IndustryCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/JobCountry.csv' INTO TABLE JobCountry FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/UserJob.csv' INTO TABLE UserJob FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/JobCode.csv' INTO TABLE JobCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/ElectionAgenda.csv' INTO TABLE ElectionAgenda FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PoliticalParty.csv' INTO TABLE PoliticalParty FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/WebUserContact.csv' INTO TABLE WebUserContact FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PartyMember.csv' INTO TABLE PartyMember FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PartyAgenda.csv' INTO TABLE PartyAgenda FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PartyInvite.csv' INTO TABLE PartyInvite FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/NextLotteryDrawing.csv' INTO TABLE NextLotteryDrawing FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/SlotMachineThree.csv' INTO TABLE SlotMachineThree FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PickThree.csv' INTO TABLE PickThree FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PickFive.csv' INTO TABLE PickFive FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PickThreeWinNumber.csv' INTO TABLE PickThreeWinNumber FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PickFiveWinNumber.csv' INTO TABLE PickFiveWinNumber FIELDS TERMINATED BY ',';

LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PickThreeWinner.csv' INTO TABLE PickThreeWinner FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PickFiveWinner.csv' INTO TABLE PickFiveWinner FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/FundTypeCode.csv' INTO TABLE FundTypeCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/ContactSource.csv' INTO TABLE ContactSource FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/ContactProvider.csv' INTO TABLE ContactProvider FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/PostContentType.csv' INTO TABLE PostContentType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE '/home/planetx/DbScripts/Data/WebJob.csv' INTO TABLE WebJob FIELDS TERMINATED BY ',';
Insert into Election 
Select 1, CountryId, utc_timestamp(),  date_add(utc_timestamp(), INTERVAL 20 Day), date_add(utc_timestamp(), INTERVAL 7 Day), 50000
from CountryCode;

SET SQL_SAFE_UPDATES=0;


SET foreign_key_checks = 1;
