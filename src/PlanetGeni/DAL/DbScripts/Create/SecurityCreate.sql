-- -----------------------------------------------------
-- Table `WeaponType`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `WeaponType` ;

CREATE  TABLE IF NOT EXISTS `WeaponType` (
  `WeaponTypeId` SMALLINT(4)  NOT NULL AUTO_INCREMENT ,
  `Name` VARCHAR(45) NOT NULL  ,
  `Description` VARCHAR(255) NOT NULL  ,
  `ImageFont` VARCHAR(50)  NOT NULL ,
  `Cost`  Decimal(15,2) NOT NULL ,
  `WeaponTypeCode` Char(1)  NOT NULL  ,
  `OffenseScore` TINYINT(3)  NOT NULL Default 0 ,
  `DefenseScore` TINYINT(3)  NOT NULL Default 0  ,
  PRIMARY KEY (`WeaponTypeId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `CountryWeapon`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CountryWeapon` ;

CREATE  TABLE IF NOT EXISTS `CountryWeapon` (
  `CountryWeaponId` INT  NOT NULL AUTO_INCREMENT ,
  `CountryId` CHAR(2) NOT NULL ,
  `UserId` INT   NOT NULL ,
  `Quantity` INT   NOT NULL ,  
  `WeaponTypeId` SMALLINT(4)  NOT NULL ,
  `WeaponCondition` TinyInt(3) UNSIGNED NOT NULL ,
  `PurchasedPrice`  Decimal(15,2) NOT NULL ,
  `PurchasedAt` DATETIME NULL DEFAULT NULL ,  
  PRIMARY KEY (`CountryWeaponId`) ,
  INDEX `ik_CountryWeapon_CountryIdx` (`CountryId` ASC),
  INDEX `ik_CountryWeapon_WeaponTypeIdx` (`WeaponTypeId` ASC) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `RequestWarKey`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `RequestWarKey` ;

CREATE  TABLE IF NOT EXISTS `RequestWarKey` (
  `TaskId` CHAR(36)  NOT NULL ,
  `RequestingCountryId` CHAR(2) NOT NULL ,
  `TaregtCountryId` CHAR(2) NOT NULL ,
  `RequestingUserId` INT   NOT NULL ,
  `RequestedAt` DATETIME NOT NULL  ,
  `ApprovalStatus` CHAR(1)  NOT NULL DEFAULT 'W' , -- A-Approved, D-Denied, W-Waiting Approval
  `WarStatus` CHAR(1)  NOT NULL DEFAULT 'N' , -- O-OnGoing, N-NotStarted, F-Finished
  `WiningCountryId` CHAR(2)  NULL  ,
  PRIMARY KEY (`TaskId`),
INDEX `ik_requestwarkey_requestingcountryIdx` (`RequestingCountryId` ASC),
INDEX `ik_requestwarkey_taregtcountryidx` (`TaregtCountryId` ASC),
INDEX `ik_requestwarkey_approvalstatusix` (`ApprovalStatus` ASC)   )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table WarResult
-- -----------------------------------------------------
DROP TABLE IF EXISTS `WarResult` ;

CREATE  TABLE IF NOT EXISTS `WarResult` (
  `TaskId` CHAR(36)  NOT NULL ,
  `CountryIdAttacker` CHAR(2) NOT NULL ,
  `CountryIdDefender` CHAR(2) NOT NULL ,
  `WinQuality` TINYINT(3) NOT NULL ,
  `WarResult` CHAR(1)  NOT NULL DEFAULT '' , -- A Attacker Won, D Defender Won
  `EndDate` DATETIME NOT NULL  ,
  PRIMARY KEY (`TaskId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
