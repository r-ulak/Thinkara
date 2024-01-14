
-- -----------------------------------------------------
-- Table `CountryTax`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryTax` ;

CREATE  TABLE IF NOT EXISTS `CountryTax` (
  `TaskId` CHAR(36) NOT NULL, 
  `CountryId` CHAR(2) NOT NULL ,   
  `Status` CHAR(1)  NOT NULL DEFAULT 'P' , -- A Approved, D - Denied, P- Pending, E-expired
  `StartDate` DATETIME NOT NULL ,
  `EndDate` DATETIME NOT NULL ,  
  `CreatedAt` DATETIME NOT NULL ,
   PRIMARY KEY (`TaskId`),
   INDEX `ik_countryid` (`CountryId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


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
-- Table `CountryRevenue`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryRevenue` ;

CREATE  TABLE IF NOT EXISTS `CountryRevenue` (
  `TaskId` CHAR(36) NOT NULL,
  `CountryId` CHAR(2) NOT NULL ,
  `Cash` DECIMAL(50,2)  NOT NULL ,
  `Status` CHAR(1)  NOT NULL , -- D- Done, P- Pending
  `TaxType` TINYINT(3) NOT NULL   ,
  `UpdatedAt` DATETIME NOT NULL  ,  
  PRIMARY KEY (`TaskId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `TaxCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `TaxCode` ;

CREATE  TABLE IF NOT EXISTS `TaxCode` (
  `TaxType` TINYINT(3) NOT NULL ,    
  `Description` VARCHAR(1000)  NOT NULL ,
  `ImageFont` VARCHAR(50)  NOT NULL ,
  `AllowEdit` TINYINT(1) DEFAULT 0 ,
  PRIMARY KEY (`TaxType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;