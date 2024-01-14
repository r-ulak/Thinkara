using System;
using BookSleeve;
using PlanetX2012.UserStatusManager;
using System.Linq;

namespace DAO.DAO.Repository
{
    public class SignalRConnectionRepository : RedisRepository, IDisposable
    {

        public SignalRConnectionRepository()
            : base()
        { }
        public SignalRConnectionRepository(int database)
            : base(database)
        {

        }

        public SignalRConnectionRepository(RedisConnection redisClient, int database)
            : base(redisClient, database)
        { }

        ~SignalRConnectionRepository()
        {
            base.Dispose(false);
        }

        public void AddSignalRConnection(string key, string connectionId)
        {

            _redisClient.Sets.Add(_database, "Connection" + key, connectionId);
        }

        public string[] GetSignalRConnection(string key)
        {
            key = "Connection" + key;
            var result = _redisClient.Sets.GetAllString(_database, key);
            result.Wait(100);
            return result.Result;
        }

        public string[] GetAllFriendsSignalRConnection(string key)
        {
            UserStatusManager chatController = new UserStatusManager();
            string[] friendsConnectionsSet = chatController.GetFriendsId(Convert.ToInt32(key)).
                Select(r => string.Concat("Connection", r)).ToArray();
            var results = _redisClient.Sets.UnionString(_database, friendsConnectionsSet);
            results.Wait(100);
            return results.Result;

        }

        public void DeleteConnection(string key, string connectionId)
        {
            _redisClient.Sets.Remove(_database, "Connection" + key, connectionId);
        }


    }
}
