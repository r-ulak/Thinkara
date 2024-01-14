
-- -----------------------------------------------------
-- Table `SlotMachineThree`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `SlotMachineThree` ;

CREATE  TABLE IF NOT EXISTS `SlotMachineThree` (
  `MatchNumber` TINYINT(2)  NOT NULL ,   
  `ImageFont` VARCHAR(50)  NOT NULL ,  
  PRIMARY KEY (`MatchNumber`)	)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `PickThree`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PickThree` ;

CREATE  TABLE IF NOT EXISTS `PickThree` (
  `PickThreeId` CHAR(36) NOT NULL ,
  `DrawingId` INT NOT NULL ,
  `UserId` MEDIUMINT(8) NOT NULL ,
  `Number1` TINYINT(2)  NOT NULL ,
  `Number2` TINYINT(2)  NOT NULL ,
  `Number3` TINYINT(2)  NOT NULL , 
  PRIMARY KEY (`PickThreeId`) ,  
   INDEX `ik_pickthree_drawingid` (`DrawingId` ASC),
   INDEX `ik_pickthree_userid` (`UserId` ASC)	)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PickFive`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PickFive` ;

CREATE  TABLE IF NOT EXISTS `PickFive` (
  `PickFiveId` CHAR(36) NOT NULL ,
  `DrawingId` INT NOT NULL ,
  `UserId` MEDIUMINT(8) NOT NULL ,
  `Number1` TINYINT(2)  NOT NULL ,
  `Number2` TINYINT(2)  NOT NULL ,
  `Number3` TINYINT(2)  NOT NULL ,
  `Number4` TINYINT(2)  NOT NULL ,
  `Number5` TINYINT(2)  NOT NULL ,
  PRIMARY KEY (`PickFiveId`) ,  
   INDEX `ik_pickfive_drawingid` (`DrawingId` ASC),
   INDEX `ik_pickfive_userid` (`UserId` ASC)	)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `PickThreeWinNumber`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PickThreeWinNumber` ;

CREATE  TABLE IF NOT EXISTS `PickThreeWinNumber` (
  `DrawingId` INT NOT NULL ,  
  `Number1` TINYINT(2)  NOT NULL ,
  `Number2` TINYINT(2)  NOT NULL ,
  `Number3` TINYINT(2)  NOT NULL ,
  `DrawingDate` DATETIME NOT NULL ,
  PRIMARY KEY (`DrawingId`),
   INDEX `ik_pickthreewinnumber_drawingid` (`DrawingId` ASC)	)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `PickFiveWinNumber`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PickFiveWinNumber` ;

CREATE  TABLE IF NOT EXISTS `PickFiveWinNumber` (
  `DrawingId` INT NOT NULL ,  
  `Number1` TINYINT(2)  NOT NULL ,
  `Number2` TINYINT(2)  NOT NULL ,
  `Number3` TINYINT(2)  NOT NULL ,
  `Number4` TINYINT(2)  NOT NULL ,
  `Number5` TINYINT(2)  NOT NULL , 
  `DrawingDate` DATETIME NOT NULL ,
  PRIMARY KEY (`DrawingId`),
   INDEX `ik_pickfivewinnumber_drawingid` (`DrawingId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `PickThreeWinNumber`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PickThreeWinner` ;

CREATE  TABLE IF NOT EXISTS `PickThreeWinner` (
  `WinId` INT   NOT NULL AUTO_INCREMENT ,
  `DrawingId` INT NOT NULL ,  
  `UserId` MEDIUMINT(8) NOT NULL ,
  `Amount` DECIMAL(50,2)  NOT NULL ,       
    PRIMARY KEY (`WinId`),
   INDEX `ik_pickthreewinner_drawingid` (`DrawingId` ASC),
   INDEX `ik_pickthreewinner_userid` (`UserId` ASC)	)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `PickFiveWinner`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PickFiveWinner` ;

CREATE  TABLE IF NOT EXISTS `PickFiveWinner` (
  `WinId` INT   NOT NULL AUTO_INCREMENT ,
  `DrawingId` INT NOT NULL ,  
  `UserId` MEDIUMINT(8) NOT NULL ,
  `Amount` DECIMAL(50,2)  NOT NULL ,       
   PRIMARY KEY (`WinId`),
   INDEX `ik_pickfivewinner_drawingid` (`DrawingId` ASC),
   INDEX `ik_pickfivewinner_userid` (`UserId` ASC)	)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `NextLotteryDrawing`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `NextLotteryDrawing` ;

CREATE  TABLE IF NOT EXISTS `NextLotteryDrawing` (
  `LotteryType` CHAR(1)  NOT NULL , -- T- Three, F- Five
  `LotteryPrice` DECIMAL(5,2) ,
  `NextDrawingDate` DATETIME NOT NULL ,
  `DrawingId` INT NOT NULL ,  
   PRIMARY KEY (`LotteryType`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
