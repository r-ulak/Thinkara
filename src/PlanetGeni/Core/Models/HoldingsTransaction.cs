using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class HoldingsTransaction
    {
        public System.Guid TransactionId { get; set; }
        public int UserId { get; set; }
        public sbyte TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
