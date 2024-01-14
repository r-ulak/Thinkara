SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALId_DATES';
DROP SCHEMA IF EXISTS `PlanetX`; 
CREATE SCHEMA IF NOT EXISTS `PlanetX` ;
USE `PlanetX` ;

-- -----------------------------------------------------
-- Table `PlanetX`.`Merchandise`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Merchandise` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Merchandise` (
  `ItemId` MEDIUMINT(8) NOT NULL ,
  `ItemType` SMALLINT(5)  NULL DEFAULT NULL ,
  `Cost` DECIMAL(10,2) NOT NULL DEFAULT '0.00' ,
  `Picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`ItemId`) ,
  INDEX `fk_merchandise_item_code_Idx` (`ItemType` ASC) ,
  INDEX `fk_asset_merchandise` (`ItemId` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`WebUser`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`WebUser` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`WebUser` (
  `UserId` MEDIUMINT(8)  NOT NULL AUTO_INCREMENT ,
  `Password` VARCHAR(45)  NOT NULL ,
  `NameFirst` VARCHAR(45)  NOT NULL ,
  `NameMIddle` VARCHAR(45)  NULL DEFAULT NULL ,
  `NameLast` VARCHAR(45)  NOT NULL ,
  `EmailId` VARCHAR(100)  NOT NULL ,
  `Picture` VARCHAR(255)  NOT NULL DEFAULT '/web/image/default.jpg' ,
  `Active` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `OnlineStatus` TINYINT(2) NULL DEFAULT '0' ,  
  `CreatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `PlanetX`.`AUDWebUser`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`WebUserUpdate` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`WebUserUpdate` (
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `NameFirst` VARCHAR(45)   NULL DEFAULT NULL,
  `NameMIddle` VARCHAR(45)  NULL DEFAULT NULL ,
  `NameLast` VARCHAR(45)  NULL DEFAULT NULL,
  `EmailId` VARCHAR(100)  NULL DEFAULT NULL,
  `OldNameFirst` VARCHAR(45)   NULL DEFAULT NULL,
  `OldNameMIddle` VARCHAR(45)  NULL DEFAULT NULL ,
  `OldNameLast` VARCHAR(45)  NULL DEFAULT NULL,
  `OldEmailId` VARCHAR(100)  NULL DEFAULT NULL,  
  `Picture` VARCHAR(255)  NOT NULL DEFAULT '/web/image/default.jpg' ,
  `ActionType` CHAR(1)  NULL DEFAULT NULL,  
  `CreatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `PlanetX`.`Asset`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Asset` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Asset` (
  `ItemId` MEDIUMINT(8) NOT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `Qty` SMALLINT(5) NULL DEFAULT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`ItemId`) ,
  INDEX `fk_asset_UserIdx` (`UserId` ASC) ,
  CONSTRAINT `fk_asset_merchandise`
    FOREIGN KEY (`ItemId` )
    REFERENCES `PlanetX`.`Merchandise` (`ItemId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Avatar`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Avatar` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Avatar` (
  `AvatarId` MEDIUMINT(8) NOT NULL ,
  `Picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`AvatarId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Blog`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Blog` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Blog` (
  `BlogId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Message` VARCHAR(45) NOT NULL ,
  `Author` VARCHAR(45) NULL DEFAULT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `CreatedAt` VARCHAR(45) NOT NULL ,
  PRIMARY KEY (`BlogId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;



-- -----------------------------------------------------
-- Table `PlanetX`.`Post`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Post` ;
CREATE  TABLE IF NOT EXISTS `PlanetX`.`Post` (
  `UserId` MEDIUMINT(8)  NOT NULL,
  `PostId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `PostContent` LONGTEXT NOT NULL,
  `CommentEnabled` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `Raters` MEDIUMINT(8)  NOT NULL DEFAULT '0',
  `Rating` FLOAT NOT NULL DEFAULT '0',
  `Slug` VARCHAR(255) NOT NULL DEFAULT '',  
  `UserACL` TINYINT(3) NOT NULL DEFAULT '0' ,  
  `ClubACL` TINYINT(3) NOT NULL DEFAULT '0' ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
 PRIMARY KEY (`PostId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

-- -----------------------------------------------------
-- Table `PlanetX`.`PostComment`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`PostComment` ;
CREATE TABLE `PlanetX`.`PostComment` (
  `PostCommentId` INT(10)  NOT NULL AUTO_INCREMENT,  
  `UserId` MEDIUMINT(8)  NOT NULL,
  `PostId` MEDIUMINT(8) NOT NULL,
  `ParentCommentId` INT(10) NOT NULL ,
  `CommentDate` DATETIME NOT NULL ,
  `Comment` TEXT NOT NULL,
  `IsApproved` TINYINT(1) NOT NULL DEFAULT '0',
  `Picture` VARCHAR(255) DEFAULT NULL,
  `IsSpam` TINYINT(1)  NOT NULL DEFAULT '0',
  `IsDeleted` TINYINT(1) NOT NULL DEFAULT '0',
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`PostCommentId`),
    INDEX `fk_post_postcommentIdx` (`PostId` ASC) ,
  CONSTRAINT `fk_post_postcomment`
    FOREIGN KEY (`PostId` )
    REFERENCES `PlanetX`.`Post` (`PostId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ENGINE=InnoDB DEFAULT CHARSET=latin1;

 -- -----------------------------------------------------
-- Table `PlanetX`.`PostWebContent`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`PostWebContent` ;
CREATE TABLE `PlanetX`.`PostWebContent` (
  `PostWebContentId` INT(10)  NOT NULL AUTO_INCREMENT,  
  `UserId` MEDIUMINT(8)  NOT NULL,
  `PostId` MEDIUMINT(8) NOT NULL,    
  `Content` TEXT NOT NULL,  
  `Title` VARCHAR(255) DEFAULT NULL,
  `Uri` VARCHAR(255) DEFAULT NULL,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`PostWebContentId`),
    INDEX `fk_post_postwebcontentIdx` (`PostId` ASC) ,
  CONSTRAINT `fk_post_postwebcontent`
    FOREIGN KEY (`PostId` )
    REFERENCES `PlanetX`.`Post` (`PostId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- -----------------------------------------------------
-- Table `PlanetX`.`PostTag`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`PostTag` ;
CREATE TABLE `PlanetX`.`PostTag` (
  `PostTagId` INT(10)  NOT NULL AUTO_INCREMENT,  
  `PostId` VARCHAR(36) NOT NULL DEFAULT '',
  `TopicTagId` INT(10)  NOT NULL,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`PostTagId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;

-- -----------------------------------------------------
-- Table `PlanetX`.`TopicTag`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`TopicTag` ;
CREATE TABLE `PlanetX`.`TopicTag` (
  `TopicTagId` INT(10)  NOT NULL AUTO_INCREMENT,    
  `Tag` VARCHAR(50) DEFAULT NULL,
  `TagCount` INT(5) DEFAULT 0,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`TopicTagId`, `Tag`),
 INDEX `TopicTag_Tag_Idx` (`Tag` ASC) 
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`PostUserACL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`PostUserACL` ;
CREATE TABLE `PlanetX`.`PostUserACL` (
  `PostId` MEDIUMINT(8) NOT NULL ,
  `UserId` MEDIUMINT(8)  NOT NULL,  
  `AccessType` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`PostId`, `UserId`))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
-- -----------------------------------------------------
-- Table `PlanetX`.`Bookmark`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Bookmark` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Bookmark` (
  `BookmarkId` BIGINT(20) NOT NULL AUTO_INCREMENT ,
  `Url` VARCHAR(255) NULL DEFAULT NULL ,
  `Rating` SMALLINT(5) NULL DEFAULT NULL ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '2' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `BookmarkCategoryId` SMALLINT(5) NOT NULL ,
  `BookmarkSubCategoryId` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`BookmarkId`) ,
  INDEX `fk_bookmark_bookmark_category_Idx` (`BookmarkCategoryId` ASC) ,
  INDEX `fk_bookmark_bookmark_sub_category_Idx` (`BookmarkSubCategoryId` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`BookmarkCategory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`BookmarkCategory` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`BookmarkCategory` (
  `BookmarkCategoryId` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`BookmarkCategoryId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`BookmarkInfo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`BookmarkInfo` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`BookmarkInfo` (
  `BookmarkInfoId` BIGINT(20) NOT NULL AUTO_INCREMENT ,
  `BookmarkId` BIGINT(20) NULL DEFAULT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `Clicks` SMALLINT(5) NULL DEFAULT NULL ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '2' ,
  PRIMARY KEY (`BookmarkInfoId`) ,
  INDEX `fk_bookmark_info_bookmark_Idx` (`BookmarkId` ASC) ,
  INDEX `fk_bookmark_info_UserIdx` (`UserId` ASC) ,
  CONSTRAINT `fk_bookmark_info_bookmark`
    FOREIGN KEY (`BookmarkId` )
    REFERENCES `PlanetX`.`Bookmark` (`BookmarkId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`BookmarkSubCategory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`BookmarkSubCategory` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`BookmarkSubCategory` (
  `BookmarkSubCategoryId` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  `BookmarkCategoryId` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`BookmarkSubCategoryId`) ,
  INDEX `fk_bookmark_sub_category_bookmark_category_Idx` (`BookmarkCategoryId` ASC) ,
  CONSTRAINT `fk_bookmark_sub_category_bookmark_category`
    FOREIGN KEY (`BookmarkCategoryId` )
    REFERENCES `PlanetX`.`BookmarkCategory` (`BookmarkCategoryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

use PlanetX;
-- -----------------------------------------------------
-- Table `PlanetX`.`Stocks`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Stocks` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Stocks` (
  `StockId` SMALLINT(5)  NOT NULL ,  
  `CurrentValue` DECIMAL(10,2) NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`StockId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;



-- -----------------------------------------------------
-- Table `PlanetX`.`StockCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`StockCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`StockCode` (
  `StockId` SMALLINT(5) NOT NULL AUTO_INCREMENT ,      
  `StockName` VARCHAR(25) NOT NULL ,
  `BusinessId` MEDIUMINT(8) NOT NULL,
  `OwnerId` MEDIUMINT(8) NOT NULL,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`StockId`) ,
  INDEX `Index_StockCode_BusinessId` (`BusinessId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;



-- -----------------------------------------------------
-- Table `PlanetX`.`StocksTransaction`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`StocksTransaction` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`StocksTransaction` (
  `StocksTransactionId` INTEGER NOT NULL AUTO_INCREMENT ,
  `OwnerId` MEDIUMINT(8) NOT NULL ,
  `TransactionType` TINYINT(3) NOT NULL ,  
  `StockId` SMALLINT(5)  NOT NULL ,  
  `NumberOfUnit` DECIMAL(10,2) NOT NULL ,
  `TotalPrice` DECIMAL(10,2) NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`StocksTransactionId`) ,
  INDEX `fk_StocksTransaction_StockIdx` (`StockId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`UserBankAccount`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`UserBankAccount` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`UserBankAccount` (
  `UserId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Cash` DECIMAL(50,2)  NOT NULL ,  
  `Gold` DECIMAL(10,2) NOT NULL ,
  `Silver` DECIMAL(10,2) NOT NULL ,
  `Stocks` DECIMAL(50,2) NOT NULL ,
  `Loan` DECIMAL(20,2) NOT NULL ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`UserId`) ,
  INDEX `Index_UserBankAccount_UserId` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`UserStocks`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`UserStocks` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`UserStocks` (
  `UserId` MEDIUMINT(8) NOT NULL,
  `StockId` SMALLINT(5)  NOT NULL ,    
  `PurchasedUnit` SMALLINT(5)  NOT NULL ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`UserId`, `StockId` ) ,
  INDEX `Index_UserStocks_StockId` (`StockId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Business`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Business` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Business` (
  `BusinessId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NOT NULL,
  `BusinessName` VARCHAR(25) NOT NULL ,
  `BusinessTypeId` SMALLINT(5)  NOT NULL ,
  `BusinessSubtypeId` SMALLINT(5)  NOT NULL ,
  `CityId` MEDIUMINT(8)  NOT NULL ,
  `NetProfit` DECIMAL(10,2) NOT NULL ,
  `RunningCost` DECIMAL(10,2) NOT NULL ,
  `Active` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`BusinessId`) ,
  INDEX `fk_business_businesstype_Idx` (`BusinessTypeId` ASC),
INDEX `fk_business_webuser_Idx` (`UserId` ASC)  )
ENGINE = InnoDB
AUTO_INCREMENT = 1
DEFAULT CHARACTER SET = latin1;



-- -----------------------------------------------------
-- Table `PlanetX`.`BusinessCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`BusinessCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`BusinessCode` (
  `BusinessTypeId` SMALLINT(5)  NOT NULL AUTO_INCREMENT ,
  `Code` VARCHAR(60) NOT NULL ,  
  `Picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `Description` VARCHAR(255) NOT NULL ,
  PRIMARY KEY (`BusinessTypeId`)
  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`BusinessSubCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`BusinessSubCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`BusinessSubCode` (
  `BusinessSubtypeId` SMALLINT(5)  NOT NULL AUTO_INCREMENT ,
  `Code` VARCHAR(60) NOT NULL ,
  `Picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `Description` VARCHAR(255) NOT NULL ,
  `BusinessTypeId` SMALLINT(5)  NOT NULL ,
  PRIMARY KEY (`BusinessSubtypeId`),
  INDEX `fk_Businesstype_BusinessSubCode` (`BusinessTypeId` ASC) ,
  CONSTRAINT `fk_BusinessSubCode_BusinessCode`
    FOREIGN KEY (`BusinessTypeId` )
    REFERENCES `PlanetX`.`BusinessCode` (`BusinessTypeId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;




-- -----------------------------------------------------
-- Table `PlanetX`.`CityCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`CityCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`CityCode` (
  `CityId` MEDIUMINT(8)  NOT NULL AUTO_INCREMENT ,
  `City` VARCHAR(250) NOT NULL ,
  `CountryId` CHAR(2) NOT NULL ,
  PRIMARY KEY (`CityId`),
INDEX `fk_CityCode_CountryCode_Idx` (`CountryId` ASC)
  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`CountryCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`CountryCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`CountryCode` (
  `CountryId` CHAR(2) NOT NULL ,
  `Code` VARCHAR(100) NOT NULL ,
  PRIMARY KEY (`CountryId`) )
ENGINE = InnoDB
AUTO_INCREMENT = 203
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`ProvinceCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`ProvinceCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`ProvinceCode` (
  `ProvinceId` SMALLINT(5)  NOT NULL AUTO_INCREMENT ,
  `Province` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`ProvinceId`) )
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`BusinessLocation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`BusinessLocation` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`BusinessLocation` (
  `BusinessId` MEDIUMINT(8) NOT NULL ,
  `CityId` MEDIUMINT(8)  NOT NULL ,
  `ProvinceId` SMALLINT(5)  NOT NULL ,
  `CountryId` CHAR(2) NOT NULL ,
  PRIMARY KEY (`BusinessId`) ,
  INDEX `fk_location_city_code_Idx` (`CityId` ASC) ,
  INDEX `fk_location_province_code_Idx` (`ProvinceId` ASC) ,
  INDEX `fk_location_country_code_Idx` (`CountryId` ASC) ,
  CONSTRAINT `fk_businesslocation_business`
    FOREIGN KEY (`BusinessId` )
    REFERENCES `PlanetX`.`Business` (`BusinessId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_businesslocation_citycode`
    FOREIGN KEY (`CityId` )
    REFERENCES `PlanetX`.`CityCode` (`CityId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_businesslocation_countrycode`
    FOREIGN KEY (`CountryId` )
    REFERENCES `PlanetX`.`CountryCode` (`CountryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_businesslocation_provincecode`
    FOREIGN KEY (`ProvinceId` )
    REFERENCES `PlanetX`.`ProvinceCode` (`ProvinceId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Card`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Card` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Card` (
  `CardId` MEDIUMINT(3) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `CardType` TINYINT(3) NULL DEFAULT NULL ,
  `Amount` DECIMAL(10,2) NULL DEFAULT NULL ,
  `ExpireDate` DATETIME NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`CardId`) ,
  INDEX `fk_event_user` (`UserId` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Chat`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Chat` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Chat` (
  `ChatId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `To` MEDIUMINT(8) NOT NULL ,
  `Msg` VARCHAR(100) NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  PRIMARY KEY (`ChatId`) ,
  INDEX `fk_chat_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Status`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Status` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Status` (
  `StatusId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Message` VARCHAR(255) NOT NULL ,
  `CreatedAt` DATETIME NOT NULL ,
  `ThumbsUp` SMALLINT(5) NULL DEFAULT NULL ,
  `ThumbsDown` SMALLINT(5) NULL DEFAULT NULL ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '0' ,
  `IsReply` TINYINT(1) NOT NULL DEFAULT '0' ,
  `ToFb` TINYINT(1) NOT NULL DEFAULT '0' ,
  `ToTwitter` TINYINT(1) NOT NULL DEFAULT '0' ,
  `UserId` MEDIUMINT(8)  NOT NULL ,
  PRIMARY KEY (`StatusId`) ,
  INDEX `fk_status_reply_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`DegreeCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`DegreeCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`DegreeCode` (
  `DegreeType` TINYINT(3)  NOT NULL AUTO_INCREMENT ,
  `Degree` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`DegreeType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Education`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Education` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Education` (
  `EducationId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `DegreeType` TINYINT(3)  NOT NULL DEFAULT '0' ,
  `MajorType` SMALLINT(5)  NOT NULL DEFAULT '0' ,
  `UniversityId` SMALLINT(5)  NULL DEFAULT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`EducationId`) ,
  INDEX `fk_education_UserIdx` (`UserId` ASC) ,
  INDEX `fk_education_degree_code_Idx` (`DegreeType` ASC) ,
  INDEX `fk_education_major_code_Idx` (`MajorType` ASC) ,
  INDEX `fk_education_university_code_Idx` (`UniversityId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Employment`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Employment` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Employment` (
  `BusinessId` MEDIUMINT(8) NOT NULL ,
  `UserId` MEDIUMINT(8)  NOT NULL DEFAULT '0' ,
  `Salary` DECIMAL(10,2) NOT NULL ,
  `JobTitle` VARCHAR(25) NOT NULL DEFAULT 'employee' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`BusinessId`, `UserId`) ,
  INDEX `fk_employment_UserIdx` (`UserId` ASC) ,
  INDEX `fk_employment_business_Idx` (`BusinessId` ASC) ,
  CONSTRAINT `fk_employment_business`
    FOREIGN KEY (`BusinessId` )
    REFERENCES `PlanetX`.`Business` (`BusinessId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Event`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Event` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Event` (
  `EventId` MEDIUMINT(3) NOT NULL AUTO_INCREMENT ,
  `EventName` VARCHAR(45) NULL DEFAULT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `Description` TEXT NULL DEFAULT NULL ,
  `EventType` SMALLINT(5)  NOT NULL ,
  `Picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `EndTime` DATETIME NULL DEFAULT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`EventId`) ,
  INDEX `fk_event_UserIdx` (`UserId` ASC) ,
  INDEX `fk_event_event_code_Idx` (`EventType` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`EventCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`EventCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`EventCode` (
  `EventType` SMALLINT(5)  NOT NULL ,
  `Code` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`EventType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`EventLocation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`EventLocation` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`EventLocation` (
  `EventId` MEDIUMINT(8) NOT NULL ,
  `CityId` MEDIUMINT(8)  NOT NULL ,
  `ProvinceId` SMALLINT(5)  NOT NULL ,
  `CountryId` CHAR(2) NOT NULL ,
  PRIMARY KEY (`EventId`) ,
  INDEX `fk_location_city_code_Idx` (`CityId` ASC) ,
  INDEX `fk_location_province_code_Idx` (`ProvinceId` ASC) ,
  INDEX `fk_location_country_code_Idx` (`CountryId` ASC) ,
  CONSTRAINT `fk_eventlocation_citycode`
    FOREIGN KEY (`CityId` )
    REFERENCES `PlanetX`.`CityCode` (`CityId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_eventlocation_countrycode`
    FOREIGN KEY (`CountryId` )
    REFERENCES `PlanetX`.`CountryCode` (`CountryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_eventlocation_event`
    FOREIGN KEY (`EventId` )
    REFERENCES `PlanetX`.`Event` (`EventId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_eventlocation_provincecode`
    FOREIGN KEY (`ProvinceId` )
    REFERENCES `PlanetX`.`ProvinceCode` (`ProvinceId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`RsvpStatusCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`RsvpStatusCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`RsvpStatusCode` (
  `StatusType` TINYINT(2)  NOT NULL AUTO_INCREMENT ,
  `Code` VARCHAR(15) NOT NULL ,
  PRIMARY KEY (`StatusType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`EventMembership`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`EventMembership` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`EventMembership` (
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `EventId` MEDIUMINT(3) NOT NULL ,
  `StatusType` TINYINT(2)  NULL DEFAULT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`UserId`, `EventId`) ,
  INDEX `fk_event_membership_UserIdx` (`UserId` ASC) ,
  INDEX `fk_event_membership_event_Idx` (`EventId` ASC) ,
  INDEX `fk_event_membership_rsvp_status_code_Idx` (`StatusType` ASC) ,
  CONSTRAINT `fk_event_membership_event`
    FOREIGN KEY (`EventId` )
    REFERENCES `PlanetX`.`Event` (`EventId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_event_membership_rsvp_status_code`
    FOREIGN KEY (`StatusType` )
    REFERENCES `PlanetX`.`RsvpStatusCode` (`StatusType` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Feed`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Feed` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Feed` (
  `FeedId` BIGINT(20) NOT NULL AUTO_INCREMENT ,
  `FeedUrl` VARCHAR(255) NULL DEFAULT NULL ,
  `Rating` SMALLINT(5) NULL DEFAULT NULL ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '2' ,
  `CreatedAt` DATETIME NOT NULL ,
  `FeedCategoryId` SMALLINT(5) NULL DEFAULT NULL ,
  `FeedSubCategoryId` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`FeedId`) ,
  INDEX `fk_feed_feed_category_Idx` (`FeedCategoryId` ASC) ,
  INDEX `fk_feed_feed_sub_category_Idx` (`FeedSubCategoryId` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`FeedCategory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`FeedCategory` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`FeedCategory` (
  `FeedCategoryId` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`FeedCategoryId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`FeedInfo`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`FeedInfo` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`FeedInfo` (
  `FeedInfoId` BIGINT(20) NOT NULL AUTO_INCREMENT ,
  `FeedId` BIGINT(20) NULL DEFAULT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `Favorite` TINYINT(3) NOT NULL DEFAULT '0' ,
  `Clicks` SMALLINT(5) NULL DEFAULT NULL ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '2' ,
  PRIMARY KEY (`FeedInfoId`) ,
  INDEX `fk_feed_info_feed_Idx` (`FeedId` ASC) ,
  INDEX `fk_feed_info_UserIdx` (`UserId` ASC) ,
  CONSTRAINT `fk_feed_info_feed`
    FOREIGN KEY (`FeedId` )
    REFERENCES `PlanetX`.`Feed` (`FeedId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`FeedSubCategory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`FeedSubCategory` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`FeedSubCategory` (
  `FeedSubCategoryId` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  `FeedCategoryId` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`FeedSubCategoryId`) ,
  INDEX `fk_feed_sub_category_feed_category_Idx` (`FeedCategoryId` ASC) ,
  CONSTRAINT `fk_feed_sub_category_feed_category`
    FOREIGN KEY (`FeedCategoryId` )
    REFERENCES `PlanetX`.`FeedCategory` (`FeedCategoryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Friend`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Friend` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Friend` (
  `FriendId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `FriendUserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `IsSubscriber` TINYINT(1) NOT NULL DEFAULT '1' ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '0' ,
  `CreatedAt` DATETIME NOT NULL ,
  `UpdatedAt` TIMESTAMP NULL DEFAULT NULL ,
  PRIMARY KEY (`FriendId`) ,
  INDEX `fk_Friend_UserIdx` (`UserId` ASC) ,
  INDEX `fk_Friend_FriendUserIdx` (`FriendUserId` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 25
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Goverment`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Goverment` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Goverment` (
  `CountryId` CHAR(2) NOT NULL ,
  `LeaderId` MEDIUMINT(8)  NOT NULL ,
  `leaderType` TINYINT(3)  NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`CountryId`, `LeaderId`, `leaderType`) ,
  INDEX `fk_goverment_UserIdx` (`LeaderId` ASC) ,
  INDEX `fk_goverment_leader_code_Idx` (`leaderType` ASC) ,
  INDEX `fk_goverment_country_code_Idx` (`CountryId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`GovermentProvince`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`GovermentProvince` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`GovermentProvince` (
  `CountryId` CHAR(2) NOT NULL ,
  `Ground` SMALLINT(5) NOT NULL ,
  `Air` SMALLINT(5) NOT NULL ,
  `Navy` SMALLINT(5) NOT NULL ,
  `Nuclear` SMALLINT(5) NOT NULL ,
  `Special` SMALLINT(5) NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`CountryId`) ,
  CONSTRAINT `fk_goverment_province_country_code`
    FOREIGN KEY (`CountryId` )
    REFERENCES `PlanetX`.`CountryCode` (`CountryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`UserGroup`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`UserGroup` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`UserGroup` (
  `UserGroupId` MEDIUMINT(3) NOT NULL AUTO_INCREMENT ,
  `UserGroupName` VARCHAR(45) NULL DEFAULT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `Description` TEXT NULL DEFAULT NULL ,
  `UserGroupType` SMALLINT(5)  NOT NULL ,
  `Picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `Url` VARCHAR(255) NULL DEFAULT NULL ,
  `Active` TINYINT(1) NOT NULL DEFAULT '1' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`UserGroupId`) ,
  INDEX `fk_usergroup_UserIdx` (`UserId` ASC) ,
  INDEX `fk_usergroup_usergroup_code_Idx` (`UserGroupType` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`GroupMembership`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`GroupMembership` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`GroupMembership` (
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `UserGroupId` MEDIUMINT(8) NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`UserGroupId`, `UserId`) ,
  INDEX `fk_groupmembership_UserIdx` (`UserId` ASC) ,
  INDEX `fk_groupmembership_group_Idx` (`UserGroupId` ASC) ,
  CONSTRAINT `fk_groupmembership_group`
    FOREIGN KEY (`UserGroupId` )
    REFERENCES `PlanetX`.`UserGroup` (`UserGroupId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`ItemCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`ItemCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`ItemCode` (
  `ItemType` SMALLINT(5)  NOT NULL AUTO_INCREMENT ,
  `Item` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`ItemType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Lang`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Lang` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Lang` (
  `LanguageId` TINYINT(3) NOT NULL AUTO_INCREMENT ,
  `Lang` VARCHAR(45) NOT NULL DEFAULT 'en' ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  PRIMARY KEY (`LanguageId`) ,
  INDEX `fk_language_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`LeaderCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`LeaderCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`LeaderCode` (
  `LeaderType` TINYINT(3)  NOT NULL AUTO_INCREMENT ,
  `Code` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`LeaderType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`LoanCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`LoanCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`LoanCode` (
  `LoanType` TINYINT(3) NOT NULL AUTO_INCREMENT ,
  `Code` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`LoanType`) )
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`LoanFromBusiness`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`LoanFromBusiness` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`LoanFromBusiness` (
  `LoanId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `BusinessId` MEDIUMINT(8) NOT NULL ,
  `LoanType` TINYINT(3) NOT NULL ,
  `LoanAmount` DECIMAL(10,2) NULL DEFAULT NULL ,
  `MonthlyInterestRate` DECIMAL(5,2) NULL DEFAULT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`LoanId`) ,
  INDEX `fk_loan_from_business_business_Idx` (`BusinessId` ASC) ,
  INDEX `fk_loan_from_business_UserIdx` (`UserId` ASC) ,
  INDEX `fk_loan_from_business_loantype_Idx` (`LoanType` ASC) ,
  CONSTRAINT `fk_loan_from_business_business`
    FOREIGN KEY (`BusinessId` )
    REFERENCES `PlanetX`.`Business` (`BusinessId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_loan_from_business_loantype`
    FOREIGN KEY (`LoanType` )
    REFERENCES `PlanetX`.`LoanCode` (`LoanType` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`LoanFromPerson`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`LoanFromPerson` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`LoanFromPerson` (
  `LoanId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `SourceId` MEDIUMINT(8)  NOT NULL ,
  `LoanType` TINYINT(3) NOT NULL ,
  `LoanAmount` DECIMAL(10,2) NULL DEFAULT NULL ,
  `MonthlyInterestRate` DECIMAL(5,2) NULL DEFAULT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`LoanId`) ,
  INDEX `fk_loan_from_person_user_1_Idx` (`SourceId` ASC) ,
  INDEX `fk_loan_from_person_user_2_Idx` (`UserId` ASC) ,
  INDEX `fk_loan_from_person_loantype_Idx` (`LoanType` ASC) ,
  CONSTRAINT `fk_loan_from_person_loantype`
    FOREIGN KEY (`LoanType` )
    REFERENCES `PlanetX`.`LoanCode` (`LoanType` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
AUTO_INCREMENT = 2
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`MajorCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`MajorCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`MajorCode` (
  `MajorType` SMALLINT(5)  NOT NULL AUTO_INCREMENT ,
  `Major` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`MajorType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Message`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Message` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Message` (
  `MessageId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Message` VARCHAR(500) NOT NULL ,
  `CreatedAt` DATETIME NOT NULL ,
  `IsRead` TINYINT(1) NOT NULL DEFAULT '0' ,
  `IsSpam` TINYINT(1) NOT NULL DEFAULT '0' ,
  `To` MEDIUMINT(8) NULL DEFAULT NULL ,
  `IsReply` TINYINT(1) NULL DEFAULT '0' ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  PRIMARY KEY (`MessageId`) ,
  INDEX `fk_message_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`MilitaryForce`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`MilitaryForce` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`MilitaryForce` (
  `CountryId` CHAR(2) NOT NULL ,
  `Ground` SMALLINT(5) NOT NULL ,
  `Air` SMALLINT(5) NOT NULL ,
  `Navy` SMALLINT(5) NOT NULL ,
  `Nuclear` SMALLINT(5) NOT NULL ,
  `Special` SMALLINT(5) NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`CountryId`) ,
  CONSTRAINT `fk_Military_force_country_code`
    FOREIGN KEY (`CountryId` )
    REFERENCES `PlanetX`.`CountryCode` (`CountryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Notification`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Notification` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Notification` (
  `NotificationId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Msg` VARCHAR(255) NULL DEFAULT NULL ,
  `Type` SMALLINT(5) NULL DEFAULT NULL ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '0' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  PRIMARY KEY (`NotificationId`) ,
  INDEX `fk_activity_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Privacy`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Privacy` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Privacy` (
  `PrivacyId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Profile` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Address` TINYINT(3) NOT NULL DEFAULT '2' ,
  `Status` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Bookmark` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Feed` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Activity` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Friend` TINYINT(3) NOT NULL DEFAULT '1' ,
  `FriendList` TINYINT(3) NOT NULL DEFAULT '0' ,
  `Nickname` TINYINT(3) NOT NULL DEFAULT '1' ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  PRIMARY KEY (`PrivacyId`) ,
  INDEX `fk_privacy_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`PrivacyType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`PrivacyType` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`PrivacyType` (
  `PrivacyTypeId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`PrivacyTypeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Profile`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Profile` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`Profile` (
  `ProfileId` BIGINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `Privacy` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Rating` TINYINT(3) NULL DEFAULT '1' ,  
  `Dob` TIMESTAMP NULL DEFAULT NULL ,
  `AboutMe` VARCHAR(160) NULL DEFAULT NULL ,
  `Relationship` VARCHAR(45) NULL DEFAULT NULL ,
  `LookingFor` VARCHAR(45) NULL DEFAULT NULL ,
  `Phone` VARCHAR(45) NULL DEFAULT NULL ,
  `Interests` VARCHAR(255) NULL DEFAULT NULL ,
  `Education` VARCHAR(255) NULL DEFAULT NULL ,
  `Hobbies` VARCHAR(255) NULL DEFAULT NULL ,
  `FavMovies` VARCHAR(255) NULL DEFAULT NULL ,
  `FavArtists` VARCHAR(255) NULL DEFAULT NULL ,
  `FavBooks` VARCHAR(255) NULL DEFAULT NULL ,
  `FavAnimals` VARCHAR(255) NULL DEFAULT NULL ,
  `Religion` TINYINT(3) NULL DEFAULT NULL ,
  `EverythingElse` VARCHAR(255) NULL DEFAULT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  PRIMARY KEY (`ProfileId`) ,
  INDEX `fk_profile_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`ThumbUpDown`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`ThumbUpDown` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`ThumbUpDown` (
  `ThumbUpDownId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `Flag` TINYINT(1) NULL DEFAULT '1' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `StatusId` MEDIUMINT(8) NULL DEFAULT NULL ,
  `FriendId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  `UserId` MEDIUMINT(8)  NULL DEFAULT NULL ,
  PRIMARY KEY (`ThumbUpDownId`) ,
  INDEX `fk_thumb_up_down_status_Idx` (`StatusId` ASC) ,
  INDEX `fk_thumb_up_down_friend_Idx` (`FriendId` ASC) ,
  INDEX `fk_thumb_up_down_user` (`UserId` ASC),
  CONSTRAINT `fk_thumb_up_down_status`
    FOREIGN KEY (`StatusId` )
    REFERENCES `PlanetX`.`Status` (`StatusId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`UniversityCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`UniversityCode` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`UniversityCode` (
  `UniversityId` SMALLINT(5)  NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(100) NOT NULL ,
  PRIMARY KEY (`UniversityId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`UniversityLocation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`UniversityLocation` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`UniversityLocation` (
  `UniversityId` SMALLINT(5)  NOT NULL ,
  `CityId` MEDIUMINT(8)  NOT NULL ,
  `ProvinceId` SMALLINT(5)  NOT NULL ,
  `CountryId` CHAR(2) NOT NULL ,
  PRIMARY KEY (`UniversityId`) ,
  INDEX `fk_location_city_code_Idx` (`CityId` ASC) ,
  INDEX `fk_location_province_code_Idx` (`ProvinceId` ASC) ,
  INDEX `fk_location_country_code_Idx` (`CountryId` ASC) ,
  CONSTRAINT `fk_universitylocation_citycode`
    FOREIGN KEY (`CityId` )
    REFERENCES `PlanetX`.`CityCode` (`CityId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_universitylocation_countrycode`
    FOREIGN KEY (`CountryId` )
    REFERENCES `PlanetX`.`CountryCode` (`CountryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_universitylocation_provincecode`
    FOREIGN KEY (`ProvinceId` )
    REFERENCES `PlanetX`.`ProvinceCode` (`ProvinceId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_universitylocation_university`
    FOREIGN KEY (`UniversityId` )
    REFERENCES `PlanetX`.`UniversityCode` (`UniversityId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`UserChatroom`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`UserChatroom` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`UserChatroom` (
  `RoomId` MEDIUMINT(8) NOT NULL ,
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `Status` TINYINT(2) NULL DEFAULT '0' , 
  PRIMARY KEY (`UserId`,`RoomId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `PlanetX`.`UserConnection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`UserConnection` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`UserConnection` (
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `ConnectionId` VARCHAR(36) NOT NULL ,
  `UserAgent` VARCHAR(36) NOT NULL ,
  PRIMARY KEY (`ConnectionId`),
  INDEX `Index_UserId_UserConnection` (`UserId` ASC)   )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `PlanetX`.`GeneralLog`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`GeneralLog` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`GeneralLog` (
  `LogId` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `LogText`  TEXT ,
  PRIMARY KEY (`LogId`)  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PlanetX`.`Club`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`Club` ;
CREATE TABLE `PlanetX`.`Club` (
  `ClubId` MEDIUMINT(8)  NOT NULL AUTO_INCREMENT,  
  `UserId` MEDIUMINT(8)  NOT NULL,
  `ClubName` VARCHAR(25) NOT NULL DEFAULT '',
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`ClubId`),
    INDEX `fk_Club_WebUserIdx` (`ClubId` ASC))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
 -- -----------------------------------------------------
-- Table `PlanetX`.`PostClubACL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`PostClubACL` ;
CREATE TABLE `PlanetX`.`PostClubACL` (
  `PostId` MEDIUMINT(8) NOT NULL ,
  `ClubId` MEDIUMINT(8)  NOT NULL,  
  `AccessType` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`PostId`, `ClubId`),    
  CONSTRAINT `fk_Club_PostClubACL`
    FOREIGN KEY (`ClubId` )
    REFERENCES `PlanetX`.`Club` (`ClubId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
-- -----------------------------------------------------
-- Table `PlanetX`.`ClubMember`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`ClubMember` ;
CREATE TABLE `PlanetX`.`ClubMember` (
  `ClubMemberId` MEDIUMINT(8)  NOT NULL AUTO_INCREMENT,  
  `UserId` MEDIUMINT(8)  NOT NULL,  
  `ClubId` MEDIUMINT(8)  NOT NULL,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`ClubMemberId`),
    INDEX `fk_ClubMember_WebUserIdx` (`ClubMemberId` ASC))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
 
-- -----------------------------------------------------
-- Table `PlanetX`.`WebUserLocation`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PlanetX`.`WebUserLocation` ;

CREATE  TABLE IF NOT EXISTS `PlanetX`.`WebUserLocation` (
  `UserId` MEDIUMINT(8)  NOT NULL ,
  `CityId` MEDIUMINT(8)  NOT NULL ,
  `ProvinceId` SMALLINT(5)  NOT NULL ,
  `CountryId` CHAR(2) NOT NULL ,
  PRIMARY KEY (`UserId`) ,
  INDEX `fk_location_city_code_Idx` (`CityId` ASC) ,
  INDEX `fk_location_province_code_Idx` (`ProvinceId` ASC) ,
  INDEX `fk_location_country_code_Idx` (`CountryId` ASC) ,
  CONSTRAINT `fk_webuserlocation_citycode`
    FOREIGN KEY (`CityId` )
    REFERENCES `PlanetX`.`CityCode` (`CityId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_webuserlocation_countrycode`
    FOREIGN KEY (`CountryId` )
    REFERENCES `PlanetX`.`CountryCode` (`CountryId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_webuserlocation_provincecode`
    FOREIGN KEY (`ProvinceId` )
    REFERENCES `PlanetX`.`ProvinceCode` (`ProvinceId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- procedure GetFinanceContent
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetFinanceContent`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetFinanceContent`(
         IN  webUserId INT
         
    )
BEGIN
 SELECT 
 (SELECT SUM(AMOUNT) 
    FROM PlanetX.Card 
    WHERE UserId = webUserId and CardType=1) creditTotal
    ,
 
 (SELECT SUM(AMOUNT) 
    FROM PlanetX.Card 
    WHERE UserId = webUserId and CardType=2) debitTotal
    ,
    
 (SELECT SUM(LOANAMOUNT)  
    FROM PlanetX.LoanFromBusiness
    WHERE UserId = webUserId) businessLoanTotal
    ,
 
 (SELECT SUM(LOANAMOUNT) 
    FROM PlanetX.LoanFromPerson
    WHERE UserId = webUserId) personLoanTotal;
    
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetFriends
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetFriends`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetFriends`(
         IN  webUserId INT         
    )
BEGIN
 SELECT FriendUserId 
    FROM PlanetX.Friend 
    WHERE UserId = webUserId; 
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetFriendsInfo
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetFriendsInfo`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetFriendsInfo`(
         IN  webUserId INT
         
    )
BEGIN
 SELECT DISTINCT
 Friend.FriendUserId,
 WebUser.NameFirst,
 WebUser.NameMIddle,
 WebUser.NameLast,
 WebUser.EmailId,
 WebUser.Picture,
 WebUser.Active,
 WebUser.OnlineStatus
 FROM WebUser, Friend
 Where
 WebUser.UserId = Friend.FriendUserId
  AND
 Friend.UserId=webUserId;
 
    
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetProfileSummary
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetProfileSummary`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetProfileSummary`(
         IN  webUserId INT
         
    )
BEGIN
 SELECT 
 (SELECT count(*) 
    FROM PlanetX.Friend 
    WHERE UserId = webUserId ) FriendCount
    ,
 (SELECT count(*) 
    FROM PlanetX.Profile 
    WHERE UserId = webUserId ) ProfileCount;

    
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure UpdateOnlineStatus
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`UpdateOnlineStatus`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `UpdateOnlineStatus`(
         IN  webUserId INT,     
		 IN  onlineStatus TINYINT(3)		
    )
BEGIN
 Update PlanetX.WebUser 
	SET OnlineStatus = onlineStatus
    WHERE UserId = webUserId; 
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCityList
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetCityList`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetCityList`()
BEGIN
Select CityId, City, CountryId from PlanetX.CityCode;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCityListByRange
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetCityListByRange`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetCityListByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select CityId, City, CountryId from PlanetX.CityCode where  CityId BETWEEN startRange and endRange;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetCityListByRangeTotal
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetCityListByRangeTotal`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetCityListByRangeTotal`()
BEGIN
Select count(*)from PlanetX.CityCode ;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetStockValue
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetStockValue`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetStockValue`(
IN stockId INT
)
BEGIN
Select CurrentValue from PlanetX.Stock where StockId = @stockId;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetWebUserByRange
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserByRange`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
SELECT `UserId`,
    `NameFirst`,
    `NameMIddle`,
    `NameLast`,
    `EmailId`,
    NULL `OldNameFirst`,
    NULL `OldNameMIddle`,
    NULL `OldNameLast`,
    NULL `OldEmailId`,
    NULL `Picture`,
    NULL `ActionType`,
    `CreatedAt`
FROM `PlanetX`.`WebUser` order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserByRangeTotal
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserByRangeTotal`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserByRangeTotal`()
BEGIN
Select count(*) from PlanetX.WebUser;
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetBusinessContent
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetBusinessContent`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetBusinessContent`(
         IN  webUserId INT	
    )
BEGIN
SELECT
`Business`.`BusinessId`,
`Business`.`UserId`,
`Business`.`BusinessName`,
`Business`.`BusinessTypeId`,
`Business`.`BusinessSubtypeId`,
`Business`.`CityId`,
`Business`.`NetProfit`,
`Business`.`RunningCost`,
`Business`.`Active`,
`Business`.`CreatedAt`,
`Business`.`UpdatedAt`
FROM `PlanetX`.`Business` Where UserId =webUserId;

 END$$

 DELIMITER;
-- -----------------------------------------------------
-- procedure GetEducationContent
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `PlanetX`.`GetEducationContent`;

DELIMITER $$

CREATE DEFINER=`root`@`%` PROCEDURE `GetEducationContent`(
         IN  webUserId INT	
    )
BEGIN
SELECT
`Education`.`EducationId`,
`Education`.`UserId`,
`Education`.`DegreeType`,
`Education`.`MajorType`,
`Education`.`UniversityId`,
`Education`.`CreatedAt`,
`Education`.`UpdatedAt`,
MajorCode.Major,
DegreeCode.Degree,
UniversityCode.Name UniversityName
FROM `PlanetX`.`Education`
LEFT JOIN `PlanetX`.`DegreeCode` ON Education.DegreeType = DegreeCode.DegreeType
LEFT JOIN `PlanetX`.`MajorCode` ON Education.MajorType = MajorCode.MajorType
LEFT JOIN `PlanetX`.`UniversityCode` ON Education.UniversityId = UniversityCode.UniversityId
WHERE
Education.UserId = webUserId;
END$$

-- -----------------------------------------------------
-- procedure GetFriendsAndClubByUser
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `PlanetX`.`GetFriendsAndClubByUser`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetFriendsAndClubByUser`(
         IN  webUserId INT	
    )
BEGIN
SELECT t1.FriendUserId as Id, CONCAT(t2.NameFirst, ' ', t2.NameMiddle, ' ', t2.NameLast) as Value, 'Friend' as MemberType
    FROM PlanetX.Friend t1, PlanetX.WebUser t2
    WHERE 
	t1.FriendUserId= t2.UserId
	AND
t1.UserId = webUserId
UNION
SELECT t3.ClubId as Id, ClubName as Value, 'Club' as MemberType
    FROM PlanetX.Club t3
    WHERE 
	t3.UserId= webUserId; 
END$$

-- -----------------------------------------------------
-- procedure GetWebContentByPost
-- -----------------------------------------------------

DELIMITER ;
DROP procedure IF EXISTS `PlanetX`.`GetWebContentByPost`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebContentByPost`(
         IN  postIdList TEXT	
    )
BEGIN
set @sql = CONCAT('select * from PostWebContent where PostId in (', postIdList, ') ORDER BY PostWebContentId');
PREPARE q FROM @sql;
execute q;
END$$
DELIMITER ;

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetCommentsByPost`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetCommentsByPost`(
         IN  postIdList TEXT	
    )
BEGIN
set @sql = CONCAT('select * from PostComment where PostId in (', postIdList, ') ORDER BY ParentCommentId');
PREPARE q FROM @sql;
execute q;
END$$

-- -----------------------------------------------------
-- procedure GetPostForUserWithLimit
-- -----------------------------------------------------
DELIMITER ;
USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetPostForUserWithLimit`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetPostForUserWithLimit`(
        IN  webUserId INT,
		IN  userPostId INT,
		IN  postLimit INT
    )
BEGIN
SELECT DISTINCT
    t1 . *
FROM
    PlanetX.Post t1,
    PlanetX.Friend t2,
    PlanetX.PostUserACL t3
Where
    (t1.PostId < userPostId OR userPostId = -1)
        AND t1.UserId = t2.FriendUserId
        AND (t3.UserId = webUserId
        OR t1.UserId = webUserId
        OR (t1.UserACL = 1 AND t1.ClubACL = 1))
        AND ((t3.PostId = t1.PostId
        AND t3.AccessType = 1
        AND t3.AccessType != 0
        AND NOT EXISTS( SELECT 
            1
        from
            PlanetX.PostClubACL t4,
            PlanetX.ClubMember t5
        WHERE
            t4.ClubId = t5.ClubId
                AND t4.PostId = t1.PostId
                AND t5.UserId = webUserId
                AND t4.AccessType = 0))
        OR exists( SELECT 
            1
        from
            PlanetX.PostClubACL t6,
            PlanetX.ClubMember t7
        WHERE
            t6.ClubId = t7.ClubId
                AND t6.PostId = t1.PostId
                AND t7.UserId = webUserId
                AND t6.AccessType = 1
                AND t6.AccessType != 0
                AND NOT EXISTS( SELECT 
                    1
                FROM
                    PlanetX.PostUserACL t8
                Where
                    t8.PostId = t1.PostId
                        AND t8.UserId = webUserId
                        AND t8.AccessType = 0))
        OR t1.UserId = webUserId
        OR (t1.UserACL = 1 AND t1.ClubACL = 1))
Order By t1.PostId Desc
LIMIT postLimit;
END$$
-- -----------------------------------------------------
-- procedure GetTopNTopicTagContent
-- -----------------------------------------------------
DELIMITER ;
USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetTopNTopicTagContent`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetTopNTopicTagContent`(
         IN  tagLimit INT,	
		 IN  timeLimitMinute INT
    )
BEGIN
SELECT Tag, TagCount FROM `PlanetX`.`TopicTag`
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
USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetTopTopicTagContent`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetTopTopicTagContent`(
         IN  tagLimit INT			 
    )
BEGIN
SELECT Tag, TagCount FROM `PlanetX`.`TopicTag`
ORDER BY UpdatedAt DESC, TagCount DESC
LIMIT tagLimit
;

END$$
-- -----------------------------------------------------
-- procedure GetTopicByTag
-- -----------------------------------------------------
DELIMITER ;
USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetTopicByTag`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetTopicByTag`(
         IN  topicTag varchar(50)	
    )
BEGIN

Select * from TopicTag where Tag = TopicTag;

END$$


-- -----------------------------------------------------
-- procedure GetConnectionByUserId
-- -----------------------------------------------------
DELIMITER ;
USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetConnectionByUserId`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetConnectionByUserId`(
         IN  webUserId INT
    )
BEGIN

Select * from UserConnection where UserId = webUserId;

END$$

-- -----------------------------------------------------
-- procedure SaveTopicTag
-- -----------------------------------------------------
DELIMITER ;
USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`SaveTopicTag`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `SaveTopicTag`(
         IN  newTag VARCHAR(50)
    )
BEGIN
DECLARE counter INT DEFAULT 0;
START TRANSACTION;

SELECT TagCount into counter FROM PlanetX.TopicTag where Tag = newTag FOR UPDATE;
if counter >0 THEN
UPDATE PlanetX.TopicTag SET TagCount= counter + 1 where Tag = newTag;
ELSE 
INSERT INTO PlanetX.TopicTag 
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

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserInsertByRange`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserInsertByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select * from PlanetX.WebUserUpdate Where ActionType= 'I' order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserInsertUpdatesByRangeTotal
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserInsertUpdatesByRangeTotal`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserInsertUpdatesByRangeTotal`()
BEGIN
Select count(*) from PlanetX.WebUserUpdate Where ActionType= 'I';
 END$$

DELIMITER ;


-- -----------------------------------------------------
-- procedure GetWebUserUpdateByRange
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserUpdateByRange`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserUpdateByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select * from PlanetX.WebUserUpdate Where ActionType= 'U' order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserUpdateUpdatesByRangeTotal
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserUpdateUpdatesByRangeTotal`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserUpdateUpdatesByRangeTotal`()
BEGIN
Select count(*) from PlanetX.WebUserUpdate Where ActionType= 'U';
 END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure GetWebUserDeleteByRange
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserDeleteByRange`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserDeleteByRange`(
 IN  startRange INT	,
 IN  endRange INT	
 
)
BEGIN
Select * from PlanetX.WebUserUpdate Where ActionType= 'D' order by  UserId LIMIT startRange,  endRange;
 END$$

DELIMITER ;



-- -----------------------------------------------------
-- procedure GetWebUserDeleteUpdatesByRangeTotal
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetWebUserDeleteUpdatesByRangeTotal`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetWebUserDeleteUpdatesByRangeTotal`()
BEGIN
Select count(*) from PlanetX.WebUserUpdate Where ActionType= 'D';
 END$$

DELIMITER ;
-- -----------------------------------------------------
-- Trigger WebUserInsert
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `PlanetX`.`TriggerWebUserInsert` ;
DELIMITER $$
CREATE TRIGGER `PlanetX`.`TriggerWebUserInsert` 
AFTER INSERT  ON `PlanetX`.`WebUser` 
FOR EACH ROW BEGIN

 
SET @action = 'I';

INSERT INTO `PlanetX`.`WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameMIddle`,
`NameLast`,
`EmailId`,
`Picture`,
`ActionType`)
VALUES
(NEW.UserId,
NEW.NameFirst,
NEW.NameMIddle,
NEW.NameLast,
NEW.EmailId,
NEW.Picture,
@action
);
END$$

 DELIMITER ;


-- -----------------------------------------------------
-- Trigger WebUserUpdate
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `PlanetX`.`TriggerWebUserUpdate` ;
DELIMITER $$
CREATE TRIGGER `PlanetX`.`TriggerWebUserUpdate` 
AFTER Update  ON `PlanetX`.`WebUser` 
FOR EACH ROW BEGIN

 
SET @action = 'U';

Insert INTO `PlanetX`.`WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameMIddle`,
`NameLast`,
`EmailId`,
`OldNameFirst`,
`OldNameMIddle`,
`OldNameLast`,
`OldEmailId`,
`Picture`,
`ActionType`)
VALUES
(NEW.UserId,
NEW.NameFirst,
NEW.NameMIddle,
NEW.NameLast,
NEW.EmailId,
OLD.NameFirst,
OLD.NameMIddle,
OLD.NameLast,
OLD.EmailId,
NEW.Picture,
@action
);
END$$

 DELIMITER ;


-- -----------------------------------------------------
-- Trigger WebUserDelete
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `PlanetX`.`TriggerWebUserDelete` ;
DELIMITER $$
CREATE TRIGGER `PlanetX`.`TriggerWebUserDelete` 
AFTER Delete  ON `PlanetX`.`WebUser` 
FOR EACH ROW BEGIN

 
SET @action = 'D';

Insert INTO `PlanetX`.`WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameMIddle`,
`NameLast`,
`EmailId`,
`ActionType`)
VALUES
(OLD.UserId,
OLD.NameFirst,
OLD.NameMIddle,
OLD.NameLast,
OLD.EmailId,
@action
);
END$$

 DELIMITER ;
-- -----------------------------------------------------
-- procedure BuyStock
-- -----------------------------------------------------

USE `PlanetX`;
DROP PROCEDURE IF EXISTS `PlanetX`.`BuyStock`;
DELIMITER $$

USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `BuyStock`(
IN parmstockId INT,
IN parmtotalValue DECIMAL,
IN parmuserId INT,
IN parmstockUnit INT,
IN parmownerID INT
)
/*
0-Not Enough Cash
1-SucessFull
2-DB error
*/
  BEGIN

DECLARE total_new INT DEFAULT 0 ;
DECLARE result INT;
START TRANSACTION ;
Select Cash-totalValue INTO  total_new from PlanetX.UserBankAccount
 Where Cash>=totalValue and UserId = parmuserId LOCK IN SHARE MODE;
	IF total_new>=0 THEN
	
			UPDATE `PlanetX`.`UserBankAccount`
			SET `Cash` = total_new
			WHERE `UserId` = parmuserId;
			
			
			INSERT INTO `PlanetX`.`UserStocks`
			(`UserId`,`StockId`,`PurchasedUnit`,`CreatedAt`,`UpdatedAt`)VALUES
			(parmuserId,parmstockId,parmstockUnit,CURRENT_TIMESTAMP)
			ON DUPLICATE KEY UPDATE stockUnit=stockUnit+VALUES(parmstockUnit);
			
			
			Select Cash + totalValue INTO  total_new from PlanetX.UserBankAccount
				Where  UserId = parmownerID LOCK IN SHARE MODE;
			UPDATE `PlanetX`.`UserBankAccount`
			SET `Cash` = total_new 
			WHERE `UserId` = parmownerID;
			COMMIT;

			INSERT INTO `PlanetX`.`StocksTransaction`
			(`OwnerId`,`TransactionType`,`StockId`,`NumberOfUnit`,`TotalPrice`,`CreatedAt`)
			VALUES
			(parmuserId,0,parmstockId,parmstockUnit,totalValue,CURRENT_TIMESTAMP);

			SET result = 1;

			ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
  END $$

DELIMITER ;

-- -----------------------------------------------------
-- procedure SellStock
-- -----------------------------------------------------

USE `PlanetX`;
DROP PROCEDURE IF EXISTS `PlanetX`.`SellStock`;
DELIMITER $$

USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `SellStock`(
IN parmstockId INT,
IN parmtotalValue DECIMAL,
IN parmuserId INT,
IN parmstockUnit INT,
IN parmownerID INT
)
/*
0-Not Enough Cash
1-SucessFull
2-DB error
TransactionType can be Buy (0), Sell (1)
*/
  BEGIN

DECLARE total_new INT DEFAULT 0 ;
DECLARE result INT;
DECLARE stockUnit_new INT DEFAULT 0 ;
START TRANSACTION ;
Select Cash+totalValue INTO  total_new from PlanetX.UserBankAccount
 Where Cash>=totalValue and UserId = parmuserId LOCK IN SHARE MODE;
	IF total_new>=0 THEN
	
			UPDATE `PlanetX`.`UserBankAccount`
			SET `Cash` = total_new
			WHERE `UserId` = parmuserId;
		

			Select stockUnit-parmstockUnit INTO  stockUnit_new from PlanetX.UserStocks		
				Where UserId = parmuserId and stockId = parmstockId LOCK IN SHARE MODE;
			UPDATE PlanetX.UserStocks SET stockUnit= stockUnit_new where UserId = parmuserId  and stockId = parmstockId;
			
			
			Select Cash- totalValue INTO  total_new from PlanetX.UserBankAccount
				Where  UserId = parmownerID LOCK IN SHARE MODE;
			UPDATE `PlanetX`.`UserBankAccount`
			SET `Cash` = total_new  
			WHERE `UserId` = parmownerID;
			COMMIT;

			
			INSERT INTO `PlanetX`.`StocksTransaction`
			(`OwnerId`,`TransactionType`,`StockId`,`NumberOfUnit`,`TotalPrice`,`CreatedAt`)
			VALUES
			(parmuserId,1,parmstockId,parmstockUnit,totalValue,CURRENT_TIMESTAMP);
			
			SET result = 1;

			ELSE
			SET result = 0;
			ROLLBACK;
				
	END IF;

    select result;
  END $$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GetUserById
-- -----------------------------------------------------

USE `PlanetX`;
DROP procedure IF EXISTS `PlanetX`.`GetUserById`;

DELIMITER $$
USE `PlanetX`$$
CREATE DEFINER=`root`@`%` PROCEDURE `GetUserById`(
         IN  webUserId INT	
    )
BEGIN
SELECT
`WebUser`.`UserId`,
`WebUser`.`Password`,
`WebUser`.`NameFirst`,
`WebUser`.`NameMIddle`,
`WebUser`.`NameLast`,
`WebUser`.`EmailId`,
`WebUser`.`Picture`,
`WebUser`.`Active`,
`WebUser`.`OnlineStatus`,
`WebUser`.`CreatedAt`
FROM `PlanetX`.`WebUser` Where UserId =webUserId Limit 1;

 END$$

 DELIMITER ;
-- -----------------------------------------------------
SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;


use PlanetX;

SET foreign_key_checks = 0;

LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\Webuser.csv' INTO TABLE PlanetX.WebUser FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\Friend.csv' INTO TABLE PlanetX.Friend FIELDS TERMINATED BY ',' SET CreatedAt = current_timestamp, UpdatedAt = current_timestamp;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\BusinessTypes.csv' INTO TABLE PlanetX.BusinessCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\BusinessSubTypes.csv' INTO TABLE PlanetX.BusinessSubCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\country_list.csv' INTO TABLE PlanetX.CountryCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetX\\PlanetX2012\\DB\\citylist.csv' INTO TABLE PlanetX.CityCode FIELDS TERMINATED BY ',' LINES TERMINATED BY '\r\n' (CountryId, City) SET CityId=NUll  ;
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
