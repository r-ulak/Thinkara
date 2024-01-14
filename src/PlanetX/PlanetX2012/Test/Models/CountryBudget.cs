using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class CountryBudget
    {
        public int BudgetId { get; set; }
        public string CountryId { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public sbyte Status { get; set; }
    }
}
