SET SQL_SAFE_UPDATES =0;
Delete From CountryBudget;
Delete From CountryBudgetByType;



  LOAD DATA LOCAL INFILE 'C:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\SnapShot\\CountryBudget.tsv' INTO TABLE planetgeni.CountryBudget FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
 LOAD DATA LOCAL INFILE 'C:\\Home\\CodeBack\\PlanetGeni\\DAL\\DbScripts\\SnapShot\\CountryBudgetByType.tsv' INTO TABLE planetgeni.CountryBudgetByType FIELDS TERMINATED BY '\t' IGNORE 1 LINES;
