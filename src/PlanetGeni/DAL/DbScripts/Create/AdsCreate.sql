
-- -----------------------------------------------------
-- Table `Advertisement`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Advertisement` ;

CREATE  TABLE IF NOT EXISTS `Advertisement` (
   `AdvertisementId` CHAR(36),
   `UserId` INT   NOT NULL ,
   `AdsTypeEmail` TinyInt(1)    DEFAULT 0 ,
   `AdsTypeFeed` TinyInt(1)    DEFAULT 0 ,
   `AdsTypePartyMember` TinyInt(1)    DEFAULT 0 ,   
   `AdsTypeCountryMember` TinyInt(1)    DEFAULT 0 ,   
   `AdsFrequencyTypeId` TINYINT(3) NOT NULL,
   `DaysS` TinyInt(1)  DEFAULT 0,   
   `DaysM` TinyInt(1)    DEFAULT 0,   
   `DaysT` TinyInt(1)    DEFAULT 0,   
   `DaysW` TinyInt(1)    DEFAULT 0,   
   `DaysTh` TinyInt(1)    DEFAULT 0,   
   `DaysF` TinyInt(1)    DEFAULT 0,   
   `DaysSa` TinyInt(1)    DEFAULT 0,      
   `AdTime` SMALLINT(4)  NOT NULL  ,
   `StartDate` DATETIME  NOT NULL  ,
   `EndDate` DATETIME  NOT NULL ,   
   `PreviewMsg` TEXT  NOT NULL ,   
   `Message` VARCHAR(1000)  NOT NULL ,
   `Cost` DECIMAL(10,2) NOT NULL , 
  PRIMARY KEY (`AdvertisementId`, `UserId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
