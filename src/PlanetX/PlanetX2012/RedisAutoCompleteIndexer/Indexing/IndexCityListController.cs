using System;
using System.Collections.Generic;
using BookSleeve;
using DAO.DAO;
using PlanetX2012.Models.DAO;
namespace RedisAutoCompleteIndexer
{
    public class IndexCityListController : AutoCompleteIndexer, IAutoCompleteIndexer
    {
        private IEnumerable<CityCountry> CityList;
        public int startRange { get; set; }
        public int endRange { get; set; }
        public int threadId { get; set; }

        public IndexCityListController(string indexKey, string prefixKey)
            : base(indexKey, prefixKey)
        {
            threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
        public IndexCityListController(int database, string indexKey, string prefixKey)
            : base(database, indexKey, prefixKey)
        {
            
            threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }


        public override void LoadDataListInsert(string procedureName)
        {
            // Get the repository data
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("startRange", startRange);
            dictionary.Add("endRange", endRange);
            CityList = sp.GetSqlData<CityCountry>(procedureName, dictionary);



            Console.WriteLine("Thread {0} Loading Data Insert List Complete StatrRange {1} EndRange {2}", threadId, startRange, endRange);

        }

       public override void LoadDataListUpdate(string procedureName){}


       public override void RemoveIndex() { }
       public override void RemovePrefixIndex() { }

        public override void IndexHashKey()
        {
            int count = 0;
            foreach (CityCountry cityCountry in CityList)
            {
                AddHashSetKey(GetJsonString(cityCountry), cityCountry.CityId.ToString());
                count++;
                if (count % 15000 == 0)
                    Console.WriteLine("Thread {0} IndexHashKey StatrRange {1} EndRange {2}  Count on {3} CurrentlyOn CityId {4} {5}"
                        , threadId, startRange, endRange, count, cityCountry.CityId, cityCountry.City.ToString());
            }
        }


        public override void IndexPrefixKeyInsert()
        {
            foreach (CityCountry cityCountry in CityList)
            {
                for (int i = 0; i < cityCountry.City.Length; i++)
                {
                    AddPrefixSortedSet("P" + Key + cityCountry.City.Substring(0, i), cityCountry.CityId.ToString(), 0.0);
                }
            }
        }

    }
}
