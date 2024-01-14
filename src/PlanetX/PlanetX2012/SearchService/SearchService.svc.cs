using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DAO.DAO.Repository.Redis;

namespace SearchService
{
    public class SearchService : ISearchService
    {
        private int redisautoCompletedatabase =
    Convert.ToInt32(ConfigurationManager.AppSettings["redis.autoComplete.database"]);

        private int redisautoCompletekeyExpire =
   Convert.ToInt32(ConfigurationManager.AppSettings["redis.autoComplete.KeyExpireSecond"]);


        private string redisautoCompleteCityhashKey =
        ConfigurationManager.AppSettings["redis.autoComplete.CityList.Key"];

        private string redisautoCompleteCityPrefixKey =
         ConfigurationManager.AppSettings["redis.autoComplete.CityList.PrefixKey"];

        private string redisautoCompleteWebUserhashKey =
         ConfigurationManager.AppSettings["redis.autoComplete.WebUserList.Key"];

        private string redisautoCompleteWebUserPrefixKey =
        ConfigurationManager.AppSettings["redis.autoComplete.WebUserList.PrefixKey"];


        public string[] SearchCity(string term)
        {
            RediAutoCompleteRepository repositoryCity = new RediAutoCompleteRepository(redisautoCompletedatabase);
            return repositoryCity.GetAutoCompleteList(term, redisautoCompleteCityPrefixKey, redisautoCompleteCityhashKey);
        }

        public string[] SearchPeople(string term)
        {
            RediAutoCompleteRepository repositoryPeople = new RediAutoCompleteRepository(redisautoCompletedatabase);
            if (term.IndexOf(' ') > 0)
                return repositoryPeople.GetIntersectAutoCompleteList(term, redisautoCompleteWebUserPrefixKey, redisautoCompleteWebUserhashKey, redisautoCompletekeyExpire);
            else
                return repositoryPeople.GetAutoCompleteList(term, redisautoCompleteWebUserPrefixKey, redisautoCompleteWebUserhashKey);
        }


    }
}
