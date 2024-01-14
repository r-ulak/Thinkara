
-- -----------------------------------------------------
-- Table `Gift`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Gift` ;

CREATE  TABLE IF NOT EXISTS `Gift` (
  `GiftId` CHAR(36) NOT NULL ,
  `FromId` INT NOT NULL ,
  `ToId` INT NOT NULL ,
  `Cash` DECIMAL(50,2)  NULL ,  
  `Gold` DECIMAL(50,2)  NULL ,
  `Silver` DECIMAL(50,2)  NULL ,  
  `TaxAmount` DECIMAL(50,2)  NULL ,  
  `MerchandiseTypeId` SMALLINT(4) NULL, 
  `MerchandiseValue` DECIMAL(50,2) NULL, 
  `CreatedAt` DATETIME NULL DEFAULT NULL ,
  PRIMARY KEY (`GiftId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


