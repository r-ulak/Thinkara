using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Stock
    {
        public short StockId { get; set; }
        public decimal PreviousDayValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal DayChange { get; set; }
        public decimal DayChangePercent { get; set; }
        public string StockName { get; set; }
        public string Ticker { get; set; }
        public string ImageFont { get; set; }
        public string Description { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
