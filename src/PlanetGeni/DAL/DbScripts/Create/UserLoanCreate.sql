

-- -----------------------------------------------------
-- Table `CreditScore`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CreditScore` ;

CREATE  TABLE IF NOT EXISTS `CreditScore` (
  `UserId` INT   NOT NULL ,
  `Score` DECIMAL(50,2) NOT NULL,
  PRIMARY KEY (`UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `CreditInterest`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CreditInterest` ;

CREATE  TABLE IF NOT EXISTS `CreditInterest` (
  `MinScore` SMALLINT   NOT NULL ,
  `MaxScore` SMALLINT NOT NULL,
  `MinMonthlyIntrestRate` DECIMAL(5,2) NOT NULL,
  `QualifiedAmount` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`MinScore`, `MaxScore`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;



-- -----------------------------------------------------
-- Table `UserLoan`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserLoan` ;

CREATE  TABLE IF NOT EXISTS `UserLoan` (
  `TaskId` CHAR(36) NOT NULL,  
  `UserId` INT   NOT NULL ,
  `LendorId` INT   NOT NULL ,  
  `LoanAmount` DECIMAL(10,2) NOT NULL ,
  `LeftAmount` DECIMAL(10,2) NOT NULL ,
  `PaidAmount` DECIMAL(10,2) NOT NULL ,
  `MonthlyInterestRate` DECIMAL(5,2) NOT NULL ,
  `Status` CHAR(1)  NOT NULL DEFAULT 'P' , -- A Approved, D - Denied, P- Pending
  `CreatedAt` DATETIME NOT NULL ,
  `UpdatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`TaskId`) ,
  INDEX `ik_userloan_useridx` (`UserId` ASC) ,
  INDEX `ik_userloan_lendoridx` (`LendorId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
