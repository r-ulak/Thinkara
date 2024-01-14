SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

DROP SCHEMA IF EXISTS `planetx` ;
CREATE SCHEMA IF NOT EXISTS `planetx` DEFAULT CHARACTER SET utf8 ;
USE `planetx` ;

-- -----------------------------------------------------
-- Table `planetx`.`user`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`user` (
  `user_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `token` MEDIUMINT(5) NOT NULL ,
  `username` VARCHAR(45) NOT NULL ,
  `password` VARCHAR(45) NOT NULL ,
  `name_first` VARCHAR(45) NOT NULL ,
  `name_middle` VARCHAR(45) NULL DEFAULT NULL ,
  `name_last` VARCHAR(45) NOT NULL ,
  `email_id` VARCHAR(100) NOT NULL ,
  `picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `active` TINYINT(3) NOT NULL DEFAULT 1 ,
  `online` TINYINT(3) NOT NULL DEFAULT 1 ,
  `created_at` DATETIME NOT NULL ,
  PRIMARY KEY (`user_id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_unicode_ci;


-- -----------------------------------------------------
-- Table `planetx`.`profile`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`profile` (
  `profile_id` BIGINT(8) NOT NULL AUTO_INCREMENT ,
  `user_id` MEDIUMINT(8) NOT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 1 ,
  `rating` TINYINT(3) NULL DEFAULT 1 ,
  `name_nepali` VARCHAR(255) NULL DEFAULT NULL ,
  `dob` TIMESTAMP NULL DEFAULT NULL ,
  `about_me` VARCHAR(160) NULL DEFAULT NULL ,
  `relationship` VARCHAR(45) NULL DEFAULT NULL ,
  `looking_for` VARCHAR(45) NULL DEFAULT NULL ,
  `phone` VARCHAR(45) NULL DEFAULT NULL ,
  `interests` VARCHAR(255) NULL DEFAULT NULL ,
  `education` VARCHAR(255) NULL DEFAULT NULL ,
  `hobbies` VARCHAR(255) NULL DEFAULT NULL ,
  `fav_movies` VARCHAR(255) NULL DEFAULT NULL ,
  `fav_artists` VARCHAR(255) NULL DEFAULT NULL ,
  `fav_books` VARCHAR(255) NULL DEFAULT NULL ,
  `fav_animals` VARCHAR(255) NULL DEFAULT NULL ,
  `religion` TINYINT(3) NULL DEFAULT NULL ,
  `everything_else` VARCHAR(255) NULL DEFAULT NULL ,
  `created_at` DATETIME NULL DEFAULT NULL ,
  PRIMARY KEY (`profile_id`) ,
  CONSTRAINT `fk_profile_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_profile_user` ON `planetx`.`profile` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`nickname`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`nickname` (
  `nickname_id` TINYINT(3) NOT NULL AUTO_INCREMENT ,
  `nickname` VARCHAR(45) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 0 ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`nickname_id`) ,
  CONSTRAINT `fk_nickname_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_nickname_user` ON `planetx`.`nickname` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`lang`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`lang` (
  `language_id` TINYINT(3) NOT NULL AUTO_INCREMENT ,
  `lang` VARCHAR(45) NOT NULL DEFAULT 'en' ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`language_id`) ,
  CONSTRAINT `fk_language_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_language_user` ON `planetx`.`lang` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`country`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`country` (
  `country_id` TINYINT(3)  UNSIGNED NOT NULL AUTO_INCREMENT ,
  `country` VARCHAR(45) NULL DEFAULT 'np' ,
  PRIMARY KEY (`country_id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `planetx`.`city`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`city` (
  `city_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `country_id` TINYINT(3)  UNSIGNED,
  `city` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`city_id`) ,
  CONSTRAINT `fk_city_country`
    FOREIGN KEY (`country_id` )
    REFERENCES `planetx`.`country` (`country_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_city_country` ON `planetx`.`city` (`country_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`address`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`address` (
  `address_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `address` VARCHAR(45) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 0 ,
  `city_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `profile_id` BIGINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`address_id`) ,
  CONSTRAINT `fk_address_city`
    FOREIGN KEY (`city_id` )
    REFERENCES `planetx`.`city` (`city_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_address_profile`
    FOREIGN KEY (`profile_id` )
    REFERENCES `planetx`.`profile` (`profile_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_address_city` ON `planetx`.`address` (`city_id` ASC) ;

CREATE INDEX `fk_address_profile` ON `planetx`.`address` (`profile_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`bookmark_category`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`bookmark_category` (
  `bookmark_category_id` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`bookmark_category_id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `planetx`.`bookmark_sub_category`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`bookmark_sub_category` (
  `bookmark_sub_category_id` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(45) NULL DEFAULT NULL ,
  `bookmark_category_id` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`bookmark_sub_category_id`) ,
  CONSTRAINT `fk_bookmark_sub_category_bookmark_category`
    FOREIGN KEY (`bookmark_category_id` )
    REFERENCES `planetx`.`bookmark_category` (`bookmark_category_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_bookmark_sub_category_bookmark_category` ON `planetx`.`bookmark_sub_category` (`bookmark_category_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`bookmark`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`bookmark` (
  `bookmark_id` BIGINT NOT NULL AUTO_INCREMENT ,
  `url` VARCHAR(255) NULL DEFAULT NULL ,
  `rating` SMALLINT(5) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 2 ,
  `created_at` DATETIME NULL DEFAULT NULL ,
  `bookmark_category_id` SMALLINT(5) NOT NULL ,
  `bookmark_sub_category_id` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`bookmark_id`) ,
  CONSTRAINT `fk_bookmark_bookmark_category`
    FOREIGN KEY (`bookmark_category_id` )
    REFERENCES `planetx`.`bookmark_category` (`bookmark_category_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_bookmark_bookmark_sub_category`
    FOREIGN KEY (`bookmark_sub_category_id` )
    REFERENCES `planetx`.`bookmark_sub_category` (`bookmark_sub_category_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_bookmark_bookmark_category` ON `planetx`.`bookmark` (`bookmark_category_id` ASC) ;

CREATE INDEX `fk_bookmark_bookmark_sub_category` ON `planetx`.`bookmark` (`bookmark_sub_category_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`feed_category`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`feed_category` (
  `feed_category_id` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`feed_category_id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `planetx`.`feed_sub_category`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`feed_sub_category` (
  `feed_sub_category_id` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(45) NULL DEFAULT NULL ,
  `feed_category_id` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`feed_sub_category_id`) ,
  CONSTRAINT `fk_feed_sub_category_feed_category`
    FOREIGN KEY (`feed_category_id` )
    REFERENCES `planetx`.`feed_category` (`feed_category_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_feed_sub_category_feed_category` ON `planetx`.`feed_sub_category` (`feed_category_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`feed`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`feed` (
  `feed_id` BIGINT NOT NULL AUTO_INCREMENT ,
  `feed_url` VARCHAR(255) NULL DEFAULT NULL ,
  `rating` SMALLINT(5) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 2 ,
  `created_at` DATETIME NOT NULL ,
  `feed_category_id` SMALLINT(5) NULL DEFAULT NULL ,
  `feed_sub_category_id` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`feed_id`) ,
  CONSTRAINT `fk_feed_feed_category`
    FOREIGN KEY (`feed_category_id` )
    REFERENCES `planetx`.`feed_category` (`feed_category_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_feed_feed_sub_category`
    FOREIGN KEY (`feed_sub_category_id` )
    REFERENCES `planetx`.`feed_sub_category` (`feed_sub_category_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_feed_feed_category` ON `planetx`.`feed` (`feed_category_id` ASC) ;

CREATE INDEX `fk_feed_feed_sub_category` ON `planetx`.`feed` (`feed_sub_category_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`bookmark_info`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`bookmark_info` (
  `bookmark_info_id` BIGINT NOT NULL AUTO_INCREMENT ,
  `bookmark_id` BIGINT NULL DEFAULT NULL ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `clicks` SMALLINT(5) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 2 ,
  PRIMARY KEY (`bookmark_info_id`) ,
  CONSTRAINT `fk_bookmark_info_bookmark`
    FOREIGN KEY (`bookmark_id` )
    REFERENCES `planetx`.`bookmark` (`bookmark_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_bookmark_info_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_bookmark_info_bookmark` ON `planetx`.`bookmark_info` (`bookmark_id` ASC) ;

CREATE INDEX `fk_bookmark_info_user` ON `planetx`.`bookmark_info` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`feed_info`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`feed_info` (
  `feed_info_id` BIGINT NOT NULL AUTO_INCREMENT ,
  `feed_id` BIGINT NULL DEFAULT NULL ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `favorite` TINYINT(3) NOT NULL DEFAULT 0 ,
  `clicks` SMALLINT(5) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 2 ,
  PRIMARY KEY (`feed_info_id`) ,
  CONSTRAINT `fk_feed_info_feed`
    FOREIGN KEY (`feed_id` )
    REFERENCES `planetx`.`feed` (`feed_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_feed_info_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_feed_info_feed` ON `planetx`.`feed_info` (`feed_id` ASC) ;

CREATE INDEX `fk_feed_info_user` ON `planetx`.`feed_info` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`friend`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`friend` (
  `friend_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `friend_user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `is_subscriber` TINYINT(1) NOT NULL DEFAULT 1 ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 0 ,
  `created_at` DATETIME NOT NULL ,
  `user_id` MEDIUMINT(8) NOT NULL ,
  `friend_list_id` SMALLINT(5) NULL DEFAULT NULL ,
  PRIMARY KEY (`friend_id`) ,
  CONSTRAINT `fk_friend_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_friend_friend_list`
    FOREIGN KEY (`friend_list_id` )
    REFERENCES `planetx`.`friend_list` (`friend_list_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_friend_user` ON `planetx`.`friend` (`user_id` ASC) ;

CREATE INDEX `fk_friend_friend_list` ON `planetx`.`friend` (`friend_list_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`friend_list`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`friend_list` (
  `friend_list_id` SMALLINT(5) NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(45) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 0 ,
  `friend_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`friend_list_id`) ,
  CONSTRAINT `fk_friend_list_friend`
    FOREIGN KEY (`friend_id` )
    REFERENCES `planetx`.`friend` (`friend_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_friend_list_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_friend_list_friend` ON `planetx`.`friend_list` (`friend_id` ASC) ;

CREATE INDEX `fk_friend_list_user` ON `planetx`.`friend_list` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`status`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`status` (
  `status_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `message` VARCHAR(255) NOT NULL ,
  `created_at` DATETIME NOT NULL ,
  `thumbs_up` SMALLINT(5) NULL DEFAULT NULL ,
  `thumbs_down` SMALLINT(5) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 0 ,
  `is_reply` TINYINT(1) NOT NULL DEFAULT 0 ,
  `to_fb` TINYINT(1) NOT NULL DEFAULT 0 ,
  `to_twitter` TINYINT(1) NOT NULL DEFAULT 0 ,
  `user_id` MEDIUMINT(8) NOT NULL ,
  PRIMARY KEY (`status_id`) ,
  CONSTRAINT `fk_status_reply_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_status_reply_user` ON `planetx`.`status` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`message`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`message` (
  `message_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `message` VARCHAR(500) NOT NULL ,
  `created_at` DATETIME NOT NULL ,
  `is_read` TINYINT(1) NOT NULL DEFAULT 0 ,
  `is_spam` TINYINT(1) NOT NULL DEFAULT 0 ,
  `to` MEDIUMINT(8) NULL DEFAULT NULL ,
  `isreply` TINYINT(1) NULL DEFAULT 0 ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`message_id`) ,
  CONSTRAINT `fk_message_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_message_user` ON `planetx`.`message` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`thumb_up_down`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`thumb_up_down` (
  `thumb_up_down_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `flag` TINYINT(1) NULL DEFAULT 1 ,
  `created_at` DATETIME NULL DEFAULT NULL ,
  `status_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `friend_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`thumb_up_down_id`) ,
  CONSTRAINT `fk_thumb_up_down_status`
    FOREIGN KEY (`status_id` )
    REFERENCES `planetx`.`status` (`status_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_thumb_up_down_friend`
    FOREIGN KEY (`friend_id` )
    REFERENCES `planetx`.`friend` (`friend_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_thumb_up_down_status` ON `planetx`.`thumb_up_down` (`status_id` ASC) ;

CREATE INDEX `fk_thumb_up_down_friend` ON `planetx`.`thumb_up_down` (`friend_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`notification`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`notification` (
  `notification_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `msg` VARCHAR(255) NULL DEFAULT NULL ,
  `type` SMALLINT(5) NULL DEFAULT NULL ,
  `privacy` TINYINT(3) NOT NULL DEFAULT 0 ,
  `created_at` DATETIME NULL DEFAULT NULL ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`notification_id`) ,
  CONSTRAINT `fk_activity_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_activity_user` ON `planetx`.`notification` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`chat`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`chat` (
  `chat_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `to` MEDIUMINT(8) NOT NULL ,
  `msg` VARCHAR(100) NOT NULL ,
  `created_at` DATETIME NULL DEFAULT NULL ,
  PRIMARY KEY (`chat_id`) ,
  CONSTRAINT `fk_chat_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_chat_user` ON `planetx`.`chat` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`privacy`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`privacy` (
  `privacy_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `profile` TINYINT(3) NOT NULL DEFAULT 1 ,
  `address` TINYINT(3) NOT NULL DEFAULT 2 ,
  `status` TINYINT(3) NOT NULL DEFAULT 1 ,
  `bookmark` TINYINT(3) NOT NULL DEFAULT 1 ,
  `feed` TINYINT(3) NOT NULL DEFAULT 1 ,
  `activity` TINYINT(3) NOT NULL DEFAULT 1 ,
  `friend` TINYINT(3) NOT NULL DEFAULT 1 ,
  `friend_list` TINYINT(3) NOT NULL DEFAULT 0 ,
  `nickname` TINYINT(3) NOT NULL DEFAULT 1 ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`privacy_id`) ,
  CONSTRAINT `fk_privacy_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_privacy_user` ON `planetx`.`privacy` (`user_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`blog`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`blog` (
  `blog_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `message` VARCHAR(45) NOT NULL ,
  `author` VARCHAR(45) NULL DEFAULT NULL ,
  `user_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `created_at` VARCHAR(45) NOT NULL ,
  PRIMARY KEY (`blog_id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `planetx`.`comment`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`comment` (
  `comment_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `message` VARCHAR(255) NULL DEFAULT NULL ,
  `created_at` DATETIME NULL DEFAULT NULL ,
  `status_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  `friend_id` MEDIUMINT(8) NULL DEFAULT NULL ,
  PRIMARY KEY (`comment_id`) ,
  CONSTRAINT `fk_comment_status`
    FOREIGN KEY (`status_id` )
    REFERENCES `planetx`.`status` (`status_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_comment_friend`
    FOREIGN KEY (`friend_id` )
    REFERENCES `planetx`.`friend` (`friend_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;

CREATE INDEX `fk_comment_status` ON `planetx`.`comment` (`status_id` ASC) ;

CREATE INDEX `fk_comment_friend` ON `planetx`.`comment` (`friend_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`privacy_type`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`privacy_type` (
  `privacy_type_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `name` VARCHAR(45) NULL DEFAULT NULL ,
  PRIMARY KEY (`privacy_type_id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;




-- -----------------------------------------------------
-- Table `planetx`.`country_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`country_code` (    
  `country_id` TINYINT(3) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `code` VARCHAR(25) NOT NULL,    
  PRIMARY KEY (`country_id`))   
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`province_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`province_code` (    
  `province_id` SMALLINT(5) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `province` VARCHAR(25) NOT NULL,    
  PRIMARY KEY (`province_id`) )  
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`city_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`city_code` (    
  `city_id` SMALLINT(5) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `city` VARCHAR(25) NOT NULL,    
  PRIMARY KEY (`city_id`))   
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;
-- -----------------------------------------------------
-- Table `planetx`.`location`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`location` (    
  `location_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `city_id` SMALLINT(5) UNSIGNED NOT NULL, 
  `province_id` SMALLINT(5) UNSIGNED NOT NULL , 
  `country_id` TINYINT(3) UNSIGNED NOT NULL,      
  PRIMARY KEY (`location_id`) ,  
  CONSTRAINT `fk_location_city_code`
    FOREIGN KEY (`city_id` )
    REFERENCES `planetx`.`city_code` (`city_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,  
  CONSTRAINT `fk_location_province_code`
    FOREIGN KEY (`province_id` )
    REFERENCES `planetx`.`province_code` (`province_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,
  CONSTRAINT `fk_location_country_code`
    FOREIGN KEY (`country_id` )
    REFERENCES `planetx`.`country_code` (`country_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION) 	
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;
CREATE INDEX `fk_location_city_code` ON `planetx`.`location` (`city_id` ASC) ;
CREATE INDEX `fk_location_province_code` ON `planetx`.`location` (`province_id` ASC) ;
CREATE INDEX `fk_location_country_code` ON `planetx`.`location` (`country_id` ASC) ;


-- -----------------------------------------------------
-- Table `planetx`.`event`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`event` (
  `event_id` MEDIUMINT(3) NOT NULL AUTO_INCREMENT ,
  `event_name` VARCHAR(45) NULL ,  
  `user_id` MEDIUMINT(8) NULL ,
  `description` TEXT,
  `event_type`	TINYINT(3) NULL,
  `picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `location_id` MEDIUMINT(3) NULL,  
  `endtime` DATETIME NULL ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`event_id`) ,
  CONSTRAINT `fk_event_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,
  CONSTRAINT `fk_event_location`
    FOREIGN KEY (`location_id` )
    REFERENCES `planetx`.`location` (`location_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_event_user` ON `planetx`.`event` (`user_id` ASC) ;
CREATE INDEX `fk_event_location` ON `planetx`.`event` (`location_id` ASC) ;

-- -----------------------------------------------------
-- Table `planetx`.`rsvp_status_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`rsvp_status_code` (
  `status_type` TINYINT(2) UNSIGNED NOT NULL AUTO_INCREMENT,
  `code` VARCHAR(15) NOT NULL,    
  PRIMARY KEY (`status_type` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`event_membership`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`event_membership` (
  `user_id` MEDIUMINT(8) NOT NULL ,
  `event_id` MEDIUMINT(3) NOT NULL,  
  `status_type` TINYINT(2) UNSIGNED ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,  
  PRIMARY KEY (`user_id`,`event_id` ) ,
  CONSTRAINT `fk_event_membership_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,
  CONSTRAINT `fk_event_membership_event`
    FOREIGN KEY (`event_id` )
    REFERENCES `planetx`.`event` (`event_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,	
  CONSTRAINT `fk_event_membership_rsvp_status_code`
    FOREIGN KEY (`status_type` )
    REFERENCES `planetx`.`rsvp_status_code` (`status_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)		
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_event_membership_user` ON `planetx`.`event_membership` (`user_id` ASC) ;
CREATE INDEX `fk_event_membership_event` ON `planetx`.`event_membership` (`event_id` ASC) ;
CREATE INDEX `fk_event_membership_rsvp_status_code` ON `planetx`.`event_membership` (`status_type` ASC) ;

-- -----------------------------------------------------
-- Table `planetx`.`card`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`card` (
  `card_id` MEDIUMINT(3) NOT NULL AUTO_INCREMENT ,
  `user_id` MEDIUMINT(8) NULL ,
  `card_type`	TINYINT(3) NULL,
  `amount` DECIMAL(10,2) NULL ,
  `expire_date` DATETIME NOT NULL ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,  
  PRIMARY KEY (`card_id`) ,
  CONSTRAINT `fk_card_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_event_user` ON `planetx`.`card` (`user_id` ASC) ;
-- -----------------------------------------------------
-- Table `planetx`.`business_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`business_code` (    
  `business_type` TINYINT(3) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `code` VARCHAR(25) NOT NULL ,   
  PRIMARY KEY (`business_type`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`business`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`business` (  
  `business_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `business_name`VARCHAR(25) NOT NULL ,   
  `business_type` TINYINT(3) UNSIGNED NOT NULL , 
  `net_profit` DECIMAL(10,2) NOT NULL ,
  `running_cost` DECIMAL(10,2) NOT NULL ,
  `location_id` MEDIUMINT(8) NULL,      
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  PRIMARY KEY (`business_id`) ,  
  CONSTRAINT `fk_business_businesstype`
    FOREIGN KEY (`business_type` )
    REFERENCES `planetx`.`business_code` (`business_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,
  CONSTRAINT `fk_business_location`
    FOREIGN KEY (`location_id` )
    REFERENCES `planetx`.`location` (`location_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
	
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_business_businesstype` ON `planetx`.`business` (`business_type` ASC) ;
CREATE INDEX `fk_business_location` ON `planetx`.`business` (`location_id` ASC) ;

-- -----------------------------------------------------
-- Table `planetx`.`loan_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`loan_code` (    
  `loan_type` TINYINT(3) NOT NULL AUTO_INCREMENT, 
  `code` VARCHAR(25) NOT NULL ,   
  PRIMARY KEY (`loan_type`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;
-- -----------------------------------------------------
-- Table `planetx`.`loan_from_business`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`loan_from_business` (
  `loan_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `user_id` MEDIUMINT(8) NOT NULL ,
  `business_id` MEDIUMINT(8) NOT NULL ,
  `loan_type` TINYINT(3) NOT NULL ,
  `loan_amount` DECIMAL(10,2) NULL ,  
  `monthly_interest_rate` DECIMAL(5,2) NULL ,    
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  PRIMARY KEY (`loan_id`) ,
  CONSTRAINT `fk_loan_from_business_business`
    FOREIGN KEY (`business_id` )
    REFERENCES `planetx`.`business` (`business_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,	
  CONSTRAINT `fk_loan_from_business_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,  	
  CONSTRAINT `fk_loan_from_business_loantype`
    FOREIGN KEY (`loan_type` )
    REFERENCES `planetx`.`loan_code` (`loan_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
ENGINE = InnoDB

DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_loan_from_business_business` ON `planetx`.`loan_from_business` (`business_id` ASC) ;
CREATE INDEX `fk_loan_from_business_user` ON `planetx`.`loan_from_business` (`user_id` ASC) ;
CREATE INDEX `fk_loan_from_business_loantype` ON `planetx`.`loan_from_business` (`loan_type` ASC) ;

-- -----------------------------------------------------
-- Table `planetx`.`loan_from_person`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`loan_from_person` (
  `loan_id` MEDIUMINT(8) NOT NULL AUTO_INCREMENT ,
  `user_id` MEDIUMINT(8) NOT NULL ,
  `source_id` MEDIUMINT(8) NOT NULL ,
  `loan_type` TINYINT(3) NOT NULL ,
  `loan_amount` DECIMAL(10,2) NULL ,  
  `monthly_interest_rate` DECIMAL(5,2) NULL ,    
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  PRIMARY KEY (`loan_id`) ,
  CONSTRAINT `fk_loan_from_person_user_1`
    FOREIGN KEY (`source_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,  
  CONSTRAINT `fk_loan_from_person_user_2`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,	
  CONSTRAINT `fk_loan_from_person_loantype`
    FOREIGN KEY (`loan_type` )
    REFERENCES `planetx`.`loan_code` (`loan_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)		
ENGINE = InnoDB

DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_loan_from_person_user_1` ON `planetx`.`loan_from_person` (`source_id` ASC) ;
CREATE INDEX `fk_loan_from_person_user_2` ON `planetx`.`loan_from_person` (`user_id` ASC) ;
CREATE INDEX `fk_loan_from_person_loantype` ON `planetx`.`loan_from_person` (`loan_type` ASC) ;

-- -----------------------------------------------------
-- Table `planetx`.`leader_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`leader_code` (    
  `leader_type` TINYINT(3) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `code` VARCHAR(25) NOT NULL,    
  PRIMARY KEY (`leader_type`) )  
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`goverment`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`goverment` (
  `country_id` TINYINT(3) UNSIGNED NOT NULL ,
  `leader_id` MEDIUMINT(8) NOT NULL ,
  `leader_type` TINYINT(3) UNSIGNED NOT NULL ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  PRIMARY KEY (`country_id`, `leader_id`, `leader_type`) ,
  CONSTRAINT `fk_goverment_user`
    FOREIGN KEY (`leader_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,  
  CONSTRAINT `fk_goverment_leader_code`
    FOREIGN KEY (`leader_type` )
    REFERENCES `planetx`.`leader_code` (`leader_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,	
  CONSTRAINT `fk_goverment_country_code`
    FOREIGN KEY (`country_id` )
    REFERENCES `planetx`.`country_code` (`country_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
ENGINE = InnoDB

DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_goverment_user` ON `planetx`.`goverment` (`leader_id` ASC) ;
CREATE INDEX `fk_goverment_leader_code` ON `planetx`.`goverment` (`leader_type` ASC) ;
CREATE INDEX `fk_goverment_country_code` ON `planetx`.`goverment` (`country_id` ASC) ;
-- -----------------------------------------------------
-- Table `planetx`.`Military`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`Military_force` (
  `country_id` TINYINT(3) UNSIGNED NOT NULL ,
  `ground` SMALLINT(5) NOT NULL ,
  `air` SMALLINT(5) NOT NULL ,
  `navy` SMALLINT(5) NOT NULL ,
  `nuclear` SMALLINT(5) NOT NULL ,
  `special` SMALLINT(5) NOT NULL ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`country_id`) ,
  CONSTRAINT `fk_Military_force_country_code`
    FOREIGN KEY (`country_id` )
    REFERENCES `planetx`.`country_code` (`country_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
ENGINE = InnoDB

DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`goverment_local`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`goverment_province` (
  `country_id` TINYINT(3) UNSIGNED NOT NULL ,
  `ground` SMALLINT(5) NOT NULL ,
  `air` SMALLINT(5) NOT NULL ,
  `navy` SMALLINT(5) NOT NULL ,
  `nuclear` SMALLINT(5) NOT NULL ,
  `special` SMALLINT(5) NOT NULL ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`country_id`) ,
  CONSTRAINT `fk_goverment_province_country_code`
    FOREIGN KEY (`country_id` )
    REFERENCES `planetx`.`country_code` (`country_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
ENGINE = InnoDB

DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`employment`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`employment` (  
  `business_id` MEDIUMINT(8) NOT NULL ,
  `user_id` MEDIUMINT(8) NULL ,
  `salary` DECIMAL (10,2) NOT NULL ,
  `job_title` VARCHAR(25) NOT NULL DEFAULT 'employee' ,  
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`business_id`,`user_id`) ,
  CONSTRAINT `fk_employment_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,
  CONSTRAINT `fk_employment_business`
    FOREIGN KEY (`business_id` )
    REFERENCES `planetx`.`business` (`business_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_employment_user` ON `planetx`.`employment` (`user_id` ASC) ;
CREATE INDEX `fk_employment_business` ON `planetx`.`employment` (`business_id` ASC) ;

-- -----------------------------------------------------
-- Table `planetx`.`item_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`item_code` (    
  `item_type` SMALLINT(5) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `item` VARCHAR(25) NOT NULL ,    
  PRIMARY KEY (`item_type`) )  
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`merchandise`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`merchandise` (  
  `item_id` MEDIUMINT(8) NOT NULL,
  `item_type` SMALLINT(5) UNSIGNED  NULL ,
  `cost` DECIMAL (10,2) NOT NULL DEFAULT 0,
  `picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`item_id`) ,
  CONSTRAINT `fk_merchandise_item_code`
    FOREIGN KEY (`item_type` )
    REFERENCES `planetx`.`item_code` (`item_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_merchandise_item_code` ON `planetx`.`merchandise` (`item_type` ASC) ;

-- -----------------------------------------------------
-- Table `planetx`.`avatar`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`avatar` (  
  `avatar_id` MEDIUMINT(8) NOT NULL,
  `picture` VARCHAR(255) NOT NULL DEFAULT '/web/image/default.jpg' ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`avatar_id`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`degree_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`degree_code` (    
  `degree_type` TINYINT(3) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `degree` VARCHAR(25) NOT NULL ,   
  PRIMARY KEY (`degree_type`) )  
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`major_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`major_code` (    
  `major_type` SMALLINT(5) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `major` VARCHAR(25) NOT NULL ,   
  PRIMARY KEY (`major_type`) )  
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`university_code`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`university_code` (    
  `university_id` SMALLINT(5) UNSIGNED NOT NULL AUTO_INCREMENT, 
  `location_id` MEDIUMINT(8) NOT NULL, 
  `name` VARCHAR(25) NOT NULL ,    
  PRIMARY KEY (`university_id`) , 
  CONSTRAINT `fk_university_code_location`
    FOREIGN KEY (`location_id` )
    REFERENCES `planetx`.`location` (`location_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
  
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

-- -----------------------------------------------------
-- Table `planetx`.`education`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`education` (  
  `user_id` MEDIUMINT(8) NOT NULL ,
  `degree_type` TINYINT(3) UNSIGNED NULL ,
  `major_type` SMALLINT(5) UNSIGNED NULL ,  
  `university_id` SMALLINT(5) UNSIGNED,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`user_id`,`degree_type`,`major_type`) ,
  CONSTRAINT `fk_education_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,
  CONSTRAINT `fk_education_degree_code`
    FOREIGN KEY (`degree_type` )
    REFERENCES `planetx`.`degree_code` (`degree_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,	
  CONSTRAINT `fk_education_major_code`
    FOREIGN KEY (`major_type` )
    REFERENCES `planetx`.`major_code` (`major_type` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION ,	
  CONSTRAINT `fk_education_university_code`
    FOREIGN KEY (`university_id` )
    REFERENCES `planetx`.`university_code` (`university_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)	
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_education_user` ON `planetx`.`education` (`user_id` ASC) ;
CREATE INDEX `fk_education_degree_code` ON `planetx`.`education` (`degree_type` ASC) ;
CREATE INDEX `fk_education_major_code` ON `planetx`.`education` (`major_type` ASC) ;
CREATE INDEX `fk_education_university_code` ON `planetx`.`education` (`university_id` ASC) ;
-- -----------------------------------------------------
-- Table `planetx`.`asset`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `planetx`.`asset` (  
  `item_id` MEDIUMINT(8) NOT NULL ,
  `user_id` MEDIUMINT(8) NULL ,
  `qty` SMALLINT(5) NULL ,
  `created_at` DATETIME ,
  `updated_at` TIMESTAMP ,
  
  PRIMARY KEY (`item_id`) ,
  CONSTRAINT `fk_asset_user`
    FOREIGN KEY (`user_id` )
    REFERENCES `planetx`.`user` (`user_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_asset_merchandise`
    FOREIGN KEY (`item_id` )
    REFERENCES `planetx`.`merchandise` (`item_id` )
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_general_ci;

CREATE INDEX `fk_asset_merchandise` ON `planetx`.`merchandise` (`item_id` ASC) ;

RENAME TABLE asset TO Asset
RENAME TABLE avatar TO Avatar
RENAME TABLE blog TO Blog
RENAME TABLE bookmark TO Bookmark
RENAME TABLE bookmark_category TO BookmarkCategory
RENAME TABLE bookmark_info TO BookmarkInfo
RENAME TABLE bookmark_sub_category TO BookmarkSubCategory
RENAME TABLE business TO Business
RENAME TABLE business_code TO BusinessCode
RENAME TABLE Card TO Card
RENAME TABLE chat TO Chat
RENAME TABLE city_code TO CityCode
RENAME TABLE comment TO Comment
RENAME TABLE country_code TO CountryCode
RENAME TABLE degree_code TO DegreeCode
RENAME TABLE education TO Education
RENAME TABLE employment TO Employment
RENAME TABLE event TO Event
RENAME TABLE event_code TO EventCode
RENAME TABLE event_membership TO EventMembership
RENAME TABLE feed TO Feed
RENAME TABLE feed_category TO FeedCategory
RENAME TABLE feed_info TO FeedInfo
RENAME TABLE feed_sub_category TO FeedSubCategory
RENAME TABLE friend TO Friend
RENAME TABLE goverment TO Goverment
RENAME TABLE goverment_province TO GovermentProvince
RENAME TABLE item_code TO ItemCode
RENAME TABLE lang TO Lang
RENAME TABLE leader_code TO LeaderCode
RENAME TABLE loan_code TO LoanCode
RENAME TABLE loan_from_business TO LoanFromBusiness
RENAME TABLE loan_from_person TO LoanFromPerson
RENAME TABLE location TO Location
RENAME TABLE major_code TO MajorCode
RENAME TABLE merchandise TO Merchandise
RENAME TABLE message TO Message
RENAME TABLE Military_force TO MilitaryForce
RENAME TABLE notification TO Notification
RENAME TABLE privacy TO Privacy
RENAME TABLE privacy_type TO Privacy_type
RENAME TABLE profile TO Profile
RENAME TABLE province_code TO ProvinceCode
RENAME TABLE rsvp_status_code TO RsvpStatusCode
RENAME TABLE status TO Status
RENAME TABLE thumb_up_down TO ThumbUpDown
RENAME TABLE university_code TO UniversityCode
RENAME Table User TO UserAcoount;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;