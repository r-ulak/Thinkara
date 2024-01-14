

using System;
using System.Configuration;
using BookSleeve;
namespace DAO.DAO.Repository
{
    public class RedisRepository
    {
        protected RedisConnection _redisClient;
        protected int _database;
        protected bool disposed;
        public RedisRepository()
        {
            string server = ConfigurationManager.AppSettings["redis.server"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["redis.port"]);
            string password = ConfigurationManager.AppSettings["redis.password"];
            _redisClient = new RedisConnection(server, port, -1, password);
            _database = 0;
            _redisClient.Open();
        }

        public RedisRepository(int database)
        {
            string server = ConfigurationManager.AppSettings["redis.server"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["redis.port"]);
            string password = ConfigurationManager.AppSettings["redis.password"];
            _redisClient = new RedisConnection(server, port, -1, password);
            _database = database;
            _redisClient.Open();
        }

        public RedisRepository(RedisConnection redisClient, int database)
        {
            _redisClient = redisClient;
            _database = database;
            _redisClient.Open();
        }

        ~RedisRepository()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("Dispong Redis COnnection^*^*^*^*^*^");
                    _redisClient.CloseAsync(false);
                    _redisClient.Dispose();
                }

                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

    }
}
