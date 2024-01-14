

-- -----------------------------------------------------
-- Table `StockTrade`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `StockTrade` ;

CREATE  TABLE IF NOT EXISTS `StockTrade` (
  `TradeId` CHAR(36) NOT NULL, 
  `UserStockId` CHAR(36)  NULL,
  `UserId` INT   NOT NULL ,
  `StockId` SMALLINT(5)   NOT NULL ,    
  `LeftUnit` INT   NOT NULL ,  
  `InitialUnit` INT   NOT NULL ,  
  `OfferPrice`  Decimal(50,2) NULL ,
  `RequestedAt` DATETIME NOT NULL ,
  `Status` CHAR(1)  NOT NULL , -- P Pending, C - Cancelled, D- done, I - InProgress, M - No Money
  `OrderType` CHAR(1)  NOT NULL , -- L - Limit Order , M - MarketValue
  `TradeType` CHAR(1)  NOT NULL , -- B - Buy , S - Sell    
  `UpdatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`TradeId`),
  INDEX `ik_stocktrade_stockid` (`StockId` ASC),
  INDEX `ik_stocktrade_userid` (`UserId` ASC),
  INDEX `ik_stocktrade_updatedat` (`UpdatedAt` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `StockTradeHistory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `StockTradeHistory` ;

CREATE  TABLE IF NOT EXISTS `StockTradeHistory` (  
  `StockTradeHistoryId` CHAR(36) NOT NULL,   
  `BuyerId` INT   NOT NULL ,
  `SellerId` INT   NOT NULL ,
  `StockId` SMALLINT(5)   NOT NULL ,    
  `Unit` INT   NOT NULL ,  
  `DealPrice`  Decimal(50,2) NULL ,  
  `UpdatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`StockTradeHistoryId`),
  INDEX `ik_stocktradehist_buyerid` (`BuyerId` ASC),
  INDEX `ik_stocktradehist_updatedat` (`UpdatedAt` DESC),
  INDEX `ik_stocktradehist_sellerid` (`SellerId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `StockHistory`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `StockHistory` ;

CREATE  TABLE IF NOT EXISTS `StockHistory` (
  `HistoryId` CHAR(36) NOT NULL,   
  `StockId` SMALLINT(5)   NOT NULL ,    
  `CurrentValue` DECIMAL(50,2) NOT NULL ,
  `UpdatedAt` DATETIME NOT NULL ,
  PRIMARY KEY (`HistoryId`),
  INDEX `ik_stockhistory_stockid` (`StockId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `UserStock`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserStock` ;

CREATE  TABLE IF NOT EXISTS `UserStock` (
  `UserStockId` CHAR(36)  NOT NULL,
  `UserId` INT  NOT NULL,
  `StockId` SMALLINT(5)   NOT NULL ,    
  `PurchasedUnit` INT   NOT NULL ,  
  `PurchasedPrice`  Decimal(50,2) NOT NULL ,
  `PurchasedAt` DATETIME NOT NULL ,      
  PRIMARY KEY (`UserStockId` ),
    INDEX `ik_userstock_userid` (`UserId` ASC),
	INDEX `ik_userstock_stockid` (`StockId` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;