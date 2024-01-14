using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class StockDTORepository : IStockDTORepository
    {
        private StoredProcedure spContext = new StoredProcedure();
        private IRedisCacheProvider cache { get; set; }
        public StockDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public StockDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public string GetStockByIdJson(int stockId)
        {
            string stockData = cache.GetStringKey(AppSettings.RedisKeyStockCode + stockId.ToString());
            if (stockData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmStockId", stockId);
                stockData = JsonConvert.SerializeObject(spContext.GetByPrimaryKey<Stock>(dictionary));
                cache.SetStringKey(AppSettings.RedisKeyStockCode + stockId.ToString(), stockData);
            }
            return (stockData);

        }
        public string GetCurrentStockJson()
        {
            string currentStockData = cache.GetStringKey(AppSettings.RedisKeyCurrentStockPrice);
            if (currentStockData == null)
            {
                currentStockData = JsonConvert.SerializeObject(spContext.GetSqlDataNoParms<Stock>(AppSettings.SPGetCurrentStock));
                cache.SetStringKey(AppSettings.RedisKeyCurrentStockPrice, currentStockData);
            }
            return (currentStockData);
        }
        public void ExpireCurrentStock()
        {
            cache.Invalidate(AppSettings.RedisKeyCurrentStockPrice);
        }
        public IQueryable<StockTradeDTO> GetStockTradeByUser
               (int userId, DateTime? parmlastDateTime = null)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmLastDateTime", parmlastDateTime);
            dictionary.Add("parmStockLimit", AppSettings.StockLimit);
            IEnumerable<StockTradeDTO> userStockTrade =
                spContext.GetSqlData<StockTradeDTO>
                (AppSettings.SPGetStockTradeByUser, dictionary);
            return userStockTrade.AsQueryable();
        }

        public IQueryable<StockTrade> GetPendingStockTrade()
        {
            IEnumerable<StockTrade> userStockTrade =
      spContext.GetSqlDataNoParms<StockTrade>
      (AppSettings.SPGetPendingStockTrade);
            return userStockTrade.AsQueryable();
        }
        public string GetTopTenStockOwnerJson()
        {
            string stockTopNData = cache.GetStringKey(AppSettings.RedisKeyStockSummaryTop10);
            if (stockTopNData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", 10);
                stockTopNData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<TopTenStockOwnerDTO>(
                AppSettings.SPGetTopNStockOwner,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyStockSummaryTop10, stockTopNData,
                    AppSettings.StockSummaryTop10CacheLimit);
            }
            return (stockTopNData);
        }

        public IQueryable<StockSummaryDTO> GetStockSummary(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            IEnumerable<StockSummaryDTO> userStockSummary =
                spContext.GetSqlData<StockSummaryDTO>
                (AppSettings.SPGetStockSummary, dictionary);
            return userStockSummary.AsQueryable();
        }

        public int ExecuteTrade(ExecuteTradeDTO executeTrade)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmSellerStockId", executeTrade.SellerStockId);
            dictionary.Add("parmStockId", executeTrade.StockId);
            dictionary.Add("parmPurchasedUnit", executeTrade.PurchasedUnit);
            dictionary.Add("parmPurchasedPrice", executeTrade.PurchasedPrice);
            dictionary.Add("parmBuyerId", executeTrade.BuyerId);
            dictionary.Add("parmSellerId", executeTrade.SellerId);
            dictionary.Add("parmTotalValue", executeTrade.TotalValue);
            dictionary.Add("parmTaxValue", executeTrade.TaxValue);
            dictionary.Add("parmTaxCode", executeTrade.TaxCode);
            dictionary.Add("parmBuyerCountryId", executeTrade.BuyerCountryId);
            dictionary.Add("parmFundType", AppSettings.StocksFundType);
            int result = (int)spContext.GetSqlDataSignleValue(AppSettings.SPExecuteTradeOrder, dictionary, "result");
            return result;
        }
        public IQueryable<UserStockDTO> GetStockByUser
       (int userId, DateTime? parmlastDateTime = null)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmLastDateTime", parmlastDateTime);
            dictionary.Add("parmStockLimit", AppSettings.StockLimit);
            IEnumerable<UserStockDTO> userStock =
                spContext.GetSqlData<UserStockDTO>
                (AppSettings.SPGetStockByUser, dictionary);
            return userStock.AsQueryable();
        }
        public void UpdateStocks(List<Stock> stocks)
        {
            if (stocks.Count > 0)
            {
                spContext.UpdateList(stocks);

            }
        }
        public int UpdateTrades(List<StockTrade> trades)
        {
            if (trades.Count > 0)
            {
                return spContext.UpdateList(trades);
            }
            else
            {
                return 0;
            }
        }
        public void RemoveCompletedTrades(Guid[] trades)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            if (trades.Length > 0)
            {
                for (int i = 0; i < trades.Length; i += 1680)
                {
                    dictionary.Clear();
                    dictionary.Add("parmTradeId", string.Join(",", trades.Skip(i).Take(1680).Select(x => string.Format("'{0}'", x))));
                    spContext.ExecuteStoredProcedure(AppSettings.SPDeleteStockTrade, dictionary);
                }
            }

        }
        public void AddStockHistory(List<StockHistory> history)
        {
            if (history.Count > 0)
            {
                spContext.AddList(history);
            }
        }
        public int AddStockTradesHistory(List<StockTradeHistory> trades)
        {
            if (trades.Count > 0)
            {
                return spContext.AddList(trades);
            }
            else
            {
                return 0;
            }
        }
        public UserStock GetStockByUserStockId(Guid userStockId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserStockId", userStockId);
            UserStock userStock = spContext.GetByPrimaryKey<UserStock>
                (dictionary);
            return userStock;
        }
        public List<StockPriceDTO> GetTodaysStock()
        {
            List<StockPriceDTO> todaysStockPrices =
              spContext.GetSqlDataNoParms<StockPriceDTO>
              (AppSettings.SPGetTodaysStockPrice).ToList();
            return todaysStockPrices;
        }
        public int GetCountStockByQty(int userId, int[] items, int qty)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string itemList = String.Join(",", items);
            dictionary.Add("parmStockIdList", itemList);
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmQuantity", qty);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPGetCountStockByQty
                , dictionary, "cnt"));
            return count;
        }

        public int TryCancelStockOrder(Guid tradeId, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmTradeId", tradeId);
            dictionary.Add("parmUserId", userId);
            return spContext.ExecuteStoredProcedure(AppSettings.SPTryCancelStockOrder, dictionary);
        }

        public bool HasThisStock(int userId, short[] items)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string itemList = String.Join(",", items);
            dictionary.Add("parmStockIdList", itemList);
            dictionary.Add("parmUserId", userId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPHasThisStock, dictionary, "cnt"));
            if (count >= items.Length)
            {
                return true;
            }
            return false;
        }

        public bool HasThisStockonPendingTrade(int userId, short[] items)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string itemList = String.Join(",", items);
            dictionary.Add("parmStockIdList", itemList);
            dictionary.Add("parmUserId", userId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPHasThisStockonPendingTrade, dictionary, "cnt"));
            if (count >= items.Length)
            {
                return true;
            }
            return false;
        }

        public bool SaveBuyStockCart(BuySellStockDTO[]
     stockCartList, int userId)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                UserBankAccount bankac =
                    spContext.GetByPrimaryKey<UserBankAccount>(dictionary);
                DateTime dateTime = DateTime.UtcNow;
                foreach (BuySellStockDTO item in stockCartList)
                {
                    StockTrade tradeStock = new StockTrade()
                    {
                        OfferPrice = item.OfferPrice,
                        OrderType = item.OrderType,
                        RequestedAt = dateTime,
                        Status = "P",
                        StockId = item.StockId,
                        TradeId = Guid.NewGuid(),
                        TradeType = "B",
                        LeftUnit = item.Quantity,
                        InitialUnit = item.Quantity,
                        UpdatedAt = dateTime,
                        UserId = userId,

                    };
                    spContext.AddUpdate(tradeStock);
                }



                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveStockCart");
                return false;
            }
        }
        public int UpdateUserStock(UserStock stock)
        {
            return spContext.Update(stock);
        }
        public bool SaveSellStockCart(BuySellStockDTO[]
      stockCartList, int userId)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                DateTime dateTime = DateTime.UtcNow;
                foreach (BuySellStockDTO item in stockCartList)
                {
                    dictionary.Clear();
                    dictionary.Add("parmUserStockId", item.UserStockId);
                    UserStock userStock =
         spContext.GetByPrimaryKey<UserStock>(dictionary);
                    StockTrade tradeStock = new StockTrade()
                    {
                        OfferPrice = item.OfferPrice,
                        OrderType = item.OrderType,
                        RequestedAt = dateTime,
                        Status = "P",
                        StockId = item.StockId,
                        TradeId = Guid.NewGuid(),
                        TradeType = "S",
                        LeftUnit = item.Quantity,
                        InitialUnit = item.Quantity,
                        UpdatedAt = dateTime,
                        UserId = userId,
                        UserStockId = item.UserStockId
                    };
                    spContext.Add(tradeStock);
                    userStock.PurchasedUnit -= item.Quantity;
                    spContext.AddUpdate(userStock);
                }
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveSellStockCart");
                return false;
            }
        }
        public void CancelStockOrderForBudget()
        {
            spContext.ExecuteStoredProcedure(AppSettings.SPCancelStockOrderForBudget, new Dictionary<string, object>());
        }
        public void BuyStockOrderForBudget()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmStockBudgetTypeStart", AppSettings.StockBudgetTypeStart);
            dictionary.Add("parmStockBudgetTypeEnd", AppSettings.StockBudgetTypeEnd);

            spContext.ExecuteStoredProcedure(AppSettings.SPBuyStockOrderForBudget, dictionary);
        }

        public void PayStockDividend()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmDividendRate", RulesSettings.StockDividentRate);
            dictionary.Add("parmNotificationTypeId", AppSettings.StockDividenNotificationId);
            dictionary.Add("parmTaxCode", AppSettings.TaxStockDividendCode);
            dictionary.Add("parmBankId", AppSettings.BankId);
            dictionary.Add("parmFundType", AppSettings.StockDividendFundType);
            dictionary.Add("parmDividendCap", RulesSettings.StockDividentCap);

            spContext.ExecuteStoredProcedure(AppSettings.SPPayStockDividend, dictionary);
        }

        public List<HistoricalStockDTO> FetchHistoricData(string[] quotes, string date)
        {
            string redisKey = AppSettings.RedisKeyStockHistory + date;
            string historyStockData = cache.GetStringKey(redisKey);
            if (historyStockData == null)
            {
                string symbolList = String.Join(",", quotes.Select(w => "\"" + w + "\"").ToArray());
                string url = string.Format(AppSettings.YQLFinanceHistoryDataURL +
                                          "select * from yahoo.finance.historicaldata where symbol in ({0}) and startDate=\"{1}\" and endDate=\"{1}\"" +
                                          "&env=store://datatables.org/alltableswithkeys&format=json", symbolList, date);


                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(url).Result;  // Blocking call!
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response body. Blocking!
                        JObject data = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        try
                        {
                            var dataObjects = data["query"]["results"]["quote"].ToObject<List<HistoricalStockDTO>>();
                            cache.SetStringKey(redisKey, JsonConvert.SerializeObject(dataObjects), AppSettings.StockHistoryDataCacheLimit);
                            return dataObjects;
                        }
                        catch (Exception ex)
                        {
                            ExceptionLogging.LogError(ex, "Error to FetchHistoricData");
                            return new List<HistoricalStockDTO>();
                        }


                    }
                    else
                    {
                        Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        return new List<HistoricalStockDTO>();
                    }

                }
            }
            return JsonConvert.DeserializeObject<List<HistoricalStockDTO>>(historyStockData);
        }

        public void SetStockForecast(string forecastDate)
        {
            List<HistoricalStockDTO> stockHistoryQuote = new List<HistoricalStockDTO>();
            List<StockForecastDTO> forecastStockValues = new List<StockForecastDTO>();
            forecastStockValues =
            JsonConvert.DeserializeObject<List<StockForecastDTO>>(
    GetCurrentStockJson());
            stockHistoryQuote = FetchHistoricData(forecastStockValues.Select(x => x.Ticker).ToArray(), forecastDate);
            if (stockHistoryQuote.Count == 0)
            {
                return;
            }
            foreach (var item in forecastStockValues)
            {
                item.ForecastValue = (decimal)stockHistoryQuote.Find(f => f.Symbol == item.Ticker).Adj_Close;
                item.DayChange = (decimal)stockHistoryQuote.Find(f => f.Symbol == item.Ticker).Adj_Close - item.CurrentValue;
                item.DayChangePercent = (item.DayChange / item.CurrentValue) * 100;
                if (item.DayChangePercent <= 0)
                {
                    item.Ratings = "Sell";
                }
                else if (item.DayChangePercent <= 5)
                {
                    item.Ratings = "Hold";
                }
                else
                {
                    item.Ratings = "Buy";
                }
            }
            cache.SetStringKey(AppSettings.RedisKeyStockForecast, JsonConvert.SerializeObject(forecastStockValues));
        }

        public string GetStockForecast()
        {
            string forecastStockData = cache.GetStringKey(AppSettings.RedisKeyStockForecast);
            if (forecastStockData == null)
            {
                DateTime date = DateTime.UtcNow;
                if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    date.AddDays(-1);
                }
                else if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    date.AddDays(-2);
                }
                SetStockForecast(date.ToString("yyyy-MM-dd"));
                forecastStockData = cache.GetStringKey(AppSettings.RedisKeyStockForecast);
            }

            return forecastStockData;
        }
        public void SendStcokForecastNotification()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmNotificationTypeId", AppSettings.StockForeCastNotificationId);
            dictionary.Add("parmPostContentTypeId", AppSettings.StockForecastContentTypeId);

            spContext.ExecuteStoredProcedure(AppSettings.SPSendStcokForecastNotification, dictionary);
        }
    }
}
