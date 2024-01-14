using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class SchoolLocation
    {
        public short SchoolId { get; set; }
        public int CityId { get; set; }
        public short ProvinceId { get; set; }
        public string CountryId { get; set; }
    }
}
