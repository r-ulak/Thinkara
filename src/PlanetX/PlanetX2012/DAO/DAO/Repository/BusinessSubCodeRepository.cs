using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Models;

namespace PlanetX2012.DataCache
{
    public interface IBusinessSubCodeRepository
    {
        void ClearCache(int businessTypeId);
        IEnumerable<BusinessSubCode> GetBusinessSubCodes(int businessTypeId);
    }

    public class BusinessSubCodeRepository : IBusinessSubCodeRepository
    {
        protected PlanetXContext DataContext { get; private set; }

        public ICacheProvider Cache { get; set; }

        public BusinessSubCodeRepository()
            : this(new DefaultCacheProvider())
        {
        }

        public BusinessSubCodeRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new PlanetXContext();
            this.Cache = cacheProvider;
        }

        public IEnumerable<BusinessSubCode> GetBusinessSubCodes(int businessTypeId)
        {
            // First, check the cache
            IEnumerable<BusinessSubCode> BusinessSubCodeData = Cache.Get("BusinessSubCodes" + businessTypeId) as IEnumerable<BusinessSubCode>;

            // If it's not in the cache, we need to read it from the repository
            if (BusinessSubCodeData == null)
            {
                // Get the repository data
                BusinessSubCodeData = DataContext.BusinessSubCodes.Where(c => c.BusinessTypeId == businessTypeId).OrderBy(v => v.Code).ToList();

                if (BusinessSubCodeData.Any())
                {
                    // Put this data into the cache for 30 minutes
                    Cache.Set("BusinessSubCodes" + businessTypeId, BusinessSubCodeData, 30);
                }
            }

            return BusinessSubCodeData;
        }

        public void ClearCache(int businessTypeId)
        {
            Cache.Invalidate("BusinessSubCodes" + businessTypeId);
        }
    }
}