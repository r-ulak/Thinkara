using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using DAO.DAO;
using DAO.DAO.Repository;
using DAO.Models;
using PlanetX.BRO;
using PlanetX2012.Models.DAO;

namespace TrendingService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    public class TrendingService : ITrendingService
    {
        private static int maxTag = Convert.ToInt32(ConfigurationManager.AppSettings["Trendingtopics.maxTag"]);
        private static bool cacheOn = Convert.ToBoolean(ConfigurationManager.AppSettings["redis.cacheOn"]);
        private static int cacheExpiration = Convert.ToInt32(ConfigurationManager.AppSettings["redis.cacheExpiration"]);
        private static int database = Convert.ToInt32(ConfigurationManager.AppSettings["redis.database"]);

        private TrendingServiceRepository topicRepository = new TrendingServiceRepository(database);
        private StoredProcedure sp = new StoredProcedure();

        public void AddNewTopicTag(string message)
        {

            IEnumerable<string> tagList = RegexExtensions.GetHashTags(message);

            PlanetXContext db = new PlanetXContext();
            string tag = string.Empty;
            foreach (string item in tagList)
            {
                tag = item.Trim().ToUpper().Substring(1);
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("newTag", tag);
                sp.ExecuteStoredProcedure("SaveTopicTag", dictionary);

            }

        }

        public KeyValuePair<string, double>[] GetTrendingTopics()
        {



            if (cacheOn)
                return topicRepository.GetTrendingTopicsFromRedis(maxTag);
            else
            {




                IEnumerable<TrendingTopics> topics = topicRepository.GetTrendingTopicsFromDb(maxTag);
                return topics.Select(x =>
                   new KeyValuePair<string, double>(x.Tag, x.TagCount)).ToArray();
            }


        }


    }
}
