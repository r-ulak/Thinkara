
-- -----------------------------------------------------
-- Table `PoliticalParty`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PoliticalParty` ;

CREATE  TABLE IF NOT EXISTS `PoliticalParty` (
  `PartyId` CHAR(36)  NOT NULL  ,      
  `PartyName` VARCHAR(60) NOT NULL ,  
  `PartyFounder` INT   NOT NULL , 
  `TotalValue` DECIMAL(60,2)   NOT NULL , 
  `StartDate` DATETIME NOT NULL ,  
  `EndDate` DATETIME  NULL , 
  `PartySize` MEDIUMINT(8) NOT NULL , 
  `CoFounderSize` MEDIUMINT(8) NOT NULL  ,  
  `LogoPictureId` VARCHAR(250)  NOT NULL ,
  `MembershipFee` DECIMAL(10,2)  NOT NULL ,
  `ElectionVictory` SMALLINT(5)  NOT NULL ,
  `Motto` VARCHAR(250)   NULL ,
  `Status` CHAR(1) NOT NULL  ,  --  P Pending Approval, A Approved, D Denined , C --Closed , H -- Hold requested closure
  `CountryId` CHAR(2) NOT NULL  ,
  PRIMARY KEY (`PartyId`) ,
  CONSTRAINT uc_partyname UNIQUE (PartyName,CountryId),
  INDEX `index_politicalparty_partyname` (`PartyName` ASC),
    INDEX `index_politicalparty_partySize` (`PartySize` ASC),
  INDEX `index_politicalparty_partyfounder` (`PartyFounder` ASC))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `PartyInvite`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PartyInvite` ;

CREATE  TABLE IF NOT EXISTS `PartyInvite` (
  `TaskId` CHAR(36)  NOT NULL  ,      
  `PartyId` CHAR(36)  NOT NULL  ,      
  `UserId` MEDIUMINT(8)  NULL ,
  `EmailId` VARCHAR(100)   NULL ,
  `MemberType` CHAR(1),  -- C CoFounder, F- Founder, M- Member
  `InvitationDate` DATETIME NULL DEFAULT NULL ,
  `Status` CHAR(1) NULL ,  --  P Pending Approval, A Approved, D Denied, M has another membership, N Not Enough Cash
  PRIMARY KEY (`TaskId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `PartyCloseRequest`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PartyCloseRequest` ;

CREATE  TABLE IF NOT EXISTS `PartyCloseRequest` (
  `TaskId` CHAR(36)  NOT NULL , 
  `PartyId` CHAR(36)  NOT NULL , 
  `UserId` INT   NOT NULL ,       
  `RequestDate` DATETIME(6) NULL DEFAULT NULL ,
  `Status` CHAR(1) NULL ,  --  P Pending Approval, A Approved, D Denied
  PRIMARY KEY (`TaskId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `PartyJoinRequest`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PartyJoinRequest` ;

CREATE  TABLE IF NOT EXISTS `PartyJoinRequest` (
  `TaskId` CHAR(36)  NOT NULL , 
  `PartyId` CHAR(36)  NOT NULL , 
  `UserId` INT   NOT NULL ,       
  `RequestDate` DATETIME(6) NULL DEFAULT NULL ,
  `Status` CHAR(1) NULL ,  --  P Pending Approval, A Approved, D Denied
  PRIMARY KEY (`TaskId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;
-- -----------------------------------------------------
-- Table `WebUserContact`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `WebUserContact` ;

CREATE  TABLE IF NOT EXISTS `WebUserContact` (
  `UserId` INT   NOT NULL ,   
  `InvitationId` CHAR(36)  NULL ,      
  `FriendEmailId` VARCHAR(100) NOT NULL ,    
  `FriendUserId` INT  NULL DEFAULT 0,   
  `NameFirst` VARCHAR(45)  NULL ,  
  `NameLast` VARCHAR(45)   NULL ,
  `PartyInvite` TINYINT(3) DEFAULT 0  ,
  `JoinInvite` TINYINT(3)  DEFAULT 0 ,
  `Unsubscribe` TINYINT(1)  DEFAULT 0 ,
  `LastInviteDate` DATETIME NULL ,
  PRIMARY KEY (`UserId`,FriendEmailId),
  INDEX `index_webusercontact_invitationid` (`InvitationId` ASC)  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PartyNomination`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PartyNomination` ;

CREATE  TABLE IF NOT EXISTS `PartyNomination` (
	`TaskId` CHAR(36)  NOT NULL  ,      
   `PartyId`CHAR(36)  NOT NULL , 
   `InitatorId` INT   NOT NULL ,
   `NomineeId` INT   NOT NULL ,
   `NomineeIdMemberType` CHAR(1),  -- C CoFounder, F- Founder, M- Member
   `NominatingMemberType` CHAR(1),  -- C CoFounder, F- Founder, M- Member
   `RequestDate` DATETIME(6) NULL DEFAULT NULL ,
   `Status` CHAR(1) NULL ,  --  P Pending Approval, A Approved, D Denied
   PRIMARY KEY (`TaskId` ),
   INDEX `index_PartyNomination_partyid` (`PartyId` ASC),
   INDEX `index_partynomination_nomineeid` (`NomineeId` ASC),
   INDEX `index_partynomination_requestdate` (`RequestDate` ASC)
   )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `PartyEjection`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PartyEjection` ;

CREATE  TABLE IF NOT EXISTS `PartyEjection` (
   `TaskId` CHAR(36)  NOT NULL  ,      
   `PartyId`CHAR(36)  NOT NULL , 
   `InitatorId` INT   NOT NULL ,
   `EjecteeId` INT   NOT NULL , 
   `EjecteeMemberType` CHAR(1) NOT NULL ,    
   `RequestDate` DATETIME(6) NULL DEFAULT NULL ,
   `Status` CHAR(1) NULL ,  --  P Pending Approval, A Approved, D Denied
   PRIMARY KEY (`TaskId` ),
   INDEX `index_partyejection_partyid` (`PartyId` ASC),
   INDEX `index_partyejection_ejecteeid` (`EjecteeId` ASC),
   INDEX `index_partyejection_requestdate` (`RequestDate` ASC)
   )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `PartyAgenda`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `PartyAgenda` ;

CREATE  TABLE IF NOT EXISTS `PartyAgenda` (
  `AgendaTypeId`  SMALLINT(3) NOT NULL ,
  `PartyId` CHAR(36) NOT NULL , 
  PRIMARY KEY (`PartyId`, `AgendaTypeId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;



-- -----------------------------------------------------
-- Table `CandidateAgenda`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `CandidateAgenda` ;

CREATE  TABLE IF NOT EXISTS `CandidateAgenda` (
  `ElectionId` MEDIUMINT(8)   NOT NULL ,
  `UserId` INT   NOT NULL ,
  `AgendaTypeId` SMALLINT(3) NOT NULL ,
  PRIMARY KEY (`ElectionId`, `UserId`,`AgendaTypeId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `ElectionAgenda`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ElectionAgenda` ;

CREATE  TABLE IF NOT EXISTS `ElectionAgenda` (
  `AgendaTypeId`  SMALLINT(3) NOT NULL ,
  `AgendaName` VARCHAR(500)  NOT NULL,
  PRIMARY KEY (`AgendaTypeId` ) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `Election`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `Election` ;

CREATE  TABLE IF NOT EXISTS `Election` (
  `ElectionId` MEDIUMINT(8)   NOT NULL  ,
  `CountryId` CHAR(2) NOT NULL ,
  `StartDate` DATETIME NOT  NULL,
  `VotingStartDate` DATETIME NOT  NULL,
  `EndDate` DATETIME NOT NULL ,
  `Fee` Decimal(10,2) NOT NULL ,
  `StartTermNotified` TINYINT(1) DEFAULT 0 ,
  `VotingStartTermNotified` TINYINT(1) DEFAULT 0 ,
  `LastDayTermNotified` TINYINT(1) DEFAULT 0 ,
  PRIMARY KEY (`ElectionId`,`CountryId`)
  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `LeaderCode`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `LeaderCode` ;

CREATE  TABLE IF NOT EXISTS `LeaderCode` (
  `LeaderType` TINYINT(3)  NOT NULL AUTO_INCREMENT ,
  `Code` VARCHAR(25) NOT NULL ,
  PRIMARY KEY (`LeaderType`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `ElectionCandidate`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ElectionCandidate` ;

CREATE  TABLE IF NOT EXISTS `ElectionCandidate` (
   `TaskId` CHAR(36)  NOT NULL  ,   
   `ElectionId` MEDIUMINT(8)   NOT NULL ,
   `UserId` INT   NOT NULL ,
   `PartyId` CHAR(36)   NULL ,
   `CountryId` CHAR(2) NOT NULL ,
   `CandidateTypeId` CHAR(1)  NOT NULL , -- P Party , I Independent
   `PositionTypeId` TINYINT(3)  NOT NULL ,
   `Status` CHAR(1) NOT NULL  ,  --  P Pending Approval, A Approved, D Denined , Q --Quit 
   `LogoPictureId` VARCHAR(250)  NULL ,
   `RequestDate` DATETIME(6) NOT NULL  ,
  PRIMARY KEY (`TaskId` ) )
   
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `ElectionVoting`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ElectionVoting` ;

CREATE  TABLE IF NOT EXISTS `ElectionVoting` (
  `ElectionId` MEDIUMINT(8)   NOT NULL ,  
  `UserId` INT  NOT NULL ,
  `Score` MEDIUMINT  NOT NULL ,
  `CountryId` CHAR(2) NOT NULL ,
  `ElectionResult` CHAR(1) NOT NULL , -- W Won, L- Loss, Q- Quit
  PRIMARY KEY ( `ElectionId` , `UserId`),
  INDEX `ik_electionvoting_countryId` (`CountryId` ASC), 
  INDEX `ik_electionvoting_score` (`Score` ASC)  )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;

-- -----------------------------------------------------
-- Table `ElectionVoter`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `ElectionVoter` ;

CREATE  TABLE IF NOT EXISTS `ElectionVoter` (
  `ElectionId` MEDIUMINT(8)   NOT NULL ,  
  `UserId` INT  NOT NULL ,
  PRIMARY KEY ( `ElectionId` , `UserId`) )
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1
;
