using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Gift
    {
        public System.Guid GiftId { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public decimal Cash { get; set; }
        public decimal Gold { get; set; }
        public decimal Silver { get; set; }
        public decimal TaxAmount { get; set; }
        public short MerchandiseTypeId { get; set; }
        public decimal MerchandiseValue { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
