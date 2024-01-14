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
    public class WeaponDetailsDTORepository : IWeaponDetailsDTORepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public WeaponDetailsDTORepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public WeaponDetailsDTORepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }
        public string GetTop10WeaponStackCountryJson()
        {
            string weaponTopNData = cache.GetStringKey(AppSettings.RedisKeyWeaponSummaryTop10);
            if (weaponTopNData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmLimit", 10);
                weaponTopNData = JsonConvert.SerializeObject(
                     spContext.GetSqlData<TopTenWeaponStackCountryDTO>(
                AppSettings.SPGetTopNWeaponStackCountry,
                dictionary));
                cache.SetStringKey(AppSettings.RedisKeyWeaponSummaryTop10, weaponTopNData,
                    AppSettings.WeaponSummaryTop10CacheLimit);
            }
            return (weaponTopNData);
        }

        public string GetWeaponCodesJson()
        {
            string WeaponCodeData = cache.GetStringKey(AppSettings.RedisKeyWeaponCodes);
            if (WeaponCodeData == null)
            {
                WeaponCodeData = JsonConvert.SerializeObject(spContext.GetSqlDataNoParms<WeaponTypeDTO>(AppSettings.SPWeaponCodesList));
                cache.SetStringKey(AppSettings.RedisKeyWeaponCodes, WeaponCodeData);
            }
            return (WeaponCodeData);
        }
        public string GetWeaponSummaryJson(string countryId)
        {
            string WeaponSummaryData = cache.GetStringKey(AppSettings.RedisKeyWeaponSummary + countryId.ToUpper());
            if (WeaponSummaryData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryId);
                WeaponSummaryData = JsonConvert.SerializeObject(spContext.GetSqlData<WeaponSummaryDTO>(AppSettings.SPgetWeaponSummary, dictionary));
                cache.SetStringKey(AppSettings.RedisKeyWeaponSummary + countryId.ToUpper(), WeaponSummaryData, AppSettings.WeaponSummaryCacheLimit);
            }
            return (WeaponSummaryData);
        }

        public IQueryable<WeaponInventoryDTO> GetWeaponInventory(string countryId, int lastWeaponId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", countryId);
            dictionary.Add("parmlastWeaponId", lastWeaponId);
            dictionary.Add("parmLimit", AppSettings.WeaponInventoryLimit);
            IEnumerable<WeaponInventoryDTO> weaponInventory =
                spContext.GetSqlData<WeaponInventoryDTO>(AppSettings.SPGetWeaponListByCountry, dictionary);
            return weaponInventory.AsQueryable();
        }
        public int GetWeaponAssetCount(string countryId)
        {
            string WeaponAssetData = cache.GetStringKey(AppSettings.RedisKeyCountryAsset + countryId.ToLower());
            if (WeaponAssetData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryId);
                WeaponAssetData = JsonConvert.SerializeObject(spContext.GetSqlDataSignleValue(AppSettings.SPGetWeaponStatByCountry, dictionary, "TotalAsset"));
                if (WeaponAssetData == "null")
                {
                    WeaponAssetData = "0";
                }
                cache.SetStringKey(AppSettings.RedisKeyCountryAsset + countryId.ToLower(), WeaponAssetData, AppSettings.CountryProfileCacheLimit);
            }
            return (Convert.ToInt32(Convert.ToDouble(WeaponAssetData)));
        }

        public bool SaveWeaponCart(CountryWeapon[] weaponCartList, Guid taskId)
        {
            bool result = false;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmTaskId", taskId);
                dictionary.Add("parmBudgetType", AppSettings.NationalBudgetType);
                CountryBudgetByType budgetType =
                    spContext.GetByPrimaryKey<CountryBudgetByType>(dictionary);
                foreach (CountryWeapon item in weaponCartList)
                {
                    budgetType.AmountLeft -= item.PurchasedPrice;

                    if (budgetType.AmountLeft >= 0)
                    {
                        spContext.Add(item);
                    }
                    else
                    {
                        ExceptionLogging.LogError(new Exception("Invalid Amount recived on SaveWeaponCart"), budgetType.AmountLeft.ToString());
                    }

                }
                spContext.Update(budgetType);

                result = true;
                return result;
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogError(ex, "Error to SaveWeaponCart");
                return false;
            }
        }
        public int GetCountryDefenseAssetRank(string countryCode)
        {
            long? countryDefenseAssetRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryDefenseAsset, countryCode.ToLower());
            if (countryDefenseAssetRank == null)
            {
                PopulateCountryDefenseAsset();

                countryDefenseAssetRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryDefenseAsset, countryCode.ToLower());

                if (countryDefenseAssetRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countryDefenseAssetRank);
                }
            }
            else
            {
                return Convert.ToInt32(countryDefenseAssetRank);
            }
        }
        private void PopulateCountryDefenseAsset()
        {
            IEnumerable<CountryDefenseAssetDTO> countryPop = (spContext.GetSqlDataNoParms<CountryDefenseAssetDTO>(AppSettings.SPGetCountryDefenseAssetRank));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountryDefenseAsset, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.DefenseScore))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountryDefenseAsset, AppSettings.CountryProfileCacheLimit);

        }
        public int GetCountryOffenseAssetRank(string countryCode)
        {
            long? countryOffenseAssetRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryOffenseAsset, countryCode.ToLower());
            if (countryOffenseAssetRank == null)
            {
                PopulateCountryOffenseAsset();

                countryOffenseAssetRank = cache.GetSortedSetRankRev(AppSettings.RedisSortedSetCountryOffenseAsset, countryCode.ToLower());

                if (countryOffenseAssetRank == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(countryOffenseAssetRank);
                }
            }
            else
            {
                return Convert.ToInt32(countryOffenseAssetRank);
            }
        }
        private void PopulateCountryOffenseAsset()
        {
            IEnumerable<CountryOffenseAssetDTO> countryPop = (spContext.GetSqlDataNoParms<CountryOffenseAssetDTO>(AppSettings.SPGetCountryOffenseAssetRank));

            cache.AddSoretedSets(AppSettings.RedisSortedSetCountryOffenseAsset, countryPop.ToDictionary(x => x.CountryId.ToLower(), x => (Convert.ToDouble(x.OffenseScore))));
            cache.ExpireKey(AppSettings.RedisSortedSetCountryOffenseAsset, AppSettings.CountryProfileCacheLimit);

        }
        public string GetSecurityProfile(string countryId)
        {
            string reidsKey = AppSettings.RedisHashCountryProfile + countryId;
            string profilesecurityData = cache.GetHash(reidsKey, "security");
            if (profilesecurityData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryId);

                profilesecurityData = JsonConvert.SerializeObject(
          spContext.GetSqlData<SecurityProfileDTO>(AppSettings.SPGetSecurityProfile, dictionary));

                cache.SetHash(reidsKey, "security", profilesecurityData);
                cache.ExpireKey(reidsKey, AppSettings.CountryProfileCacheLimit);
            }

            return profilesecurityData;
        }
        public int UpdateWeaponCondition()
        {
            return spContext.ExecuteSPWithOutput(AppSettings.SPWeaponCondition, new Dictionary<string, object>(), "parmCount");
        }
        public void ClearCache()
        {
            cache.Invalidate(AppSettings.RedisKeyWeaponCodes);
        }
    }
}
