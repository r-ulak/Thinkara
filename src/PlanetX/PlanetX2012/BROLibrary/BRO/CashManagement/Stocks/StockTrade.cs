using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO.Models;
using PlanetX2012.Models.DAO;

namespace BROLibrary.BRO.CashManagement.Stocks
{

    public class StockTrade
    {
        private StoredProcedure sp = new StoredProcedure();
        /// <summary>
        ///  //1.Cacluate the cost prive stockunit * stock proce( lookup by stockId)
        ///2.Check If the UserBacnkAccount if user has enough money
        ///3. 
        ///• Add a row to StocksTransaction
        ///◦OwnerId is  the userID
        ///◦TransactionType can be Buy (0), Sell (1)
        ///•Updated UserBankAcccount
        ///◦deduct cash cost
        ///◦update Stocks by adding new stocks units not value
        ///•Add /update row to UserStocks
        ///◦update to PurchasedUnit
        ///•Update the UserBankAccount for the user who owns the Stock
        ///• increase the stock value by % +- random
        /// </summary>
        public void BuyStock(int stockId, int userId, int stockUnit)
        {
            decimal stockValue = GetStockValue(stockId);
            decimal totalValue = stockValue * stockUnit;
            int ownerId = GetStockOwnerId(stockId);

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmstockId", stockId);
            dictionary.Add("parmtotalValue", totalValue);
            dictionary.Add("parmuserId", userId);
            dictionary.Add("parmstockUnit", stockUnit);
            dictionary.Add("parmownerID", ownerId);
            sp.GetSqlDataSignleValue("BuyStock", dictionary, "CurrentValue");
            IncreaseStockAfterBuyingStock(stockId, userId, stockUnit, stockValue, totalValue);

        }
        /// <summary>
        /// ◦ Add a row to StocksTransaction
        ///◾OwnerId is  the userID
        ///◾TransactionType can be Buy (0), Sell (1)
        ///◦Updated UserBankAcccount
        ///◾add cash cost
        ///◾update Stocks by adding new stocks units 
        ///◦update row to UserStocks
        ///◾update to PurchasedUnit
        ///◦Update the UserBankAccount for the user who owns the Stock deduct some cash
        ///◦decrease stock value by % +- random
        /// </summary>
        /// <param name="stockid"></param>
        /// <param name="userid"></param>
        /// <param name="stockunit"></param>

        public void SellStock(int stockId, int userId, int stockUnit)
        {
            decimal stockValue = GetStockValue(stockId);
            decimal totalValue = stockValue * stockUnit;
            int ownerId = GetStockOwnerId(stockId);

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmstockId", stockId);
            dictionary.Add("parmtotalValue", totalValue);
            dictionary.Add("parmuserId", userId);
            dictionary.Add("parmstockUnit", stockUnit);
            dictionary.Add("parmownerID", ownerId);
            sp.GetSqlDataSignleValue("SellStock", dictionary, "CurrentValue");
            DecreaseStockAfterBuyingStock(stockId, userId, stockUnit, stockValue, totalValue);
        }

        /// <summary>
        /// Get OwnerId for a Stock
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>

        private int GetStockOwnerId(int stockId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("stockId", stockId);
            return (int)sp.GetSqlDataSignleValue("GetStockOwnerId", dictionary, "CurrentValue");

        }
        /// <summary>
        /// Get StockValue for a given Stock ID
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public decimal GetStockValue(int stockId)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("stockId", stockId);

            return (decimal)sp.GetSqlDataSignleValue("GetStockValue", dictionary, "CurrentValue");

        }
        /// <summary>
        /// Increase Stock After Buying Stock
        /// </summary>
        /// <param name="stockId"></param>
        /// <param name="userId"></param>
        /// <param name="stockUnit"></param>
        /// <param name="stockValue"></param>
        /// <param name="totalValue"></param>
        /// <returns></returns>
        private int IncreaseStockAfterBuyingStock(int stockId, int userId, int stockUnit, decimal stockValue, decimal totalValue)
        {
            var r = new Random();
            stockValue = stockValue + totalValue * (decimal)r.NextDouble();


            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmstockId", stockId);
            dictionary.Add("parmstockValue", stockValue);

            return sp.ExecuteStoredProcedure("UpdateStock", dictionary);

        }

        private int DecreaseStockAfterBuyingStock(int stockId, int userId, int stockUnit, decimal stockValue, decimal totalValue)
        {
            var r = new Random();
            stockValue = stockValue - totalValue * (decimal)r.NextDouble();
            if (stockValue < 0)
                stockValue = 0;

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmstockId", stockId);
            dictionary.Add("parmstockValue", stockValue);

            return sp.ExecuteStoredProcedure("UpdateStock", dictionary);

        }


    }
}
