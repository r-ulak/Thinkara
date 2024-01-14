using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class EventLocation
    {
        public int EventId { get; set; }
        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public string CountryId { get; set; }
    }
}
