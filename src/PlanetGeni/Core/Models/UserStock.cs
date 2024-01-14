using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserStock
    {
        public Guid UserStockId { get; set; }
        public int UserId { get; set; }
        public short StockId { get; set; }
        public int PurchasedUnit { get; set; }
        public decimal PurchasedPrice { get; set; }
        public System.DateTime PurchasedAt { get; set; }
    }
}
