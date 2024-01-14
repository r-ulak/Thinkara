
-- -----------------------------------------------------
-- Table `CountryBudget`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryBudget` ;

CREATE  TABLE IF NOT EXISTS `CountryBudget` (
  `TaskId` CHAR(36) NOT NULL ,
  `CountryId` CHAR(2) NOT NULL , 
  `TotalAmount` DECIMAL(60,2) NOT NULL ,
  `StartDate` DATETIME NOT NULL ,
  `EndDate` DATETIME NOT NULL ,
  `Status` CHAR(1) NOT NULL DEFAULT 'P' , -- A Approved, D - Denied, P- Pending, E-expired
  `CreatedAt` DATETIME NOT NULL ,
   PRIMARY KEY (`TaskId`),
   INDEX `ik_countryid` (`CountryId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `CountryBudgetByType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryBudgetByType` ;

CREATE  TABLE IF NOT EXISTS `CountryBudgetByType` (
  `TaskId` CHAR(36) NOT NULL ,  
  `Amount` DECIMAL(30,2) NOT NULL ,
  `AmountLeft` DECIMAL(30,2) NOT NULL ,
  `BudgetType` TINYINT(3) NOT NULL ,  
  PRIMARY KEY (`TaskId`, `BudgetType`  ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;


-- -----------------------------------------------------
-- Table `BudgetCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `BudgetCode` ;

CREATE  TABLE IF NOT EXISTS `BudgetCode` (
  `BudgetType` TINYINT(3) NOT NULL ,  
  `Description` VARCHAR(1000)  NOT NULL ,
  `ImageFont` VARCHAR(50) NOT NULL  ,
  PRIMARY KEY (`BudgetType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
