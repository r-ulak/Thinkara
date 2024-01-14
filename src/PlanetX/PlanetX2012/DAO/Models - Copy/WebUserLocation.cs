using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class WebUserLocation
    {
        public int UserId { get; set; }
        public int CityId { get; set; }
        public short ProvinceId { get; set; }
        public string CountryId { get; set; }
        public virtual CityCode CityCode { get; set; }
        public virtual CountryCode CountryCode { get; set; }
        public virtual ProvinceCode ProvinceCode { get; set; }
    }
}
