using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Models;

namespace PlanetX2012.DataCache
{
    public interface IDegreeCodeRepository
    {
        void ClearCache();
        IEnumerable<DegreeCode> GetDegreeCodes();
    }

    public class DegreeCodeRepository : IDegreeCodeRepository
    {
        protected PlanetXContext DataContext { get; private set; }

        public ICacheProvider Cache { get; set; }

        public DegreeCodeRepository()
            : this(new DefaultCacheProvider())
        {
        }

        public DegreeCodeRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new PlanetXContext();
            this.Cache = cacheProvider;
        }

        public IEnumerable<DegreeCode> GetDegreeCodes()
        {
            // First, check the cache
            IEnumerable<DegreeCode> DegreeCodeData = Cache.Get("DegreeCodes") as IEnumerable<DegreeCode>;

            // If it's not in the cache, we need to read it from the repository
            if (DegreeCodeData == null)
            {
                // Get the repository data
                DegreeCodeData = DataContext.DegreeCodes.OrderBy(v => v.Degree).ToList();

                if (DegreeCodeData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("DegreeCodes", DegreeCodeData, 30);
                }
            }

            return DegreeCodeData;
        }

        public void ClearCache()
        {
            Cache.Invalidate("DegreeCodes");
        }
    }
}