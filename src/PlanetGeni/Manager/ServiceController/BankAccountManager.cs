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
    public class BankAccountManager
    {
        IWebUserDTORepository webRepo;
        IUserBankAccountDTORepository _repository;

        private IUserNotificationDetailsDTORepository userNotif;
        public BankAccountManager(IUserBankAccountDTORepository repo)
        {
            _repository = repo;
            webRepo = new WebUserDTORepository();
            userNotif = new UserNotificationDetailsDTORepository();
        }
        public BankAccountManager()
        {
            userNotif = new UserNotificationDetailsDTORepository();
            webRepo = new WebUserDTORepository();
            _repository = new UserBankAccountDTORepository();
        }

        public void ProcessBuySellMetal(BuySellMetalDTO buysellMetal)
        {
            try
            {
                BankAccountRules bankRule = new BankAccountRules();
                String parmText = "";
                short notificationTypeId = 0;
                UserBankAccount bankAc = _repository.GetUserBankDetails(buysellMetal.UserId);
                ValidationResult validationResult = ValidationResult.Success;
                List<CapitalType> capitalTypes = JsonConvert
                       .DeserializeObject<List<CapitalType>>
                       (_repository.GetMetalPrices());
                if (buysellMetal.OrderType == "B")
                {


                    validationResult =
                       bankRule.IsValidBuy(ref buysellMetal, bankAc, capitalTypes);

                }
                else if (buysellMetal.OrderType == "S")
                {
                    validationResult =
                   bankRule.IsValidSell(ref buysellMetal, bankAc, capitalTypes);
                }
                string orderType = buysellMetal.OrderType == "B" ? "Buy" : "Sell";
                sbyte priority = 0;
                DateTime dateTime = DateTime.UtcNow;
                if (validationResult == ValidationResult.Success)
                {
                    bool result = _repository.SaveBuySellMetalCart(buysellMetal);
                    if (!result)
                    {
                        //Add a notification to resubmit 
                        parmText = string.Format("{0}|{1}|{2}|{3}",
                           orderType, buysellMetal.GoldDelta, buysellMetal.SilverDelta,
                            AppSettings.UnexpectedErrorMsg);
                        notificationTypeId = AppSettings.BuySellMetalFailNotificationId;
                        priority = 7;
                    }
                    else
                    {
                        parmText = string.Format("{0}|{1}|{2}",
                            orderType, buysellMetal.GoldDelta, buysellMetal.SilverDelta);
                        notificationTypeId = AppSettings.BuySellMetalSuccessNotificationId;
                    }
                }
                else
                {
                    parmText = string.Format("{0}|{1}|{2}|{3}",
                     orderType, buysellMetal.GoldDelta, buysellMetal.SilverDelta,
                     validationResult.ErrorMessage);
                    notificationTypeId = AppSettings.BuySellMetalFailNotificationId;
                    priority = 6;
                }
                userNotif.AddNotification(false, string.Empty,
           notificationTypeId, parmText.ToString(), priority, buysellMetal.UserId);
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to ProcessAppForOffice");
            }
        }


    }
}


