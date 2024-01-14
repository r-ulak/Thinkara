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
    public class CountryLeaderRepository : ICountryLeaderRepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public CountryLeaderRepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public CountryLeaderRepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetActiveSeneatorJson(string countryId)
        {
            string activeSenetaorsData = cache.GetStringKey(AppSettings.RedisKeyActiveSeneator + countryId);
            if (activeSenetaorsData == null)
            {
                activeSenetaorsData = JsonConvert.SerializeObject(GetActiveSeneator(countryId));
                cache.SetStringKey(AppSettings.RedisKeyActiveSeneator + countryId, activeSenetaorsData);
            }
            return (activeSenetaorsData);
        }

        public IEnumerable<CountryLeader> GetActiveSeneator(string countryId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("parmCountryId", countryId);
            dictionary.Add("parmPostiontypeId", AppSettings.SenatorPositionType);
            return spContext.GetSqlData<CountryLeader>(AppSettings.SPGetActiveLeaders, dictionary);

        }

        public int GetTotalActiveSeneator(string countryId)
        {
            string activeSenetaorsCountData = cache.GetStringKey(AppSettings.RedisKeyActiveSeneatorCount + countryId);
            if (activeSenetaorsCountData == null)
            {
                List<CountryLeader> leaders =
                JsonConvert.DeserializeObject<List<CountryLeader>>(GetActiveSeneatorJson(countryId));
                activeSenetaorsCountData = leaders.Count.ToString();
                cache.SetStringKey(AppSettings.RedisKeyActiveSeneatorCount + countryId, activeSenetaorsCountData);
            }
            return Convert.ToInt32(activeSenetaorsCountData);
        }
        public string GetActiveLeadersProfile(string countryId)
        {
            string reidsKey = AppSettings.RedisHashCountryProfile + countryId;
            string profilesecurityData = cache.GetHash(reidsKey, "leader");
            if (profilesecurityData == null)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("parmCountryId", countryId);

                profilesecurityData = JsonConvert.SerializeObject(
          spContext.GetSqlData<CountryLeaderProfileDTO>(AppSettings.SPGetActiveLeadersProfile, dictionary));

                cache.SetHash(reidsKey, "leader", profilesecurityData);
                cache.ExpireKey(reidsKey, AppSettings.CountryProfileCacheLimit);
            }

            return profilesecurityData;
        }

        public void ClearCache(string countryId)
        {
            cache.Invalidate(AppSettings.RedisKeyActiveSeneator + countryId);
        }
    }
}
