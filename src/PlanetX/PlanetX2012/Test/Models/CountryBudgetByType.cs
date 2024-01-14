using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class CountryBudgetByType
    {
        public int BudgetId { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountLeft { get; set; }
        public sbyte BudgetType { get; set; }
    }
}
