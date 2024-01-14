using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class EventLocation
    {
        public int EventId { get; set; }
        public int CityId { get; set; }
        public short ProvinceId { get; set; }
        public string CountryId { get; set; }
        public virtual CityCode CityCode { get; set; }
        public virtual CountryCode CountryCode { get; set; }
        public virtual Event Event { get; set; }
        public virtual ProvinceCode ProvinceCode { get; set; }
    }
}
