using DAO.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using PlanetX2012.Models.DAO;
using DAO.DAO;
namespace PlanetX2012.Chat
{
    public class ChatManager
    {
        private StoredProcedure sp = new StoredProcedure();
        public void UpdateStatus(int webUserId, EnumClass.UserStatus onlineStatus)
        {

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", webUserId);
            dictionary.Add("onlineStatus", onlineStatus);
            sp.ExecuteStoredProcedure("UpdateOnlineStatus", dictionary);

        }

        public void InsertConnection(int webUserId, EnumClass.UserStatus status, string connectionId)
        {
            var webUser = new WebUser() { UserId = webUserId, OnlineStatus = (sbyte)status };
            var userConnection = new UserConnection() { UserId = webUserId, ConnectionId = connectionId };

            using (var dbxinsert = new PlanetXContext())
            {
                dbxinsert.WebUsers.Attach(webUser);
                dbxinsert.UserConnections.Add(userConnection);
                dbxinsert.SaveChanges();

            }

        }

        public void DeleteConnection(int webUserId, string connectionId)
        {
            var userConnection = new UserConnection() { UserId = webUserId, ConnectionId = connectionId };
            using (var dbxdelete = new PlanetXContext())
            {
                dbxdelete.Entry(userConnection).State = EntityState.Deleted;
                dbxdelete.SaveChanges();

            }

        }
        public IEnumerable<FriendStatus> GetFriends(int webUserId)
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", webUserId);
            return sp.GetSqlData<FriendStatus>("GetFriends", dictionary);

        }

    }
}