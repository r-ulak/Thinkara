using System;
using System.Collections.Generic;
using System.Linq;
using DAO.DAO;
using DAO.Models;
using PlanetX2012.Models.DAO;

namespace PlanetX2012.DataCache
{
    public interface ICityCodeRepository
    {
        void ClearCache();        
        void LoadCityCodesSP();
        IEnumerable<CityCountry> GetCityCodes();
    }

    public class CityCodeRepository : ICityCodeRepository
    {
        protected PlanetXContext DataContext { get; private set; }

        public ICacheProvider Cache { get; set; }

        public CityCodeRepository()
            : this(new DefaultCacheProvider())
        {
        }

        public CityCodeRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new PlanetXContext();
            this.Cache = cacheProvider;
        }


        public IEnumerable<CityCountry> GetCityCodes()
        {
            // First, check the cache
            IEnumerable<CityCountry> CityCodeData = Cache.Get("CityCodes") as IEnumerable<CityCountry>;

            // If it's not in the cache, we need to read it from the repository
            if (CityCodeData == null)
            {
                // Get the repository data
                StoredProcedure sp = new StoredProcedure();

                CityCodeData = sp.GetSqlDataNoParms<CityCountry>("GetCityList");

                if (CityCodeData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("CityCodes", CityCodeData, 30);
                }
            }

            return CityCodeData;
        }


        public void LoadCityCodesSP()
        {
            // First, check the cache
            IEnumerable<CityCountry> CityCodeData = Cache.Get("CityCodes") as IEnumerable<CityCountry>;

            // If it's not in the cache, we need to read it from the repository
            if (CityCodeData == null)
            {
                // Get the repository data
                StoredProcedure sp = new StoredProcedure();

                CityCodeData = sp.GetSqlDataNoParms<CityCountry>("GetCityList");

                if (CityCodeData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("CityCodes", CityCodeData, 30);
                }
            }

            
        }
        
            
        



        public void ClearCache()
        {
            Cache.Invalidate("CityCodes");
        }
    }
}