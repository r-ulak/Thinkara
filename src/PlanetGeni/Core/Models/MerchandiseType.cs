using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class MerchandiseType
    {
        public short MerchandiseTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public decimal Cost { get; set; }
        public decimal ResaleRate { get; set; }
        public decimal RentalPrice { get; set; }
        public sbyte MerchandiseTypeCode { get; set; }
    }
}
