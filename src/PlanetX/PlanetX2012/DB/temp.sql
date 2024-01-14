-- -----------------------------------------------------
-- Table 'planetx'.`user_group`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS 'planetx'.`user_group` (
  `user_id` MEDIUMINT(8) NOT NULL,
  `description` TEXT NOT NULL ,
  `group_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT,
  `group_sub_type` SMALLINT NULL ,
  `group_type` SMALLINT NULL ,
  `picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `name` VARCHAR(45) NOT NULL ,  
  `url` VARCHAR(255)  NULL,
  `active` BOOLEAN NOT NULL DEFAULT 1 ,  
  `created_at` TIMESTAMP NOT NULL ,
  `updated_at` TIMESTAMP NOT NULL ,
  PRIMARY KEY (`group_id`) ,
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;


-- -----------------------------------------------------
-- Table 'planetx'.`group_membership`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS 'planetx'.`group_membership` (
  `user_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,  
  `group_id` MEDIUMINT(8) NOT NULL ,
  `created_at` TIMESTAMP NOT NULL ,
  `updated_at` TIMESTAMP NOT NULL ,
  PRIMARY KEY (`group_id`) ,
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;
