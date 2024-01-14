using Common;
using DAO.Models;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Jobs
{

    public class StockBrokerManager
    {
        private List<StockTrade> pendingTrades;
        private List<StockTrade> pendingBuyTrades;
        private List<StockTrade> pendingSellTrades;
        private List<StockTradeHistory> tradeHistories;
        private List<StockHistory> stockHistorys;
        private IStockDTORepository _stockRepo;
        private IUserBankAccountDTORepository _bankRepo;
        private IWebUserDTORepository _webUserRepo;
        private ICountryTaxDetailsDTORepository _taxRepo;
        private Stock currentStock;
        private DateTime processedTime;
        private StringBuilder response;
        private List<Stock> currentStockValues;
        private IUserNotificationDetailsDTORepository userNotif;
        private List<HistoricalStockDTO> stockHistoryQuote;
        public StockBrokerManager()
        {
            _stockRepo = new StockDTORepository();
            _bankRepo = new UserBankAccountDTORepository();
            _webUserRepo = new WebUserDTORepository();
            _taxRepo = new CountryTaxDetailsDTORepository();
            currentStockValues = new List<Stock>();
            response = new StringBuilder();
            userNotif = new UserNotificationDetailsDTORepository();
            tradeHistories = new List<StockTradeHistory>();
            stockHistorys = new List<StockHistory>();
            stockHistoryQuote = new List<HistoricalStockDTO>();
        }
        public StringBuilder ProcessAll(string date)
        {
            processedTime = DateTime.UtcNow;
            Console.WriteLine("Fetching pending Stcok trades");
            pendingTrades = _stockRepo.GetPendingStockTrade().ToList(); //getData from DB
            Console.WriteLine("Fecthced {0} trades request", pendingTrades.Count);

            pendingBuyTrades = pendingTrades.Where(q => q.TradeType == "B" && (q.Status == "P" || q.Status == "I")).ToList();
            pendingSellTrades = pendingTrades.Where(q => q.TradeType == "S" && (q.Status == "P" || q.Status == "I")).ToList();
            Console.WriteLine("Fecthced {0} buy trades request", pendingBuyTrades.Count);
            response.AppendFormat("Fecthced {0} buy trades request \n", pendingBuyTrades.Count);
            Console.WriteLine("Fecthced {0} sell trades request", pendingSellTrades.Count);
            response.AppendFormat("Fecthced {0} sell trades request \n", pendingSellTrades.Count);

            currentStockValues = JsonConvert.DeserializeObject<List<Stock>>(
    _stockRepo.GetCurrentStockJson());
            Console.WriteLine("Fecthced Current Stock values");
            foreach (var item in currentStockValues)
            {
                Console.WriteLine("StockId {0}  Value {1}", item.StockId, item.CurrentValue);
                response.AppendFormat("StockId {0}  Value {1} \n", item.StockId, item.CurrentValue);
            }

            stockHistoryQuote = _stockRepo.FetchHistoricData(currentStockValues.Select(x => x.Ticker).ToArray(), date);

            try
            {
                int total = pendingBuyTrades.Count;
                for (int i = 0; i < pendingBuyTrades.Count; i++)
                {
                    if (pendingBuyTrades[i].Status == "P" || pendingBuyTrades[i].Status == "I")
                    {
                        currentStock = currentStockValues.Where(q => q.StockId == pendingBuyTrades[i].StockId).First();
                        Console.WriteLine("Processing Trade Id {0} {1} out of  {2}", pendingBuyTrades[i].TradeId, i, total);
                        ProcessTrade(pendingBuyTrades[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessStock Trading");
            }
            SaveResult();
            return response;
        }
        private void ProcessTrade(StockTrade tradeOrder)
        {

            try
            {
                BuyTrade(tradeOrder);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessTrade");

            }

        }
        private void BuyTrade(StockTrade tradeOrder)
        {
            List<StockTrade> currentSellTrades = pendingSellTrades.Where(f => (f.Status == "P" || f.Status == "I") && (f.StockId == tradeOrder.StockId && f.UserId != tradeOrder.UserId && f.LeftUnit > 0)).ToList();
            foreach (var order in currentSellTrades)
            {
                if (order.Status == "D" || order.Status == "C" || order.LeftUnit == 0)
                {
                    continue;
                }
                if (tradeOrder.Status == "D" || tradeOrder.LeftUnit == 0 || tradeOrder.Status == "C" || tradeOrder.Status == "M")
                {
                    break;
                }
                if (tradeOrder.OrderType == "L")
                {
                    if (order.OrderType == "L" && tradeOrder.OfferPrice >= order.OfferPrice)
                    {
                        ExecuteTrade(tradeOrder, order, order.OfferPrice);

                        continue;
                    }
                    else if (order.OrderType == "M" && tradeOrder.OfferPrice >= currentStock.CurrentValue)
                    {
                        ExecuteTrade(tradeOrder, order, currentStock.CurrentValue);
                        continue;
                    }

                }
                else if (tradeOrder.OrderType == "M")
                {
                    if (order.OrderType == "L" && currentStock.CurrentValue >= order.OfferPrice)
                    {
                        ExecuteTrade(tradeOrder, order, order.OfferPrice);

                        continue;
                    }
                    else if (order.OrderType == "M")
                    {
                        ExecuteTrade(tradeOrder, order, currentStock.CurrentValue);
                        continue;
                    }
                }
            }
        }
        private void ExecuteTrade(StockTrade buyOrder, StockTrade sellOrder, decimal dealPrice)
        {
            string buyerCountryId = _webUserRepo.GetCountryId(buyOrder.UserId);
            ExecuteTradeDTO executeTrade = new ExecuteTradeDTO();

            if (buyOrder.LeftUnit > sellOrder.LeftUnit)
            {
                executeTrade.PurchasedUnit = sellOrder.LeftUnit;
            }
            else
            {
                executeTrade.PurchasedUnit = buyOrder.LeftUnit;
            }

            if (executeTrade.PurchasedUnit > 0)
            {
                decimal stocktaxRate = 0;
                try
                {
                    if (buyOrder.UserId > AppSettings.MaxCountryId)
                    {
                        stocktaxRate = _taxRepo.GetCountryTaxByCode(buyerCountryId, AppSettings.TaxStockCode);
                    }
                }
                catch (Exception ex)
                {
                    ExceptionLogging.LogError(ex, "Error to get Tax");

                }
                decimal totalPrice = dealPrice * executeTrade.PurchasedUnit;
                executeTrade.BuyerCountryId = buyerCountryId;
                executeTrade.BuyerId = buyOrder.UserId;
                executeTrade.PurchasedPrice = dealPrice;
                executeTrade.SellerId = sellOrder.UserId;
                executeTrade.SellerStockId = new Guid(sellOrder.UserStockId.ToString());
                executeTrade.StockId = buyOrder.StockId;
                executeTrade.TaxCode = AppSettings.TaxStockCode;
                executeTrade.TaxValue = totalPrice * stocktaxRate / 100;
                executeTrade.TotalValue = totalPrice + executeTrade.TaxValue;

                int result = _stockRepo.ExecuteTrade(executeTrade);
                Console.WriteLine("Trade Executed buy Order {0} sell order {1}", buyOrder.TradeId, sellOrder.TradeId);
                if (result == 0)
                {
                    buyOrder.Status = "M";
                    buyOrder.UpdatedAt = processedTime;
                    sellOrder.UpdatedAt = processedTime;
                    //Not Enough Money return;
                    AddTradeNotification(buyOrder, executeTrade);
                }
                else if (result == 2)
                {
                    buyOrder.Status = "C";
                    buyOrder.UpdatedAt = processedTime;
                    sellOrder.UpdatedAt = processedTime;
                    AddTradeNotification(buyOrder, executeTrade);
                }
                else if (result == 3)  // not enough stock as advertised.
                {
                    sellOrder.Status = "C";
                    sellOrder.UpdatedAt = processedTime;
                    AddTradeNotification(sellOrder, executeTrade);
                }
                else if (result == 1)
                {
                    StockTradeHistory tradeHist = new StockTradeHistory();
                    tradeHist.StockTradeHistoryId = Guid.NewGuid();
                    tradeHist.BuyerId = buyOrder.UserId;
                    tradeHist.SellerId = sellOrder.UserId;
                    tradeHist.StockId = buyOrder.StockId;
                    tradeHist.Unit = executeTrade.PurchasedUnit;
                    tradeHist.DealPrice = dealPrice;
                    tradeHist.UpdatedAt = DateTime.UtcNow;
                    tradeHistories.Add(tradeHist);

                    sellOrder.UpdatedAt = processedTime;
                    buyOrder.UpdatedAt = processedTime;
                    if (buyOrder.LeftUnit > sellOrder.LeftUnit)
                    {
                        sellOrder.Status = "D";
                        buyOrder.LeftUnit -= sellOrder.LeftUnit;
                        buyOrder.Status = "I";
                        sellOrder.LeftUnit = 0;
                    }
                    else if (buyOrder.LeftUnit < sellOrder.LeftUnit)
                    {
                        sellOrder.LeftUnit -= buyOrder.LeftUnit;
                        sellOrder.Status = "I";
                        buyOrder.Status = "D";
                        buyOrder.LeftUnit = 0;

                    }
                    else if (buyOrder.LeftUnit == sellOrder.LeftUnit)
                    {
                        sellOrder.Status = "D";
                        buyOrder.Status = "D";
                        sellOrder.LeftUnit = 0;
                        buyOrder.LeftUnit = 0;

                    }
                    AddTradeNotification(buyOrder, executeTrade);
                    AddTradeNotification(sellOrder, executeTrade);
                }

            }
        }

        private void AddTradeNotification(StockTrade tradeOrder, ExecuteTradeDTO executeTrade)
        {
            if (tradeOrder.UserId <= AppSettings.MaxCountryId)
                return;
            StringBuilder parmText = new StringBuilder();
            short notificationTypeId = 0;
            sbyte priority = 0;

            string tradeType = "Buy";
            string failmsg = AppSettings.UnexpectedErrorMsg;
            if (tradeOrder.TradeType == "S")
            {
                tradeType = "Sell";
            }
            if (tradeOrder.Status == "C" || tradeOrder.Status == "M")
            {
                notificationTypeId = AppSettings.StockBrokerTradeFailNotificationId;
                if (tradeOrder.Status == "M")
                {
                    failmsg = "you do not have enough cash to fullfill your order";
                }
                else if (tradeOrder.Status == "C")
                {
                    failmsg = "you do not have enough stock to fullfill your order";
                }
                parmText.AppendFormat("{0}|{1}|{2}|{3}",
                            tradeType, tradeOrder.LeftUnit,
                            currentStockValues.Find(f => f.StockId == tradeOrder.StockId).StockName,
                            failmsg
                            );
                priority = 8;
            }
            else
            {
                notificationTypeId = AppSettings.StockBrokerTradeSuccessNotificationId;
                parmText.AppendFormat("{0}|{1}|{2}|{3}|{4}",
                            tradeType, tradeOrder.LeftUnit + executeTrade.PurchasedUnit,
                            currentStockValues.Find(f => f.StockId == tradeOrder.StockId).StockName,
                            executeTrade.PurchasedUnit,
                            executeTrade.PurchasedPrice
                            );
                priority = 4;

            }


            userNotif.AddNotification(false, string.Empty,
        notificationTypeId, parmText.ToString(), priority, tradeOrder.UserId);
        }
        private void SaveResult()
        {
            Console.WriteLine("Saving Result");
            List<StockTrade> updatedTrades = pendingTrades.Where(q => q.UpdatedAt == processedTime && q.Status == "I").ToList();
            Guid[] deleteTrades = pendingTrades.Where(q => q.Status == "D" || q.Status == "M" || q.Status == "C").Select(l => l.TradeId).ToArray();

            int tradeupdates = _stockRepo.UpdateTrades(updatedTrades);
            _stockRepo.RemoveCompletedTrades(deleteTrades);
            int tradehistory = _stockRepo.AddStockTradesHistory(tradeHistories);
            CalculateNewStockPrice();


            string userStockId = string.Join(",", updatedTrades.Select(x => x.UserStockId));
            response.Append(Environment.NewLine);
            response.Append(userStockId);
            response.Append(Environment.NewLine);
            response.Append("Total Updates done to DB: ");
            response.Append(tradeupdates);
        }



        private void CalculateNewStockPrice()
        {
            List<StockPriceDTO> todaysprices = _stockRepo.GetTodaysStock();
            List<StockHistory> stockhistorys = new List<StockHistory>();

            DateTime updatedTime = DateTime.UtcNow;
            foreach (var item in currentStockValues)
            {
                decimal previousvalue = item.PreviousDayValue;
                if (item.UpdatedAt.Date.AddDays(1) <= updatedTime.Date)
                {
                    item.PreviousDayValue = item.CurrentValue;
                    item.UpdatedAt = updatedTime;
                }
                decimal newPrice = item.CurrentValue;
                if (stockHistoryQuote.Count > 0)
                {
                    newPrice = (decimal)stockHistoryQuote.Find(x => x.Symbol == item.Ticker).Close;
                }

                if (todaysprices.Where(c => c.StockId == item.StockId).Sum(d => d.TotalUnit) > RulesSettings.StockThresholdForStockPrice)
                {
                    StockPriceDTO newstockprice = todaysprices.Find(f => f.StockId == item.StockId);
                    newPrice += newstockprice.Price * 3;
                    newPrice = newPrice / 4; //3 parts from trade 1 parts from history data of yql
                    UpdateStockHistory(item, newPrice, updatedTime, previousvalue, stockhistorys);
                }
                else if (Math.Abs(newPrice - item.CurrentValue) > 5)
                {
                    newPrice += item.CurrentValue * 3;
                    newPrice = newPrice / 4;
                    UpdateStockHistory(item, newPrice, updatedTime, previousvalue, stockhistorys);
                }

            }
            if (stockhistorys.Count > 0)
            {
                _stockRepo.ExpireCurrentStock();
                _stockRepo.UpdateStocks(currentStockValues.Where(q => q.UpdatedAt == updatedTime).ToList());
                _stockRepo.AddStockHistory(stockhistorys);
            }
        }

        private void UpdateStockHistory(Stock item, decimal newPrice, DateTime updatedTime, decimal previousvalue, List<StockHistory> stockhistorys)
        {
            item.UpdatedAt = updatedTime;
            item.CurrentValue = newPrice;
            item.DayChange = item.CurrentValue - previousvalue;
            item.DayChangePercent = item.DayChange / previousvalue * 100;
            if (item.CurrentValue != previousvalue)
            {
                StockHistory stockHistory = new StockHistory
                {
                    HistoryId = Guid.NewGuid(),
                    UpdatedAt = DateTime.UtcNow,
                    StockId = item.StockId,
                    CurrentValue = item.CurrentValue
                };
                stockhistorys.Add(stockHistory);
            }
            Console.WriteLine("Stock {0} New Price {1}", item.StockName, item.CurrentValue);

        }
    }
}
