using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class ProvinceCode
    {
        public ProvinceCode()
        {
            this.BusinessLocations = new List<BusinessLocation>();
            this.EventLocations = new List<EventLocation>();
            this.UniversityLocations = new List<UniversityLocation>();
            this.WebUserLocations = new List<WebUserLocation>();
        }

        public short ProvinceId { get; set; }
        public string Province { get; set; }
        public virtual ICollection<BusinessLocation> BusinessLocations { get; set; }
        public virtual ICollection<EventLocation> EventLocations { get; set; }
        public virtual ICollection<UniversityLocation> UniversityLocations { get; set; }
        public virtual ICollection<WebUserLocation> WebUserLocations { get; set; }
    }
}
