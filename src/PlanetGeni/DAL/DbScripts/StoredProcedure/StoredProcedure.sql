-- -----------------------------------------------------
-- procedure GetActiveLeadersProfile
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetActiveLeadersProfile` $$
CREATE PROCEDURE `GetActiveLeadersProfile` (
	IN parmCountryId CHAR(2)
)

	BEGIN 

		SELECT  
		t3.UserId,
		CONCAT( t3.NameFirst, ' ',  t3.NameLast) FullName,
		t3.Picture,
		t3.OnlineStatus,
		t3.CountryId,
		t2.ElectionPositionName				
		FROM CountryLeader t1, ElectionPosition t2, WebUser t3 WHERE
			t1.UserId = t3.UserId
		AND t2.PositionTypeId =  t1.PositionTypeId
		AND	t1.EndDate>= UTC_TIMESTAMP(6) 
		AND t1.StartDate<=UTC_TIMESTAMP
		AND	t1.CountryId = parmCountryId;
		
END$$

DELIMITER ;
		
-- -----------------------------------------------------
-- procedure GetSecurityProfile
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetSecurityProfile`;
DELIMITER $$
CREATE  PROCEDURE `GetSecurityProfile`(
			IN parmCountryId CHAR(2)
)
BEGIN
Select 
t1.Name,
t1.Description,
t1.ImageFont,
t1.Cost,
t2.Quantity,
t2.PurchasedPrice,
t2.WeaponCondition
 from WeaponType t1, CountryWeapon t2
 Where 
		t1.WeaponTypeId = t2.WeaponTypeId
 AND 	t2.CountryId = parmCountryId
 AND t2.Quantity > 0 
 LIMIT 25
  ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryCitizenWealthLevel
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryCitizenWealthLevel`;

DELIMITER $$

CREATE  PROCEDURE `GetCountryCitizenWealthLevel`()
BEGIN
Select 
t7.CountryId, AVG(NetWorth.Total) AverageNetWorth
 from (
 
	 (SELECT Sum(PurchasedUnit) * t1.CurrentValue Total, t2.UserId PersonId
		FROM Stock t1, UserStock t2
		WHERE  t1.StockId = t2.StockId
	Group By t2.UserId, t2.StockId) 

union all

	(Select -Sum(LeftAmount) Total, t3.UserId PersonId from UserLoan t3 Where t3.Status ='A' Group By UserId) 

union all

	(Select Sum(LeftAmount) Total, t4.LendorId PersonId from UserLoan t4 Where t4.Status ='A' Group By LendorId) 

union all

	(SELECT (Quantity) *t5.Cost Total, t6.UserId 
		FROM MerchandiseType t5, UserMerchandise t6
		WHERE t6.MerchandiseTypeId = t5.MerchandiseTypeId
	Group By  t6.UserId, t6.MerchandiseTypeId) 

union all

(Select IFNull(Cash + Gold * (Select Cost From CapitalType Where CapitalTypeId=1) + Silver * (Select Cost From CapitalType Where CapitalTypeId=2),0) Total, UserId PersonId from UserBankAccount t6 ) )NetWorth  
	, WebUser t7
 Where 
		NetWorth.PersonId = t7.UserId
 AND 	t7.Active = 1
 Group By t7.CountryId
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryLiteracyScore
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryLiteracyScore`;
DELIMITER $$
CREATE  PROCEDURE `GetCountryLiteracyScore`()
BEGIN
Select SUM(DegreeId) EducationScore, CountryId
	From Education t1, WebUser t2
	Where
		t1.UserId = t2.UserId
		AND t1.Status = 'C'		
		AND t2.Active = 1		
		Group By t2.CountryId ;		
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryAvgJob
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryAvgJob`;
DELIMITER $$
CREATE  PROCEDURE `GetCountryAvgJob`()
BEGIN
Select AVG(Salary) AvgSalary, CountryId
	From UserJob t1, WebUser t2
	Where
		t1.UserId = t2.UserId
		AND t1.Status = 'A'		
		AND t2.Active = 1
		AND StartDate<=UTC_TIMESTAMP(6)
		AND EndDate>=UTC_TIMESTAMP(6)
		Group By t2.CountryId ;		
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountrySafestRank
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountrySafestRank`;
DELIMITER $$
CREATE  PROCEDURE `GetCountrySafestRank`(
			IN parmBudgetType TINYINT(3)
			)
BEGIN
Select 
(cbt.Amount) SeurityBudget, CountryId
	From CountryBudgetByType cbt , CountryBudget cb
	Where
		cbt.BudgetType = parmBudgetType
		AND cb.Status ='A'
		AND cbt.TaskId = cb.TaskId
		AND StartDate<=UTC_TIMESTAMP(6)
		AND EndDate>=UTC_TIMESTAMP(6)
		Group By cb.CountryId ;		
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryDefenseAssetRank
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryDefenseAssetRank`;

DELIMITER $$

CREATE  PROCEDURE `GetCountryDefenseAssetRank`()
BEGIN
Select 
Sum(t2.DefenseScore) DefenseScore, CountryId
	From CountryWeapon t1 , WeaponType t2
	 Where t1.WeaponTypeId = t2.WeaponTypeId
		Group By t1.CountryId ;		
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryOffenseAssetRank
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryOffenseAssetRank`;

DELIMITER $$

CREATE  PROCEDURE `GetCountryOffenseAssetRank`()
BEGIN
Select 
Sum(t2.OffenseScore) OffenseScore, CountryId
	From CountryWeapon t1 , WeaponType t2
	Where t1.WeaponTypeId = t2.WeaponTypeId
		Group By t1.CountryId ;
		
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetRevenueByCountry
-- ---------------------------------------------------

DROP procedure IF EXISTS `GetRevenueByCountry`;

DELIMITER $$

CREATE  PROCEDURE `GetRevenueByCountry`(
         IN  parmCountryId CHAR(2)
         
    )
BEGIN
DECLARE totalRevenue   INT DEFAULT 0 ;
Select Sum(Cash) into totalRevenue 
	From CountryRevenue
	Where 	CountryId = parmCountryId;

Select Sum(Cash)/ totalRevenue * 100 TotalPercent, t2.Description, t2.ImageFont 
	From CountryRevenue t1, TaxCode t2
	Where t1.CountryId = parmCountryId
	AND t1.TaxType = t2.TaxType
	Group By t1.TaxType
	Order By TotalPercent desc;

 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetWeaponStatByCountry
-- ---------------------------------------------------

DROP procedure IF EXISTS `GetWeaponStatByCountry`;

DELIMITER $$

CREATE  PROCEDURE `GetWeaponStatByCountry`(
         IN  parmCountryId CHAR(2)
         
    )
BEGIN
Select Sum(Quantity) TotalAsset from 
CountryWeapon t1 Where t1.CountryId = parmCountryId
	And t1.WeaponCondition >0  ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryBudgetRank
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryBudgetRank`;

DELIMITER $$

CREATE  PROCEDURE `GetCountryBudgetRank`()
BEGIN
Select 
t1.TotalAmount, t1.CountryId
from CountryBudget t1 Where t1.Status ='A'
			AND UTC_TIMESTAMP() BETWEEN t1.StartDate AND t1.EndDate
		Group By t1.CountryId ;
		
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetCountryPopulationRank
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryPopulationRank`;

DELIMITER $$

CREATE  PROCEDURE `GetCountryPopulationRank`()
BEGIN
Select 
Count(*) TotalPopulation, CountryId
from WebUser t1 Where t1.Active =1
		Group By t1.CountryId ;
		
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetTopNRichest
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetTopNRichest`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNRichest`(
		IN parmLimit INT
		)
BEGIN
Select 
t7.UserId,
CONCAT( t7.NameFirst, ' ',  t7.NameLast) FullName,
t7.Picture,
t7.OnlineStatus,
t7.CountryId,
Sum(NetWorth.Total) TotalWorth
 from (
 
	 (SELECT Sum(PurchasedUnit) * t1.CurrentValue Total, t2.UserId PersonId
		FROM Stock t1, UserStock t2
		WHERE  t1.StockId = t2.StockId
	Group By t2.UserId, t2.StockId) 

union all

	(Select -Sum(LeftAmount) Total, t3.UserId PersonId from UserLoan t3 Where t3.Status ='A' Group By UserId) 

union all

	(Select Sum(LeftAmount) Total, t4.LendorId PersonId from UserLoan t4 Where t4.Status ='A' Group By LendorId) 

union all

	(SELECT (Quantity) *t5.Cost Total, t6.UserId 
		FROM MerchandiseType t5, UserMerchandise t6
		WHERE t6.MerchandiseTypeId = t5.MerchandiseTypeId
	Group By  t6.UserId, t6.MerchandiseTypeId) 

union all

(Select IFNull(Cash + Gold * (Select Cost From CapitalType Where CapitalTypeId=1) + Silver * (Select Cost From CapitalType Where CapitalTypeId=2),0) Total, UserId PersonId from UserBankAccount t6 ) )NetWorth  
	, WebUser t7
 Where 
		NetWorth.PersonId = t7.UserId
 AND 	t7.Active = 1
 Group By t7.UserId
 Order by TotalWorth desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetFinanceContent
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetFinanceContent`;

DELIMITER $$

CREATE  PROCEDURE `GetFinanceContent`(
         IN  parmUserId INT  )
BEGIN

(Select SUM(StockTotal) Total, 'Stocks' CapitalType , 'fa icon-man254 text-warning' ImageFont from 
(SELECT SUM(PurchasedUnit) *t1.CurrentValue StockTotal
    FROM Stock t1, UserStock t2
    WHERE t2.UserId = parmUserId 
	AND	  t1.StockId = t2.StockId
Group By t2.StockId) StockTotalById) 

union all

(Select Sum(LeftAmount)  Total, 'LoanLeftToPay' CapitalType, 'fa icon-businessman90 text-danger' ImageFont from UserLoan t3 Where t3.UserId = parmUserId and t3.Status ='A') 

union all

(Select Sum(LeftAmount) Total, 'LoanLeftToCollect' CapitalType, 'fa icon-businessman90 text-success' ImageFont   from UserLoan t4 Where t4.LendorId = parmUserId and t4.Status ='A') 

union all

(Select SUM(MerchandiseTotal) Total, 'Assets' CapitalType, 'fa icon-stepped text-info' ImageFont   from 
(SELECT SUM(Quantity) *t5.Cost MerchandiseTotal
    FROM MerchandiseType t5, UserMerchandise t6
    WHERE t6.UserId = parmUserId 
	AND	  t6.MerchandiseTypeId = t5.MerchandiseTypeId
Group By t6.MerchandiseTypeId) MerchandiseTotalById) ;

 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecuteBuySellGoldAndSilver
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteBuySellGoldAndSilver`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteBuySellGoldAndSilver`(         
		 IN parmUserId INT,		 
		 IN parmDelta DECIMAL(50,2)	,	 
		 IN parmDeltaGold DECIMAL(50,2)	,
		 IN parmOrderType CHAR(1)	,  -- Buy  B Or Sell S
		 IN parmFundType  TINYINT(3),
		 IN parmDeltaSilver DECIMAL(50,2)	
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalBuyerCash_new   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 SET result = 0;
 IF parmDelta<0 THEN
  Select Cash + parmDelta INTO  totalBuyerCash_new from UserBankAccount
	Where Cash>=ABS(parmDelta) and UserId = parmUserId LOCK IN SHARE MODE;  
  ELSE
  Select Cash + parmDelta INTO  totalBuyerCash_new from UserBankAccount
	Where UserId = parmUserId LOCK IN SHARE MODE;    
 END IF;
 IF totalBuyerCash_new>=0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalBuyerCash_new,
			 Gold = Gold + parmDeltaGold ,
			 Silver = Silver + parmDeltaSilver ,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;

		IF parmOrderType = 'B' THEN
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,`FundType`,`CreatedAT`) VALUES
			(UUID(), parmUserId, 0, ABS(parmDelta), parmFundType, UTC_TIMESTAMP(6));
		ELSEIF	parmOrderType = 'S' THEN
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `RecipentId`,`SourceId`,`Amount`,`FundType`,`CreatedAT`) VALUES
			(UUID(), parmUserId, 0, ABS(parmDelta), parmFundType, UTC_TIMESTAMP(6));
		END IF;	
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;
COMMIT;
    select result;


END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetCapitalTransactionLogById
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetCapitalTransactionLogById`;

DELIMITER $$

CREATE  PROCEDURE `GetCapitalTransactionLogById`(         
		 IN parmLastCreatedAt DATETIME(6),
		 IN  parmUserId INT ,
		 IN parmLimit INT         
    )
BEGIN
Select * from 
(
SELECT t1.SourceId, t1.Amount, t1.TaxAmount,  t1.FundType, t1.CreatedAT, t1.RecipentId,
 CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
 t2.Picture, 
 t2.OnlineStatus,
 t2.CountryId, 
 t2.UserId
    FROM CapitalTransactionLog t1 
	LEFT JOIN	WebUser t2 ON t1.RecipentId =t2.UserId
    WHERE 	
		(SourceId = parmUserId  )
	And (t1.CreatedAT < parmLastCreatedAt OR parmLastCreatedAt is NULL)			

union all
SELECT t3.SourceId, t3.Amount, t3.TaxAmount,  t3.FundType, t3.CreatedAT, t3.RecipentId,
 CONCAT( t4.NameFirst, ' ',  t4.NameLast) FullName,
 t4.Picture, 
 t4.OnlineStatus,
 t4.CountryId,
 t4.UserId
 FROM CapitalTransactionLog t3 
	LEFT JOIN	WebUser t4 ON t3.SourceId =t4.UserId
    WHERE 		
		(t3.RecipentId = parmUserId  )
	And (t3.CreatedAT < parmLastCreatedAt OR parmLastCreatedAt is NULL)		
) trn Order By CreatedAT desc Limit parmLimit
;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetAllFundType
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetAllFundType`;

DELIMITER $$

CREATE  PROCEDURE `GetAllFundType`()
BEGIN
 SELECT * 
    FROM FundTypeCode Order By FundType; 
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetAllCapitalType
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetAllCapitalType`;

DELIMITER $$

CREATE  PROCEDURE `GetAllCapitalType`()
BEGIN
 SELECT * 
    FROM CapitalType Order By CapitalTypeId ; 
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetWebUserDTO
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetWebUserDTO`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserDTO`(
         IN  parmUserId INT
         
    )
BEGIN
 SELECT   
 CONCAT( WebUser.NameFirst, ' ',  WebUser.NameLast) FullName,
 WebUser.Picture, 
 WebUser.OnlineStatus,
 WebUser.CountryId
 FROM WebUser
 Where
 WebUser.UserId =parmUserId;   
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetWebUserCache
-- ---------------------------------------------------

DROP procedure IF EXISTS `GetWebUserCache`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserCache`(
         IN  parmUserId INT
         
    )
BEGIN
 SELECT   
 CONCAT( t1.NameFirst, ' ',  t1.NameLast) FullName,
 t1.NameFirst,
 t1.NameLast,
 t1.Picture, 
 t1.OnlineStatus,
 t1.CountryId, 
 t1.UserLevel,
		(Select 
			Case WHEN Count(*) =1 THEN 'true'
			ELSE 'false'
			END leader
			FROM CountryLeader Where 
				EndDate>= UTC_TIMESTAMP 
			AND StartDate<=UTC_TIMESTAMP
			AND	CountryId = t1.CountryId	
			AND UserId = parmUserId
			) IsLeader
 FROM WebUser t1
 Where
 t1.UserId =parmUserId   
  LIMIT 1
  ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetWebUserIndexDTO
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetWebUserIndexDTO`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserIndexDTO`(
         IN  parmUserId INT    )
BEGIN
 SELECT   
	 UserId,
	 CONCAT( NameFirst, ' ',  NameLast) FullName,
	 NameFirst, 
	 NameLast,
	 Picture, 
	 CountryId,
	 EmailId 
 FROM WebUser
 Where
	UserId =parmUserId;   
 END$$
 DELIMITER ;

-- -----------------------------------------------------
-- procedure GetFriendsInfo
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetFriendsInfo`;

DELIMITER $$

CREATE  PROCEDURE `GetFriendsInfo`(
         IN  parmUserId INT
         
    )
BEGIN
 SELECT 
 t2.FriendUserId,
 t2.RelationDirection, 
 t1.NameFirst,
 t1.NameLast,
 CONCAT( t1.NameFirst, ' ',  t1.NameLast) FullName,
 t1.Picture,
 t1.Active,
 t1.OnlineStatus,
 t1.CountryId
 FROM WebUser t1,
(SELECT f.FriendUserId,
       (CASE WHEN COUNT(DISTINCT RelationType) = 1 THEN MIN(RelationType)
             ELSE 'B'
        END) RelationDirection
FROM (Select FollowerUserId as FriendUserId, 'FD' as RelationType From Friend Where FollowingUserId = parmUserId
      Union ALL
      Select FollowingUserId as FriendUserId, 'FG' as RelationType From Friend Where FollowerUserId = parmUserId
     ) f
GROUP BY f.FriendUserId) t2
Where t1.UserId = t2.FriendUserId Order By FullName;
  
    
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateOnlineStatus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateOnlineStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateOnlineStatus`(
         IN  webUserId INT,     
		 IN  onlineStatus TINYINT(3)		
    )
BEGIN
 Update WebUser 
	SET OnlineStatus = onlineStatus
    WHERE UserId = webUserId; 
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateProfileName
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateProfileName`;

DELIMITER $$

CREATE  PROCEDURE `UpdateProfileName`(
         IN	parmUserId INT,
		 IN	parmNameFirst	VARCHAR(45), 		
		 IN	parmNameLast	VARCHAR(45)
    )
BEGIN
 Update WebUser 
	SET NameFirst = parmNameFirst,
		NameLast = parmNameLast
    WHERE UserId = parmUserId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateProfilePic
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateProfilePic`;

DELIMITER $$

CREATE  PROCEDURE `UpdateProfilePic`(
         IN	parmUserId INT,
		 IN parmPicture VARCHAR(255)	 
    )
BEGIN
 Update WebUser 
	SET Picture = parmPicture
    WHERE UserId = parmUserId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCityList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCityList`;

DELIMITER $$

CREATE  PROCEDURE `GetCityList`()
BEGIN
Select CityId, City, CountryId from CityCode;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryCodes
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCountryCodes`;

DELIMITER $$

CREATE  PROCEDURE `GetCountryCodes`()
BEGIN
Select  * from CountryCode Order by Code asc;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetIndustryCodeList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetIndustryCodeList`;

DELIMITER $$

CREATE  PROCEDURE `GetIndustryCodeList`()
BEGIN
Select  * from IndustryCode Order by IndustryId asc;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetDegreeCodeList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetDegreeCodeList`;

DELIMITER $$

CREATE  PROCEDURE `GetDegreeCodeList`()
BEGIN
Select * from DegreeCode;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetMajorCodeList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetMajorCodeList`;

DELIMITER $$

CREATE  PROCEDURE `GetMajorCodeList`()
BEGIN
Select MajorId, MajorName, ImageFont, Description, MajorRank, Cost, Duration, JobProbability, 0 DegreeId
 from MajorCode;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCityListByRange
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCityListByRange`;

DELIMITER $$

CREATE  PROCEDURE `GetCityListByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select CityId, City, CountryId from CityCode where  CityId BETWEEN startRange and endRange;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCityListByRangeTotal
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCityListByRangeTotal`;

DELIMITER $$

CREATE  PROCEDURE `GetCityListByRangeTotal`()
BEGIN
Select count(*)from CityCode ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetWebUserByRange
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserByRange`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
SELECT `UserId`,
    `NameFirst`,    
    `NameLast`,
    `EmailId`,
    NULL `OldNameFirst`,    
    NULL `OldNameLast`,
    NULL `OldEmailId`,
    NULL `Picture`,
    NULL `ActionType`,
    `CreatedAt`
FROM `WebUser` order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserByRangeTotal
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserByRangeTotal`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserByRangeTotal`()
BEGIN
Select count(*) from WebUser;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetCommentsForPost
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `GetCommentsForPost`;

DELIMITER $$

CREATE  PROCEDURE `GetCommentsForPost`(
         IN  postIdList TEXT	,
		 IN  parmUserId INT,
		 IN  commentLimit INT
    )
BEGIN
 set @concatsql = CONCAT('select 
t1.PostCommentId,
t2.UserId,
CONCAT( t2.NameFirst, SPACE(1), t2.NameLast) FullName, 
t2.Picture ,
t1.PostId,
t1.ParentCommentId,
t1.ChildCommentCount,
t1.DigIt,
t1.CoolIt,
t3.DigType,
t1.CommentText,
t1.CreatedAt
from PostComment t1 
INNER JOIN WebUser t2 ON t1.UserId = t2.UserId
LEFT JOIN UserDig t3 ON t3.PostCommentId = t1.PostCommentId AND ',parmUserId  , ' = t3.UserId
where t1.PostId in (''',
                  replace(postIdList, ',', ''','''),
                  ''') 
AND t1.ParentCommentId is NULL
ORDER BY t1.CreatedAt desc, t1.UserId  LIMIT ', commentLimit);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetCommentListByPostId
-- -----------------------------------------------------

DELIMITER ;

DROP procedure IF EXISTS `GetCommentListByPostId`;

DELIMITER $$

CREATE  PROCEDURE `GetCommentListByPostId`(
         IN  parmPostId CHAR(36) ,
		 IN lastDateTime DateTime(6),
		 IN  parmUserId INT,
		 IN  commentLimit INT
    )
BEGIN
select t1.PostCommentId,
t2.UserId,
CONCAT( t2.NameFirst, ' ', t2.NameLast) FullName, 
t2.Picture,
t1.PostId,
t1.ParentCommentId,
t1.ChildCommentCount,
t1.DigIt,
t1.CoolIt,
t3.DigType,
t1.CommentText,
t1.CreatedAt
From PostComment t1 
INNER JOIN WebUser t2 ON t1.UserId = t2.UserId
LEFT JOIN UserDig t3 ON t3.PostCommentId = t1.PostCommentId AND parmUserId = t3.UserId
WHERE
t1.PostId = parmPostId 
AND (t1.ParentCommentId is NULL ) 
AND (t1.CreatedAt <lastDateTime OR lastDateTime is NULL)
 ORDER BY t1.CreatedAt DESC, t1.UserId 
 LIMIT commentLimit;

END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetCommentListByParentId
-- -----------------------------------------------------

DELIMITER ;

DROP procedure IF EXISTS `GetCommentListByParentId`;

DELIMITER $$

CREATE  PROCEDURE `GetCommentListByParentId`(
         IN  parentCommentId CHAR(36) ,
		 IN lastDateTime DateTime(6),
		 IN  parmUserId INT,
		 IN  commentLimit INT
    )
BEGIN
select t1.PostCommentId,
t2.UserId,
CONCAT( t2.NameFirst, ' ', t2.NameLast) FullName, 
t2.Picture ,
t1.PostId,
t1.ParentCommentId,
t1.ChildCommentCount,
t1.DigIt,
t1.CoolIt,
t3.DigType,
t1.CommentText,
t1.CreatedAt
From PostComment t1 
INNER JOIN WebUser t2 ON t1.UserId = t2.UserId
LEFT JOIN UserDig t3 ON t3.PostCommentId = t1.PostCommentId AND parmUserId = t3.UserId
WHERE
	t1.ParentCommentId = parentCommentId and t2.UserId= t1.UserId
AND (t1.CreatedAt <lastDateTime OR lastDateTime is NULL)
 ORDER BY t1.CreatedAt DESC, t1.UserId
 LIMIT commentLimit;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetPostForUserWithLimit
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `GetPostForUserWithLimit`;

DELIMITER $$

CREATE  PROCEDURE `GetPostForUserWithLimit`(
        IN  parmUserId INT,
		IN  lastPostId CHAR(36),
		IN  lastCreatedAt DateTime,
		IN  postLimit INT,
		IN  parmCountryId CHAR(2),
		IN  parmPartyId CHAR(36)
		
    )
BEGIN

 SELECT
t1.PostId,
t1.PostContent,
t1.PostTitle,
t1.ChildCommentCount,
t1.DigIt,
t1.CoolIt,
t2.DigType,
t1.CommentEnabled,
t1.ImageName,
t1.CountryId,
t1.Parms,
t1.PostContentTypeId,
t1.PartyId,
t1.CreatedAt,
t1.UserId 
FROM
    Post t1
	LEFT JOIN UserDig t2 ON t2.PostCommentId = t1.PostId AND parmUserId = t2.UserId

Where
    (t1.PostId !=lastPostId OR lastPostId is NULL)
        AND (t1.CreatedAt < lastCreatedAt OR lastCreatedAt is NULL )
		AND (
			   (t1.UserId = parmUserId  or t1.UserId in (Select FollowingUserId from Friend t2 where FollowerUserId = parmUserId))
			OR (t1.CountryId = parmCountryId AND parmCountryId IS NOT NULL)	
			OR (t1.PartyId = parmPartyId AND parmPartyId IS NOT NULL)	
			OR (t1.UserId =0 AND t1.CountryId is NULL AND t1.PartyId is NULL)	
			)				
Order By t1.CreatedAt Desc, t1.UserId
LIMIT postLimit;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetNewUserPostWithLimit
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetNewUserPostWithLimit`;

DELIMITER $$

CREATE  PROCEDURE `GetNewUserPostWithLimit`(
        IN  parmUserId INT,
		IN  lastPostId CHAR(36),
		IN  lastCreatedAt DateTime,
		IN  postLimit INT,
		IN  parmCountryId CHAR(2),
		IN  parmPartyId CHAR(36)
	
)
BEGIN

 SELECT
t1.PostId,
t1.PostContent,
t1.PostTitle,
t1.ChildCommentCount,
t1.DigIt,
t1.CoolIt,
t2.DigType,
t1.CommentEnabled,
t1.ImageName,
t1.CountryId,
t1.Parms,
t1.PostContentTypeId,
t1.PartyId,
t1.CreatedAt,
t1.UserId 
FROM
    Post t1
	LEFT JOIN UserDig t2 ON t2.PostCommentId = t1.PostId AND parmUserId = t2.UserId

Where
    (t1.PostId !=lastPostId OR lastPostId is NULL)
        AND (t1.CreatedAt > lastCreatedAt OR lastCreatedAt is NULL )
		AND (
			   (t1.UserId = parmUserId  or t1.UserId in (Select FollowingUserId from Friend t2 where FollowerUserId = parmUserId))
			OR (t1.CountryId = parmCountryId AND parmCountryId IS NOT NULL)	
			OR (t1.PartyId = parmPartyId AND parmPartyId IS NOT NULL)	
			OR (t1.UserId =0 AND t1.CountryId is NULL AND t1.PartyId is NULL)	
			)				
Order By t1.CreatedAt Desc, t1.UserId
LIMIT postLimit;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateUserDigAdd
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateUserDigAdd`;

DELIMITER $$

CREATE  PROCEDURE `UpdateUserDigAdd`(
		 IN  parmPostCommentId CHAR(36),
		 IN  parmDigType TINYINT(2),
		 IN  parmOldDigType TINYINT(2),		 		 
		 IN  parmPostCommentType CHAR(1)
		 
    )
BEGIN
DECLARE adddigit   MEDIUMINT(8)  DEFAULT 0 ;
DECLARE addcoolIt  MEDIUMINT(8)  DEFAULT 0 ;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
ROLLBACK;
END;

START TRANSACTION;
IF parmDigType =1 THEN
	SET adddigit =1;
ELSEIF  parmDigType =2  THEN
	SET addcoolIt =1;
END IF;

IF parmOldDigType =1 THEN
	SET adddigit = -1;
ELSEIF  parmOldDigType =2  THEN
	SET addcoolIt = -1;
END IF;	

IF	parmPostCommentType = 'P' THEN
	SELECT * FROM Post Where PostId = parmPostCommentId LOCK IN SHARE MODE;
	
	UPDATE Post SET 
	DigIt = DigIt + adddigit,
	CoolIt = CoolIt + addcoolIt
	Where PostId = parmPostCommentId;
	
ELSEIF  parmPostCommentType = 'C' THEN
	
	SELECT * FROM PostComment Where PostCommentId = parmPostCommentId LOCK IN SHARE MODE;
	
	UPDATE PostComment SET
	DigIt = DigIt + adddigit,
	CoolIt = CoolIt + addcoolIt
	Where PostCommentId = parmPostCommentId;

END IF;	
COMMIT;
	
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure UpdateUserDigMinus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateUserDigMinus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateUserDigMinus`(
		 IN  parmPostCommentId CHAR(36),
		 IN  parmOldDigType TINYINT(2),
		 IN  parmPostCommentType CHAR(1),
		 IN  parmUserId INT
		 
    )
BEGIN
DECLARE adddigit   MEDIUMINT(8)  DEFAULT 0 ;
DECLARE addcoolIt  MEDIUMINT(8)  DEFAULT 0 ;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
ROLLBACK;
END;

START TRANSACTION;
IF parmOldDigType =1 THEN
	SET adddigit = -1;
ELSEIF  parmOldDigType =2  THEN
	SET addcoolIt = -1;
END IF;
	

IF	parmPostCommentType = 'P' THEN
	SELECT * FROM Post Where PostId = parmPostCommentId LOCK IN SHARE MODE;
	
	UPDATE Post SET 
	DigIt = DigIt + adddigit,
	CoolIt = CoolIt + addcoolIt
	Where PostId = parmPostCommentId;
	Select adddigit;
	
ELSEIF  parmPostCommentType = 'C' THEN
	SELECT * FROM PostComment Where PostCommentId = parmPostCommentId LOCK IN SHARE MODE;
	
	UPDATE PostComment SET
	DigIt = DigIt + adddigit,
	CoolIt = CoolIt + addcoolIt
	Where PostCommentId = parmPostCommentId;
	Select addcoolIt;

END IF;	
  Delete  FROM UserDig 
	    WHERE 	PostCommentId = parmPostCommentId
		AND		UserId = parmUserId; 
COMMIT;
	
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetTopNTopicTagContent
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `GetTopNTopicTagContent`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNTopicTagContent`(
         IN  tagLimit INT,	
		 IN  timeLimitMinute INT
    )
BEGIN
SELECT Tag, TagCount FROM `TopicTag`
WHERE
DATE_ADD(`TopicTag`.UpdatedAt, INTERVAL timeLimitMinute MINUTE) > NOW() 
ORDER BY TagCount DESC
LIMIT tagLimit
;

END$$

-- -----------------------------------------------------
-- procedure GetTopTopicTagContent
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `GetTopTopicTagContent`;

DELIMITER $$

CREATE  PROCEDURE `GetTopTopicTagContent`(
         IN  tagLimit INT			 
    )
BEGIN
SELECT Tag, TagCount FROM `TopicTag`
ORDER BY UpdatedAt DESC, TagCount DESC
LIMIT tagLimit
;

END$$
-- -----------------------------------------------------
-- procedure GetTopicByTag
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `GetTopicByTag`;

DELIMITER $$

CREATE  PROCEDURE `GetTopicByTag`(
         IN  topicTag varchar(50)	
    )
BEGIN

Select * from TopicTag where Tag = TopicTag;

END$$

-- -----------------------------------------------------
-- procedure GetJobCodeList
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `GetJobCodeList`;

DELIMITER $$

CREATE  PROCEDURE `GetJobCodeList`()
BEGIN
Select * from JobCode;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetConnectionByUserId
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `GetConnectionByUserId`;

DELIMITER $$

CREATE  PROCEDURE `GetConnectionByUserId`(
         IN  webUserId INT
    )
BEGIN

Select * from UserConnection where UserId = webUserId;

END$$

-- -----------------------------------------------------
-- procedure SaveTopicTag
-- -----------------------------------------------------
DELIMITER ;

DROP procedure IF EXISTS `SaveTopicTag`;

DELIMITER $$

CREATE  PROCEDURE `SaveTopicTag`(
         IN  newTag VARCHAR(50)
    )
BEGIN
DECLARE counter INT DEFAULT 0;
START TRANSACTION;

SELECT TagCount into counter FROM TopicTag where Tag = newTag LOCK IN SHARE MODE;
if counter >0 THEN
UPDATE TopicTag SET TagCount= counter + 1 where Tag = newTag;
ELSE 
INSERT INTO TopicTag 
	(Tag,
	TagCount,
	CreatedAt,
	UpdatedAt)
	VALUES
	(
	newTag,
	1,
	current_timestamp,
	current_timestamp
	);
END IF;
COMMIT;


END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetWebUserInsertByRange
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserInsertByRange`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserInsertByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select * from WebUserUpdate Where ActionType= 'I' order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserInsertUpdatesByRangeTotal
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserInsertUpdatesByRangeTotal`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserInsertUpdatesByRangeTotal`()
BEGIN
Select count(*) from WebUserUpdate Where ActionType= 'I';
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetWebUserUpdateByRange
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserUpdateByRange`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserUpdateByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select * from WebUserUpdate Where ActionType= 'U' order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserUpdateUpdatesByRangeTotal
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserUpdateUpdatesByRangeTotal`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserUpdateUpdatesByRangeTotal`()
BEGIN
Select count(*) from WebUserUpdate Where ActionType= 'U';
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetWebUserDeleteByRange
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserDeleteByRange`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserDeleteByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select * from WebUserUpdate Where ActionType= 'D' order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserDeleteUpdatesByRangeTotal
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWebUserDeleteUpdatesByRangeTotal`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserDeleteUpdatesByRangeTotal`()
BEGIN
Select count(*) from WebUserUpdate Where ActionType= 'D';
 END$$

DELIMITER ;
/*
-- -----------------------------------------------------
-- Trigger WebUserInsert
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `TriggerWebUserInsert` ;
DELIMITER $$
CREATE TRIGGER `TriggerWebUserInsert` 
AFTER INSERT  ON `WebUser` 
FOR EACH ROW BEGIN

 
SET action = 'I';

INSERT INTO `WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameLast`,
`EmailId`,
`Picture`,
`ActionType`)
VALUES
(NEW.UserId,
NEW.NameFirst,
NEW.NameLast,
NEW.EmailId,
NEW.Picture,
action
);
END$$

 DELIMITER ;


-- -----------------------------------------------------
-- Trigger WebUserUpdate
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `TriggerWebUserUpdate` ;
DELIMITER $$
CREATE TRIGGER `TriggerWebUserUpdate` 
AFTER Update  ON `WebUser` 
FOR EACH ROW BEGIN

 
SET action = 'U';

Insert INTO `WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameLast`,
`EmailId`,
`OldNameFirst`,
`OldNameLast`,
`OldEmailId`,
`Picture`,
`ActionType`)
VALUES
(NEW.UserId,
NEW.NameFirst,
NEW.NameLast,
NEW.EmailId,
OLD.NameFirst,
OLD.NameLast,
OLD.EmailId,
NEW.Picture,
action
);
END$$

 DELIMITER ;


-- -----------------------------------------------------
-- Trigger WebUserDelete
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `TriggerWebUserDelete` ;
DELIMITER $$
CREATE TRIGGER `TriggerWebUserDelete` 
AFTER Delete  ON `WebUser` 
FOR EACH ROW BEGIN

 
SET action = 'D';

Insert INTO `WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameLast`,
`EmailId`,
`ActionType`)
VALUES
(OLD.UserId,
OLD.NameFirst,
OLD.NameLast,
OLD.EmailId,
action
);
END$$

 DELIMITER ;
 */
 
-- -----------------------------------------------------
-- procedure GetUserById
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetUserById`;

DELIMITER $$

CREATE  PROCEDURE `GetUserById`(
         IN  webUserId INT	
    )
BEGIN
SELECT
`WebUser`.`UserId`,
`WebUser`.`NameFirst`,
`WebUser`.`NameLast`,
`WebUser`.`EmailId`,
`WebUser`.`Picture`,
`WebUser`.`Active`,
`WebUser`.`OnlineStatus`,
`WebUser`.`CreatedAt`
FROM `WebUser` Where UserId =webUserId Limit 1;

 END$$

 DELIMITER ;
 
 -- -----------------------------------------------------
-- procedure GetBudgetBracketByTaskId
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetBudgetBracketByTaskId` $$
CREATE PROCEDURE `GetBudgetBracketByTaskId` (
	IN parmTaskId CHAR(36) 
)

	BEGIN 

		SELECT  * FROM CountryBudget WHERE
				TaskId = parmTaskId;
			
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCurrentCountryBudget
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetCurrentCountryBudget` $$
CREATE PROCEDURE `GetCurrentCountryBudget` (
	IN parmCountryId CHAR(2) 
)

	BEGIN 

		SELECT  TaskId, CountryId, TotalAmount, StartDate, EndDate, Status FROM CountryBudget WHERE
			StartDate<=UTC_TIMESTAMP(6)
		AND	EndDate>= UTC_TIMESTAMP(6)
		AND	CountryId = parmCountryId
		And Status = 'A'
			order by EndDate desc limit 1;
END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetCountryBudgetByIdList
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetCountryBudgetByIdList` $$
CREATE PROCEDURE `GetCountryBudgetByIdList` (
	 IN parmTaskId CHAR(36)
	 
	 )

	BEGIN 
	SELECT  cbt.TaskId, cbt.Amount, cbt.BudgetType , bc.Description, bc.ImageFont
	FROM CountryBudgetByType cbt , BudgetCode bc
	
	WHERE
			cbt.BudgetType = bc.BudgetType
		AND cbt.TaskId = parmTaskId;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryBudgetByType
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetCountryBudgetByType` $$
CREATE PROCEDURE `GetCountryBudgetByType` (
	 IN parmBudgetType TINYINT(3),
	 IN parmCountryId CHAR(2) 
	 
	 )

	BEGIN 
	SELECT  cbt.*
	FROM CountryBudgetByType cbt , CountryBudget cb
	
	WHERE
			cbt.BudgetType = parmBudgetType
		AND cb.CountryId = parmCountryId
		AND cbt.TaskId = cb.TaskId
		AND cb.Status ='A'
		AND StartDate<=UTC_TIMESTAMP(6)
		AND EndDate>=UTC_TIMESTAMP(6)
		ORDER BY EndDate DESC LIMIT 1;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCurrentCountryTax
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetCurrentCountryTax` $$
CREATE PROCEDURE `GetCurrentCountryTax` (
	IN parmCountryId CHAR(2) 
)

	BEGIN 

		SELECT  TaskId, CountryId, Status  FROM CountryTax 
		Where CountryId = parmCountryId
		And Status = 'A'
			order by EndDate desc limit 1;
END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetTaxbracketByTaskId
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetTaxbracketByTaskId` $$
CREATE PROCEDURE `GetTaxbracketByTaskId` (
	IN parmTaskId CHAR(36) 
)

	BEGIN 

		SELECT  TaskId, CountryId, Status FROM CountryTax WHERE
			TaskId = parmTaskId;
			
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryTaxTypeById
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetCountryTaxTypeById` $$
CREATE PROCEDURE `GetCountryTaxTypeById` (
	 IN parmTaskId CHAR(36)
	 
	 )

	BEGIN 
	SELECT  ctt.TaskId, ctt.TaxPercent, ctt.TaxType , tc.Description, tc.ImageFont
	FROM CountryTaxByType ctt , TaxCode tc
	
	WHERE
			ctt.TaxType = tc.TaxType
		AND ctt.TaskId = parmTaskId
		AND tc.AllowEdit = 1;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateNewCountryTax
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateNewCountryTax`;

DELIMITER $$

CREATE  PROCEDURE `UpdateNewCountryTax`(
         IN  parmTaskIdCurrent CHAR(36),
		 IN  parmTaskIdNew CHAR(36)
    )
BEGIN
DECLARE  syscurrent_date DATETIME(6);
SET syscurrent_date = UTC_TIMESTAMP(6);
 Update CountryTax
	SET Status = 'A',
	StartDate = DATE_ADD(syscurrent_date, INTERVAL 1 second)
    WHERE TaskId = parmTaskIdNew; 
	
 Update CountryTax
	SET EndDate = syscurrent_date,
	Status = 'E'
    WHERE TaskId = parmTaskIdCurrent; 
	
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateNewCountryBudget
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateNewCountryBudget`;

DELIMITER $$

CREATE  PROCEDURE `UpdateNewCountryBudget`(
         IN  parmTaskIdCurrent CHAR(36),
		 IN  parmTaskIdNew CHAR(36)
    )
BEGIN
DECLARE  syscurrent_date DATETIME(6);
SET syscurrent_date = UTC_TIMESTAMP(6);
 Update CountryBudget
	SET Status = 'A',
	StartDate = DATE_ADD(syscurrent_date, INTERVAL 1 second)
    WHERE TaskId = parmTaskIdNew; 
	
 Update CountryBudget
	SET EndDate = syscurrent_date,
	Status = 'E'
    WHERE TaskId = parmTaskIdCurrent; 
	
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure BudgetCodeList
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `BudgetCodeList` $$
CREATE PROCEDURE `BudgetCodeList` ()

	BEGIN 
		SELECT  BudgetType, Description, ImageFont FROM BudgetCode;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAdsTypeList
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetAdsTypeList` $$
CREATE PROCEDURE `GetAdsTypeList` ()

	BEGIN 
		SELECT  *  FROM AdsType;
END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetAdsFrequencyTypeList
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetAdsFrequencyTypeList` $$
CREATE PROCEDURE `GetAdsFrequencyTypeList` ()

	BEGIN 
		SELECT  *  FROM AdsFrequencyType;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdatePostCommentCount
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdatePostCommentCount`;

DELIMITER $$

CREATE  PROCEDURE `UpdatePostCommentCount`(
		 IN  parmPostId char(36)         
    )
BEGIN
SELECT ChildCommentCount FROM Post Where PostId = parmPostId LOCK IN SHARE MODE;
UPDATE Post SET ChildCommentCount = ChildCommentCount + 1 Where PostId = parmPostId;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateCommentCount
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateCommentCount`;

DELIMITER $$

CREATE  PROCEDURE `UpdateCommentCount`(
		 IN  parmPostId char(36),
         IN  parmParentCommentId char(36) 
    )
BEGIN
SELECT ChildCommentCount FROM Post Where PostId = parmPostId LOCK IN SHARE MODE;
UPDATE Post SET ChildCommentCount = ChildCommentCount + 1 Where PostId = parmPostId;


SELECT ChildCommentCount FROM PostComment Where PostCommentId = parmParentCommentId LOCK IN SHARE MODE;
UPDATE PostComment SET ChildCommentCount = ChildCommentCount + 1  Where PostCommentId = parmParentCommentId ;

 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetWeaponTypeList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWeaponTypeList`;

DELIMITER $$

CREATE  PROCEDURE `GetWeaponTypeList`()
BEGIN
Select 
WeaponTypeId,
Name,
Description,
Cost,
ImageFont,
WeaponTypeCode
 from WeaponType;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetWeaponListByCountryId
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWeaponListByCountryId`;

DELIMITER $$

CREATE  PROCEDURE `GetWeaponListByCountryId`(
IN parmCountryId char(2),
IN parmlastWeaponId INT,
IN parmLimit INT
)
BEGIN
Select 
CountryWeaponId,
Name,
Description,
ImageFont,
WeaponTypeCode,
PurchasedPrice,
PurchasedAt,
WeaponCondition,
Quantity
 from WeaponType t1, CountryWeapon t2
 Where 
		t1.WeaponTypeId = t2.WeaponTypeId
 AND 	t2.CountryId = parmCountryId
 AND (t2.CountryWeaponId < parmlastWeaponId or parmlastWeaponId =0)
 Order By CountryWeaponId desc
 LIMIT parmLimit
 ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetWeaponSummary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetWeaponSummary`;

DELIMITER $$

CREATE  PROCEDURE `GetWeaponSummary`(
IN parmCountryId char(2)
)
BEGIN
Select 
WeaponTypeCode,
CountryId,
Sum(PurchasedPrice) TotalPrice,
Avg(WeaponCondition) AverageCondition,
Sum(Quantity) TotalQty
 from WeaponType t1, CountryWeapon t2
 Where 
		t1.WeaponTypeId = t2.WeaponTypeId
 AND 	t2.CountryId = parmCountryId
 Group By WeaponTypeCode
 Order by WeaponTypeCode
 ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetActiveLeaders
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetActiveLeaders` $$
CREATE PROCEDURE `GetActiveLeaders` (
	IN parmCountryId CHAR(2) ,
	IN parmPostiontypeId TINYINT(3) 
)

	BEGIN 

		SELECT  *
		FROM CountryLeader WHERE
			EndDate>= UTC_TIMESTAMP 
		AND StartDate<=UTC_TIMESTAMP
		AND	CountryId = parmCountryId
		AND PositionTypeId = parmPostiontypeId;
END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetPendingWarRequestByCountryId
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPendingWarRequestByCountryId`;

DELIMITER $$

CREATE  PROCEDURE `GetPendingWarRequestByCountryId`(
IN parmCountryId char(2),
IN parmTargetCountryId char(2)
)
BEGIN
Select 
*
 from RequestWarKey t1
 Where 
		t1.ApprovalStatus = "W"
 AND 	t1.TaregtCountryId = parmTargetCountryId
 AND 	t1.RequestingCountryId = parmCountryId
LIMIT 1
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetUserTaskList
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetUserTaskList` $$
CREATE PROCEDURE `GetUserTaskList` (
	IN parmuserId INT,
	IN parmlastTaskId CHAR(36),
	IN lastCreatedAt DATETIME(6),
	IN parmLimit INT
	
)

	BEGIN 
		SELECT 
t1.TaskId,
t1.CompletionPercent,
t1.Flagged,
t1.Status,
t1.Parms,
t1.DueDate,
t2.ShortDescription,
t2.Description,
t1.CreatedAt
		FROM UserTask t1, TaskType t2 WHERE
			t1.UserId = parmuserId
		AND	Status IN ('I', 'P')
		AND t1.TaskTypeId = t2.TaskTypeId
		And (t1.TaskId != parmlastTaskId Or parmlastTaskId is NULL)
		And (t1.CreatedAt >= lastCreatedAt OR lastCreatedAt is NULL)
		Order By t1.CreatedAt asc, t1.Priority, t1.DueDate
		LIMIT parmLimit;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetCountByChoiceForTask
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCountByChoiceForTask`;

DELIMITER $$

CREATE  PROCEDURE `GetCountByChoiceForTask`(
	IN parmTaskId CHAR(36)	
)
BEGIN
Select ChoiceId, CAST(SUM(Score) AS SIGNED) VoteCount
 from UserVoteSelectedChoice where  TaskId = parmTaskId
 Group BY ChoiceId;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetVotingDetailsByTaskId
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetVotingDetailsByTaskId` $$
CREATE PROCEDURE `GetVotingDetailsByTaskId` (	
	IN parmTaskId CHAR(36),
	IN parmUserId INT
)

	BEGIN 
		SELECT 
t1.AssignerUserId InitiatorUserId,
t1.TaskTypeId ,
t3.Active,
CONCAT( t3.NameFirst, ' ',  t3.NameLast) FullName,
t3.OnlineStatus ,
t3.Picture UserPicture,
t2.Picture LogoPicture,
t2.ShortDescription,
t2.Description,
t1.Parms,
t2.ChoiceType,
t2.MaxChoiceCount,
t1.CreatedAt
		FROM UserTask t1, TaskType t2, WebUser t3 WHERE
			t1.TaskId = parmTaskId
		AND	t1.AssignerUserId = t3.UserId
		AND t1.TaskTypeId = t2.TaskTypeId
		AND t1.DueDate >=UTC_TIMESTAMP(6)
		AND	t1.Status IN ('I', 'P')
		AND t1.UserId = parmUserId
		;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetVoteChoiceByTaskType
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetVoteChoiceByTaskType`;

DELIMITER $$

CREATE  PROCEDURE `GetVoteChoiceByTaskType`(
IN parmTaskTypeId INT
)
BEGIN
Select *
 from UserVoteChoice where  TaskTypeId = parmTaskTypeId ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateTaskStatus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateTaskStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateTaskStatus`(
         IN  parmTaskId CHAR(36),
		 IN parmUserId INT,
		 IN  parmCompletionPercent TINYINT(2),
		 IN  parmStatus CHAR(1)
    )
BEGIN
 Update UserTask 
	SET CompletionPercent = parmCompletionPercent,
	Status = parmStatus
    WHERE TaskId = parmTaskId AND UserId = parmUserId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure CheckIncompleteTask
-- -----------------------------------------------------


DROP procedure IF EXISTS `CheckIncompleteTask`;

DELIMITER $$

CREATE  PROCEDURE `CheckIncompleteTask`(
         IN  parmTaskId CHAR(36),
		 IN parmUserId INT
    )
BEGIN
 Select Count(*) cnt FROM UserTask 	
    WHERE TaskId = parmTaskId AND UserId = parmUserId AND ( CompletionPercent <100); 
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetTaskCountById
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `GetTaskCountById` $$

CREATE  PROCEDURE `GetTaskCountById`
(In parmTaskId CHAR(36))

BEGIN
  SELECT count(*) cnt from UserTask WHERE TaskId = parmTaskId;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure ExecuteLoanPayment
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteLoanPayment`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteLoanPayment`(        		 
		 IN parmLendorId INT,		
		 IN parmSourceId INT,		
		 IN parmFundType  TINYINT(3),		 		 
		 IN parmTaskId CHAR(36),	
		 IN parmBankId INT,		 
		 IN parmPayingAmount DECIMAL(10,2)		 
		 
		 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalCashNew   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select Cash-parmPayingAmount INTO  totalCashLeft from UserBankAccount
 Where Cash>=(parmPayingAmount) and UserId = parmSourceId LOCK IN SHARE MODE;
 
 
	IF parmLendorId !=parmBankId THEN
		 Select Cash+parmPayingAmount INTO  totalCashNew from UserBankAccount
		 Where UserId = parmLendorId LOCK IN SHARE MODE;
	END IF;
  
	IF totalCashLeft>=0 AND parmLendorId>0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashLeft,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmSourceId;

		IF parmLendorId !=parmBankId THEN		
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashNew,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmLendorId;
		END IF;
			
		INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			( UUID(), parmSourceId, parmLendorId, parmPayingAmount, 0, parmFundType, UTC_TIMESTAMP(6));
		
	 Update UserLoan
	SET LeftAmount = LeftAmount- parmPayingAmount,
	PaidAmount = PaidAmount + parmPayingAmount
    WHERE TaskId = parmTaskId; 
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure ApproveLoanRequest
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ApproveLoanRequest`;

DELIMITER $$

CREATE  PROCEDURE `ApproveLoanRequest`(        		 
		 IN parmLendorId INT,		
		 IN parmUserId INT,		
		 IN parmFundType  TINYINT(3),		 		 
		 IN parmTaskId CHAR(36),		 		 
		 IN parmBankId INT,
		 IN parmLoanAmount DECIMAL(10,2)		 
		 
		 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalCashNew   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select Cash+parmLoanAmount INTO  totalCashNew from UserBankAccount
 Where UserId = parmUserId LOCK IN SHARE MODE;
 
 
	IF parmLendorId !=parmBankId THEN
		 Select Cash-parmLoanAmount INTO  totalCashLeft  from UserBankAccount
		 Where Cash>=parmLoanAmount and UserId = parmLendorId LOCK IN SHARE MODE;
	ELSE
		SET totalCashLeft =0;
	END IF;
  
	IF totalCashLeft>=0 AND parmLendorId>0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashNew,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;

		IF parmLendorId !=parmBankId THEN		
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashLeft,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmLendorId;
		END IF;
			
		INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			( UUID(), parmLendorId , parmUserId, parmLoanAmount, 0, parmFundType, UTC_TIMESTAMP(6));
		
		Update UserLoan 
			SET Status = 'A'
			WHERE TaskId = parmTaskId; 
	
					
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateLoanStatus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateLoanStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateLoanStatus`(
         IN  parmTaskId CHAR(36),		 
		 IN  parmStatus CHAR(1)
    )
BEGIN
 Update UserLoan 
	SET Status = parmStatus
    WHERE TaskId = parmTaskId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetLoanListByUserId
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetLoanListByUserId` $$
CREATE PROCEDURE `GetLoanListByUserId` (
	IN parmuserId INT,
	IN parmlastLendorUpdatedAt DATETIME(6),
	IN parmlastBorrowerUpdatedAt DATETIME(6),
	IN parmLimit INT
)

	BEGIN 
		(SELECT 
 t2.UserId ,
t2.Picture,
t2.OnlineStatus,
CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
t2.UserId NextPartyId,
t1.LoanAmount,
t1.MonthlyInterestRate,
t1.PaidAmount,
t1.LeftAmount,
'B' LoanSourceType,
t1.Status,
t1.TaskId,
t1.UpdatedAt,
t1.CreatedAt
		FROM UserLoan t1, WebUser t2
		Where t1.LendorId = t2.UserId 
		AND t1.UserId = parmUserId		
		And (t1.UpdatedAt < parmlastBorrowerUpdatedAt OR parmlastBorrowerUpdatedAt is NULL)
		Order By t1.UpdatedAt desc, t1.UserId
		LIMIT parmLimit)
UNION

		(SELECT 
t2.UserId ,
t2.Picture,
t2.OnlineStatus,
CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
t2.UserId NextPartyId,
t1.LoanAmount,
t1.MonthlyInterestRate,
t1.PaidAmount,
t1.LeftAmount,
'L' LoanSourceType,
t1.Status,
t1.TaskId,
t1.UpdatedAt,
t1.CreatedAt
		FROM UserLoan t1, WebUser t2
		Where t1.UserId = t2.UserId 
		AND t1.LendorId = parmUserId
		AND (t1.UpdatedAt < parmlastLendorUpdatedAt OR parmlastLendorUpdatedAt is NULL)
		Order By t1.UpdatedAt desc, t1.UserId
		LIMIT parmLimit);
		
END$$

DELIMITER ;		
			
-- -----------------------------------------------------
-- procedure GetQuailfiedIntrestedRate
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetQuailfiedIntrestedRate` $$
CREATE PROCEDURE `GetQuailfiedIntrestedRate` (
	IN parmUserId INT
)

BEGIN 
	SELECT 	
	MinMonthlyIntrestRate,
	QualifiedAmount
	From CreditScore t1, CreditInterest t2
	Where t1.UserId = parmUserId
	AND	  t1.Score>= t2.MinScore 
	AND   t1.Score<= t2.MaxScore 
	LIMIT 1	;
		
END$$

DELIMITER ;	
		
-- -----------------------------------------------------
-- procedure GetMerchandiseListByUserId
-- -----------------------------------------------------
DELIMITER $$
DROP procedure IF EXISTS `GetMerchandiseListByUserId`;

CREATE  PROCEDURE `GetMerchandiseListByUserId`(
IN parmUserId INT,
IN parmlastMerchandiseTypeId INT,
IN parmLimit INT
)
BEGIN
Select 
*
 from MerchandiseType t1, UserMerchandise t2
 Where 
		t1.MerchandiseTypeId = t2.MerchandiseTypeId
 AND 	t2.UserId = parmUserId
 AND t2.Quantity > 0
 AND (t2.MerchandiseTypeId < parmlastMerchandiseTypeId or parmlastMerchandiseTypeId =0)
 Order By t2.MerchandiseTypeId desc
 LIMIT parmLimit
 ;
 END$$

DELIMITER ;
		
-- -----------------------------------------------------
-- procedure GetMerchandiseProfile
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetMerchandiseProfile`;
DELIMITER $$
CREATE  PROCEDURE `GetMerchandiseProfile`(
			IN parmUserId INT
)
BEGIN
Select 
t1.MerchandiseTypeId,
t1.Name,
t1.Description,
t1.ImageFont,
t1.Cost,
t2.Quantity,
t2.PurchasedPrice,
t2.MerchandiseCondition
 from MerchandiseType t1, UserMerchandise t2
 Where 
		t1.MerchandiseTypeId = t2.MerchandiseTypeId
 AND 	t2.UserId = parmUserId
 AND t2.Quantity > 0 
 Order By t1.Cost Desc
  ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetMerchandiseSummary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetMerchandiseSummary`;

DELIMITER $$

CREATE  PROCEDURE `GetMerchandiseSummary`(
IN parmUserId INT
)
BEGIN
Select 
MerchandiseTypeCode,
UserId,
Sum(PurchasedPrice) TotalPrice,
Avg(MerchandiseCondition) AverageCondition,
Sum(Quantity) TotalQty
 from MerchandiseType t1, UserMerchandise t2
 Where 
		t1.MerchandiseTypeId = t2.MerchandiseTypeId
 AND 	t2.UserId = parmUserId
 Group By MerchandiseTypeCode
 Order by MerchandiseTypeCode
 ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetMerchandiseTypeList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetMerchandiseTypeList`;

DELIMITER $$

CREATE  PROCEDURE `GetMerchandiseTypeList`(
         IN parmLastMerchandiseTypeId INT,
		 IN parmTypeCode TINYINT(2),
		 IN parmMerchandiseLimit INT
)
BEGIN
Select *
 from MerchandiseType
 Where 
 MerchandiseTypeId > parmLastMerchandiseTypeId
 AND MerchandiseTypeCode = parmTypeCode
 ORDER BY MerchandiseTypeId
 LIMIT parmMerchandiseLimit;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetTopNPropertyOwner
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetTopNPropertyOwner`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNPropertyOwner`(
IN parmLimit INT
)
BEGIN
Select 
t2.UserId,
CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
t2.Picture,
t2.OnlineStatus,
t2.CountryId,
Sum(PurchasedPrice) TotalValue
 from UserMerchandise t1, WebUser t2
 Where 
		t1.UserId = t2.UserId
 AND 	t2.Active = 1
 Group By t1.UserId
 Order by TotalValue desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetTopNWeaponStackCountry
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetTopNWeaponStackCountry`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNWeaponStackCountry`(
IN parmLimit INT
)
BEGIN
Select 
t2.*,
Sum(PurchasedPrice) TotalValue
 from CountryWeapon t1, CountryCode t2
 Where 
		t1.CountryId = t2.CountryId
 Group By t1.CountryId
 Order by TotalValue desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure HasThisMerchandise
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `HasThisMerchandise`;

DELIMITER $$

CREATE  PROCEDURE `HasThisMerchandise`(
         IN  parmMerchandiseIdList TEXT,
		 IN parmuserId INT
    )
BEGIN
 set @concatsql = CONCAT('select count(*) cnt from UserMerchandise where MerchandiseTypeId  in (', parmMerchandiseIdList, ') AND Quantity > 0  AND UserId = ', parmUserId);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetMerchandiseTotal
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `GetMerchandiseTotal`;

DELIMITER $$

CREATE  PROCEDURE `GetMerchandiseTotal`(
         IN  parmMerchandiseIdList TEXT,
		 IN parmuserId INT
    )
BEGIN
 set @concatsql = CONCAT('select Sum(PurchasedPrice) totalPrice from UserMerchandise where MerchandiseTypeId  in (', parmMerchandiseIdList, ') AND Quantity > 0  AND UserId = ', parmUserId);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAllMerchandiseTypeList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetAllMerchandiseTypeList`;

DELIMITER $$

CREATE  PROCEDURE `GetAllMerchandiseTypeList`()
BEGIN
Select 
MerchandiseTypeId,
ResaleRate,
Cost
 from MerchandiseType;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountMerchandiseByQty
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `GetCountMerchandiseByQty`;

DELIMITER $$

CREATE  PROCEDURE `GetCountMerchandiseByQty`(
         IN  parmMerchandiseIdList TEXT,
		 IN parmUserId INT,
		 IN parmQuantity INT
    )
BEGIN
 set @concatsql = CONCAT('select Count(*) cnt from UserMerchandise where MerchandiseTypeId  in (', parmMerchandiseIdList, ') AND Quantity >= ',parmQuantity , '  AND UserId = ', parmUserId);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetTodaysStockPrice
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetTodaysStockPrice`;

DELIMITER $$

CREATE  PROCEDURE `GetTodaysStockPrice`()
BEGIN
Select  StockId, Sum(DealPrice*Unit)/Sum(Unit) Price, Sum(Unit) TotalUnit
FROM StockTradeHistory Where DATE(UpdatedAt)= DATE(UTC_TIMESTAMP()) Group By StockId;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCurrentStock
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCurrentStock`;

DELIMITER $$

CREATE  PROCEDURE `GetCurrentStock`()
BEGIN
Select 
*
FROM Stock
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetStockTradeByUser
-- -----------------------------------------------------

DELIMITER ;

DROP procedure IF EXISTS `GetStockTradeByUser`;

DELIMITER $$

CREATE  PROCEDURE `GetStockTradeByUser`(
         IN parmUserId INT,
		 IN parmLastDateTime DateTime(6),
		 IN parmStockLimit INT
    )
BEGIN
select *
from StockTrade t1 , Stock t2 where 
t1.UserId = parmUserId 
AND t2.StockId= t1.StockId 
AND (t1.UpdatedAt <parmLastDateTime OR parmLastDateTime is NULL)
 ORDER BY t1.UpdatedAt DESC, t1.StockId 
 LIMIT parmStockLimit;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetStockByUser
-- -----------------------------------------------------

DELIMITER ;

DROP procedure IF EXISTS `GetStockByUser`;

DELIMITER $$

CREATE  PROCEDURE `GetStockByUser`(
         IN parmUserId INT,
		 IN parmLastDateTime DateTime(6),
		 IN parmStockLimit INT
    )
BEGIN
select *
from UserStock t1 , Stock t2 where 
t1.UserId = parmUserId 
AND t2.StockId= t1.StockId 
AND t1.PurchasedUnit >0
AND (t1.PurchasedAt <parmLastDateTime OR parmLastDateTime is NULL)
 ORDER BY t1.PurchasedAt DESC, t1.StockId 
 LIMIT parmStockLimit;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountStockByQty
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `GetCountStockByQty`;

DELIMITER $$

CREATE  PROCEDURE `GetCountStockByQty`(
         IN  parmStockIdList TEXT,
		 IN parmUserId INT,
		 IN parmQuantity INT
    )
BEGIN
 set @concatsql = CONCAT('select Count(*) cnt from UserStock where StockId  in (', parmStockIdList, ') AND PurchasedUnit >= ',parmQuantity , '  AND UserId = ', parmUserId);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure TryCancelStockOrder
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `TryCancelStockOrder`;

DELIMITER $$

CREATE  PROCEDURE `TryCancelStockOrder`(
         IN  parmTradeId CHAR(36),
		 IN parmUserId INT
    )
BEGIN
SELECT Status FROM StockTrade WHERE UserId = parmUserId and TradeId = parmTradeId LOCK IN SHARE MODE;
 Update StockTrade 
	SET Status = 'C'
    WHERE UserId = parmUserId and TradeId = parmTradeId and Status !='D'; 
END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure HasThisStock
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `HasThisStock`;

DELIMITER $$

CREATE  PROCEDURE `HasThisStock`(
         IN  parmStockIdList TEXT,
		 IN parmUserId INT
		 
    )
BEGIN
 set @concatsql = CONCAT('select count(*) cnt from UserStock where StockId  in (', parmStockIdList, ') AND PurchasedUnit > 0  AND UserId = ', parmUserId);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure HasThisStockonPendingTrade
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `HasThisStockonPendingTrade`;

DELIMITER $$

CREATE  PROCEDURE `HasThisStockonPendingTrade`(
         IN  parmStockIdList TEXT,
		 IN parmUserId INT
		 
    )
BEGIN
 set @concatsql = CONCAT('select count(*) cnt from StockTrade where StockId  in (', parmStockIdList, ') AND PurchasedUnit > 0  AND UserId = ', parmUserId, '  AND Status =\'P\'');
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetStockSummary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetStockSummary`;

DELIMITER $$

CREATE  PROCEDURE `GetStockSummary`(
IN parmUserId INT
)
BEGIN
Select 
t1.*,
Sum(PurchasedPrice * PurchasedUnit) TotalPurchaseValue,
Sum(PurchasedUnit) * CurrentValue  TotalCurrentValue,
Sum(PurchasedUnit) TotalUnit
 from Stock t1, UserStock t2
 Where 
		t1.StockId = t2.StockId
 AND 	t2.UserId = parmUserId
 Group By t2.StockId
 Order by t2.StockId 
 ;
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetTopNStockOwner
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetTopNStockOwner`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNStockOwner`(
IN parmLimit INT
)
BEGIN
Select 
t2.UserId,
CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
t2.Picture,
t2.OnlineStatus,
t2.CountryId,
Sum(t1.PurchasedPrice * t1.PurchasedUnit) TotalPurchaseValue,
Sum(t3.CurrentValue * t1.PurchasedUnit) TotalCurrentValue
 from UserStock t1, WebUser t2, Stock t3
 Where 
		t1.UserId = t2.UserId
 AND	t1.StockId = t3.StockId
 AND 	t2.Active = 1
 Group By t2.UserId
 Order by TotalCurrentValue desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure ExecuteTradeOrder
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteTradeOrder`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteTradeOrder`(
         IN parmSellerStockId CHAR(36),
		 IN parmStockId SMALLINT(5),
		 IN parmPurchasedUnit INT,
		 IN parmPurchasedPrice INT,
		 IN parmBuyerId INT,
		 IN parmSellerId INT,
		 IN parmTotalValue DECIMAL(50,2),
		 IN parmTaxValue DECIMAL(50,2),		 
		 IN parmTaxCode TINYINT(3),	
		 IN parmFundType  TINYINT(3),		 
		 IN parmBuyerCountryId CHAR(2)
		 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
3- not enoug stock
*/
DECLARE totalBuyerCash_new   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalSellerCash_new   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalSellerStock_new   INT DEFAULT -1 ;
DECLARE result INT DEFAULT 0;
DECLARE guid CHAR(36);
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;

SET guid = UUID();
START TRANSACTION;
 Select Cash-parmTotalValue INTO  totalBuyerCash_new from UserBankAccount
 Where Cash>=parmTotalValue and UserId = parmBuyerId LOCK IN SHARE MODE;
 	IF totalBuyerCash_new>=0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalBuyerCash_new,
			UpdatedAt = UTC_TIMESTAMP(6)
			WHERE `UserId` = parmBuyerId;
			
			Select Cash+parmTotalValue-parmTaxValue INTO  totalSellerCash_new from UserBankAccount
			Where  UserId = parmSellerId LOCK IN SHARE MODE;
			UPDATE `UserBankAccount`
			SET `Cash` = totalSellerCash_new,
			UpdatedAt = UTC_TIMESTAMP(6)
			WHERE `UserId` = parmSellerId;
			
			
			Select PurchasedUnit - parmPurchasedUnit INTO  totalSellerStock_new from UserStock
			WHERE `UserStockId` = parmSellerStockId LOCK IN SHARE MODE;
		
			IF totalSellerStock_new>=0 THEN
				UPDATE `UserStock`
				SET `PurchasedUnit` = totalSellerStock_new				
				WHERE `UserStockId` = parmSellerStockId;
				SET result = 1;
			ELSE
				SET result = 3;				
			END IF;
			
			INSERT INTO `UserStock`
			(`UserStockId`, `UserId`,`StockId`,`PurchasedUnit`, PurchasedPrice, `PurchasedAt`)VALUES
			(guid, parmBuyerId, parmStockId, parmPurchasedUnit, parmPurchasedPrice, UTC_TIMESTAMP(6));
			
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`, `TaxAmount`,`FundType`,`CreatedAT`) VALUES
			(guid, parmBuyerId, parmSellerId, parmTotalValue, parmTaxValue, parmFundType, UTC_TIMESTAMP(6));
			
			INSERT INTO `CountryRevenue`
			(`TaskId`,`CountryId`, `Cash`,`Status`,`TaxType`, `UpdatedAt`)VALUES
			(guid, parmBuyerCountryId, parmTaxValue, 'P', parmTaxCode, UTC_TIMESTAMP(6));
		
				IF (result <> 1) THEN
					ROLLBACK;
				ELSE
					COMMIT;
				END IF;
		
			
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;
-- -------------
-- -----------------------------------------------------
-- procedure GetPendingStockTrade
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPendingStockTrade`;

DELIMITER $$

CREATE  PROCEDURE `GetPendingStockTrade`()
BEGIN
Select 
*
FROM StockTrade WHERE  Status IN ('P', 'I') Order BY RequestedAt asc
 ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure DeleteStockTrade
-- -----------------------------------------------------
DROP procedure IF EXISTS `DeleteStockTrade`;

DELIMITER $$

CREATE  PROCEDURE `DeleteStockTrade`(
		IN parmTradeId TEXT
		)

BEGIN
 set @concatsql = CONCAT('DELETE FROM StockTrade WHERE  TradeId  in (', parmTradeId, ')');
	 PREPARE q FROM @concatsql;
	execute q;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure DeleteReadNotification
-- -----------------------------------------------------


DROP procedure IF EXISTS `DeleteReadNotification`;

DELIMITER $$

CREATE  PROCEDURE `DeleteReadNotification`(
IN parmNotificationId CHAR(36) 
)
BEGIN
  Delete  FROM UserNotification 
	    WHERE NotificationId = parmNotificationId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure DeleteAllNotification
-- -----------------------------------------------------


DROP procedure IF EXISTS `DeleteAllNotification`;

DELIMITER $$

CREATE  PROCEDURE `DeleteAllNotification`(
IN parmUserId INT
)
BEGIN
  Delete  FROM UserNotification 
	    WHERE UserId = parmUserId; 
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetNewUserNotificationList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetNewUserNotificationList`;

DELIMITER $$

CREATE  PROCEDURE `GetNewUserNotificationList`(
	IN parmuserId INT,
	IN parmRecentNotificationId CHAR(36),
	IN parmRecentUpdatedAt DATETIME(6)
	
)
BEGIN
Select 
*
		FROM UserNotification t1, NotificationType t2 WHERE
		t1.UserId = parmuserId		
		AND t1.NotificationTypeId = t2.NotificationTypeId
		And (t1.NotificationId != parmRecentNotificationId Or parmRecentNotificationId is NULL)
		And (t1.UpdatedAt > parmRecentUpdatedAt OR parmRecentUpdatedAt is NULL)
		Order By t1.UpdatedAt desc;		
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetOldUserNotificationList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetOldUserNotificationList`;

DELIMITER $$

CREATE  PROCEDURE `GetOldUserNotificationList`(
	IN parmuserId INT,
	IN parmLastNotificationId CHAR(36),
	IN parmLastUpdatedAt DATETIME(6),
	IN parmLimit INT
)
BEGIN
Select 
*
		FROM UserNotification t1, NotificationType t2 WHERE
		t1.UserId = parmuserId		
		AND t1.NotificationTypeId = t2.NotificationTypeId
		And (t1.NotificationId != parmlastNotificationId Or parmlastNotificationId is NULL)
		And (t1.UpdatedAt < parmLastUpdatedAt OR parmLastUpdatedAt is NULL)
		Order By t1.UpdatedAt desc
		LIMIT parmLimit;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetEducationByUserId
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetEducationByUserId`;

DELIMITER $$

CREATE  PROCEDURE `GetEducationByUserId`(
         IN  parmUserId INT         
    )
BEGIN
 SELECT  t1.*, t2.MajorName, t2.ImageFont, t2.Description, t2.JobProbability, t2.MajorRank, t2.Duration, t3.*
    FROM Education t1, MajorCode t2, DegreeCode t3
    WHERE UserId = parmUserId
	AND t1.MajorId = t2.MajorId
	AND t1.DegreeId = t3.DegreeId; 
	
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetEducationSummary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetEducationSummary`;

DELIMITER $$

CREATE  PROCEDURE `GetEducationSummary`(
         IN  parmUserId INT         
    )
BEGIN
 SELECT  t2.*, t1.Status, count(*) Total
    FROM Education t1, DegreeCode t2
    WHERE UserId = parmUserId
	AND t1.DegreeId = t2.DegreeId
	GROUP BY t1.Status, t1.DegreeId
	;
	
	
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetTopNDegreeHolder
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetTopNDegreeHolder`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNDegreeHolder`(
IN parmLimit INT
)
BEGIN
Select 
t2.UserId,
CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
t2.Picture,
t2.OnlineStatus,
t2.CountryId,
Sum(t3.DegreeRank) TotalScore
 from Education t1, WebUser t2, DegreeCode t3
 Where 
		t1.UserId = t2.UserId
 AND 	t2.Active = 1
 AND	t1.DegreeId = t3.DegreeId
 Group By t2.UserId
 Order by TotalScore desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCountryPopulation
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetCountryPopulation`;

DELIMITER $$

CREATE  PROCEDURE `GetCountryPopulation`(
IN parmCountryId CHAR(2)
)
BEGIN
Select 
Count(*) TotalPopulation
from WebUser t1
Where 
		t1.CountryId = parmCountryId;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetRandomWebUsers
-- -----------------------------------------------------


DELIMITER $$
DROP procedure IF EXISTS `GetRandomWebUsers`;

CREATE  PROCEDURE `GetRandomWebUsers`(
IN parmLimit INT,
IN parmCountryId CHAR(2)
)
BEGIN
Select 
*
 from WebUser t1
 Where 
		t1.CountryId = parmCountryId
 Order By RAND()
 LIMIT parmLimit
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure ExecuteUpdateUserBankAc
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteUpdateUserBankAc`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteUpdateUserBankAc`(         
		 IN parmUserId INT,		 
		 IN parmDelta DECIMAL(50,2)		 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalBuyerCash_new   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 SET result = 0;
 IF parmDelta<0 THEN
  Select Cash + parmDelta INTO  totalBuyerCash_new from UserBankAccount
	Where Cash>=ABS(parmDelta) and UserId = parmUserId LOCK IN SHARE MODE;  
  ELSE
  Select Cash + parmDelta INTO  totalBuyerCash_new from UserBankAccount
	Where UserId = parmUserId LOCK IN SHARE MODE;    
 END IF;
 IF totalBuyerCash_new>=0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalBuyerCash_new,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure SearchJob
-- -----------------------------------------------------

DROP procedure IF EXISTS `SearchJob`;

DELIMITER $$

CREATE  PROCEDURE `SearchJob`(
         IN  parmJobType TEXT ,
		 IN  parmIndustryId TEXT ,
		 IN  parmMajorId TEXT ,
		 IN  parmCountryId CHAR(2) ,
		 IN  parmSalaryRangeUp DECIMAL(50,2) ,
		 IN  parmlastJobCodeId INT ,
		 IN  parmLimit INT ,
		 IN  parmOverTime INT ,
		 IN  parmSalaryRangeDown DECIMAL(50,2) 
		 
    )
BEGIN


  set @concatsql = CONCAT(' Select t2.*, t1.Salary, 
  t3.MajorName ReqMajorName,
  t5.DegreeName ReqDegreeName,
  t7.IndustryName, t7.ImageFont  
  FROM  JobCountry t1, JobCode t2, MajorCode t3, DegreeCode t5, IndustryCode t7
  WHERE 
  t1.JobCodeId = t2.JobCodeId
  AND t2.ReqMajorId = t3.MajorId
  AND t2.ReqDegreeId = t5.DegreeId
  AND t2.IndustryId = t7.IndustryId  
  AND ( t2.IndustryId IN( ', parmIndustryId, ' )) 
  AND ( t2.ReqMajorId IN( ', parmMajorId, ' ))   
  AND ( t2.JobType IN( ', parmJobType, ' )) 
  AND (t1.QuantityAvailable >= 1)
  AND (t1.Salary >= ', parmSalaryRangeDown ,')
  AND (t1.Salary <= ', parmSalaryRangeUp , ' OR ', parmSalaryRangeUp,' =0) 
  AND (t1.CountryId = \'', parmCountryId , '\' )
  AND (t1.JobCodeId > ', parmlastJobCodeId , ' )
  AND (t2.OverTimeRate > ', parmOverTime , ' OR ', parmOverTime ,'= -1)
  Order BY JobCodeId ASC
  LIMIT ' , parmLimit
  );

PREPARE q FROM @concatsql;
execute q;
	
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCurrentJobs
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCurrentJobs`;

DELIMITER $$

CREATE  PROCEDURE `GetCurrentJobs`(
         IN parmUserId INT		 
    )
BEGIN
  Select t1.*, t2.*, t3.* FROM  UserJob t1, JobCode t2, IndustryCode t3
  WHERE 
  t1.JobCodeId = t2.JobCodeId
AND t2.IndustryId = t3.IndustryId  
 AND t1.UserId = parmUserId
 AND t1.Status ='A'
 AND t1.EndDate >UTC_TIMESTAMP()
 ;
	
	
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetCurrentJobsTotalHPW
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCurrentJobsTotalHPW`;

DELIMITER $$

CREATE  PROCEDURE `GetCurrentJobsTotalHPW`(
         IN parmUserId INT		 
    )
BEGIN
  Select SUM(t2.MaxHPW) TotalMaxHPW FROM  UserJob t1, JobCode t2 
  WHERE 
  t1.JobCodeId = t2.JobCodeId
 AND t1.UserId = parmUserId
 AND t1.Status ='A'
 AND t1.EndDate >UTC_TIMESTAMP()
 GROUP BY t1.UserId
 ;
	
	
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure QuitJob
-- -----------------------------------------------------


DROP procedure IF EXISTS `QuitJob`;

DELIMITER $$

CREATE  PROCEDURE `QuitJob`(
		 IN parmTaskId CHAR(36)	,
		 IN parmUserId INT
    )
BEGIN
  UPDATE UserJob t1
  SET Status = 'Q',
  EndDate =UTC_TIMESTAMP()  
  Where TaskId= parmTaskId
	 AND t1.UserId = parmUserId
	 AND Status ='A';
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetUserJob
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetUserJob`;

DELIMITER $$

CREATE  PROCEDURE `GetUserJob`(
		 IN parmTaskId CHAR(36)	,
		 IN parmUserId INT
    )
BEGIN
  SELECT * from UserJob
  Where TaskId= parmTaskId
	 AND UserId = parmUserId;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetJobProfile
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetJobProfile`;

DELIMITER $$

CREATE  PROCEDURE `GetJobProfile`(
         IN parmUserId INT		 
    )
BEGIN
  Select t1.StartDate, 
		 t1.EndDate,
		 t1.Salary,
		 t1.IncomeYearToDate,
		 t2.Title,
		 t2.OverTimeRate,
		 CASE t2.JobType
			WHEN 'F' THEN 'FullTime'
			WHEN 'P' THEN 'PartTime'
			WHEN 'C' THEN 'Contract'
		 END AS JobTypeName,
		 t3.IndustryName,
	     t3.ImageFont

 FROM  UserJob t1, JobCode t2, IndustryCode t3
  WHERE 
  t1.JobCodeId = t2.JobCodeId
  AND t2.IndustryId = t3.IndustryId  
 AND t1.UserId = parmUserId  
 AND t1.Status ='A'
 ORDER BY t1.UpdatedAt DESC, t1.JobCodeId 
  ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetJobHistory
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetJobHistory`;

DELIMITER $$

CREATE  PROCEDURE `GetJobHistory`(
         IN parmLastDateTime DateTime,
		 IN parmJobimit INT,
		 IN parmUserId INT		 
    )
BEGIN
  Select t1.*, t2.*, t3.* FROM  UserJob t1, JobCode t2, IndustryCode t3
  WHERE 
  t1.JobCodeId = t2.JobCodeId
  AND t2.IndustryId = t3.IndustryId  
 AND t1.UserId = parmUserId 
 AND (t1.UpdatedAt <parmLastDateTime OR parmLastDateTime is NULL)
 ORDER BY t1.UpdatedAt DESC, t1.JobCodeId 
  LIMIT parmJobimit;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetTopNIncomeSalary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetTopNIncomeSalary`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNIncomeSalary`(
         IN parmLimit INT	 
    )
BEGIN
Select 
t2.UserId,
CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
t2.Picture,
t2.OnlineStatus,
t2.CountryId,
Sum(IncomeYearToDate) TotalSalary
 from UserJob t1, WebUser t2
 Where 
		t1.UserId = t2.UserId
 AND 	t2.Active = 1
 Group By t1.UserId
 Order by TotalSalary desc
 Limit parmLimit
 ;
	
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetJobSummary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetJobSummary`;

DELIMITER $$

CREATE  PROCEDURE `GetJobSummary`(
         IN parmUserId INT	 
    )
BEGIN
Select 
t1.Status, Count(*) Total
 from UserJob t1
 Where 
 t1.UserId =parmUserId

 Group By t1.Status

 
 ;
	
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetJobSummaryTotal
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetJobSummaryTotal`;

DELIMITER $$

CREATE  PROCEDURE `GetJobSummaryTotal`(
         IN parmUserId INT	 
    )
BEGIN
Select 
Sum(Amount) TotalIncomeTaxYTD,
Sum(Tax) TotalIncomeTaxYTD
  From JobPayCheck t2 Where UserId = parmUserId ;
	
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure CurrentOrAppliedJob
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `CurrentOrAppliedJob`;

DELIMITER $$

CREATE  PROCEDURE `CurrentOrAppliedJob`(
         IN  parmJobCodeList TEXT,
		 IN  parmJobCodeStatusList TEXT,
		 IN parmUserId INT
    )
BEGIN
 set @concatsql = CONCAT('select count(*) cnt from UserJob where JobCodeId  in (', parmJobCodeList, ') AND EndDate>= UTC_TIMESTAMP() AND StartDate<=UTC_TIMESTAMP() AND Status  IN ( ', parmJobCodeStatusList, ' )   AND UserId = ', parmUserId);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure HasPendingOrOpenOfferForSameJob
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `HasPendingOrOpenOfferForSameJob`;

DELIMITER $$

CREATE  PROCEDURE `HasPendingOrOpenOfferForSameJob`(
         IN  parmJobCodeList TEXT,
		 IN  parmJobCodeStatusList TEXT,
		 IN parmUserId INT
    )
BEGIN
 set @concatsql = CONCAT('select t2.* from UserJob t1, JobCode t2 where 
	t1.JobCodeId= t2.JobCodeId
	AND t1.JobCodeId  in (', parmJobCodeList, ')
	AND t1.Status  IN ( ', parmJobCodeStatusList, ' )  
	AND t1.UserId = ', parmUserId);
PREPARE q FROM @concatsql;
execute q;
END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetJobCodeAndSalary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetJobCodeAndSalary`;

DELIMITER $$

CREATE  PROCEDURE `GetJobCodeAndSalary`(
		 IN parmJobCodeId smallint(5),
		 IN parmCountryId CHAR(2)
		 
    )
BEGIN
  SELECT t1.*, t2.* FROM JobCode t1, JobCountry t2 
  WHERE
  t1.JobCodeId = t2.JobCodeId
  AND t1.JobCodeId = parmJobCodeId 
  AND t2.CountryId = parmCountryId
  ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WithDrawJob
-- -----------------------------------------------------


DROP procedure IF EXISTS `WithDrawJob`;

DELIMITER $$

CREATE  PROCEDURE `WithDrawJob`(
		 IN parmTaskId CHAR(36)	,
		 IN parmUserId INT
    )
BEGIN
  UPDATE UserJob t1
  SET Status = 'W',
  
  EndDate =UTC_TIMESTAMP()  
  Where TaskId= parmTaskId
	 AND t1.UserId = parmUserId
	 AND Status ='P';
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SearchParty
-- -----------------------------------------------------

DROP procedure IF EXISTS `SearchParty`;

DELIMITER $$

CREATE  PROCEDURE `SearchParty`(
         IN  parmAgendaType TEXT ,
		 IN  parmPartySizeRangeUp INT,
		 IN  parmPartySizeRangeDown INT,
		 IN  parmPartyVictoryRangeUp INT,
		 IN  parmPartyVictoryRangeDown INT,
		 IN  parmPartyFeeRangeUp DECIMAL(10,2),
		 IN  parmPartyFeeRangeDown DECIMAL(10,2),
		 IN  parmPartyWorthRangeUp DECIMAL(60,2),
		 IN  parmPartyWorthRangeDown DECIMAL(60,2),		 
		 IN  parmlastStartDate DATETIME,
		 IN  parmLimit INT 
    )
BEGIN


  set @concatsql = CONCAT(' Select DISTINCT t1.*
  
  FROM  PoliticalParty t1
 LEFT JOIN  PartyAgenda t2
 ON t1.PartyId = t2.PartyId 
  WHERE   
  ( t2.AgendaTypeId IN( ', parmAgendaType, ' ))   
  AND (t1.PartySize >= ', parmPartySizeRangeDown ,')
  AND (t1.PartySize <= ', parmPartySizeRangeUp , ' OR ', parmPartySizeRangeUp,' = 0 )
  AND (t1.ElectionVictory >= ', parmPartyVictoryRangeDown ,' )
  AND (t1.ElectionVictory <= ', parmPartyVictoryRangeUp ,' OR ',parmPartyVictoryRangeUp,'  = 0)
  AND (t1.MembershipFee >= ', parmPartyFeeRangeDown ,' )
  AND (t1.MembershipFee <= ', parmPartyFeeRangeUp , ' OR ',parmPartyFeeRangeUp ,' = 0.00)
  AND (t1.TotalValue >= ', parmPartyWorthRangeDown ,' )
  AND (t1.TotalValue <= ', parmPartyWorthRangeUp , ' OR ',parmPartyWorthRangeUp,'  = 0.00)
  AND (t1.StartDate > \'', parmlastStartDate , '\' )  
  AND t1.Status in ("A","P")
  AND t1.EndDate is NULL
  Order BY t1.StartDate asc
  LIMIT ' , parmLimit
  );
PREPARE q FROM @concatsql;
execute q;
	
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure DeleteAgenda
-- -----------------------------------------------------


DROP procedure IF EXISTS `DeleteAgenda`;

DELIMITER $$

CREATE  PROCEDURE `DeleteAgenda`(
IN parmPartyId CHAR(36) ,
IN parmAgendaTypeId SMALLINT(3)
)
BEGIN
  Delete  FROM PartyAgenda 
	    WHERE PartyId = parmPartyId
		AND AgendaTypeId = parmAgendaTypeId; 
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetCurrentParty
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetCurrentParty`;

DELIMITER $$

CREATE  PROCEDURE `GetCurrentParty`(		 
		 IN parmUserId INT		 
    )
BEGIN
  SELECT t1.*, t2.* FROM PartyMember t1, PoliticalParty t2 
  WHERE
  t1.PartyId = t2.PartyId
  AND t1.UserId = parmUserId 
  AND (t1.MemberEndDate =0 or t1.MemberEndDate='0001-01-01 00:00:00' or t1.MemberEndDate is NULL)   
  AND (t2.EndDate =0 or t2.EndDate='0001-01-01 00:00:00' or t2.EndDate is NULL);  
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateMemberNomination
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateMemberNomination`;

DELIMITER $$

CREATE  PROCEDURE `UpdateMemberNomination`(
IN parmMemberStatus Char(1),
IN parmUserId INT,
IN parmPartyId CHAR(36) 
)
BEGIN
UPDATE PartyMember t1
SET MemberStatus =parmMemberStatus
WHERE  
 t1.UserId =parmUserId
 AND t1.PartyId = parmPartyId
   AND (t1.MemberEndDate =0 or t1.MemberEndDate='0001-01-01 00:00:00' or t1.MemberEndDate is NULL)  
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdatePartyJoinRequest
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdatePartyJoinRequest`;

DELIMITER $$

CREATE  PROCEDURE `UpdatePartyJoinRequest`(
IN parmStatus Char(1),
IN parmUserId INT,
IN parmPartyId CHAR(36) 

)
BEGIN
UPDATE PartyJoinRequest t1
SET Status =parmStatus
WHERE  
 t1.UserId =parmUserId
 AND t1.PartyId = parmPartyId 
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetPastParty
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPastParty`;

DELIMITER $$

CREATE  PROCEDURE `GetPastParty`(		 
		 IN parmUserId INT		 
    )
BEGIN
  SELECT t1.*, t2.* FROM PartyMember t1, PoliticalParty t2 
  WHERE
  t1.PartyId = t2.PartyId
  AND t1.UserId = parmUserId 
  AND (t1.MemberEndDate <=UTC_TIMESTAMP()  and t1.MemberEndDate !=0)    ;  
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetAllUserParty
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetAllUserParty`;

DELIMITER $$

CREATE  PROCEDURE `GetAllUserParty`(		 
		 IN parmUserId INT		 
    )
BEGIN
  SELECT t1.MemberType,
  t1.MemberEndDate,
  t1.MemberStartDate,
  t2.PartyName,
  t2.TotalValue,
  t2.PartySize,
  t2.LogoPictureId,
  t2.Motto,
  t2.PartyId,
  t2.CountryId
  FROM PartyMember t1, PoliticalParty t2 
  WHERE
  t1.PartyId = t2.PartyId
  AND t1.UserId = parmUserId  
  Order By t1.MemberStartDate DESC
  ;  
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetActiveUserParty
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetActiveUserParty`;

DELIMITER $$

CREATE  PROCEDURE `GetActiveUserParty`(		 
		 IN parmUserId INT		 
    )
BEGIN
  SELECT t1.* FROM PartyMember t1
  WHERE
   t1.UserId = parmUserId 
  AND (t1.MemberEndDate =0 or t1.MemberEndDate='0001-01-01 00:00:00' or t1.MemberEndDate is NULL)  ;  
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure IsCurrentOrPastParty
-- -----------------------------------------------------


DROP procedure IF EXISTS `IsCurrentOrPastParty`;

DELIMITER $$

CREATE  PROCEDURE `IsCurrentOrPastParty`(
		 IN parmPartyId CHAR(36)	,
		 IN parmUserId INT
    )
BEGIN
SELECT Count(*) cnt FROM PartyMember t1
  WHERE
  t1.PartyId = parmPartyId
AND t1.UserId =  parmUserId ;
  END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetPartyByMemberType
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPartyByMemberType`;

DELIMITER $$

CREATE  PROCEDURE `GetPartyByMemberType`(		 
		 IN parmPartyId CHAR(36) ,		 
		 IN parmMemberType CHAR(1)		 
    )
BEGIN
  SELECT t1.* FROM PartyMember t1
  WHERE
  t1.PartyId = parmPartyId
  AND t1.MemberType = parmMemberType
   AND (t1.MemberEndDate =0 or t1.MemberEndDate='0001-01-01 00:00:00' or t1.MemberEndDate is NULL)  ;  
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetPendingNomination
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPendingNomination`;

DELIMITER $$

CREATE  PROCEDURE `GetPendingNomination`(
		 IN parmPartyId CHAR(36)	,
		 IN parmUserId INT
    )
BEGIN
SELECT t1.* FROM PartyNomination t1
  WHERE
  t1.PartyId = parmPartyId
  AND t1.NomineeId = parmUserId
  AND t1.Status='P';    
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure EjectPartyMember
-- -----------------------------------------------------


DROP procedure IF EXISTS `EjectPartyMember`;

DELIMITER $$

CREATE  PROCEDURE `EjectPartyMember`(
		 IN parmPartyId CHAR(36)	,
		 IN parmStatus CHAR(1)	,
		 IN parmUserId INT
    )
BEGIN
  UPDATE PartyMember t1
  SET MemberStatus = parmStatus,
  MemberEndDate =UTC_TIMESTAMP()  
  Where t1.PartyId= parmPartyId
	 AND t1.UserId = parmUserId	 ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecuteDonateParty
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteDonateParty`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteDonateParty`(         
		 IN parmUserId INT,		 
		 IN parmPartyId CHAR(36),		
		 IN parmFundType  TINYINT(3),
		 IN parmAmount DECIMAL(50,2)		 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalCash_new DECIMAL(60,2) DEFAULT 0 ;
DECLARE totalCashUser_new DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalDonation_new DECIMAL(50,2) DEFAULT 0 ;
DECLARE result INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select TotalValue + parmAmount INTO  totalCash_new from PoliticalParty
 Where PartyId = parmPartyId LOCK IN SHARE MODE;
 
  Select Cash-parmAmount INTO  totalCashUser_new from UserBankAccount
 Where Cash>=parmAmount and UserId = parmUserId LOCK IN SHARE MODE;
 
  Select DonationAmount + parmAmount INTO  totalDonation_new from PartyMember
 Where PartyId = parmPartyId AND UserId = parmUserId LOCK IN SHARE MODE;
 	IF totalCashUser_new>=0 THEN
	
			UPDATE `PoliticalParty`
			SET `TotalValue` = totalCash_new
			WHERE `PartyId` = parmPartyId;
			
			UPDATE `PartyMember`
			SET `DonationAmount` = totalDonation_new
			WHERE PartyId = parmPartyId AND UserId = parmUserId;

					UPDATE `UserBankAccount`
			SET `Cash` = totalCashUser_new,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;
						
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentGuid`,`Amount`,`FundType`,`CreatedAT`) VALUES
			(UUID(), parmUserId, parmPartyId, parmAmount, parmFundType, UTC_TIMESTAMP(6));
		
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecuteJoinPartyFee
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteJoinPartyFee`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteJoinPartyFee`(         
		 IN parmUserId INT,		 
		 IN parmPartyId CHAR(36),		
		 IN parmFundType  TINYINT(3),
		 IN parmAmount DECIMAL(50,2)		 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalCash_new DECIMAL(60,2) DEFAULT 0 ;
DECLARE totalCashUser_new DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalDonation_new DECIMAL(50,2) DEFAULT 0 ;
DECLARE result INT DEFAULT 0;
DECLARE newpartySize INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select TotalValue + parmAmount, PartySize+1 INTO  totalCash_new, newpartySize from PoliticalParty
 Where PartyId = parmPartyId LOCK IN SHARE MODE;
 
  Select Cash-parmAmount INTO  totalCashUser_new from UserBankAccount
 Where Cash>=parmAmount and UserId = parmUserId LOCK IN SHARE MODE;
 
 	IF totalCashUser_new>=0 THEN
	
			UPDATE `PoliticalParty`
			SET `TotalValue` = totalCash_new,
				`PartySize` = newpartySize
			WHERE `PartyId` = parmPartyId;
			

			UPDATE `UserBankAccount`
			SET `Cash` = totalCashUser_new,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;
						
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentGuid`,`Amount`,`FundType`,`CreatedAT`) VALUES
			(UUID(), parmUserId, parmPartyId, parmAmount, parmFundType, UTC_TIMESTAMP(6));
		
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecutePartyPayment
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePartyPayment`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePartyPayment`(         
		 IN parmUserId INT,		 
		 IN parmPartyId CHAR(36),		
		 IN parmFundType  TINYINT(3),
		 IN parmAmount DECIMAL(50,2)		 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/

DECLARE totalCashUser_new   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;

DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
  
 Select TotalValue-parmAmount INTO  totalCashLeft from PoliticalParty
 Where TotalValue>=parmAmount and PartyId = parmPartyId LOCK IN SHARE MODE;
 
 Select Cash+parmAmount INTO  totalCashUser_new from UserBankAccount
 Where Cash>=parmAmount and UserId = parmUserId LOCK IN SHARE MODE;
 
 
 	IF totalCashLeft>0 THEN
	
		UPDATE `UserBankAccount`
			SET `Cash` = totalCashUser_new,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;

		UPDATE `PoliticalParty`
			SET `TotalValue` = totalCashLeft
			WHERE PartyId = parmPartyId;
			
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceGuid`,`RecipentId`,`Amount`,`FundType`,`CreatedAT`) VALUES
			(UUID(), parmPartyId, parmUserId, parmAmount, parmFundType, UTC_TIMESTAMP(6));
		
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetUserPartyInviteSummary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetUserPartyInviteSummary`;

DELIMITER $$

CREATE  PROCEDURE `GetUserPartyInviteSummary`(
IN parmUserId INT
)
BEGIN
Select 
t1.MemberType, Count(*) TotalCount
 from PartyInvite t1
 Where 		
	t1.UserId = parmUserId
 Group By t1.MemberType 
 ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetUserPartyMemberSummary
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetUserPartyMemberSummary`;

DELIMITER $$

CREATE  PROCEDURE `GetUserPartyMemberSummary`(
IN parmUserId INT
)
BEGIN
Select 
Sum(DonationAmount) TotalDonation,
Count(*) TotalParties
 from PartyMember t1
 Where 		
	t1.UserId = parmUserId
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetTopNPartyByMember
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetTopNPartyByMember`;

DELIMITER $$

CREATE  PROCEDURE `GetTopNPartyByMember`(
IN parmLimit INT
)
BEGIN
Select 
t1.*
 from PoliticalParty t1
 Where 
		t1.Status = 'A'
 
 Order by t1.PartySize desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdatePartyStatus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdatePartyStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdatePartyStatus`(
		 IN parmPartyId CHAR(36)	,
		 IN parmPartyStatus CHAR(1)	
    )
BEGIN
  UPDATE PoliticalParty t1
  SET Status = parmPartyStatus 
  Where t1.PartyId= parmPartyId	 ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateClosePartyStatus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateClosePartyStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateClosePartyStatus`(
		 IN parmTaskId CHAR(36)	,
		 IN parmPartyStatus CHAR(1)	
    )
BEGIN
  UPDATE PartyCloseRequest t1
  SET Status = parmPartyStatus 
  Where t1.TaskId= parmTaskId	 ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdatePartySizeStatus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdatePartySizeStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdatePartySizeStatus`(
		 IN parmPartyId CHAR(36)	,
		 IN parmPartyStatus CHAR(1),	
		 IN parmPartySize INT
    )
BEGIN
  UPDATE PoliticalParty t1
  SET Status = parmPartyStatus ,
  PartySize = parmPartySize
  Where t1.PartyId= parmPartyId	 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateManagePartyLogo
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateManagePartyLogo`;

DELIMITER $$

CREATE  PROCEDURE `UpdateManagePartyLogo`(
		 IN parmPartyId CHAR(36)	,		 
		 IN parmLogoPictureId VARCHAR(250),
		 IN parmPartyFounder int(11)
    )
BEGIN
UPDATE PoliticalParty SET 
				LogoPictureId = parmLogoPictureId			
		WHERE
			PartyId = parmPartyId 
		AND PartyFounder = parmPartyFounder;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateManageParty
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateManageParty`;

DELIMITER $$

CREATE  PROCEDURE `UpdateManageParty`(
		 IN parmPartyId CHAR(36)	,		 
		 IN parmMotto varchar(250),
		 IN parmMembershipFee decimal(10,2),		 
		 IN parmPartyName varchar(60),
		 IN parmPartyFounder int(11)
    )
BEGIN
UPDATE PoliticalParty SET 
				PartyName = parmPartyName,				
				MembershipFee = parmMembershipFee,
				Motto = parmMotto				
		WHERE
			PartyId = parmPartyId 
		AND PartyFounder = parmPartyFounder;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure CloseParty
-- -----------------------------------------------------


DROP procedure IF EXISTS `CloseParty`;

DELIMITER $$

CREATE  PROCEDURE `CloseParty`(
		 IN parmPartyId CHAR(36)	
    )
BEGIN
  UPDATE PoliticalParty t1
  SET Status = 'C',
  EndDate =UTC_TIMESTAMP() 
  Where t1.PartyId= parmPartyId	 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetEmailInvitationList
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetEmailInvitationList`;

DELIMITER $$

CREATE  PROCEDURE `GetEmailInvitationList`(		 
		 IN parmUserId INT ,
		 IN parmLimit INT ,
		 IN parmLastFriendEmailId VARCHAR(100) 
    )
BEGIN
  SELECT 
  t1.FriendEmailId,  
 COALESCE(TRIM(CONCAT( t1.NameFirst, ' ',  t1.NameLast)) , '') FullName
  FROM WebUserContact t1
  WHERE
  t1.UserId = parmUserId  
  AND (t1.FriendEmailId > parmLastFriendEmailId OR parmLastFriendEmailId is NULL or parmLastFriendEmailId='')
  AND FriendUserId =0
  AND Unsubscribe =0
  ORDER BY FriendEmailId
  LIMIT parmLimit
  ;    
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetPoliticalPostionsList
-- -----------------------------------------------------
DROP procedure IF EXISTS `GetPoliticalPostionsList`;

DELIMITER $$

CREATE  PROCEDURE `GetPoliticalPostionsList`()
BEGIN
  SELECT t1.* FROM ElectionPosition t1;  
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetPartyAgenda
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPartyAgenda`;

DELIMITER $$

CREATE  PROCEDURE `GetPartyAgenda`(		 
		 IN parmPartyId CHAR(36)		 
    )
BEGIN
  SELECT t1.* FROM PartyAgenda t1
  WHERE
  t1.PartyId = parmPartyId  ;  
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAllPoliticalAgenda
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetAllPoliticalAgenda`;

DELIMITER $$

CREATE  PROCEDURE `GetAllPoliticalAgenda`(   )
BEGIN
  SELECT t1.* FROM ElectionAgenda t1    ;  
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetPartyMember
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPartyMember`;

DELIMITER $$

CREATE  PROCEDURE `GetPartyMember`(		 
		 IN parmPartyId CHAR(36)		 ,
		 IN parmLastStartDate DateTime ,
		 IN parmMemberType CHAR(1)		 ,
		 IN parmLimit INT	
    )
BEGIN
  SELECT  t1.MemberType,
  t1.MemberStartDate,
 CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
 t2.Picture ,
 t1.MemberStatus,
 t1.DonationAmount,
 t1.UserId
 FROM PartyMember t1, WebUser t2
  WHERE
  t1.PartyId = parmPartyId  
  AND t1.UserId = t2.UserId
  AND t1.MemberType = parmMemberType
  AND (t1.MemberEndDate =0 or t1.MemberEndDate='0001-01-01 00:00:00' or t1.MemberEndDate is NULL)
  AND (t1.MemberStartDate >  parmLastStartDate or parmLastStartDate IS NULL)
  ORDER BY t1.MemberStartDate, t1.UserId
  LIMIT parmLimit
  ;  
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetAllPartyMember
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetAllPartyMember`;

DELIMITER $$

CREATE  PROCEDURE `GetAllPartyMember`(		 
		 IN parmPartyId CHAR(36)
    )
BEGIN
  SELECT t1.UserId FROM PartyMember t1
  WHERE
  t1.PartyId = parmPartyId  
  AND (t1.MemberEndDate =0 or t1.MemberEndDate='0001-01-01 00:00:00' or t1.MemberEndDate is NULL);
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAllPartyMemberWithNames
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetAllPartyMemberWithNames`;

DELIMITER $$

CREATE  PROCEDURE `GetAllPartyMemberWithNames`(		 
		 IN parmPartyId CHAR(36)
    )
BEGIN
  SELECT t1.MemberType,
  t1.MemberStartDate,
 CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
 t2.Picture 
 
  FROM PartyMember t1, WebUser t2
  WHERE
  t1.PartyId = parmPartyId  
  AND t1.UserId = t2.UserId
  AND t2.Active =1
  AND (t1.MemberEndDate =0 or t1.MemberEndDate='0001-01-01 00:00:00' or t1.MemberEndDate is NULL);
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetPartyNames
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPartyNames`;

DELIMITER $$

CREATE  PROCEDURE `GetPartyNames`(	
 IN parmCountryId CHAR(2)		 	
    )
BEGIN
  SELECT PartyName, CountryId FROM PoliticalParty t1  
  WHERE t1.CountryId=parmCountryId;  
 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure HasPendingJoinRequest
-- -----------------------------------------------------


DROP procedure IF EXISTS `HasPendingJoinRequest`;

DELIMITER $$

CREATE  PROCEDURE `HasPendingJoinRequest`(
		 IN parmPartyId CHAR(36)	,
		 IN parmUserId INT

)
BEGIN
Select 
*
FROM PartyJoinRequest t1
  Where t1.PartyId= parmPartyId
	 AND t1.UserId = parmUserId	
	AND t1.Status ='P'
	  ;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure HasPendingPartyInvite
-- -----------------------------------------------------


DROP procedure IF EXISTS `HasPendingPartyInvite`;

DELIMITER $$

CREATE  PROCEDURE `HasPendingPartyInvite`(
		 IN parmPartyId CHAR(36)	,
		 IN parmUserId INT

)
BEGIN
Select 
*
FROM PartyInvite t1
  Where t1.PartyId= parmPartyId
	 AND t1.UserId = parmUserId	
	AND t1.Status ='P'
	  ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure HasPendingPartyInviteByEmail
-- -----------------------------------------------------


DROP procedure IF EXISTS `HasPendingPartyInviteByEmail`;

DELIMITER $$

CREATE  PROCEDURE `HasPendingPartyInviteByEmail`(
		 IN parmPartyId CHAR(36)	,
		 IN parmEmailId VARCHAR(100)

)
BEGIN
Select 
*
FROM PartyInvite t1
  Where t1.PartyId= parmPartyId
	 AND t1.EmailId = parmEmailId	
	AND t1.Status ='P'
	  ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure PostContentTypeList
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `PostContentTypeList`;

DELIMITER $$

CREATE  PROCEDURE `PostContentTypeList`(  )
BEGIN
SELECT * FROM PostContentType;
END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure NotificationTypeList
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `NotificationTypeList`;

DELIMITER $$

CREATE  PROCEDURE `NotificationTypeList`(  )
BEGIN
SELECT * FROM NotificationType;
END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskTypeList
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `TaskTypeList`;

DELIMITER $$

CREATE  PROCEDURE `TaskTypeList`(  )
BEGIN
SELECT * FROM TaskType;
END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetMyThreePicks
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetMyThreePicks`;

DELIMITER $$

CREATE  PROCEDURE `GetMyThreePicks`(
IN parmUserId INT,
IN parmLimit INT,
IN parmLastDrawingId INT
)
BEGIN
Select 
t1.DrawingId, 
t2.Amount,
t1.Number1,
t1.Number2,
t1.Number3
 from PickThree t1
 LEFT JOIN PickThreeWinner t2 ON t1.DrawingId = t2.DrawingId AND t1.UserId = t2.UserId 
 Where 		
	t1.UserId = parmUserId
		AND ((t1.DrawingId <parmLastDrawingId AND t1.DrawingId >=(parmLastDrawingId -parmLimit ))OR parmLastDrawingId=0)
 ;

 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetMyFivePicks
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetMyFivePicks`;

DELIMITER $$

CREATE  PROCEDURE `GetMyFivePicks`(
IN parmUserId INT,
IN parmLimit INT,
IN parmLastDrawingId INT
)
BEGIN
Select 
t1.DrawingId, 
t2.Amount,
t1.Number1,
t1.Number2,
t1.Number3,
t1.Number4,
t1.Number5
 from PickFive t1
 LEFT JOIN PickFiveWinner t2 ON t1.DrawingId = t2.DrawingId AND t1.UserId = t2.UserId 
 Where 		
	t1.UserId = parmUserId
	AND ((t1.DrawingId <parmLastDrawingId AND t1.DrawingId >=(parmLastDrawingId -parmLimit ))OR parmLastDrawingId=0)
		
 Order by DrawingId desc
;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetSlotMachineThreeList
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetSlotMachineThreeList` $$
CREATE PROCEDURE `GetSlotMachineThreeList` ()

	BEGIN 

		SELECT *
		FROM SlotMachineThree t1 ORDER BY t1.MatchNumber;
		
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetNextLotteryDrawingDate
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetNextLotteryDrawingDate`;

DELIMITER $$

CREATE  PROCEDURE `GetNextLotteryDrawingDate`()
BEGIN
Select * from NextLotteryDrawing ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetPickThreeWinNumber
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPickThreeWinNumber`;

DELIMITER $$

CREATE  PROCEDURE `GetPickThreeWinNumber`(
IN parmLimit INT,
IN parmLastDrawingId INT
)
BEGIN
Select 
*
 from PickThreeWinNumber t1
 Where 
(t1.DrawingId <parmLastDrawingId OR parmLastDrawingId=0)
		
 Order by DrawingId desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetPickFiveWinNumber
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetPickFiveWinNumber`;

DELIMITER $$

CREATE  PROCEDURE `GetPickFiveWinNumber`(
IN parmLimit INT,
IN parmLastDrawingId INT
)
BEGIN
Select 
*
 from PickFiveWinNumber t1
 Where 
(t1.DrawingId <parmLastDrawingId OR parmLastDrawingId=0)
		
 Order by DrawingId desc
 Limit parmLimit
 ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecutePick3LotteryOrder
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePick3LotteryOrder`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePick3LotteryOrder`(
		 IN parmBuyerId INT,
		 IN parmBankId INT,
		 IN parmDrawingId INT,
		 IN parmTotalValue DECIMAL(5,2),
		 IN parmFundType TINYINT(3) ,
		 IN parmNumber1 TINYINT(2) ,
		 IN parmNumber2 TINYINT(2) ,
		 IN parmNumber3 TINYINT(2) 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalBuyerCash_new   DECIMAL(50,2) DEFAULT 0 ;
DECLARE result INT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select Cash-parmTotalValue INTO  totalBuyerCash_new from UserBankAccount
 Where Cash>=parmTotalValue and UserId = parmBuyerId LOCK IN SHARE MODE;
 	IF totalBuyerCash_new>=0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalBuyerCash_new,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmBuyerId;
						
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,`FundType`,`CreatedAT`) VALUES
			(UUID(), parmBuyerId, parmBankId, parmTotalValue, parmFundType, UTC_TIMESTAMP(6));
					
			
			INSERT INTO `PickThree`(`PickThreeId`,`DrawingId`,`UserId`,`Number1`,`Number2`,`Number3`) VALUES 
			(UUID(),parmDrawingId,parmBuyerId,parmNumber1, parmNumber2,parmNumber3);
						
			COMMIT;

			SET result = 1;
			ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecutePick5LotteryOrder
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePick5LotteryOrder`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePick5LotteryOrder`(
		 IN parmBuyerId INT,
		 IN parmBankId INT,
		 IN parmDrawingId INT,
		 IN parmTotalValue DECIMAL(5,2),
		 IN parmFundType TINYINT(3) ,
		 IN parmNumber1 TINYINT(2) ,
		 IN parmNumber2 TINYINT(2) ,
		 IN parmNumber3 TINYINT(2) ,
		 IN parmNumber4 TINYINT(2) ,
		 IN parmNumber5 TINYINT(2) 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalBuyerCash_new   DECIMAL(50,2) DEFAULT 0 ;
DECLARE result INT;

DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select Cash-parmTotalValue INTO  totalBuyerCash_new from UserBankAccount
 Where Cash>=parmTotalValue and UserId = parmBuyerId LOCK IN SHARE MODE;
 	IF totalBuyerCash_new>=0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalBuyerCash_new,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmBuyerId;
						
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,`FundType`,`CreatedAT`) VALUES
			(UUID(), parmBuyerId, parmBankId, parmTotalValue, parmFundType, UTC_TIMESTAMP(6));
					
			
			INSERT INTO `PickFive`(`PickFiveId`,`DrawingId`,`UserId`,`Number1`,`Number2`,`Number3`,`Number4`,`Number5`) VALUES 
			(UUID(),parmDrawingId, parmBuyerId,parmNumber1, parmNumber2,parmNumber3,parmNumber4,parmNumber5);
						
			COMMIT;

			SET result = 1;
			ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetMatchLottoPick5
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetMatchLottoPick5`;

DELIMITER $$

CREATE  PROCEDURE `GetMatchLottoPick5`(         
	   IN parmDrawingId INT ,
	   IN parmNumber1 TINYINT(2)  ,
	   IN parmNumber2 TINYINT(2)  ,
	   IN parmNumber3 TINYINT(2)  ,
	   IN parmNumber4 TINYINT(2)  ,
	   IN parmNumber5 TINYINT(2)   		 
	
    )
BEGIN
 	  Select 5 PickMatch, UserId from PickFive Where DrawingId = parmDrawingId AND Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 
	  union
	  Select 4 PickMatch, UserId from PickFive Where DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 ))
	  union
	  Select 3 PickMatch, UserId from PickFive Where  DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 ))
		union
	  Select 2 PickMatch, UserId from PickFive Where  DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 )||				
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 ))

	union
	  Select 1 PickMatch, UserId from PickFive Where  DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 )||				
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 ))

				;
	
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure DeleteTaskNotComplete
-- -----------------------------------------------------


DROP procedure IF EXISTS `DeleteTaskNotComplete`;

DELIMITER $$

CREATE  PROCEDURE `DeleteTaskNotComplete`(
IN parmTaskId CHAR(36)
)
BEGIN
DELETE FROM UserTask 
WHERE
	TaskId = parmTaskId
AND Status = 'I' ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure ElmahErrorCount
-- -----------------------------------------------------


DROP procedure IF EXISTS `ElmahErrorCount`;

DELIMITER $$

CREATE  PROCEDURE `ElmahErrorCount`()
BEGIN
SELECT count(*) cnt FROM elmah.elmah_error;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetUserIdByEmail
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetUserIdByEmail`;

DELIMITER $$

CREATE  PROCEDURE `GetUserIdByEmail`(		 
		 IN parmEmailId VARCHAR(100)

)
BEGIN
Select 
t1.UserId
FROM WebUser t1
  Where t1.EmailId = parmEmailId		
	  ;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetEmailByUserId
-- -----------------------------------------------------


DROP procedure IF EXISTS `GetEmailByUserId`;

DELIMITER $$

CREATE  PROCEDURE `GetEmailByUserId`(		 
		 IN parmUserId INT

)
BEGIN
Select 
t1.EmailId
FROM WebUser t1
  Where t1.UserId = parmUserId	
	AND Active = 1	 
	  ;
 END$$

DELIMITER ;


 -- -----------------------------------------------------
-- procedure GetPendingOrApprovedElectionApp
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `GetPendingOrApprovedElectionApp` $$
CREATE PROCEDURE `GetPendingOrApprovedElectionApp`
(
 IN parmUserId INT
)

	BEGIN 

		SELECT  count(*) cnt FROM ElectionCandidate WHERE
				(Status = 'P' OR Status = 'A' )
				AND UserId =parmUserId;
			
END$$
DELIMITER ;

 -- -----------------------------------------------------
-- procedure NumberOfApprovedCandidate
-- -----------------------------------------------------

DELIMITER $$
DROP PROCEDURE IF EXISTS `NumberOfApprovedCandidate` $$
CREATE PROCEDURE `NumberOfApprovedCandidate` 
(
 IN parmElectionId INT,
 IN parmCountryId CHAR(2)
 
)

	BEGIN 

		SELECT  count(*) cnt FROM ElectionCandidate WHERE
				Status = 'A'
				AND CountryId = parmCountryId
				AND ElectionID=parmElectionID;
			
END$$
DELIMITER $$

 -- -----------------------------------------------------
-- procedure QuitElection
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `QuitElection` $$
CREATE PROCEDURE `QuitElection` 
(
 IN parmElectionId INT,
 IN parmUserId INT
)
	BEGIN 
UPDATE ElectionCandidate
SET Status = 'Q'
		WHERE UserId= parmUserId
		AND
		ElectionId = parmElectionId;
					
END$$
DELIMITER $$

-- -----------------------------------------------------
-- procedure GetLastNoVoteCountedElectionPeriod
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `GetLastNoVoteCountedElectionPeriod` $$

CREATE  PROCEDURE `GetLastNoVoteCountedElectionPeriod`
()

BEGIN
Select DISTINCT t1.ElectionId, t1.CountryId from ElectionVoting t1, (
  
  SELECT DISTINCT t4.* from Election t4,
	(
	  SELECT Max(EndDate) LastMaxEndDate, CountryId  
	  FROM Election 
	  WHERE   
	   EndDate<UTC_TIMESTAMP() 
	   Group By CountryId
	)t3
	WHERE t3.CountryId = t4.CountryId 
	AND t4.EndDate = t3.LastMaxEndDate
   
   ) t2
  WHERE t1.ElectionId = t2.ElectionId
  AND t1.CountryId = t2.CountryId
  AND ElectionResult =''  
  ;

END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetCurrentElectionPeriod
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `GetCurrentElectionPeriod` $$

CREATE  PROCEDURE `GetCurrentElectionPeriod`
(In parmCountryId CHAR(2))

BEGIN
  SELECT * from Election WHERE UTC_TIMESTAMP BETWEEN StartDate and EndDate AND CountryId=parmCountryId;

END$$
DELIMITER ;
 -- -----------------------------------------------------
-- procedure NumberOfApprovedPartyMember`
-- -----------------------------------------------------
 
 DELIMITER $$
DROP PROCEDURE IF EXISTS `NumberofApprovedPartyMember` $$
CREATE PROCEDURE `NumberOfApprovedPartyMember` 
(IN parmElectionId INT, 
IN parmPartyId CHAR(36)
)

	BEGIN 
		SELECT  count(*) cnt FROM ElectionCandidate
		
WHERE UserId 
IN 
(
		SELECT UserId FROM PartyMember 
	    WHERE PartyId=parmPartyId
	     AND
		 (MemberEndDate =0 or MemberEndDate='0001-01-01 00:00:00' or MemberEndDate is NULL)   
		  AND 
		  Status = 'A');
	
END$$
DELIMITER $$

-- -----------------------------------------------------
-- procedure ExecutePayNationWithOutput
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePayNationWithOutput`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePayNationWithOutput`(         
		 IN parmUserId INT,		 
		 IN parmCountryUserId INT,
		 IN parmCountryId CHAR(2),		 
		 IN parmTaskId CHAR(36),
		 IN parmFundType  TINYINT(3),		 
		 IN parmTaxCode TINYINT(3),		 
		 IN parmAmount DECIMAL(50,2),		 
		 IN parmTaxAmount DECIMAL(50,2),
		 OUT parmResult INT
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/


DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
SET parmResult = 0;

START TRANSACTION;
  
 Select Cash-parmAmount-parmTaxAmount INTO  totalCashLeft from UserBankAccount
 Where Cash>=parmAmount+parmTaxAmount and UserId = parmUserId LOCK IN SHARE MODE;
 
 	IF totalCashLeft>0 THEN
	
		UPDATE `UserBankAccount`
			SET `Cash` = totalCashLeft,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;


			
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			(parmTaskId, parmUserId, parmCountryUserId, parmAmount, parmTaxAmount, parmFundType, UTC_TIMESTAMP(6));
		
		  INSERT INTO `CountryRevenue`
			(`TaskId`,`CountryId`, `Cash`,`Status`,`TaxType`, `UpdatedAt`)VALUES
			(parmTaskId, parmCountryId, parmAmount + parmTaxAmount, 'P', parmTaxCode, UTC_TIMESTAMP(6));
						
			COMMIT;

			SET parmResult = 1;
	ELSE
			SET parmResult = 0;
			ROLLBACK;
				
	END IF;
    
COMMIT;

END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure ExecutePayNation
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePayNation`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePayNation`(         
		 IN parmUserId INT,		 
		 IN parmCountryUserId INT,
		 IN parmCountryId CHAR(2),		 
		 IN parmTaskId CHAR(36),
		 IN parmFundType  TINYINT(3),		 
		 IN parmTaxCode TINYINT(3),		 
		 IN parmAmount DECIMAL(50,2),		 
		 IN parmTaxAmount DECIMAL(50,2)
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/


DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;

DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
  
 Select Cash-parmAmount-parmTaxAmount INTO  totalCashLeft from UserBankAccount
 Where Cash>=parmAmount+parmTaxAmount and UserId = parmUserId LOCK IN SHARE MODE;
 
 	IF totalCashLeft>0 THEN
	
		UPDATE `UserBankAccount`
			SET `Cash` = totalCashLeft,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;


			
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			(parmTaskId, parmUserId, parmCountryUserId, parmAmount, parmTaxAmount, parmFundType, UTC_TIMESTAMP(6));
		
		  INSERT INTO `CountryRevenue`
			(`TaskId`,`CountryId`, `Cash`,`Status`,`TaxType`, `UpdatedAt`)VALUES
			(parmTaskId, parmCountryId, parmAmount + parmTaxAmount, 'P', parmTaxCode, UTC_TIMESTAMP(6));
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateCandidateStatus
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateCandidateStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateCandidateStatus`(
         IN  parmTaskId CHAR(36),		 
		 IN  parmStatus CHAR(1)		 
		 
    )
BEGIN
 Update ElectionCandidate 
	SET Status = parmStatus
    WHERE TaskId = parmTaskId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure DeleteTaskElectionCap
-- -----------------------------------------------------


DROP procedure IF EXISTS `DeleteTaskElectionCap`;

DELIMITER $$

CREATE  PROCEDURE `DeleteTaskElectionCap`(
IN parmElectionId INT
)
BEGIN
DELETE FROM UserTask 
WHERE
	TaskId IN (Select TaskId From ElectionCandidate t1 Where t1.ElectionId = parmElectionId and t1.Status ='P')
AND Status = 'I' ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetConsecutiveTerm
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetConsecutiveTerm`;

DELIMITER $$

CREATE  PROCEDURE `GetConsecutiveTerm`(
         IN  parmUserId INT ,
		 IN  parmElectionId INT         		 
    )
BEGIN
 SELECT Count(*) cnt
    FROM ElectionVoting 
    WHERE UserId = parmUserId
	AND ElectionId>= parmElectionId-4
	AND ElectionResult='W';
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetRunForOfficeTicket
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetRunForOfficeTicket`;

DELIMITER $$
-- Opportunity for Reading joined table from Cache
CREATE  PROCEDURE `GetRunForOfficeTicket`(
         IN  parmTaskId CHAR(36)        		 
    )
BEGIN
 SELECT 
 CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
 t2.Picture, 
 t2.CountryId,
 t1.LogoPictureId,
 t1.CandidateTypeId,
 t4.ElectionPositionName,
 t3.StartDate,
 t3.VotingStartDate,
 t3.EndDate,
 t1.PartyId,
 t1.ElectionId 
 FROM ElectionCandidate t1, WebUser t2, Election t3, ElectionPosition t4 
 Where t1.TaskId = parmTaskId
 AND t1.UserId = t2.UserId
 AND t3.ElectionId = t1.ElectionId 
 AND t4.PositionTypeId=t1.PositionTypeId 
 ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCandidateAgenda
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetCandidateAgenda`;

DELIMITER $$

CREATE  PROCEDURE `GetCandidateAgenda`(
         IN  parmUserId INT ,
		 IN  parmElectionId INT         		 
    )
BEGIN
 SELECT AgendaTypeId
    FROM CandidateAgenda 
    WHERE UserId = parmUserId
	AND ElectionId= parmElectionId
	;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetElectionLast12
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `GetElectionLast12` $$

CREATE  PROCEDURE `GetElectionLast12`
(In parmCountryId CHAR(2))

BEGIN
  Select ElectionId FROM Election Where ElectionId+13> (
  SELECT ElectionId from Election WHERE UTC_TIMESTAMP BETWEEN StartDate and EndDate AND CountryId=parmCountryId) AND CountryId=parmCountryId;


END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCandidateByElection
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetCandidateByElection`;

DELIMITER $$

CREATE  PROCEDURE `GetCandidateByElection`(
         IN  parmCountryId CHAR(2) ,
		 IN  parmElectionId INT ,		 
		 IN  parmLimit INT ,
		 IN  parmLimitOffset INT 
    )
BEGIN
 SELECT 
 CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
 t2.Picture,
 t2.CountryId, 
 t1.CandidateTypeId,
 t1.ElectionId,
 t1.UserId,
 t1.PartyId,
 t3.ElectionPositionName ,
 t1.RequestDate,
 t4.ElectionResult,
 t4.Score,
 t1.Status,
 t1.LogoPictureId 
    FROM ElectionCandidate t1, WebUser t2, ElectionPosition t3, ElectionVoting t4
    WHERE t1.CountryId = parmCountryId
	AND t1.UserId = t2.UserId
	AND t1.PositionTypeId = t3.PositionTypeId
	AND t4.ElectionId= t1.ElectionId
	AND t4.UserId= t1.UserId
	AND t1.ElectionId= parmElectionId
	AND Status= 'A'	
	ORDER BY t4.Score DESC, t2.UserId 
	LIMIT parmLimit OFFSET parmLimitOffset;
	
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecutePayWithTax
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePayWithTax`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePayWithTax`(         
		 IN parmUserId INT,		 
		 IN parmSourceId INT,		
		 IN parmFundType  TINYINT(3),		 
		 IN parmTaxCode TINYINT(3),		 
		 IN parmTaskId CHAR(36),		 
		 IN parmCountryId CHAR(2),		
		 IN parmAmount DECIMAL(50,2),	
		 IN parmTaxAmount DECIMAL(50,2)	 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalCashNew   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select Cash-parmAmount-parmTaxAmount INTO  totalCashLeft from UserBankAccount
 Where Cash>=(parmAmount+parmTaxAmount) and UserId = parmSourceId LOCK IN SHARE MODE;
 
 Select Cash+parmAmount INTO  totalCashNew from UserBankAccount
 Where UserId = parmUserId LOCK IN SHARE MODE;
 
 	IF totalCashLeft>=0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashLeft,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmSourceId;

			UPDATE `UserBankAccount`
			SET `Cash` = totalCashNew,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;

			
		INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			(parmTaskId, parmSourceId, parmUserId, parmAmount, parmTaxAmount, parmFundType, UTC_TIMESTAMP(6));
		
		INSERT INTO `CountryRevenue`
			(`TaskId`,`CountryId`, `Cash`,`Status`,`TaxType`, `UpdatedAt`)VALUES
			(parmTaskId, parmCountryId, parmTaxAmount, 'P', parmTaxCode, UTC_TIMESTAMP(6));	
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetVoteResultByElection
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `GetVoteResultByElection` $$

CREATE  PROCEDURE `GetVoteResultByElection`
(
	IN parmUserId INT
)

BEGIN
  SELECT * from ElectionVoting WHERE  UserId = parmUserId;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetElectionCandidateCount
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `GetElectionCandidateCount` $$

CREATE  PROCEDURE `GetElectionCandidateCount`
(
	IN parmCountryId CHAR(2),
	IN parmElectionId INT	
)

BEGIN
  SELECT Count(*) cnt from ElectionCandidate
  WHERE  CountryId = parmCountryId
  AND	ElectionId = parmElectionId
  AND	Status ='A';

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure ExecuteElectionVoteCounting
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteElectionVoteCounting`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteElectionVoteCounting`(         
		 IN parmCountryId CHAR(2),	
		 IN parmCountryName  VARCHAR(100),	
		 IN parmLimit INT,
		 IN parmPostContentTypeId TINYINT(3),	 
		 IN parmWinNotificationTypeId MEDIUMINT,	 
		 IN parmLossNotificationTypeId MEDIUMINT,	 		 
		 IN parmPriority TINYINT(2),
		 IN parmElectionId INT				  
    )
BEGIN
/*
1-SuccessFull
2-DB error
*/
DECLARE score_new   INT DEFAULT 0;
DECLARE result INT;
DECLARE  syscurrent_date DATETIME(6);

DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 SET result = 0;
 SET syscurrent_date = UTC_TIMESTAMP(6);
		
        Update  ElectionVoting 
            SET ElectionResult =''
            Where ElectionId= parmElectionId and CountryId =parmCountryId;
            
        UPDATE `ElectionVoting` t1
            INNER JOIN (
					Select ElectionId, t3.UserId, t3.CountryId 
					FROM ElectionVoting t3, WebUser t4 
					WHERE 
					t3.UserId = t4.UserId
					AND t3.CountryId = t4.CountryId
					AND t4.Active = 1
					AND t3.CountryId = parmCountryId 
					AND ElectionId = parmElectionId 
					Order By Score desc Limit parmLimit
						) t2
             ON t2.ElectionId = t1.ElectionId AND t1.UserId = t2.UserId AND t1.CountryId = t2.CountryId
			 SET `ElectionResult` = 'W';
		
        UPDATE `ElectionVoting` t1           
			SET `ElectionResult` = 'L'
            WHERE `ElectionResult` <> 'W'
			AND t1.CountryId = parmCountryId
			AND t1.ElectionId = parmElectionId 
			;
        UPDATE `Election` t1           
			SET `EndDate` = syscurrent_date
            WHERE t1.CountryId = parmCountryId
			AND t1.ElectionId = parmElectionId 
			;			
		
        UPDATE `CountryLeader` t1           
			SET `EndDate` = syscurrent_date
            WHERE CountryId =parmCountryId;
		
		INSERT INTO CountryLeader
			(SELECT parmCountryId, t2.UserId,CandidateTypeId, PositionTypeId, syscurrent_date, DATE_ADD(syscurrent_date,INTERVAL 365 DAY) 
			FROM
			ElectionCandidate t1, ElectionVoting t2
			WHERE
			t1.ElectionId = t2.ElectionId
			AND	t2.ElectionId =parmElectionId
			AND t1.UserId = t2.UserId
			AND t1.Status = 'A'
			AND t2.CountryId = parmCountryId
			AND t2.ElectionResult ='W'			
		);
		
		INSERT INTO Post
				SELECT 0, parmCountryId, null, UUID(),'','',0,1,0,0,'',1,0,0				
				,CONCAT(t2.UserId,
				'|',t3.Picture,
				'|',CONCAT( t3.NameFirst, ' ',t3.NameLast),		
				'|',t4.ElectionPositionName,
				'|',parmCountryName,
				'|',parmCountryId,
				'|',t2.Score)
				,parmPostContentTypeId,syscurrent_date,syscurrent_date
                FROM 
					ElectionCandidate t1, ElectionVoting t2, WebUser t3, ElectionPosition t4
					WHERE
					t1.ElectionId = t2.ElectionId
					AND t3.UserId = t2.UserId
					AND t4.PositionTypeId = t1.PositionTypeId
					AND	t2.ElectionId =parmElectionId
					AND t1.UserId = t2.UserId
					AND t1.Status = 'A'
					AND t2.CountryId = parmCountryId
					AND t2.ElectionResult ='W'		;	
		
		INSERT INTO UserNotification
				SELECT UUID(),t2.UserId	,parmWinNotificationTypeId, parmPriority, 0	
				,CONCAT(
				 t4.ElectionPositionName,
				'|',parmCountryName,
				'|',parmCountryId,
				'|',t2.Score)
				,0,syscurrent_date
                FROM 
					ElectionCandidate t1, ElectionVoting t2, WebUser t3, ElectionPosition t4
					WHERE
					t1.ElectionId = t2.ElectionId
					AND t3.UserId = t2.UserId
					AND t4.PositionTypeId = t1.PositionTypeId
					AND	t2.ElectionId =parmElectionId
					AND t1.UserId = t2.UserId
					AND t1.Status = 'A'
					AND t2.CountryId = parmCountryId
					AND t2.ElectionResult ='W'		;	
					
		INSERT INTO UserNotification
				SELECT UUID(),t2.UserId	,parmLossNotificationTypeId, parmPriority, 0	
				,CONCAT(
				 t4.ElectionPositionName,
				'|',parmCountryName,
				'|',parmCountryId,
				'|',t2.Score)
				,0,syscurrent_date
                FROM 
					ElectionCandidate t1, ElectionVoting t2, WebUser t3, ElectionPosition t4
					WHERE
					t1.ElectionId = t2.ElectionId
					AND t3.UserId = t2.UserId
					AND t4.PositionTypeId = t1.PositionTypeId
					AND	t2.ElectionId =parmElectionId
					AND t1.UserId = t2.UserId
					AND t1.Status = 'A'
					AND t2.CountryId = parmCountryId
					AND t2.ElectionResult ='L'		;					
			
SET result = 1;
select result;
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure NotifyStartOfElectionPeroid
-- -----------------------------------------------------


DROP procedure IF EXISTS `NotifyStartOfElectionPeroid`;

DELIMITER $$

CREATE  PROCEDURE `NotifyStartOfElectionPeroid`(
	IN parmPostContentTypeId TINYINT(3)
)
BEGIN							
	INSERT INTO	Post
			SELECT 0, 
			CountryId, 
			null, 
			UUID(),
			'','',0,1,0,0,'',1,0,0,'',
			parmPostContentTypeId,
			UTC_TIMESTAMP(),
			UTC_TIMESTAMP()
 			FROM	
			(SELECT DISTINCT CountryId FROM Election
					WHERE (CountryId, ElectionId ) IN
									(
									SELECT CountryId,  MAX(ElectionId) ElectionId
									FROM Election 
									GROUP BY CountryId	
									) 
					AND StartTermNotified =0		
					AND StartDate < UTC_TIMESTAMP()
                    ) t1;
					
	UPDATE Election t2
	INNER Join 			
				(SELECT DISTINCT CountryId, ElectionId FROM Election
					WHERE (CountryId, ElectionId ) IN
									(
									SELECT CountryId,  MAX(ElectionId) ElectionId
									FROM Election 
									GROUP BY CountryId	
									) 
					AND StartTermNotified =0		
					AND StartDate < UTC_TIMESTAMP()
                    ) t1   ON t1.CountryId = t2.CountryId AND t1.ElectionId = t2.ElectionId
	SET StartTermNotified = 1;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure NotifyStartOfVotingPeroid
-- -----------------------------------------------------


DROP procedure IF EXISTS `NotifyStartOfVotingPeroid`;

DELIMITER $$

CREATE  PROCEDURE `NotifyStartOfVotingPeroid`(
	IN parmPostContentTypeId TINYINT(3)
)
BEGIN							
	INSERT INTO	Post
			SELECT 0, 
			CountryId, 
			null, 
			UUID(),
			'','',0,1,0,0,'',1,0,0,'',
			parmPostContentTypeId,
			UTC_TIMESTAMP(),
			UTC_TIMESTAMP()
 			FROM	
			(SELECT DISTINCT CountryId FROM Election
					WHERE (CountryId, ElectionId ) IN
									(
									SELECT CountryId,  MAX(ElectionId) ElectionId
									FROM Election 
									GROUP BY CountryId	
									) 
					AND VotingStartTermNotified =0		
					AND VotingStartDate < UTC_TIMESTAMP()
                    ) t1;
					
	UPDATE Election t2
	INNER Join 			
				(SELECT DISTINCT CountryId, ElectionId FROM Election
					WHERE (CountryId, ElectionId ) IN
									(
									SELECT CountryId,  MAX(ElectionId) ElectionId
									FROM Election 
									GROUP BY CountryId	
									) 
					AND VotingStartTermNotified =0		
					AND VotingStartDate < UTC_TIMESTAMP()					
                    ) t1   ON t1.CountryId = t2.CountryId AND t1.ElectionId = t2.ElectionId
	SET VotingStartTermNotified = 1;
	
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure NotifyLastDayOfVotingPeroid
-- -----------------------------------------------------


DROP procedure IF EXISTS `NotifyLastDayOfVotingPeroid`;

DELIMITER $$

CREATE  PROCEDURE `NotifyLastDayOfVotingPeroid`(
	IN parmPostContentTypeId TINYINT(3)
)
BEGIN								
	INSERT INTO	Post
			SELECT 0, 
			CountryId, 
			null, 
			UUID(),
			'','',0,1,0,0,'',1,0,0,'',
			parmPostContentTypeId,
			UTC_TIMESTAMP(),
			UTC_TIMESTAMP()
 			FROM	
			(SELECT DISTINCT CountryId FROM Election
					WHERE (CountryId, ElectionId ) IN
									(
									SELECT CountryId,  MAX(ElectionId) ElectionId
									FROM Election 
									GROUP BY CountryId	
									) 
					AND LastDayTermNotified =0		
					AND DATE_SUB(EndDate, INTERVAL 1 DAY) < UTC_TIMESTAMP()					
					AND EndDate > UTC_TIMESTAMP()					
                    ) t1;
					
	UPDATE Election t2
	INNER Join 			
				(SELECT DISTINCT CountryId, ElectionId FROM Election
					WHERE (CountryId, ElectionId ) IN
									(
									SELECT CountryId,  MAX(ElectionId) ElectionId
									FROM Election 
									GROUP BY CountryId	
									) 
					AND LastDayTermNotified =0		
					AND DATE_SUB(EndDate, INTERVAL 1 DAY) < UTC_TIMESTAMP()					
					AND EndDate > UTC_TIMESTAMP()					
                    ) t1   ON t1.CountryId = t2.CountryId AND t1.ElectionId = t2.ElectionId
	SET LastDayTermNotified = 1;
	
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure QuitElection
-- -----------------------------------------------------


DROP procedure IF EXISTS `QuitElection`;

DELIMITER $$

CREATE  PROCEDURE `QuitElection`(
		 IN parmElectionId INT	,
		 IN parmUserId INT
    )
BEGIN
  UPDATE ElectionCandidate t1
  SET Status = 'Q'
  Where ElectionId= parmElectionId
	 AND t1.UserId = parmUserId
	 AND Status !='Q';
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetElectionCandidate
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetElectionCandidate`;

DELIMITER $$

CREATE  PROCEDURE `GetElectionCandidate`(
         IN  parmUserId INT ,
		 IN  parmElectionId INT ,        		 
		 IN  parmCountryId CHAR(2) 
    )
BEGIN
 SELECT *
    FROM ElectionCandidate 
    WHERE UserId = parmUserId
	AND ElectionId = parmElectionId
	AND CountryId = parmCountryId;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCurrentVotingInfo
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetCurrentVotingInfo`;

DELIMITER $$

CREATE  PROCEDURE `GetCurrentVotingInfo`(
         IN  parmCountryId CHAR(2) ,
		 IN  parmElectionId INT,
		 IN  parmPositionTypeId TINYINT(3)		 		 
		 
    )
BEGIN
 SELECT 
 CONCAT( t2.NameFirst, ' ',  t2.NameLast) FullName,
 t2.Picture,
 t2.CountryId,
 t1.TaskId,
 t1.CandidateTypeId,
 t1.ElectionId,
 t1.UserId,
 t1.PartyId, 
 t1.LogoPictureId 
    FROM ElectionCandidate t1, WebUser t2, ElectionPosition t3, ElectionVoting t4
    WHERE t1.CountryId = parmCountryId
	AND t1.UserId = t2.UserId
	AND t1.PositionTypeId = t3.PositionTypeId
	AND t1.PositionTypeId = parmPositionTypeId
	AND t4.ElectionId= t1.ElectionId
	AND t4.UserId= t1.UserId
	AND t1.ElectionId= parmElectionId
	AND Status= 'A'	
	ORDER BY t4.Score DESC, t2.UserId ;
	
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetElectionCandidateCountry
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetElectionCandidateCountry`;

DELIMITER $$

CREATE  PROCEDURE `GetElectionCandidateCountry`(
         IN  parmUserIdList TEXT,
		 IN  parmElectionId INT
    )
BEGIN

	 set @concatsql = CONCAT('select DISTINCT CountryId CountryId from ElectionCandidate where UserId  in (', parmUserIdList, ') AND ElectionId = ',parmElectionId);
	 PREPARE q FROM @concatsql;
execute q;
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecuteUpdateVotingScore
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteUpdateVotingScore`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteUpdateVotingScore`(         
		 IN parmUserId INT,		 
		 IN parmCountryId CHAR(2),	
		 IN parmPoints INT	,			 
		 IN parmElectionId INT		
		  
    )
BEGIN
/*
1-SuccessFull
2-DB error
*/
DECLARE score_new   INT DEFAULT 0;
DECLARE result INT;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 SET result = 0;

  Select Score+ parmPoints INTO  score_new from ElectionVoting
	Where ElectionId= parmElectionId and UserId = parmUserId and CountryId = parmCountryId LOCK IN SHARE MODE;  

			UPDATE `ElectionVoting`
			SET `Score` = score_new
			WHERE ElectionId= parmElectionId and UserId = parmUserId and CountryId = parmCountryId ; 
						
			COMMIT;

			SET result = 1;
    select result;
COMMIT;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetProfileStat
-- ---------------------------------------------------

DROP procedure IF EXISTS `GetProfileStat`;

DELIMITER $$

CREATE  PROCEDURE `GetProfileStat`(
         IN  parmUserId INT
         
    )
BEGIN
Select 
( SELECT Sum(Quantity)   FROM UserMerchandise t1
 Where t1.UserId =parmUserId )TotalProperty,
 
( SELECT Count(*)   FROM Education t2
 Where t2.UserId =parmUserId 
AND Status ='C' )TotalDegree,
 10 TotalAwards
  ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetEducationProfile
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetEducationProfile`;

DELIMITER $$

CREATE  PROCEDURE `GetEducationProfile`(
         IN  parmUserId INT         
    )
BEGIN
 SELECT  t1.DegreeId, 		 		 
		 t2.MajorName,
		 t2.ImageFont,
		 t2.Description, 
		 t3.DegreeName,
		 t3.DegreeImageFont
    FROM Education t1, MajorCode t2, DegreeCode t3
    WHERE UserId = parmUserId
	AND t1.MajorId = t2.MajorId
	AND t1.Status = 'C'
	AND t1.DegreeId = t3.DegreeId; 
	
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateWebUserContactUserId
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateWebUserContactUserId`;

DELIMITER $$

CREATE  PROCEDURE `UpdateWebUserContactUserId`(
         IN  parmUserId INT,     
		 IN parmFriendEmailId varchar(100)	
    )
BEGIN
 Update WebUserContact 
	SET FriendUserId = parmUserId
    WHERE FriendEmailId = parmFriendEmailId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure AddNextElectionTerm
-- -----------------------------------------------------


DROP procedure IF EXISTS `AddNextElectionTerm`;

DELIMITER $$

CREATE  PROCEDURE `AddNextElectionTerm`(
         IN  parmNumberOfDaysofElection INT,     
		 IN  parmNumberOfDaysToElection INT	
)
BEGIN
INSERT INTO Election
	Select t1.ElectionId,
	t1.CountryId, 
	utc_timestamp(),  
	date_add(utc_timestamp(), 
	INTERVAL parmNumberOfDaysToElection Day), 
	date_add(utc_timestamp(), INTERVAL (parmNumberOfDaysofElection + parmNumberOfDaysToElection) Day), 
	t1.Fee,0,0,0 FROM	
			(SELECT DISTINCT CountryId, MAX(ElectionId) + 1 ElectionId, MAX(Fee) Fee FROM Election
					WHERE CountryId NOT IN
									(
									SELECT CountryId
									FROM Election 
									WHERE 
									EndDate> utc_timestamp()
									) 
					GROUP BY CountryId								
                    ) t1
								;
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateWebUserContactSendInvite
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateWebUserContactSendInvite`;

DELIMITER $$

CREATE  PROCEDURE `UpdateWebUserContactSendInvite`(
         IN  parmEmailId TEXT,
		 IN  parmUserId INT,
		 IN	 parmInvitationId CHAR(36)
    )
BEGIN
 set @concatsql = CONCAT('Update WebUserContact 
	SET JoinInvite = JoinInvite+1,
	LastInviteDate = UTC_TIMESTAMP(),
	InvitationId = \'', parmInvitationId, '\'
    WHERE FriendEmailId IN (', parmEmailId, ') AND UserId =', parmUserId);
PREPARE q FROM @concatsql;
execute q;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure WebUserContactInviteAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserContactInviteAddUpdate` $$
CREATE PROCEDURE `WebUserContactInviteAddUpdate` (
	 IN parmUserId int(11),
	 IN parmFriendEmailId varchar(100),
	 IN parmFriendUserId int(11),
	 IN parmNameFirst varchar(45),
	 IN parmInvitationId CHAR(36),
	 IN parmNameLast varchar(45),
	 IN parmPartyInvite tinyint(3),
	 IN parmJoinInvite tinyint(3)
)


	BEGIN 

		INSERT INTO WebUserContact( UserId, InvitationId, FriendEmailId, FriendUserId, NameFirst, NameLast, PartyInvite, JoinInvite)
		VALUES (parmUserId,parmInvitationId, parmFriendEmailId,parmFriendUserId,parmNameFirst,parmNameLast,parmPartyInvite,parmJoinInvite)


	ON DUPLICATE KEY UPDATE
				FriendUserId = parmFriendUserId,
				NameFirst = parmNameFirst,
				NameLast = parmNameLast,
				PartyInvite = PartyInvite + parmPartyInvite,
				JoinInvite = JoinInvite + parmJoinInvite,
				InvitationId= parmInvitationId,
				LastInviteDate = UTC_TIMESTAMP()
;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAllContactProvider
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetAllContactProvider`;
DELIMITER $$
CREATE  PROCEDURE `GetAllContactProvider`( )
BEGIN
 SELECT  t1.*
    FROM ContactProvider t1 Order by ProviderId; 	
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetContactSource
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetContactSource`;

DELIMITER $$

CREATE  PROCEDURE `GetContactSource`(
			IN  parmUserId INT     
    )
BEGIN
 SELECT  t1.Total, t1.UpdatedAt, t1.ProviderId
    FROM ContactSource t1 Where t1.UserId = parmUserId; 	
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure ExecutePayWithTaxBank
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePayWithTaxBank`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePayWithTaxBank`(         		 
		 IN parmSourceId INT,		
		 IN parmFundType  TINYINT(3),		 
		 IN parmTaxCode TINYINT(3),		 
		 IN parmTaskId CHAR(36),		 
		 IN parmCountryId CHAR(2),		
		 IN parmAmount DECIMAL(50,2),	
		 IN parmTaxAmount DECIMAL(50,2)	 
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select Cash-parmAmount-parmTaxAmount INTO  totalCashLeft from UserBankAccount
 Where Cash>=(parmAmount+parmTaxAmount) and UserId = parmSourceId LOCK IN SHARE MODE;
 
 
 	IF totalCashLeft>=0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashLeft,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmSourceId;

			
		INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			(parmTaskId, parmSourceId, parmAmount, parmTaxAmount, parmFundType, UTC_TIMESTAMP(6));
		
		INSERT INTO `CountryRevenue`
			(`TaskId`,`CountryId`, `Cash`,`Status`,`TaxType`, `UpdatedAt`)VALUES
			(parmTaskId, parmCountryId, parmTaxAmount, 'P', parmTaxCode, UTC_TIMESTAMP(6));	
						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure DegreeCheck
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `DegreeCheck` $$

CREATE  PROCEDURE `DegreeCheck`
(In parmRunId INT )

BEGIN
DECLARE dateTimeCheck  DateTime ;
SET dateTimeCheck = UTC_TIMESTAMP(6);
 
 SELECT  t1.UserId, t2.MajorName, t2.MajorId, t2.ImageFont, t2.Description, t3.DegreeName, t3.DegreeId, t3.DegreeImageFont
    FROM Education t1, MajorCode t2, DegreeCode t3
    WHERE 
	 t1.MajorId = t2.MajorId
	AND t1.DegreeId = t3.DegreeId
	AND t1.ExpectedCompletion <=dateTimeCheck
	AND t1.Status ='I';
	
	
INSERT INTO DegreeCheckJob
  SELECT parmRunId, UserId, MajorId, DegreeId
  From Education
  Where ExpectedCompletion <=dateTimeCheck
  AND Status ='I';
  
  Update Education
	SET Status ='C'
	Where ExpectedCompletion <=dateTimeCheck
	AND Status ='I';
  

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ProcessUserWithRentalProperty
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `ProcessUserWithRentalProperty` $$

CREATE  PROCEDURE `ProcessUserWithRentalProperty`
(	 
	IN parmBankId INT,
	IN parmCountryId CHAR(2),		 		 
	IN parmFundType  TINYINT(3),		 
	IN parmTaxRate DECIMAL(5,2),
	IN parmNotificationTypeId SMALLINT,
	IN parmPostContentTypeId tinyint(3),
	IN parmTaxCode TINYINT(3),
	IN parmRentalCap DECIMAL(15,2),
	OUT parmCount INT
	
)

BEGIN
	DECLARE fullName VARCHAR(90);
	DECLARE picture VARCHAR(255);	
	DECLARE totalRental DECIMAL(50,2) DEFAULT 0;
	DECLARE tax DECIMAL(50,2) DEFAULT 0;
	DECLARE userId INT;		
	DECLARE done INT DEFAULT FALSE;

	DECLARE userCursor CURSOR  for	
	  SELECT t1.UserId, 
	  CONCAT( t1.NameFirst, ' ',  t1.NameLast) FullName,
	  t1.Picture, 	  
	  Sum(t3.RentalPrice) TotalRental
	  from WebUser t1 , UserMerchandise t2, MerchandiseType t3
	  WHERE  t1.CountryId=parmCountryId
	  AND t1.UserId  = t2.UserId
	  AND t2.Quantity >0 
	  AND t2.MerchandiseCondition >0
	  AND ROUND(DATEDIFF(UTC_TIMESTAMP(), t2.PurchasedAt))>=2
	  AND t3.MerchandiseTypeId = t2.MerchandiseTypeId  
	  AND t3.RentalPrice >0
	  Group By t1.UserId
	  ;
	  DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	
	  SET parmCount = 0;
	  	  OPEN userCursor;
		  myloop: LOOP
			FETCH userCursor INTO userId, fullName, picture, totalRental;
			IF done THEN
			  LEAVE myloop;
			END IF;			
			SET tax = totalRental * parmTaxRate /100;
			SET totalRental = totalRental * (1- parmTaxRate /100);			
			CALL ExecutePayMeWithOutput(userId, parmBankId, parmCountryId, 
										UUID(), parmFundType, parmTaxCode, totalRental, tax, @payResult);					
			IF	@payResult = 1 THEN
					CALL UserNotificationAdd(
							UUID(), userId,	parmNotificationTypeId,
							2, false, FORMAT(totalRental,2),0,UTC_TIMESTAMP(6),@lastId
							);
				SET parmCount = parmCount +1;			
				IF	totalRental > parmRentalCap THEN
					CALL PostAddUpdate(
							userId, null, null, UUID(),
							'','',0,1,0,0,'',1,0,0,
							CONCAT(userId,'|',picture,'|',fullName,'|',FORMAT(totalRental,2)),
							parmPostContentTypeId,
							UTC_TIMESTAMP(6), UTC_TIMESTAMP(6),
							@lastId
							);
				END IF;				
			END IF;			
		END LOOP;
		CLOSE userCursor;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ProcessUserWithoutHouse
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `ProcessUserWithoutHouse` $$

CREATE  PROCEDURE `ProcessUserWithoutHouse`
(
	
	IN parmCountryId CHAR(2),	
	IN parmCountryUserId INT,	
	IN parmFundType  TINYINT(3),		 	
	IN parmNotificationTypeId SMALLINT,	
	IN parmTaxCode TINYINT(3),
	IN parmWeeklyRent DECIMAL(15,2),
	IN parmRentalPenalty DECIMAL(15,2),
	OUT parmCount INT
	
)

BEGIN
	DECLARE userPayingRentId INT;	
	DECLARE done INT DEFAULT FALSE;
	
	DECLARE userCursor CURSOR  for	
		SELECT t1.UserId
		  from WebUser t1  WHERE  CountryId=parmCountryId
		  AND ROUND(DATEDIFF(UTC_TIMESTAMP(), t1.CreatedAt))<2
		  AND t1.UserId NOT IN
		  (
			SELECT UserId FROM UserMerchandise t2, MerchandiseType t3 WHERE
			t2.Quantity >0 
			AND t2.MerchandiseCondition >0
			AND t3.MerchandiseTypeCode =2
			AND t3.MerchandiseTypeId = t2.MerchandiseTypeId
		  )  
		  ;
		DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	
		SET parmCount = 0;
  	  OPEN userCursor;
		  myloop: LOOP
			FETCH userCursor INTO userPayingRentId;
			IF done THEN
			  LEAVE myloop;
			END IF;						
			
			CALL ExecutePayNationWithOutput(userPayingRentId, parmCountryUserId, parmCountryId, 
										UUID(), parmFundType, parmTaxCode, 0, parmWeeklyRent, @payResult);					
			IF	@payResult = 1 THEN
					CALL UserNotificationAdd(
							UUID(), userPayingRentId,	parmNotificationTypeId,
							2, false, FORMAT(parmWeeklyRent,2), 0,	UTC_TIMESTAMP(6),@lastId
							);
				SET parmCount = parmCount +1;	
				ELSEIF	@payResult = 0 THEN			
					Update CreditScore 
						SET Score = Score + parmRentalPenalty 
						Where UserId = userPayingRentId; 						
			END IF;			
		END LOOP;
		CLOSE userCursor;
END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetLastWebJobRunTime
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetLastWebJobRunTime`;

DELIMITER $$

CREATE  PROCEDURE `GetLastWebJobRunTime`(
			IN  parmJobId TINYINT(3)					
    )
BEGIN
 SELECT  *
    FROM WebJobHistory t1
	Where t1.JobId = parmJobId
	Order By CreatedAT desc
	LIMIT 1; 	
 END$$
DELIMITER ;




-- -----------------------------------------------------
-- procedure ExecutePayMeWithOutput
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePayMeWithOutput`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePayMeWithOutput`(         
		 IN parmUserId INT,		 
		 IN parmSourceId INT,
		 IN parmCountryId CHAR(2),		 
		 IN parmTaskId CHAR(36),
		 IN parmFundType  TINYINT(3),		 
		 IN parmTaxCode TINYINT(3),		 
		 IN parmAmount DECIMAL(50,2),		 
		 IN parmTaxAmount DECIMAL(50,2),
		 OUT parmResult INT
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/


DECLARE totalCashNew   DECIMAL(50,2) DEFAULT -1 ;

/*
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET parmResult = 2;
ROLLBACK; 
END;
*/
SET parmResult = 0;

START TRANSACTION;
  
 Select Cash+parmAmount INTO  totalCashNew from UserBankAccount
 Where UserId = parmUserId LOCK IN SHARE MODE;	
		UPDATE `UserBankAccount`
			SET `Cash` = totalCashNew,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;
			
			INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			(parmTaskId, parmSourceId, parmUserId, parmAmount, parmTaxAmount, parmFundType, UTC_TIMESTAMP(6));
	
		IF parmTaxAmount> 0 THEN
		  INSERT INTO `CountryRevenue`
			(`TaskId`,`CountryId`, `Cash`,`Status`,`TaxType`, `UpdatedAt`)VALUES
			(parmTaskId, parmCountryId, parmTaxAmount, 'P', parmTaxCode, UTC_TIMESTAMP(6));
		END IF;							
			COMMIT;
			SET parmResult = 1;    
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ExecutePayMe
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePayMe`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePayMe`(         
		 IN parmUserId INT,		 
		 IN parmSourceId INT,
		 IN parmCountryId CHAR(2),		 
		 IN parmTaskId CHAR(36),
		 IN parmFundType  TINYINT(3),		 
		 IN parmTaxCode TINYINT(3),		 
		 IN parmAmount DECIMAL(50,2),		 
		 IN parmTaxAmount DECIMAL(50,2)
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/


DECLARE totalCashNew   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;

DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
  
 Select Cash+parmAmount INTO  totalCashNew from UserBankAccount
 Where UserId = parmUserId LOCK IN SHARE MODE;
 
 	
	
		UPDATE `UserBankAccount`
			SET `Cash` = totalCashNew,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;
			
		INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			(parmTaskId, parmSourceId, parmUserId, parmAmount, parmTaxAmount, parmFundType, UTC_TIMESTAMP(6));
		
		IF parmTaxAmount> 0 THEN
		  INSERT INTO `CountryRevenue`
			(`TaskId`,`CountryId`, `Cash`,`Status`,`TaxType`, `UpdatedAt`)VALUES
			(parmTaskId, parmCountryId, parmTaxAmount, 'P', parmTaxCode, UTC_TIMESTAMP(6));
		END IF;				
			COMMIT;
			SET result = 1;
	

    select result;
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ApplyCreidtScore
-- -----------------------------------------------------
DROP procedure IF EXISTS `ApplyCreidtScore`;

DELIMITER $$

CREATE  PROCEDURE `ApplyCreidtScore`()
BEGIN

START TRANSACTION;
-- SET SQL_SAFE_UPDATES = 0;

Delete From CreditScore;
-- SET SQL_SAFE_UPDATES = 1;
Insert into CreditScore 
Select  UserId , CAST( Sum(Score) AS DECIMAL(50,2)) CreditScore from (

Select Sum(StockTotal)/10000 Score, UserId from (
Select t2.CurrentValue * SUM(t1.PurchasedUnit) StockTotal , t1.UserId From  UserStock t1, Stock t2
Where 
t1.StockId = t2.StockId group by t2.StockId, t1.UserId) c1 Group By c1.UserId

union all

Select Sum(MerchandiseTotal)/10000 Score, UserId from (
Select t2.Cost * SUM(t1.Quantity) MerchandiseTotal , t1.UserId From  UserMerchandise t1, MerchandiseType t2
Where 
t1.MerchandiseTypeId = t2.MerchandiseTypeId group by t2.MerchandiseTypeId, t1.UserId) c1 Group By c1.UserId

union all

Select (Cash/10000 + Gold/100 + Silver/1000) Score, UserId from UserBankAccount Group By UserId

union all

Select Sum(Salary)/100 Score, UserId from UserJob Where EndDate> UTC_TIMESTAMP() Group By UserId

union all

Select Count(*) * 20 Score, UserId from Education Where Status ='C' Group By UserId
union all
Select Count(*) * 5 Score, UserId from Education Where Status ='I' Group By UserId

union all
Select Count(*) * 10 Score, FromId UserId from Gift Group By FromId

union all
Select (Cash/100 + Gold + Silver/10)  Score, FromId UserId from Gift Group By FromId

union all
Select CASE MemberType
	WHEN 'C' THEN 75
	WHEN 'F' THEN 150
	WHEN 'M' THEN 50
END 
 Score ,  UserId from PartyMember  Where  (MemberEndDate =0 or MemberEndDate='0001-01-01 00:00:00' or MemberEndDate is NULL)  Group By UserId

union all
Select  Count(*) * 100 Score, UserId from ElectionVoting Where ElectionResult='W' Group By UserId

union all
Select  SUM(Score) Score, UserId from ElectionVoting Where ElectionResult !='Q' Group By UserId

union all
Select  -SUM(LoanAmount)/100 Score, UserId from UserLoan Where Status ='A' Group By UserId

union all
Select  SUM(LoanAmount)/1000 Score, LendorId as UserId from UserLoan Where Status ='A' Group By LendorId


) g1 Group By UserId;

COMMIT;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UserChangingLevel
-- -----------------------------------------------------

DROP procedure IF EXISTS `UserChangingLevel`;

DELIMITER $$

CREATE  PROCEDURE `UserChangingLevel`( 
	IN parmNotificationTypeId SMALLINT,
	IN parmPostContentTypeId tinyint(3)
)
BEGIN
	DECLARE fullName VARCHAR(90);
	DECLARE picture VARCHAR(255);
	DECLARE userId INT;	
	DECLARE done INT DEFAULT FALSE;


	DECLARE userCursor CURSOR  for	
	SELECT  t1.UserId,
		CONCAT( t1.NameFirst, ' ',  t1.NameLast) FullName,
		t1.Picture
    FROM WebUser t1, CreditScore t2
	Where t1.UserId = t2.UserId
	AND (t2.Score - t1.UserLevel) >=1000
	AND (t1.UserLevel) <7000;
    

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN userCursor;
		  myloop: LOOP
			FETCH userCursor INTO userId, fullName, picture;
			IF done THEN
			  LEAVE myloop;
			END IF;
			CALL UserNotificationAdd(
			UUID(), userId,	parmNotificationTypeId,
			2, false, fullName,	0, UTC_TIMESTAMP(6),@lastId
			);
			CALL PostAddUpdate(
				userId, null, null, UUID(),
				'',
				'Experience ++',0,1,0,0,'',1,0,0,
				CONCAT(userId,'|',picture,'|',fullName),
				parmPostContentTypeId,
				UTC_TIMESTAMP(6), UTC_TIMESTAMP(6),
				@lastId
			);	
	END LOOP;
  CLOSE userCursor;
  
  	UPDATE WebUser t1
	INNER JOIN CreditScore t2 ON t1.UserId = t2.UserId
	SET t1.UserLevel = t2.Score
	Where (t2.Score - t1.UserLevel)>=0;
  
 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure MerchandiseCondition
-- -----------------------------------------------------

DROP procedure IF EXISTS `MerchandiseCondition`;

DELIMITER $$

CREATE  PROCEDURE `MerchandiseCondition`( 
OUT parmCount INT)
BEGIN
		UPDATE 
		UserMerchandise t1
		SET t1.MerchandiseCondition = 
		t1.MerchandiseCondition- DATEDIFF(UTC_TIMESTAMP(), t1.PurchasedAt)/t1.PurchasedPrice * 500
		Where t1.Quantity>0 
	AND t1.MerchandiseCondition >1;				
		Select ROW_COUNT() INTO parmCount;	
 END$$
DELIMITER ;




-- -----------------------------------------------------
-- procedure WeaponCondition
-- -----------------------------------------------------

DROP procedure IF EXISTS `WeaponCondition`;

DELIMITER $$

CREATE  PROCEDURE `WeaponCondition`( 
	OUT parmCount INT)
BEGIN
		UPDATE 
		CountryWeapon t1
		SET t1.WeaponCondition = 
		t1.WeaponCondition- DATEDIFF(UTC_TIMESTAMP(), t1.PurchasedAt)/t1.PurchasedPrice * 500
		Where t1.Quantity>0 
		AND t1.WeaponCondition >1
		;				
		Select ROW_COUNT() INTO parmCount;
				
 END$$
DELIMITER ;



-- -----------------------------------------------------
-- procedure GetMatchLottoPick5
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetMatchLottoPick5`;

DELIMITER $$

CREATE  PROCEDURE `GetMatchLottoPick5`(         
	   IN parmDrawingId INT ,
	   IN parmNumber1 TINYINT(2)  ,
	   IN parmNumber2 TINYINT(2)  ,
	   IN parmNumber3 TINYINT(2)  ,
	   IN parmNumber4 TINYINT(2)  ,
	   IN parmNumber5 TINYINT(2)   		 
	
    )
BEGIN
 	  Select 5 PickMatch, UserId from PickFive Where DrawingId = parmDrawingId AND Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 
	  union all
	  Select 4 PickMatch, UserId from PickFive Where DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 ))
	  union all
	  Select 3 PickMatch, UserId from PickFive Where  DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 ))
		union all
	  Select 2 PickMatch, UserId from PickFive Where  DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5=parmNumber5 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 )||				
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 ))

	union all
	  Select 1 PickMatch, UserId from PickFive Where  DrawingId = parmDrawingId AND (
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 AND Number4!=parmNumber4 AND Number5!=parmNumber5 )||
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4=parmNumber4 AND Number5!=parmNumber5 )||				
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 AND Number4!=parmNumber4 AND Number5=parmNumber5 ))

				;
	
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetMatchLottoPick3
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetMatchLottoPick3`;

DELIMITER $$

CREATE  PROCEDURE `GetMatchLottoPick3`(         
	   IN parmDrawingId INT ,
	   IN parmNumber1 TINYINT(2)  ,
	   IN parmNumber2 TINYINT(2)  ,
	   IN parmNumber3 TINYINT(2)
	
    )
BEGIN
 	  Select 3 PickMatch, UserId from PickThree Where DrawingId = parmDrawingId AND Number1=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 
	  union all
	  Select 2 PickMatch, UserId from PickThree Where DrawingId = parmDrawingId AND (				
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3=parmNumber3 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 )||
				(Number1=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 ))
	  union all
	  Select 1 PickMatch, UserId from PickThree Where  DrawingId = parmDrawingId AND (				
				(Number1!=parmNumber1 AND Number2!=parmNumber2 AND Number3=parmNumber3 )||
				(Number1=parmNumber1 AND Number2!=parmNumber2 AND Number3!=parmNumber3 )||
				(Number1!=parmNumber1 AND Number2=parmNumber2 AND Number3!=parmNumber3 ))
	

				;
	
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure BudgetPopulationStimulator
-- -----------------------------------------------------

DROP procedure IF EXISTS `BudgetPopulationStimulator`;

DELIMITER $$

CREATE  PROCEDURE `BudgetPopulationStimulator`( 
	IN  parmPopulationScale DECIMAL(7,2),
	IN  parmBudgetSimulatorTaxType TINYINT(3),
	OUT parmResultCount INT )
BEGIN

	INSERT INTO `CountryRevenue`
		Select UUID(), CountryId, Count(*) * parmPopulationScale, 'P',
		parmBudgetSimulatorTaxType, UTC_TIMESTAMP(6)
		FROM WebUser t1
		Where Active =1	
		GROUP BY CountryId;	
	SELECT ROW_COUNT() INTO parmResultCount;
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure BudgetWarStimulator
-- -----------------------------------------------------

DROP procedure IF EXISTS `BudgetWarStimulator`;

DELIMITER $$

CREATE  PROCEDURE `BudgetWarStimulator`( 	
	IN  parmWarWinScale DECIMAL(7,2),
	IN  parmWarLooseScale DECIMAL(7,2),	
	IN  parmBudgetWarVictoryTaxType TINYINT(3),
	IN  parmBudgetWarLossTaxType TINYINT(3),	
	IN  parmWinDaysCredit INT,
	IN  parmLostDaysCredit INT
	)
BEGIN

	INSERT INTO `CountryRevenue`
		Select UUID(), CountryIdAttacker, Count(*) * WinQuality * parmWarWinScale, 'P',
		parmBudgetWarVictoryTaxType, UTC_TIMESTAMP(6)
		FROM WarResult
		Where EndDate>= date_sub(UTC_TIMESTAMP(), INTERVAL  parmWinDaysCredit DAY)
		AND WarResult ='A'
		GROUP BY CountryIdAttacker;	
	
	INSERT INTO `CountryRevenue`
		Select UUID(), CountryIdDefender, Count(*) * WinQuality * parmWarWinScale, 'P',
		parmBudgetWarVictoryTaxType, UTC_TIMESTAMP(6)
		FROM WarResult
		Where EndDate>= date_sub(UTC_TIMESTAMP(), INTERVAL  parmWinDaysCredit DAY)
		AND WarResult ='D'
		GROUP BY CountryIdDefender;		
	
	INSERT INTO `CountryRevenue`
		Select UUID(), CountryIdAttacker, -Count(*) * WinQuality * parmWarLooseScale, 'P',
		parmBudgetWarLossTaxType, UTC_TIMESTAMP(6)
		FROM WarResult
		Where EndDate>= date_sub(UTC_TIMESTAMP(), INTERVAL  parmLostDaysCredit DAY)
		AND WarResult ='D'
		GROUP BY CountryIdAttacker;	
	
	INSERT INTO `CountryRevenue`
		Select UUID(), CountryIdDefender, -Count(*) * WinQuality * parmWarLooseScale, 'P',
		parmBudgetWarLossTaxType, UTC_TIMESTAMP(6)
		FROM WarResult
		Where EndDate>= date_sub(UTC_TIMESTAMP(), INTERVAL  parmLostDaysCredit DAY)
		AND WarResult ='A'
		GROUP BY CountryIdDefender;	
		

 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure BudgetStimulator
-- -----------------------------------------------------

DROP procedure IF EXISTS `BudgetStimulator`;

DELIMITER $$

CREATE  PROCEDURE `BudgetStimulator`( OUT parmResultCount INT )
BEGIN
	
	DECLARE firtDayOfNextWeek DATETIME ;
	DECLARE lastDayOfNextWeek DATETIME ;
	
	SET firtDayOfNextWeek = date_add(UTC_TIMESTAMP(), INTERVAL 8-DAYOFWEEK(UTC_TIMESTAMP()) DAY) + INTERVAL 0 SECOND;
	SET lastDayOfNextWeek = date_add(UTC_TIMESTAMP(), INTERVAL 14-DAYOFWEEK(UTC_TIMESTAMP()) DAY) + INTERVAL 86399 SECOND ;
	INSERT INTO CountryBudget 
	SELECT UUID(), t1.CountryId, SUM(t1.Cash),
	firtDayOfNextWeek, 
	lastDayOfNextWeek,
	'P',
	UTC_TIMESTAMP()
	FROM CountryRevenue t1
	WHERE Status = 'P'
	AND NOT EXISTS ( Select 1 From CountryBudget t2 
						Where t1.CountryId = t2.CountryId 
						AND t2.StartDate =firtDayOfNextWeek
						AND t2.EndDate =lastDayOfNextWeek
						AND t2.Status ='P')
	
	GROUP BY CountryId;	
	SELECT ROW_COUNT() INTO parmResultCount;
	Update CountryRevenue
	SET  Status = 'D'
	Where Status ='P';
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAllCountryApplyingJob
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetAllCountryApplyingJob`;

DELIMITER $$

CREATE  PROCEDURE `GetAllCountryApplyingJob`( )
BEGIN

	SELECT DISTINCT t2.CountryId From UserJob t1, WebUser t2
	WHERE t1.UserId = t2.UserId	
	AND t1.Status ='P'
	;
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAllApplyJobCodeByCountry
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetAllApplyJobCodeByCountry`;

DELIMITER $$

CREATE  PROCEDURE `GetAllApplyJobCodeByCountry`( 
	IN parmCountryId CHAR(2))
BEGIN

	SELECT t1.JobCodeId, t1.UserId, t1.TaskId From UserJob t1, WebUser t2
	WHERE t1.UserId = t2.UserId
	AND t2.CountryId = parmCountryId	
	AND t1.Status ='P'
	;
	
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure UpdateUserJobStatus
-- -----------------------------------------------------

DROP procedure IF EXISTS `UpdateUserJobStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateUserJobStatus`( 
	IN parmStatus CHAR(1),
	IN parmTaskIdList TEXT
	)
BEGIN
		set @concatsql = CONCAT('UPDATE UserJob SET Status =\'', parmStatus,
		'\', UpdatedAt =UTC_TIMESTAMP(6) WHERE TaskId IN (', parmTaskIdList, ' )');
	
	PREPARE q FROM @concatsql;
	execute q;
	
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure SendBulkNotificationsAndPost
-- -----------------------------------------------------

DROP procedure IF EXISTS `SendBulkNotificationsAndPost`;

DELIMITER $$

CREATE  PROCEDURE `SendBulkNotificationsAndPost`( 
	 IN parmNotificationTypeId SMALLINT,
	 IN parmPriority tinyint(2),
	 IN parmHasTask tinyint(1),
	 IN parmNotficationParms varchar(1000),
	 IN parmCountryId char(2),
	 IN parmPartyId char(36),	
	 IN parmPostParms longtext,
	 IN parmPostContentTypeId tinyint(3),
	 IN parmUserIdList TEXT
	)
BEGIN
DECLARE  syscurrent_date DATETIME(6);
SET syscurrent_date = UTC_TIMESTAMP(6);


SET @separatorLength = 1;
 
WHILE parmUserIdList != '' > 0 DO
    SET @userId = SUBSTRING_INDEX(parmUserIdList, ',', 1);
	SET parmUserIdList = SUBSTRING(parmUserIdList, CHAR_LENGTH(@userId) + @separatorLength + 1);	
		IF  parmNotificationTypeId >0 THEN
		
			INSERT INTO UserNotification( NotificationId, UserId, NotificationTypeId, Priority, HasTask, Parms, UpdatedAt)
			VALUES (UUID(),@userId,parmNotificationTypeId,parmPriority,parmHasTask,parmNotficationParms,syscurrent_date);
		
		END IF;
				
		IF  parmPostContentTypeId >0 THEN
		
			INSERT INTO Post( UserId, CountryId, PartyId, PostId, PostContent, PostTitle, ChildCommentCount, CommentEnabled, DigIt, CoolIt, ImageName, IsApproved, IsSpam, IsDeleted, Parms, PostContentTypeId, UpdatedAt, CreatedAt)
			VALUES (@userId,parmCountryId,parmPartyId,UUID(),'','',0,1,0,0,'',1,0,0,parmPostParms,parmPostContentTypeId,syscurrent_date,syscurrent_date);
			
		END IF;
		
		
	END WHILE;
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetJobMatchScore
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetJobMatchScore`;

DELIMITER $$

CREATE  PROCEDURE `GetJobMatchScore`( 	
	IN parmMatchMajorFactor SMALLINT(3),
	IN parmDurationJobCodeFactor SMALLINT(3),
	IN parmDurationIndustryFactor SMALLINT(3),
	IN parmDurationAnyExperinceFactor SMALLINT(3),
	IN parmJobCodeId SMALLINT(5),
	IN parmIndustryId SMALLINT(5),
	IN parmMajorId SMALLINT(3),
	IN parmDegreeId SMALLINT(3),
	IN parmUserIdList TEXT
	)
BEGIN
set @concatsql = CONCAT('SELECT UserId, Sum(Score) AS MatchScore FROM ( SELECT 	UserId,
			SUM(DegreeId) * ', parmMatchMajorFactor ,' AS Score
	FROM Education 
	WHERE UserId IN ( ', parmUserIdList, ' ) AND MajorId = ', parmMajorId ,' 
	AND Status =\'C\'
	GROUP BY UserId
	
	UNION ALL
	
	SELECT 	UserId, 
			SUM(TIMESTAMPDIFF(HOUR, StartDate, EndDate) )/',parmDurationJobCodeFactor ,' AS Score
	FROM UserJob t1
	WHERE  UserId IN ( ', parmUserIdList, ' ) 
	AND t1.Status IN (\'E\',\'Q\')
	AND t1.JobCodeId = ', parmJobCodeId	,' 
	GROUP BY UserId
	
	UNION ALL
	
	SELECT 	UserId, 
			SUM(TIMESTAMPDIFF(HOUR, StartDate, EndDate) )/',parmDurationIndustryFactor ,' AS Score
	FROM UserJob t1, JobCode t2
	WHERE  UserId IN ( ', parmUserIdList, ' )
	AND t1.Status IN (\'E\',\'Q\')
	AND t2.IndustryId = ', parmIndustryId,' 	
	AND t1.JobCodeId = t2.JobCodeId
	GROUP BY UserId

	UNION ALL
	
	SELECT 	UserId, 
			SUM(TIMESTAMPDIFF(HOUR, StartDate, EndDate) )/',parmDurationAnyExperinceFactor ,' AS Score
	FROM UserJob t1
	WHERE  UserId IN ( ', parmUserIdList, ' )
	AND t1.Status IN (\'E\',\'Q\')
	GROUP BY UserId	) scoreTable GROUP BY UserId Order BY MatchScore DESC, UserId');
	PREPARE q FROM @concatsql;	
	execute q;
	
	
	 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure GetWebUserList
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetWebUserList`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserList`( 	
	IN parmUserIdList TEXT
	)
BEGIN
		set @concatsql = CONCAT('Select t1.UserId,
		CONCAT( t1.NameFirst,  t1.NameLast) FullName,
		t1.Picture
		FROM WebUser t1 WHERE t1.Active =1 AND t1.UserId IN (', parmUserIdList, ' )');

	PREPARE q FROM @concatsql;
	execute q;
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetWebUserIndexList
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetWebUserIndexList`;

DELIMITER $$

CREATE  PROCEDURE `GetWebUserIndexList`( )
BEGIN
 SELECT   
	CONCAT( NameFirst, ' ',  NameLast) FullName,
	 NameFirst, 
	 NameLast,
	 Picture,   
	 CountryId,
	 EmailId,
	 UserId
 FROM WebUser
 WHERE
 Active =1;   
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SendBulkTaskAndReminder
-- -----------------------------------------------------

DROP procedure IF EXISTS `SendBulkTaskAndReminder`;

DELIMITER $$

CREATE  PROCEDURE `SendBulkTaskAndReminder`( 
	 IN parmTaskId char(36),	 
	 IN parmAssignerUserId int(11),
	 IN parmCompletionPercent tinyint(2),
	 IN parmFlagged tinyint(1),
	 IN parmStatus char(1),
	 IN parmParms varchar(1000),
	 IN parmTaskTypeId smallint(5),
	 IN parmDueDate datetime,
	 IN parmDefaultResponse smallint,
	 IN parmPriority tinyint(2),
	 IN parmCreatedAt datetime(6),
	 IN parmReminderFrequency smallint(6),
	 IN parmReminderTransPort varchar(2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmUserIdList TEXT
	)
BEGIN
DECLARE  syscurrent_date DATETIME(6);
SET syscurrent_date = UTC_TIMESTAMP(6);


SET @separatorLength = 1;

		
INSERT INTO TaskReminder( TaskId, ReminderFrequency, ReminderTransPort, StartDate, EndDate)
VALUES (parmTaskId,parmReminderFrequency,parmReminderTransPort,parmStartDate,parmEndDate);	
 
WHILE parmUserIdList != '' > 0 DO
    SET @userId = SUBSTRING_INDEX(parmUserIdList, ',', 1);
	SET parmUserIdList = SUBSTRING(parmUserIdList, CHAR_LENGTH(@userId) + @separatorLength + 1);	
	
		IF  parmTaskTypeId >0 THEN
		
			IF  parmAssignerUserId =0 THEN
				SET parmAssignerUserId = @userId;
			END IF;
		INSERT INTO UserTask( TaskId, UserId, AssignerUserId, CompletionPercent, Flagged, Status, Parms, TaskTypeId, DueDate, DefaultResponse, Priority, CreatedAt)
		VALUES (parmTaskId,@userId,parmAssignerUserId,parmCompletionPercent,parmFlagged,parmStatus,parmParms,parmTaskTypeId,parmDueDate,parmDefaultResponse,parmPriority,parmCreatedAt);
	
		
		END IF;
		
		
	END WHILE;
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure SendBulkTaskListAndReminder
-- -----------------------------------------------------

DROP procedure IF EXISTS `SendBulkTaskListAndReminder`;

DELIMITER $$

CREATE  PROCEDURE `SendBulkTaskListAndReminder`( 
	 IN parmTaskIdList TEXT,	 
	 IN parmAssignerUserId int(11),
	 IN parmCompletionPercent tinyint(2),
	 IN parmFlagged tinyint(1),
	 IN parmStatus char(1),
	 IN parmParms varchar(1000),
	 IN parmTaskTypeId smallint(5),
	 IN parmDueDate datetime,
	 IN parmDefaultResponse varchar(255),
	 IN parmPriority tinyint(2),
	 IN parmCreatedAt datetime(6),
	 IN parmReminderFrequency smallint(6),
	 IN parmReminderTransPort varchar(2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmUserIdList TEXT
	)
BEGIN
DECLARE  syscurrent_date DATETIME(6);
DECLARE  assignedUserId INT(11) DEFAULT 0;
SET syscurrent_date = UTC_TIMESTAMP(6);


SET @separatorLength = 1;

		

WHILE parmUserIdList != '' > 0 DO
    SET @userId = SUBSTRING_INDEX(parmUserIdList, ',', 1);
	SET @taskId = SUBSTRING_INDEX(parmTaskIdList, ',', 1);
	SET parmUserIdList = SUBSTRING(parmUserIdList, CHAR_LENGTH(@userId) + @separatorLength + 1);	
	SET parmTaskIdList = SUBSTRING(parmTaskIdList, CHAR_LENGTH(@taskId) + @separatorLength + 1);	
	
		IF  parmTaskTypeId >0 THEN
		
			IF  parmAssignerUserId =0 THEN
				SET assignedUserId = @userId;
			ELSE
				SET assignedUserId = parmAssignerUserId;
			END IF;
		INSERT INTO UserTask( TaskId, UserId, AssignerUserId, CompletionPercent, Flagged, Status, Parms, TaskTypeId, DueDate, DefaultResponse, Priority, CreatedAt)
		VALUES (@taskId,@userId,assignedUserId,parmCompletionPercent,parmFlagged,parmStatus,parmParms,parmTaskTypeId,parmDueDate,parmDefaultResponse,parmPriority,parmCreatedAt);
		
		INSERT INTO TaskReminder( TaskId, ReminderFrequency, ReminderTransPort, StartDate, EndDate)
		VALUES (@taskId,parmReminderFrequency,parmReminderTransPort,parmStartDate,parmEndDate);	
 
		
		END IF;
		
		
	END WHILE;
	
	 END$$
DELIMITER ;



-- -----------------------------------------------------
-- procedure GetIncompletePastDueTask
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetIncompletePastDueTask`;

DELIMITER $$

CREATE  PROCEDURE `GetIncompletePastDueTask`()
BEGIN

	SELECT t1.TaskId,t1.UserId, t1.DefaultResponse, t1.TaskTypeId From UserTask t1
	WHERE t1.DueDate <= UTC_TIMESTAMP(6)
	AND t1.Status !='C'
	;
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure AddCashAndLeftAmountToNewBudget
-- -----------------------------------------------------

DROP procedure IF EXISTS `AddCashAndLeftAmountToNewBudget`;

DELIMITER $$

CREATE  PROCEDURE `AddCashAndLeftAmountToNewBudget`()
BEGIN
	UPDATE 
		CountryBudget t1
		JOIN CountryCode t3 ON t1.CountryId = t3.CountryId
		JOIN UserBankAccount t2 ON t3.CountryUserId = t2.UserId
			SET TotalAmount = TotalAmount + t2.Cash 
			Where 
			t1.Status = 'A'	
			AND YEAR(t1.EndDate) = YEAR(UTC_TIMESTAMP())							
			AND MONTH(t1.EndDate) = MONTH(UTC_TIMESTAMP());
						
	UPDATE UserBankAccount 
	SET Cash =0,
	UpdatedAt = UTC_TIMESTAMP()
	Where UserId in (Select CountryUserId from CountryCode);

	UPDATE 
		CountryBudget t1
	INNER JOIN(    
    Select t3.CountryId, Sum(t2.AmountLeft) TotalLeft FROM   CountryBudget t3 , CountryBudgetByType t2 
			WHERE t3.Status = 'A'
			AND MONTH(t3.EndDate) =MONTH(DATE_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))		
			AND YEAR(t3.EndDate) = YEAR(Date_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))	
			AND t2.TaskId = t3.TaskId	
			AND t2.AmountLeft >0
            GROUP BY t2.TaskId
		
        ) t4 ON t4.CountryId = t1.CountryId        
        SET t1.TotalAmount = t1.TotalAmount + (t4.TotalLeft) 
			Where 
			t1.Status = 'A'
			AND YEAR(t1.EndDate) = YEAR(UTC_TIMESTAMP())
			AND MONTH(t1.EndDate) = MONTH(UTC_TIMESTAMP());
			
	
	
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure RedistributeBudgetTotal
-- -----------------------------------------------------

DROP procedure IF EXISTS `RedistributeBudgetTotal`;

DELIMITER $$

CREATE  PROCEDURE `RedistributeBudgetTotal`(
	 IN parmBaseTaskId CHAR(36),
	 IN parmOldbudgetTotal DECIMAL(50,2)
)
BEGIN
	DECLARE done INT DEFAULT FALSE;
	DECLARE budgetPercent Decimal(5,5);	
	DECLARE budgetNewTotal Decimal(50,2);	
	DECLARE budgetType TINYINT(3);
	
	


	DECLARE budgetCursor CURSOR  for	
	SELECT  t1.BudgetType, (t1.Amount / parmOldbudgetTotal) TotalPercent		
    FROM CountryBudgetByType t1
	Where t1.TaskId = parmBaseTaskId;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN budgetCursor;
      
      	SELECT  t1.TotalAmount INTO budgetNewTotal 		
			FROM CountryBudget t1
			Where t1.TaskId = parmBaseTaskId;
            
		  myloop: LOOP
			FETCH budgetCursor INTO budgetType, budgetPercent;
			IF done THEN
			  LEAVE myloop;
			END IF;
			UPDATE CountryBudgetByType
				SET AMOUNT = budgetPercent * budgetNewTotal
				Where TaskId = parmBaseTaskId AND 
				BudgetType = budgetType;		
	END LOOP;	
  CLOSE budgetCursor;   

	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure ReCalculateBudget
-- -----------------------------------------------------

DROP procedure IF EXISTS `ReCalculateBudget`;

DELIMITER $$

CREATE  PROCEDURE `ReCalculateBudget`()
BEGIN
	DECLARE done INT DEFAULT FALSE;	
	DECLARE budgetOldTotal Decimal(50,2);	
	DECLARE budgetTaskId CHAR(36);	
	
	
	
	DECLARE budgetCursor CURSOR  for	
	SELECT  t1.TotalAmount, t1.TaskId		
    FROM CountryBudget t1
	Where t1.Status = 'A'
	AND YEAR(t1.EndDate) = YEAR(UTC_TIMESTAMP())
	AND MONTH(t1.EndDate) =MONTH(UTC_TIMESTAMP());

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN budgetCursor;
		CALL AddCashAndLeftAmountToNewBudget();
		  myloop: LOOP
			FETCH budgetCursor INTO budgetOldTotal, budgetTaskId;
			IF done THEN
			  LEAVE myloop;
			END IF;	
		CALL RedistributeBudgetTotal(budgetTaskId, budgetOldTotal );
	END LOOP;	
  CLOSE budgetCursor;   
 
	
	 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure CancelStockOrderForBudget
-- -----------------------------------------------------

DROP procedure IF EXISTS `CancelStockOrderForBudget`;

DELIMITER $$

CREATE  PROCEDURE `CancelStockOrderForBudget`()
BEGIN

	DELETE FROM StockTrade Where UserId in ( Select CountryUserId From CountryCode) AND Status in ('P', 'I');	
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure BuyStockOrderForBudget
-- -----------------------------------------------------

DROP procedure IF EXISTS `BuyStockOrderForBudget`;

DELIMITER $$

CREATE  PROCEDURE `BuyStockOrderForBudget`(
	IN parmStockBudgetTypeStart TINYINT(3) ,
	IN parmStockBudgetTypeEnd TINYINT(3) 
)
BEGIN
-- Buying Stock based on invested amount on that bucket for that given stock, buys by market value calculated qty based on 10% increase from market value.
INSERT INTO StockTrade
	SELECT UUID(),null, t3.CountryUserId, t4.StockId, FLOOR(t1.AmountLeft/(t4.CurrentValue * 1.1)), FLOOR(t1.AmountLeft/(t4.CurrentValue * 1.1)), 0,UTC_TIMESTAMP(),'P','M','B', UTC_TIMESTAMP()
	FROM CountryBudgetByType t1, CountryBudget t2, CountryCode t3, Stock t4
	 WHERE t1.TaskId = t2.TaskId
	 AND   t2.CountryId= t3.CountryId
	 AND   t2.StartDate<= UTC_TIMESTAMP()
	 AND   t1.AmountLeft>0
	 AND   t2.EndDate>= UTC_TIMESTAMP()
	 AND   t1.BudgetType>=parmStockBudgetTypeStart 
	 AND   t1.BudgetType<=parmStockBudgetTypeEnd
	 AND   t1.BudgetType-(parmStockBudgetTypeStart-1) = t4.StockId ;
	
	-- Do not Change the Order on BudgetCode or StockCode -6 is to make sure that BudgetType of 7 Gold maps to 1 on STockCode.
	
	
	DELETE FROM StockTrade Where UserId in ( Select CountryUserId From CountryCode) AND Status in ('P', 'I') AND InitialUnit=0;	

-- Add the amount to UserBankAccount for amountLeft
	
	UPDATE UserBankAccount t1
		INNER JOIN(    
    Select t5.CountryUserId, Sum(t2.AmountLeft) TotalLeft FROM   CountryBudget t3 , CountryBudgetByType t2 , CountryCode t5
			WHERE t3.Status = 'A'
			AND MONTH(t3.EndDate) =MONTH(UTC_TIMESTAMP())	
			AND YEAR(t3.EndDate) = YEAR(UTC_TIMESTAMP())			
			AND t2.TaskId = t3.TaskId	
			AND t2.BudgetType>=parmStockBudgetTypeStart 
			AND t2.BudgetType<=parmStockBudgetTypeEnd
			AND t5.CountryId = t3.CountryId
			AND t2.AmountLeft>0
            GROUP BY t2.TaskId		
        ) t4 ON t4.CountryUserId = t1.UserId   
	SET t1.Cash = t1.Cash + (t4.TotalLeft),
	UpdatedAt =UTC_TIMESTAMP()
	;
	
-- Set the AmountLeft to 0 for CountryBudgetType for Stocks
	UPDATE 
		CountryBudgetByType t1		
		JOIN CountryBudget t2 ON t1.TaskId = t2.TaskId
			SET t1.AmountLeft = 0
			Where 
			t2.Status = 'A'
			AND t1.BudgetType>=parmStockBudgetTypeStart 
			AND t1.BudgetType<=parmStockBudgetTypeEnd	
			AND YEAR(t2.EndDate) = YEAR(UTC_TIMESTAMP())			
			AND MONTH(t2.EndDate) = MONTH(UTC_TIMESTAMP());
								
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure IncreaseSalaryBudget
-- -----------------------------------------------------

DROP procedure IF EXISTS `IncreaseSalaryBudget`;

DELIMITER $$

CREATE  PROCEDURE `IncreaseSalaryBudget`(
	IN parmSalaryBudgetType TINYINT(3) ,
	IN parmSeneatorJobCode SMALLINT(5)
)
BEGIN

	UPDATE JobCountry t1
		INNER JOIN(    
		Select  t2.AmountLeft/Sum(t5.Salary) TotalPercentChange, t5.CountryId
			FROM  CountryBudget t3 , 
				  CountryBudgetByType t2, JobCountry t5
			WHERE t3.Status = 'A'
			AND MONTH(t3.EndDate) =MONTH(UTC_TIMESTAMP())
			AND YEAR(t3.EndDate) = YEAR(UTC_TIMESTAMP())
			AND t5.JobCodeId !=parmSeneatorJobCode
			AND t2.TaskId = t3.TaskId	
			AND t3.CountryId = t5.CountryId	
			AND t2.AmountLeft>0
			AND t2.BudgetType =parmSalaryBudgetType					
            GROUP BY  t5.CountryId		
        ) t4 ON t4.CountryId = t1.CountryId  
	SET t1.Salary = t1.Salary *(1+ t4.TotalPercentChange),
		t1.QuantityAvailable = (t1.QuantityAvailable + 1) *(1+ t4.TotalPercentChange)
		Where t1.JobCodeId !=parmSeneatorJobCode
	;
	
-- Set the AmountLeft to 0 for CountryBudgetType for Stocks
	UPDATE 
		CountryBudgetByType t1		
		JOIN CountryBudget t2 ON t1.TaskId = t2.TaskId
			SET t1.AmountLeft = 0
			Where 
			t2.Status = 'A'
			AND t1.BudgetType =parmSalaryBudgetType
			AND YEAR(t2.EndDate) = YEAR(UTC_TIMESTAMP())
			AND MONTH(t2.EndDate) = MONTH(UTC_TIMESTAMP());
								
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GiveEducationCreditForCountry
-- -----------------------------------------------------

DROP procedure IF EXISTS `GiveEducationCreditForCountry`;

DELIMITER $$

CREATE  PROCEDURE `GiveEducationCreditForCountry`(
	IN	parmEducationBudgetType TINYINT(3) ,
	IN 	parmNotificationTypeId SMALLINT,
	IN 	parmPostContentTypeId TINYINT(3),
	IN 	parmEducationFundType TINYINT(3)
)
BEGIN

	DECLARE done INT DEFAULT FALSE;	
	DECLARE percentCredit DECIMAL(10,6);	
	DECLARE parmOutTotalCreditGiven DECIMAL(50,2);	
	DECLARE budgetTaskId CHAR(36);	
	DECLARE currentcountryId CHAR(2);	
	DECLARE countryName VARCHAR(100);	
	
	DECLARE budgetCursor CURSOR  for	
		Select  t2.AmountLeft/Sum(t5.CompletionCost) TotalPercentChange, t3.CountryId, t3.TaskId
			FROM  CountryBudget t3 , 
				  CountryBudgetByType t2, Education t5, WebUser t6
			WHERE t3.Status = 'A'
			AND MONTH(t3.EndDate) = MONTH(Date_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))		
			AND YEAR(t3.EndDate) = YEAR(Date_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))			
			AND MONTH(t5.CreatedAt) =MONTH(Date_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))		
			AND YEAR(t5.CreatedAt) = YEAR(Date_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))		
			AND t2.TaskId = t3.TaskId	
			AND t5.UserId = t6.UserId
			AND	t6.Active = 1		
			AND t2.AmountLeft>0
			AND t3.CountryId = t6.CountryId	
			AND t2.BudgetType =parmEducationBudgetType					
            GROUP BY  t3.CountryId;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN budgetCursor;		
		  myloop: LOOP
					FETCH budgetCursor INTO percentCredit, currentcountryId, budgetTaskId;
					IF done THEN
					  LEAVE myloop;
					END IF;						
					SELECT Code INTO countryName FROM CountryCode WHERE CountryId = currentcountryId;
					
					CALL GiveEducationCreditForUser(parmEducationBudgetType, percentCredit, budgetTaskId, 
					parmEducationFundType, currentcountryId, parmNotificationTypeId, @parmOutTotalCreditGiven );
					
					
					CALL PostAdd(
							0, currentcountryId, null, UUID(),
							'','',0,1,0,0,'',1,0,0,
							CONCAT(countryName,'|',currentcountryId,'|',@parmOutTotalCreditGiven),
							parmPostContentTypeId,
							UTC_TIMESTAMP(6), UTC_TIMESTAMP(6),
							@lastId
							);					

					
				END LOOP;	
  CLOSE budgetCursor;  
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GiveEducationCreditForUser
-- -----------------------------------------------------

DROP procedure IF EXISTS `GiveEducationCreditForUser`;

DELIMITER $$

CREATE  PROCEDURE `GiveEducationCreditForUser`(
	IN parmEducationBudgetType TINYINT(3) ,
	IN parmCreditPercent DECIMAL(10,6)	,
	IN parmBudgetTaskId CHAR(36),	
	IN parmEducationFundType TINYINT(3),	
	IN parmCountryId CHAR(2),
	IN parmNotificationTypeId SMALLINT,
	OUT parmTotalCreditGiven DECIMAL(50,2)
)
BEGIN

	DECLARE done INT DEFAULT FALSE;	
	DECLARE creditAmount DECIMAL(50,2);		
	DECLARE leftBudget DECIMAL(50,2) DEFAULT 0;	
	DECLARE currentUserId INT;	
	DECLARE countryUserId INT;	
	
	DECLARE educationCursor CURSOR  for	
		Select SUM(t1.CompletionCost) * parmCreditPercent TotalCredit, t1.UserId			
			FROM Education t1, WebUser t2
			WHERE t1.UserId = t2.UserId
			AND MONTH(t1.CreatedAt) = MONTH(Date_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))		
			AND YEAR(t1.CreatedAt) = YEAR(Date_SUB(UTC_TIMESTAMP(), INTERVAL 1 MONTH))					
			AND	t2.Active = 1	
			AND t1.CompletionCost>0
			AND t2.CountryId = parmCountryId
            GROUP BY  t1.UserId;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN educationCursor;		
		SELECT CountryUserId INTO countryUserId FROM CountryCode WHERE CountryId = parmCountryId;		
		SET parmTotalCreditGiven  =0;
		  myloop: LOOP
					FETCH educationCursor INTO creditAmount, currentUserId;
					IF done THEN
						  LEAVE myloop;
					END IF;	
					CALL ExecutePayMeWithOutput(currentUserId, CountryUserId, parmCountryId, UUID(), parmEducationFundType, 0, creditAmount, 0, @payresult );
					CALL UserNotificationAdd(
							UUID(), currentUserId,	parmNotificationTypeId,
							2, false, FORMAT(creditAmount,2),0,	UTC_TIMESTAMP(6),@lastId
							);
					SET parmTotalCreditGiven = parmTotalCreditGiven + creditAmount;
				END LOOP;	
  CLOSE educationCursor;  

-- Set the AmountLeft to 0 for CountryBudgetType for Education

	SELECT (AmountLeft - parmTotalCreditGiven) INTO  leftBudget FROM CountryBudgetByType WHERE TaskId = parmBudgetTaskId AND BudgetType =parmEducationBudgetType;
	
	IF leftBudget < 1 THEN
		SET leftBudget = 0;
	END IF;
	
	UPDATE 
		CountryBudgetByType t1				
			SET t1.AmountLeft = leftBudget
			Where 
			t1.TaskId = parmBudgetTaskId
			AND t1.BudgetType =parmEducationBudgetType;
								
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure IncreaseArmyJob
-- -----------------------------------------------------

DROP procedure IF EXISTS `IncreaseArmyJob`;

DELIMITER $$

CREATE  PROCEDURE `IncreaseArmyJob`(
	IN parmArmyBudgetType TINYINT(3) ,
	IN parmArmyJobCode SMALLINT(5)
)
BEGIN

	UPDATE JobCountry t1
		INNER JOIN(    
		Select  t2.AmountLeft/t5.Salary Quantity, t5.CountryId
			FROM  CountryBudget t3 , 
				  CountryBudgetByType t2, JobCountry t5
			WHERE t3.Status = 'A'
			AND MONTH(t3.EndDate) =MONTH(UTC_TIMESTAMP())
			AND YEAR(t3.EndDate) = YEAR(UTC_TIMESTAMP())					
			AND t5.JobCodeId =parmArmyJobCode
			AND t2.TaskId = t3.TaskId	
			AND t3.CountryId = t5.CountryId	
			AND t2.AmountLeft>0
			AND t2.BudgetType =parmArmyBudgetType
        ) t4 ON t4.CountryId = t1.CountryId  
	SET	t1.QuantityAvailable = (t1.QuantityAvailable + t4.Quantity)
	Where t1.JobCodeId =parmArmyJobCode
	;
	
-- Set the AmountLeft to 0 for CountryBudgetType for Stocks
	UPDATE 
		CountryBudgetByType t1		
		JOIN CountryBudget t2 ON t1.TaskId = t2.TaskId
			SET t1.AmountLeft = 0
			Where 
			t2.Status = 'A'
			AND t1.BudgetType =parmArmyBudgetType
			AND YEAR(t2.EndDate) = YEAR(UTC_TIMESTAMP())					
			AND MONTH(t2.EndDate) = MONTH(UTC_TIMESTAMP());
								
	
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure IncreaseLeadersSalary
-- -----------------------------------------------------

DROP procedure IF EXISTS `IncreaseLeadersSalary`;

DELIMITER $$

CREATE  PROCEDURE `IncreaseLeadersSalary`(
	IN parmLeadersJobBudgetType TINYINT(3) ,
	IN parmSeneatorJobCode SMALLINT(5)
)
BEGIN

	UPDATE JobCountry t1
		INNER JOIN(    
		Select   
		t2.AmountLeft/(t5.Salary * (count(*) + 1 ))
		 TotalPercentChange, t5.CountryId
			FROM  CountryBudget t3 , 
				  CountryBudgetByType t2, JobCountry t5, CountryLeader t6
			WHERE t3.Status = 'A'
			AND MONTH(t3.EndDate) =MONTH(UTC_TIMESTAMP())
			AND YEAR(t3.EndDate) = YEAR(UTC_TIMESTAMP())					
			AND t5.JobCodeId =parmSeneatorJobCode
			AND t2.TaskId = t3.TaskId	
			AND t3.CountryId = t5.CountryId	
			AND t6.CountryId = t5.CountryId
			AND t6.StartDate<= UTC_TIMESTAMP() 
			AND t6.EndDate>= UTC_TIMESTAMP()
			AND t2.AmountLeft>0
			AND t2.BudgetType =parmLeadersJobBudgetType
			GROUP By t6.CountryId
        ) t4 ON t4.CountryId = t1.CountryId  
	SET t1.Salary = t1.Salary *(1+ t4.TotalPercentChange)
	Where t1.JobCodeId =parmSeneatorJobCode
	;
	
-- Set the AmountLeft to 0 for CountryBudgetType for Stocks
	UPDATE 
		CountryBudgetByType t1		
		JOIN CountryBudget t2 ON t1.TaskId = t2.TaskId
			SET t1.AmountLeft = 0
			Where 
			t2.Status = 'A'
			AND t1.BudgetType =parmLeadersJobBudgetType
			AND YEAR(t2.EndDate) = YEAR(UTC_TIMESTAMP())					
			AND MONTH(t2.EndDate) = MONTH(UTC_TIMESTAMP());
								
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure SendBudgetImpNotify
-- -----------------------------------------------------

DROP procedure IF EXISTS `SendBudgetImpNotify`;

DELIMITER $$

CREATE  PROCEDURE `SendBudgetImpNotify`(	
	IN 	parmPostContentTypeId TINYINT(3)
)
BEGIN

	DECLARE done INT DEFAULT FALSE;	
	DECLARE currentcountryId CHAR(2);	
	DECLARE countryName VARCHAR(100);	
	
	DECLARE notifyCursor CURSOR  for	
		Select  t3.CountryId, t4.Code
			FROM  CountryBudget t3 , CountryCode t4				 
			WHERE t3.Status = 'A'
			AND MONTH(t3.EndDate) = MONTH(UTC_TIMESTAMP())					
			AND YEAR(t3.EndDate) = YEAR(UTC_TIMESTAMP())					
			AND t3.CountryId = t4.CountryId;
			
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN notifyCursor;		
		  myloop: LOOP
					FETCH notifyCursor INTO  currentcountryId, countryName;
					IF done THEN
					  LEAVE myloop;
					END IF;						
					
					CALL PostAdd(
							0, currentcountryId, null, UUID(),
							'','',0,1,0,0,'',1,0,0,
							CONCAT(countryName,'|',currentcountryId),
							parmPostContentTypeId,
							UTC_TIMESTAMP(6), UTC_TIMESTAMP(6),
							@lastId
							);					

					
				END LOOP;	
  CLOSE notifyCursor;  
	
	 END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure UnFollowFriend
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `UnFollowFriend` $$

CREATE  PROCEDURE `UnFollowFriend`
(	IN parmUserId	INT,
	IN parmFriendId	INT)

BEGIN
  DELETE FROM `Friend`
		Where FollowerUserId = parmUserId 
		AND FollowingUserId =parmFriendId;

END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure UpdateMutalFriend
-- -----------------------------------------------------
-- i am following t1 and t2 and they are both following t3. so add t3
DELIMITER $$
DROP procedure IF EXISTS `UpdateMutalFriend` $$

CREATE  PROCEDURE `UpdateMutalFriend`
(	IN parmUserId	INT,
	IN parmNotificationTypeId SMALLINT
)

BEGIN
Update SuggestFriend  
SET MatchScore =20 WHERE 
	 UserId = parmUserId
 AND UserIgnore =0
 AND SuggestionUserId IN (
	SELECT t3.FollowingUserId
	FROM Friend t1, Friend t2, Friend t3,  Friend t4
	WHERE	t1.FollowerUserId =parmUserId
	AND		t2.FollowerUserId =parmUserId
	AND		t1.FollowingUserId != t2.FollowingUserId
	AND 	t1.FollowingUserId = t3.FollowerUserId
	AND 	t2.FollowingUserId = t4.FollowerUserId
    AND 	t3.FollowingUserId != parmUserId
	AND 	t3.FollowingUserId = t4.FollowingUserId
    AND     t3.FollowingUserId NOT IN (SELECT FollowingUserId FROM
											Friend 
											WHERE FollowerUserId = parmUserId)
                          );           


END$$
DELIMITER ;
-- -----------------------------------------------------
-- procedure AddFriendOfMyFriendSuggestion
-- -----------------------------------------------------
-- friends of friends who's not my friend
DELIMITER $$
DROP procedure IF EXISTS `AddFriendOfMyFriendSuggestion` $$

CREATE  PROCEDURE `AddFriendOfMyFriendSuggestion`
(	IN parmUserId	INT,
	IN parmNotificationTypeId SMALLINT
	)

BEGIN
 INSERT INTO SuggestFriend 
	SELECT parmUserId id, t2.FollowingUserId, 10, false 
	FROM Friend t1
	INNER JOIN Friend t2 ON t1.FollowingUserId = t2.FollowerUserId	
		WHERE t2.FollowingUserId 
		NOT IN (SELECT FollowingUserId FROM
			Friend 
				WHERE FollowerUserId = parmUserId)
		AND t2.FollowingUserId 
		NOT IN (SELECT SuggestionUserId FROM
			SuggestFriend 
				WHERE UserId = parmUserId)
		AND t1.FollowerUserId = parmUserId
		AND t2.FollowingUserId != parmUserId
		GRoup by id, t2.FollowingUserId 
		;
 IF ROW_COUNT() >0 THEN
 
		CALL UserNotificationAdd(
			UUID(), parmUserId,	parmNotificationTypeId,
				2, false, '',	0,UTC_TIMESTAMP(6),@lastId);
 END IF;
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure AddFriendSuggestionByEmail
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `AddFriendSuggestionByEmail` $$

CREATE  PROCEDURE `AddFriendSuggestionByEmail`
(	IN parmUserId	INT,
	IN parmNotificationTypeId SMALLINT
)

BEGIN
Update WebUserContact t1
INNER JOIN WebUser t2 ON t1.FriendEmailId =  t2.EmailId
SET FriendUserId = t2.UserId
Where t1.UserId = parmUserId;

 INSERT INTO SuggestFriend 
	SELECT parmUserId, t1.UserId, 30, false
	FROM WebUser t1, WebUserContact t2
	WHERE	t1.EmailId =t2.FriendEmailId
	AND		t1.Active =1
	AND		t2.UserId =parmUserId
	AND		t2.UserId != t1.UserId
	AND 	t1.UserId 
			NOT IN (SELECT FollowingUserId FROM
					Friend 
					WHERE FollowerUserId = parmUserId)
	AND 	t1.UserId 
			NOT IN (SELECT SuggestionUserId FROM
					SuggestFriend 
					WHERE UserId = parmUserId);
					
 IF ROW_COUNT() >0 THEN
		
		CALL UserNotificationAdd(
			UUID(), parmUserId,	parmNotificationTypeId,
				2, false, '', 0, UTC_TIMESTAMP(6),@lastId);
 END IF;
	

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure FollowAllFriend
-- -----------------------------------------------------

DROP procedure IF EXISTS `FollowAllFriend`;

DELIMITER $$

CREATE  PROCEDURE `FollowAllFriend`( 
	 IN parmUserIdList TEXT,
	 IN parmUserId INT
	)
BEGIN
DECLARE  syscurrent_date DATETIME(6);
DECLARE result INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SELECT result;
END;

SET @separatorLength = 1;
SET syscurrent_date = UTC_TIMESTAMP(6);
 
WHILE parmUserIdList != '' > 0 DO
    SET @userId = SUBSTRING_INDEX(parmUserIdList, ',', 1);
	SET parmUserIdList = SUBSTRING(parmUserIdList, CHAR_LENGTH(@userId) + @separatorLength + 1);	
		INSERT INTO Friend
			VALUES (parmUserId,@userId,syscurrent_date)
			
			ON DUPLICATE KEY UPDATE
				FollowingUserId = parmUserId,
				FollowerUserId = @userId;
				
		SET result = result +1;

	END WHILE;
	SELECT result;
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure BlockFollower
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `BlockFollower` $$

CREATE  PROCEDURE `BlockFollower`
(	IN parmUserId	INT,
	IN parmFriendId	INT)

BEGIN
  DELETE FROM `Friend`
		Where FollowerUserId = parmFriendId 
		AND FollowingUserId =parmUserId;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetFriendSuggestion
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetFriendSuggestion`;

DELIMITER $$

CREATE  PROCEDURE `GetFriendSuggestion`(
		IN parmUserId	INT,
		IN parmLimit INT,
		IN parmLastSuggestionUserId INT
		)
BEGIN

	SELECT
	 t2.UserId,
	 CONCAT( NameFirst, ' ',  NameLast) FullName,	 
	 Picture, 
	 CountryId
	FROM SuggestFriend t1, WebUser t2
	WHERE
		t1.UserId = parmUserId
	AND t2.UserId = t1.SuggestionUserId
	AND t2.Active =1
	AND UserIgnore = 0
	AND t1.SuggestionUserId > parmLastSuggestionUserId
	Order By t1.MatchScore desc, t1.SuggestionUserId asc LIMIT parmLimit
	;
	
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure RemoveFriendSuggestion
-- -----------------------------------------------------

DROP procedure IF EXISTS `RemoveFriendSuggestion`;

DELIMITER $$

CREATE  PROCEDURE `RemoveFriendSuggestion`(
		IN parmUserId	INT,		
		IN parmSuggestionUserId TEXT
		)
BEGIN
	
	 set @concatsql = CONCAT('DELETE FROM SuggestFriend WHERE  SuggestionUserId  in (', parmSuggestionUserId, ') 
	 AND UserId =',parmUserId);
	 PREPARE q FROM @concatsql;
	execute q;
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure IgnoreFriendSuggestion
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `IgnoreFriendSuggestion` $$

CREATE  PROCEDURE `IgnoreFriendSuggestion`
(	IN parmUserId	INT,
	IN parmSuggestionUserId	INT)

BEGIN
  UPDATE SuggestFriend
  SET UserIgnore =1
		Where UserId = parmUserId 
		AND SuggestionUserId =parmSuggestionUserId;

END$$
DELIMITER ;

	
-- -----------------------------------------------------
-- procedure IsThisFirstLogin
-- -----------------------------------------------------

DROP procedure IF EXISTS `IsThisFirstLogin`;
DELIMITER $$
CREATE  PROCEDURE `IsThisFirstLogin`(
			IN parmUserId INT
)
BEGIN
Select 
	if (Count(*)=0,'Y','N') IsFirstLogin
	FROM UserActivityLog
	WHERE 
		UserId = parmUserId  
	AND TIMESTAMPDIFF(SECOND, FirstLogin, UTC_TIMESTAMP())>60;
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure SaveUserActivityLog
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SaveUserActivityLog` $$
CREATE PROCEDURE `SaveUserActivityLog` (
	 IN parmUserId INT(11),
	 IN parmIPAddress VARCHAR(45),
	 IN parmLastLogin DATETIME(6),
	 IN parmFirstLogin DATETIME(6),
	 IN parmHit INT	
)


	BEGIN 

		INSERT INTO UserActivityLog( UserId, IPAddress,Hit, LastLogin,  FirstLogin)
		VALUES (parmUserId,parmIPAddress, parmHit, parmLastLogin, parmFirstLogin )


	ON DUPLICATE KEY UPDATE
				LastLogin = parmLastLogin,			
				Hit = Hit + parmHit
				
;

END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetOfflineNotificationById
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetOfflineNotificationById`;

DELIMITER $$

CREATE  PROCEDURE `GetOfflineNotificationById`( 
		IN   parmNotificationTypeId SMALLINT
	)
BEGIN
 SELECT   
	*
 FROM NotificationType
 WHERE
		NotificationTypeId =parmNotificationTypeId
 AND	EmailNotification =1;   
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetPendingLoanPayment
-- -----------------------------------------------------

DELIMITER $$
DROP procedure IF EXISTS `GetPendingLoanPayment` $$

CREATE  PROCEDURE `GetPendingLoanPayment`
( 	IN parmLendorId INT ,
	IN parmUserId INT 
)

BEGIN
  SELECT COALESCE(Sum(LeftAmount), 0) TotalLeftAmount from UserLoan 
  WHERE Status ='A'
	AND	LendorId = parmLendorId
	AND UserId = parmUserId
  ;

END$$
DELIMITER ; 
-- -----------------------------------------------------
-- procedure GetInvitationSender
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetInvitationSender`;
DELIMITER $$
CREATE  PROCEDURE `GetInvitationSender`( 
	IN parmInvitationId CHAR(36),
	IN parmEmailId VARCHAR(100)
	
	
)
BEGIN
 SELECT  t1.*
    FROM WebUserContact t1 
	WHERE InvitationId = parmInvitationId
	AND FriendEmailId = parmEmailId
	; 	
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure PayStockDividend
-- -----------------------------------------------------

DROP procedure IF EXISTS `PayStockDividend`;

DELIMITER $$

CREATE  PROCEDURE `PayStockDividend`(
	IN parmDividendRate DECIMAL(5,2) ,
	IN parmNotificationTypeId SMALLINT,		
	IN parmTaxCode TINYINT(3),			
	IN parmBankId INT,		
	IN parmFundType  TINYINT(3),		 
	IN parmDividendCap DECIMAL(5,2)
)
BEGIN

	DECLARE done INT DEFAULT FALSE;	
	DECLARE creditAmount DECIMAL(50,2);		
	DECLARE currentUserId INT;	
	DECLARE taxPercent TINYINT(3);		
	DECLARE countryId CHAR(2);	
	
	DECLARE dividenCursorCursor CURSOR  for	
	Select Sum(CreditByStock) TotalCredit, t6.TaxPercent, t3.UserId, t4.CountryId from (
		Select SUM(t1.PurchasedUnit) * LEAST( t2.CurrentValue * parmDividendRate /100, parmDividendCap) CreditByStock
		, t1.UserId, t1.StockId			
			FROM UserStock t1, Stock t2
			Where t1.StockId = t2.StockId
            GROUP BY  t1.UserId, t1.StockId) t3 , WebUser t4, CountryTax t5, CountryTaxByType t6
            WHERE 	t3.UserId = t4.UserId
            AND 	t4.Active =1
            AND		t5.CountryId = t4.CountryId
            AND 	t5.Status ='A'
			AND 	t5.StartDate<=UTC_TIMESTAMP(6)
			AND 	t5.EndDate>=UTC_TIMESTAMP(6)
            AND		t5.TaskId = t6.TaskId
            AND		t6.TaxType = parmTaxCode
            Group by t3.UserId;

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN dividenCursorCursor;						
		  myloop: LOOP
					FETCH dividenCursorCursor INTO creditAmount, taxPercent, currentUserId, countryId;
					IF done THEN
						  LEAVE myloop;
					END IF;	
					CALL ExecutePayMeWithOutput(currentUserId, parmBankId, countryId, UUID(), parmFundType, parmTaxCode, (1-  taxPercent/100) * creditAmount,creditAmount * taxPercent/100, @payresult );
			IF	@payResult = 1 THEN					
					CALL UserNotificationAdd(
							UUID(), currentUserId,	parmNotificationTypeId,
							2, false, CONCAT(FORMAT((1-  taxPercent/100) * creditAmount,2), '|',FORMAT( creditAmount * taxPercent/100,2) ),	0, UTC_TIMESTAMP(6),@lastId
							);
			END IF;						
				END LOOP;	
	CLOSE dividenCursorCursor;  
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure RunPayRoll
-- -----------------------------------------------------

DROP procedure IF EXISTS `RunPayRoll`;

DELIMITER $$

CREATE  PROCEDURE `RunPayRoll`(
	IN parmWorkingDays DECIMAL(9,6) ,
	IN parmLastPayCheckDate DateTime ,
	IN parmCurrentPayCheckDate DateTime ,
	IN parmNotificationTypeId SMALLINT,		
	IN parmTaxCode TINYINT(3),
	IN parmBankId INT,
	IN parmFundType  TINYINT(3)
)
BEGIN

	DECLARE done INT DEFAULT FALSE;	
	DECLARE creditAmount DECIMAL(50,2);		
	DECLARE currentUserId INT;	
	DECLARE taxPercent TINYINT(3);		
	DECLARE countryId CHAR(2);	
	DECLARE overtimehr SMALLINT;
	DECLARE taskId CHAR(36);
	
	DECLARE payCheckCursorCursor CURSOR  for	
	Select  ((parmWorkingDays * t3.Salary * t3.MaxHPW/7) + (t3.Salary * t3.OverTimeRate * t3.ThisCycleOverTimeHr))  TotalPayCheck,
			t6.TaxPercent, t3.UserId, t4.CountryId, t3.ThisCycleOverTimeHr, t3.TaskId from (
		Select t1.UserId, t1.TaskId, (t1.OverTimeHours - t1.LastCycleOverTimeHours) ThisCycleOverTimeHr, t2.MaxHPW, t2.OverTimeRate, t1.JobCodeId, t1.Salary
			FROM UserJob t1, JobCode t2
			Where 	t1.JobCodeId = t2.JobCodeId
			AND 	t1.StartDate <= parmCurrentPayCheckDate
			AND		t1.EndDate > parmLastPayCheckDate
			AND 	t1.Status ='A'
            ) t3 , WebUser t4, CountryTax t5, CountryTaxByType t6
            WHERE 	t3.UserId = t4.UserId
            AND 	t4.Active =1
            AND		t5.CountryId = t4.CountryId			
            AND 	t5.Status ='A'
			AND 	t5.StartDate<=UTC_TIMESTAMP(6)
			AND 	t5.EndDate>=UTC_TIMESTAMP(6)
            AND		t5.TaskId = t6.TaskId
            AND		t6.TaxType = parmTaxCode;
            

	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;	

	  OPEN payCheckCursorCursor;						
		  myloop: LOOP
					FETCH payCheckCursorCursor INTO creditAmount, taxPercent, currentUserId, countryId, overtimehr, taskId;
					IF done THEN
						  LEAVE myloop;
					END IF;	
					CALL ExecutePayMeWithOutput(currentUserId, parmBankId, countryId, UUID(), parmFundType, parmTaxCode, (1-  taxPercent/100) * creditAmount,creditAmount * taxPercent/100, @payresult );
			IF	@payResult = 1 THEN					
					CALL UserNotificationAdd(
							UUID(), currentUserId,	parmNotificationTypeId,
							2, false, CONCAT(FORMAT((1-  taxPercent/100) * creditAmount,2), '|',FORMAT(parmWorkingDays,2) ,'|',FORMAT( creditAmount * taxPercent/100,2),'|',overtimehr),0,UTC_TIMESTAMP(6),@lastId
							);
			END IF;						
				END LOOP;	
	CLOSE payCheckCursorCursor; 
			
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateUserJobAfterPayRoll
-- -----------------------------------------------------

DROP procedure IF EXISTS `UpdateUserJobAfterPayRoll`;

DELIMITER $$

CREATE  PROCEDURE `UpdateUserJobAfterPayRoll`(
	IN parmWorkingDays DECIMAL(9,6) ,
	IN parmLastPayCheckDate DateTime ,
	IN parmCurrentPayCheckDate DateTime ,	
	IN parmTaxCode TINYINT(3)
)
BEGIN
Update 	UserJob t8		
		INNER Join		
			(Select  ((parmWorkingDays * t3.Salary * t3.MaxHPW/7) + (t3.Salary * t3.OverTimeRate * 			t3.ThisCycleOverTimeHr))  TotalPayCheck,
				t6.TaxPercent, t3.UserId, t4.CountryId, t3.ThisCycleOverTimeHr, t3.TaskId from (
				Select t1.UserId, t1.TaskId, (t1.OverTimeHours - t1.LastCycleOverTimeHours) ThisCycleOverTimeHr, t2.MaxHPW, t2.OverTimeRate, t1.JobCodeId, t1.Salary
				FROM UserJob t1, JobCode t2
				Where 	t1.JobCodeId = t2.JobCodeId
				AND 	t1.StartDate <= parmCurrentPayCheckDate
				AND		t1.EndDate > parmLastPayCheckDate
				AND 	t1.Status ='A'
				) t3 , WebUser t4, CountryTax t5, CountryTaxByType t6
				WHERE 	t3.UserId = t4.UserId
				AND 	t4.Active =1
				AND		t5.CountryId = t4.CountryId			
				AND 	t5.Status ='A'
				AND 	t5.StartDate<=UTC_TIMESTAMP(6)
				AND 	t5.EndDate>=UTC_TIMESTAMP(6)
				AND		t5.TaskId = t6.TaskId
				AND		t6.TaxType = parmTaxCode) t7 ON t8.TaskId = t7.TaskId
		SET IncomeYearToDate = IncomeYearToDate + ((1-  t7.TaxPercent/100) * t7.TotalPayCheck),
			LastCycleOverTimeHours = OverTimeHours;
			
			
	 END$$
DELIMITER ;



-- -----------------------------------------------------
-- procedure UpdateEmailSentStatus
-- -----------------------------------------------------

DROP procedure IF EXISTS `UpdateEmailSentStatus`;

DELIMITER $$

CREATE  PROCEDURE `UpdateEmailSentStatus`( 
	IN parmEmailSent TINYINT(1),
	IN parmTaskIdList TEXT
	)
BEGIN
		set @concatsql = CONCAT('UPDATE UserNotification SET EmailSent =', parmEmailSent,
		 ', UpdatedAt =UTC_TIMESTAMP(6) WHERE NotificationId IN (', parmTaskIdList, ' )');
	
	PREPARE q FROM @concatsql;
	execute q;
	
	 END$$
DELIMITER ;



-- -----------------------------------------------------
-- procedure GetNewNotificationThatNeedsToBeEmailed
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetNewNotificationThatNeedsToBeEmailed`;

DELIMITER $$

CREATE  PROCEDURE `GetNewNotificationThatNeedsToBeEmailed`(
	IN parmTimeLimit TINYINT(2),
	IN parmEmailSentTime DateTime(6)

 )
BEGIN
		SELECT t1.NotificationTypeId, t1.Parms,
		t1.UserId, 
		NameFirst ,
		EmailId        
		FROM UserNotification t1, WebUser t2, ( SELECT max(LastLogin) MaxLastLogin, UserId FROM UserActivityLog GROUP BY UserId ) t3, NotificationType t4		
		WHERE UpdatedAt >  t3.MaxLastLogin
		AND t1.NotificationTypeId = t4.NotificationTypeId
		AND t4.EmailNotification =1
		AND UpdatedAt <= parmEmailSentTime
        AND	t1.UserId = t3.UserId
        AND (time_to_sec(timediff(parmEmailSentTime, t3.MaxLastLogin )) / 3600) > parmTimeLimit
		AND t1.UserId = t2.UserId	
		AND t2.Active = 1
		AND EmailSent =0;
	
	 END$$
DELIMITER ;




-- -----------------------------------------------------
-- procedure UpdateEmailSentByTime
-- -----------------------------------------------------

DROP procedure IF EXISTS `UpdateEmailSentByTime`;

DELIMITER $$

CREATE  PROCEDURE `UpdateEmailSentByTime`( 
	IN parmEmailSent TINYINT(1),
	IN parmTimeLimit TINYINT(2),
	IN parmEmailSentTime DateTime(6)
)
BEGIN

	UPDATE UserNotification t1
	INNER Join (SELECT max(LastLogin) MaxLastLogin, UserId FROM UserActivityLog GROUP BY UserId ) t3 ON 	
			t1.UserId = t3.UserId	
	INNER Join	NotificationType t4	ON t1.NotificationTypeId = t4.NotificationTypeId
		SET EmailSent = parmEmailSent
		WHERE t1.UpdatedAt >  t3.MaxLastLogin
		AND t1.UpdatedAt <= parmEmailSentTime  		
		AND t4.EmailNotification =1		
        AND (time_to_sec(timediff(parmEmailSentTime, t3.MaxLastLogin )) / 3600) > parmTimeLimit
		AND EmailSent =0 ;
	
	 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure SendStcokForecastNotification
-- -----------------------------------------------------

DROP procedure IF EXISTS `SendStcokForecastNotification`;

DELIMITER $$

CREATE  PROCEDURE `SendStcokForecastNotification`(
	IN parmNotificationTypeId SMALLINT,
	IN parmPostContentTypeId TINYINT(3)	
)
BEGIN
	
	INSERT INTO UserNotification
		SELECT UUID(), UserId, parmNotificationTypeId, 2, false, ' ',0,UTC_TIMESTAMP() 
			FROM UserStock 
			WHERE PurchasedUnit >0 Group BY UserId;
	
	CALL PostAddUpdate(
		0, null, null, UUID(),
		'','',0,1,0,0,'',1,0,0,
		' ',
		parmPostContentTypeId,
		UTC_TIMESTAMP(0), UTC_TIMESTAMP(0),
		@lastId
		);			

			
	 END$$
DELIMITER ;



-- -----------------------------------------------------
-- procedure ExecuteStealProperty
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecuteStealProperty`;

DELIMITER $$

CREATE  PROCEDURE `ExecuteStealProperty`(        		 
		 IN parmUserId INT,		
		 IN parmVictimId INT,		
		 IN parmRobberId INT,			 
		 IN parmMerchandiseTypeId  TINYINT(3),	 		 		 
		 IN parmTaskId CHAR(36),		 
		 IN parmQuantity  TINYINT(3)
    )
BEGIN
/*
0-Not Enough Quantity
1-SuccessFull
2-DB error
*/
DECLARE totalQuantityLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalQuantityNew   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalPurchasedPrice   DECIMAL(50,2) DEFAULT -1 ;
DECLARE merchandiseConditionNow   TINYINT(3) UNSIGNED DEFAULT 0 ;
DECLARE result INT DEFAULT 0;

START TRANSACTION;
 Select Quantity - parmQuantity, MerchandiseCondition, PurchasedPrice * parmQuantity INTO  totalQuantityLeft, merchandiseConditionNow, totalPurchasedPrice from UserMerchandise
 Where Quantity>=(parmQuantity) and UserId = parmVictimId and  MerchandiseTypeId = parmMerchandiseTypeId LOCK IN SHARE MODE;
 
 Select Quantity + parmQuantity INTO  totalQuantityNew from UserMerchandise
		 Where  UserId = parmUserId 
		 AND  MerchandiseTypeId = parmMerchandiseTypeId  LOCK IN SHARE MODE;

  
	IF totalQuantityLeft>=0 AND parmUserId>0 THEN
	
			UPDATE `UserMerchandise`
			SET `Quantity` = totalQuantityLeft,
			`Quantity` = totalQuantityLeft
			WHERE `UserId` = parmVictimId
			AND  MerchandiseTypeId = parmMerchandiseTypeId;

		IF totalQuantityNew>=parmQuantity THEN
			UPDATE `UserMerchandise`
			SET 
			MerchandiseCondition =(MerchandiseCondition * Quantity + merchandiseConditionNow * parmQuantity)/(parmQuantity + Quantity),
			Quantity = totalQuantityNew
			WHERE `UserId` = parmUserId
			AND  MerchandiseTypeId = parmMerchandiseTypeId;
		ELSE
			INSERT INTO UserMerchandise
				SELECT MerchandiseTypeId, parmUserId, parmQuantity, MerchandiseCondition, PurchasedPrice, 0, UTC_TIMESTAMP()
				FROM UserMerchandise 
				WHERE 	UserId = parmVictimId
				AND		MerchandiseTypeId = parmMerchandiseTypeId 	;
		
		END IF;
			
		
		INSERT INTO CrimeIncident (IncidentId, UserId, VictimId, Amount, IncidentDate, MerchandiseTypeId, IncidentType) VALUES (parmTaskId, parmUserId, parmVictimId, totalPurchasedPrice , UTC_TIMESTAMP(6), parmMerchandiseTypeId,'P');
		
		UPDATE CrimeReport
		SET  IncidentCount = IncidentCount + 1,
		LootToDate = LootToDate + totalPurchasedPrice,
		UpdatedAt = UTC_TIMESTAMP()
		WHERE UserId = parmUserId;
		

						
			

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure ExecutePickPocket
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ExecutePickPocket`;

DELIMITER $$

CREATE  PROCEDURE `ExecutePickPocket`(        		 
		 IN parmUserId INT,		
		 IN parmVictimId INT,		
		 IN parmRobberId INT,			 
		 IN parmFundType  TINYINT(3),		 		 
		 IN parmTaskId CHAR(36),		 
		 IN parmAmount DECIMAL(50,2)	
    )
BEGIN
/*
0-Not Enough Cash
1-SuccessFull
2-DB error
*/
DECLARE totalCashLeft   DECIMAL(50,2) DEFAULT -1 ;
DECLARE totalCashNew   DECIMAL(50,2) DEFAULT -1 ;
DECLARE result INT DEFAULT 0;
DECLARE EXIT HANDLER FOR SQLEXCEPTION 
BEGIN
SET result = 2;
ROLLBACK;
 select result;
END;


START TRANSACTION;
 Select Cash-parmAmount INTO  totalCashLeft from UserBankAccount
 Where Cash>=(parmAmount) and UserId = parmVictimId LOCK IN SHARE MODE;
 
 Select Cash+parmAmount INTO  totalCashNew from UserBankAccount
		 Where  UserId = parmUserId LOCK IN SHARE MODE;

  
	IF totalCashLeft>=0 AND parmUserId>0 THEN
	
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashLeft,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmVictimId;

		
			UPDATE `UserBankAccount`
			SET `Cash` = totalCashNew,
			UpdatedAt =UTC_TIMESTAMP()
			WHERE `UserId` = parmUserId;

			
		INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			( UUID(), parmVictimId, parmRobberId, parmAmount, 0, parmFundType, UTC_TIMESTAMP(6));
		
		INSERT INTO `CapitalTransactionLog`
			(`LogId`, `SourceId`,`RecipentId`,`Amount`,TaxAmount,`FundType`,`CreatedAT`) VALUES
			( UUID(), parmRobberId, parmUserId, parmAmount, 0, parmFundType, UTC_TIMESTAMP(6));			
		
		INSERT INTO CrimeIncident (IncidentId, UserId, VictimId, Amount, IncidentDate,IncidentType) VALUES (parmTaskId, parmUserId, parmVictimId, parmAmount, UTC_TIMESTAMP(6),'C');
		
		UPDATE CrimeReport
		SET IncidentCount = IncidentCount + 1,
		LootToDate = LootToDate + parmAmount,
		UpdatedAt = UTC_TIMESTAMP()
		WHERE UserId = parmUserId;
		

						
			COMMIT;

			SET result = 1;
	ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
COMMIT;

END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure ReportSuspectToAuthority
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ReportSuspectToAuthority`;

DELIMITER $$

CREATE  PROCEDURE `ReportSuspectToAuthority`(        		 
		 IN parmSuspectId INT,
		 IN parmIncidentId CHAR(36),		 
		 IN parmWantedScoreFactor DECIMAL(5,4)
		 
    )
BEGIN
UPDATE CrimeReport t1
	INNER JOIN CrimeIncident t2 ON t1.UserId =  t2.UserId and t2.IncidentId = parmIncidentId
	SET SuspectCount = SuspectCount + 1,
	WantedScore	 = WantedScore + parmWantedScoreFactor
	Where  t1.UserId = parmSuspectId;
	
	

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure NotRobbedInLastNDayByUser
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `NotRobbedInLastNDayByUser`;

DELIMITER $$

CREATE  PROCEDURE `NotRobbedInLastNDayByUser`(        		 		 
		 IN parmRobberyFrequencyonSamePersonCap SMALLINT,
		 IN parmUserId INT, 
		 IN parmVictimId INT
    )
BEGIN
	
		SELECT
	CASE WHEN DATEDIFF(UTC_TIMESTAMP(), MAX(IncidentDate))< 
		parmRobberyFrequencyonSamePersonCap THEN 1
		ELSE 0		
	END  as Result
	FROM CrimeIncident
			WHERE 	UserId = parmUserId
			AND		VictimId = parmVictimId
			AND 	IncidentType !='T';
	
	

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure ArrestUser
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `ArrestUser`;

DELIMITER $$

CREATE  PROCEDURE `ArrestUser`(
		 IN parmNewNetWorth DECIMAL (50,2),
		 IN parmUserId INT
		 
    )
BEGIN
	
	UPDATE UserBankAccount
		SET	Cash = parmNewNetWorth,
		Gold =0,
		Silver =0
	WHERE UserId = parmUserId;
	
	DELETE FROM UserStock		
	WHERE UserId = parmUserId;
	
	DELETE FROM UserMerchandise		
	WHERE UserId = parmUserId;
	
	UPDATE CrimeReport
		SET	WantedScore = 0,
		ArrestCount = ArrestCount + 1,		
		LastArrestDate = UTC_TIMESTAMP(),
		UpdatedAt = UTC_TIMESTAMP()
	WHERE UserId = parmUserId;
	
	DELETE FROM InJail		
	WHERE UserId = parmUserId;
	
	-- ToDO if we redistribute money to victime need to delete this later
	DELETE FROM CrimeIncident		
	WHERE UserId = parmUserId;

END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure CrimeWatchWantedJob
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `CrimeWatchWantedJob`;

DELIMITER $$

CREATE  PROCEDURE `CrimeWatchWantedJob`( 
 IN parmIncidentPerDayRate DECIMAL( 5,4),
 IN parmDeductionPerDayRate DECIMAL( 5,4),
 IN parmMaxWantedLevel DECIMAL( 5,4)
 )
BEGIN
	DECLARE done INT DEFAULT FALSE;	
	DECLARE newWantedScore DECIMAL(5,4);
	DECLARE currentIncidentPerDay INT;
	DECLARE currentUserId INT;
	
	DECLARE crimeCursor CURSOR  for	
		SELECT Count(*)/ SUM(DATEDIFF(UTC_TIMESTAMP(), t1.IncidentDate)) IncidentPerDay, t1.UserId , WantedScore
		FROM CrimeIncident t1, CrimeReport t2		
		WHERE 	t1.UserId = t2.UserId
		AND		t1.IncidentDate> t2.LastArrestDate
		GROUP BY t1.UserId; 
		
	DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;		
	
OPEN crimeCursor;		
		  myloop: LOOP
					FETCH crimeCursor INTO currentIncidentPerDay, currentUserId, newWantedScore;
					IF done THEN
					  LEAVE myloop;
					END IF;						
				SET newWantedScore = newWantedScore + currentIncidentPerDay * parmIncidentPerDayRate;
				
				IF newWantedScore >= parmMaxWantedLevel THEN
				  INSERT INTO InJail VALUES(currentUserId);
				ELSE
					UPDATE CrimeReport
					SET WantedScore = WantedScore - parmDeductionPerDayRate
					WHERE UserId = currentUserId;
				END IF;
						
				END LOOP;	
  CLOSE crimeCursor;  
END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure IsMyFriend
-- -----------------------------------------------------

DROP procedure IF EXISTS `IsMyFriend`;

DELIMITER $$

CREATE  PROCEDURE `IsMyFriend`(
	IN parmUserId INT,
	IN parmFriendId INT

 )
BEGIN
		SELECT COUNT(*) Cnt
		FROM Friend
		WHERE (FollowerUserId =parmUserId AND  FollowingUserId =  parmFriendId) OR
		(FollowerUserId =parmFriendId AND  FollowingUserId =  parmUserId)
		;
	
	 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetCrimeReportByIncident
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetCrimeReportByIncident`;
DELIMITER $$
CREATE  PROCEDURE `GetCrimeReportByIncident`( 
	IN	parmIncidentId CHAR(36)
	)
BEGIN
 SELECT t1.VictimId,
		t1.Amount,
		t1.IncidentDate,
		t1.MerchandiseTypeId,
		t2.Name,
		t2.Description,
		t2.ImageFont,
		t2.MerchandiseTypeCode,
		t1.IncidentType
    FROM CrimeIncident t1 
	LEFT JOIN MerchandiseType t2 ON t1.MerchandiseTypeId = t2.MerchandiseTypeId
	WHERE	t1.IncidentId =	parmIncidentId;
	
	
	
 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetCrimeReportByUser
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetCrimeReportByUser`;
DELIMITER $$
CREATE  PROCEDURE `GetCrimeReportByUser`( 
	IN	parmUserId CHAR(36)
	)
BEGIN
 SELECT t1.*
    FROM CrimeReport t1 	
	WHERE	t1.UserId =	parmUserId;
	
	
	
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetAllUserInJail
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetAllUserInJail`;
DELIMITER $$
CREATE  PROCEDURE `GetAllUserInJail`( 
	
	)
BEGIN
 SELECT t2.UserId, Count(*) TotalIncident
    FROM InJail t1, CrimeIncident t2, CrimeReport t3		
	Where t1.UserId = t2.UserId
	AND t3.UserId = t2.UserId
	AND	t2.IncidentDate> t3.LastArrestDate
	Group By t2.UserId
 	;
		
 END$$
DELIMITER ;


-- -----------------------------------------------------
-- procedure GetUserThatHasLowSocialAsset
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetUserThatHasLowSocialAsset`;
DELIMITER $$
CREATE  PROCEDURE `GetUserThatHasLowSocialAsset`( 
	IN	parmMinimumInviteSent MEDIUMINT,
	IN	parmMinimumNumberOfFriends MEDIUMINT
	)
BEGIN
Select DISTINCT t4.UserId, t4.EmailId, t4.NameFirst  FROM (
 Select UserId, EmailId, NameFirst from WebUser Where UserId NOT IN (
	Select t1.FollowerUserId UserId FROM		 
		 (SELECT t1.FollowerUserId , Count(*) Cnt
			FROM Friend t1		
			GROUP BY t1.FollowerUserId ) t1,
			
		 (SELECT t1.FollowingUserId , Count(*) Cnt
			FROM Friend t1		
			GROUP BY t1.FollowingUserId ) t2
			
		  WHERE t1.FollowerUserId = t2.FollowingUserId
		  AND (t1.Cnt + t2.Cnt)> parmMinimumNumberOfFriends			
			) 
	And Active =1

UNION ALL

  Select UserId, EmailId, NameFirst from WebUser Where UserId NOT IN (
		 SELECT t1.UserId
			FROM WebUserContact t1
			WHERE t1.FriendUserId>0	
			AND t1.InvitationId <> '00000000-0000-0000-0000-000000000000'
			GROUP BY t1.UserId HAVING Count(*) > parmMinimumInviteSent) 
	And Active =1) t4
GROUP BY t4.UserId HAVING COUNT(*) >=2
    
    ;
	
		
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure GetUserWithExpiringJobs
-- -----------------------------------------------------

DROP procedure IF EXISTS `GetUserWithExpiringJobs`;
DELIMITER $$
CREATE  PROCEDURE `GetUserWithExpiringJobs`( 
		IN	parmDayRange TINYINT(2)
	)
BEGIN
 SELECT t1.UserId, t3.Title, t1.EndDate, t1.Salary, t1.IncomeYearToDate, t1.TaskId
    FROM UserJob t1, JobCode t3
	Where t1.JobCodeId = t3.JobCodeId
	AND 
	(
		(
		t1.EndDate<= DATE_ADD(UTC_TIMESTAMP(),INTERVAL parmDayRange DAY)
		AND t1.EndDate>= DATE_SUB(UTC_TIMESTAMP(),INTERVAL parmDayRange DAY) 
		)
	 OR	
		(t1.JobExiredEmailSent = 0 
		 AND t1.EndDate<=UTC_TIMESTAMP())	
	)
	AND t1.Status = 'A'
	
 	;
		
 END$$
DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateExpireJobEmailSent
-- -----------------------------------------------------


DROP procedure IF EXISTS `UpdateExpireJobEmailSent`;

DELIMITER $$

CREATE  PROCEDURE `UpdateExpireJobEmailSent`(
         IN  parmTaskId TEXT
    )
BEGIN
 set @concatsql = CONCAT('Update UserJob 
	SET JobExiredEmailSent = 1
	WHERE TaskId IN (', parmTaskId, ')');
PREPARE q FROM @concatsql;
execute q;
 END$$

DELIMITER ;