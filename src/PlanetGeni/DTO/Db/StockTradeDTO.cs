using System;
namespace DAO.Models
{
    public partial class StockTradeDTO : Stock
    {
        public System.DateTime RequestedAt { get; set; }
        public string Status { get; set; }
        public string OrderType { get; set; }
        public string TradeType { get; set; }
        public int Unit { get; set; }
        public System.Guid TradeId { get; set; }
        public Nullable<decimal> OfferPrice { get; set; }

    }
}
