using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CountryBudgetByType
    {
        public Guid TaskId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountLeft { get; set; }
        public sbyte BudgetType { get; set; }
    }
}
