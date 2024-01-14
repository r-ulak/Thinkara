using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class ProvinceCode
    {
        public ProvinceCode()
        {
            this.BusinessLocations = new List<BusinessLocation>();
            this.WebUserLocations = new List<WebUserLocation>();
        }

        public short ProvinceId { get; set; }
        public string Province { get; set; }
        public virtual ICollection<BusinessLocation> BusinessLocations { get; set; }
        public virtual ICollection<WebUserLocation> WebUserLocations { get; set; }
    }
}
