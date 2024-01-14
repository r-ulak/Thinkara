using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class AdsType
    {
        public sbyte AdsTypeId { get; set; }
        public string AdName { get; set; }
        public decimal BaseCost { get; set; }
        public decimal PricePerChar { get; set; }
        public decimal PricePerImageByte { get; set; }
        public string FontCss { get; set; }
    }
}
