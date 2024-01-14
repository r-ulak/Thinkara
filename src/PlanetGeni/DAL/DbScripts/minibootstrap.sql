
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
   `Title` VARCHAR(100)  NOT NULL ,  
   `Cost` DECIMAL(10,2) NOT NULL , 
  PRIMARY KEY (`AdvertisementId`, `UserId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `AdsType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `AdsType` ;

CREATE  TABLE IF NOT EXISTS `AdsType` (
   `AdsTypeId` TINYINT(3)  NOT NULL AUTO_INCREMENT,
   `AdName` VARCHAR(250)  NOT NULL,   
   `BaseCost` DECIMAL(8,2) NOT NULL ,   
   `PricePerChar` DECIMAL(8,2) NOT NULL, 
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
-- Table `CountryTaxByType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryTaxByType` ;

CREATE  TABLE IF NOT EXISTS `CountryTaxByType` (
  `TaskId` CHAR(36) NOT NULL,
  `TaxPercent` DECIMAL(5,2) NOT NULL ,  
  `TaxType` TINYINT(3) NOT NULL ,  
  PRIMARY KEY (`TaskId`, `TaxType`  ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
-- -----------------------------------------------------
-- Table `TaxCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `TaxCode` ;

CREATE  TABLE IF NOT EXISTS `TaxCode` (
  `TaxType` TINYINT(3) NOT NULL ,    
  `Description` VARCHAR(1000)  NOT NULL ,
  `ImageFont` VARCHAR(50)  NOT NULL ,
  PRIMARY KEY (`TaxType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\Data\\TaxCode.csv' INTO TABLE planetgeni.TaxCode FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\Data\\CountryTaxByType.csv' INTO TABLE planetgeni.CountryTaxByType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\Data\\AdsFrequencyType.csv' INTO TABLE planetgeni.AdsFrequencyType FIELDS TERMINATED BY ',';
LOAD DATA LOCAL INFILE 'D:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\Data\\AdsType.csv' INTO TABLE planetgeni.AdsType FIELDS TERMINATED BY ',';
