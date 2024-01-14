using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO.Models;
using PlanetX2012.Models.DAO;

namespace PlanetX2012.Topic
{
    public class TopicManager
    {
        private StoredProcedure sp = new StoredProcedure();
        
        public IEnumerable<TopicTag> GetTrendingTopcis(int tagLimit)
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("tagLimit", tagLimit);
            return sp.GetSqlData<TopicTag>("GetTopNTopicTagContent", dictionary);

        }

    }
}
