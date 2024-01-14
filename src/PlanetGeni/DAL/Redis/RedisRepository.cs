using System;
using Common;
using StackExchange.Redis;
using System.Text;
namespace DAO.DAO.Repository
{
    public class RedisRepository
    {
        protected static bool AllowAdmin { get; set; }
        public RedisRepository(bool allowAdmin)
        {
            AllowAdmin = allowAdmin;
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(GetRedisConfiguration());
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        private static string GetRedisConfiguration()
        {
            string server = AppSettings.RedisServer;
            int port = AppSettings.RedisPort;
            int syncTimeout = AppSettings.RedisSyncTimeOut;
            string password = AppSettings.RedisPassword;
            bool ssl = AppSettings.RedisSSL;
            StringBuilder connectionString = new StringBuilder();
            bool abortConnect = false;
            connectionString.AppendFormat("{0}:{1},ssl={2},password={3},syncTimeout={4}, abortConnect ={5}", server, port, ssl, password, syncTimeout, abortConnect);
            if (AllowAdmin)
            {
                connectionString.Append(",allowAdmin=true");

            }
            return connectionString.ToString();
        }

    }
}
