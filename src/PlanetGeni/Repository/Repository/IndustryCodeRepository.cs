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
    public class IndustryCodeRepository : IIndustryCodeRepository
    {
        private IRedisCacheProvider cache { get; set; }
        private StoredProcedure spContext = new StoredProcedure();

        public IndustryCodeRepository()
            : this(new RedisCacheProvider(AppSettings.RedisDatabaseId))
        {
        }

        public IndustryCodeRepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string GetIndustryCodes()
        {
            string IndustryCodeData = cache.GetStringKey(AppSettings.RedisKeyIndustryCodes);
            if (IndustryCodeData == null)
            {
                IndustryCodeData = JsonConvert.SerializeObject(spContext.GetSqlDataNoParms<IndustryCode>(AppSettings.SPGetIndustryCodeList));
                cache.SetStringKey(AppSettings.RedisKeyIndustryCodes, IndustryCodeData);
            }
            return (IndustryCodeData);
        }

    }
}
