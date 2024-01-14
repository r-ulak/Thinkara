using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class StockHistory
    {
        public System.Guid HistoryId { get; set; }
        public short StockId { get; set; }
        public decimal CurrentValue { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
