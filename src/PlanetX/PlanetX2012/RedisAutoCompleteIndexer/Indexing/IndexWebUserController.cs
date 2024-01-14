using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Models;
using PlanetX2012.Models.DAO;

namespace RedisAutoCompleteIndexer
{
    public class IndexWebUserController : AutoCompleteIndexer, IAutoCompleteIndexer
    {
        private IEnumerable<WebUserUpdate> WebUserList;
        public int startRange { get; set; }
        public int endRange { get; set; }
        public int threadId { get; set; }

        public IndexWebUserController(string indexKey, string prefixKey)
            : base(indexKey, prefixKey)
        {
            threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }
        public IndexWebUserController(int database, string indexKey, string prefixKey)
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
            WebUserList = sp.GetSqlData<WebUserUpdate>(procedureName, dictionary);
            Console.WriteLine("Thread {0} Loading Data Insert List Complete StatrRange {1} EndRange {2}", threadId, startRange, endRange);

        }

        public override void LoadDataListUpdate(string procedureName)
        {
            // Get the repository data
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("startRange", startRange);
            dictionary.Add("endRange", endRange);
            WebUserList = sp.GetSqlData<WebUserUpdate>(procedureName, dictionary);
            Console.WriteLine("Thread {0} Loading Data List Update Complete StatrRange {1} EndRange {2}", threadId, startRange, endRange);

        }

        public override void LoadDataListDelete(string procedureName)
        {
            // Get the repository data
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("startRange", startRange);
            dictionary.Add("endRange", endRange);
            WebUserList = sp.GetSqlData<WebUserUpdate>(procedureName, dictionary);
            Console.WriteLine("Thread {0} Loading Data Delete List Complete StatrRange {1} EndRange {2}", threadId, startRange, endRange);

        }

        public override void RemoveIndex()
        {
            foreach (WebUserUpdate webUser in WebUserList)
            {
                RemoveHashKey(webUser.UserId.ToString());
            }
        }

        public override void IndexHashKey()
        {
            int count = 0;
            foreach (WebUserUpdate webUser in WebUserList)
            {
                AddHashSetKey(GetJsonString(new
                {
                    f = webUser.NameFirst,
                    l = webUser.NameLast,
                    e = webUser.EmailId,
                    p = webUser.Picture
                }), webUser.UserId.ToString());
                count++;
                if (count % 15000 == 0)
                    Console.WriteLine("Thread {0} IndexHashKey StatrRange {1} EndRange {2}  Count on {3} CurrentlyOn WebUserId {4} {5}"
                        , threadId, startRange, endRange, count,
                        webUser.UserId, webUser.NameFirst);
            }
        }

        public override void IndexPrefixKeyInsert()
        {

            foreach (WebUserUpdate webUser in WebUserList)
            {
                IndexPrefix(webUser.NameFirst, webUser.UserId.ToString());
                IndexPrefix(webUser.NameLast, webUser.UserId.ToString());
                IndexPrefix(webUser.NameMIddle, webUser.UserId.ToString());
                IndexPrefix(webUser.EmailId, webUser.UserId.ToString());
            }
        }

        public override void IndexPrefixKeyUpdate()
        {

            foreach (WebUserUpdate webUser in WebUserList)
            {
                if (webUser.OldNameFirst != null)
                    if (webUser.NameFirst.ToUpper().Trim() != webUser.OldNameFirst.ToUpper().Trim())
                        RemovePrefixKey(webUser.OldNameFirst, webUser.UserId.ToString());
                if (webUser.OldNameLast != null)
                    if (webUser.NameLast.ToUpper().Trim() != webUser.OldNameLast.ToUpper().Trim())
                        RemovePrefixKey(webUser.OldNameLast, webUser.UserId.ToString());
                if (webUser.OldNameMIddle != null)
                    if (webUser.NameMIddle.ToUpper().Trim() != webUser.OldNameMIddle.ToUpper().Trim())
                        RemovePrefixKey(webUser.OldNameMIddle, webUser.UserId.ToString());

                if (webUser.OldEmailId != null)
                    if (webUser.EmailId.ToUpper().Trim() != webUser.OldEmailId.ToUpper().Trim())
                        RemovePrefixKey(webUser.OldEmailId, webUser.UserId.ToString());

            }

            var taskHashIndex = Task.Factory.StartNew(IndexHashKey);
            var taskIndexPrefixKeyInsert = Task.Factory.StartNew(IndexPrefixKeyInsert);

            taskHashIndex.Wait();
            taskIndexPrefixKeyInsert.Wait();

        }

        public void IndexPrefix(string item, string id)
        {
            for (int i = 1; i <= item.Length; i++)
            {
                //Console.WriteLine(PrefixKey + item.Substring(0, i));
                AddPrefixSortedSet(PrefixKey + item.Substring(0, i),
                    id, 0.0);
            }
        }


        public override void RemovePrefixIndex()
        {
            foreach (WebUserUpdate webUser in WebUserList)
            {                
                RemovePrefixKey(webUser.NameFirst, webUser.UserId.ToString());
                RemovePrefixKey(webUser.NameLast, webUser.UserId.ToString());
                RemovePrefixKey(webUser.NameMIddle, webUser.UserId.ToString());
                RemovePrefixKey(webUser.EmailId, webUser.UserId.ToString());

            }
            Console.WriteLine("Pinging.");
            
            _redisClient.Server.Ping();

            Console.WriteLine("Pinging Done");

            
        }

    }
}
