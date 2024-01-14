using DAO.DAO.Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCache
{
    public class RedisCacheProviderExtender : RedisRepository
    {
        public RedisCacheProviderExtender(bool admin, int database)
            : base(admin)
        {
            Connection.GetDatabase(database);
        }

        public void FlushDatabase(int dbId )
        {
            var endpoints = Connection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = Connection.GetServer(endpoint);
                server.FlushDatabase(dbId);
            }
        }

        public void FlushAllDatabase()
        {
            var endpoints = Connection.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = Connection.GetServer(endpoint);
                server.FlushAllDatabases();
            }
        }
    }
}
