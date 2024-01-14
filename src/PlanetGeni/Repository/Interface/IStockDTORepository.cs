using DAO.Models;
using DTO.Custom;
using DTO.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IStockDTORepository
    {
        string GetStockForecast();
        void SetStockForecast(string forecastDate);
        List<HistoricalStockDTO> FetchHistoricData(string[] quotes, string date);
        void PayStockDividend();
        void BuyStockOrderForBudget();
        void CancelStockOrderForBudget();
        void AddStockHistory(List<StockHistory> history);
        void UpdateStocks(List<Stock> stocks);
        void ExpireCurrentStock();
        List<StockPriceDTO> GetTodaysStock();
        void RemoveCompletedTrades(Guid[] trades);
        string GetStockByIdJson(int stockId);
        int AddStockTradesHistory(List<StockTradeHistory> trades);
        IQueryable<StockTradeDTO> GetStockTradeByUser
            (int userId, DateTime? parmlastDateTime = null);
        bool HasThisStockonPendingTrade(int userId, short[] items);
        bool HasThisStock(int userId, short[] items);
        int TryCancelStockOrder(Guid tradeId, int userId);
        int GetCountStockByQty(int userId, int[] items, int qty);
        IQueryable<UserStockDTO> GetStockByUser(int userId, DateTime? parmlastDateTime = null);
        string GetCurrentStockJson();
        bool SaveBuyStockCart(BuySellStockDTO[]
            stockCartList, int userId);
        bool SaveSellStockCart(BuySellStockDTO[]
        stockCartList, int userId);
        string GetTopTenStockOwnerJson();
        int UpdateUserStock(UserStock stock);
        UserStock GetStockByUserStockId(Guid userStockId);
        IQueryable<StockSummaryDTO> GetStockSummary(int userId);

        int ExecuteTrade(ExecuteTradeDTO executeTrade);
        IQueryable<StockTrade> GetPendingStockTrade();
        int UpdateTrades(List<StockTrade> trades);
        void SendStcokForecastNotification();


    }
}
