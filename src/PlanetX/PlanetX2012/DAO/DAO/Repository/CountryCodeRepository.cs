using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Models;

namespace PlanetX2012.DataCache
{
    public interface ICountryCodeRepository
    {
        void ClearCache();
        IEnumerable<CountryCode> GetCountryCodes();
    }

    public class CountryCodeRepository : ICountryCodeRepository
    {
        protected PlanetXContext DataContext { get; private set; }

        public ICacheProvider Cache { get; set; }

        public CountryCodeRepository()
            : this(new DefaultCacheProvider())
        {
        }

        public CountryCodeRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new PlanetXContext();
            this.Cache = cacheProvider;
        }

        public IEnumerable<CountryCode> GetCountryCodes()
        {
            // First, check the cache
            IEnumerable<CountryCode> CountryCodeData = Cache.Get("CountryCodes") as IEnumerable<CountryCode>;

            // If it's not in the cache, we need to read it from the repository
            if (CountryCodeData == null)
            {
                // Get the repository data
                CountryCodeData = DataContext.CountryCodes.OrderBy(v => v.Code).ToList();

                if (CountryCodeData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("CountryCodes", CountryCodeData, 30);
                }
            }

            return CountryCodeData;
        }

        public void ClearCache()
        {
            Cache.Invalidate("CountryCodes");
        }
    }
}