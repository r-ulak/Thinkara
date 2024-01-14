using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class StockTrade
    {
        public System.Guid TradeId { get; set; }
        public Nullable<Guid> UserStockId { get; set; }
        public int UserId { get; set; }
        public short StockId { get; set; }
        public int LeftUnit { get; set; }
        public int InitialUnit { get; set; }
        public decimal OfferPrice { get; set; }
        public System.DateTime RequestedAt { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public string TradeType { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
