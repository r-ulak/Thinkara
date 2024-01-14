using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Models;

namespace PlanetX2012.DataCache
{
    public interface IMajorCodeRepository
    {
        void ClearCache();
        IEnumerable<MajorCode> GetMajorCodes();
    }

    public class MajorCodeRepository : IMajorCodeRepository
    {
        protected PlanetXContext DataContext { get; private set; }

        public ICacheProvider Cache { get; set; }

        public MajorCodeRepository()
            : this(new DefaultCacheProvider())
        {
        }

        public MajorCodeRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new PlanetXContext();
            this.Cache = cacheProvider;
        }

        public IEnumerable<MajorCode> GetMajorCodes()
        {
            // First, check the cache
            IEnumerable<MajorCode> MajorCodeData = Cache.Get("MajorCodes") as IEnumerable<MajorCode>;

            // If it's not in the cache, we need to read it from the repository
            if (MajorCodeData == null)
            {
                // Get the repository data
                MajorCodeData = DataContext.MajorCodes.OrderBy(v => v.Major).ToList();

                if (MajorCodeData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("MajorCodes", MajorCodeData, 30);
                }
            }

            return MajorCodeData;
        }

        public void ClearCache()
        {
            Cache.Invalidate("MajorCodes");
        }
    }
}