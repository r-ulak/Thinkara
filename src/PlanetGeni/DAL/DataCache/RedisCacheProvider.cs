using DAO.DAO.Repository;
using StackExchange.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace DataCache
{
    public interface IRedisCacheProvider
    {

        void SetHashSetKey(string key, Dictionary<String, String> myDic, int cacheTime = 0);
        void SetStringKey(string key, string value, int cacheTime = 0);
        string GetStringKey(string key);
        bool IsSet(string key);
        void SetAdd(string key, string value);
        void SetRemove(string key, string value);
        bool IsSetMember(string key, string value);
        bool KeyExists(string key);
        string[] FindKeys(string keyPattern);
        void HashRemoveAsync(string key, string value);
        void HashRemove(string key, string value);

        void SetHash(string key, string field, string value);
        void SetHashDictionary(string key, Dictionary<string, string> value);
        string[] GetMultipleHash(string key, string[] fields);
        string GetHash(string key, string field);
        string GetString(byte[] bytes);
        byte[] GetBytes(string str);
        string[] GetAllSet(string key);
        void Invalidate(string[] keys);
        void Invalidate(string key);
        long GetSetCount(string key);
        void ExpireKey(string key, int cacheTime);
        double? GetSortedSetScore(string key, string field);
        void RemoveSortedSetMember(string key, string field);
        void RemoveSortedSetMembers(string key, string[] sortValues);
        void AddSoretedSets(string key, Dictionary<string, double> sortValues);
        Dictionary<string, string> GetHashSet(string key);
        long? GetSortedSetRankRev(string key, string field);
        string[] AutoCompleteSearch(string hashkey, string setkey, string querystring, long skip, long limit);
    }
    public class RedisCacheProvider : RedisRepository, IRedisCacheProvider
    {
        private readonly IDatabase _redisClient;
        private readonly int _database;
        public RedisCacheProvider(int database)
            : base(false)
        {
            _database = database;
            _redisClient = Connection.GetDatabase(_database);
        }

        public string GetStringKey(string key)
        {
            string data = _redisClient.StringGet(key);
            if (data == "null")
            {
                return null;
            }
            return data;
        }

        public bool IsSetMember(string key, string value)
        {
            return _redisClient.SetContains(key, value);
        }
        public void SetAdd(string key, string value)
        {
            _redisClient.SetAdd(key, value, flags: CommandFlags.FireAndForget);
        }
        public void SetRemove(string key, string value)
        {
            var result = _redisClient.SetRemoveAsync(key, value);

        }
        public string[] GetAllSet(string key)
        {
            return _redisClient.SetMembers(key).ToStringArray();
        }
        public long GetSetCount(string key)
        {
            return _redisClient.SetLength(key);
        }
        public Dictionary<string, string> GetHashSet(string key)
        {
            return _redisClient.HashGetAll(key).ToStringDictionary();
        }

        public void HashRemoveAsync(string key, string value)
        {
            _redisClient.HashDeleteAsync(key, value);
        }
        public void HashRemove(string key, string value)
        {
            _redisClient.HashDelete(key, value);
        }

        public string[] GetMultipleHash(string key, string[] fields)
        {
            RedisValue[] redisfields =
            Array.ConvertAll(fields, x => (RedisValue)x);
            return _redisClient.HashGet(key, redisfields).ToStringArray();
        }
        public string GetHash(string key, string field)
        {
            return _redisClient.HashGet(key, field);
        }
        public string[] GetSortedSetByLex(string key, string min, string max, long skip, long limit)
        {
            return _redisClient.SortedSetRangeByValue(key, min, max, Exclude.None, skip, limit).ToStringArray();
        }

        public string[] AutoCompleteSearch(string hashkey, string setkey, string querystring, long skip, long limit)
        {
            string[] hashItems = GetSortedSetByLex(setkey, querystring.ToLower(), querystring.ToLower() + @"xff",
               skip, limit);
            var result = hashItems.Select(e => e.Split(':')[1]).Distinct().ToArray();
            return GetMultipleHash(hashkey, result);
        }


        public string[] FindKeys(string keyPattern)
        {
            var endpoints = Connection.GetEndPoints(true);
            List<RedisKey> items = new List<RedisKey>();
            foreach (var endpoint in endpoints)
            {
                var server = Connection.GetServer(endpoint);
                items.AddRange(server.Keys(pattern: keyPattern).ToArray());
            }

            var keys = items.Select(key => (string)key).ToArray();
            return keys;

        }

        public bool KeyExists(string key)
        {
            return _redisClient.KeyExists(key);
        }

        public void SetStringKey(string key, string value, int cacheTime = 0)
        {
            if (cacheTime == 0)
            {
                _redisClient.StringSetAsync(key, value,
              flags: CommandFlags.FireAndForget);
            }
            else
            {
                _redisClient.StringSetAsync(key, value, TimeSpan.FromSeconds(cacheTime),
           flags: CommandFlags.FireAndForget);
            }

        }
        public void SetHash(string key, string field, string value)
        {
            _redisClient.HashSetAsync(key, field,
               value);
        }
        public void SetHashDictionary(string key, Dictionary<string, string> value)
        {
            var fields = value.Select(
                pair => new HashEntry(pair.Key, pair.Value)).ToArray();
            _redisClient.HashSet(key, fields);
        }

        public void SetHashSetKey(string key, Dictionary<String, String> myDic, int cacheTime = 0)
        {

            foreach (KeyValuePair<string, string> entry in myDic)
            {
                _redisClient.HashSetAsync(key, entry.Key, entry.Value);
            }

            if (cacheTime > 0)
                _redisClient.KeyExpireAsync(key, TimeSpan.FromSeconds(cacheTime));
        }
        public void ExpireKey(string key, int cacheTime)
        {
            _redisClient.KeyExpireAsync(key, TimeSpan.FromSeconds(cacheTime));
        }
        public bool IsSet(string key)
        {
            return _redisClient.KeyExists(key);
        }

        public void AddSoretedSets(string key, Dictionary<string, double> sortValues)
        {
            var sortedEntries = sortValues.Select(
                pair => new SortedSetEntry(pair.Key, pair.Value)).ToArray();
            _redisClient.SortedSetAdd(key, sortedEntries);
        }
        public double? GetSortedSetScore(string key, string field)
        {
            return _redisClient.SortedSetScore(key, field);
        }

        public void RemoveSortedSetMember(string key, string field)
        {
            _redisClient.SortedSetRemoveAsync(key, field);
        }

        public void RemoveSortedSetMembers(string key, string[] sortValues)
        {
            RedisValue[] redisValues =
          Array.ConvertAll(sortValues, x => (RedisValue)x);
            _redisClient.SortedSetRemove(key, redisValues);
        }

        public long? GetSortedSetRankRev(string key, string field)
        {
            return _redisClient.SortedSetRank(key, field, order: Order.Descending) + 1;
        }


        public void Invalidate(string key)
        {

            _redisClient.KeyDelete(key);
        }
        public void Invalidate(string[] keys)
        {
            var expirekeys = keys.Select(key => (RedisKey)key).ToArray();
            _redisClient.KeyDelete(expirekeys);
        }

        public byte[] GetBytes(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            return bytes;
        }

        public string GetString(byte[] bytes)
        {
            return ASCIIEncoding.ASCII.GetString(bytes);
        }


    }
}