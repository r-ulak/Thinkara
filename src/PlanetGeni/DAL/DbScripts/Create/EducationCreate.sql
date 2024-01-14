
-- -----------------------------------------------------
-- Table `DegreeCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `DegreeCode` ;

CREATE  TABLE IF NOT EXISTS `DegreeCode` (
  `DegreeId` TINYINT(3)  NOT NULL  ,
  `DegreeName` VARCHAR(25) NOT NULL ,
  `DegreeImageFont` VARCHAR(50) NOT NULL ,
  `DegreeRank` TINYINT(3) NOT NULL ,
  PRIMARY KEY (`DegreeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `MajorCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `MajorCode` ;

CREATE  TABLE IF NOT EXISTS `MajorCode` (
  `MajorId` SMALLINT(3)   NOT NULL  ,
  `MajorName` VARCHAR(50) NOT NULL ,
  `ImageFont` VARCHAR(50) NOT NULL ,
  `Description` VARCHAR(250) NOT NULL ,
  `MajorRank` TINYINT(3) NOT NULL ,
  `Cost` DECIMAL(50,2) NOT NULL DEFAULT '0.00' ,
  `Duration` TINYINT(3) NOT NULL ,  -- in hrs
  `JobProbability` DECIMAL(5,2) NOT NULL ,
  `IndustryId` TINYINT(3)   NOT NULL  ,
  PRIMARY KEY (`MajorId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `Education`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Education` ;

CREATE  TABLE IF NOT EXISTS `Education` (  
  `UserId` INT   NOT NULL ,
  `MajorId` SMALLINT(3)   NOT NULL  ,
  `DegreeId` TINYINT(3)  NOT NULL  ,
  `Status` CHAR(1)  NOT NULL DEFAULT 'I', -- C- done, I - InProgress
  `CompletionCost` DECIMAL(50,2) NOT NULL DEFAULT '0.00' ,
  `ExpectedCompletion` DATETIME NOT NULL ,  
  `NextBoostAt` DATETIME NOT NULL ,  
  `CreatedAt` DATETIME NOT NULL ,  
  PRIMARY KEY (`UserId`,`MajorId`, `DegreeId` ))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

