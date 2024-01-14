-- -----------------------------------------------------
-- procedure AdsFrequencyTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsFrequencyTypeAddUpdate` $$
CREATE PROCEDURE `AdsFrequencyTypeAddUpdate` (
	 IN parmAdsFrequencyTypeId tinyint(3),
	 IN parmFrequencyName varchar(250),
	 IN parmFrequencyMultiple tinyint(3),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO AdsFrequencyType( AdsFrequencyTypeId, FrequencyName, FrequencyMultiple)
		VALUES (parmAdsFrequencyTypeId,parmFrequencyName,parmFrequencyMultiple)


	ON DUPLICATE KEY UPDATE
				FrequencyName = parmFrequencyName,
				FrequencyMultiple = parmFrequencyMultiple
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdsFrequencyTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsFrequencyTypeUpdate` $$
CREATE PROCEDURE `AdsFrequencyTypeUpdate` (
	 IN parmAdsFrequencyTypeId tinyint(3),
	 IN parmFrequencyName varchar(250),
	 IN parmFrequencyMultiple tinyint(3)
)

	BEGIN 

	UPDATE AdsFrequencyType SET 
				FrequencyName = parmFrequencyName,
				FrequencyMultiple = parmFrequencyMultiple
		WHERE
			AdsFrequencyTypeId = parmAdsFrequencyTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdsFrequencyTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsFrequencyTypeAdd` $$
CREATE PROCEDURE `AdsFrequencyTypeAdd` (
	 IN parmAdsFrequencyTypeId tinyint(3),
	 IN parmFrequencyName varchar(250),
	 IN parmFrequencyMultiple tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO AdsFrequencyType( AdsFrequencyTypeId, FrequencyName, FrequencyMultiple)
		VALUES (parmAdsFrequencyTypeId,parmFrequencyName,parmFrequencyMultiple);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdsFrequencyTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsFrequencyTypeSelect` $$
CREATE PROCEDURE `AdsFrequencyTypeSelect` (
	 IN parmAdsFrequencyTypeId tinyint(3)
)

	BEGIN 

		SELECT  AdsFrequencyTypeId, FrequencyName, FrequencyMultiple FROM AdsFrequencyType WHERE
			AdsFrequencyTypeId = parmAdsFrequencyTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdsTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsTypeAddUpdate` $$
CREATE PROCEDURE `AdsTypeAddUpdate` (
	 IN parmAdsTypeId tinyint(3),
	 IN parmAdName varchar(250),
	 IN parmBaseCost decimal(8,2),
	 IN parmPricePerChar decimal(8,2),
	 IN parmPricePerImageByte decimal(10,6),
	 IN parmFontCss varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO AdsType( AdsTypeId, AdName, BaseCost, PricePerChar, PricePerImageByte, FontCss)
		VALUES (parmAdsTypeId,parmAdName,parmBaseCost,parmPricePerChar,parmPricePerImageByte,parmFontCss)


	ON DUPLICATE KEY UPDATE
				AdName = parmAdName,
				BaseCost = parmBaseCost,
				PricePerChar = parmPricePerChar,
				PricePerImageByte = parmPricePerImageByte,
				FontCss = parmFontCss
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdsTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsTypeUpdate` $$
CREATE PROCEDURE `AdsTypeUpdate` (
	 IN parmAdsTypeId tinyint(3),
	 IN parmAdName varchar(250),
	 IN parmBaseCost decimal(8,2),
	 IN parmPricePerChar decimal(8,2),
	 IN parmPricePerImageByte decimal(10,6),
	 IN parmFontCss varchar(50)
)

	BEGIN 

	UPDATE AdsType SET 
				AdName = parmAdName,
				BaseCost = parmBaseCost,
				PricePerChar = parmPricePerChar,
				PricePerImageByte = parmPricePerImageByte,
				FontCss = parmFontCss
		WHERE
			AdsTypeId = parmAdsTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdsTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsTypeAdd` $$
CREATE PROCEDURE `AdsTypeAdd` (
	 IN parmAdsTypeId tinyint(3),
	 IN parmAdName varchar(250),
	 IN parmBaseCost decimal(8,2),
	 IN parmPricePerChar decimal(8,2),
	 IN parmPricePerImageByte decimal(10,6),
	 IN parmFontCss varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO AdsType( AdsTypeId, AdName, BaseCost, PricePerChar, PricePerImageByte, FontCss)
		VALUES (parmAdsTypeId,parmAdName,parmBaseCost,parmPricePerChar,parmPricePerImageByte,parmFontCss);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdsTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdsTypeSelect` $$
CREATE PROCEDURE `AdsTypeSelect` (
	 IN parmAdsTypeId tinyint(3)
)

	BEGIN 

		SELECT  AdsTypeId, AdName, BaseCost, PricePerChar, PricePerImageByte, FontCss FROM AdsType WHERE
			AdsTypeId = parmAdsTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdvertisementAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdvertisementAddUpdate` $$
CREATE PROCEDURE `AdvertisementAddUpdate` (
	 IN parmAdvertisementId char(36),
	 IN parmUserId int(11),
	 IN parmAdsTypeEmail tinyint(1),
	 IN parmAdsTypeFeed tinyint(1),
	 IN parmAdsTypePartyMember tinyint(1),
	 IN parmAdsTypeCountryMember tinyint(1),
	 IN parmAdsFrequencyTypeId tinyint(3),
	 IN parmDaysS tinyint(1),
	 IN parmDaysM tinyint(1),
	 IN parmDaysT tinyint(1),
	 IN parmDaysW tinyint(1),
	 IN parmDaysTh tinyint(1),
	 IN parmDaysF tinyint(1),
	 IN parmDaysSa tinyint(1),
	 IN parmAdTime smallint(4),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmPreviewMsg text,
	 IN parmMessage varchar(1000),
	 IN parmCost decimal(10,2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO Advertisement( AdvertisementId, UserId, AdsTypeEmail, AdsTypeFeed, AdsTypePartyMember, AdsTypeCountryMember, AdsFrequencyTypeId, DaysS, DaysM, DaysT, DaysW, DaysTh, DaysF, DaysSa, AdTime, StartDate, EndDate, PreviewMsg, Message, Cost)
		VALUES (parmAdvertisementId,parmUserId,parmAdsTypeEmail,parmAdsTypeFeed,parmAdsTypePartyMember,parmAdsTypeCountryMember,parmAdsFrequencyTypeId,parmDaysS,parmDaysM,parmDaysT,parmDaysW,parmDaysTh,parmDaysF,parmDaysSa,parmAdTime,parmStartDate,parmEndDate,parmPreviewMsg,parmMessage,parmCost)


	ON DUPLICATE KEY UPDATE
				AdsTypeEmail = parmAdsTypeEmail,
				AdsTypeFeed = parmAdsTypeFeed,
				AdsTypePartyMember = parmAdsTypePartyMember,
				AdsTypeCountryMember = parmAdsTypeCountryMember,
				AdsFrequencyTypeId = parmAdsFrequencyTypeId,
				DaysS = parmDaysS,
				DaysM = parmDaysM,
				DaysT = parmDaysT,
				DaysW = parmDaysW,
				DaysTh = parmDaysTh,
				DaysF = parmDaysF,
				DaysSa = parmDaysSa,
				AdTime = parmAdTime,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				PreviewMsg = parmPreviewMsg,
				Message = parmMessage,
				Cost = parmCost
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdvertisementUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdvertisementUpdate` $$
CREATE PROCEDURE `AdvertisementUpdate` (
	 IN parmAdvertisementId char(36),
	 IN parmUserId int(11),
	 IN parmAdsTypeEmail tinyint(1),
	 IN parmAdsTypeFeed tinyint(1),
	 IN parmAdsTypePartyMember tinyint(1),
	 IN parmAdsTypeCountryMember tinyint(1),
	 IN parmAdsFrequencyTypeId tinyint(3),
	 IN parmDaysS tinyint(1),
	 IN parmDaysM tinyint(1),
	 IN parmDaysT tinyint(1),
	 IN parmDaysW tinyint(1),
	 IN parmDaysTh tinyint(1),
	 IN parmDaysF tinyint(1),
	 IN parmDaysSa tinyint(1),
	 IN parmAdTime smallint(4),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmPreviewMsg text,
	 IN parmMessage varchar(1000),
	 IN parmCost decimal(10,2)
)

	BEGIN 

	UPDATE Advertisement SET 
				AdsTypeEmail = parmAdsTypeEmail,
				AdsTypeFeed = parmAdsTypeFeed,
				AdsTypePartyMember = parmAdsTypePartyMember,
				AdsTypeCountryMember = parmAdsTypeCountryMember,
				AdsFrequencyTypeId = parmAdsFrequencyTypeId,
				DaysS = parmDaysS,
				DaysM = parmDaysM,
				DaysT = parmDaysT,
				DaysW = parmDaysW,
				DaysTh = parmDaysTh,
				DaysF = parmDaysF,
				DaysSa = parmDaysSa,
				AdTime = parmAdTime,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				PreviewMsg = parmPreviewMsg,
				Message = parmMessage,
				Cost = parmCost
		WHERE
			AdvertisementId = parmAdvertisementId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdvertisementAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdvertisementAdd` $$
CREATE PROCEDURE `AdvertisementAdd` (
	 IN parmAdvertisementId char(36),
	 IN parmUserId int(11),
	 IN parmAdsTypeEmail tinyint(1),
	 IN parmAdsTypeFeed tinyint(1),
	 IN parmAdsTypePartyMember tinyint(1),
	 IN parmAdsTypeCountryMember tinyint(1),
	 IN parmAdsFrequencyTypeId tinyint(3),
	 IN parmDaysS tinyint(1),
	 IN parmDaysM tinyint(1),
	 IN parmDaysT tinyint(1),
	 IN parmDaysW tinyint(1),
	 IN parmDaysTh tinyint(1),
	 IN parmDaysF tinyint(1),
	 IN parmDaysSa tinyint(1),
	 IN parmAdTime smallint(4),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmPreviewMsg text,
	 IN parmMessage varchar(1000),
	 IN parmCost decimal(10,2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO Advertisement( AdvertisementId, UserId, AdsTypeEmail, AdsTypeFeed, AdsTypePartyMember, AdsTypeCountryMember, AdsFrequencyTypeId, DaysS, DaysM, DaysT, DaysW, DaysTh, DaysF, DaysSa, AdTime, StartDate, EndDate, PreviewMsg, Message, Cost)
		VALUES (parmAdvertisementId,parmUserId,parmAdsTypeEmail,parmAdsTypeFeed,parmAdsTypePartyMember,parmAdsTypeCountryMember,parmAdsFrequencyTypeId,parmDaysS,parmDaysM,parmDaysT,parmDaysW,parmDaysTh,parmDaysF,parmDaysSa,parmAdTime,parmStartDate,parmEndDate,parmPreviewMsg,parmMessage,parmCost);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AdvertisementSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AdvertisementSelect` $$
CREATE PROCEDURE `AdvertisementSelect` (
	 IN parmAdvertisementId char(36),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  AdvertisementId, UserId, AdsTypeEmail, AdsTypeFeed, AdsTypePartyMember, AdsTypeCountryMember, AdsFrequencyTypeId, DaysS, DaysM, DaysT, DaysW, DaysTh, DaysF, DaysSa, AdTime, StartDate, EndDate, PreviewMsg, Message, Cost FROM Advertisement WHERE
			AdvertisementId = parmAdvertisementId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AllowedWebUserAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AllowedWebUserAdd` $$
CREATE PROCEDURE `AllowedWebUserAdd` (
	 IN parmEmailId varchar(100),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO AllowedWebUser( EmailId)
		VALUES (parmEmailId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure AllowedWebUserSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `AllowedWebUserSelect` $$
CREATE PROCEDURE `AllowedWebUserSelect` (
	 IN parmEmailId varchar(100)
)

	BEGIN 

		SELECT  EmailId FROM AllowedWebUser WHERE
			EmailId = parmEmailId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure BudgetCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `BudgetCodeAddUpdate` $$
CREATE PROCEDURE `BudgetCodeAddUpdate` (
	 IN parmBudgetType tinyint(3),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO BudgetCode( BudgetType, Description, ImageFont)
		VALUES (parmBudgetType,parmDescription,parmImageFont)


	ON DUPLICATE KEY UPDATE
				Description = parmDescription,
				ImageFont = parmImageFont
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure BudgetCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `BudgetCodeUpdate` $$
CREATE PROCEDURE `BudgetCodeUpdate` (
	 IN parmBudgetType tinyint(3),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50)
)

	BEGIN 

	UPDATE BudgetCode SET 
				Description = parmDescription,
				ImageFont = parmImageFont
		WHERE
			BudgetType = parmBudgetType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure BudgetCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `BudgetCodeAdd` $$
CREATE PROCEDURE `BudgetCodeAdd` (
	 IN parmBudgetType tinyint(3),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO BudgetCode( BudgetType, Description, ImageFont)
		VALUES (parmBudgetType,parmDescription,parmImageFont);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure BudgetCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `BudgetCodeSelect` $$
CREATE PROCEDURE `BudgetCodeSelect` (
	 IN parmBudgetType tinyint(3)
)

	BEGIN 

		SELECT  BudgetType, Description, ImageFont FROM BudgetCode WHERE
			BudgetType = parmBudgetType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CandidateAgendaAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CandidateAgendaAdd` $$
CREATE PROCEDURE `CandidateAgendaAdd` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmAgendaTypeId smallint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CandidateAgenda( ElectionId, UserId, AgendaTypeId)
		VALUES (parmElectionId,parmUserId,parmAgendaTypeId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CandidateAgendaSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CandidateAgendaSelect` $$
CREATE PROCEDURE `CandidateAgendaSelect` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmAgendaTypeId smallint(3)
)

	BEGIN 

		SELECT  ElectionId, UserId, AgendaTypeId FROM CandidateAgenda WHERE
			ElectionId = parmElectionId AND
		UserId = parmUserId AND
		AgendaTypeId = parmAgendaTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTransactionLogAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTransactionLogAddUpdate` $$
CREATE PROCEDURE `CapitalTransactionLogAddUpdate` (
	 IN parmLogId char(36),
	 IN parmSourceId int(11),
	 IN parmSourceGuid char(36),
	 IN parmRecipentId int(11),
	 IN parmRecipentGuid char(36),
	 IN parmAmount decimal(50,2),
	 IN parmTaxAmount decimal(50,2),
	 IN parmFundType tinyint(3),
	 IN parmCreatedAT datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CapitalTransactionLog( LogId, SourceId, SourceGuid, RecipentId, RecipentGuid, Amount, TaxAmount, FundType, CreatedAT)
		VALUES (parmLogId,parmSourceId,parmSourceGuid,parmRecipentId,parmRecipentGuid,parmAmount,parmTaxAmount,parmFundType,parmCreatedAT)


	ON DUPLICATE KEY UPDATE
				SourceId = parmSourceId,
				SourceGuid = parmSourceGuid,
				RecipentId = parmRecipentId,
				RecipentGuid = parmRecipentGuid,
				Amount = parmAmount,
				TaxAmount = parmTaxAmount,
				FundType = parmFundType,
				CreatedAT = parmCreatedAT
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTransactionLogUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTransactionLogUpdate` $$
CREATE PROCEDURE `CapitalTransactionLogUpdate` (
	 IN parmLogId char(36),
	 IN parmSourceId int(11),
	 IN parmSourceGuid char(36),
	 IN parmRecipentId int(11),
	 IN parmRecipentGuid char(36),
	 IN parmAmount decimal(50,2),
	 IN parmTaxAmount decimal(50,2),
	 IN parmFundType tinyint(3),
	 IN parmCreatedAT datetime(6)
)

	BEGIN 

	UPDATE CapitalTransactionLog SET 
				SourceId = parmSourceId,
				SourceGuid = parmSourceGuid,
				RecipentId = parmRecipentId,
				RecipentGuid = parmRecipentGuid,
				Amount = parmAmount,
				TaxAmount = parmTaxAmount,
				FundType = parmFundType,
				CreatedAT = parmCreatedAT
		WHERE
			LogId = parmLogId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTransactionLogAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTransactionLogAdd` $$
CREATE PROCEDURE `CapitalTransactionLogAdd` (
	 IN parmLogId char(36),
	 IN parmSourceId int(11),
	 IN parmSourceGuid char(36),
	 IN parmRecipentId int(11),
	 IN parmRecipentGuid char(36),
	 IN parmAmount decimal(50,2),
	 IN parmTaxAmount decimal(50,2),
	 IN parmFundType tinyint(3),
	 IN parmCreatedAT datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CapitalTransactionLog( LogId, SourceId, SourceGuid, RecipentId, RecipentGuid, Amount, TaxAmount, FundType, CreatedAT)
		VALUES (parmLogId,parmSourceId,parmSourceGuid,parmRecipentId,parmRecipentGuid,parmAmount,parmTaxAmount,parmFundType,parmCreatedAT);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTransactionLogSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTransactionLogSelect` $$
CREATE PROCEDURE `CapitalTransactionLogSelect` (
	 IN parmLogId char(36)
)

	BEGIN 

		SELECT  LogId, SourceId, SourceGuid, RecipentId, RecipentGuid, Amount, TaxAmount, FundType, CreatedAT FROM CapitalTransactionLog WHERE
			LogId = parmLogId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTypeAddUpdate` $$
CREATE PROCEDURE `CapitalTypeAddUpdate` (
	 IN parmCapitalTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(50,2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CapitalType( CapitalTypeId, Name, Description, ImageFont, Cost)
		VALUES (parmCapitalTypeId,parmName,parmDescription,parmImageFont,parmCost)


	ON DUPLICATE KEY UPDATE
				Name = parmName,
				Description = parmDescription,
				ImageFont = parmImageFont,
				Cost = parmCost
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTypeUpdate` $$
CREATE PROCEDURE `CapitalTypeUpdate` (
	 IN parmCapitalTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(50,2)
)

	BEGIN 

	UPDATE CapitalType SET 
				Name = parmName,
				Description = parmDescription,
				ImageFont = parmImageFont,
				Cost = parmCost
		WHERE
			CapitalTypeId = parmCapitalTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTypeAdd` $$
CREATE PROCEDURE `CapitalTypeAdd` (
	 IN parmCapitalTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(50,2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CapitalType( CapitalTypeId, Name, Description, ImageFont, Cost)
		VALUES (parmCapitalTypeId,parmName,parmDescription,parmImageFont,parmCost);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CapitalTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CapitalTypeSelect` $$
CREATE PROCEDURE `CapitalTypeSelect` (
	 IN parmCapitalTypeId smallint(4)
)

	BEGIN 

		SELECT  CapitalTypeId, Name, Description, ImageFont, Cost FROM CapitalType WHERE
			CapitalTypeId = parmCapitalTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactProviderAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactProviderAddUpdate` $$
CREATE PROCEDURE `ContactProviderAddUpdate` (
	 IN parmProviderId tinyint(2),
	 IN parmProviderName varchar(20),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO ContactProvider( ProviderId, ProviderName, ImageFont)
		VALUES (parmProviderId,parmProviderName,parmImageFont)


	ON DUPLICATE KEY UPDATE
				ProviderName = parmProviderName,
				ImageFont = parmImageFont
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactProviderUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactProviderUpdate` $$
CREATE PROCEDURE `ContactProviderUpdate` (
	 IN parmProviderId tinyint(2),
	 IN parmProviderName varchar(20),
	 IN parmImageFont varchar(50)
)

	BEGIN 

	UPDATE ContactProvider SET 
				ProviderName = parmProviderName,
				ImageFont = parmImageFont
		WHERE
			ProviderId = parmProviderId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactProviderAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactProviderAdd` $$
CREATE PROCEDURE `ContactProviderAdd` (
	 IN parmProviderId tinyint(2),
	 IN parmProviderName varchar(20),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ContactProvider( ProviderId, ProviderName, ImageFont)
		VALUES (parmProviderId,parmProviderName,parmImageFont);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactProviderSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactProviderSelect` $$
CREATE PROCEDURE `ContactProviderSelect` (
	 IN parmProviderId tinyint(2)
)

	BEGIN 

		SELECT  ProviderId, ProviderName, ImageFont FROM ContactProvider WHERE
			ProviderId = parmProviderId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactSourceAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactSourceAddUpdate` $$
CREATE PROCEDURE `ContactSourceAddUpdate` (
	 IN parmProviderId tinyint(2),
	 IN parmUserId int(11),
	 IN parmTotal smallint(5),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO ContactSource( ProviderId, UserId, Total, UpdatedAt)
		VALUES (parmProviderId,parmUserId,parmTotal,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				Total = parmTotal,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactSourceUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactSourceUpdate` $$
CREATE PROCEDURE `ContactSourceUpdate` (
	 IN parmProviderId tinyint(2),
	 IN parmUserId int(11),
	 IN parmTotal smallint(5),
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE ContactSource SET 
				Total = parmTotal,
				UpdatedAt = parmUpdatedAt
		WHERE
			ProviderId = parmProviderId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactSourceAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactSourceAdd` $$
CREATE PROCEDURE `ContactSourceAdd` (
	 IN parmProviderId tinyint(2),
	 IN parmUserId int(11),
	 IN parmTotal smallint(5),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ContactSource( ProviderId, UserId, Total, UpdatedAt)
		VALUES (parmProviderId,parmUserId,parmTotal,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ContactSourceSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ContactSourceSelect` $$
CREATE PROCEDURE `ContactSourceSelect` (
	 IN parmProviderId tinyint(2),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  ProviderId, UserId, Total, UpdatedAt FROM ContactSource WHERE
			ProviderId = parmProviderId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetAddUpdate` $$
CREATE PROCEDURE `CountryBudgetAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmTotalAmount decimal(60,2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmStatus char(1),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryBudget( TaskId, CountryId, TotalAmount, StartDate, EndDate, Status, CreatedAt)
		VALUES (parmTaskId,parmCountryId,parmTotalAmount,parmStartDate,parmEndDate,parmStatus,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				CountryId = parmCountryId,
				TotalAmount = parmTotalAmount,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				Status = parmStatus,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetUpdate` $$
CREATE PROCEDURE `CountryBudgetUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmTotalAmount decimal(60,2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmStatus char(1),
	 IN parmCreatedAt datetime
)

	BEGIN 

	UPDATE CountryBudget SET 
				CountryId = parmCountryId,
				TotalAmount = parmTotalAmount,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				Status = parmStatus,
				CreatedAt = parmCreatedAt
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetAdd` $$
CREATE PROCEDURE `CountryBudgetAdd` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmTotalAmount decimal(60,2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmStatus char(1),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryBudget( TaskId, CountryId, TotalAmount, StartDate, EndDate, Status, CreatedAt)
		VALUES (parmTaskId,parmCountryId,parmTotalAmount,parmStartDate,parmEndDate,parmStatus,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetSelect` $$
CREATE PROCEDURE `CountryBudgetSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, CountryId, TotalAmount, StartDate, EndDate, Status, CreatedAt FROM CountryBudget WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetByTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetByTypeAddUpdate` $$
CREATE PROCEDURE `CountryBudgetByTypeAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmAmount decimal(30,2),
	 IN parmAmountLeft decimal(30,2),
	 IN parmBudgetType tinyint(3),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryBudgetByType( TaskId, Amount, AmountLeft, BudgetType)
		VALUES (parmTaskId,parmAmount,parmAmountLeft,parmBudgetType)


	ON DUPLICATE KEY UPDATE
				Amount = parmAmount,
				AmountLeft = parmAmountLeft
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetByTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetByTypeUpdate` $$
CREATE PROCEDURE `CountryBudgetByTypeUpdate` (
	 IN parmTaskId char(36),
	 IN parmAmount decimal(30,2),
	 IN parmAmountLeft decimal(30,2),
	 IN parmBudgetType tinyint(3)
)

	BEGIN 

	UPDATE CountryBudgetByType SET 
				Amount = parmAmount,
				AmountLeft = parmAmountLeft
		WHERE
			TaskId = parmTaskId AND
		BudgetType = parmBudgetType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetByTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetByTypeAdd` $$
CREATE PROCEDURE `CountryBudgetByTypeAdd` (
	 IN parmTaskId char(36),
	 IN parmAmount decimal(30,2),
	 IN parmAmountLeft decimal(30,2),
	 IN parmBudgetType tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryBudgetByType( TaskId, Amount, AmountLeft, BudgetType)
		VALUES (parmTaskId,parmAmount,parmAmountLeft,parmBudgetType);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryBudgetByTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryBudgetByTypeSelect` $$
CREATE PROCEDURE `CountryBudgetByTypeSelect` (
	 IN parmTaskId char(36),
	 IN parmBudgetType tinyint(3)
)

	BEGIN 

		SELECT  TaskId, Amount, AmountLeft, BudgetType FROM CountryBudgetByType WHERE
			TaskId = parmTaskId AND
		BudgetType = parmBudgetType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryCodeAddUpdate` $$
CREATE PROCEDURE `CountryCodeAddUpdate` (
	 IN parmCountryUserId int(11),
	 IN parmCountryId char(2),
	 IN parmCode varchar(100),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryCode( CountryUserId, CountryId, Code)
		VALUES (parmCountryUserId,parmCountryId,parmCode)


	ON DUPLICATE KEY UPDATE
				CountryUserId = parmCountryUserId,
				Code = parmCode
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryCodeUpdate` $$
CREATE PROCEDURE `CountryCodeUpdate` (
	 IN parmCountryUserId int(11),
	 IN parmCountryId char(2),
	 IN parmCode varchar(100)
)

	BEGIN 

	UPDATE CountryCode SET 
				CountryUserId = parmCountryUserId,
				Code = parmCode
		WHERE
			CountryId = parmCountryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryCodeAdd` $$
CREATE PROCEDURE `CountryCodeAdd` (
	 IN parmCountryUserId int(11),
	 IN parmCountryId char(2),
	 IN parmCode varchar(100),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryCode( CountryUserId, CountryId, Code)
		VALUES (parmCountryUserId,parmCountryId,parmCode);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryCodeSelect` $$
CREATE PROCEDURE `CountryCodeSelect` (
	 IN parmCountryId char(2)
)

	BEGIN 

		SELECT  CountryUserId, CountryId, Code FROM CountryCode WHERE
			CountryId = parmCountryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryLeaderAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryLeaderAddUpdate` $$
CREATE PROCEDURE `CountryLeaderAddUpdate` (
	 IN parmCountryId char(2),
	 IN parmUserId int(11),
	 IN parmCandidateTypeId char(1),
	 IN parmPositionTypeId tinyint(3),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryLeader( CountryId, UserId, CandidateTypeId, PositionTypeId, StartDate, EndDate)
		VALUES (parmCountryId,parmUserId,parmCandidateTypeId,parmPositionTypeId,parmStartDate,parmEndDate)


	ON DUPLICATE KEY UPDATE
				CountryId = parmCountryId,
				CandidateTypeId = parmCandidateTypeId,
				PositionTypeId = parmPositionTypeId
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryLeaderUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryLeaderUpdate` $$
CREATE PROCEDURE `CountryLeaderUpdate` (
	 IN parmCountryId char(2),
	 IN parmUserId int(11),
	 IN parmCandidateTypeId char(1),
	 IN parmPositionTypeId tinyint(3),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime
)

	BEGIN 

	UPDATE CountryLeader SET 
				CountryId = parmCountryId,
				CandidateTypeId = parmCandidateTypeId,
				PositionTypeId = parmPositionTypeId
		WHERE
			UserId = parmUserId AND
		StartDate = parmStartDate AND
		EndDate = parmEndDate ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryLeaderAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryLeaderAdd` $$
CREATE PROCEDURE `CountryLeaderAdd` (
	 IN parmCountryId char(2),
	 IN parmUserId int(11),
	 IN parmCandidateTypeId char(1),
	 IN parmPositionTypeId tinyint(3),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryLeader( CountryId, UserId, CandidateTypeId, PositionTypeId, StartDate, EndDate)
		VALUES (parmCountryId,parmUserId,parmCandidateTypeId,parmPositionTypeId,parmStartDate,parmEndDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryLeaderSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryLeaderSelect` $$
CREATE PROCEDURE `CountryLeaderSelect` (
	 IN parmUserId int(11),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime
)

	BEGIN 

		SELECT  CountryId, UserId, CandidateTypeId, PositionTypeId, StartDate, EndDate FROM CountryLeader WHERE
			UserId = parmUserId AND
		StartDate = parmStartDate AND
		EndDate = parmEndDate ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryRevenueAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryRevenueAddUpdate` $$
CREATE PROCEDURE `CountryRevenueAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmCash decimal(50,2),
	 IN parmStatus char(1),
	 IN parmTaxType tinyint(3),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryRevenue( TaskId, CountryId, Cash, Status, TaxType, UpdatedAt)
		VALUES (parmTaskId,parmCountryId,parmCash,parmStatus,parmTaxType,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				CountryId = parmCountryId,
				Cash = parmCash,
				Status = parmStatus,
				TaxType = parmTaxType,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryRevenueUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryRevenueUpdate` $$
CREATE PROCEDURE `CountryRevenueUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmCash decimal(50,2),
	 IN parmStatus char(1),
	 IN parmTaxType tinyint(3),
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE CountryRevenue SET 
				CountryId = parmCountryId,
				Cash = parmCash,
				Status = parmStatus,
				TaxType = parmTaxType,
				UpdatedAt = parmUpdatedAt
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryRevenueAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryRevenueAdd` $$
CREATE PROCEDURE `CountryRevenueAdd` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmCash decimal(50,2),
	 IN parmStatus char(1),
	 IN parmTaxType tinyint(3),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryRevenue( TaskId, CountryId, Cash, Status, TaxType, UpdatedAt)
		VALUES (parmTaskId,parmCountryId,parmCash,parmStatus,parmTaxType,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryRevenueSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryRevenueSelect` $$
CREATE PROCEDURE `CountryRevenueSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, CountryId, Cash, Status, TaxType, UpdatedAt FROM CountryRevenue WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxAddUpdate` $$
CREATE PROCEDURE `CountryTaxAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmStatus char(1),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryTax( TaskId, CountryId, Status, StartDate, EndDate, CreatedAt)
		VALUES (parmTaskId,parmCountryId,parmStatus,parmStartDate,parmEndDate,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				CountryId = parmCountryId,
				Status = parmStatus,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxUpdate` $$
CREATE PROCEDURE `CountryTaxUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmStatus char(1),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmCreatedAt datetime
)

	BEGIN 

	UPDATE CountryTax SET 
				CountryId = parmCountryId,
				Status = parmStatus,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				CreatedAt = parmCreatedAt
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxAdd` $$
CREATE PROCEDURE `CountryTaxAdd` (
	 IN parmTaskId char(36),
	 IN parmCountryId char(2),
	 IN parmStatus char(1),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryTax( TaskId, CountryId, Status, StartDate, EndDate, CreatedAt)
		VALUES (parmTaskId,parmCountryId,parmStatus,parmStartDate,parmEndDate,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxSelect` $$
CREATE PROCEDURE `CountryTaxSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, CountryId, Status, StartDate, EndDate, CreatedAt FROM CountryTax WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxByTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxByTypeAddUpdate` $$
CREATE PROCEDURE `CountryTaxByTypeAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmTaxPercent decimal(5,2),
	 IN parmTaxType tinyint(3),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryTaxByType( TaskId, TaxPercent, TaxType)
		VALUES (parmTaskId,parmTaxPercent,parmTaxType)


	ON DUPLICATE KEY UPDATE
				TaxPercent = parmTaxPercent
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxByTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxByTypeUpdate` $$
CREATE PROCEDURE `CountryTaxByTypeUpdate` (
	 IN parmTaskId char(36),
	 IN parmTaxPercent decimal(5,2),
	 IN parmTaxType tinyint(3)
)

	BEGIN 

	UPDATE CountryTaxByType SET 
				TaxPercent = parmTaxPercent
		WHERE
			TaskId = parmTaskId AND
		TaxType = parmTaxType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxByTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxByTypeAdd` $$
CREATE PROCEDURE `CountryTaxByTypeAdd` (
	 IN parmTaskId char(36),
	 IN parmTaxPercent decimal(5,2),
	 IN parmTaxType tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryTaxByType( TaskId, TaxPercent, TaxType)
		VALUES (parmTaskId,parmTaxPercent,parmTaxType);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryTaxByTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryTaxByTypeSelect` $$
CREATE PROCEDURE `CountryTaxByTypeSelect` (
	 IN parmTaskId char(36),
	 IN parmTaxType tinyint(3)
)

	BEGIN 

		SELECT  TaskId, TaxPercent, TaxType FROM CountryTaxByType WHERE
			TaskId = parmTaskId AND
		TaxType = parmTaxType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryWeaponAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryWeaponAddUpdate` $$
CREATE PROCEDURE `CountryWeaponAddUpdate` (
	 IN parmCountryWeaponId int(11),
	 IN parmCountryId char(2),
	 IN parmUserId int(11),
	 IN parmQuantity int(11),
	 IN parmWeaponTypeId smallint(4),
	 IN parmWeaponCondition tinyint(3) unsigned,
	 IN parmPurchasedPrice decimal(15,2),
	 IN parmPurchasedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CountryWeapon( CountryWeaponId, CountryId, UserId, Quantity, WeaponTypeId, WeaponCondition, PurchasedPrice, PurchasedAt)
		VALUES (parmCountryWeaponId,parmCountryId,parmUserId,parmQuantity,parmWeaponTypeId,parmWeaponCondition,parmPurchasedPrice,parmPurchasedAt)


	ON DUPLICATE KEY UPDATE
				CountryId = parmCountryId,
				UserId = parmUserId,
				Quantity = parmQuantity,
				WeaponTypeId = parmWeaponTypeId,
				WeaponCondition = parmWeaponCondition,
				PurchasedPrice = parmPurchasedPrice,
				PurchasedAt = parmPurchasedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryWeaponUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryWeaponUpdate` $$
CREATE PROCEDURE `CountryWeaponUpdate` (
	 IN parmCountryWeaponId int(11),
	 IN parmCountryId char(2),
	 IN parmUserId int(11),
	 IN parmQuantity int(11),
	 IN parmWeaponTypeId smallint(4),
	 IN parmWeaponCondition tinyint(3) unsigned,
	 IN parmPurchasedPrice decimal(15,2),
	 IN parmPurchasedAt datetime
)

	BEGIN 

	UPDATE CountryWeapon SET 
				CountryId = parmCountryId,
				UserId = parmUserId,
				Quantity = parmQuantity,
				WeaponTypeId = parmWeaponTypeId,
				WeaponCondition = parmWeaponCondition,
				PurchasedPrice = parmPurchasedPrice,
				PurchasedAt = parmPurchasedAt
		WHERE
			CountryWeaponId = parmCountryWeaponId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryWeaponAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryWeaponAdd` $$
CREATE PROCEDURE `CountryWeaponAdd` (
	 IN parmCountryWeaponId int(11),
	 IN parmCountryId char(2),
	 IN parmUserId int(11),
	 IN parmQuantity int(11),
	 IN parmWeaponTypeId smallint(4),
	 IN parmWeaponCondition tinyint(3) unsigned,
	 IN parmPurchasedPrice decimal(15,2),
	 IN parmPurchasedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CountryWeapon( CountryWeaponId, CountryId, UserId, Quantity, WeaponTypeId, WeaponCondition, PurchasedPrice, PurchasedAt)
		VALUES (parmCountryWeaponId,parmCountryId,parmUserId,parmQuantity,parmWeaponTypeId,parmWeaponCondition,parmPurchasedPrice,parmPurchasedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CountryWeaponSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CountryWeaponSelect` $$
CREATE PROCEDURE `CountryWeaponSelect` (
	 IN parmCountryWeaponId int(11)
)

	BEGIN 

		SELECT  CountryWeaponId, CountryId, UserId, Quantity, WeaponTypeId, WeaponCondition, PurchasedPrice, PurchasedAt FROM CountryWeapon WHERE
			CountryWeaponId = parmCountryWeaponId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditInterestAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditInterestAddUpdate` $$
CREATE PROCEDURE `CreditInterestAddUpdate` (
	 IN parmMinScore smallint(6),
	 IN parmMaxScore smallint(6),
	 IN parmMinMonthlyIntrestRate decimal(5,2),
	 IN parmQualifiedAmount decimal(10,2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CreditInterest( MinScore, MaxScore, MinMonthlyIntrestRate, QualifiedAmount)
		VALUES (parmMinScore,parmMaxScore,parmMinMonthlyIntrestRate,parmQualifiedAmount)


	ON DUPLICATE KEY UPDATE
				MinMonthlyIntrestRate = parmMinMonthlyIntrestRate,
				QualifiedAmount = parmQualifiedAmount
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditInterestUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditInterestUpdate` $$
CREATE PROCEDURE `CreditInterestUpdate` (
	 IN parmMinScore smallint(6),
	 IN parmMaxScore smallint(6),
	 IN parmMinMonthlyIntrestRate decimal(5,2),
	 IN parmQualifiedAmount decimal(10,2)
)

	BEGIN 

	UPDATE CreditInterest SET 
				MinMonthlyIntrestRate = parmMinMonthlyIntrestRate,
				QualifiedAmount = parmQualifiedAmount
		WHERE
			MinScore = parmMinScore AND
		MaxScore = parmMaxScore ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditInterestAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditInterestAdd` $$
CREATE PROCEDURE `CreditInterestAdd` (
	 IN parmMinScore smallint(6),
	 IN parmMaxScore smallint(6),
	 IN parmMinMonthlyIntrestRate decimal(5,2),
	 IN parmQualifiedAmount decimal(10,2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CreditInterest( MinScore, MaxScore, MinMonthlyIntrestRate, QualifiedAmount)
		VALUES (parmMinScore,parmMaxScore,parmMinMonthlyIntrestRate,parmQualifiedAmount);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditInterestSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditInterestSelect` $$
CREATE PROCEDURE `CreditInterestSelect` (
	 IN parmMinScore smallint(6),
	 IN parmMaxScore smallint(6)
)

	BEGIN 

		SELECT  MinScore, MaxScore, MinMonthlyIntrestRate, QualifiedAmount FROM CreditInterest WHERE
			MinScore = parmMinScore AND
		MaxScore = parmMaxScore ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditScoreAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditScoreAddUpdate` $$
CREATE PROCEDURE `CreditScoreAddUpdate` (
	 IN parmUserId int(11),
	 IN parmScore decimal(50,2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CreditScore( UserId, Score)
		VALUES (parmUserId,parmScore)


	ON DUPLICATE KEY UPDATE
				Score = parmScore
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditScoreUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditScoreUpdate` $$
CREATE PROCEDURE `CreditScoreUpdate` (
	 IN parmUserId int(11),
	 IN parmScore decimal(50,2)
)

	BEGIN 

	UPDATE CreditScore SET 
				Score = parmScore
		WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditScoreAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditScoreAdd` $$
CREATE PROCEDURE `CreditScoreAdd` (
	 IN parmUserId int(11),
	 IN parmScore decimal(50,2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CreditScore( UserId, Score)
		VALUES (parmUserId,parmScore);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CreditScoreSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CreditScoreSelect` $$
CREATE PROCEDURE `CreditScoreSelect` (
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  UserId, Score FROM CreditScore WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentAddUpdate` $$
CREATE PROCEDURE `CrimeIncidentAddUpdate` (
	 IN parmIncidentId char(36),
	 IN parmUserId int(11),
	 IN parmVictimId int(11),
	 IN parmAmount decimal(50,2),
	 IN parmIncidentDate timestamp,
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmIncidentType char(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CrimeIncident( IncidentId, UserId, VictimId, Amount, IncidentDate, MerchandiseTypeId, IncidentType)
		VALUES (parmIncidentId,parmUserId,parmVictimId,parmAmount,parmIncidentDate,parmMerchandiseTypeId,parmIncidentType)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				VictimId = parmVictimId,
				Amount = parmAmount,
				IncidentDate = parmIncidentDate,
				MerchandiseTypeId = parmMerchandiseTypeId,
				IncidentType = parmIncidentType
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentUpdate` $$
CREATE PROCEDURE `CrimeIncidentUpdate` (
	 IN parmIncidentId char(36),
	 IN parmUserId int(11),
	 IN parmVictimId int(11),
	 IN parmAmount decimal(50,2),
	 IN parmIncidentDate timestamp,
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmIncidentType char(1)
)

	BEGIN 

	UPDATE CrimeIncident SET 
				UserId = parmUserId,
				VictimId = parmVictimId,
				Amount = parmAmount,
				IncidentDate = parmIncidentDate,
				MerchandiseTypeId = parmMerchandiseTypeId,
				IncidentType = parmIncidentType
		WHERE
			IncidentId = parmIncidentId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentAdd` $$
CREATE PROCEDURE `CrimeIncidentAdd` (
	 IN parmIncidentId char(36),
	 IN parmUserId int(11),
	 IN parmVictimId int(11),
	 IN parmAmount decimal(50,2),
	 IN parmIncidentDate timestamp,
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmIncidentType char(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CrimeIncident( IncidentId, UserId, VictimId, Amount, IncidentDate, MerchandiseTypeId, IncidentType)
		VALUES (parmIncidentId,parmUserId,parmVictimId,parmAmount,parmIncidentDate,parmMerchandiseTypeId,parmIncidentType);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentSelect` $$
CREATE PROCEDURE `CrimeIncidentSelect` (
	 IN parmIncidentId char(36)
)

	BEGIN 

		SELECT  IncidentId, UserId, VictimId, Amount, IncidentDate, MerchandiseTypeId, IncidentType FROM CrimeIncident WHERE
			IncidentId = parmIncidentId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentSuspectAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentSuspectAddUpdate` $$
CREATE PROCEDURE `CrimeIncidentSuspectAddUpdate` (
	 IN parmIncidentId char(36),
	 IN parmUserId int(11),
	 IN parmSuspectId int(11),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CrimeIncidentSuspect( IncidentId, UserId, SuspectId)
		VALUES (parmIncidentId,parmUserId,parmSuspectId)


	ON DUPLICATE KEY UPDATE
				SuspectId = parmSuspectId
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentSuspectUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentSuspectUpdate` $$
CREATE PROCEDURE `CrimeIncidentSuspectUpdate` (
	 IN parmIncidentId char(36),
	 IN parmUserId int(11),
	 IN parmSuspectId int(11)
)

	BEGIN 

	UPDATE CrimeIncidentSuspect SET 
				SuspectId = parmSuspectId
		WHERE
			IncidentId = parmIncidentId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentSuspectAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentSuspectAdd` $$
CREATE PROCEDURE `CrimeIncidentSuspectAdd` (
	 IN parmIncidentId char(36),
	 IN parmUserId int(11),
	 IN parmSuspectId int(11),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CrimeIncidentSuspect( IncidentId, UserId, SuspectId)
		VALUES (parmIncidentId,parmUserId,parmSuspectId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeIncidentSuspectSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeIncidentSuspectSelect` $$
CREATE PROCEDURE `CrimeIncidentSuspectSelect` (
	 IN parmIncidentId char(36),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  IncidentId, UserId, SuspectId FROM CrimeIncidentSuspect WHERE
			IncidentId = parmIncidentId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeReportAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeReportAddUpdate` $$
CREATE PROCEDURE `CrimeReportAddUpdate` (
	 IN parmUserId int(11),
	 IN parmWantedScore decimal(5,4),
	 IN parmArrestCount int(11),
	 IN parmLootToDate decimal(50,2),
	 IN parmSuspectCount int(11),
	 IN parmIncidentCount int(11),
	 IN parmLastArrestDate timestamp,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO CrimeReport( UserId, WantedScore, ArrestCount, LootToDate, SuspectCount, IncidentCount, LastArrestDate, UpdatedAt)
		VALUES (parmUserId,parmWantedScore,parmArrestCount,parmLootToDate,parmSuspectCount,parmIncidentCount,parmLastArrestDate,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				WantedScore = parmWantedScore,
				ArrestCount = parmArrestCount,
				LootToDate = parmLootToDate,
				SuspectCount = parmSuspectCount,
				IncidentCount = parmIncidentCount,
				LastArrestDate = parmLastArrestDate,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeReportUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeReportUpdate` $$
CREATE PROCEDURE `CrimeReportUpdate` (
	 IN parmUserId int(11),
	 IN parmWantedScore decimal(5,4),
	 IN parmArrestCount int(11),
	 IN parmLootToDate decimal(50,2),
	 IN parmSuspectCount int(11),
	 IN parmIncidentCount int(11),
	 IN parmLastArrestDate timestamp,
	 IN parmUpdatedAt timestamp
)

	BEGIN 

	UPDATE CrimeReport SET 
				WantedScore = parmWantedScore,
				ArrestCount = parmArrestCount,
				LootToDate = parmLootToDate,
				SuspectCount = parmSuspectCount,
				IncidentCount = parmIncidentCount,
				LastArrestDate = parmLastArrestDate,
				UpdatedAt = parmUpdatedAt
		WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeReportAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeReportAdd` $$
CREATE PROCEDURE `CrimeReportAdd` (
	 IN parmUserId int(11),
	 IN parmWantedScore decimal(5,4),
	 IN parmArrestCount int(11),
	 IN parmLootToDate decimal(50,2),
	 IN parmSuspectCount int(11),
	 IN parmIncidentCount int(11),
	 IN parmLastArrestDate timestamp,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO CrimeReport( UserId, WantedScore, ArrestCount, LootToDate, SuspectCount, IncidentCount, LastArrestDate, UpdatedAt)
		VALUES (parmUserId,parmWantedScore,parmArrestCount,parmLootToDate,parmSuspectCount,parmIncidentCount,parmLastArrestDate,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure CrimeReportSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `CrimeReportSelect` $$
CREATE PROCEDURE `CrimeReportSelect` (
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  UserId, WantedScore, ArrestCount, LootToDate, SuspectCount, IncidentCount, LastArrestDate, UpdatedAt FROM CrimeReport WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure DegreeCheckJobAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `DegreeCheckJobAdd` $$
CREATE PROCEDURE `DegreeCheckJobAdd` (
	 IN parmRunId int(11),
	 IN parmUserId int(11),
	 IN parmMajorId smallint(3),
	 IN parmDegreeId tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO DegreeCheckJob( RunId, UserId, MajorId, DegreeId)
		VALUES (parmRunId,parmUserId,parmMajorId,parmDegreeId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure DegreeCheckJobSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `DegreeCheckJobSelect` $$
CREATE PROCEDURE `DegreeCheckJobSelect` (
	 IN parmRunId int(11),
	 IN parmUserId int(11),
	 IN parmMajorId smallint(3),
	 IN parmDegreeId tinyint(3)
)

	BEGIN 

		SELECT  RunId, UserId, MajorId, DegreeId FROM DegreeCheckJob WHERE
			RunId = parmRunId AND
		UserId = parmUserId AND
		MajorId = parmMajorId AND
		DegreeId = parmDegreeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure DegreeCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `DegreeCodeAddUpdate` $$
CREATE PROCEDURE `DegreeCodeAddUpdate` (
	 IN parmDegreeId tinyint(3),
	 IN parmDegreeName varchar(25),
	 IN parmDegreeImageFont varchar(50),
	 IN parmDegreeRank tinyint(3),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO DegreeCode( DegreeId, DegreeName, DegreeImageFont, DegreeRank)
		VALUES (parmDegreeId,parmDegreeName,parmDegreeImageFont,parmDegreeRank)


	ON DUPLICATE KEY UPDATE
				DegreeName = parmDegreeName,
				DegreeImageFont = parmDegreeImageFont,
				DegreeRank = parmDegreeRank
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure DegreeCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `DegreeCodeUpdate` $$
CREATE PROCEDURE `DegreeCodeUpdate` (
	 IN parmDegreeId tinyint(3),
	 IN parmDegreeName varchar(25),
	 IN parmDegreeImageFont varchar(50),
	 IN parmDegreeRank tinyint(3)
)

	BEGIN 

	UPDATE DegreeCode SET 
				DegreeName = parmDegreeName,
				DegreeImageFont = parmDegreeImageFont,
				DegreeRank = parmDegreeRank
		WHERE
			DegreeId = parmDegreeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure DegreeCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `DegreeCodeAdd` $$
CREATE PROCEDURE `DegreeCodeAdd` (
	 IN parmDegreeId tinyint(3),
	 IN parmDegreeName varchar(25),
	 IN parmDegreeImageFont varchar(50),
	 IN parmDegreeRank tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO DegreeCode( DegreeId, DegreeName, DegreeImageFont, DegreeRank)
		VALUES (parmDegreeId,parmDegreeName,parmDegreeImageFont,parmDegreeRank);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure DegreeCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `DegreeCodeSelect` $$
CREATE PROCEDURE `DegreeCodeSelect` (
	 IN parmDegreeId tinyint(3)
)

	BEGIN 

		SELECT  DegreeId, DegreeName, DegreeImageFont, DegreeRank FROM DegreeCode WHERE
			DegreeId = parmDegreeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EducationAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EducationAddUpdate` $$
CREATE PROCEDURE `EducationAddUpdate` (
	 IN parmUserId int(11),
	 IN parmMajorId smallint(3),
	 IN parmDegreeId tinyint(3),
	 IN parmStatus char(1),
	 IN parmCompletionCost decimal(50,2),
	 IN parmExpectedCompletion datetime,
	 IN parmNextBoostAt datetime,
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO Education( UserId, MajorId, DegreeId, Status, CompletionCost, ExpectedCompletion, NextBoostAt, CreatedAt)
		VALUES (parmUserId,parmMajorId,parmDegreeId,parmStatus,parmCompletionCost,parmExpectedCompletion,parmNextBoostAt,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				Status = parmStatus,
				CompletionCost = parmCompletionCost,
				ExpectedCompletion = parmExpectedCompletion,
				NextBoostAt = parmNextBoostAt,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EducationUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EducationUpdate` $$
CREATE PROCEDURE `EducationUpdate` (
	 IN parmUserId int(11),
	 IN parmMajorId smallint(3),
	 IN parmDegreeId tinyint(3),
	 IN parmStatus char(1),
	 IN parmCompletionCost decimal(50,2),
	 IN parmExpectedCompletion datetime,
	 IN parmNextBoostAt datetime,
	 IN parmCreatedAt datetime
)

	BEGIN 

	UPDATE Education SET 
				Status = parmStatus,
				CompletionCost = parmCompletionCost,
				ExpectedCompletion = parmExpectedCompletion,
				NextBoostAt = parmNextBoostAt,
				CreatedAt = parmCreatedAt
		WHERE
			UserId = parmUserId AND
		MajorId = parmMajorId AND
		DegreeId = parmDegreeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EducationAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EducationAdd` $$
CREATE PROCEDURE `EducationAdd` (
	 IN parmUserId int(11),
	 IN parmMajorId smallint(3),
	 IN parmDegreeId tinyint(3),
	 IN parmStatus char(1),
	 IN parmCompletionCost decimal(50,2),
	 IN parmExpectedCompletion datetime,
	 IN parmNextBoostAt datetime,
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO Education( UserId, MajorId, DegreeId, Status, CompletionCost, ExpectedCompletion, NextBoostAt, CreatedAt)
		VALUES (parmUserId,parmMajorId,parmDegreeId,parmStatus,parmCompletionCost,parmExpectedCompletion,parmNextBoostAt,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EducationSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EducationSelect` $$
CREATE PROCEDURE `EducationSelect` (
	 IN parmUserId int(11),
	 IN parmMajorId smallint(3),
	 IN parmDegreeId tinyint(3)
)

	BEGIN 

		SELECT  UserId, MajorId, DegreeId, Status, CompletionCost, ExpectedCompletion, NextBoostAt, CreatedAt FROM Education WHERE
			UserId = parmUserId AND
		MajorId = parmMajorId AND
		DegreeId = parmDegreeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionAddUpdate` $$
CREATE PROCEDURE `ElectionAddUpdate` (
	 IN parmElectionId mediumint(8),
	 IN parmCountryId char(2),
	 IN parmStartDate datetime,
	 IN parmVotingStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmFee decimal(10,2),
	 IN parmStartTermNotified tinyint(1),
	 IN parmVotingStartTermNotified tinyint(1),
	 IN parmLastDayTermNotified tinyint(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO Election( ElectionId, CountryId, StartDate, VotingStartDate, EndDate, Fee, StartTermNotified, VotingStartTermNotified, LastDayTermNotified)
		VALUES (parmElectionId,parmCountryId,parmStartDate,parmVotingStartDate,parmEndDate,parmFee,parmStartTermNotified,parmVotingStartTermNotified,parmLastDayTermNotified)


	ON DUPLICATE KEY UPDATE
				StartDate = parmStartDate,
				VotingStartDate = parmVotingStartDate,
				EndDate = parmEndDate,
				Fee = parmFee,
				StartTermNotified = parmStartTermNotified,
				VotingStartTermNotified = parmVotingStartTermNotified,
				LastDayTermNotified = parmLastDayTermNotified
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionUpdate` $$
CREATE PROCEDURE `ElectionUpdate` (
	 IN parmElectionId mediumint(8),
	 IN parmCountryId char(2),
	 IN parmStartDate datetime,
	 IN parmVotingStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmFee decimal(10,2),
	 IN parmStartTermNotified tinyint(1),
	 IN parmVotingStartTermNotified tinyint(1),
	 IN parmLastDayTermNotified tinyint(1)
)

	BEGIN 

	UPDATE Election SET 
				StartDate = parmStartDate,
				VotingStartDate = parmVotingStartDate,
				EndDate = parmEndDate,
				Fee = parmFee,
				StartTermNotified = parmStartTermNotified,
				VotingStartTermNotified = parmVotingStartTermNotified,
				LastDayTermNotified = parmLastDayTermNotified
		WHERE
			ElectionId = parmElectionId AND
		CountryId = parmCountryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionAdd` $$
CREATE PROCEDURE `ElectionAdd` (
	 IN parmElectionId mediumint(8),
	 IN parmCountryId char(2),
	 IN parmStartDate datetime,
	 IN parmVotingStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmFee decimal(10,2),
	 IN parmStartTermNotified tinyint(1),
	 IN parmVotingStartTermNotified tinyint(1),
	 IN parmLastDayTermNotified tinyint(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO Election( ElectionId, CountryId, StartDate, VotingStartDate, EndDate, Fee, StartTermNotified, VotingStartTermNotified, LastDayTermNotified)
		VALUES (parmElectionId,parmCountryId,parmStartDate,parmVotingStartDate,parmEndDate,parmFee,parmStartTermNotified,parmVotingStartTermNotified,parmLastDayTermNotified);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionSelect` $$
CREATE PROCEDURE `ElectionSelect` (
	 IN parmElectionId mediumint(8),
	 IN parmCountryId char(2)
)

	BEGIN 

		SELECT  ElectionId, CountryId, StartDate, VotingStartDate, EndDate, Fee, StartTermNotified, VotingStartTermNotified, LastDayTermNotified FROM Election WHERE
			ElectionId = parmElectionId AND
		CountryId = parmCountryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionAgendaAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionAgendaAddUpdate` $$
CREATE PROCEDURE `ElectionAgendaAddUpdate` (
	 IN parmAgendaTypeId smallint(3),
	 IN parmAgendaName varchar(500),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO ElectionAgenda( AgendaTypeId, AgendaName)
		VALUES (parmAgendaTypeId,parmAgendaName)


	ON DUPLICATE KEY UPDATE
				AgendaName = parmAgendaName
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionAgendaUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionAgendaUpdate` $$
CREATE PROCEDURE `ElectionAgendaUpdate` (
	 IN parmAgendaTypeId smallint(3),
	 IN parmAgendaName varchar(500)
)

	BEGIN 

	UPDATE ElectionAgenda SET 
				AgendaName = parmAgendaName
		WHERE
			AgendaTypeId = parmAgendaTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionAgendaAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionAgendaAdd` $$
CREATE PROCEDURE `ElectionAgendaAdd` (
	 IN parmAgendaTypeId smallint(3),
	 IN parmAgendaName varchar(500),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ElectionAgenda( AgendaTypeId, AgendaName)
		VALUES (parmAgendaTypeId,parmAgendaName);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionAgendaSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionAgendaSelect` $$
CREATE PROCEDURE `ElectionAgendaSelect` (
	 IN parmAgendaTypeId smallint(3)
)

	BEGIN 

		SELECT  AgendaTypeId, AgendaName FROM ElectionAgenda WHERE
			AgendaTypeId = parmAgendaTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionCandidateAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionCandidateAddUpdate` $$
CREATE PROCEDURE `ElectionCandidateAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmPartyId char(36),
	 IN parmCountryId char(2),
	 IN parmCandidateTypeId char(1),
	 IN parmPositionTypeId tinyint(3),
	 IN parmStatus char(1),
	 IN parmLogoPictureId varchar(250),
	 IN parmRequestDate datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO ElectionCandidate( TaskId, ElectionId, UserId, PartyId, CountryId, CandidateTypeId, PositionTypeId, Status, LogoPictureId, RequestDate)
		VALUES (parmTaskId,parmElectionId,parmUserId,parmPartyId,parmCountryId,parmCandidateTypeId,parmPositionTypeId,parmStatus,parmLogoPictureId,parmRequestDate)


	ON DUPLICATE KEY UPDATE
				ElectionId = parmElectionId,
				UserId = parmUserId,
				PartyId = parmPartyId,
				CountryId = parmCountryId,
				CandidateTypeId = parmCandidateTypeId,
				PositionTypeId = parmPositionTypeId,
				Status = parmStatus,
				LogoPictureId = parmLogoPictureId,
				RequestDate = parmRequestDate
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionCandidateUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionCandidateUpdate` $$
CREATE PROCEDURE `ElectionCandidateUpdate` (
	 IN parmTaskId char(36),
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmPartyId char(36),
	 IN parmCountryId char(2),
	 IN parmCandidateTypeId char(1),
	 IN parmPositionTypeId tinyint(3),
	 IN parmStatus char(1),
	 IN parmLogoPictureId varchar(250),
	 IN parmRequestDate datetime(6)
)

	BEGIN 

	UPDATE ElectionCandidate SET 
				ElectionId = parmElectionId,
				UserId = parmUserId,
				PartyId = parmPartyId,
				CountryId = parmCountryId,
				CandidateTypeId = parmCandidateTypeId,
				PositionTypeId = parmPositionTypeId,
				Status = parmStatus,
				LogoPictureId = parmLogoPictureId,
				RequestDate = parmRequestDate
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionCandidateAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionCandidateAdd` $$
CREATE PROCEDURE `ElectionCandidateAdd` (
	 IN parmTaskId char(36),
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmPartyId char(36),
	 IN parmCountryId char(2),
	 IN parmCandidateTypeId char(1),
	 IN parmPositionTypeId tinyint(3),
	 IN parmStatus char(1),
	 IN parmLogoPictureId varchar(250),
	 IN parmRequestDate datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ElectionCandidate( TaskId, ElectionId, UserId, PartyId, CountryId, CandidateTypeId, PositionTypeId, Status, LogoPictureId, RequestDate)
		VALUES (parmTaskId,parmElectionId,parmUserId,parmPartyId,parmCountryId,parmCandidateTypeId,parmPositionTypeId,parmStatus,parmLogoPictureId,parmRequestDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionCandidateSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionCandidateSelect` $$
CREATE PROCEDURE `ElectionCandidateSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, ElectionId, UserId, PartyId, CountryId, CandidateTypeId, PositionTypeId, Status, LogoPictureId, RequestDate FROM ElectionCandidate WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionPositionAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionPositionAddUpdate` $$
CREATE PROCEDURE `ElectionPositionAddUpdate` (
	 IN parmPositionTypeId tinyint(3),
	 IN parmElectionPositionName varchar(25),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO ElectionPosition( PositionTypeId, ElectionPositionName)
		VALUES (parmPositionTypeId,parmElectionPositionName)


	ON DUPLICATE KEY UPDATE
				ElectionPositionName = parmElectionPositionName
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionPositionUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionPositionUpdate` $$
CREATE PROCEDURE `ElectionPositionUpdate` (
	 IN parmPositionTypeId tinyint(3),
	 IN parmElectionPositionName varchar(25)
)

	BEGIN 

	UPDATE ElectionPosition SET 
				ElectionPositionName = parmElectionPositionName
		WHERE
			PositionTypeId = parmPositionTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionPositionAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionPositionAdd` $$
CREATE PROCEDURE `ElectionPositionAdd` (
	 IN parmPositionTypeId tinyint(3),
	 IN parmElectionPositionName varchar(25),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ElectionPosition( PositionTypeId, ElectionPositionName)
		VALUES (parmPositionTypeId,parmElectionPositionName);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionPositionSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionPositionSelect` $$
CREATE PROCEDURE `ElectionPositionSelect` (
	 IN parmPositionTypeId tinyint(3)
)

	BEGIN 

		SELECT  PositionTypeId, ElectionPositionName FROM ElectionPosition WHERE
			PositionTypeId = parmPositionTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionVoterAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionVoterAdd` $$
CREATE PROCEDURE `ElectionVoterAdd` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ElectionVoter( ElectionId, UserId)
		VALUES (parmElectionId,parmUserId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionVoterSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionVoterSelect` $$
CREATE PROCEDURE `ElectionVoterSelect` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  ElectionId, UserId FROM ElectionVoter WHERE
			ElectionId = parmElectionId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionVotingAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionVotingAddUpdate` $$
CREATE PROCEDURE `ElectionVotingAddUpdate` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmScore mediumint(9),
	 IN parmCountryId char(2),
	 IN parmElectionResult char(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO ElectionVoting( ElectionId, UserId, Score, CountryId, ElectionResult)
		VALUES (parmElectionId,parmUserId,parmScore,parmCountryId,parmElectionResult)


	ON DUPLICATE KEY UPDATE
				Score = parmScore,
				CountryId = parmCountryId,
				ElectionResult = parmElectionResult
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionVotingUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionVotingUpdate` $$
CREATE PROCEDURE `ElectionVotingUpdate` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmScore mediumint(9),
	 IN parmCountryId char(2),
	 IN parmElectionResult char(1)
)

	BEGIN 

	UPDATE ElectionVoting SET 
				Score = parmScore,
				CountryId = parmCountryId,
				ElectionResult = parmElectionResult
		WHERE
			ElectionId = parmElectionId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionVotingAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionVotingAdd` $$
CREATE PROCEDURE `ElectionVotingAdd` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11),
	 IN parmScore mediumint(9),
	 IN parmCountryId char(2),
	 IN parmElectionResult char(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ElectionVoting( ElectionId, UserId, Score, CountryId, ElectionResult)
		VALUES (parmElectionId,parmUserId,parmScore,parmCountryId,parmElectionResult);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ElectionVotingSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ElectionVotingSelect` $$
CREATE PROCEDURE `ElectionVotingSelect` (
	 IN parmElectionId mediumint(8),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  ElectionId, UserId, Score, CountryId, ElectionResult FROM ElectionVoting WHERE
			ElectionId = parmElectionId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EmailSubscriptionAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EmailSubscriptionAddUpdate` $$
CREATE PROCEDURE `EmailSubscriptionAddUpdate` (
	 IN parmUserId int(11),
	 IN parmAllowEventNotification tinyint(1),
	 IN parmAllowPromotions tinyint(1),
	 IN parmUpdatedAt datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO EmailSubscription( UserId, AllowEventNotification, AllowPromotions, UpdatedAt)
		VALUES (parmUserId,parmAllowEventNotification,parmAllowPromotions,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				AllowEventNotification = parmAllowEventNotification,
				AllowPromotions = parmAllowPromotions,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EmailSubscriptionUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EmailSubscriptionUpdate` $$
CREATE PROCEDURE `EmailSubscriptionUpdate` (
	 IN parmUserId int(11),
	 IN parmAllowEventNotification tinyint(1),
	 IN parmAllowPromotions tinyint(1),
	 IN parmUpdatedAt datetime(6)
)

	BEGIN 

	UPDATE EmailSubscription SET 
				AllowEventNotification = parmAllowEventNotification,
				AllowPromotions = parmAllowPromotions,
				UpdatedAt = parmUpdatedAt
		WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EmailSubscriptionAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EmailSubscriptionAdd` $$
CREATE PROCEDURE `EmailSubscriptionAdd` (
	 IN parmUserId int(11),
	 IN parmAllowEventNotification tinyint(1),
	 IN parmAllowPromotions tinyint(1),
	 IN parmUpdatedAt datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO EmailSubscription( UserId, AllowEventNotification, AllowPromotions, UpdatedAt)
		VALUES (parmUserId,parmAllowEventNotification,parmAllowPromotions,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure EmailSubscriptionSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `EmailSubscriptionSelect` $$
CREATE PROCEDURE `EmailSubscriptionSelect` (
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  UserId, AllowEventNotification, AllowPromotions, UpdatedAt FROM EmailSubscription WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FriendAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FriendAddUpdate` $$
CREATE PROCEDURE `FriendAddUpdate` (
	 IN parmFollowerUserId int(11),
	 IN parmFollowingUserId int(11),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO Friend( FollowerUserId, FollowingUserId, CreatedAt)
		VALUES (parmFollowerUserId,parmFollowingUserId,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FriendUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FriendUpdate` $$
CREATE PROCEDURE `FriendUpdate` (
	 IN parmFollowerUserId int(11),
	 IN parmFollowingUserId int(11),
	 IN parmCreatedAt datetime
)

	BEGIN 

	UPDATE Friend SET 
				CreatedAt = parmCreatedAt
		WHERE
			FollowerUserId = parmFollowerUserId AND
		FollowingUserId = parmFollowingUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FriendAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FriendAdd` $$
CREATE PROCEDURE `FriendAdd` (
	 IN parmFollowerUserId int(11),
	 IN parmFollowingUserId int(11),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO Friend( FollowerUserId, FollowingUserId, CreatedAt)
		VALUES (parmFollowerUserId,parmFollowingUserId,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FriendSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FriendSelect` $$
CREATE PROCEDURE `FriendSelect` (
	 IN parmFollowerUserId int(11),
	 IN parmFollowingUserId int(11)
)

	BEGIN 

		SELECT  FollowerUserId, FollowingUserId, CreatedAt FROM Friend WHERE
			FollowerUserId = parmFollowerUserId AND
		FollowingUserId = parmFollowingUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FundTypeCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FundTypeCodeAddUpdate` $$
CREATE PROCEDURE `FundTypeCodeAddUpdate` (
	 IN parmFundType tinyint(3),
	 IN parmCode varchar(45),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO FundTypeCode( FundType, Code, ImageFont)
		VALUES (parmFundType,parmCode,parmImageFont)


	ON DUPLICATE KEY UPDATE
				Code = parmCode,
				ImageFont = parmImageFont
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FundTypeCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FundTypeCodeUpdate` $$
CREATE PROCEDURE `FundTypeCodeUpdate` (
	 IN parmFundType tinyint(3),
	 IN parmCode varchar(45),
	 IN parmImageFont varchar(50)
)

	BEGIN 

	UPDATE FundTypeCode SET 
				Code = parmCode,
				ImageFont = parmImageFont
		WHERE
			FundType = parmFundType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FundTypeCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FundTypeCodeAdd` $$
CREATE PROCEDURE `FundTypeCodeAdd` (
	 IN parmFundType tinyint(3),
	 IN parmCode varchar(45),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO FundTypeCode( FundType, Code, ImageFont)
		VALUES (parmFundType,parmCode,parmImageFont);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure FundTypeCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `FundTypeCodeSelect` $$
CREATE PROCEDURE `FundTypeCodeSelect` (
	 IN parmFundType tinyint(3)
)

	BEGIN 

		SELECT  FundType, Code, ImageFont FROM FundTypeCode WHERE
			FundType = parmFundType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GiftAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `GiftAddUpdate` $$
CREATE PROCEDURE `GiftAddUpdate` (
	 IN parmGiftId char(36),
	 IN parmFromId int(11),
	 IN parmToId int(11),
	 IN parmCash decimal(50,2),
	 IN parmGold decimal(50,2),
	 IN parmSilver decimal(50,2),
	 IN parmTaxAmount decimal(50,2),
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmMerchandiseValue decimal(50,2),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO Gift( GiftId, FromId, ToId, Cash, Gold, Silver, TaxAmount, MerchandiseTypeId, MerchandiseValue, CreatedAt)
		VALUES (parmGiftId,parmFromId,parmToId,parmCash,parmGold,parmSilver,parmTaxAmount,parmMerchandiseTypeId,parmMerchandiseValue,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				FromId = parmFromId,
				ToId = parmToId,
				Cash = parmCash,
				Gold = parmGold,
				Silver = parmSilver,
				TaxAmount = parmTaxAmount,
				MerchandiseTypeId = parmMerchandiseTypeId,
				MerchandiseValue = parmMerchandiseValue,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GiftUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `GiftUpdate` $$
CREATE PROCEDURE `GiftUpdate` (
	 IN parmGiftId char(36),
	 IN parmFromId int(11),
	 IN parmToId int(11),
	 IN parmCash decimal(50,2),
	 IN parmGold decimal(50,2),
	 IN parmSilver decimal(50,2),
	 IN parmTaxAmount decimal(50,2),
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmMerchandiseValue decimal(50,2),
	 IN parmCreatedAt datetime
)

	BEGIN 

	UPDATE Gift SET 
				FromId = parmFromId,
				ToId = parmToId,
				Cash = parmCash,
				Gold = parmGold,
				Silver = parmSilver,
				TaxAmount = parmTaxAmount,
				MerchandiseTypeId = parmMerchandiseTypeId,
				MerchandiseValue = parmMerchandiseValue,
				CreatedAt = parmCreatedAt
		WHERE
			GiftId = parmGiftId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GiftAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `GiftAdd` $$
CREATE PROCEDURE `GiftAdd` (
	 IN parmGiftId char(36),
	 IN parmFromId int(11),
	 IN parmToId int(11),
	 IN parmCash decimal(50,2),
	 IN parmGold decimal(50,2),
	 IN parmSilver decimal(50,2),
	 IN parmTaxAmount decimal(50,2),
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmMerchandiseValue decimal(50,2),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO Gift( GiftId, FromId, ToId, Cash, Gold, Silver, TaxAmount, MerchandiseTypeId, MerchandiseValue, CreatedAt)
		VALUES (parmGiftId,parmFromId,parmToId,parmCash,parmGold,parmSilver,parmTaxAmount,parmMerchandiseTypeId,parmMerchandiseValue,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure GiftSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `GiftSelect` $$
CREATE PROCEDURE `GiftSelect` (
	 IN parmGiftId char(36)
)

	BEGIN 

		SELECT  GiftId, FromId, ToId, Cash, Gold, Silver, TaxAmount, MerchandiseTypeId, MerchandiseValue, CreatedAt FROM Gift WHERE
			GiftId = parmGiftId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure InJailAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `InJailAdd` $$
CREATE PROCEDURE `InJailAdd` (
	 IN parmUserId int(11),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO InJail( UserId)
		VALUES (parmUserId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure InJailSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `InJailSelect` $$
CREATE PROCEDURE `InJailSelect` (
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  UserId FROM InJail WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure IndustryCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `IndustryCodeAddUpdate` $$
CREATE PROCEDURE `IndustryCodeAddUpdate` (
	 IN parmIndustryId tinyint(3),
	 IN parmIndustryName varchar(100),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO IndustryCode( IndustryId, IndustryName, ImageFont)
		VALUES (parmIndustryId,parmIndustryName,parmImageFont)


	ON DUPLICATE KEY UPDATE
				IndustryName = parmIndustryName,
				ImageFont = parmImageFont
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure IndustryCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `IndustryCodeUpdate` $$
CREATE PROCEDURE `IndustryCodeUpdate` (
	 IN parmIndustryId tinyint(3),
	 IN parmIndustryName varchar(100),
	 IN parmImageFont varchar(50)
)

	BEGIN 

	UPDATE IndustryCode SET 
				IndustryName = parmIndustryName,
				ImageFont = parmImageFont
		WHERE
			IndustryId = parmIndustryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure IndustryCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `IndustryCodeAdd` $$
CREATE PROCEDURE `IndustryCodeAdd` (
	 IN parmIndustryId tinyint(3),
	 IN parmIndustryName varchar(100),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO IndustryCode( IndustryId, IndustryName, ImageFont)
		VALUES (parmIndustryId,parmIndustryName,parmImageFont);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure IndustryCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `IndustryCodeSelect` $$
CREATE PROCEDURE `IndustryCodeSelect` (
	 IN parmIndustryId tinyint(3)
)

	BEGIN 

		SELECT  IndustryId, IndustryName, ImageFont FROM IndustryCode WHERE
			IndustryId = parmIndustryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCodeAddUpdate` $$
CREATE PROCEDURE `JobCodeAddUpdate` (
	 IN parmJobCodeId smallint(5),
	 IN parmTitle varchar(250),
	 IN parmJobType char(1),
	 IN parmDuration smallint(5),
	 IN parmCheckInDuration tinyint(2),
	 IN parmMinimumMatchScore int(11),
	 IN parmMaxHPW tinyint(3),
	 IN parmOverTimeRate decimal(4,2),
	 IN parmIndustryId tinyint(3),
	 IN parmReqMajorId smallint(3),
	 IN parmReqDegreeId tinyint(3),
	 IN parmIndustryExperience smallint(5),
	 IN parmJobExperience smallint(5),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO JobCode( JobCodeId, Title, JobType, Duration, CheckInDuration, MinimumMatchScore, MaxHPW, OverTimeRate, IndustryId, ReqMajorId, ReqDegreeId, IndustryExperience, JobExperience)
		VALUES (parmJobCodeId,parmTitle,parmJobType,parmDuration,parmCheckInDuration,parmMinimumMatchScore,parmMaxHPW,parmOverTimeRate,parmIndustryId,parmReqMajorId,parmReqDegreeId,parmIndustryExperience,parmJobExperience)


	ON DUPLICATE KEY UPDATE
				Title = parmTitle,
				JobType = parmJobType,
				Duration = parmDuration,
				CheckInDuration = parmCheckInDuration,
				MinimumMatchScore = parmMinimumMatchScore,
				MaxHPW = parmMaxHPW,
				OverTimeRate = parmOverTimeRate,
				IndustryId = parmIndustryId,
				ReqMajorId = parmReqMajorId,
				ReqDegreeId = parmReqDegreeId,
				IndustryExperience = parmIndustryExperience,
				JobExperience = parmJobExperience
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCodeUpdate` $$
CREATE PROCEDURE `JobCodeUpdate` (
	 IN parmJobCodeId smallint(5),
	 IN parmTitle varchar(250),
	 IN parmJobType char(1),
	 IN parmDuration smallint(5),
	 IN parmCheckInDuration tinyint(2),
	 IN parmMinimumMatchScore int(11),
	 IN parmMaxHPW tinyint(3),
	 IN parmOverTimeRate decimal(4,2),
	 IN parmIndustryId tinyint(3),
	 IN parmReqMajorId smallint(3),
	 IN parmReqDegreeId tinyint(3),
	 IN parmIndustryExperience smallint(5),
	 IN parmJobExperience smallint(5)
)

	BEGIN 

	UPDATE JobCode SET 
				Title = parmTitle,
				JobType = parmJobType,
				Duration = parmDuration,
				CheckInDuration = parmCheckInDuration,
				MinimumMatchScore = parmMinimumMatchScore,
				MaxHPW = parmMaxHPW,
				OverTimeRate = parmOverTimeRate,
				IndustryId = parmIndustryId,
				ReqMajorId = parmReqMajorId,
				ReqDegreeId = parmReqDegreeId,
				IndustryExperience = parmIndustryExperience,
				JobExperience = parmJobExperience
		WHERE
			JobCodeId = parmJobCodeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCodeAdd` $$
CREATE PROCEDURE `JobCodeAdd` (
	 IN parmJobCodeId smallint(5),
	 IN parmTitle varchar(250),
	 IN parmJobType char(1),
	 IN parmDuration smallint(5),
	 IN parmCheckInDuration tinyint(2),
	 IN parmMinimumMatchScore int(11),
	 IN parmMaxHPW tinyint(3),
	 IN parmOverTimeRate decimal(4,2),
	 IN parmIndustryId tinyint(3),
	 IN parmReqMajorId smallint(3),
	 IN parmReqDegreeId tinyint(3),
	 IN parmIndustryExperience smallint(5),
	 IN parmJobExperience smallint(5),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO JobCode( JobCodeId, Title, JobType, Duration, CheckInDuration, MinimumMatchScore, MaxHPW, OverTimeRate, IndustryId, ReqMajorId, ReqDegreeId, IndustryExperience, JobExperience)
		VALUES (parmJobCodeId,parmTitle,parmJobType,parmDuration,parmCheckInDuration,parmMinimumMatchScore,parmMaxHPW,parmOverTimeRate,parmIndustryId,parmReqMajorId,parmReqDegreeId,parmIndustryExperience,parmJobExperience);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCodeSelect` $$
CREATE PROCEDURE `JobCodeSelect` (
	 IN parmJobCodeId smallint(5)
)

	BEGIN 

		SELECT  JobCodeId, Title, JobType, Duration, CheckInDuration, MinimumMatchScore, MaxHPW, OverTimeRate, IndustryId, ReqMajorId, ReqDegreeId, IndustryExperience, JobExperience FROM JobCode WHERE
			JobCodeId = parmJobCodeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCountryAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCountryAddUpdate` $$
CREATE PROCEDURE `JobCountryAddUpdate` (
	 IN parmJobCodeId smallint(5),
	 IN parmCountryId char(2),
	 IN parmSalary decimal(50,2),
	 IN parmQuantityAvailable int(11),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO JobCountry( JobCodeId, CountryId, Salary, QuantityAvailable)
		VALUES (parmJobCodeId,parmCountryId,parmSalary,parmQuantityAvailable)


	ON DUPLICATE KEY UPDATE
				Salary = parmSalary,
				QuantityAvailable = parmQuantityAvailable
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCountryUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCountryUpdate` $$
CREATE PROCEDURE `JobCountryUpdate` (
	 IN parmJobCodeId smallint(5),
	 IN parmCountryId char(2),
	 IN parmSalary decimal(50,2),
	 IN parmQuantityAvailable int(11)
)

	BEGIN 

	UPDATE JobCountry SET 
				Salary = parmSalary,
				QuantityAvailable = parmQuantityAvailable
		WHERE
			JobCodeId = parmJobCodeId AND
		CountryId = parmCountryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCountryAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCountryAdd` $$
CREATE PROCEDURE `JobCountryAdd` (
	 IN parmJobCodeId smallint(5),
	 IN parmCountryId char(2),
	 IN parmSalary decimal(50,2),
	 IN parmQuantityAvailable int(11),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO JobCountry( JobCodeId, CountryId, Salary, QuantityAvailable)
		VALUES (parmJobCodeId,parmCountryId,parmSalary,parmQuantityAvailable);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobCountrySelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobCountrySelect` $$
CREATE PROCEDURE `JobCountrySelect` (
	 IN parmJobCodeId smallint(5),
	 IN parmCountryId char(2)
)

	BEGIN 

		SELECT  JobCodeId, CountryId, Salary, QuantityAvailable FROM JobCountry WHERE
			JobCodeId = parmJobCodeId AND
		CountryId = parmCountryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobPayCheckAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobPayCheckAddUpdate` $$
CREATE PROCEDURE `JobPayCheckAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmAmount decimal(50,2),
	 IN parmTax decimal(50,2),
	 IN parmPayDate datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO JobPayCheck( TaskId, UserId, Amount, Tax, PayDate)
		VALUES (parmTaskId,parmUserId,parmAmount,parmTax,parmPayDate)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				Amount = parmAmount,
				Tax = parmTax,
				PayDate = parmPayDate
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobPayCheckUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobPayCheckUpdate` $$
CREATE PROCEDURE `JobPayCheckUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmAmount decimal(50,2),
	 IN parmTax decimal(50,2),
	 IN parmPayDate datetime
)

	BEGIN 

	UPDATE JobPayCheck SET 
				UserId = parmUserId,
				Amount = parmAmount,
				Tax = parmTax,
				PayDate = parmPayDate
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobPayCheckAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobPayCheckAdd` $$
CREATE PROCEDURE `JobPayCheckAdd` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmAmount decimal(50,2),
	 IN parmTax decimal(50,2),
	 IN parmPayDate datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO JobPayCheck( TaskId, UserId, Amount, Tax, PayDate)
		VALUES (parmTaskId,parmUserId,parmAmount,parmTax,parmPayDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure JobPayCheckSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `JobPayCheckSelect` $$
CREATE PROCEDURE `JobPayCheckSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, UserId, Amount, Tax, PayDate FROM JobPayCheck WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure LeaderCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `LeaderCodeAddUpdate` $$
CREATE PROCEDURE `LeaderCodeAddUpdate` (
	 IN parmLeaderType tinyint(3),
	 IN parmCode varchar(25),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO LeaderCode( LeaderType, Code)
		VALUES (parmLeaderType,parmCode)


	ON DUPLICATE KEY UPDATE
				Code = parmCode
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure LeaderCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `LeaderCodeUpdate` $$
CREATE PROCEDURE `LeaderCodeUpdate` (
	 IN parmLeaderType tinyint(3),
	 IN parmCode varchar(25)
)

	BEGIN 

	UPDATE LeaderCode SET 
				Code = parmCode
		WHERE
			LeaderType = parmLeaderType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure LeaderCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `LeaderCodeAdd` $$
CREATE PROCEDURE `LeaderCodeAdd` (
	 IN parmLeaderType tinyint(3),
	 IN parmCode varchar(25),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO LeaderCode( LeaderType, Code)
		VALUES (parmLeaderType,parmCode);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure LeaderCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `LeaderCodeSelect` $$
CREATE PROCEDURE `LeaderCodeSelect` (
	 IN parmLeaderType tinyint(3)
)

	BEGIN 

		SELECT  LeaderType, Code FROM LeaderCode WHERE
			LeaderType = parmLeaderType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MajorCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MajorCodeAddUpdate` $$
CREATE PROCEDURE `MajorCodeAddUpdate` (
	 IN parmMajorId smallint(3),
	 IN parmMajorName varchar(50),
	 IN parmImageFont varchar(50),
	 IN parmDescription varchar(250),
	 IN parmMajorRank tinyint(3),
	 IN parmCost decimal(50,2),
	 IN parmDuration tinyint(3),
	 IN parmJobProbability decimal(5,2),
	 IN parmIndustryId tinyint(3),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO MajorCode( MajorId, MajorName, ImageFont, Description, MajorRank, Cost, Duration, JobProbability, IndustryId)
		VALUES (parmMajorId,parmMajorName,parmImageFont,parmDescription,parmMajorRank,parmCost,parmDuration,parmJobProbability,parmIndustryId)


	ON DUPLICATE KEY UPDATE
				MajorName = parmMajorName,
				ImageFont = parmImageFont,
				Description = parmDescription,
				MajorRank = parmMajorRank,
				Cost = parmCost,
				Duration = parmDuration,
				JobProbability = parmJobProbability,
				IndustryId = parmIndustryId
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MajorCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MajorCodeUpdate` $$
CREATE PROCEDURE `MajorCodeUpdate` (
	 IN parmMajorId smallint(3),
	 IN parmMajorName varchar(50),
	 IN parmImageFont varchar(50),
	 IN parmDescription varchar(250),
	 IN parmMajorRank tinyint(3),
	 IN parmCost decimal(50,2),
	 IN parmDuration tinyint(3),
	 IN parmJobProbability decimal(5,2),
	 IN parmIndustryId tinyint(3)
)

	BEGIN 

	UPDATE MajorCode SET 
				MajorName = parmMajorName,
				ImageFont = parmImageFont,
				Description = parmDescription,
				MajorRank = parmMajorRank,
				Cost = parmCost,
				Duration = parmDuration,
				JobProbability = parmJobProbability,
				IndustryId = parmIndustryId
		WHERE
			MajorId = parmMajorId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MajorCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MajorCodeAdd` $$
CREATE PROCEDURE `MajorCodeAdd` (
	 IN parmMajorId smallint(3),
	 IN parmMajorName varchar(50),
	 IN parmImageFont varchar(50),
	 IN parmDescription varchar(250),
	 IN parmMajorRank tinyint(3),
	 IN parmCost decimal(50,2),
	 IN parmDuration tinyint(3),
	 IN parmJobProbability decimal(5,2),
	 IN parmIndustryId tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO MajorCode( MajorId, MajorName, ImageFont, Description, MajorRank, Cost, Duration, JobProbability, IndustryId)
		VALUES (parmMajorId,parmMajorName,parmImageFont,parmDescription,parmMajorRank,parmCost,parmDuration,parmJobProbability,parmIndustryId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MajorCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MajorCodeSelect` $$
CREATE PROCEDURE `MajorCodeSelect` (
	 IN parmMajorId smallint(3)
)

	BEGIN 

		SELECT  MajorId, MajorName, ImageFont, Description, MajorRank, Cost, Duration, JobProbability, IndustryId FROM MajorCode WHERE
			MajorId = parmMajorId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MerchandiseTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MerchandiseTypeAddUpdate` $$
CREATE PROCEDURE `MerchandiseTypeAddUpdate` (
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(50,2),
	 IN parmResaleRate decimal(5,2),
	 IN parmRentalPrice decimal(25,2),
	 IN parmMerchandiseTypeCode tinyint(2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO MerchandiseType( MerchandiseTypeId, Name, Description, ImageFont, Cost, ResaleRate, RentalPrice, MerchandiseTypeCode)
		VALUES (parmMerchandiseTypeId,parmName,parmDescription,parmImageFont,parmCost,parmResaleRate,parmRentalPrice,parmMerchandiseTypeCode)


	ON DUPLICATE KEY UPDATE
				Name = parmName,
				Description = parmDescription,
				ImageFont = parmImageFont,
				Cost = parmCost,
				ResaleRate = parmResaleRate,
				RentalPrice = parmRentalPrice,
				MerchandiseTypeCode = parmMerchandiseTypeCode
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MerchandiseTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MerchandiseTypeUpdate` $$
CREATE PROCEDURE `MerchandiseTypeUpdate` (
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(50,2),
	 IN parmResaleRate decimal(5,2),
	 IN parmRentalPrice decimal(25,2),
	 IN parmMerchandiseTypeCode tinyint(2)
)

	BEGIN 

	UPDATE MerchandiseType SET 
				Name = parmName,
				Description = parmDescription,
				ImageFont = parmImageFont,
				Cost = parmCost,
				ResaleRate = parmResaleRate,
				RentalPrice = parmRentalPrice,
				MerchandiseTypeCode = parmMerchandiseTypeCode
		WHERE
			MerchandiseTypeId = parmMerchandiseTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MerchandiseTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MerchandiseTypeAdd` $$
CREATE PROCEDURE `MerchandiseTypeAdd` (
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(50,2),
	 IN parmResaleRate decimal(5,2),
	 IN parmRentalPrice decimal(25,2),
	 IN parmMerchandiseTypeCode tinyint(2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO MerchandiseType( MerchandiseTypeId, Name, Description, ImageFont, Cost, ResaleRate, RentalPrice, MerchandiseTypeCode)
		VALUES (parmMerchandiseTypeId,parmName,parmDescription,parmImageFont,parmCost,parmResaleRate,parmRentalPrice,parmMerchandiseTypeCode);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure MerchandiseTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `MerchandiseTypeSelect` $$
CREATE PROCEDURE `MerchandiseTypeSelect` (
	 IN parmMerchandiseTypeId smallint(4)
)

	BEGIN 

		SELECT  MerchandiseTypeId, Name, Description, ImageFont, Cost, ResaleRate, RentalPrice, MerchandiseTypeCode FROM MerchandiseType WHERE
			MerchandiseTypeId = parmMerchandiseTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NextLotteryDrawingAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NextLotteryDrawingAddUpdate` $$
CREATE PROCEDURE `NextLotteryDrawingAddUpdate` (
	 IN parmLotteryType char(1),
	 IN parmLotteryPrice decimal(5,2),
	 IN parmNextDrawingDate datetime,
	 IN parmDrawingId int(11),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO NextLotteryDrawing( LotteryType, LotteryPrice, NextDrawingDate, DrawingId)
		VALUES (parmLotteryType,parmLotteryPrice,parmNextDrawingDate,parmDrawingId)


	ON DUPLICATE KEY UPDATE
				LotteryPrice = parmLotteryPrice,
				NextDrawingDate = parmNextDrawingDate,
				DrawingId = parmDrawingId
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NextLotteryDrawingUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NextLotteryDrawingUpdate` $$
CREATE PROCEDURE `NextLotteryDrawingUpdate` (
	 IN parmLotteryType char(1),
	 IN parmLotteryPrice decimal(5,2),
	 IN parmNextDrawingDate datetime,
	 IN parmDrawingId int(11)
)

	BEGIN 

	UPDATE NextLotteryDrawing SET 
				LotteryPrice = parmLotteryPrice,
				NextDrawingDate = parmNextDrawingDate,
				DrawingId = parmDrawingId
		WHERE
			LotteryType = parmLotteryType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NextLotteryDrawingAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NextLotteryDrawingAdd` $$
CREATE PROCEDURE `NextLotteryDrawingAdd` (
	 IN parmLotteryType char(1),
	 IN parmLotteryPrice decimal(5,2),
	 IN parmNextDrawingDate datetime,
	 IN parmDrawingId int(11),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO NextLotteryDrawing( LotteryType, LotteryPrice, NextDrawingDate, DrawingId)
		VALUES (parmLotteryType,parmLotteryPrice,parmNextDrawingDate,parmDrawingId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NextLotteryDrawingSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NextLotteryDrawingSelect` $$
CREATE PROCEDURE `NextLotteryDrawingSelect` (
	 IN parmLotteryType char(1)
)

	BEGIN 

		SELECT  LotteryType, LotteryPrice, NextDrawingDate, DrawingId FROM NextLotteryDrawing WHERE
			LotteryType = parmLotteryType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NotificationTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NotificationTypeAddUpdate` $$
CREATE PROCEDURE `NotificationTypeAddUpdate` (
	 IN parmNotificationTypeId smallint(6),
	 IN parmShortDescription varchar(200),
	 IN parmEmailNotification tinyint(1),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO NotificationType( NotificationTypeId, ShortDescription, EmailNotification, Description, ImageFont)
		VALUES (parmNotificationTypeId,parmShortDescription,parmEmailNotification,parmDescription,parmImageFont)


	ON DUPLICATE KEY UPDATE
				ShortDescription = parmShortDescription,
				EmailNotification = parmEmailNotification,
				Description = parmDescription,
				ImageFont = parmImageFont
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NotificationTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NotificationTypeUpdate` $$
CREATE PROCEDURE `NotificationTypeUpdate` (
	 IN parmNotificationTypeId smallint(6),
	 IN parmShortDescription varchar(200),
	 IN parmEmailNotification tinyint(1),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50)
)

	BEGIN 

	UPDATE NotificationType SET 
				ShortDescription = parmShortDescription,
				EmailNotification = parmEmailNotification,
				Description = parmDescription,
				ImageFont = parmImageFont
		WHERE
			NotificationTypeId = parmNotificationTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NotificationTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NotificationTypeAdd` $$
CREATE PROCEDURE `NotificationTypeAdd` (
	 IN parmNotificationTypeId smallint(6),
	 IN parmShortDescription varchar(200),
	 IN parmEmailNotification tinyint(1),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO NotificationType( NotificationTypeId, ShortDescription, EmailNotification, Description, ImageFont)
		VALUES (parmNotificationTypeId,parmShortDescription,parmEmailNotification,parmDescription,parmImageFont);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure NotificationTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `NotificationTypeSelect` $$
CREATE PROCEDURE `NotificationTypeSelect` (
	 IN parmNotificationTypeId smallint(6)
)

	BEGIN 

		SELECT  NotificationTypeId, ShortDescription, EmailNotification, Description, ImageFont FROM NotificationType WHERE
			NotificationTypeId = parmNotificationTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyAgendaAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyAgendaAdd` $$
CREATE PROCEDURE `PartyAgendaAdd` (
	 IN parmAgendaTypeId smallint(3),
	 IN parmPartyId char(36),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PartyAgenda( AgendaTypeId, PartyId)
		VALUES (parmAgendaTypeId,parmPartyId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyAgendaSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyAgendaSelect` $$
CREATE PROCEDURE `PartyAgendaSelect` (
	 IN parmAgendaTypeId smallint(3),
	 IN parmPartyId char(36)
)

	BEGIN 

		SELECT  AgendaTypeId, PartyId FROM PartyAgenda WHERE
			AgendaTypeId = parmAgendaTypeId AND
		PartyId = parmPartyId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyCloseRequestAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyCloseRequestAddUpdate` $$
CREATE PROCEDURE `PartyCloseRequestAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PartyCloseRequest( TaskId, PartyId, UserId, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmUserId,parmRequestDate,parmStatus)


	ON DUPLICATE KEY UPDATE
				PartyId = parmPartyId,
				UserId = parmUserId,
				RequestDate = parmRequestDate,
				Status = parmStatus
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyCloseRequestUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyCloseRequestUpdate` $$
CREATE PROCEDURE `PartyCloseRequestUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1)
)

	BEGIN 

	UPDATE PartyCloseRequest SET 
				PartyId = parmPartyId,
				UserId = parmUserId,
				RequestDate = parmRequestDate,
				Status = parmStatus
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyCloseRequestAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyCloseRequestAdd` $$
CREATE PROCEDURE `PartyCloseRequestAdd` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PartyCloseRequest( TaskId, PartyId, UserId, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmUserId,parmRequestDate,parmStatus);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyCloseRequestSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyCloseRequestSelect` $$
CREATE PROCEDURE `PartyCloseRequestSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, PartyId, UserId, RequestDate, Status FROM PartyCloseRequest WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyEjectionAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyEjectionAddUpdate` $$
CREATE PROCEDURE `PartyEjectionAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmInitatorId int(11),
	 IN parmEjecteeId int(11),
	 IN parmEjecteeMemberType char(1),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PartyEjection( TaskId, PartyId, InitatorId, EjecteeId, EjecteeMemberType, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmInitatorId,parmEjecteeId,parmEjecteeMemberType,parmRequestDate,parmStatus)


	ON DUPLICATE KEY UPDATE
				PartyId = parmPartyId,
				InitatorId = parmInitatorId,
				EjecteeId = parmEjecteeId,
				EjecteeMemberType = parmEjecteeMemberType,
				RequestDate = parmRequestDate,
				Status = parmStatus
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyEjectionUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyEjectionUpdate` $$
CREATE PROCEDURE `PartyEjectionUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmInitatorId int(11),
	 IN parmEjecteeId int(11),
	 IN parmEjecteeMemberType char(1),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1)
)

	BEGIN 

	UPDATE PartyEjection SET 
				PartyId = parmPartyId,
				InitatorId = parmInitatorId,
				EjecteeId = parmEjecteeId,
				EjecteeMemberType = parmEjecteeMemberType,
				RequestDate = parmRequestDate,
				Status = parmStatus
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyEjectionAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyEjectionAdd` $$
CREATE PROCEDURE `PartyEjectionAdd` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmInitatorId int(11),
	 IN parmEjecteeId int(11),
	 IN parmEjecteeMemberType char(1),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PartyEjection( TaskId, PartyId, InitatorId, EjecteeId, EjecteeMemberType, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmInitatorId,parmEjecteeId,parmEjecteeMemberType,parmRequestDate,parmStatus);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyEjectionSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyEjectionSelect` $$
CREATE PROCEDURE `PartyEjectionSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, PartyId, InitatorId, EjecteeId, EjecteeMemberType, RequestDate, Status FROM PartyEjection WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyInviteAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyInviteAddUpdate` $$
CREATE PROCEDURE `PartyInviteAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId mediumint(8),
	 IN parmEmailId varchar(100),
	 IN parmMemberType char(1),
	 IN parmInvitationDate datetime,
	 IN parmStatus char(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PartyInvite( TaskId, PartyId, UserId, EmailId, MemberType, InvitationDate, Status)
		VALUES (parmTaskId,parmPartyId,parmUserId,parmEmailId,parmMemberType,parmInvitationDate,parmStatus)


	ON DUPLICATE KEY UPDATE
				PartyId = parmPartyId,
				UserId = parmUserId,
				EmailId = parmEmailId,
				MemberType = parmMemberType,
				InvitationDate = parmInvitationDate,
				Status = parmStatus
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyInviteUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyInviteUpdate` $$
CREATE PROCEDURE `PartyInviteUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId mediumint(8),
	 IN parmEmailId varchar(100),
	 IN parmMemberType char(1),
	 IN parmInvitationDate datetime,
	 IN parmStatus char(1)
)

	BEGIN 

	UPDATE PartyInvite SET 
				PartyId = parmPartyId,
				UserId = parmUserId,
				EmailId = parmEmailId,
				MemberType = parmMemberType,
				InvitationDate = parmInvitationDate,
				Status = parmStatus
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyInviteAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyInviteAdd` $$
CREATE PROCEDURE `PartyInviteAdd` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId mediumint(8),
	 IN parmEmailId varchar(100),
	 IN parmMemberType char(1),
	 IN parmInvitationDate datetime,
	 IN parmStatus char(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PartyInvite( TaskId, PartyId, UserId, EmailId, MemberType, InvitationDate, Status)
		VALUES (parmTaskId,parmPartyId,parmUserId,parmEmailId,parmMemberType,parmInvitationDate,parmStatus);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyInviteSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyInviteSelect` $$
CREATE PROCEDURE `PartyInviteSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, PartyId, UserId, EmailId, MemberType, InvitationDate, Status FROM PartyInvite WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyJoinRequestAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyJoinRequestAddUpdate` $$
CREATE PROCEDURE `PartyJoinRequestAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PartyJoinRequest( TaskId, PartyId, UserId, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmUserId,parmRequestDate,parmStatus)


	ON DUPLICATE KEY UPDATE
				PartyId = parmPartyId,
				UserId = parmUserId,
				RequestDate = parmRequestDate,
				Status = parmStatus
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyJoinRequestUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyJoinRequestUpdate` $$
CREATE PROCEDURE `PartyJoinRequestUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1)
)

	BEGIN 

	UPDATE PartyJoinRequest SET 
				PartyId = parmPartyId,
				UserId = parmUserId,
				RequestDate = parmRequestDate,
				Status = parmStatus
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyJoinRequestAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyJoinRequestAdd` $$
CREATE PROCEDURE `PartyJoinRequestAdd` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PartyJoinRequest( TaskId, PartyId, UserId, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmUserId,parmRequestDate,parmStatus);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyJoinRequestSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyJoinRequestSelect` $$
CREATE PROCEDURE `PartyJoinRequestSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, PartyId, UserId, RequestDate, Status FROM PartyJoinRequest WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyMemberAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyMemberAddUpdate` $$
CREATE PROCEDURE `PartyMemberAddUpdate` (
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmMemberType char(1),
	 IN parmMemberStatus char(1),
	 IN parmMemberStartDate datetime,
	 IN parmMemberEndDate datetime,
	 IN parmDonationAmount decimal(50,2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PartyMember( PartyId, UserId, MemberType, MemberStatus, MemberStartDate, MemberEndDate, DonationAmount)
		VALUES (parmPartyId,parmUserId,parmMemberType,parmMemberStatus,parmMemberStartDate,parmMemberEndDate,parmDonationAmount)


	ON DUPLICATE KEY UPDATE
				MemberStatus = parmMemberStatus,
				MemberEndDate = parmMemberEndDate,
				DonationAmount = parmDonationAmount
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyMemberUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyMemberUpdate` $$
CREATE PROCEDURE `PartyMemberUpdate` (
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmMemberType char(1),
	 IN parmMemberStatus char(1),
	 IN parmMemberStartDate datetime,
	 IN parmMemberEndDate datetime,
	 IN parmDonationAmount decimal(50,2)
)

	BEGIN 

	UPDATE PartyMember SET 
				MemberStatus = parmMemberStatus,
				MemberEndDate = parmMemberEndDate,
				DonationAmount = parmDonationAmount
		WHERE
			PartyId = parmPartyId AND
		UserId = parmUserId AND
		MemberType = parmMemberType AND
		MemberStartDate = parmMemberStartDate ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyMemberAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyMemberAdd` $$
CREATE PROCEDURE `PartyMemberAdd` (
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmMemberType char(1),
	 IN parmMemberStatus char(1),
	 IN parmMemberStartDate datetime,
	 IN parmMemberEndDate datetime,
	 IN parmDonationAmount decimal(50,2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PartyMember( PartyId, UserId, MemberType, MemberStatus, MemberStartDate, MemberEndDate, DonationAmount)
		VALUES (parmPartyId,parmUserId,parmMemberType,parmMemberStatus,parmMemberStartDate,parmMemberEndDate,parmDonationAmount);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyMemberSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyMemberSelect` $$
CREATE PROCEDURE `PartyMemberSelect` (
	 IN parmPartyId char(36),
	 IN parmUserId int(11),
	 IN parmMemberType char(1),
	 IN parmMemberStartDate datetime
)

	BEGIN 

		SELECT  PartyId, UserId, MemberType, MemberStatus, MemberStartDate, MemberEndDate, DonationAmount FROM PartyMember WHERE
			PartyId = parmPartyId AND
		UserId = parmUserId AND
		MemberType = parmMemberType AND
		MemberStartDate = parmMemberStartDate ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyNominationAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyNominationAddUpdate` $$
CREATE PROCEDURE `PartyNominationAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmInitatorId int(11),
	 IN parmNomineeId int(11),
	 IN parmNomineeIdMemberType char(1),
	 IN parmNominatingMemberType char(1),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PartyNomination( TaskId, PartyId, InitatorId, NomineeId, NomineeIdMemberType, NominatingMemberType, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmInitatorId,parmNomineeId,parmNomineeIdMemberType,parmNominatingMemberType,parmRequestDate,parmStatus)


	ON DUPLICATE KEY UPDATE
				PartyId = parmPartyId,
				InitatorId = parmInitatorId,
				NomineeId = parmNomineeId,
				NomineeIdMemberType = parmNomineeIdMemberType,
				NominatingMemberType = parmNominatingMemberType,
				RequestDate = parmRequestDate,
				Status = parmStatus
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyNominationUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyNominationUpdate` $$
CREATE PROCEDURE `PartyNominationUpdate` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmInitatorId int(11),
	 IN parmNomineeId int(11),
	 IN parmNomineeIdMemberType char(1),
	 IN parmNominatingMemberType char(1),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1)
)

	BEGIN 

	UPDATE PartyNomination SET 
				PartyId = parmPartyId,
				InitatorId = parmInitatorId,
				NomineeId = parmNomineeId,
				NomineeIdMemberType = parmNomineeIdMemberType,
				NominatingMemberType = parmNominatingMemberType,
				RequestDate = parmRequestDate,
				Status = parmStatus
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyNominationAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyNominationAdd` $$
CREATE PROCEDURE `PartyNominationAdd` (
	 IN parmTaskId char(36),
	 IN parmPartyId char(36),
	 IN parmInitatorId int(11),
	 IN parmNomineeId int(11),
	 IN parmNomineeIdMemberType char(1),
	 IN parmNominatingMemberType char(1),
	 IN parmRequestDate datetime(6),
	 IN parmStatus char(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PartyNomination( TaskId, PartyId, InitatorId, NomineeId, NomineeIdMemberType, NominatingMemberType, RequestDate, Status)
		VALUES (parmTaskId,parmPartyId,parmInitatorId,parmNomineeId,parmNomineeIdMemberType,parmNominatingMemberType,parmRequestDate,parmStatus);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PartyNominationSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PartyNominationSelect` $$
CREATE PROCEDURE `PartyNominationSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, PartyId, InitatorId, NomineeId, NomineeIdMemberType, NominatingMemberType, RequestDate, Status FROM PartyNomination WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveAddUpdate` $$
CREATE PROCEDURE `PickFiveAddUpdate` (
	 IN parmPickFiveId char(36),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmNumber4 tinyint(2),
	 IN parmNumber5 tinyint(2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PickFive( PickFiveId, DrawingId, UserId, Number1, Number2, Number3, Number4, Number5)
		VALUES (parmPickFiveId,parmDrawingId,parmUserId,parmNumber1,parmNumber2,parmNumber3,parmNumber4,parmNumber5)


	ON DUPLICATE KEY UPDATE
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3,
				Number4 = parmNumber4,
				Number5 = parmNumber5
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveUpdate` $$
CREATE PROCEDURE `PickFiveUpdate` (
	 IN parmPickFiveId char(36),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmNumber4 tinyint(2),
	 IN parmNumber5 tinyint(2)
)

	BEGIN 

	UPDATE PickFive SET 
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3,
				Number4 = parmNumber4,
				Number5 = parmNumber5
		WHERE
			PickFiveId = parmPickFiveId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveAdd` $$
CREATE PROCEDURE `PickFiveAdd` (
	 IN parmPickFiveId char(36),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmNumber4 tinyint(2),
	 IN parmNumber5 tinyint(2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PickFive( PickFiveId, DrawingId, UserId, Number1, Number2, Number3, Number4, Number5)
		VALUES (parmPickFiveId,parmDrawingId,parmUserId,parmNumber1,parmNumber2,parmNumber3,parmNumber4,parmNumber5);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveSelect` $$
CREATE PROCEDURE `PickFiveSelect` (
	 IN parmPickFiveId char(36)
)

	BEGIN 

		SELECT  PickFiveId, DrawingId, UserId, Number1, Number2, Number3, Number4, Number5 FROM PickFive WHERE
			PickFiveId = parmPickFiveId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinNumberAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinNumberAddUpdate` $$
CREATE PROCEDURE `PickFiveWinNumberAddUpdate` (
	 IN parmDrawingId int(11),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmNumber4 tinyint(2),
	 IN parmNumber5 tinyint(2),
	 IN parmDrawingDate datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PickFiveWinNumber( DrawingId, Number1, Number2, Number3, Number4, Number5, DrawingDate)
		VALUES (parmDrawingId,parmNumber1,parmNumber2,parmNumber3,parmNumber4,parmNumber5,parmDrawingDate)


	ON DUPLICATE KEY UPDATE
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3,
				Number4 = parmNumber4,
				Number5 = parmNumber5,
				DrawingDate = parmDrawingDate
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinNumberUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinNumberUpdate` $$
CREATE PROCEDURE `PickFiveWinNumberUpdate` (
	 IN parmDrawingId int(11),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmNumber4 tinyint(2),
	 IN parmNumber5 tinyint(2),
	 IN parmDrawingDate datetime
)

	BEGIN 

	UPDATE PickFiveWinNumber SET 
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3,
				Number4 = parmNumber4,
				Number5 = parmNumber5,
				DrawingDate = parmDrawingDate
		WHERE
			DrawingId = parmDrawingId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinNumberAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinNumberAdd` $$
CREATE PROCEDURE `PickFiveWinNumberAdd` (
	 IN parmDrawingId int(11),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmNumber4 tinyint(2),
	 IN parmNumber5 tinyint(2),
	 IN parmDrawingDate datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PickFiveWinNumber( DrawingId, Number1, Number2, Number3, Number4, Number5, DrawingDate)
		VALUES (parmDrawingId,parmNumber1,parmNumber2,parmNumber3,parmNumber4,parmNumber5,parmDrawingDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinNumberSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinNumberSelect` $$
CREATE PROCEDURE `PickFiveWinNumberSelect` (
	 IN parmDrawingId int(11)
)

	BEGIN 

		SELECT  DrawingId, Number1, Number2, Number3, Number4, Number5, DrawingDate FROM PickFiveWinNumber WHERE
			DrawingId = parmDrawingId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinnerAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinnerAddUpdate` $$
CREATE PROCEDURE `PickFiveWinnerAddUpdate` (
	 IN parmWinId int(11),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmAmount decimal(50,2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PickFiveWinner( WinId, DrawingId, UserId, Amount)
		VALUES (parmWinId,parmDrawingId,parmUserId,parmAmount)


	ON DUPLICATE KEY UPDATE
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Amount = parmAmount
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinnerUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinnerUpdate` $$
CREATE PROCEDURE `PickFiveWinnerUpdate` (
	 IN parmWinId int(11),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmAmount decimal(50,2)
)

	BEGIN 

	UPDATE PickFiveWinner SET 
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Amount = parmAmount
		WHERE
			WinId = parmWinId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinnerAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinnerAdd` $$
CREATE PROCEDURE `PickFiveWinnerAdd` (
	 IN parmWinId int(11),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmAmount decimal(50,2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PickFiveWinner( WinId, DrawingId, UserId, Amount)
		VALUES (parmWinId,parmDrawingId,parmUserId,parmAmount);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickFiveWinnerSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickFiveWinnerSelect` $$
CREATE PROCEDURE `PickFiveWinnerSelect` (
	 IN parmWinId int(11)
)

	BEGIN 

		SELECT  WinId, DrawingId, UserId, Amount FROM PickFiveWinner WHERE
			WinId = parmWinId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeAddUpdate` $$
CREATE PROCEDURE `PickThreeAddUpdate` (
	 IN parmPickThreeId char(36),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PickThree( PickThreeId, DrawingId, UserId, Number1, Number2, Number3)
		VALUES (parmPickThreeId,parmDrawingId,parmUserId,parmNumber1,parmNumber2,parmNumber3)


	ON DUPLICATE KEY UPDATE
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeUpdate` $$
CREATE PROCEDURE `PickThreeUpdate` (
	 IN parmPickThreeId char(36),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2)
)

	BEGIN 

	UPDATE PickThree SET 
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3
		WHERE
			PickThreeId = parmPickThreeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeAdd` $$
CREATE PROCEDURE `PickThreeAdd` (
	 IN parmPickThreeId char(36),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PickThree( PickThreeId, DrawingId, UserId, Number1, Number2, Number3)
		VALUES (parmPickThreeId,parmDrawingId,parmUserId,parmNumber1,parmNumber2,parmNumber3);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeSelect` $$
CREATE PROCEDURE `PickThreeSelect` (
	 IN parmPickThreeId char(36)
)

	BEGIN 

		SELECT  PickThreeId, DrawingId, UserId, Number1, Number2, Number3 FROM PickThree WHERE
			PickThreeId = parmPickThreeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinNumberAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinNumberAddUpdate` $$
CREATE PROCEDURE `PickThreeWinNumberAddUpdate` (
	 IN parmDrawingId int(11),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmDrawingDate datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PickThreeWinNumber( DrawingId, Number1, Number2, Number3, DrawingDate)
		VALUES (parmDrawingId,parmNumber1,parmNumber2,parmNumber3,parmDrawingDate)


	ON DUPLICATE KEY UPDATE
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3,
				DrawingDate = parmDrawingDate
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinNumberUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinNumberUpdate` $$
CREATE PROCEDURE `PickThreeWinNumberUpdate` (
	 IN parmDrawingId int(11),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmDrawingDate datetime
)

	BEGIN 

	UPDATE PickThreeWinNumber SET 
				Number1 = parmNumber1,
				Number2 = parmNumber2,
				Number3 = parmNumber3,
				DrawingDate = parmDrawingDate
		WHERE
			DrawingId = parmDrawingId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinNumberAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinNumberAdd` $$
CREATE PROCEDURE `PickThreeWinNumberAdd` (
	 IN parmDrawingId int(11),
	 IN parmNumber1 tinyint(2),
	 IN parmNumber2 tinyint(2),
	 IN parmNumber3 tinyint(2),
	 IN parmDrawingDate datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PickThreeWinNumber( DrawingId, Number1, Number2, Number3, DrawingDate)
		VALUES (parmDrawingId,parmNumber1,parmNumber2,parmNumber3,parmDrawingDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinNumberSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinNumberSelect` $$
CREATE PROCEDURE `PickThreeWinNumberSelect` (
	 IN parmDrawingId int(11)
)

	BEGIN 

		SELECT  DrawingId, Number1, Number2, Number3, DrawingDate FROM PickThreeWinNumber WHERE
			DrawingId = parmDrawingId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinnerAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinnerAddUpdate` $$
CREATE PROCEDURE `PickThreeWinnerAddUpdate` (
	 IN parmWinId int(11),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmAmount decimal(50,2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PickThreeWinner( WinId, DrawingId, UserId, Amount)
		VALUES (parmWinId,parmDrawingId,parmUserId,parmAmount)


	ON DUPLICATE KEY UPDATE
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Amount = parmAmount
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinnerUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinnerUpdate` $$
CREATE PROCEDURE `PickThreeWinnerUpdate` (
	 IN parmWinId int(11),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmAmount decimal(50,2)
)

	BEGIN 

	UPDATE PickThreeWinner SET 
				DrawingId = parmDrawingId,
				UserId = parmUserId,
				Amount = parmAmount
		WHERE
			WinId = parmWinId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinnerAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinnerAdd` $$
CREATE PROCEDURE `PickThreeWinnerAdd` (
	 IN parmWinId int(11),
	 IN parmDrawingId int(11),
	 IN parmUserId mediumint(8),
	 IN parmAmount decimal(50,2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PickThreeWinner( WinId, DrawingId, UserId, Amount)
		VALUES (parmWinId,parmDrawingId,parmUserId,parmAmount);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PickThreeWinnerSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PickThreeWinnerSelect` $$
CREATE PROCEDURE `PickThreeWinnerSelect` (
	 IN parmWinId int(11)
)

	BEGIN 

		SELECT  WinId, DrawingId, UserId, Amount FROM PickThreeWinner WHERE
			WinId = parmWinId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PoliticalPartyAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PoliticalPartyAddUpdate` $$
CREATE PROCEDURE `PoliticalPartyAddUpdate` (
	 IN parmPartyId char(36),
	 IN parmPartyName varchar(60),
	 IN parmPartyFounder int(11),
	 IN parmTotalValue decimal(60,2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmPartySize mediumint(8),
	 IN parmCoFounderSize mediumint(8),
	 IN parmLogoPictureId varchar(250),
	 IN parmMembershipFee decimal(10,2),
	 IN parmElectionVictory smallint(5),
	 IN parmMotto varchar(250),
	 IN parmStatus char(1),
	 IN parmCountryId char(2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PoliticalParty( PartyId, PartyName, PartyFounder, TotalValue, StartDate, EndDate, PartySize, CoFounderSize, LogoPictureId, MembershipFee, ElectionVictory, Motto, Status, CountryId)
		VALUES (parmPartyId,parmPartyName,parmPartyFounder,parmTotalValue,parmStartDate,parmEndDate,parmPartySize,parmCoFounderSize,parmLogoPictureId,parmMembershipFee,parmElectionVictory,parmMotto,parmStatus,parmCountryId)


	ON DUPLICATE KEY UPDATE
				PartyName = parmPartyName,
				PartyFounder = parmPartyFounder,
				TotalValue = parmTotalValue,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				PartySize = parmPartySize,
				CoFounderSize = parmCoFounderSize,
				LogoPictureId = parmLogoPictureId,
				MembershipFee = parmMembershipFee,
				ElectionVictory = parmElectionVictory,
				Motto = parmMotto,
				Status = parmStatus,
				CountryId = parmCountryId
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PoliticalPartyUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PoliticalPartyUpdate` $$
CREATE PROCEDURE `PoliticalPartyUpdate` (
	 IN parmPartyId char(36),
	 IN parmPartyName varchar(60),
	 IN parmPartyFounder int(11),
	 IN parmTotalValue decimal(60,2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmPartySize mediumint(8),
	 IN parmCoFounderSize mediumint(8),
	 IN parmLogoPictureId varchar(250),
	 IN parmMembershipFee decimal(10,2),
	 IN parmElectionVictory smallint(5),
	 IN parmMotto varchar(250),
	 IN parmStatus char(1),
	 IN parmCountryId char(2)
)

	BEGIN 

	UPDATE PoliticalParty SET 
				PartyName = parmPartyName,
				PartyFounder = parmPartyFounder,
				TotalValue = parmTotalValue,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				PartySize = parmPartySize,
				CoFounderSize = parmCoFounderSize,
				LogoPictureId = parmLogoPictureId,
				MembershipFee = parmMembershipFee,
				ElectionVictory = parmElectionVictory,
				Motto = parmMotto,
				Status = parmStatus,
				CountryId = parmCountryId
		WHERE
			PartyId = parmPartyId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PoliticalPartyAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PoliticalPartyAdd` $$
CREATE PROCEDURE `PoliticalPartyAdd` (
	 IN parmPartyId char(36),
	 IN parmPartyName varchar(60),
	 IN parmPartyFounder int(11),
	 IN parmTotalValue decimal(60,2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmPartySize mediumint(8),
	 IN parmCoFounderSize mediumint(8),
	 IN parmLogoPictureId varchar(250),
	 IN parmMembershipFee decimal(10,2),
	 IN parmElectionVictory smallint(5),
	 IN parmMotto varchar(250),
	 IN parmStatus char(1),
	 IN parmCountryId char(2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PoliticalParty( PartyId, PartyName, PartyFounder, TotalValue, StartDate, EndDate, PartySize, CoFounderSize, LogoPictureId, MembershipFee, ElectionVictory, Motto, Status, CountryId)
		VALUES (parmPartyId,parmPartyName,parmPartyFounder,parmTotalValue,parmStartDate,parmEndDate,parmPartySize,parmCoFounderSize,parmLogoPictureId,parmMembershipFee,parmElectionVictory,parmMotto,parmStatus,parmCountryId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PoliticalPartySelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PoliticalPartySelect` $$
CREATE PROCEDURE `PoliticalPartySelect` (
	 IN parmPartyId char(36)
)

	BEGIN 

		SELECT  PartyId, PartyName, PartyFounder, TotalValue, StartDate, EndDate, PartySize, CoFounderSize, LogoPictureId, MembershipFee, ElectionVictory, Motto, Status, CountryId FROM PoliticalParty WHERE
			PartyId = parmPartyId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostAddUpdate` $$
CREATE PROCEDURE `PostAddUpdate` (
	 IN parmUserId int(11),
	 IN parmCountryId char(2),
	 IN parmPartyId char(36),
	 IN parmPostId char(36),
	 IN parmPostContent longtext,
	 IN parmPostTitle text,
	 IN parmChildCommentCount mediumint(8),
	 IN parmCommentEnabled tinyint(1),
	 IN parmDigIt mediumint(8) unsigned,
	 IN parmCoolIt mediumint(8) unsigned,
	 IN parmImageName varchar(255),
	 IN parmIsApproved tinyint(1),
	 IN parmIsSpam tinyint(1),
	 IN parmIsDeleted tinyint(1),
	 IN parmParms longtext,
	 IN parmPostContentTypeId tinyint(3),
	 IN parmUpdatedAt datetime(6),
	 IN parmCreatedAt datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO Post( UserId, CountryId, PartyId, PostId, PostContent, PostTitle, ChildCommentCount, CommentEnabled, DigIt, CoolIt, ImageName, IsApproved, IsSpam, IsDeleted, Parms, PostContentTypeId, UpdatedAt, CreatedAt)
		VALUES (parmUserId,parmCountryId,parmPartyId,parmPostId,parmPostContent,parmPostTitle,parmChildCommentCount,parmCommentEnabled,parmDigIt,parmCoolIt,parmImageName,parmIsApproved,parmIsSpam,parmIsDeleted,parmParms,parmPostContentTypeId,parmUpdatedAt,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				CountryId = parmCountryId,
				PartyId = parmPartyId,
				PostContent = parmPostContent,
				PostTitle = parmPostTitle,
				ChildCommentCount = parmChildCommentCount,
				CommentEnabled = parmCommentEnabled,
				DigIt = parmDigIt,
				CoolIt = parmCoolIt,
				ImageName = parmImageName,
				IsApproved = parmIsApproved,
				IsSpam = parmIsSpam,
				IsDeleted = parmIsDeleted,
				Parms = parmParms,
				PostContentTypeId = parmPostContentTypeId,
				UpdatedAt = parmUpdatedAt,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostUpdate` $$
CREATE PROCEDURE `PostUpdate` (
	 IN parmUserId int(11),
	 IN parmCountryId char(2),
	 IN parmPartyId char(36),
	 IN parmPostId char(36),
	 IN parmPostContent longtext,
	 IN parmPostTitle text,
	 IN parmChildCommentCount mediumint(8),
	 IN parmCommentEnabled tinyint(1),
	 IN parmDigIt mediumint(8) unsigned,
	 IN parmCoolIt mediumint(8) unsigned,
	 IN parmImageName varchar(255),
	 IN parmIsApproved tinyint(1),
	 IN parmIsSpam tinyint(1),
	 IN parmIsDeleted tinyint(1),
	 IN parmParms longtext,
	 IN parmPostContentTypeId tinyint(3),
	 IN parmUpdatedAt datetime(6),
	 IN parmCreatedAt datetime(6)
)

	BEGIN 

	UPDATE Post SET 
				UserId = parmUserId,
				CountryId = parmCountryId,
				PartyId = parmPartyId,
				PostContent = parmPostContent,
				PostTitle = parmPostTitle,
				ChildCommentCount = parmChildCommentCount,
				CommentEnabled = parmCommentEnabled,
				DigIt = parmDigIt,
				CoolIt = parmCoolIt,
				ImageName = parmImageName,
				IsApproved = parmIsApproved,
				IsSpam = parmIsSpam,
				IsDeleted = parmIsDeleted,
				Parms = parmParms,
				PostContentTypeId = parmPostContentTypeId,
				UpdatedAt = parmUpdatedAt,
				CreatedAt = parmCreatedAt
		WHERE
			PostId = parmPostId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostAdd` $$
CREATE PROCEDURE `PostAdd` (
	 IN parmUserId int(11),
	 IN parmCountryId char(2),
	 IN parmPartyId char(36),
	 IN parmPostId char(36),
	 IN parmPostContent longtext,
	 IN parmPostTitle text,
	 IN parmChildCommentCount mediumint(8),
	 IN parmCommentEnabled tinyint(1),
	 IN parmDigIt mediumint(8) unsigned,
	 IN parmCoolIt mediumint(8) unsigned,
	 IN parmImageName varchar(255),
	 IN parmIsApproved tinyint(1),
	 IN parmIsSpam tinyint(1),
	 IN parmIsDeleted tinyint(1),
	 IN parmParms longtext,
	 IN parmPostContentTypeId tinyint(3),
	 IN parmUpdatedAt datetime(6),
	 IN parmCreatedAt datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO Post( UserId, CountryId, PartyId, PostId, PostContent, PostTitle, ChildCommentCount, CommentEnabled, DigIt, CoolIt, ImageName, IsApproved, IsSpam, IsDeleted, Parms, PostContentTypeId, UpdatedAt, CreatedAt)
		VALUES (parmUserId,parmCountryId,parmPartyId,parmPostId,parmPostContent,parmPostTitle,parmChildCommentCount,parmCommentEnabled,parmDigIt,parmCoolIt,parmImageName,parmIsApproved,parmIsSpam,parmIsDeleted,parmParms,parmPostContentTypeId,parmUpdatedAt,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostSelect` $$
CREATE PROCEDURE `PostSelect` (
	 IN parmPostId char(36)
)

	BEGIN 

		SELECT  UserId, CountryId, PartyId, PostId, PostContent, PostTitle, ChildCommentCount, CommentEnabled, DigIt, CoolIt, ImageName, IsApproved, IsSpam, IsDeleted, Parms, PostContentTypeId, UpdatedAt, CreatedAt FROM Post WHERE
			PostId = parmPostId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostCommentAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostCommentAddUpdate` $$
CREATE PROCEDURE `PostCommentAddUpdate` (
	 IN parmPostCommentId char(36),
	 IN parmUserId int(11),
	 IN parmPostId char(36),
	 IN parmParentCommentId char(36),
	 IN parmDigIt mediumint(8) unsigned,
	 IN parmCoolIt mediumint(8) unsigned,
	 IN parmChildCommentCount mediumint(9),
	 IN parmCommentText varchar(500),
	 IN parmIsApproved tinyint(1),
	 IN parmIsSpam tinyint(1),
	 IN parmIsDeleted tinyint(1),
	 IN parmCreatedAt datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PostComment( PostCommentId, UserId, PostId, ParentCommentId, DigIt, CoolIt, ChildCommentCount, CommentText, IsApproved, IsSpam, IsDeleted, CreatedAt)
		VALUES (parmPostCommentId,parmUserId,parmPostId,parmParentCommentId,parmDigIt,parmCoolIt,parmChildCommentCount,parmCommentText,parmIsApproved,parmIsSpam,parmIsDeleted,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				PostId = parmPostId,
				ParentCommentId = parmParentCommentId,
				DigIt = parmDigIt,
				CoolIt = parmCoolIt,
				ChildCommentCount = parmChildCommentCount,
				CommentText = parmCommentText,
				IsApproved = parmIsApproved,
				IsSpam = parmIsSpam,
				IsDeleted = parmIsDeleted,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostCommentUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostCommentUpdate` $$
CREATE PROCEDURE `PostCommentUpdate` (
	 IN parmPostCommentId char(36),
	 IN parmUserId int(11),
	 IN parmPostId char(36),
	 IN parmParentCommentId char(36),
	 IN parmDigIt mediumint(8) unsigned,
	 IN parmCoolIt mediumint(8) unsigned,
	 IN parmChildCommentCount mediumint(9),
	 IN parmCommentText varchar(500),
	 IN parmIsApproved tinyint(1),
	 IN parmIsSpam tinyint(1),
	 IN parmIsDeleted tinyint(1),
	 IN parmCreatedAt datetime(6)
)

	BEGIN 

	UPDATE PostComment SET 
				UserId = parmUserId,
				PostId = parmPostId,
				ParentCommentId = parmParentCommentId,
				DigIt = parmDigIt,
				CoolIt = parmCoolIt,
				ChildCommentCount = parmChildCommentCount,
				CommentText = parmCommentText,
				IsApproved = parmIsApproved,
				IsSpam = parmIsSpam,
				IsDeleted = parmIsDeleted,
				CreatedAt = parmCreatedAt
		WHERE
			PostCommentId = parmPostCommentId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostCommentAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostCommentAdd` $$
CREATE PROCEDURE `PostCommentAdd` (
	 IN parmPostCommentId char(36),
	 IN parmUserId int(11),
	 IN parmPostId char(36),
	 IN parmParentCommentId char(36),
	 IN parmDigIt mediumint(8) unsigned,
	 IN parmCoolIt mediumint(8) unsigned,
	 IN parmChildCommentCount mediumint(9),
	 IN parmCommentText varchar(500),
	 IN parmIsApproved tinyint(1),
	 IN parmIsSpam tinyint(1),
	 IN parmIsDeleted tinyint(1),
	 IN parmCreatedAt datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PostComment( PostCommentId, UserId, PostId, ParentCommentId, DigIt, CoolIt, ChildCommentCount, CommentText, IsApproved, IsSpam, IsDeleted, CreatedAt)
		VALUES (parmPostCommentId,parmUserId,parmPostId,parmParentCommentId,parmDigIt,parmCoolIt,parmChildCommentCount,parmCommentText,parmIsApproved,parmIsSpam,parmIsDeleted,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostCommentSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostCommentSelect` $$
CREATE PROCEDURE `PostCommentSelect` (
	 IN parmPostCommentId char(36)
)

	BEGIN 

		SELECT  PostCommentId, UserId, PostId, ParentCommentId, DigIt, CoolIt, ChildCommentCount, CommentText, IsApproved, IsSpam, IsDeleted, CreatedAt FROM PostComment WHERE
			PostCommentId = parmPostCommentId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostContentTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostContentTypeAddUpdate` $$
CREATE PROCEDURE `PostContentTypeAddUpdate` (
	 IN parmPostContentTypeId tinyint(3),
	 IN parmShortDescription varchar(200),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	 IN parmFontCss varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PostContentType( PostContentTypeId, ShortDescription, Description, ImageFont, FontCss)
		VALUES (parmPostContentTypeId,parmShortDescription,parmDescription,parmImageFont,parmFontCss)


	ON DUPLICATE KEY UPDATE
				ShortDescription = parmShortDescription,
				Description = parmDescription,
				ImageFont = parmImageFont,
				FontCss = parmFontCss
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostContentTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostContentTypeUpdate` $$
CREATE PROCEDURE `PostContentTypeUpdate` (
	 IN parmPostContentTypeId tinyint(3),
	 IN parmShortDescription varchar(200),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	 IN parmFontCss varchar(50)
)

	BEGIN 

	UPDATE PostContentType SET 
				ShortDescription = parmShortDescription,
				Description = parmDescription,
				ImageFont = parmImageFont,
				FontCss = parmFontCss
		WHERE
			PostContentTypeId = parmPostContentTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostContentTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostContentTypeAdd` $$
CREATE PROCEDURE `PostContentTypeAdd` (
	 IN parmPostContentTypeId tinyint(3),
	 IN parmShortDescription varchar(200),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	 IN parmFontCss varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PostContentType( PostContentTypeId, ShortDescription, Description, ImageFont, FontCss)
		VALUES (parmPostContentTypeId,parmShortDescription,parmDescription,parmImageFont,parmFontCss);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostContentTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostContentTypeSelect` $$
CREATE PROCEDURE `PostContentTypeSelect` (
	 IN parmPostContentTypeId tinyint(3)
)

	BEGIN 

		SELECT  PostContentTypeId, ShortDescription, Description, ImageFont, FontCss FROM PostContentType WHERE
			PostContentTypeId = parmPostContentTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostTagAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostTagAddUpdate` $$
CREATE PROCEDURE `PostTagAddUpdate` (
	 IN parmPostTagId int(10),
	 IN parmPostId char(36),
	 IN parmTopicTagId int(10),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PostTag( PostTagId, PostId, TopicTagId, CreatedAt, UpdatedAt)
		VALUES (parmPostTagId,parmPostId,parmTopicTagId,parmCreatedAt,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				PostId = parmPostId,
				TopicTagId = parmTopicTagId,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostTagUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostTagUpdate` $$
CREATE PROCEDURE `PostTagUpdate` (
	 IN parmPostTagId int(10),
	 IN parmPostId char(36),
	 IN parmTopicTagId int(10),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp
)

	BEGIN 

	UPDATE PostTag SET 
				PostId = parmPostId,
				TopicTagId = parmTopicTagId,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
		WHERE
			PostTagId = parmPostTagId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostTagAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostTagAdd` $$
CREATE PROCEDURE `PostTagAdd` (
	 IN parmPostTagId int(10),
	 IN parmPostId char(36),
	 IN parmTopicTagId int(10),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PostTag( PostTagId, PostId, TopicTagId, CreatedAt, UpdatedAt)
		VALUES (parmPostTagId,parmPostId,parmTopicTagId,parmCreatedAt,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostTagSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostTagSelect` $$
CREATE PROCEDURE `PostTagSelect` (
	 IN parmPostTagId int(10)
)

	BEGIN 

		SELECT  PostTagId, PostId, TopicTagId, CreatedAt, UpdatedAt FROM PostTag WHERE
			PostTagId = parmPostTagId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostUserACLAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostUserACLAddUpdate` $$
CREATE PROCEDURE `PostUserACLAddUpdate` (
	 IN parmPostId bigint(20),
	 IN parmUserId int(11),
	 IN parmAccessType tinyint(3),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PostUserACL( PostId, UserId, AccessType, CreatedAt, UpdatedAt)
		VALUES (parmPostId,parmUserId,parmAccessType,parmCreatedAt,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				AccessType = parmAccessType,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostUserACLUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostUserACLUpdate` $$
CREATE PROCEDURE `PostUserACLUpdate` (
	 IN parmPostId bigint(20),
	 IN parmUserId int(11),
	 IN parmAccessType tinyint(3),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp
)

	BEGIN 

	UPDATE PostUserACL SET 
				AccessType = parmAccessType,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
		WHERE
			PostId = parmPostId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostUserACLAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostUserACLAdd` $$
CREATE PROCEDURE `PostUserACLAdd` (
	 IN parmPostId bigint(20),
	 IN parmUserId int(11),
	 IN parmAccessType tinyint(3),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PostUserACL( PostId, UserId, AccessType, CreatedAt, UpdatedAt)
		VALUES (parmPostId,parmUserId,parmAccessType,parmCreatedAt,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostUserACLSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostUserACLSelect` $$
CREATE PROCEDURE `PostUserACLSelect` (
	 IN parmPostId bigint(20),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  PostId, UserId, AccessType, CreatedAt, UpdatedAt FROM PostUserACL WHERE
			PostId = parmPostId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostWebContentAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostWebContentAddUpdate` $$
CREATE PROCEDURE `PostWebContentAddUpdate` (
	 IN parmPostWebContentId int(10),
	 IN parmUserId int(11),
	 IN parmPostId char(36),
	 IN parmContent text,
	 IN parmTitle varchar(255),
	 IN parmUri varchar(255),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO PostWebContent( PostWebContentId, UserId, PostId, Content, Title, Uri, CreatedAt, UpdatedAt)
		VALUES (parmPostWebContentId,parmUserId,parmPostId,parmContent,parmTitle,parmUri,parmCreatedAt,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				PostId = parmPostId,
				Content = parmContent,
				Title = parmTitle,
				Uri = parmUri,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostWebContentUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostWebContentUpdate` $$
CREATE PROCEDURE `PostWebContentUpdate` (
	 IN parmPostWebContentId int(10),
	 IN parmUserId int(11),
	 IN parmPostId char(36),
	 IN parmContent text,
	 IN parmTitle varchar(255),
	 IN parmUri varchar(255),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp
)

	BEGIN 

	UPDATE PostWebContent SET 
				UserId = parmUserId,
				PostId = parmPostId,
				Content = parmContent,
				Title = parmTitle,
				Uri = parmUri,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
		WHERE
			PostWebContentId = parmPostWebContentId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostWebContentAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostWebContentAdd` $$
CREATE PROCEDURE `PostWebContentAdd` (
	 IN parmPostWebContentId int(10),
	 IN parmUserId int(11),
	 IN parmPostId char(36),
	 IN parmContent text,
	 IN parmTitle varchar(255),
	 IN parmUri varchar(255),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO PostWebContent( PostWebContentId, UserId, PostId, Content, Title, Uri, CreatedAt, UpdatedAt)
		VALUES (parmPostWebContentId,parmUserId,parmPostId,parmContent,parmTitle,parmUri,parmCreatedAt,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure PostWebContentSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `PostWebContentSelect` $$
CREATE PROCEDURE `PostWebContentSelect` (
	 IN parmPostWebContentId int(10)
)

	BEGIN 

		SELECT  PostWebContentId, UserId, PostId, Content, Title, Uri, CreatedAt, UpdatedAt FROM PostWebContent WHERE
			PostWebContentId = parmPostWebContentId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ProvinceCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ProvinceCodeAddUpdate` $$
CREATE PROCEDURE `ProvinceCodeAddUpdate` (
	 IN parmProvinceId smallint(5),
	 IN parmProvince varchar(25),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO ProvinceCode( ProvinceId, Province)
		VALUES (parmProvinceId,parmProvince)


	ON DUPLICATE KEY UPDATE
				Province = parmProvince
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ProvinceCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ProvinceCodeUpdate` $$
CREATE PROCEDURE `ProvinceCodeUpdate` (
	 IN parmProvinceId smallint(5),
	 IN parmProvince varchar(25)
)

	BEGIN 

	UPDATE ProvinceCode SET 
				Province = parmProvince
		WHERE
			ProvinceId = parmProvinceId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ProvinceCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ProvinceCodeAdd` $$
CREATE PROCEDURE `ProvinceCodeAdd` (
	 IN parmProvinceId smallint(5),
	 IN parmProvince varchar(25),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO ProvinceCode( ProvinceId, Province)
		VALUES (parmProvinceId,parmProvince);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure ProvinceCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `ProvinceCodeSelect` $$
CREATE PROCEDURE `ProvinceCodeSelect` (
	 IN parmProvinceId smallint(5)
)

	BEGIN 

		SELECT  ProvinceId, Province FROM ProvinceCode WHERE
			ProvinceId = parmProvinceId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure RequestWarKeyAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `RequestWarKeyAddUpdate` $$
CREATE PROCEDURE `RequestWarKeyAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmRequestingCountryId char(2),
	 IN parmTaregtCountryId char(2),
	 IN parmRequestingUserId int(11),
	 IN parmRequestedAt datetime,
	 IN parmApprovalStatus char(1),
	 IN parmWarStatus char(1),
	 IN parmWiningCountryId char(2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO RequestWarKey( TaskId, RequestingCountryId, TaregtCountryId, RequestingUserId, RequestedAt, ApprovalStatus, WarStatus, WiningCountryId)
		VALUES (parmTaskId,parmRequestingCountryId,parmTaregtCountryId,parmRequestingUserId,parmRequestedAt,parmApprovalStatus,parmWarStatus,parmWiningCountryId)


	ON DUPLICATE KEY UPDATE
				RequestingCountryId = parmRequestingCountryId,
				TaregtCountryId = parmTaregtCountryId,
				RequestingUserId = parmRequestingUserId,
				RequestedAt = parmRequestedAt,
				ApprovalStatus = parmApprovalStatus,
				WarStatus = parmWarStatus,
				WiningCountryId = parmWiningCountryId
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure RequestWarKeyUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `RequestWarKeyUpdate` $$
CREATE PROCEDURE `RequestWarKeyUpdate` (
	 IN parmTaskId char(36),
	 IN parmRequestingCountryId char(2),
	 IN parmTaregtCountryId char(2),
	 IN parmRequestingUserId int(11),
	 IN parmRequestedAt datetime,
	 IN parmApprovalStatus char(1),
	 IN parmWarStatus char(1),
	 IN parmWiningCountryId char(2)
)

	BEGIN 

	UPDATE RequestWarKey SET 
				RequestingCountryId = parmRequestingCountryId,
				TaregtCountryId = parmTaregtCountryId,
				RequestingUserId = parmRequestingUserId,
				RequestedAt = parmRequestedAt,
				ApprovalStatus = parmApprovalStatus,
				WarStatus = parmWarStatus,
				WiningCountryId = parmWiningCountryId
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure RequestWarKeyAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `RequestWarKeyAdd` $$
CREATE PROCEDURE `RequestWarKeyAdd` (
	 IN parmTaskId char(36),
	 IN parmRequestingCountryId char(2),
	 IN parmTaregtCountryId char(2),
	 IN parmRequestingUserId int(11),
	 IN parmRequestedAt datetime,
	 IN parmApprovalStatus char(1),
	 IN parmWarStatus char(1),
	 IN parmWiningCountryId char(2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO RequestWarKey( TaskId, RequestingCountryId, TaregtCountryId, RequestingUserId, RequestedAt, ApprovalStatus, WarStatus, WiningCountryId)
		VALUES (parmTaskId,parmRequestingCountryId,parmTaregtCountryId,parmRequestingUserId,parmRequestedAt,parmApprovalStatus,parmWarStatus,parmWiningCountryId);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure RequestWarKeySelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `RequestWarKeySelect` $$
CREATE PROCEDURE `RequestWarKeySelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, RequestingCountryId, TaregtCountryId, RequestingUserId, RequestedAt, ApprovalStatus, WarStatus, WiningCountryId FROM RequestWarKey WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SlotMachineThreeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SlotMachineThreeAddUpdate` $$
CREATE PROCEDURE `SlotMachineThreeAddUpdate` (
	 IN parmMatchNumber tinyint(2),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO SlotMachineThree( MatchNumber, ImageFont)
		VALUES (parmMatchNumber,parmImageFont)


	ON DUPLICATE KEY UPDATE
				ImageFont = parmImageFont
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SlotMachineThreeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SlotMachineThreeUpdate` $$
CREATE PROCEDURE `SlotMachineThreeUpdate` (
	 IN parmMatchNumber tinyint(2),
	 IN parmImageFont varchar(50)
)

	BEGIN 

	UPDATE SlotMachineThree SET 
				ImageFont = parmImageFont
		WHERE
			MatchNumber = parmMatchNumber ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SlotMachineThreeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SlotMachineThreeAdd` $$
CREATE PROCEDURE `SlotMachineThreeAdd` (
	 IN parmMatchNumber tinyint(2),
	 IN parmImageFont varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO SlotMachineThree( MatchNumber, ImageFont)
		VALUES (parmMatchNumber,parmImageFont);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SlotMachineThreeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SlotMachineThreeSelect` $$
CREATE PROCEDURE `SlotMachineThreeSelect` (
	 IN parmMatchNumber tinyint(2)
)

	BEGIN 

		SELECT  MatchNumber, ImageFont FROM SlotMachineThree WHERE
			MatchNumber = parmMatchNumber ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockAddUpdate` $$
CREATE PROCEDURE `StockAddUpdate` (
	 IN parmStockId smallint(5),
	 IN parmPreviousDayValue decimal(50,2),
	 IN parmCurrentValue decimal(50,2),
	 IN parmDayChange decimal(10,2),
	 IN parmDayChangePercent decimal(10,2),
	 IN parmStockName varchar(25),
	 IN parmImageFont varchar(50),
	 IN parmTicker varchar(5),
	 IN parmDescription varchar(255),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO Stock( StockId, PreviousDayValue, CurrentValue, DayChange, DayChangePercent, StockName, ImageFont, Ticker, Description, UpdatedAt)
		VALUES (parmStockId,parmPreviousDayValue,parmCurrentValue,parmDayChange,parmDayChangePercent,parmStockName,parmImageFont,parmTicker,parmDescription,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				PreviousDayValue = parmPreviousDayValue,
				CurrentValue = parmCurrentValue,
				DayChange = parmDayChange,
				DayChangePercent = parmDayChangePercent,
				StockName = parmStockName,
				ImageFont = parmImageFont,
				Ticker = parmTicker,
				Description = parmDescription,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockUpdate` $$
CREATE PROCEDURE `StockUpdate` (
	 IN parmStockId smallint(5),
	 IN parmPreviousDayValue decimal(50,2),
	 IN parmCurrentValue decimal(50,2),
	 IN parmDayChange decimal(10,2),
	 IN parmDayChangePercent decimal(10,2),
	 IN parmStockName varchar(25),
	 IN parmImageFont varchar(50),
	 IN parmTicker varchar(5),
	 IN parmDescription varchar(255),
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE Stock SET 
				PreviousDayValue = parmPreviousDayValue,
				CurrentValue = parmCurrentValue,
				DayChange = parmDayChange,
				DayChangePercent = parmDayChangePercent,
				StockName = parmStockName,
				ImageFont = parmImageFont,
				Ticker = parmTicker,
				Description = parmDescription,
				UpdatedAt = parmUpdatedAt
		WHERE
			StockId = parmStockId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockAdd` $$
CREATE PROCEDURE `StockAdd` (
	 IN parmStockId smallint(5),
	 IN parmPreviousDayValue decimal(50,2),
	 IN parmCurrentValue decimal(50,2),
	 IN parmDayChange decimal(10,2),
	 IN parmDayChangePercent decimal(10,2),
	 IN parmStockName varchar(25),
	 IN parmImageFont varchar(50),
	 IN parmTicker varchar(5),
	 IN parmDescription varchar(255),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO Stock( StockId, PreviousDayValue, CurrentValue, DayChange, DayChangePercent, StockName, ImageFont, Ticker, Description, UpdatedAt)
		VALUES (parmStockId,parmPreviousDayValue,parmCurrentValue,parmDayChange,parmDayChangePercent,parmStockName,parmImageFont,parmTicker,parmDescription,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockSelect` $$
CREATE PROCEDURE `StockSelect` (
	 IN parmStockId smallint(5)
)

	BEGIN 

		SELECT  StockId, PreviousDayValue, CurrentValue, DayChange, DayChangePercent, StockName, ImageFont, Ticker, Description, UpdatedAt FROM Stock WHERE
			StockId = parmStockId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockHistoryAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockHistoryAddUpdate` $$
CREATE PROCEDURE `StockHistoryAddUpdate` (
	 IN parmHistoryId char(36),
	 IN parmStockId smallint(5),
	 IN parmCurrentValue decimal(50,2),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO StockHistory( HistoryId, StockId, CurrentValue, UpdatedAt)
		VALUES (parmHistoryId,parmStockId,parmCurrentValue,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				StockId = parmStockId,
				CurrentValue = parmCurrentValue,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockHistoryUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockHistoryUpdate` $$
CREATE PROCEDURE `StockHistoryUpdate` (
	 IN parmHistoryId char(36),
	 IN parmStockId smallint(5),
	 IN parmCurrentValue decimal(50,2),
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE StockHistory SET 
				StockId = parmStockId,
				CurrentValue = parmCurrentValue,
				UpdatedAt = parmUpdatedAt
		WHERE
			HistoryId = parmHistoryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockHistoryAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockHistoryAdd` $$
CREATE PROCEDURE `StockHistoryAdd` (
	 IN parmHistoryId char(36),
	 IN parmStockId smallint(5),
	 IN parmCurrentValue decimal(50,2),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO StockHistory( HistoryId, StockId, CurrentValue, UpdatedAt)
		VALUES (parmHistoryId,parmStockId,parmCurrentValue,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockHistorySelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockHistorySelect` $$
CREATE PROCEDURE `StockHistorySelect` (
	 IN parmHistoryId char(36)
)

	BEGIN 

		SELECT  HistoryId, StockId, CurrentValue, UpdatedAt FROM StockHistory WHERE
			HistoryId = parmHistoryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeAddUpdate` $$
CREATE PROCEDURE `StockTradeAddUpdate` (
	 IN parmTradeId char(36),
	 IN parmUserStockId char(36),
	 IN parmUserId int(11),
	 IN parmStockId smallint(5),
	 IN parmLeftUnit int(11),
	 IN parmInitialUnit int(11),
	 IN parmOfferPrice decimal(50,2),
	 IN parmRequestedAt datetime,
	 IN parmStatus char(1),
	 IN parmOrderType char(1),
	 IN parmTradeType char(1),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO StockTrade( TradeId, UserStockId, UserId, StockId, LeftUnit, InitialUnit, OfferPrice, RequestedAt, Status, OrderType, TradeType, UpdatedAt)
		VALUES (parmTradeId,parmUserStockId,parmUserId,parmStockId,parmLeftUnit,parmInitialUnit,parmOfferPrice,parmRequestedAt,parmStatus,parmOrderType,parmTradeType,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				UserStockId = parmUserStockId,
				UserId = parmUserId,
				StockId = parmStockId,
				LeftUnit = parmLeftUnit,
				InitialUnit = parmInitialUnit,
				OfferPrice = parmOfferPrice,
				RequestedAt = parmRequestedAt,
				Status = parmStatus,
				OrderType = parmOrderType,
				TradeType = parmTradeType,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeUpdate` $$
CREATE PROCEDURE `StockTradeUpdate` (
	 IN parmTradeId char(36),
	 IN parmUserStockId char(36),
	 IN parmUserId int(11),
	 IN parmStockId smallint(5),
	 IN parmLeftUnit int(11),
	 IN parmInitialUnit int(11),
	 IN parmOfferPrice decimal(50,2),
	 IN parmRequestedAt datetime,
	 IN parmStatus char(1),
	 IN parmOrderType char(1),
	 IN parmTradeType char(1),
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE StockTrade SET 
				UserStockId = parmUserStockId,
				UserId = parmUserId,
				StockId = parmStockId,
				LeftUnit = parmLeftUnit,
				InitialUnit = parmInitialUnit,
				OfferPrice = parmOfferPrice,
				RequestedAt = parmRequestedAt,
				Status = parmStatus,
				OrderType = parmOrderType,
				TradeType = parmTradeType,
				UpdatedAt = parmUpdatedAt
		WHERE
			TradeId = parmTradeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeAdd` $$
CREATE PROCEDURE `StockTradeAdd` (
	 IN parmTradeId char(36),
	 IN parmUserStockId char(36),
	 IN parmUserId int(11),
	 IN parmStockId smallint(5),
	 IN parmLeftUnit int(11),
	 IN parmInitialUnit int(11),
	 IN parmOfferPrice decimal(50,2),
	 IN parmRequestedAt datetime,
	 IN parmStatus char(1),
	 IN parmOrderType char(1),
	 IN parmTradeType char(1),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO StockTrade( TradeId, UserStockId, UserId, StockId, LeftUnit, InitialUnit, OfferPrice, RequestedAt, Status, OrderType, TradeType, UpdatedAt)
		VALUES (parmTradeId,parmUserStockId,parmUserId,parmStockId,parmLeftUnit,parmInitialUnit,parmOfferPrice,parmRequestedAt,parmStatus,parmOrderType,parmTradeType,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeSelect` $$
CREATE PROCEDURE `StockTradeSelect` (
	 IN parmTradeId char(36)
)

	BEGIN 

		SELECT  TradeId, UserStockId, UserId, StockId, LeftUnit, InitialUnit, OfferPrice, RequestedAt, Status, OrderType, TradeType, UpdatedAt FROM StockTrade WHERE
			TradeId = parmTradeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeHistoryAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeHistoryAddUpdate` $$
CREATE PROCEDURE `StockTradeHistoryAddUpdate` (
	 IN parmStockTradeHistoryId char(36),
	 IN parmBuyerId int(11),
	 IN parmSellerId int(11),
	 IN parmStockId smallint(5),
	 IN parmUnit int(11),
	 IN parmDealPrice decimal(50,2),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO StockTradeHistory( StockTradeHistoryId, BuyerId, SellerId, StockId, Unit, DealPrice, UpdatedAt)
		VALUES (parmStockTradeHistoryId,parmBuyerId,parmSellerId,parmStockId,parmUnit,parmDealPrice,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				BuyerId = parmBuyerId,
				SellerId = parmSellerId,
				StockId = parmStockId,
				Unit = parmUnit,
				DealPrice = parmDealPrice,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeHistoryUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeHistoryUpdate` $$
CREATE PROCEDURE `StockTradeHistoryUpdate` (
	 IN parmStockTradeHistoryId char(36),
	 IN parmBuyerId int(11),
	 IN parmSellerId int(11),
	 IN parmStockId smallint(5),
	 IN parmUnit int(11),
	 IN parmDealPrice decimal(50,2),
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE StockTradeHistory SET 
				BuyerId = parmBuyerId,
				SellerId = parmSellerId,
				StockId = parmStockId,
				Unit = parmUnit,
				DealPrice = parmDealPrice,
				UpdatedAt = parmUpdatedAt
		WHERE
			StockTradeHistoryId = parmStockTradeHistoryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeHistoryAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeHistoryAdd` $$
CREATE PROCEDURE `StockTradeHistoryAdd` (
	 IN parmStockTradeHistoryId char(36),
	 IN parmBuyerId int(11),
	 IN parmSellerId int(11),
	 IN parmStockId smallint(5),
	 IN parmUnit int(11),
	 IN parmDealPrice decimal(50,2),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO StockTradeHistory( StockTradeHistoryId, BuyerId, SellerId, StockId, Unit, DealPrice, UpdatedAt)
		VALUES (parmStockTradeHistoryId,parmBuyerId,parmSellerId,parmStockId,parmUnit,parmDealPrice,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure StockTradeHistorySelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `StockTradeHistorySelect` $$
CREATE PROCEDURE `StockTradeHistorySelect` (
	 IN parmStockTradeHistoryId char(36)
)

	BEGIN 

		SELECT  StockTradeHistoryId, BuyerId, SellerId, StockId, Unit, DealPrice, UpdatedAt FROM StockTradeHistory WHERE
			StockTradeHistoryId = parmStockTradeHistoryId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SuggestFriendAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SuggestFriendAddUpdate` $$
CREATE PROCEDURE `SuggestFriendAddUpdate` (
	 IN parmUserId int(11),
	 IN parmSuggestionUserId int(11),
	 IN parmMatchScore tinyint(3),
	 IN parmUserIgnore tinyint(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO SuggestFriend( UserId, SuggestionUserId, MatchScore, UserIgnore)
		VALUES (parmUserId,parmSuggestionUserId,parmMatchScore,parmUserIgnore)


	ON DUPLICATE KEY UPDATE
				MatchScore = parmMatchScore,
				UserIgnore = parmUserIgnore
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SuggestFriendUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SuggestFriendUpdate` $$
CREATE PROCEDURE `SuggestFriendUpdate` (
	 IN parmUserId int(11),
	 IN parmSuggestionUserId int(11),
	 IN parmMatchScore tinyint(3),
	 IN parmUserIgnore tinyint(1)
)

	BEGIN 

	UPDATE SuggestFriend SET 
				MatchScore = parmMatchScore,
				UserIgnore = parmUserIgnore
		WHERE
			UserId = parmUserId AND
		SuggestionUserId = parmSuggestionUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SuggestFriendAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SuggestFriendAdd` $$
CREATE PROCEDURE `SuggestFriendAdd` (
	 IN parmUserId int(11),
	 IN parmSuggestionUserId int(11),
	 IN parmMatchScore tinyint(3),
	 IN parmUserIgnore tinyint(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO SuggestFriend( UserId, SuggestionUserId, MatchScore, UserIgnore)
		VALUES (parmUserId,parmSuggestionUserId,parmMatchScore,parmUserIgnore);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure SuggestFriendSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `SuggestFriendSelect` $$
CREATE PROCEDURE `SuggestFriendSelect` (
	 IN parmUserId int(11),
	 IN parmSuggestionUserId int(11)
)

	BEGIN 

		SELECT  UserId, SuggestionUserId, MatchScore, UserIgnore FROM SuggestFriend WHERE
			UserId = parmUserId AND
		SuggestionUserId = parmSuggestionUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskReminderAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskReminderAddUpdate` $$
CREATE PROCEDURE `TaskReminderAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmReminderFrequency smallint(6),
	 IN parmReminderTransPort varchar(2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO TaskReminder( TaskId, ReminderFrequency, ReminderTransPort, StartDate, EndDate)
		VALUES (parmTaskId,parmReminderFrequency,parmReminderTransPort,parmStartDate,parmEndDate)


	ON DUPLICATE KEY UPDATE
				ReminderFrequency = parmReminderFrequency,
				ReminderTransPort = parmReminderTransPort,
				StartDate = parmStartDate,
				EndDate = parmEndDate
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskReminderUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskReminderUpdate` $$
CREATE PROCEDURE `TaskReminderUpdate` (
	 IN parmTaskId char(36),
	 IN parmReminderFrequency smallint(6),
	 IN parmReminderTransPort varchar(2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime
)

	BEGIN 

	UPDATE TaskReminder SET 
				ReminderFrequency = parmReminderFrequency,
				ReminderTransPort = parmReminderTransPort,
				StartDate = parmStartDate,
				EndDate = parmEndDate
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskReminderAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskReminderAdd` $$
CREATE PROCEDURE `TaskReminderAdd` (
	 IN parmTaskId char(36),
	 IN parmReminderFrequency smallint(6),
	 IN parmReminderTransPort varchar(2),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO TaskReminder( TaskId, ReminderFrequency, ReminderTransPort, StartDate, EndDate)
		VALUES (parmTaskId,parmReminderFrequency,parmReminderTransPort,parmStartDate,parmEndDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskReminderSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskReminderSelect` $$
CREATE PROCEDURE `TaskReminderSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, ReminderFrequency, ReminderTransPort, StartDate, EndDate FROM TaskReminder WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskTypeAddUpdate` $$
CREATE PROCEDURE `TaskTypeAddUpdate` (
	 IN parmTaskTypeId smallint(5),
	 IN parmShortDescription varchar(200),
	 IN parmDescription varchar(2000),
	 IN parmPicture varchar(255),
	 IN parmChoiceType tinyint(3),
	 IN parmMaxChoiceCount tinyint(3),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO TaskType( TaskTypeId, ShortDescription, Description, Picture, ChoiceType, MaxChoiceCount)
		VALUES (parmTaskTypeId,parmShortDescription,parmDescription,parmPicture,parmChoiceType,parmMaxChoiceCount)


	ON DUPLICATE KEY UPDATE
				ShortDescription = parmShortDescription,
				Description = parmDescription,
				Picture = parmPicture,
				ChoiceType = parmChoiceType,
				MaxChoiceCount = parmMaxChoiceCount
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskTypeUpdate` $$
CREATE PROCEDURE `TaskTypeUpdate` (
	 IN parmTaskTypeId smallint(5),
	 IN parmShortDescription varchar(200),
	 IN parmDescription varchar(2000),
	 IN parmPicture varchar(255),
	 IN parmChoiceType tinyint(3),
	 IN parmMaxChoiceCount tinyint(3)
)

	BEGIN 

	UPDATE TaskType SET 
				ShortDescription = parmShortDescription,
				Description = parmDescription,
				Picture = parmPicture,
				ChoiceType = parmChoiceType,
				MaxChoiceCount = parmMaxChoiceCount
		WHERE
			TaskTypeId = parmTaskTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskTypeAdd` $$
CREATE PROCEDURE `TaskTypeAdd` (
	 IN parmTaskTypeId smallint(5),
	 IN parmShortDescription varchar(200),
	 IN parmDescription varchar(2000),
	 IN parmPicture varchar(255),
	 IN parmChoiceType tinyint(3),
	 IN parmMaxChoiceCount tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO TaskType( TaskTypeId, ShortDescription, Description, Picture, ChoiceType, MaxChoiceCount)
		VALUES (parmTaskTypeId,parmShortDescription,parmDescription,parmPicture,parmChoiceType,parmMaxChoiceCount);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaskTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaskTypeSelect` $$
CREATE PROCEDURE `TaskTypeSelect` (
	 IN parmTaskTypeId smallint(5)
)

	BEGIN 

		SELECT  TaskTypeId, ShortDescription, Description, Picture, ChoiceType, MaxChoiceCount FROM TaskType WHERE
			TaskTypeId = parmTaskTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaxCodeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaxCodeAddUpdate` $$
CREATE PROCEDURE `TaxCodeAddUpdate` (
	 IN parmTaxType tinyint(3),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	 IN parmAllowEdit tinyint(1),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO TaxCode( TaxType, Description, ImageFont, AllowEdit)
		VALUES (parmTaxType,parmDescription,parmImageFont,parmAllowEdit)


	ON DUPLICATE KEY UPDATE
				Description = parmDescription,
				ImageFont = parmImageFont,
				AllowEdit = parmAllowEdit
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaxCodeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaxCodeUpdate` $$
CREATE PROCEDURE `TaxCodeUpdate` (
	 IN parmTaxType tinyint(3),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	 IN parmAllowEdit tinyint(1)
)

	BEGIN 

	UPDATE TaxCode SET 
				Description = parmDescription,
				ImageFont = parmImageFont,
				AllowEdit = parmAllowEdit
		WHERE
			TaxType = parmTaxType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaxCodeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaxCodeAdd` $$
CREATE PROCEDURE `TaxCodeAdd` (
	 IN parmTaxType tinyint(3),
	 IN parmDescription varchar(1000),
	 IN parmImageFont varchar(50),
	 IN parmAllowEdit tinyint(1),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO TaxCode( TaxType, Description, ImageFont, AllowEdit)
		VALUES (parmTaxType,parmDescription,parmImageFont,parmAllowEdit);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TaxCodeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TaxCodeSelect` $$
CREATE PROCEDURE `TaxCodeSelect` (
	 IN parmTaxType tinyint(3)
)

	BEGIN 

		SELECT  TaxType, Description, ImageFont, AllowEdit FROM TaxCode WHERE
			TaxType = parmTaxType ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TopicTagAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TopicTagAddUpdate` $$
CREATE PROCEDURE `TopicTagAddUpdate` (
	 IN parmTopicTagId int(10),
	 IN parmTag varchar(50),
	 IN parmTagCount int(5),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO TopicTag( TopicTagId, Tag, TagCount, CreatedAt, UpdatedAt)
		VALUES (parmTopicTagId,parmTag,parmTagCount,parmCreatedAt,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				TagCount = parmTagCount,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TopicTagUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TopicTagUpdate` $$
CREATE PROCEDURE `TopicTagUpdate` (
	 IN parmTopicTagId int(10),
	 IN parmTag varchar(50),
	 IN parmTagCount int(5),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp
)

	BEGIN 

	UPDATE TopicTag SET 
				TagCount = parmTagCount,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
		WHERE
			TopicTagId = parmTopicTagId AND
		Tag = parmTag ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TopicTagAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TopicTagAdd` $$
CREATE PROCEDURE `TopicTagAdd` (
	 IN parmTopicTagId int(10),
	 IN parmTag varchar(50),
	 IN parmTagCount int(5),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt timestamp,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO TopicTag( TopicTagId, Tag, TagCount, CreatedAt, UpdatedAt)
		VALUES (parmTopicTagId,parmTag,parmTagCount,parmCreatedAt,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure TopicTagSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `TopicTagSelect` $$
CREATE PROCEDURE `TopicTagSelect` (
	 IN parmTopicTagId int(10),
	 IN parmTag varchar(50)
)

	BEGIN 

		SELECT  TopicTagId, Tag, TagCount, CreatedAt, UpdatedAt FROM TopicTag WHERE
			TopicTagId = parmTopicTagId AND
		Tag = parmTag ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserActivityLogAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserActivityLogAddUpdate` $$
CREATE PROCEDURE `UserActivityLogAddUpdate` (
	 IN parmUserId int(11),
	 IN parmIPAddress varchar(45),
	 IN parmHit int(11),
	 IN parmLastLogin datetime,
	 IN parmFirstLogin datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserActivityLog( UserId, IPAddress, Hit, LastLogin, FirstLogin)
		VALUES (parmUserId,parmIPAddress,parmHit,parmLastLogin,parmFirstLogin)


	ON DUPLICATE KEY UPDATE
				Hit = parmHit,
				LastLogin = parmLastLogin,
				FirstLogin = parmFirstLogin
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserActivityLogUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserActivityLogUpdate` $$
CREATE PROCEDURE `UserActivityLogUpdate` (
	 IN parmUserId int(11),
	 IN parmIPAddress varchar(45),
	 IN parmHit int(11),
	 IN parmLastLogin datetime,
	 IN parmFirstLogin datetime
)

	BEGIN 

	UPDATE UserActivityLog SET 
				Hit = parmHit,
				LastLogin = parmLastLogin,
				FirstLogin = parmFirstLogin
		WHERE
			UserId = parmUserId AND
		IPAddress = parmIPAddress ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserActivityLogAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserActivityLogAdd` $$
CREATE PROCEDURE `UserActivityLogAdd` (
	 IN parmUserId int(11),
	 IN parmIPAddress varchar(45),
	 IN parmHit int(11),
	 IN parmLastLogin datetime,
	 IN parmFirstLogin datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserActivityLog( UserId, IPAddress, Hit, LastLogin, FirstLogin)
		VALUES (parmUserId,parmIPAddress,parmHit,parmLastLogin,parmFirstLogin);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserActivityLogSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserActivityLogSelect` $$
CREATE PROCEDURE `UserActivityLogSelect` (
	 IN parmUserId int(11),
	 IN parmIPAddress varchar(45)
)

	BEGIN 

		SELECT  UserId, IPAddress, Hit, LastLogin, FirstLogin FROM UserActivityLog WHERE
			UserId = parmUserId AND
		IPAddress = parmIPAddress ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserBankAccountAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserBankAccountAddUpdate` $$
CREATE PROCEDURE `UserBankAccountAddUpdate` (
	 IN parmUserId int(11),
	 IN parmCash decimal(50,2),
	 IN parmGold decimal(50,2),
	 IN parmSilver decimal(50,2),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserBankAccount( UserId, Cash, Gold, Silver, CreatedAt, UpdatedAt)
		VALUES (parmUserId,parmCash,parmGold,parmSilver,parmCreatedAt,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				Cash = parmCash,
				Gold = parmGold,
				Silver = parmSilver,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserBankAccountUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserBankAccountUpdate` $$
CREATE PROCEDURE `UserBankAccountUpdate` (
	 IN parmUserId int(11),
	 IN parmCash decimal(50,2),
	 IN parmGold decimal(50,2),
	 IN parmSilver decimal(50,2),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE UserBankAccount SET 
				Cash = parmCash,
				Gold = parmGold,
				Silver = parmSilver,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
		WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserBankAccountAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserBankAccountAdd` $$
CREATE PROCEDURE `UserBankAccountAdd` (
	 IN parmUserId int(11),
	 IN parmCash decimal(50,2),
	 IN parmGold decimal(50,2),
	 IN parmSilver decimal(50,2),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserBankAccount( UserId, Cash, Gold, Silver, CreatedAt, UpdatedAt)
		VALUES (parmUserId,parmCash,parmGold,parmSilver,parmCreatedAt,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserBankAccountSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserBankAccountSelect` $$
CREATE PROCEDURE `UserBankAccountSelect` (
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  UserId, Cash, Gold, Silver, CreatedAt, UpdatedAt FROM UserBankAccount WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserDigAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserDigAddUpdate` $$
CREATE PROCEDURE `UserDigAddUpdate` (
	 IN parmPostCommentId char(36),
	 IN parmUserId int(11),
	 IN parmDigType tinyint(2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserDig( PostCommentId, UserId, DigType)
		VALUES (parmPostCommentId,parmUserId,parmDigType)


	ON DUPLICATE KEY UPDATE
				DigType = parmDigType
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserDigUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserDigUpdate` $$
CREATE PROCEDURE `UserDigUpdate` (
	 IN parmPostCommentId char(36),
	 IN parmUserId int(11),
	 IN parmDigType tinyint(2)
)

	BEGIN 

	UPDATE UserDig SET 
				DigType = parmDigType
		WHERE
			PostCommentId = parmPostCommentId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserDigAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserDigAdd` $$
CREATE PROCEDURE `UserDigAdd` (
	 IN parmPostCommentId char(36),
	 IN parmUserId int(11),
	 IN parmDigType tinyint(2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserDig( PostCommentId, UserId, DigType)
		VALUES (parmPostCommentId,parmUserId,parmDigType);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserDigSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserDigSelect` $$
CREATE PROCEDURE `UserDigSelect` (
	 IN parmPostCommentId char(36),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  PostCommentId, UserId, DigType FROM UserDig WHERE
			PostCommentId = parmPostCommentId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserJobAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserJobAddUpdate` $$
CREATE PROCEDURE `UserJobAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmJobCodeId smallint(5),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmSalary decimal(50,2),
	 IN parmIncomeYearToDate decimal(50,2),
	 IN parmNextOverTimeCheckIn datetime,
	 IN parmCheckInDuration tinyint(2),
	 IN parmOverTimeHours smallint(5),
	 IN parmLastCycleOverTimeHours smallint(5),
	 IN parmStatus char(1),
	 IN parmAppliedOn datetime,
	 IN parmJobExiredEmailSent tinyint(1),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserJob( TaskId, UserId, JobCodeId, StartDate, EndDate, Salary, IncomeYearToDate, NextOverTimeCheckIn, CheckInDuration, OverTimeHours, LastCycleOverTimeHours, Status, AppliedOn, JobExiredEmailSent, UpdatedAt)
		VALUES (parmTaskId,parmUserId,parmJobCodeId,parmStartDate,parmEndDate,parmSalary,parmIncomeYearToDate,parmNextOverTimeCheckIn,parmCheckInDuration,parmOverTimeHours,parmLastCycleOverTimeHours,parmStatus,parmAppliedOn,parmJobExiredEmailSent,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				JobCodeId = parmJobCodeId,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				Salary = parmSalary,
				IncomeYearToDate = parmIncomeYearToDate,
				NextOverTimeCheckIn = parmNextOverTimeCheckIn,
				CheckInDuration = parmCheckInDuration,
				OverTimeHours = parmOverTimeHours,
				LastCycleOverTimeHours = parmLastCycleOverTimeHours,
				Status = parmStatus,
				AppliedOn = parmAppliedOn,
				JobExiredEmailSent = parmJobExiredEmailSent,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserJobUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserJobUpdate` $$
CREATE PROCEDURE `UserJobUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmJobCodeId smallint(5),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmSalary decimal(50,2),
	 IN parmIncomeYearToDate decimal(50,2),
	 IN parmNextOverTimeCheckIn datetime,
	 IN parmCheckInDuration tinyint(2),
	 IN parmOverTimeHours smallint(5),
	 IN parmLastCycleOverTimeHours smallint(5),
	 IN parmStatus char(1),
	 IN parmAppliedOn datetime,
	 IN parmJobExiredEmailSent tinyint(1),
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE UserJob SET 
				UserId = parmUserId,
				JobCodeId = parmJobCodeId,
				StartDate = parmStartDate,
				EndDate = parmEndDate,
				Salary = parmSalary,
				IncomeYearToDate = parmIncomeYearToDate,
				NextOverTimeCheckIn = parmNextOverTimeCheckIn,
				CheckInDuration = parmCheckInDuration,
				OverTimeHours = parmOverTimeHours,
				LastCycleOverTimeHours = parmLastCycleOverTimeHours,
				Status = parmStatus,
				AppliedOn = parmAppliedOn,
				JobExiredEmailSent = parmJobExiredEmailSent,
				UpdatedAt = parmUpdatedAt
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserJobAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserJobAdd` $$
CREATE PROCEDURE `UserJobAdd` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmJobCodeId smallint(5),
	 IN parmStartDate datetime,
	 IN parmEndDate datetime,
	 IN parmSalary decimal(50,2),
	 IN parmIncomeYearToDate decimal(50,2),
	 IN parmNextOverTimeCheckIn datetime,
	 IN parmCheckInDuration tinyint(2),
	 IN parmOverTimeHours smallint(5),
	 IN parmLastCycleOverTimeHours smallint(5),
	 IN parmStatus char(1),
	 IN parmAppliedOn datetime,
	 IN parmJobExiredEmailSent tinyint(1),
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserJob( TaskId, UserId, JobCodeId, StartDate, EndDate, Salary, IncomeYearToDate, NextOverTimeCheckIn, CheckInDuration, OverTimeHours, LastCycleOverTimeHours, Status, AppliedOn, JobExiredEmailSent, UpdatedAt)
		VALUES (parmTaskId,parmUserId,parmJobCodeId,parmStartDate,parmEndDate,parmSalary,parmIncomeYearToDate,parmNextOverTimeCheckIn,parmCheckInDuration,parmOverTimeHours,parmLastCycleOverTimeHours,parmStatus,parmAppliedOn,parmJobExiredEmailSent,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserJobSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserJobSelect` $$
CREATE PROCEDURE `UserJobSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, UserId, JobCodeId, StartDate, EndDate, Salary, IncomeYearToDate, NextOverTimeCheckIn, CheckInDuration, OverTimeHours, LastCycleOverTimeHours, Status, AppliedOn, JobExiredEmailSent, UpdatedAt FROM UserJob WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserLoanAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserLoanAddUpdate` $$
CREATE PROCEDURE `UserLoanAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmLendorId int(11),
	 IN parmLoanAmount decimal(10,2),
	 IN parmLeftAmount decimal(10,2),
	 IN parmPaidAmount decimal(10,2),
	 IN parmMonthlyInterestRate decimal(5,2),
	 IN parmStatus char(1),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserLoan( TaskId, UserId, LendorId, LoanAmount, LeftAmount, PaidAmount, MonthlyInterestRate, Status, CreatedAt, UpdatedAt)
		VALUES (parmTaskId,parmUserId,parmLendorId,parmLoanAmount,parmLeftAmount,parmPaidAmount,parmMonthlyInterestRate,parmStatus,parmCreatedAt,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				LendorId = parmLendorId,
				LoanAmount = parmLoanAmount,
				LeftAmount = parmLeftAmount,
				PaidAmount = parmPaidAmount,
				MonthlyInterestRate = parmMonthlyInterestRate,
				Status = parmStatus,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserLoanUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserLoanUpdate` $$
CREATE PROCEDURE `UserLoanUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmLendorId int(11),
	 IN parmLoanAmount decimal(10,2),
	 IN parmLeftAmount decimal(10,2),
	 IN parmPaidAmount decimal(10,2),
	 IN parmMonthlyInterestRate decimal(5,2),
	 IN parmStatus char(1),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt datetime
)

	BEGIN 

	UPDATE UserLoan SET 
				UserId = parmUserId,
				LendorId = parmLendorId,
				LoanAmount = parmLoanAmount,
				LeftAmount = parmLeftAmount,
				PaidAmount = parmPaidAmount,
				MonthlyInterestRate = parmMonthlyInterestRate,
				Status = parmStatus,
				CreatedAt = parmCreatedAt,
				UpdatedAt = parmUpdatedAt
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserLoanAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserLoanAdd` $$
CREATE PROCEDURE `UserLoanAdd` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmLendorId int(11),
	 IN parmLoanAmount decimal(10,2),
	 IN parmLeftAmount decimal(10,2),
	 IN parmPaidAmount decimal(10,2),
	 IN parmMonthlyInterestRate decimal(5,2),
	 IN parmStatus char(1),
	 IN parmCreatedAt datetime,
	 IN parmUpdatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserLoan( TaskId, UserId, LendorId, LoanAmount, LeftAmount, PaidAmount, MonthlyInterestRate, Status, CreatedAt, UpdatedAt)
		VALUES (parmTaskId,parmUserId,parmLendorId,parmLoanAmount,parmLeftAmount,parmPaidAmount,parmMonthlyInterestRate,parmStatus,parmCreatedAt,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserLoanSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserLoanSelect` $$
CREATE PROCEDURE `UserLoanSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, UserId, LendorId, LoanAmount, LeftAmount, PaidAmount, MonthlyInterestRate, Status, CreatedAt, UpdatedAt FROM UserLoan WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserMerchandiseAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserMerchandiseAddUpdate` $$
CREATE PROCEDURE `UserMerchandiseAddUpdate` (
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmUserId int(11),
	 IN parmQuantity smallint(5),
	 IN parmMerchandiseCondition tinyint(3) unsigned,
	 IN parmPurchasedPrice decimal(50,2),
	 IN parmTax decimal(50,2),
	 IN parmPurchasedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserMerchandise( MerchandiseTypeId, UserId, Quantity, MerchandiseCondition, PurchasedPrice, Tax, PurchasedAt)
		VALUES (parmMerchandiseTypeId,parmUserId,parmQuantity,parmMerchandiseCondition,parmPurchasedPrice,parmTax,parmPurchasedAt)


	ON DUPLICATE KEY UPDATE
				Quantity = parmQuantity,
				MerchandiseCondition = parmMerchandiseCondition,
				PurchasedPrice = parmPurchasedPrice,
				Tax = parmTax,
				PurchasedAt = parmPurchasedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserMerchandiseUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserMerchandiseUpdate` $$
CREATE PROCEDURE `UserMerchandiseUpdate` (
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmUserId int(11),
	 IN parmQuantity smallint(5),
	 IN parmMerchandiseCondition tinyint(3) unsigned,
	 IN parmPurchasedPrice decimal(50,2),
	 IN parmTax decimal(50,2),
	 IN parmPurchasedAt datetime
)

	BEGIN 

	UPDATE UserMerchandise SET 
				Quantity = parmQuantity,
				MerchandiseCondition = parmMerchandiseCondition,
				PurchasedPrice = parmPurchasedPrice,
				Tax = parmTax,
				PurchasedAt = parmPurchasedAt
		WHERE
			MerchandiseTypeId = parmMerchandiseTypeId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserMerchandiseAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserMerchandiseAdd` $$
CREATE PROCEDURE `UserMerchandiseAdd` (
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmUserId int(11),
	 IN parmQuantity smallint(5),
	 IN parmMerchandiseCondition tinyint(3) unsigned,
	 IN parmPurchasedPrice decimal(50,2),
	 IN parmTax decimal(50,2),
	 IN parmPurchasedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserMerchandise( MerchandiseTypeId, UserId, Quantity, MerchandiseCondition, PurchasedPrice, Tax, PurchasedAt)
		VALUES (parmMerchandiseTypeId,parmUserId,parmQuantity,parmMerchandiseCondition,parmPurchasedPrice,parmTax,parmPurchasedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserMerchandiseSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserMerchandiseSelect` $$
CREATE PROCEDURE `UserMerchandiseSelect` (
	 IN parmMerchandiseTypeId smallint(4),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  MerchandiseTypeId, UserId, Quantity, MerchandiseCondition, PurchasedPrice, Tax, PurchasedAt FROM UserMerchandise WHERE
			MerchandiseTypeId = parmMerchandiseTypeId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserNotificationAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserNotificationAddUpdate` $$
CREATE PROCEDURE `UserNotificationAddUpdate` (
	 IN parmNotificationId char(36),
	 IN parmUserId int(11),
	 IN parmNotificationTypeId smallint(6),
	 IN parmPriority tinyint(2),
	 IN parmHasTask tinyint(1),
	 IN parmParms varchar(1000),
	 IN parmEmailSent tinyint(1),
	 IN parmUpdatedAt datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserNotification( NotificationId, UserId, NotificationTypeId, Priority, HasTask, Parms, EmailSent, UpdatedAt)
		VALUES (parmNotificationId,parmUserId,parmNotificationTypeId,parmPriority,parmHasTask,parmParms,parmEmailSent,parmUpdatedAt)


	ON DUPLICATE KEY UPDATE
				Priority = parmPriority,
				HasTask = parmHasTask,
				Parms = parmParms,
				EmailSent = parmEmailSent,
				UpdatedAt = parmUpdatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserNotificationUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserNotificationUpdate` $$
CREATE PROCEDURE `UserNotificationUpdate` (
	 IN parmNotificationId char(36),
	 IN parmUserId int(11),
	 IN parmNotificationTypeId smallint(6),
	 IN parmPriority tinyint(2),
	 IN parmHasTask tinyint(1),
	 IN parmParms varchar(1000),
	 IN parmEmailSent tinyint(1),
	 IN parmUpdatedAt datetime(6)
)

	BEGIN 

	UPDATE UserNotification SET 
				Priority = parmPriority,
				HasTask = parmHasTask,
				Parms = parmParms,
				EmailSent = parmEmailSent,
				UpdatedAt = parmUpdatedAt
		WHERE
			NotificationId = parmNotificationId AND
		UserId = parmUserId AND
		NotificationTypeId = parmNotificationTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserNotificationAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserNotificationAdd` $$
CREATE PROCEDURE `UserNotificationAdd` (
	 IN parmNotificationId char(36),
	 IN parmUserId int(11),
	 IN parmNotificationTypeId smallint(6),
	 IN parmPriority tinyint(2),
	 IN parmHasTask tinyint(1),
	 IN parmParms varchar(1000),
	 IN parmEmailSent tinyint(1),
	 IN parmUpdatedAt datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserNotification( NotificationId, UserId, NotificationTypeId, Priority, HasTask, Parms, EmailSent, UpdatedAt)
		VALUES (parmNotificationId,parmUserId,parmNotificationTypeId,parmPriority,parmHasTask,parmParms,parmEmailSent,parmUpdatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserNotificationSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserNotificationSelect` $$
CREATE PROCEDURE `UserNotificationSelect` (
	 IN parmNotificationId char(36),
	 IN parmUserId int(11),
	 IN parmNotificationTypeId smallint(6)
)

	BEGIN 

		SELECT  NotificationId, UserId, NotificationTypeId, Priority, HasTask, Parms, EmailSent, UpdatedAt FROM UserNotification WHERE
			NotificationId = parmNotificationId AND
		UserId = parmUserId AND
		NotificationTypeId = parmNotificationTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserStockAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserStockAddUpdate` $$
CREATE PROCEDURE `UserStockAddUpdate` (
	 IN parmUserStockId char(36),
	 IN parmUserId int(11),
	 IN parmStockId smallint(5),
	 IN parmPurchasedUnit int(11),
	 IN parmPurchasedPrice decimal(50,2),
	 IN parmPurchasedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserStock( UserStockId, UserId, StockId, PurchasedUnit, PurchasedPrice, PurchasedAt)
		VALUES (parmUserStockId,parmUserId,parmStockId,parmPurchasedUnit,parmPurchasedPrice,parmPurchasedAt)


	ON DUPLICATE KEY UPDATE
				UserId = parmUserId,
				StockId = parmStockId,
				PurchasedUnit = parmPurchasedUnit,
				PurchasedPrice = parmPurchasedPrice,
				PurchasedAt = parmPurchasedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserStockUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserStockUpdate` $$
CREATE PROCEDURE `UserStockUpdate` (
	 IN parmUserStockId char(36),
	 IN parmUserId int(11),
	 IN parmStockId smallint(5),
	 IN parmPurchasedUnit int(11),
	 IN parmPurchasedPrice decimal(50,2),
	 IN parmPurchasedAt datetime
)

	BEGIN 

	UPDATE UserStock SET 
				UserId = parmUserId,
				StockId = parmStockId,
				PurchasedUnit = parmPurchasedUnit,
				PurchasedPrice = parmPurchasedPrice,
				PurchasedAt = parmPurchasedAt
		WHERE
			UserStockId = parmUserStockId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserStockAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserStockAdd` $$
CREATE PROCEDURE `UserStockAdd` (
	 IN parmUserStockId char(36),
	 IN parmUserId int(11),
	 IN parmStockId smallint(5),
	 IN parmPurchasedUnit int(11),
	 IN parmPurchasedPrice decimal(50,2),
	 IN parmPurchasedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserStock( UserStockId, UserId, StockId, PurchasedUnit, PurchasedPrice, PurchasedAt)
		VALUES (parmUserStockId,parmUserId,parmStockId,parmPurchasedUnit,parmPurchasedPrice,parmPurchasedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserStockSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserStockSelect` $$
CREATE PROCEDURE `UserStockSelect` (
	 IN parmUserStockId char(36)
)

	BEGIN 

		SELECT  UserStockId, UserId, StockId, PurchasedUnit, PurchasedPrice, PurchasedAt FROM UserStock WHERE
			UserStockId = parmUserStockId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserTaskAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserTaskAddUpdate` $$
CREATE PROCEDURE `UserTaskAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmAssignerUserId int(11),
	 IN parmCompletionPercent tinyint(2),
	 IN parmFlagged tinyint(1),
	 IN parmStatus char(1),
	 IN parmParms varchar(1000),
	 IN parmTaskTypeId smallint(5),
	 IN parmDueDate datetime,
	 IN parmDefaultResponse smallint(6),
	 IN parmPriority tinyint(2),
	 IN parmCreatedAt datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserTask( TaskId, UserId, AssignerUserId, CompletionPercent, Flagged, Status, Parms, TaskTypeId, DueDate, DefaultResponse, Priority, CreatedAt)
		VALUES (parmTaskId,parmUserId,parmAssignerUserId,parmCompletionPercent,parmFlagged,parmStatus,parmParms,parmTaskTypeId,parmDueDate,parmDefaultResponse,parmPriority,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				AssignerUserId = parmAssignerUserId,
				CompletionPercent = parmCompletionPercent,
				Flagged = parmFlagged,
				Status = parmStatus,
				Parms = parmParms,
				TaskTypeId = parmTaskTypeId,
				DueDate = parmDueDate,
				DefaultResponse = parmDefaultResponse,
				Priority = parmPriority,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserTaskUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserTaskUpdate` $$
CREATE PROCEDURE `UserTaskUpdate` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmAssignerUserId int(11),
	 IN parmCompletionPercent tinyint(2),
	 IN parmFlagged tinyint(1),
	 IN parmStatus char(1),
	 IN parmParms varchar(1000),
	 IN parmTaskTypeId smallint(5),
	 IN parmDueDate datetime,
	 IN parmDefaultResponse smallint(6),
	 IN parmPriority tinyint(2),
	 IN parmCreatedAt datetime(6)
)

	BEGIN 

	UPDATE UserTask SET 
				AssignerUserId = parmAssignerUserId,
				CompletionPercent = parmCompletionPercent,
				Flagged = parmFlagged,
				Status = parmStatus,
				Parms = parmParms,
				TaskTypeId = parmTaskTypeId,
				DueDate = parmDueDate,
				DefaultResponse = parmDefaultResponse,
				Priority = parmPriority,
				CreatedAt = parmCreatedAt
		WHERE
			TaskId = parmTaskId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserTaskAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserTaskAdd` $$
CREATE PROCEDURE `UserTaskAdd` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11),
	 IN parmAssignerUserId int(11),
	 IN parmCompletionPercent tinyint(2),
	 IN parmFlagged tinyint(1),
	 IN parmStatus char(1),
	 IN parmParms varchar(1000),
	 IN parmTaskTypeId smallint(5),
	 IN parmDueDate datetime,
	 IN parmDefaultResponse smallint(6),
	 IN parmPriority tinyint(2),
	 IN parmCreatedAt datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserTask( TaskId, UserId, AssignerUserId, CompletionPercent, Flagged, Status, Parms, TaskTypeId, DueDate, DefaultResponse, Priority, CreatedAt)
		VALUES (parmTaskId,parmUserId,parmAssignerUserId,parmCompletionPercent,parmFlagged,parmStatus,parmParms,parmTaskTypeId,parmDueDate,parmDefaultResponse,parmPriority,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserTaskSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserTaskSelect` $$
CREATE PROCEDURE `UserTaskSelect` (
	 IN parmTaskId char(36),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  TaskId, UserId, AssignerUserId, CompletionPercent, Flagged, Status, Parms, TaskTypeId, DueDate, DefaultResponse, Priority, CreatedAt FROM UserTask WHERE
			TaskId = parmTaskId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteChoiceAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteChoiceAddUpdate` $$
CREATE PROCEDURE `UserVoteChoiceAddUpdate` (
	 IN parmChoiceId smallint(6),
	 IN parmTaskTypeId smallint(5),
	 IN parmChoiceText varchar(1000),
	 IN parmChoiceLogo varchar(50),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserVoteChoice( ChoiceId, TaskTypeId, ChoiceText, ChoiceLogo)
		VALUES (parmChoiceId,parmTaskTypeId,parmChoiceText,parmChoiceLogo)


	ON DUPLICATE KEY UPDATE
				ChoiceText = parmChoiceText,
				ChoiceLogo = parmChoiceLogo
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteChoiceUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteChoiceUpdate` $$
CREATE PROCEDURE `UserVoteChoiceUpdate` (
	 IN parmChoiceId smallint(6),
	 IN parmTaskTypeId smallint(5),
	 IN parmChoiceText varchar(1000),
	 IN parmChoiceLogo varchar(50)
)

	BEGIN 

	UPDATE UserVoteChoice SET 
				ChoiceText = parmChoiceText,
				ChoiceLogo = parmChoiceLogo
		WHERE
			ChoiceId = parmChoiceId AND
		TaskTypeId = parmTaskTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteChoiceAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteChoiceAdd` $$
CREATE PROCEDURE `UserVoteChoiceAdd` (
	 IN parmChoiceId smallint(6),
	 IN parmTaskTypeId smallint(5),
	 IN parmChoiceText varchar(1000),
	 IN parmChoiceLogo varchar(50),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserVoteChoice( ChoiceId, TaskTypeId, ChoiceText, ChoiceLogo)
		VALUES (parmChoiceId,parmTaskTypeId,parmChoiceText,parmChoiceLogo);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteChoiceSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteChoiceSelect` $$
CREATE PROCEDURE `UserVoteChoiceSelect` (
	 IN parmChoiceId smallint(6),
	 IN parmTaskTypeId smallint(5)
)

	BEGIN 

		SELECT  ChoiceId, TaskTypeId, ChoiceText, ChoiceLogo FROM UserVoteChoice WHERE
			ChoiceId = parmChoiceId AND
		TaskTypeId = parmTaskTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteSelectedChoiceAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteSelectedChoiceAddUpdate` $$
CREATE PROCEDURE `UserVoteSelectedChoiceAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmChoiceId smallint(6),
	 IN parmUserId int(11),
	 IN parmScore tinyint(2),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO UserVoteSelectedChoice( TaskId, ChoiceId, UserId, Score)
		VALUES (parmTaskId,parmChoiceId,parmUserId,parmScore)


	ON DUPLICATE KEY UPDATE
				Score = parmScore
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteSelectedChoiceUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteSelectedChoiceUpdate` $$
CREATE PROCEDURE `UserVoteSelectedChoiceUpdate` (
	 IN parmTaskId char(36),
	 IN parmChoiceId smallint(6),
	 IN parmUserId int(11),
	 IN parmScore tinyint(2)
)

	BEGIN 

	UPDATE UserVoteSelectedChoice SET 
				Score = parmScore
		WHERE
			TaskId = parmTaskId AND
		ChoiceId = parmChoiceId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteSelectedChoiceAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteSelectedChoiceAdd` $$
CREATE PROCEDURE `UserVoteSelectedChoiceAdd` (
	 IN parmTaskId char(36),
	 IN parmChoiceId smallint(6),
	 IN parmUserId int(11),
	 IN parmScore tinyint(2),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO UserVoteSelectedChoice( TaskId, ChoiceId, UserId, Score)
		VALUES (parmTaskId,parmChoiceId,parmUserId,parmScore);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure UserVoteSelectedChoiceSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `UserVoteSelectedChoiceSelect` $$
CREATE PROCEDURE `UserVoteSelectedChoiceSelect` (
	 IN parmTaskId char(36),
	 IN parmChoiceId smallint(6),
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  TaskId, ChoiceId, UserId, Score FROM UserVoteSelectedChoice WHERE
			TaskId = parmTaskId AND
		ChoiceId = parmChoiceId AND
		UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WarResultAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WarResultAddUpdate` $$
CREATE PROCEDURE `WarResultAddUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryIdAttacker char(2),
	 IN parmCountryIdDefender char(2),
	 IN parmWinQuality tinyint(3),
	 IN parmWarResult char(1),
	 IN parmEndDate datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO WarResult( TaskId, CountryIdAttacker, CountryIdDefender, WinQuality, WarResult, EndDate)
		VALUES (parmTaskId,parmCountryIdAttacker,parmCountryIdDefender,parmWinQuality,parmWarResult,parmEndDate)


	ON DUPLICATE KEY UPDATE
				CountryIdAttacker = parmCountryIdAttacker,
				CountryIdDefender = parmCountryIdDefender,
				WinQuality = parmWinQuality,
				WarResult = parmWarResult,
				EndDate = parmEndDate
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WarResultUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WarResultUpdate` $$
CREATE PROCEDURE `WarResultUpdate` (
	 IN parmTaskId char(36),
	 IN parmCountryIdAttacker char(2),
	 IN parmCountryIdDefender char(2),
	 IN parmWinQuality tinyint(3),
	 IN parmWarResult char(1),
	 IN parmEndDate datetime
)

	BEGIN 

	UPDATE WarResult SET 
				CountryIdAttacker = parmCountryIdAttacker,
				CountryIdDefender = parmCountryIdDefender,
				WinQuality = parmWinQuality,
				WarResult = parmWarResult,
				EndDate = parmEndDate
		WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WarResultAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WarResultAdd` $$
CREATE PROCEDURE `WarResultAdd` (
	 IN parmTaskId char(36),
	 IN parmCountryIdAttacker char(2),
	 IN parmCountryIdDefender char(2),
	 IN parmWinQuality tinyint(3),
	 IN parmWarResult char(1),
	 IN parmEndDate datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO WarResult( TaskId, CountryIdAttacker, CountryIdDefender, WinQuality, WarResult, EndDate)
		VALUES (parmTaskId,parmCountryIdAttacker,parmCountryIdDefender,parmWinQuality,parmWarResult,parmEndDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WarResultSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WarResultSelect` $$
CREATE PROCEDURE `WarResultSelect` (
	 IN parmTaskId char(36)
)

	BEGIN 

		SELECT  TaskId, CountryIdAttacker, CountryIdDefender, WinQuality, WarResult, EndDate FROM WarResult WHERE
			TaskId = parmTaskId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WeaponTypeAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WeaponTypeAddUpdate` $$
CREATE PROCEDURE `WeaponTypeAddUpdate` (
	 IN parmWeaponTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(15,2),
	 IN parmWeaponTypeCode char(1),
	 IN parmOffenseScore tinyint(3),
	 IN parmDefenseScore tinyint(3),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO WeaponType( WeaponTypeId, Name, Description, ImageFont, Cost, WeaponTypeCode, OffenseScore, DefenseScore)
		VALUES (parmWeaponTypeId,parmName,parmDescription,parmImageFont,parmCost,parmWeaponTypeCode,parmOffenseScore,parmDefenseScore)


	ON DUPLICATE KEY UPDATE
				Name = parmName,
				Description = parmDescription,
				ImageFont = parmImageFont,
				Cost = parmCost,
				WeaponTypeCode = parmWeaponTypeCode,
				OffenseScore = parmOffenseScore,
				DefenseScore = parmDefenseScore
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WeaponTypeUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WeaponTypeUpdate` $$
CREATE PROCEDURE `WeaponTypeUpdate` (
	 IN parmWeaponTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(15,2),
	 IN parmWeaponTypeCode char(1),
	 IN parmOffenseScore tinyint(3),
	 IN parmDefenseScore tinyint(3)
)

	BEGIN 

	UPDATE WeaponType SET 
				Name = parmName,
				Description = parmDescription,
				ImageFont = parmImageFont,
				Cost = parmCost,
				WeaponTypeCode = parmWeaponTypeCode,
				OffenseScore = parmOffenseScore,
				DefenseScore = parmDefenseScore
		WHERE
			WeaponTypeId = parmWeaponTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WeaponTypeAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WeaponTypeAdd` $$
CREATE PROCEDURE `WeaponTypeAdd` (
	 IN parmWeaponTypeId smallint(4),
	 IN parmName varchar(45),
	 IN parmDescription varchar(255),
	 IN parmImageFont varchar(50),
	 IN parmCost decimal(15,2),
	 IN parmWeaponTypeCode char(1),
	 IN parmOffenseScore tinyint(3),
	 IN parmDefenseScore tinyint(3),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO WeaponType( WeaponTypeId, Name, Description, ImageFont, Cost, WeaponTypeCode, OffenseScore, DefenseScore)
		VALUES (parmWeaponTypeId,parmName,parmDescription,parmImageFont,parmCost,parmWeaponTypeCode,parmOffenseScore,parmDefenseScore);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WeaponTypeSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WeaponTypeSelect` $$
CREATE PROCEDURE `WeaponTypeSelect` (
	 IN parmWeaponTypeId smallint(4)
)

	BEGIN 

		SELECT  WeaponTypeId, Name, Description, ImageFont, Cost, WeaponTypeCode, OffenseScore, DefenseScore FROM WeaponType WHERE
			WeaponTypeId = parmWeaponTypeId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobAddUpdate` $$
CREATE PROCEDURE `WebJobAddUpdate` (
	 IN parmJobId tinyint(3),
	 IN parmJobName varchar(30),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO WebJob( JobId, JobName)
		VALUES (parmJobId,parmJobName)


	ON DUPLICATE KEY UPDATE
				JobName = parmJobName
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobUpdate` $$
CREATE PROCEDURE `WebJobUpdate` (
	 IN parmJobId tinyint(3),
	 IN parmJobName varchar(30)
)

	BEGIN 

	UPDATE WebJob SET 
				JobName = parmJobName
		WHERE
			JobId = parmJobId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobAdd` $$
CREATE PROCEDURE `WebJobAdd` (
	 IN parmJobId tinyint(3),
	 IN parmJobName varchar(30),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO WebJob( JobId, JobName)
		VALUES (parmJobId,parmJobName);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobSelect` $$
CREATE PROCEDURE `WebJobSelect` (
	 IN parmJobId tinyint(3)
)

	BEGIN 

		SELECT  JobId, JobName FROM WebJob WHERE
			JobId = parmJobId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobHistoryAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobHistoryAddUpdate` $$
CREATE PROCEDURE `WebJobHistoryAddUpdate` (
	 IN parmJobId tinyint(3),
	 IN parmRunId int(11),
	 IN parmCreatedAT datetime(6),
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO WebJobHistory( JobId, RunId, CreatedAT)
		VALUES (parmJobId,parmRunId,parmCreatedAT)


	ON DUPLICATE KEY UPDATE
				JobId = parmJobId,
				CreatedAT = parmCreatedAT
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobHistoryUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobHistoryUpdate` $$
CREATE PROCEDURE `WebJobHistoryUpdate` (
	 IN parmJobId tinyint(3),
	 IN parmRunId int(11),
	 IN parmCreatedAT datetime(6)
)

	BEGIN 

	UPDATE WebJobHistory SET 
				JobId = parmJobId,
				CreatedAT = parmCreatedAT
		WHERE
			RunId = parmRunId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobHistoryAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobHistoryAdd` $$
CREATE PROCEDURE `WebJobHistoryAdd` (
	 IN parmJobId tinyint(3),
	 IN parmRunId int(11),
	 IN parmCreatedAT datetime(6),
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO WebJobHistory( JobId, RunId, CreatedAT)
		VALUES (parmJobId,parmRunId,parmCreatedAT);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebJobHistorySelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebJobHistorySelect` $$
CREATE PROCEDURE `WebJobHistorySelect` (
	 IN parmRunId int(11)
)

	BEGIN 

		SELECT  JobId, RunId, CreatedAT FROM WebJobHistory WHERE
			RunId = parmRunId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserAddUpdate` $$
CREATE PROCEDURE `WebUserAddUpdate` (
	 IN parmUserId int(11),
	 IN parmNameFirst varchar(45),
	 IN parmNameLast varchar(45),
	 IN parmEmailId varchar(100),
	 IN parmPicture varchar(255),
	 IN parmActive tinyint(3),
	 IN parmOnlineStatus tinyint(2),
	 IN parmCountryId char(2),
	 IN parmUserLevel int(11),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO WebUser( UserId, NameFirst, NameLast, EmailId, Picture, Active, OnlineStatus, CountryId, UserLevel, CreatedAt)
		VALUES (parmUserId,parmNameFirst,parmNameLast,parmEmailId,parmPicture,parmActive,parmOnlineStatus,parmCountryId,parmUserLevel,parmCreatedAt)


	ON DUPLICATE KEY UPDATE
				NameFirst = parmNameFirst,
				NameLast = parmNameLast,
				EmailId = parmEmailId,
				Picture = parmPicture,
				Active = parmActive,
				OnlineStatus = parmOnlineStatus,
				CountryId = parmCountryId,
				UserLevel = parmUserLevel,
				CreatedAt = parmCreatedAt
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserUpdate` $$
CREATE PROCEDURE `WebUserUpdate` (
	 IN parmUserId int(11),
	 IN parmNameFirst varchar(45),
	 IN parmNameLast varchar(45),
	 IN parmEmailId varchar(100),
	 IN parmPicture varchar(255),
	 IN parmActive tinyint(3),
	 IN parmOnlineStatus tinyint(2),
	 IN parmCountryId char(2),
	 IN parmUserLevel int(11),
	 IN parmCreatedAt datetime
)

	BEGIN 

	UPDATE WebUser SET 
				NameFirst = parmNameFirst,
				NameLast = parmNameLast,
				EmailId = parmEmailId,
				Picture = parmPicture,
				Active = parmActive,
				OnlineStatus = parmOnlineStatus,
				CountryId = parmCountryId,
				UserLevel = parmUserLevel,
				CreatedAt = parmCreatedAt
		WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserAdd` $$
CREATE PROCEDURE `WebUserAdd` (
	 IN parmUserId int(11),
	 IN parmNameFirst varchar(45),
	 IN parmNameLast varchar(45),
	 IN parmEmailId varchar(100),
	 IN parmPicture varchar(255),
	 IN parmActive tinyint(3),
	 IN parmOnlineStatus tinyint(2),
	 IN parmCountryId char(2),
	 IN parmUserLevel int(11),
	 IN parmCreatedAt datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO WebUser( UserId, NameFirst, NameLast, EmailId, Picture, Active, OnlineStatus, CountryId, UserLevel, CreatedAt)
		VALUES (parmUserId,parmNameFirst,parmNameLast,parmEmailId,parmPicture,parmActive,parmOnlineStatus,parmCountryId,parmUserLevel,parmCreatedAt);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserSelect` $$
CREATE PROCEDURE `WebUserSelect` (
	 IN parmUserId int(11)
)

	BEGIN 

		SELECT  UserId, NameFirst, NameLast, EmailId, Picture, Active, OnlineStatus, CountryId, UserLevel, CreatedAt FROM WebUser WHERE
			UserId = parmUserId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserContactAddUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserContactAddUpdate` $$
CREATE PROCEDURE `WebUserContactAddUpdate` (
	 IN parmUserId int(11),
	 IN parmInvitationId char(36),
	 IN parmFriendEmailId varchar(100),
	 IN parmFriendUserId int(11),
	 IN parmNameFirst varchar(45),
	 IN parmNameLast varchar(45),
	 IN parmPartyInvite tinyint(3),
	 IN parmJoinInvite tinyint(3),
	 IN parmUnsubscribe tinyint(1),
	 IN parmLastInviteDate datetime,
	OUT parmLastId INT
)


	BEGIN 

		INSERT INTO WebUserContact( UserId, InvitationId, FriendEmailId, FriendUserId, NameFirst, NameLast, PartyInvite, JoinInvite, Unsubscribe, LastInviteDate)
		VALUES (parmUserId,parmInvitationId,parmFriendEmailId,parmFriendUserId,parmNameFirst,parmNameLast,parmPartyInvite,parmJoinInvite,parmUnsubscribe,parmLastInviteDate)


	ON DUPLICATE KEY UPDATE
				InvitationId = parmInvitationId,
				FriendUserId = parmFriendUserId,
				NameFirst = parmNameFirst,
				NameLast = parmNameLast,
				PartyInvite = parmPartyInvite,
				JoinInvite = parmJoinInvite,
				Unsubscribe = parmUnsubscribe,
				LastInviteDate = parmLastInviteDate
;

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserContactUpdate
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserContactUpdate` $$
CREATE PROCEDURE `WebUserContactUpdate` (
	 IN parmUserId int(11),
	 IN parmInvitationId char(36),
	 IN parmFriendEmailId varchar(100),
	 IN parmFriendUserId int(11),
	 IN parmNameFirst varchar(45),
	 IN parmNameLast varchar(45),
	 IN parmPartyInvite tinyint(3),
	 IN parmJoinInvite tinyint(3),
	 IN parmUnsubscribe tinyint(1),
	 IN parmLastInviteDate datetime
)

	BEGIN 

	UPDATE WebUserContact SET 
				InvitationId = parmInvitationId,
				FriendUserId = parmFriendUserId,
				NameFirst = parmNameFirst,
				NameLast = parmNameLast,
				PartyInvite = parmPartyInvite,
				JoinInvite = parmJoinInvite,
				Unsubscribe = parmUnsubscribe,
				LastInviteDate = parmLastInviteDate
		WHERE
			UserId = parmUserId AND
		FriendEmailId = parmFriendEmailId ;
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserContactAdd
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserContactAdd` $$
CREATE PROCEDURE `WebUserContactAdd` (
	 IN parmUserId int(11),
	 IN parmInvitationId char(36),
	 IN parmFriendEmailId varchar(100),
	 IN parmFriendUserId int(11),
	 IN parmNameFirst varchar(45),
	 IN parmNameLast varchar(45),
	 IN parmPartyInvite tinyint(3),
	 IN parmJoinInvite tinyint(3),
	 IN parmUnsubscribe tinyint(1),
	 IN parmLastInviteDate datetime,
	OUT parmLastId INT
)

	BEGIN 

		INSERT INTO WebUserContact( UserId, InvitationId, FriendEmailId, FriendUserId, NameFirst, NameLast, PartyInvite, JoinInvite, Unsubscribe, LastInviteDate)
		VALUES (parmUserId,parmInvitationId,parmFriendEmailId,parmFriendUserId,parmNameFirst,parmNameLast,parmPartyInvite,parmJoinInvite,parmUnsubscribe,parmLastInviteDate);

SET parmLastId = LAST_INSERT_ID();
END$$

DELIMITER ;
-- -----------------------------------------------------
-- procedure WebUserContactSelect
-- -----------------------------------------------------
DELIMITER $$
DROP PROCEDURE IF EXISTS `WebUserContactSelect` $$
CREATE PROCEDURE `WebUserContactSelect` (
	 IN parmUserId int(11),
	 IN parmFriendEmailId varchar(100)
)

	BEGIN 

		SELECT  UserId, InvitationId, FriendEmailId, FriendUserId, NameFirst, NameLast, PartyInvite, JoinInvite, Unsubscribe, LastInviteDate FROM WebUserContact WHERE
			UserId = parmUserId AND
		FriendEmailId = parmFriendEmailId ;
END$$

DELIMITER ;
