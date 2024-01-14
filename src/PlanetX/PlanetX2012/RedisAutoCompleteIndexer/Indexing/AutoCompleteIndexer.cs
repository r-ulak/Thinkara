using System;
using System.Collections.Generic;
using BookSleeve;
using DAO.DAO.Repository;
using Newtonsoft.Json;

namespace RedisAutoCompleteIndexer
{

    public interface IAutoCompleteIndexer
    {
        void RemoveIndex();
        void RemovePrefixIndex();
        void IndexHashKey();
        void IndexPrefixKeyInsert();
        void IndexPrefixKeyUpdate();
        void LoadDataListInsert(string procedureName);
        void LoadDataListUpdate(string procedureName);
        void LoadDataListDelete(string procedureName);

    }

    public class AutoCompleteIndexer : RedisRepository, IAutoCompleteIndexer
    {
        public string Key { get; set; }
        public string PrefixKey { get; set; }

        public virtual string GetJsonString(object item)
        {

            return JsonConvert.SerializeObject(item);
        }
        public virtual void AddPrefixSortedSet(string key, string item, double score)
        {
            _redisClient.SortedSets.Add(_database, key, item, score);
        }
        public virtual void AddHashSetKey(string item, string hashField)
        {
            _redisClient.Hashes.Set(_database, Key, hashField, item);
        }


        public virtual void RemoveHashKey(string hashField)
        {
            _redisClient.Hashes.Remove(_database, Key, hashField);
        }
        public virtual void RemovePrefixKey(string item, string id)
        {

            for (int i = 1; i <= item.Length; i++)
            {
             //   Console.WriteLine("Remvoing PrefixKey {0} ", PrefixKey + item.Substring(0, i));
                _redisClient.SortedSets.Remove(_database,
                    PrefixKey + item.Substring(0, i), id);
            }
        }
        public virtual void RemoveIndex() { }
        public virtual void RemovePrefixIndex() { }
        public virtual void IndexHashKey() { }
        public virtual void IndexPrefixKeyInsert() { }
        public virtual void IndexPrefixKeyUpdate() { }
        public virtual void LoadDataListInsert(string procedureName) { }
        public virtual void LoadDataListUpdate(string procedureName) { }
        public virtual void LoadDataListDelete(string procedureName) { }



        public AutoCompleteIndexer(string indexKey, string prefixKey)
            : base()
        {
            Key = indexKey;
            PrefixKey = prefixKey;
        }
        public AutoCompleteIndexer(int database, string indexKey, string prefixKey)
            : base(database)
        {
            Key = indexKey;
            PrefixKey = prefixKey;
        }

        public AutoCompleteIndexer(RedisConnection redisClient, int database, string indexKey, string prefixKey)
            : base(redisClient, database)
        {
            Key = indexKey;
            PrefixKey = prefixKey;
        }

        ~AutoCompleteIndexer()
        {
            base.Dispose(false);
        }

    }
}
