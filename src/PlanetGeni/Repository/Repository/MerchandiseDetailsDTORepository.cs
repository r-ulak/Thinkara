using Common;
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
    public class MerchandiseDetailsDTORepository : IMerchandiseDetailsDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public MerchandiseDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public MerchandiseDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public UserMerchandise GetUserMerchandises(BuySellMerchandiseDTO
            merchandiseCart, int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmMerchandiseTypeId", merchandiseCart.MerchandiseTypeId);
            UserMerchandise cartMerchandises =
                spContext.GetByPrimaryKey<UserMerchandise>(dictionary);
            cartMerchandises.UserId = userId;
            cartMerchandises.MerchandiseTypeId = merchandiseCart.MerchandiseTypeId;
            cartMerchandises.MerchandiseCondition = Convert.ToSByte(
                (cartMerchandises.MerchandiseCondition * cartMerchandises.Quantity +
                100 * merchandiseCart.Quantity) / (cartMerchandises.Quantity + merchandiseCart.Quantity));
            cartMerchandises.PurchasedPrice = (
                merchandiseCart.Cost * merchandiseCart.Quantity +
                cartMerchandises.PurchasedPrice * cartMerchandises.Quantity
                ) / (cartMerchandises.Quantity + merchandiseCart.Quantity);
            cartMerchandises.Quantity += merchandiseCart.Quantity;
            cartMerchandises.Tax += merchandiseCart.Tax;
            cartMerchandises.PurchasedAt = DateTime.UtcNow;


            return cartMerchandises;

        }
        public string GetMerchandiseCodesJson(MerchandiseCodeSearchDTO merchandiseCode)
        {
            string redisKey = AppSettings.RedisKeyMerchandiseCodes + "mc" + merchandiseCode.MerchandiseTypeCode + "mt" + merchandiseCode.MerchandiseTypeId;
            string MerchandiseCodeData = cache.GetStringKey(redisKey);
            if (MerchandiseCodeData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLastMerchandiseTypeId", merchandiseCode.MerchandiseTypeId);
                dictionary.Add("parmTypeCode", merchandiseCode.MerchandiseTypeCode);
                dictionary.Add("parmMerchandiseLimit", AppSettings.MerchandiseCodeLimit);
                MerchandiseCodeData = JsonConvert.SerializeObject(spContext.GetSqlData<MerchandiseTypeDTO>(AppSettings.SPMerchandiseCodesList, dictionary));
                cache.SetStringKey(redisKey, MerchandiseCodeData);
            }
            return (MerchandiseCodeData);
        }
        public string[] GetMerchandiseCodesJson(short[] typeIds)
        {
            string[] fields = typeIds.Select(x => x.ToString()).ToArray();
            string[] merchandiseCodeData = cache.GetMultipleHash(AppSettings.RedisHashMerchandiseCodes, fields);
            if (merchandiseCodeData[0] == null)
            {
                IEnumerable<MerchandiseCache> merchandiseCache =
                spContext.GetSqlDataNoParms<MerchandiseCache>(AppSettings.SPGetAllMerchandiseTypeList);
                Dictionary<string, string> merchandiseCodedict = new Dictionary<string, string>();
                foreach (var item in merchandiseCache)
                {
                    merchandiseCodedict.Add(item.MerchandiseTypeId.ToString(),
                         JsonConvert.SerializeObject(item)
                        );
                }
                cache.SetHashDictionary(AppSettings.RedisHashMerchandiseCodes, merchandiseCodedict);
                merchandiseCodeData = merchandiseCodedict.Select(x => x.Value).ToArray();
            }
            return (merchandiseCodeData);
        }
        public List<MerchandiseCache> GetMerchandiseCodesById(short[] typeIds)
        {
            List<MerchandiseCache> merchandiseCache = new List<MerchandiseCache>();
            foreach (var item in GetMerchandiseCodesJson(typeIds))
            {
                merchandiseCache.Add(JsonConvert.DeserializeObject<MerchandiseCache>(item));
            }
            return merchandiseCache;
        }
        public IEnumerable<MerchandiseSummaryDTO> GetMerchandiseSummaryJson(int userId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            IEnumerable<MerchandiseSummaryDTO> merchandiseSummary =
                spContext.GetSqlData<MerchandiseSummaryDTO>(
                AppSettings.SPgetMerchandiseSummary,
                dictionary);

            return merchandiseSummary;
        }

        public string GetTopTenPropertyOwnerJson()
        {
            string merchandiseTopNData = cache.GetStringKey(AppSettings.RedisKeyMerchandiseSummaryTop10);
            if (merchandiseTopNData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", 10);
                merchandiseTopNData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<TopTenPropertyOwnerDTO>(
                AppSettings.SPGetTopNPropertyOwner,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyMerchandiseSummaryTop10, merchandiseTopNData,
                    AppSettings.MerchandiseSummaryTop10CacheLimit);
            }
            return (merchandiseTopNData);
        }

        public IEnumerable<MerchandiseInventoryDTO> GetMerchandiseInventory(int userId, int lastMerchandiseTypeId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmlastMerchandiseTypeId", lastMerchandiseTypeId);
            dictionary.Add("parmLimit", AppSettings.MerchandiseInventoryLimit);
            IEnumerable<MerchandiseInventoryDTO> merchandiseInventory =
                spContext.GetSqlData<MerchandiseInventoryDTO>(AppSettings.SPGetMerchandiseListByUser, dictionary);
            return merchandiseInventory;
        }
        public string GetMerchandiseProfile(int userId)
        {
            string reidsKey = AppSettings.RedisHashUserProfile + userId;
            string profilemerchandiseData = cache.GetHash(reidsKey, "merchandise");
            if (profilemerchandiseData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmUserId", userId);

                profilemerchandiseData = JsonConvert.SerializeObject(
          spContext.GetSqlData<PropertyProfileDTO>(AppSettings.SPGetMerchandiseProfile, dictionary));

                cache.SetHash(reidsKey, "merchandise", profilemerchandiseData);
                cache.ExpireKey(reidsKey, AppSettings.UserProfileCacheLimit);
            }

            return profilemerchandiseData;
        }
        public MerchandiseType GetMerchandiseType(short merchandiseType)
        {
            string reidsKey = AppSettings.RedisHashMerchadiseType;
            string merchandiseData = cache.GetHash(reidsKey, merchandiseType.ToString());
            if (merchandiseData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmMerchandiseTypeId", merchandiseType);
                var data = spContext.GetByPrimaryKey<MerchandiseType>(dictionary);
                merchandiseData = JsonConvert.SerializeObject(data
          );
                cache.SetHash(reidsKey, merchandiseType.ToString(), merchandiseData);
                return data;
            }

            return JsonConvert.DeserializeObject<MerchandiseType>(merchandiseData);
        }
        public bool HasThisMerchandise(int userId, short[] items)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string itemList = String.Join(",", items);
            dictionary.Add("parmMerchandiseIdList", itemList);
            dictionary.Add("parmUserId", userId);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPHasThisMerchandise, dictionary, "cnt"));
            if (count == items.Length)
            {
                return true;
            }
            return false;
        }

        public int GetMerchandiseByQty(int userId, short[] items, int qty)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string itemList = String.Join(",", items);
            dictionary.Add("parmMerchandiseIdList", itemList);
            dictionary.Add("parmUserId", userId);
            dictionary.Add("parmQuantity", qty);
            int count = Convert.ToInt32(spContext.GetSqlDataSignleValue(AppSettings.SPGetCountMerchandiseByQty
                , dictionary, "cnt"));
            return count;
        }
        public decimal GetMerchandiseTotal(int userId, short[] items)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            string itemList = String.Join(",", items);
            dictionary.Add("parmMerchandiseIdList", itemList);
            dictionary.Add("parmUserId", userId);
            decimal sum = Convert.ToDecimal(spContext.GetSqlDataSignleValue(
                AppSettings.SPGetMerchandiseTotal, dictionary, "totalPrice"));
            return sum;
        }
        public bool SaveMerchandiseCart(BuySellMerchandiseDTO[]
            merchandiseCartList, int userId, string countryId)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmSourceId", userId);
                dictionary.Add("parmCountryId", countryId);
                dictionary.Add("parmTaskId", Guid.NewGuid());
                dictionary.Add("parmFundType", AppSettings.PropertyFundType);
                dictionary.Add("parmTaxCode", AppSettings.TaxMerchandiseCode);
                dictionary.Add("parmAmount", merchandiseCartList.Sum(x => x.Cost));
                dictionary.Add("parmTaxAmount", merchandiseCartList.Sum(x => x.Tax));

                List<UserMerchandise> merchandiseList = new List<UserMerchandise>();
                foreach (BuySellMerchandiseDTO item in merchandiseCartList)
                {
                    UserMerchandise userMerchandise = GetUserMerchandises(item, userId);
                    merchandiseList.Add(userMerchandise);
                }

                int response = (int)spContext.GetSqlDataSignleValue
            (AppSettings.SPExecutePayWithTaxBank, dictionary, "result");

                if (response != 1)
                {
                    return false;
                }
                spContext.AddUpdateList(merchandiseList);
                string reidsKey = AppSettings.RedisHashUserProfile + userId.ToString();
                cache.SetHash(reidsKey, "merchandise", null);
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveMerchandiseCart");
                return false;
            }
        }
        public bool SaveSellMerchandiseCart(BuySellMerchandiseDTO[]
      sellingItems, int userId, string countryId)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                decimal credit = 0;
                decimal resaleRate = 0;
                List<MerchandiseCache> merchandiseCodeList =
                    GetMerchandiseCodesById(sellingItems.Select(x => x.MerchandiseTypeId).ToArray());
                List<UserMerchandise> merchandiseList = new List<UserMerchandise>();
                foreach (BuySellMerchandiseDTO item in sellingItems)
                {
                    dictionary.Clear();
                    dictionary.Add("parmUserId", userId);
                    dictionary.Add("parmMerchandiseTypeId", item.MerchandiseTypeId);

                    UserMerchandise userMerchandise =
                    spContext.GetByPrimaryKey<UserMerchandise>(dictionary);
                    resaleRate = merchandiseCodeList.First(x => x.MerchandiseTypeId == item.MerchandiseTypeId).ResaleRate;
                    if (item.SellingUnit <= userMerchandise.Quantity)
                    {
                        credit += userMerchandise.PurchasedPrice * (resaleRate / 100) * (userMerchandise.MerchandiseCondition) / 100 * item.SellingUnit;

                        userMerchandise.Quantity -= item.SellingUnit;
                        merchandiseList.Add(userMerchandise);
                    }

                }
                ICountryTaxDetailsDTORepository merchandiseTax = new CountryTaxDetailsDTORepository();
                decimal tax = merchandiseTax.GetCountryTaxByCode(countryId, AppSettings.TaxIncomeCode);

                dictionary.Clear();
                dictionary.Add("parmSourceId", userId);
                dictionary.Add("parmCountryId", countryId);
                dictionary.Add("parmTaskId", Guid.NewGuid());
                dictionary.Add("parmFundType", AppSettings.PropertyFundType);
                dictionary.Add("parmTaxCode", AppSettings.TaxIncomeCode);
                dictionary.Add("parmAmount", -credit);
                dictionary.Add("parmTaxAmount", credit * tax / 100);
                int response = (int)spContext.GetSqlDataSignleValue
                                (AppSettings.SPExecutePayWithTaxBank, dictionary, "result");

                if (response != 1)
                {
                    return false;
                }
                spContext.UpdateList(merchandiseList);

                string reidsKey = AppSettings.RedisHashUserProfile + userId.ToString();
                cache.SetHash(reidsKey, "merchandise", null);
                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveMerchandiseCart");
                return false;
            }
        }
        public int ProcessUserWithoutHouse(string countryId, int countryUserId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", countryId);
            dictionary.Add("parmCountryUserId", countryUserId);
            dictionary.Add("parmFundType", AppSettings.RentFundType);
            dictionary.Add("parmNotificationTypeId", AppSettings.RentalPaymentNotificationId);
            dictionary.Add("parmTaxCode", AppSettings.TaxMerchandiseCode);
            dictionary.Add("parmWeeklyRent", RulesSettings.WeeklyRent);
            dictionary.Add("parmRentalPenalty", RulesSettings.NorentPenalty);



            return
            spContext.ExecuteSPWithOutput(AppSettings.SPProcessUserWithoutHouse, dictionary, "parmCount");

        }
        public int ProcessUserWithRentalProperty(string countryId, decimal taxRate)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmBankId", AppSettings.BankId);
            dictionary.Add("parmCountryId", countryId);
            dictionary.Add("parmFundType", AppSettings.RentFundType);
            dictionary.Add("parmTaxRate", taxRate);
            dictionary.Add("parmNotificationTypeId", AppSettings.RentalCollectionNotificationId);
            dictionary.Add("parmPostContentTypeId", AppSettings.RentalPostContentTypeId);
            dictionary.Add("parmTaxCode", AppSettings.TaxIncomeCode);
            dictionary.Add("parmRentalCap", RulesSettings.ColleRentalCapForPost);



            return
            spContext.ExecuteSPWithOutput(AppSettings.SPProcessUserWithRentalProperty, dictionary, "parmCount");

        }
        public int UpdateMerchandiseCondition()
        {
            return spContext.ExecuteSPWithOutput(AppSettings.SPMerchandiseCondition, new Dictionary<string, object>(), "parmCount");
        }

        public void ClearCache()
        {
            cache.Invalidate(AppSettings.RedisKeyMerchandiseCodes);
            cache.Invalidate(AppSettings.RedisKeyMerchandiseSummaryTop10);
        }
    }
}
