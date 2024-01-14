
-- -----------------------------------------------------
-- Table `UserMerchandise`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `UserMerchandise` ;

CREATE  TABLE IF NOT EXISTS `UserMerchandise` (
  `MerchandiseTypeId` SMALLINT(4)  NOT NULL ,
  `UserId` INT   NOT NULL ,
  `Quantity` SMALLINT(5)  NOT NULL ,
  `MerchandiseCondition` TINYINT(3) UNSIGNED NOT NULL ,
  `PurchasedPrice`  Decimal(50,2) NOT NULL ,
  `Tax`  Decimal(50,2) NOT NULL ,
  `PurchasedAt` DATETIME NOT NULL ,  
   PRIMARY KEY (`MerchandiseTypeId`, `UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
