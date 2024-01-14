using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class StocksTransaction
    {
        public int StocksTransactionId { get; set; }
        public int OwnerId { get; set; }
        public sbyte TransactionType { get; set; }
        public short StockId { get; set; }
        public decimal ValuePerUnit { get; set; }
        public short UnitPrice { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
