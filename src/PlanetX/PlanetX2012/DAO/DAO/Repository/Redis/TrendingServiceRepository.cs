
using System;
using System.Collections.Generic;
using System.Configuration;
using BookSleeve;
using PlanetX2012.Models.DAO;
using System.Linq;
namespace DAO.DAO.Repository
{
    public class TrendingServiceRepository : RedisRepository
    {
        private int cacheExpiration = Convert.ToInt32(ConfigurationManager.AppSettings["redis.cacheExpiration"]);
        private int timeLimitMinute = Convert.ToInt32(ConfigurationManager.AppSettings["Trendingtopics.timeLimitMinute"]);
        private string trendingTopicsKey = ConfigurationManager.AppSettings["redis.TrendingTopicsKey"];
        public TrendingServiceRepository()
            : base()
        { }
        public TrendingServiceRepository(int database)
            : base(database)
        {

        }

        public TrendingServiceRepository(RedisConnection redisClient, int database)
            : base(redisClient, database)
        { }

        ~TrendingServiceRepository()
        {
            base.Dispose(false);
        }

        public void AddNewTopicTag(TrendingTopics topics)
        {
            _redisClient.SortedSets.Add(_database, trendingTopicsKey
                , topics.Tag, topics.TagCount);
        }
        public KeyValuePair<string, double>[] GetTrendingTopicsFromRedis(int maxTag)
        {
            var task = _redisClient.SortedSets.RangeString(_database, trendingTopicsKey, 0, maxTag, false);
            task.Wait(100);
            var result = task.Result;
            if (result.Length == 0)
            {
                IEnumerable<TrendingTopics> topics = GetTrendingTopicsFromDb(maxTag);
                foreach (TrendingTopics item in topics)
                {
                    AddNewTopicTag(item);
                }
                _redisClient.Keys.Expire(_database, trendingTopicsKey, cacheExpiration);

                var gettopics = _redisClient.SortedSets.RangeString(_database, trendingTopicsKey, 0, maxTag, false);
                gettopics.Wait(100);
                return gettopics.Result;
            }
            else
            {
                return result;
            }


        }

        public IEnumerable<TrendingTopics> GetTrendingTopicsFromDb(int maxTag)
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("tagLimit", maxTag);
            dictionary.Add("timeLimitMinute", timeLimitMinute);
            IEnumerable<TrendingTopics> trendingTopics;
            trendingTopics = sp.GetSqlData<TrendingTopics>("GetTopNTopicTagContent", dictionary);
            if (trendingTopics.Count() > 0)
                return trendingTopics;
            else
            {
                dictionary.Remove("timeLimitMinute");
                trendingTopics = sp.GetSqlData<TrendingTopics>("GetTopTopicTagContent", dictionary);
                return trendingTopics;
            }
        }
    }
}
