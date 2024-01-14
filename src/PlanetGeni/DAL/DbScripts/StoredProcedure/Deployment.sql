ALTER TABLE `planetgeni`.`Election` 
ADD COLUMN `StartTermNotified` TINYINT(1) NULL DEFAULT 0 COMMENT '' AFTER `Fee`,
ADD COLUMN `VotingStartTermNotified` TINYINT(1) NULL DEFAULT 0 COMMENT '' AFTER `StartTermNotified`,
ADD COLUMN `LastDayTermNotified` TINYINT(1) NULL DEFAULT 0 COMMENT '' AFTER `VotingStartTermNotified`;


-- -----------------------------------------------------
-- Table `WebJob`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `WebJob` ;

CREATE  TABLE IF NOT EXISTS `WebJob` (
  `JobId` TINYINT(3)  NOT NULL  ,
  `JobName` VARCHAR(30) NOT NULL ,  
  PRIMARY KEY (`JobId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

LOAD DATA LOCAL INFILE 'C:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\Data\\WebJob.csv' INTO TABLE WebJob FIELDS TERMINATED BY ',';

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

LOAD DATA LOCAL INFILE 'C:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\Data\\NotificationType.csv' INTO TABLE NotificationType FIELDS TERMINATED BY ',';


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

LOAD DATA LOCAL INFILE 'C:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\Data\\PostContentType.csv' INTO TABLE PostContentType FIELDS TERMINATED BY ',';
