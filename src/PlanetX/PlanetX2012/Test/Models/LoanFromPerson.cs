using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class LoanFromPerson
    {
        public int LoanId { get; set; }
        public int UserId { get; set; }
        public int SourceId { get; set; }
        public sbyte LoanType { get; set; }
        public Nullable<decimal> LoanAmount { get; set; }
        public Nullable<decimal> MonthlyInterestRate { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual LoanCode LoanCode { get; set; }
    }
}
