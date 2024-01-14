using System;
using System.Collections.Generic;
using System.Text;
using BookSleeve;


namespace DAO.DAO.Repository.Redis
{
    public class RediAutoCompleteRepository : RedisRepository, IDisposable
    {
        public RediAutoCompleteRepository()
            : base()
        { }
        public RediAutoCompleteRepository(int database)
            : base(database)
        {

        }

        public RediAutoCompleteRepository(RedisConnection redisClient, int database)
            : base(redisClient, database)
        { }

        ~RediAutoCompleteRepository()
        {
            base.Dispose(false);
        }

        public string[] GetAutoCompleteList(string term, string keyPrefix, string hashKeyPrefix)
        {
            var result = _redisClient.SortedSets.Range(_database, keyPrefix + term, 0, -1);
            result.Wait();
            int keyCount = result.Result.Length;
            if (keyCount > 0)
            {
                string[] fields = new string[keyCount];
                int i = 0;
                foreach (KeyValuePair<byte[], double> item in result.Result)
                {
                    fields[i] = System.Text.Encoding.Default.GetString(item.Key);
                    i++;

                }

                var resultautocomplete = _redisClient.Hashes.GetString(_database, hashKeyPrefix, fields);
                resultautocomplete.Wait();

                return resultautocomplete.Result;
            }
            else
            {
                return null;
            }

        }

        public string[] GetIntersectAutoCompleteList(string term, string keyPrefix, string hashKeyPrefix, int expireperoid)
        {

            term = keyPrefix + term;
            string[] terms = term.Replace(" ", " " + keyPrefix).Split(' ');
            var intersectresult = _redisClient.SortedSets.IntersectAndStore(_database, "cache" + term, terms);
            intersectresult.Wait();
            _redisClient.Keys.Expire(_database, "cache" + term, expireperoid);


            return GetAutoCompleteList(term, "cache", hashKeyPrefix);


        }

    }
}
