using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class CountryCode
    {
        public CountryCode()
        {
            this.BusinessLocations = new List<BusinessLocation>();
            this.WebUserLocations = new List<WebUserLocation>();
        }

        public string CountryId { get; set; }
        public string Code { get; set; }
        public virtual ICollection<BusinessLocation> BusinessLocations { get; set; }
        public virtual MilitaryForce MilitaryForce { get; set; }
        public virtual ICollection<WebUserLocation> WebUserLocations { get; set; }
    }
}
