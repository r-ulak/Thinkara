
 -- -----------------------------------------------------
-- Table `CrimeReport`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CrimeReport` ;
CREATE TABLE `CrimeReport` (  
  `UserId` INT   NOT NULL,
  `WantedScore` DECIMAL(5,4) DEFAULT 0,
  `ArrestCount` INT NOT NULL,  
  `LootToDate` DECIMAL(50,2),
  `SuspectCount` INT NOT NULL,  
  `IncidentCount` INT NOT NULL,
  `LastArrestDate` TIMESTAMP NULL , 
  `UpdatedAt` TIMESTAMP NOT NULL , 
  PRIMARY KEY (`UserId`))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;

 
 -- -----------------------------------------------------
-- Table `CrimeIncident`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CrimeIncident` ;
CREATE TABLE `CrimeIncident` (  
  `IncidentId` CHAR(36)  NOT NULL,  
  `UserId` INT   NOT NULL,
  `VictimId` INT   NOT NULL,  
  `Amount` DECIMAL(50,2) NOT NULL,  
  `IncidentDate` TIMESTAMP NOT NULL , 
  `MerchandiseTypeId` SMALLINT(4),   
  IncidentType CHAR(1) NOT NULL, -- T Threat, C -Cash , P -Property
  PRIMARY KEY (`IncidentId`))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
 -- -----------------------------------------------------
-- Table `InJail`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `InJail` ;
CREATE TABLE `InJail` (    
  `UserId` INT   NOT NULL,  
  PRIMARY KEY (`UserId`))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;
 
  

 -- -----------------------------------------------------
-- Table `CrimeIncidentSuspect`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CrimeIncidentSuspect` ;
CREATE TABLE `CrimeIncidentSuspect` (  
  `IncidentId` CHAR(36)  NOT NULL,  
  `UserId` INT   NOT NULL,
  `SuspectId` INT   NOT NULL,  
  PRIMARY KEY (`IncidentId`,`UserId`))
 ENGINE=InnoDB DEFAULT CHARSET=latin1;

   
