using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserStock
    {
        public int UserId { get; set; }
        public short StockId { get; set; }
        public short PurchasedUnit { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
