using Common;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class GiftDTORepository : IGiftDTORepository
    {
        private StoredProcedure spContext = new StoredProcedure();

        public GiftDTORepository()
        {
        }

        public GiftRateDTO GetGiftRate(string countryId)
        {

            try
            {
                ICountryTaxDetailsDTORepository _countryrepository = new CountryTaxDetailsDTORepository();
                IStockDTORepository _stockrepository = new StockDTORepository();
                decimal taxRate = _countryrepository.GetCountryTaxByCode(countryId,
          AppSettings.TaxGiftCode);

                Stock gold = JsonConvert.DeserializeObject<Stock>(_stockrepository.GetStockByIdJson(AppSettings.GoldStockType));
                Stock silver = JsonConvert.DeserializeObject<Stock>(_stockrepository.GetStockByIdJson(AppSettings.SilverStockType));

                GiftRateDTO giftRate = new GiftRateDTO()
                {
                    TaxRate = taxRate,
                    CurrentGoldValue = gold.CurrentValue,
                    CurrentSilverValue = silver.CurrentValue
                };


                return giftRate;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to Send GiftRate");

                return null;
            }



        }
        public bool SaveSendGift(GiftDTO gifts,
            UserBankAccount bankAc,
            int userid, GiftRateDTO giftRate, string fullName)
        {
            bool result = false;
            try
            {
                decimal taxAmount = 0;
                decimal taxableAmount = 0;
                taxableAmount = (gifts.Gold * giftRate.CurrentGoldValue +
                             gifts.Silver * giftRate.CurrentSilverValue +
                             gifts.Cash);
                taxAmount = taxableAmount * giftRate.TaxRate / 100;
                gifts.ToId.AddRange(gifts.NationId);
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (var recipent in gifts.ToId)
                {
                    Gift userGift = new Gift()
                    {
                        CreatedAt = DateTime.UtcNow,
                        FromId = userid,
                        GiftId = Guid.NewGuid(),
                        TaxAmount = taxAmount,
                        ToId = recipent,
                        Cash = gifts.Cash,
                        Gold = gifts.Gold,
                        Silver = gifts.Silver
                    };
                    dictionary.Clear();
                    dictionary.Add("parmUserId", recipent);
                    UserBankAccount bankAccount = spContext.GetByPrimaryKey<UserBankAccount>(dictionary);
                    bankAccount.Gold += gifts.Gold;
                    bankAccount.Cash += gifts.Cash;
                    bankAccount.Silver += gifts.Silver;
                    bankAccount.UpdatedAt = DateTime.UtcNow;
                    spContext.Update(bankAccount);
                    CapitalTransactionLog capitallog = new CapitalTransactionLog
                    {
                        Amount = taxableAmount,
                        CreatedAT = DateTime.UtcNow,
                        FundType = AppSettings.GiftFundType,
                        LogId = Guid.NewGuid(),
                        RecipentId = recipent,
                        SourceId = userid,
                        TaxAmount = taxAmount,
                        SourceGuid = Guid.Empty,
                        RecipentGuid = Guid.Empty,

                        
                    };
                    spContext.Add(capitallog);
                    spContext.Add(userGift);

                    SendReciveGiftNotficationMsg("Capital", taxableAmount, userid, fullName, recipent);
                }
                spContext.Update(bankAc);

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveSendGift");
                return false;
            }
        }

        private void SendReciveGiftNotficationMsg(string giftType, decimal giftValue, int userid, string fullName, int reciverId)
        {
            String parmText = "";

            parmText = string.Format("{0}|{1}|{2}|{3}|<strong>Date:{4}</strong>",
                       giftType, giftValue, userid, fullName, DateTime.UtcNow);
            IUserNotificationDetailsDTORepository userNotif = new UserNotificationDetailsDTORepository();
            userNotif.AddNotification(false, string.Empty,
                                      AppSettings.RecivedGiftFailNotificationId,
                                      parmText, 2, reciverId);


        }

        public bool SaveSendPropertyGift(GiftDTO gifts,
    UserBankAccount bankAc,
    int userid, GiftRateDTO giftRate, string fullName)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                foreach (short item in gifts.MerchandiseTypeId)
                {
                    dictionary.Add("parmMerchandiseTypeId", item);
                    dictionary.Add("parmUserId", userid);
                    UserMerchandise userMerchandise =
                         spContext.GetByPrimaryKey<UserMerchandise>(dictionary);
                    dictionary.Clear();
                    Gift userGift = new Gift()
                    {
                        CreatedAt = DateTime.UtcNow,
                        FromId = userid,
                        TaxAmount = userMerchandise.PurchasedPrice * giftRate.TaxRate / 100,
                        MerchandiseTypeId = item,
                        MerchandiseValue = userMerchandise.PurchasedPrice
                    };
                    BuySellMerchandiseDTO addMerchandise = new BuySellMerchandiseDTO()
                    {
                        Cost = userMerchandise.PurchasedPrice,
                        Quantity = 1,
                        MerchandiseTypeId = userMerchandise.MerchandiseTypeId,
                        Tax = userGift.TaxAmount
                    };
                    foreach (var recipent in gifts.ToId)
                    {
                        userGift.ToId = recipent;
                        userGift.GiftId = Guid.NewGuid();

                        spContext.Add(userGift);
                        IMerchandiseDetailsDTORepository merchandiseRepo = new MerchandiseDetailsDTORepository();
                        spContext.AddUpdate(merchandiseRepo.GetUserMerchandises(addMerchandise, recipent));
                        userMerchandise.Quantity -= 1;
                        SendReciveGiftNotficationMsg("Property", userMerchandise.PurchasedPrice, userid, fullName, recipent);

                    }
                    spContext.Update(userMerchandise);

                }
                spContext.Update(bankAc);

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveSendPropertyGift");
                return false;
            }
        }

    }
}
