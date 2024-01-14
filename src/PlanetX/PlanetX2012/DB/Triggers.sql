-- -----------------------------------------------------
-- Trigger WebUserInsert
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `PlanetX`.`TriggerWebUserInsert` ;
DELIMITER $$
CREATE TRIGGER `PlanetX`.`TriggerWebUserInsert` 
AFTER INSERT  ON `PlanetX`.`WebUser` 
FOR EACH ROW BEGIN

 
SET @action = 'I';

INSERT INTO `PlanetX`.`WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameMIddle`,
`NameLast`,
`EmailId`,
`Picture`,
`ActionType`)
VALUES
(NEW.UserId,
NEW.NameFirst,
NEW.NameMIddle,
NEW.NameLast,
NEW.EmailId,
NEW.Picture,
@action
);
END$$

 DELIMITER ;


-- -----------------------------------------------------
-- Trigger WebUserUpdate
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `PlanetX`.`TriggerWebUserUpdate` ;
DELIMITER $$
CREATE TRIGGER `PlanetX`.`TriggerWebUserUpdate` 
AFTER Update  ON `PlanetX`.`WebUser` 
FOR EACH ROW BEGIN

 
SET @action = 'U';

Insert INTO `PlanetX`.`WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameMIddle`,
`NameLast`,
`EmailId`,
`OldNameFirst`,
`OldNameMIddle`,
`OldNameLast`,
`OldEmailId`,
`Picture`,
`ActionType`)
VALUES
(NEW.UserId,
NEW.NameFirst,
NEW.NameMIddle,
NEW.NameLast,
NEW.EmailId,
OLD.NameFirst,
OLD.NameMIddle,
OLD.NameLast,
OLD.EmailId,
NEW.Picture,
@action
);
END$$

 DELIMITER ;


-- -----------------------------------------------------
-- Trigger WebUserDelete
-- -----------------------------------------------------


DROP TRIGGER IF EXISTS `PlanetX`.`TriggerWebUserDelete` ;
DELIMITER $$
CREATE TRIGGER `PlanetX`.`TriggerWebUserDelete` 
AFTER Delete  ON `PlanetX`.`WebUser` 
FOR EACH ROW BEGIN

 
SET @action = 'D';

Insert INTO `PlanetX`.`WebUserUpdate`
(`UserId`,
`NameFirst`,
`NameMIddle`,
`NameLast`,
`EmailId`,
`ActionType`)
VALUES
(OLD.UserId,
OLD.NameFirst,
OLD.NameMIddle,
OLD.NameLast,
OLD.EmailId,
@action
);
END$$

 DELIMITER ;
