using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class StockTradeHistory
    {
        public System.Guid StockTradeHistoryId { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public short StockId { get; set; }
        public int Unit { get; set; }
        public decimal DealPrice { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
