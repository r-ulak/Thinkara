
-- -----------------------------------------------------
-- Table `AllowedWebUser`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `AllowedWebUser` ;

CREATE  TABLE IF NOT EXISTS `AllowedWebUser` (  
  `EmailId` VARCHAR(100)  NOT NULL ,  
  PRIMARY KEY (`EmailId`)  
  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
 -- -----------------------------------------------------
-- Table `UserActivityLog`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserActivityLog` ;

CREATE  TABLE IF NOT EXISTS `UserActivityLog` (  
  `UserId` INT ,      
  `IPAddress` VARCHAR(45) ,
  `Hit` INT,  
  `LastLogin` DATETIME NOT NULL,     
  `FirstLogin` DATETIME NOT NULL,  
  PRIMARY KEY (`UserId`, `IPAddress`),
  INDEX `ik_useractivitylog_userid` (`UserId` ASC),
  INDEX `ik_useractivitylog_lastlogin` (`LastLogin` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
-- -----------------------------------------------------
-- Table `WebUser`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `WebUser` ;

CREATE  TABLE IF NOT EXISTS `WebUser` (
  `UserId` INT   NOT NULL AUTO_INCREMENT ,  
  `NameFirst` VARCHAR(45)  NOT NULL ,  
  `NameLast` VARCHAR(45)  NOT NULL ,
  `EmailId` VARCHAR(100)  NOT NULL ,
  `Picture` VARCHAR(255)  NOT NULL DEFAULT '/web/image/default.jpg' ,
  `Active` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `OnlineStatus` TINYINT(2) NULL DEFAULT '0' ,    
  `CountryId` Char(2) NULL DEFAULT '' ,
  `UserLevel` INT NULL DEFAULT 1 ,  
  `CreatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `ik_webuser_emailid` (`EmailId`)
  )
ENGINE = InnoDB
AUTO_INCREMENT=20000
DEFAULT CHARACTER SET = latin1
;
INSERT INTO `WebUser`
(`UserId`,`NameFirst`,`NameLast`,`EmailId`,`Picture`,`Active`,`OnlineStatus`,`CountryId`,`UserLevel`)
VALUES (10001,'Bank','Of Geni','bank@planetgeni.com','bank3.png',0,1,'us',1);
INSERT INTO `WebUser`
(`UserId`,`NameFirst`,`NameLast`,`EmailId`,`Picture`,`Active`,`OnlineStatus`,`CountryId`,`UserLevel`)
VALUES (10000,'Outlaws','Of Geni','Outlaws@planetgeni.com','robber4.png',0,1,'us',1);


-- -----------------------------------------------------
-- Table `PostContentType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PostContentType` ;

CREATE  TABLE IF NOT EXISTS `PostContentType` (
  `PostContentTypeId`  TINYINT(3) NOT NULL ,      
  `ShortDescription` VARCHAR(200) NOT NULL DEFAULT ' ' ,    
  `Description` VARCHAR(1000)  NOT NULL ,
  `ImageFont` VARCHAR(50) NOT NULL  ,
  `FontCss` VARCHAR(50) NOT NULL  ,
  PRIMARY KEY (`PostContentTypeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `Post`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Post` ;
CREATE  TABLE IF NOT EXISTS `Post` (
  `UserId` INT  NULL,
  `CountryId` CHAR(2) NULL ,
  `PartyId` CHAR(36) NULL ,  
  `PostId` CHAR(36) NOT NULL,
  `PostContent` LONGTEXT NULL,
  `PostTitle` TEXT  NULL,  
  `ChildCommentCount` MEDIUMINT(8) NOT NULL DEFAULT 0,
  `CommentEnabled` TINYINT(1) NOT NULL DEFAULT '1' ,  
  `DigIt` MEDIUMINT(8) unsigned NOT NULL DEFAULT 0,
  `CoolIt` MEDIUMINT(8) unsigned NOT NULL DEFAULT 0,
  `ImageName` VARCHAR(255) NULL DEFAULT '',  
  `IsApproved` TINYINT(1) NOT NULL DEFAULT '1',
  `IsSpam` TINYINT(1)  NOT NULL DEFAULT '0',
  `IsDeleted` TINYINT(1) NOT NULL DEFAULT '0',  
  `Parms` LONGTEXT  NULL  ,
  `PostContentTypeId`  TinyInt(3)    NOT NULL ,
  `UpdatedAt` DATETIME(6) NOT NULL,  
  `CreatedAt` DATETIME(6) NOT NULL,  
 PRIMARY KEY (`PostId`) ,
   INDEX `ik_post_userIdx` (`UserId` ASC),
   INDEX `ik_post_countryidx` (`CountryId` ASC),
   INDEX `ik_post_partyidx` (`PartyId` ASC),
   INDEX `ik_post_createdatx` (`CreatedAt` ASC),
   INDEX `ik_post_postidx` (`PostId` ASC)
   )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

-- -----------------------------------------------------
-- Table `PostComment`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PostComment` ;
CREATE TABLE `PostComment` (
  `PostCommentId` CHAR(36) DEFAULT NULL,
  `UserId` INT   NOT NULL,
  `PostId` CHAR(36) NOT NULL,
  `ParentCommentId` CHAR(36) DEFAULT NULL ,    
  `DigIt` MEDIUMINT(8) unsigned NOT NULL DEFAULT 0,
  `CoolIt` MEDIUMINT(8) unsigned NOT NULL DEFAULT 0,
  `ChildCommentCount` MEDIUMINT NULL ,
  `CommentText` VARCHAR(500) NOT NULL,
  `IsApproved` TINYINT(1) NOT NULL DEFAULT '1',
  `IsSpam` TINYINT(1)  NOT NULL DEFAULT '0',
  `IsDeleted` TINYINT(1) NOT NULL DEFAULT '0',
  `CreatedAt` DATETIME(6) NULL DEFAULT NULL ,  
  PRIMARY KEY (`PostCommentId`),
    INDEX `ik_post_postcommentIdx` (`PostId` ASC),
	INDEX `ik_post_parentcommentIdx` (`ParentCommentId` ASC))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
 -- -----------------------------------------------------
-- Table `UserDig`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserDig` ;

CREATE  TABLE IF NOT EXISTS `UserDig` (
  `PostCommentId` CHAR(36) NOT NULL ,      
  `UserId` INT ,    
  `DigType` TINYINT(2) ,
  PRIMARY KEY (`PostCommentId`, `UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
 -- -----------------------------------------------------
-- Table `PostWebContent`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PostWebContent` ;
CREATE TABLE `PostWebContent` (
  `PostWebContentId` INT(10)  NOT NULL AUTO_INCREMENT,  
  `UserId` INT   NOT NULL,
  `PostId` CHAR(36) DEFAULT NULL,
  `Content` TEXT NOT NULL,  
  `Title` VARCHAR(255) DEFAULT NULL,
  `Uri` VARCHAR(255) DEFAULT NULL,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`PostWebContentId`),
    INDEX `ik_post_postwebcontentIdx` (`PostId` ASC))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- -----------------------------------------------------
-- Table `PostTag`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PostTag` ;
CREATE TABLE `PostTag` (
  `PostTagId` INT(10)  NOT NULL AUTO_INCREMENT,  
  `PostId` CHAR(36) DEFAULT NULL,
  `TopicTagId` INT(10)  NOT NULL,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`PostTagId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- -----------------------------------------------------
-- Table `TopicTag`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `TopicTag` ;
CREATE TABLE `TopicTag` (
  `TopicTagId` INT(10)  NOT NULL AUTO_INCREMENT,    
  `Tag` VARCHAR(50) DEFAULT NULL,
  `TagCount` INT(5) DEFAULT 0,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`TopicTagId`, `Tag`),
 INDEX `TopicTag_Tag_Idx` (`Tag` ASC) 
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;


-- -----------------------------------------------------
-- Table `PostUserACL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PostUserACL` ;
CREATE TABLE `PostUserACL` (
  `PostId` BIGINT  NOT NULL ,
  `UserId` INT   NOT NULL,  
  `AccessType` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`PostId`, `UserId`),
   INDEX `ik_postUserACLx` (`AccessType` ASC) )
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 


-- -----------------------------------------------------
-- Table `UserBankAccount`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserBankAccount` ;

CREATE  TABLE IF NOT EXISTS `UserBankAccount` (
  `UserId` INT  NOT NULL  ,
  `Cash` DECIMAL(50,2)  NOT NULL ,  
  `Gold` DECIMAL(50,2) NOT NULL ,
  `Silver` DECIMAL(50,2) NOT NULL ,
  `CreatedAt` DATETIME NOT NULL  ,
  `UpdatedAt` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `CapitalType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CapitalType` ;

CREATE  TABLE IF NOT EXISTS `CapitalType` (
  `CapitalTypeId` SMALLINT(4)  NOT NULL ,
  `Name` VARCHAR(45) NOT NULL  ,
  `Description` VARCHAR(255) NOT NULL  ,
  `ImageFont` VARCHAR(50)NOT NULL ,
  `Cost` DECIMAL(50,2) NOT NULL DEFAULT '0.00' ,  
  PRIMARY KEY (`CapitalTypeId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `CapitalTransactionLog`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CapitalTransactionLog` ;

CREATE  TABLE IF NOT EXISTS `CapitalTransactionLog` (
  `LogId` CHAR(36) NOT NULL ,   
  `SourceId` INT  NULL ,   
  `SourceGuid` CHAR(36)  NULL , 
  `RecipentId` INT   NULL , 
  `RecipentGuid` CHAR(36)  NULL , 
  `Amount` DECIMAL(50,2) NOT NULL,     -- Total Amount with tax 
  `TaxAmount` DECIMAL(50,2)  NULL,      
  `FundType` TINYINT(3) NOT NULL ,   
  `CreatedAT` DATETIME(6) NOT NULL ,
  PRIMARY KEY (`LogId` ) ,
  INDEX `ik_capitaltransactionlog_sourceidx` (`SourceId` ASC) ,
  INDEX `ik_capitaltransactionlog_recipentidx` (`RecipentId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `FundTypeCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `FundTypeCode` ;

CREATE  TABLE IF NOT EXISTS `FundTypeCode` (
  `FundType` TINYINT(3) NOT NULL,
  `Code` VARCHAR(45) NULL DEFAULT NULL ,
  `ImageFont` VARCHAR(50) NOT NULL  ,
  PRIMARY KEY (`FundType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `CountryCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryCode` ;

CREATE  TABLE IF NOT EXISTS `CountryCode` (
  `CountryUserId` INT NOT NULL ,
  `CountryId` CHAR(2) NOT NULL ,
  `Code` VARCHAR(100) NOT NULL ,
  PRIMARY KEY (`CountryId`) )
ENGINE = InnoDB
AUTO_INCREMENT = 203
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `ProvinceCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ProvinceCode` ;

CREATE  TABLE IF NOT EXISTS `ProvinceCode` (
  `ProvinceId` SMALLINT(5)   NOT NULL AUTO_INCREMENT ,
  `Province` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`ProvinceId`) )
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `Friend`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Friend` ;

CREATE  TABLE IF NOT EXISTS `Friend` (  
  `FollowerUserId` INT   NOT NULL ,
  `FollowingUserId` INT  NOT NULL,   
  `CreatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`FollowerUserId`, FollowingUserId ) )
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `SuggestFriend`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SuggestFriend` ;

CREATE  TABLE IF NOT EXISTS `SuggestFriend` (  
  `UserId` INT   NOT NULL ,
  `SuggestionUserId` INT  NOT NULL,   
  `MatchScore` TINYINT(3)  NOT NULL,   
  `UserIgnore` TINYINT(1)  NOT NULL,  
  PRIMARY KEY (`UserId`, SuggestionUserId ) )
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `UserVoteSelectedChoice`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserVoteSelectedChoice` ;

CREATE  TABLE IF NOT EXISTS `UserVoteSelectedChoice` (
  `TaskId` CHAR(36) NOT NULL  ,
  `ChoiceId` SMALLINT   NOT NULL  ,  
  `UserId` INT  NOT NULL ,
  `Score` TINYINT(2)  NOT NULL ,
  PRIMARY KEY ( `TaskId` , `ChoiceId`, `UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `UserVoteChoice`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserVoteChoice` ;

CREATE  TABLE IF NOT EXISTS `UserVoteChoice` (
  `ChoiceId` SMALLINT   NOT NULL  ,
  `TaskTypeId`  SMALLINT(5)   NOT NULL ,  
  `ChoiceText` VARCHAR(1000)  NOT NULL ,
  `ChoiceLogo` VARCHAR(50)  NULL ,
  PRIMARY KEY (`TaskTypeId`, `ChoiceId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
-- -----------------------------------------------------
-- Table `UserTask`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserTask` ;

CREATE  TABLE IF NOT EXISTS `UserTask` (
  `TaskId` CHAR(36)  NOT NULL  ,
  `UserId` INT  NOT NULL ,
  `AssignerUserId` INT  NOT NULL ,  
  `CompletionPercent` TINYINT(2)   NOT NULL DEFAULT 0 ,
  `Flagged` TINYINT(1)  NOT NULL DEFAULT 0 ,
  `Status` CHAR(1)  NOT NULL DEFAULT 'I' , -- I incomplete, C - Complete, P- InpRogress, A- auto Complete
  `Parms` VARCHAR(1000)  NULL  ,
  `TaskTypeId`  SMALLINT(5)    NOT NULL ,
  `DueDate` DATETIME NOT NULL ,  
  `DefaultResponse` SMALLINT   NOT NULL  ,    
  `Priority` TINYINT(2)   NOT NULL DEFAULT 0 ,
  `CreatedAt` DATETIME(6) NOT NULL ,
  PRIMARY KEY (`TaskId`,`UserId` ),  
INDEX `ik_usertask_assigneruseridx` (`AssignerUserId` ASC),
INDEX `ik_usertask_statusx` (`Status` ASC)   )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `TaskType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `TaskType` ;

CREATE  TABLE IF NOT EXISTS `TaskType` (
  `TaskTypeId` SMALLINT(5)  NOT NULL AUTO_INCREMENT ,
  `ShortDescription` VARCHAR(200) NOT NULL DEFAULT ' ' ,    
  `Description` VARCHAR(2000) NOT NULL DEFAULT ' ' ,    
  `Picture` VARCHAR(255)  NOT NULL DEFAULT '/web/image/default.jpg' ,
  `ChoiceType` TINYINT(3) NOT NULL DEFAULT '0' ,  
  `MaxChoiceCount` TINYINT(3) NOT NULL DEFAULT '0' ,   
  PRIMARY KEY (`TaskTypeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `TaskReminder`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `TaskReminder` ;

CREATE  TABLE IF NOT EXISTS `TaskReminder` (
  `TaskId` CHAR(36)  NOT NULL,
  `ReminderFrequency` SMALLINT  NOT NULL ,
  `ReminderTransPort` VARCHAR(2)  NULL ,-- M- Message, P-Post, MP- both 
  `StartDate` DATETIME NOT NULL ,
  `EndDate` DATETIME NOT NULL ,
  PRIMARY KEY (`TaskId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `UserNotification`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserNotification` ;

CREATE  TABLE IF NOT EXISTS `UserNotification` (
  `NotificationId` CHAR(36)  NOT NULL  ,
  `UserId` INT  NOT NULL  ,  
  `NotificationTypeId` SMALLINT  NOT NULL  ,  
  `Priority` TINYINT(2)   NOT NULL DEFAULT 0 ,  
  `HasTask` TINYINT(1) NOT NULL DEFAULT '0' , -- this joins with UserTask by NotifcationId= taskId
  `Parms` VARCHAR(1000)  NULL  ,
  `EmailSent` TINYINT(1) DEFAULT 0   ,
  `UpdatedAt` DATETIME(6) NOT NULL  ,  
  PRIMARY KEY (`NotificationId`, `UserId`,`NotificationTypeId`),
  INDEX `ik_usernotification_updatedat` (`UpdatedAt` ASC)
 )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `NotificationType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `NotificationType` ;

CREATE  TABLE IF NOT EXISTS `NotificationType` (
  `NotificationTypeId` SMALLINT NOT NULL AUTO_INCREMENT,    
  `ShortDescription` VARCHAR(200) NOT NULL DEFAULT ' ' ,    
  `EmailNotification` TINYINT(1)  NOT NULL ,
  `Description` VARCHAR(1000)  NOT NULL ,
  `ImageFont` VARCHAR(50) NOT NULL  ,
  PRIMARY KEY (`NotificationTypeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `AdsType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `AdsType` ;

CREATE  TABLE IF NOT EXISTS `AdsType` (
   `AdsTypeId` TINYINT(3)  NOT NULL AUTO_INCREMENT,
   `AdName` VARCHAR(250)  NOT NULL,   
   `BaseCost` DECIMAL(8,2) NOT NULL ,   
   `PricePerChar` DECIMAL(8,2) NOT NULL, 
   `PricePerImageByte` DECIMAL(10,6) NOT NULL, 
   `FontCss` VARCHAR(50) NOT NULL ,    
  PRIMARY KEY (`AdsTypeId` ) )
ENGINE = InnoDB

DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `AdsFrequencyType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `AdsFrequencyType` ;

CREATE  TABLE IF NOT EXISTS `AdsFrequencyType` (
   `AdsFrequencyTypeId` TINYINT(3)  NOT NULL AUTO_INCREMENT,
   `FrequencyName` VARCHAR(250)  NOT NULL,      
   `FrequencyMultiple` TINYINT(3)  NOT NULL,     -- Used to multiple base price from adsType
  PRIMARY KEY (`AdsFrequencyTypeId` ) )
ENGINE = InnoDB

DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `ElectionPosition`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ElectionPosition` ;

CREATE  TABLE IF NOT EXISTS `ElectionPosition` (
  `PositionTypeId` TINYINT(3)  NOT NULL AUTO_INCREMENT ,
  `ElectionPositionName` VARCHAR(25) NOT NULL ,  
  PRIMARY KEY (`PositionTypeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `PartyMember`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PartyMember` ;

CREATE  TABLE IF NOT EXISTS `PartyMember` (
  `PartyId` CHAR(36)  NOT NULL , 
  `UserId` INT   NOT NULL ,   
  `MemberType` CHAR(1),  -- C CoFounder, F- Founder, M- Member
  `MemberStatus` CHAR(1),  -- E -- Pending Ejection,    Q Quit, F- Fired, N -- Pending Nomination, C -- PartyClosed
  `MemberStartDate` DATETIME NULL DEFAULT NULL ,
  `MemberEndDate` DATETIME NULL DEFAULT NULL ,  
  `DonationAmount`DECIMAL(50,2) DEFAULT 0,   
  PRIMARY KEY (`UserId`, `PartyId`,`MemberType`,`MemberStartDate` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `MerchandiseType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `MerchandiseType` ;

CREATE  TABLE IF NOT EXISTS `MerchandiseType` (
  `MerchandiseTypeId` SMALLINT(4)  NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL  ,
  `Description` VARCHAR(255) NOT NULL  ,
  `ImageFont` VARCHAR(50)NOT NULL ,
  `Cost` DECIMAL(50,2) NOT NULL DEFAULT '0.00' ,
  `ResaleRate` DECIMAL(5,2) NOT NULL ,
  `RentalPrice` DECIMAL(25,2) NOT NULL ,
  `MerchandiseTypeCode` TINYINT(2)   NOT NULL  ,  
  PRIMARY KEY (`MerchandiseTypeId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `Stock`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Stock` ;

CREATE  TABLE IF NOT EXISTS `Stock` (
  `StockId` SMALLINT(5)   NOT NULL ,  
  `PreviousDayValue` DECIMAL(50,2) NOT NULL ,
  `CurrentValue` DECIMAL(50,2) NOT NULL ,
  `DayChange` DECIMAL(10,2) NOT NULL ,
  `DayChangePercent` DECIMAL(10,2) NOT NULL ,
  `StockName` VARCHAR(25) NOT NULL ,  
  `ImageFont` VARCHAR(50)NOT NULL ,
  `Ticker` VARCHAR(5) NOT NULL ,
  `Description` VARCHAR(255) NOT NULL ,  
  `UpdatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`StockId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `ContactProvider`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ContactProvider` ;

CREATE  TABLE IF NOT EXISTS `ContactProvider` (
  `ProviderId` TINYINT(2)     NOT NULL ,   
  `ProviderName` VARCHAR(20) NOT NULL ,    
  `ImageFont` VARCHAR(50)NOT NULL ,
  PRIMARY KEY (`ProviderId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `ContactSource`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ContactSource` ;

CREATE  TABLE IF NOT EXISTS `ContactSource` (
  `ProviderId` TINYINT(2)  NOT NULL ,   
  `UserId` INT   NOT NULL ,     
  `Total`  SMALLINT(5),
  `UpdatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`ProviderId`, `UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `EmailSubscription`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `EmailSubscription` ;

CREATE  TABLE IF NOT EXISTS `EmailSubscription` (  
  `UserId` INT  NOT NULL  ,  
  `AllowEventNotification` TINYINT(1)  NOT NULL  ,  
  `AllowPromotions` TINYINT(1)  NOT NULL  ,      
  `UpdatedAt` DATETIME(6) NOT NULL  ,  
  PRIMARY KEY (`UserId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `CountryLeader`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryLeader` ;

CREATE  TABLE IF NOT EXISTS `CountryLeader` (
   `CountryId` CHAR(2) NOT NULL ,
   `UserId` INT   NOT NULL ,
   `CandidateTypeId` CHAR(1)  NOT NULL , -- P Party , I Independent
   `PositionTypeId` TINYINT(3)  NOT NULL ,
   `StartDate` DATETIME NOT NULL ,
   `EndDate` DATETIME NOT NULL ,
    PRIMARY KEY (`UserId`, StartDate, EndDate),
	INDEX `ik_countryleaders_countryid` (`CountryId` ASC),
	INDEX `ik_countryleaders_positiontypeid` (`PositionTypeId` ASC)
  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;