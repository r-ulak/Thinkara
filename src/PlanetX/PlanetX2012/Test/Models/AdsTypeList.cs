using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class AdsTypeList
    {
        public int AdsTypeListId { get; set; }
        public sbyte AdsTypeId { get; set; }
        public string AdName { get; set; }
        public decimal Cost { get; set; }
    }
}
