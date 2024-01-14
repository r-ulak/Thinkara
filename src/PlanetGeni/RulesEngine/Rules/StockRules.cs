using DAO.Models;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine
{
    public class StockRules : IRules
    {
        public BuySellStockDTO[] StockList;
        private decimal TaxRate;
        private List<Stock> StockCodeList;
        private bool HasAllStockList;
        private UserBankAccount BuyerBankAccount;

        public StockRules()
        {

        }
        public StockRules(BuySellStockDTO[] stockList, string stockCodes,
            UserBankAccount buyerBankAccount,
          decimal taxRate)
        {
            StockList = stockList;
            TaxRate = taxRate;
            StockCodeList = JsonConvert.DeserializeObject<List<Stock>>(stockCodes);
            BuyerBankAccount = buyerBankAccount;
        }

        public StockRules(bool hasAllStockList)
        {
            HasAllStockList = hasAllStockList;
        }
        public ValidationResult IsValid()
        {
            decimal totalwithtax = 0;
            foreach (var item in StockList)
            {
                Stock stock =
                     StockCodeList.First(x => x.StockId == item.StockId);
                if (stock.StockId != item.StockId)
                {
                    return new ValidationResult(("invalid Item detected in buy cart"));
                }
                if (item.OrderType == "M")
                {

                    totalwithtax += (stock.CurrentValue * item.Quantity) * (1 + TaxRate / 100);
                }
                if (item.OrderType == "L")
                {

                    totalwithtax += (item.OfferPrice * item.Quantity) * (1 + TaxRate / 100);
                }
            }
            if (totalwithtax > BuyerBankAccount.Cash)
            {
                return new ValidationResult(("not enough cash to buy"));
            }
            return ValidationResult.Success;
        }

        public ValidationResult IsValidSellCart()
        {
            if (HasAllStockList == false)
            {
                return new ValidationResult(("Invalid Item detected in sell cart"));
            }
            return ValidationResult.Success;
        }

        public ValidationResult IsValidCartTxn(bool hasThisStockonPendingTrade)
        {
            if (hasThisStockonPendingTrade == false)
            {
                return new ValidationResult(("Stock Not in Trade Order"));
            }
            return ValidationResult.Success;
        }
        public bool AllowUpdateInsert()
        {
            bool result = false;
            result = true;
            //TODO 
            //Check to see if they have access to Edit PostComment then send 1 else 0.
            return result;
        }

    }
}
