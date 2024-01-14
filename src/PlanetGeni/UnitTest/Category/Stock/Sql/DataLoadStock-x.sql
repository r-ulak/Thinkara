SET SQL_SAFE_UPDATES =0;
Delete From UserStock;
Delete From StockTrade;
Delete From StockTradeHistory;
Delete From StockHistory;


LOAD DATA LOCAL INFILE '{0}UserStock-{1}.tsv' INTO TABLE UserStock FIELDS TERMINATED BY '\t';
LOAD DATA LOCAL INFILE '{0}StockTrade-{1}.tsv' INTO TABLE StockTrade FIELDS TERMINATED BY '\t';
