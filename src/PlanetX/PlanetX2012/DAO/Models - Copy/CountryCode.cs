using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CountryCode
    {
        public CountryCode()
        {
            this.BusinessLocations = new List<BusinessLocation>();
            this.EventLocations = new List<EventLocation>();
            this.UniversityLocations = new List<UniversityLocation>();
            this.WebUserLocations = new List<WebUserLocation>();
        }

        public string CountryId { get; set; }
        public string Code { get; set; }
        public virtual ICollection<BusinessLocation> BusinessLocations { get; set; }
        public virtual ICollection<EventLocation> EventLocations { get; set; }
        public virtual GovermentProvince GovermentProvince { get; set; }
        public virtual MilitaryForce MilitaryForce { get; set; }
        public virtual ICollection<UniversityLocation> UniversityLocations { get; set; }
        public virtual ICollection<WebUserLocation> WebUserLocations { get; set; }
    }
}
