using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CapitalTransactionLog
    {
        public System.Guid LogId { get; set; }
        public int SourceId { get; set; }
        public Guid SourceGuid { get; set; }
        public int RecipentId { get; set; }
        public Guid RecipentGuid { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxAmount { get; set; }
        public sbyte FundType { get; set; }
        public System.DateTime CreatedAT { get; set; }
    }
}
