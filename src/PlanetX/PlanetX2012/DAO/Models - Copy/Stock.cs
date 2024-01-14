using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Stock
    {
        public short StockId { get; set; }
        public decimal CurrentValue { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
