-- -----------------------------------------------------
-- Table `JobCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `JobCode` ;

CREATE  TABLE IF NOT EXISTS `JobCode` (
  `JobCodeId` SMALLINT(5)   NOT NULL  AUTO_INCREMENT,
  `Title` VARCHAR(250) NOT NULL ,     
  `JobType` CHAR(1)   NOT NULL , -- F FullTime, C Contract, P PartTime
  `Duration` SMALLINT(5)  NOT NULL  ,
  `CheckInDuration` TINYINT(2)  NOT NULL  ,
  `MinimumMatchScore` INT NOT NULL  ,
  `MaxHPW` TINYINT(3)  NOT NULL  ,  -- HPW Hours Per Week  
  `OverTimeRate` Decimal(4,2)  NOT NULL  ,
  `IndustryId` TINYINT(3)   NOT NULL  ,
  `ReqMajorId` SMALLINT(3)   NOT NULL  ,
  `ReqDegreeId` TINYINT(3)  NOT NULL  ,    
  `IndustryExperience` SMALLINT(5)  NOT NULL  ,
  `JobExperience` SMALLINT(5)  NOT NULL  , 
 PRIMARY KEY (`JobCodeId`) ,
 INDEX `ik_jobcode_jobtype` (`JobType` ASC) ,
 INDEX `ik_jobcode_industryidx` (`IndustryId` ASC) ,
 INDEX `ik_jobcode_ReqMajorIdx` (`ReqMajorId` ASC),
 INDEX `ik_jobcode_ReqDegreeIdx` (`ReqDegreeId` ASC) 
 )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `UserJob`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserJob` ;

CREATE  TABLE IF NOT EXISTS `UserJob` (  
  `TaskId` CHAR(36) NOT NULL  ,
  `UserId` INT  NOT NULL ,
  `JobCodeId` SMALLINT(5)   NOT NULL  ,  
  `StartDate` DATETIME NOT NULL ,  
  `EndDate` DATETIME NOT NULL ,  
  `Salary` DECIMAL(50,2) NOT NULL ,    
  `IncomeYearToDate` DECIMAL(50,2) NOT NULL ,    
  `NextOverTimeCheckIn` DATETIME NOT NULL ,  
  `CheckInDuration` TINYINT(2)  NOT NULL  ,
  `OverTimeHours` SMALLINT(5) NOT NULL ,  
  `LastCycleOverTimeHours` SMALLINT(5) DEFAULT 0 ,
  `Status` CHAR(1) NOT NULL ,  -- A Accepted, P Pending, D Denied, E ended, O Open Offer, R Rejected, Q Quit, W Withdrawn
  `AppliedOn` DATETIME NOT NULL ,  
  `JobExiredEmailSent` TINYINT(1) DEFAULT 0
  `UpdatedAt` DATETIME NOT NULL ,  
  PRIMARY KEY (`TaskId` ),
  INDEX `ik_userjob_useridx` (`UserId` ASC) ,
  INDEX `ik_userjob_jobcodeidx` (`JobCodeId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `JobPayCheck`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `JobPayCheck` ;

CREATE  TABLE IF NOT EXISTS `JobPayCheck` (  
  `TaskId` CHAR(36) NOT NULL  ,
  `UserId` INT  NOT NULL ,  
  `Amount` DECIMAL(50,2) NOT NULL ,  
  `Tax` DECIMAL(50,2) NOT NULL ,  
  `PayDate` DATETIME NOT NULL ,    
  PRIMARY KEY (`TaskId` ),
  INDEX `ik_userjob_useridx` (`UserId` ASC) ,
  INDEX `ik_paycheck_paydateidx` (`PayDate` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `JobCountry`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `JobCountry` ;

CREATE  TABLE IF NOT EXISTS `JobCountry` (  
  `JobCodeId` SMALLINT(5)   NOT NULL  ,
  `CountryId` CHAR(2) NOT NULL ,  
  `Salary` DECIMAL(50,2) NOT NULL ,  
  `QuantityAvailable` INT NOT NULL ,    
  PRIMARY KEY (`JobCodeId`, `CountryId`  ),
  INDEX `ik_JobCountry_Availablex` (`QuantityAvailable` ASC) USING BTREE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `IndustryCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `IndustryCode` ;

CREATE  TABLE IF NOT EXISTS `IndustryCode` (
  `IndustryId` TINYINT(3)   NOT NULL  ,
  `IndustryName` VARCHAR(100) NOT NULL ,  
  `ImageFont` VARCHAR(50)NOT NULL ,
  PRIMARY KEY (`IndustryId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
