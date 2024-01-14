
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

-- -----------------------------------------------------
-- Table `WebJobHistory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `WebJobHistory` ;

CREATE  TABLE IF NOT EXISTS `WebJobHistory` (
  `JobId` TINYINT(3)  NOT NULL  ,
  `RunId`  INT  NOT NULL  AUTO_INCREMENT,
  `CreatedAT` DATETIME(6) NOT NULL ,
  PRIMARY KEY (`RunId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `DegreeCheckJob`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `DegreeCheckJob` ;

CREATE  TABLE IF NOT EXISTS `DegreeCheckJob` (  
  `RunId`  INT NOT NULL  ,
  `UserId` INT   NOT NULL ,
  `MajorId` SMALLINT(3)   NOT NULL  ,
  `DegreeId` TINYINT(3)  NOT NULL  ,
  PRIMARY KEY (`RunId`, `UserId`,`MajorId` , `DegreeId`  ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
