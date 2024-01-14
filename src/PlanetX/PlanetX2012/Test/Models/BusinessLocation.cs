using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class BusinessLocation
    {
        public int BusinessId { get; set; }
        public int CityId { get; set; }
        public short ProvinceId { get; set; }
        public string CountryId { get; set; }
        public virtual Business Business { get; set; }
        public virtual CityCode CityCode { get; set; }
        public virtual CountryCode CountryCode { get; set; }
        public virtual ProvinceCode ProvinceCode { get; set; }
    }
}
