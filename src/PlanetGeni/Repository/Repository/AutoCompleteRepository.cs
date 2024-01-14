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
    public class AutoCompleteRepository
    {
        private StoredProcedure spContext = new StoredProcedure();
        private IRedisCacheProvider cache { get; set; }

        public AutoCompleteRepository()
            : this(new RedisCacheProvider(AppSettings.RedisAutocompleteDatabaseId))
        {
        }

        public AutoCompleteRepository(IRedisCacheProvider cacheProvider)
        {
            this.cache = cacheProvider;
        }

        public string AutoCompleteUserSearch(AutoCompleteDTO autoComplete)
        {
            var result = cache.AutoCompleteSearch(AppSettings.RedisHashIndexWebUser, AppSettings.RedisSetIndexWebUser, autoComplete.QueryString, autoComplete.Skip, AppSettings.AutoCompleteWebUserSearchLimit);
            StringBuilder jsonArray = new StringBuilder("[");
            jsonArray.Append(string.Join(",", result));
            jsonArray.Append("]");
            return jsonArray.ToString();
        }



    }
}
