using System;
using System.Collections.Generic;
using System.Linq;
using DAO.Models;

namespace PlanetX2012.DataCache
{
	public interface IBusinessCodeRepository
	{
		void ClearCache();
		IEnumerable<BusinessCode> GetBusinessCodes();
	}

	public class BusinessCodeRepository : IBusinessCodeRepository
	{
		protected PlanetXContext DataContext { get; private set; }

		public ICacheProvider Cache { get; set; }

		public BusinessCodeRepository()
			: this(new DefaultCacheProvider())
		{
		}

		public BusinessCodeRepository(ICacheProvider cacheProvider)
		{
            this.DataContext = new PlanetXContext();
			this.Cache = cacheProvider;
		}

		public IEnumerable<BusinessCode> GetBusinessCodes()
		{
			// First, check the cache
			IEnumerable<BusinessCode> BusinessCodeData = Cache.Get("BusinessCodes") as IEnumerable<BusinessCode>;

			// If it's not in the cache, we need to read it from the repository
			if (BusinessCodeData == null)
			{
				// Get the repository data
				BusinessCodeData = DataContext.BusinessCodes.OrderBy(v => v.Code).ToList();

				if (BusinessCodeData.Any())
				{
					// Put this data into the cache for 30 minutes
					Cache.Set("BusinessCodes", BusinessCodeData, 30);
				}
			}

			return BusinessCodeData;
		}

		public void ClearCache()
		{
			Cache.Invalidate("BusinessCodes");
		}
	}
}