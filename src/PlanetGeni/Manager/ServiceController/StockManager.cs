
using Common;
using DAO.Models;
using DTO.Db;
using Newtonsoft.Json;
using Repository;
using RulesEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.ServiceController
{
    public class StockManager
    {
        IStockDTORepository _repository;

        private IUserNotificationDetailsDTORepository userNotif;
        public StockManager(IStockDTORepository repo)
        {
            _repository = repo;
            userNotif = new UserNotificationDetailsDTORepository();
        }
        public StockManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            _repository = new StockDTORepository();
        }

        public void ProcessBuyStockCart(BuySellStockDTO[] stockList, int userid, string countryId)
        {
            string stockCodesJson = _repository.GetCurrentStockJson();

            ICountryTaxDetailsDTORepository stockTax = new CountryTaxDetailsDTORepository();
            decimal tax = stockTax.GetCountryTaxByCode(countryId, AppSettings.TaxStockCode);
            IUserBankAccountDTORepository bankAc = new UserBankAccountDTORepository();
            UserBankAccount buyerBankAccount = bankAc.GetUserBankDetails(userid);

            StockRules stockCartrules =
            new StockRules(stockList, stockCodesJson,
                buyerBankAccount, tax);
            String parmText = "";
            short notificationTypeId = 0;
            ValidationResult validationResult = stockCartrules.IsValid();
            DateTime dateTime = DateTime.UtcNow;
            sbyte priority = 0;
            if (validationResult == ValidationResult.Success)
            {
                bool result = _repository.SaveBuyStockCart(
                    stockList, userid);

                /// TODO : If 
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Buy",
                        dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.StockTradeFailNotificationId;
                    priority = 7;

                }
                else
                {
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Buy",
                dateTime.ToString());
                    notificationTypeId = AppSettings.StockTradeSuccessNotificationId;
                }
            }
            else
            {
                parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Buy",
     dateTime.ToString(), validationResult.ErrorMessage);
                notificationTypeId = AppSettings.StockTradeFailNotificationId;
                priority = 6;
            }
            userNotif.AddNotification(false, string.Empty,
                 notificationTypeId, parmText.ToString(), priority, userid);
        }
        public void ProcessSellStockCart(BuySellStockDTO[] stockCartList, int userid)
        {
            short[] stockIds = stockCartList.Select(x => x.StockId).ToArray();
            StockRules stockCartrules =
            new StockRules(_repository.HasThisStock(userid, stockIds));
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            String parmText = "";
            short notificationTypeId = 0;
            ValidationResult validationResult = stockCartrules.IsValidSellCart();
            DateTime dateTime = DateTime.UtcNow;
            sbyte priority = 0;
            if (validationResult == ValidationResult.Success)
            {
                bool result = _repository.SaveSellStockCart(stockCartList, userid);

                /// TODO : If 
                if (!result)
                {
                    //Add a notification to resubmit 
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Sell",
                        dateTime.ToString(), AppSettings.UnexpectedErrorMsg);
                    notificationTypeId = AppSettings.StockTradeFailNotificationId;
                    priority = 7;

                }
                else
                {
                    parmText = string.Format("{0}|<strong>Date:{1}</strong>", "Sell",
                         dateTime.ToString());
                    notificationTypeId = AppSettings.StockTradeSuccessNotificationId;

                }
            }
            else
            {
                parmText = string.Format("{0}|<strong>Date:{1}</strong>|{2}", "Sell",
     dateTime.ToString(), validationResult.ErrorMessage);
                notificationTypeId = AppSettings.StockTradeFailNotificationId;
                priority = 6;
            }
            userNotif.AddNotification(false, string.Empty,
                 notificationTypeId, parmText.ToString(), priority, userid);

        }
        public void ProcessOrderStockCart(Guid[] stockList, int userid)
        {
            String parmText = "";
            short notificationTypeId = 0;
            DateTime dateTime = DateTime.UtcNow;
            sbyte priority = 0;
            int count = 0;
            for (int i = 0; i < stockList.Length; i++)
            {
                count += _repository.TryCancelStockOrder(stockList[i], userid);
            }
            bool result = false;
            if (count > 0)
            {
                result = true;
            }
            if (!result)
            {
                //Add a notification to resubmit 
                parmText = string.Format("{0}|{1}", stockList.Length,
                     AppSettings.UnexpectedErrorMsg);
                notificationTypeId = AppSettings.StockTradeCancelFailOrderNotificationId;
                priority = 7;
            }
            else
            {
                parmText = string.Format("{0}|{1}", count, stockList.Length);
                notificationTypeId = AppSettings.StockTradeCancelOrderNotificationId;
            }
            userNotif.AddNotification(false, string.Empty,
                 notificationTypeId, parmText.ToString(), priority, userid);
        }
    }
}


