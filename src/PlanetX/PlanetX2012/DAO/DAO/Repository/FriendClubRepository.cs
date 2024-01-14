using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using DAO.DAO.ComplexType;
using DAO.Models;
using PlanetX2012.Models.DAO;

namespace PlanetX2012.DataCache
{
    public interface IFriendClubRepository
    {
        void ClearCache();
        IEnumerable<FriendAndClub> GetFriendClubs(int webUserId);
    }

    public class FriendClubRepository : IFriendClubRepository
    {
        protected PlanetXContext DataContext { get; private set; }

        public ICacheProvider Cache { get; set; }

        public FriendClubRepository()
            : this(new DefaultCacheProvider())
        {
        }

        public FriendClubRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new PlanetXContext();
            this.Cache = cacheProvider;
        }


        public IEnumerable<FriendAndClub> GetFriendClubs(int webUserId)
        {
            // First, check the cache
            IEnumerable<FriendAndClub> FriendClubData = Cache.Get("FriendClubs" + webUserId) as IEnumerable<FriendAndClub>;

            // If it's not in the cache, we need to read it from the repository
            if (FriendClubData == null)
            {
                // Get the repository data


                StoredProcedure sp = new StoredProcedure();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("webUserId", webUserId);
                FriendClubData = sp.GetSqlData<FriendAndClub>("GetFriendsAndClubByUser", dictionary);

                if (FriendClubData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("FriendClubs" + webUserId, FriendClubData, 30);
                }
            }

            return FriendClubData;
        }








        public void ClearCache()
        {
            Cache.Invalidate("FriendClubs");
        }
    }
}