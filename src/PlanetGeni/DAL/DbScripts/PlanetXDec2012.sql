-- -----------------------------------------------------
-- Table `Avatar`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Avatar` ;

CREATE  TABLE IF NOT EXISTS `Avatar` (
  `AvatarId` MEDIUMINT(8)  NOT NULL ,
  `Picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`AvatarId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `Goverment`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Goverment` ;

CREATE  TABLE IF NOT EXISTS `Goverment` (
  `CountryId` CHAR(2) NOT NULL ,
  `LeaderId` MEDIUMINT(8)   NOT NULL ,
  `leaderType` TINYINT(3)  NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`CountryId`, `LeaderId`, `leaderType`) ,
  INDEX `ik_goverment_UserIdx` (`LeaderId` ASC) ,
  INDEX `ik_goverment_leader_code_Idx` (`leaderType` ASC) ,
  INDEX `ik_goverment_country_code_Idx` (`CountryId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `GovermentProvince`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GovermentProvince` ;

CREATE  TABLE IF NOT EXISTS `GovermentProvince` (
  `CountryId` CHAR(2) NOT NULL ,
  `Ground` SMALLINT(5)  NOT NULL ,
  `Air` SMALLINT(5)  NOT NULL ,
  `Navy` SMALLINT(5)  NOT NULL ,
  `Nuclear` SMALLINT(5)  NOT NULL ,
  `Special` SMALLINT(5)  NOT NULL ,
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP ,
  PRIMARY KEY (`CountryId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `Lang`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Lang` ;

CREATE  TABLE IF NOT EXISTS `Lang` (
  `LanguageId` TINYINT(3) NOT NULL AUTO_INCREMENT ,
  `Lang` VARCHAR(45) NOT NULL DEFAULT 'en' ,
  `UserId` INT   NULL DEFAULT NULL ,
  PRIMARY KEY (`LanguageId`) ,
  INDEX `ik_language_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `Privacy`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Privacy` ;

CREATE  TABLE IF NOT EXISTS `Privacy` (
  `PrivacyId` MEDIUMINT(8)  NOT NULL AUTO_INCREMENT ,
  `Profile` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Address` TINYINT(3) NOT NULL DEFAULT '2' ,
  `Status` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Bookmark` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Feed` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Activity` TINYINT(3) NOT NULL DEFAULT '1' ,
  `Friend` TINYINT(3) NOT NULL DEFAULT '1' ,
  `FriendList` TINYINT(3) NOT NULL DEFAULT '0' ,
  `Nickname` TINYINT(3) NOT NULL DEFAULT '1' ,
  `UserId` INT   NULL DEFAULT NULL ,
  PRIMARY KEY (`PrivacyId`) ,
  INDEX `ik_privacy_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PrivacyType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PrivacyType` ;

CREATE  TABLE IF NOT EXISTS `PrivacyType` (
  `PrivacyTypeId` MEDIUMINT(8)  NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`PrivacyTypeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `Profile`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Profile` ;

CREATE  TABLE IF NOT EXISTS `Profile` (
  `ProfileId` BIGINT(8) NOT NULL AUTO_INCREMENT ,
  `UserId` INT   NOT NULL ,
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
  INDEX `ik_profile_UserIdx` (`UserId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;



-- -----------------------------------------------------
-- Table `UserChatroom`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserChatroom` ;

CREATE  TABLE IF NOT EXISTS `UserChatroom` (
  `RoomId` MEDIUMINT(8)  NOT NULL ,
  `UserId` INT   NOT NULL ,
  `Status` TINYINT(2) NULL DEFAULT '0' , 
  PRIMARY KEY (`UserId`,`RoomId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `UserConnection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserConnection` ;

CREATE  TABLE IF NOT EXISTS `UserConnection` (
  `UserId` INT   NOT NULL ,
  `ConnectionId` VARCHAR(36) NOT NULL ,
  `UserAgent` VARCHAR(36) NOT NULL ,
  PRIMARY KEY (`ConnectionId`),
  INDEX `Index_UserId_UserConnection` (`UserId` ASC)   )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `GeneralLog`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `GeneralLog` ;

CREATE  TABLE IF NOT EXISTS `GeneralLog` (
  `LogId` MEDIUMINT(8)  NOT NULL AUTO_INCREMENT ,
  `LogText`  TEXT ,
  PRIMARY KEY (`LogId`)  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `Club`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Club` ;
CREATE TABLE `Club` (
  `ClubId` MEDIUMINT(8)   NOT NULL AUTO_INCREMENT,  
  `UserId` INT   NOT NULL,
  `ClubName` VARCHAR(25) NOT NULL DEFAULT '',
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`ClubId`),
    INDEX `ik_Club_WebUserIdx` (`ClubId` ASC))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
 -- -----------------------------------------------------
-- Table `PostClubACL`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PostClubACL` ;
CREATE TABLE `PostClubACL` (
  `PostId` BIGINT  NOT NULL ,
  `ClubId` MEDIUMINT(8)   NOT NULL,  
  `AccessType` TINYINT(3) NOT NULL DEFAULT '1' ,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`PostId`, `ClubId`),    
  CONSTRAINT `fk_Club_PostClubACL`
    FOREIGN KEY (`ClubId` )
    REFERENCES `Club` (`ClubId` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
-- -----------------------------------------------------
-- Table `ClubMember`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ClubMember` ;
CREATE TABLE `ClubMember` (
  `ClubMemberId` MEDIUMINT(8)   NOT NULL AUTO_INCREMENT,  
  `UserId` INT   NOT NULL,  
  `ClubId` MEDIUMINT(8)   NOT NULL,  
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  `UpdatedAt` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP , 
  PRIMARY KEY (`ClubMemberId`),
    INDEX `ik_ClubMember_UserIdx` (`UserId` ASC),
	INDEX `ik_ClubMember_ClubIdx` (`ClubId` ASC))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
 