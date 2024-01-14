using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class CityCode
    {
        public CityCode()
        {
            this.BusinessLocations = new List<BusinessLocation>();
            this.WebUserLocations = new List<WebUserLocation>();
        }

        public int CityId { get; set; }
        public string City { get; set; }
        public string CountryId { get; set; }
        public virtual ICollection<BusinessLocation> BusinessLocations { get; set; }
        public virtual ICollection<WebUserLocation> WebUserLocations { get; set; }
    }
}
