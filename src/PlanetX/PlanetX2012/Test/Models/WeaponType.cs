using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class WeaponType
    {
        public short WeaponTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public decimal Cost { get; set; }
        public string WeaponTypeCode { get; set; }
        public sbyte OffenseScore { get; set; }
        public sbyte DefenseScore { get; set; }
    }
}
