using Common;
using Dao.Models;
using DAO;
using DAO.Models;
using DataCache;
using DTO.Custom;
using DTO.Db;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UserBankAccountDTORepository : IUserBankAccountDTORepository
    {
        private StoredProcedure spContext = new StoredProcedure();
        private IRedisCacheProvider cache { get; set; }
        public UserBankAccountDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }
        public UserBankAccountDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public UserBankAccount GetUserBankDetails(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            UserBankAccount userBankDetails =
                spContext.GetByPrimaryKey<UserBankAccount>(dictionary);
            return userBankDetails;
        }

        public bool UpdateBankAc(decimal delta, int userId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                dictionary.Add("parmDelta", delta);
                int result = (int)spContext.GetSqlDataSignleValue
                    (AppSettings.SPExecuteUpdateUserBankAc, dictionary, "result");
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to UpdateBankAc");
                return false;
            }
        }
        public decimal GetNetWorth(int userId)
        {
            List<BankViewDTO> bankAcView = GetBankAccountViewInfo(userId).ToList();

            decimal netWorth = bankAcView.Where(f => f.CapitalType != "LoanLeftToPay").Sum(x => x.Total);
            netWorth -= bankAcView.Find(f => f.CapitalType == "LoanLeftToPay").Total;
            return netWorth;
        }

        public IEnumerable<BankViewDTO> GetBankAccountViewInfo(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            Task<UserBankAccount> taskbankAc =
                 Task<UserBankAccount>.Factory.StartNew(() => GetUserBankDetails(userId));
            List<BankViewDTO> bankAcView =
                 spContext.GetSqlData<BankViewDTO>
                 (AppSettings.SPGetFinanceContent, dictionary).ToList();
            List<CapitalType> metalPrice = JsonConvert.DeserializeObject<List<CapitalType>>(
             GetMetalPrices());
            taskbankAc.Wait();
            UserBankAccount bankAc = taskbankAc.Result;
            bankAcView.Add(new BankViewDTO
            {
                CapitalType = "Cash",
                ImageFont = "fa icon-money28 text-success",
                Total = bankAc.Cash
            });
            bankAcView.Add(new BankViewDTO
            {
                CapitalType = "Silver",
                ImageFont = "fa icon-coins2 silvercolor",
                Total = bankAc.Silver * metalPrice.Find(f => f.CapitalTypeId == 2).Cost
            });
            bankAcView.Add(new BankViewDTO
            {
                CapitalType = "Gold",
                ImageFont = "fa icon-gold2 goldcolor",
                Total = bankAc.Gold * metalPrice.Find(f => f.CapitalTypeId == 1).Cost
            });


            return bankAcView;
        }

        public string GetTopTenRichestJson()
        {
            string richestTopNData = cache.GetStringKey(AppSettings.RedisKeyRichestSummaryTop10);
            if (richestTopNData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", 25);
                richestTopNData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<TopTenUserCapital>(
                AppSettings.SPGetTopNRichest,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyRichestSummaryTop10, richestTopNData,
                    AppSettings.StockSummaryTop10CacheLimit);
            }
            return (richestTopNData);
        }

        public IEnumerable<CapitalTransactionDTO> GetBankStatement
       (int userId, DateTime? parmlastDateTime = null)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmLastCreatedAt", parmlastDateTime);
            dictionary.Add("parmLimit", AppSettings.CapitalTrnLimit);
            IEnumerable<CapitalTransactionDTO> captialTrn =
                spContext.GetSqlData<CapitalTransactionDTO>
                (AppSettings.SPGetCapitalTransactionLogById, dictionary);
            return captialTrn;
        }

        public string GetMetalPrices()
        {
            string metalPriceData = cache.GetStringKey(AppSettings.RedisKeyMetalPrices);
            if (metalPriceData == null)
            {
                metalPriceData = JsonConvert.SerializeObject(
                     spContext.GetSqlDataNoParms<CapitalType>(
                AppSettings.SPGetAllCapitalType));
                cache.SetStringKey(AppSettings.RedisKeyMetalPrices, metalPriceData);
            }
            return (metalPriceData);
        }
        public string GetAllFundTypes()
        {
            string fundtypeData = cache.GetStringKey(AppSettings.RedisKeyFundTypes);
            if (fundtypeData == null)
            {
                fundtypeData = JsonConvert.SerializeObject(
                     spContext.GetSqlDataNoParms<FundTypeCode>(
                AppSettings.SPGetAllFundType));
                cache.SetStringKey(AppSettings.RedisKeyFundTypes, fundtypeData);
            }
            return (fundtypeData);
        }

        public bool SaveBuySellMetalCart(BuySellMetalDTO metalCart)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", metalCart.UserId);
                dictionary.Add("parmDelta", metalCart.Delta);
                dictionary.Add("parmDeltaGold", metalCart.GoldDelta);
                dictionary.Add("parmDeltaSilver", metalCart.SilverDelta);
                dictionary.Add("parmOrderType", metalCart.OrderType);
                dictionary.Add("parmFundType", AppSettings.MetalFundType);
                int result = (int)spContext.GetSqlDataSignleValue
               (AppSettings.SPExecuteBuySellGoldAndSilver, dictionary, "result");
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveBuySellMetalCart");
                return false;
            }
        }

        public int GetCountryCitizenWealthRank(string countryCode)
        {
            long? countryCitizenWealthRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryCitizenWealth, countryCode.ToLower());
            if (countryCitizenWealthRank == null)
            {
                PopulateCountryCitizenWealth();

                countryCitizenWealthRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryCitizenWealth, countryCode.ToLower());

                if (countryCitizenWealthRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countryCitizenWealthRank);
                }
            }
            else
            {
                return Convert.ToInt32(countryCitizenWealthRank);
            }
        }
        private void PopulateCountryCitizenWealth()
        {
            IEnumerable<CountryRichestCitizenDTO> countryPop = (spContext.GetSqlDataNoParms<CountryRichestCitizenDTO>(AppSettings.SPGetCountryCitizenWealthLevel));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountryCitizenWealth, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.AverageNetWorth))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountryCitizenWealth, AppSettings.CountryProfileCacheLimit);

        }
        public int PayMe(PayMeDTO payMe)
        {
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("parmUserId", payMe.ReciepentId);
                dictionary.Add("parmSourceId", payMe.SourceUserId);
                dictionary.Add("parmCountryId", payMe.CountryId);
                dictionary.Add("parmTaskId", payMe.TaskId);
                dictionary.Add("parmFundType", payMe.FundType);
                dictionary.Add("parmTaxCode", payMe.TaxCode);
                dictionary.Add("parmAmount", payMe.Amount);
                dictionary.Add("parmTaxAmount", payMe.Tax);

                int response = (int)spContext.GetSqlDataSignleValue
              (AppSettings.SPExecutePayMe, dictionary, "result");

                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to PayAds");
                return 2;
            }

        }
        public int PayNation(PayNationDTO payNation)
        {
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("parmUserId", payNation.UserId);
                dictionary.Add("parmCountryUserId", payNation.CountryUserId);
                dictionary.Add("parmCountryId", payNation.CountryId);
                dictionary.Add("parmTaskId", payNation.TaskId);
                dictionary.Add("parmFundType", payNation.FundType);
                dictionary.Add("parmTaxCode",payNation.TaxCode);
                dictionary.Add("parmAmount", payNation.Amount);
                dictionary.Add("parmTaxAmount", payNation.Tax);

                int response = (int)spContext.GetSqlDataSignleValue
              (AppSettings.SPExecutePayNation, dictionary, "result");

                return response;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to PayAds");
                return 2;
            }

        }

        public void ApplyCreditScore(decimal  deltascore, int userId)
        { 
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);
                CreditScore creditscore = spContext.GetByPrimaryKey<CreditScore>(dictionary);
                creditscore.Score += deltascore;
                spContext.AddUpdate(creditscore);

        }
    }
}
