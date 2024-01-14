using DAO.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using PlanetX2012.Models.DAO;
using DAO.DAO;
namespace PlanetX2012.UserStatusManager
{
    public class UserStatusManager
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
            //Process currentProcess = Process.GetCurrentProcess();
            var webUser = new WebUser() { UserId = webUserId, OnlineStatus = (sbyte)status };
            //var userConnection = new UserConnection() { UserId = webUserId, ConnectionId = connectionId, UserAgent = currentProcess.Id.ToString() };

            using (var dbxinsert = new PlanetXContext())
            {
                try
                {


                    dbxinsert.WebUsers.Attach(webUser);
                    //      dbxinsert.UserConnections.Add(userConnection);
                    dbxinsert.SaveChanges();
                }
                catch (Exception)
                {


                }

            }

        }

        public void DeleteConnection(int webUserId, string connectionId)
        {
            var webUser = new WebUser() { UserId = webUserId, OnlineStatus = (sbyte)EnumClass.UserStatus.Offline };
            using (var dbxinsert = new PlanetXContext())
            {
                try
                {
                    dbxinsert.WebUsers.Attach(webUser);
                    dbxinsert.SaveChanges();
                }
                catch (Exception)
                {


                }
            }
        }

        public IEnumerable<FriendStatus> GetFriends(int webUserId)
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", webUserId);
            return sp.GetSqlData<FriendStatus>("GetFriends", dictionary);

        }

        public IEnumerable<int> GetFriendsId(int webUserId)
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", webUserId);
            return sp.GetSqlData("GetFriends", dictionary);

        }

        public IEnumerable<String> GetConnections(string webUserId)
        {
            StoredProcedure sp = new StoredProcedure();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("webUserId", webUserId);
            return sp.GetSqlData<UserConnection>("GetConnectionByUserId", dictionary).Select(c => c.ConnectionId);

        }

    }
}