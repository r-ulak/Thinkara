using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Models;

namespace PlanetX2012.DataCache
{
    public interface IUniversityCodeRepository
    {
        void ClearCache();
        IEnumerable<UniversityCode> GetUniversityCodes();
    }

    public class UniversityCodeRepository : IUniversityCodeRepository
    {
        protected PlanetXContext DataContext { get; private set; }

        public ICacheProvider Cache { get; set; }

        public UniversityCodeRepository()
            : this(new DefaultCacheProvider())
        {
        }

        public UniversityCodeRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new PlanetXContext();
            this.Cache = cacheProvider;
        }

        public IEnumerable<UniversityCode> GetUniversityCodes()
        {
            // First, check the cache
            IEnumerable<UniversityCode> UniversityCodeData = Cache.Get("UniversityCodes") as IEnumerable<UniversityCode>;

            // If it's not in the cache, we need to read it from the repository
            if (UniversityCodeData == null)
            {
                // Get the repository data
                UniversityCodeData = DataContext.UniversityCodes.OrderBy(v => v.Name).ToList();

                if (UniversityCodeData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("UniversityCodes", UniversityCodeData, 30);
                }
            }

            return UniversityCodeData;
        }

        public void ClearCache()
        {
            Cache.Invalidate("UniversityCodes");
        }
    }
}