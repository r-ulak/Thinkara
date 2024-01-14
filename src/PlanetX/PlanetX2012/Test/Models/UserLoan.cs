using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class UserLoan
    {
        public System.Guid LoanId { get; set; }
        public Nullable<System.Guid> TaskId { get; set; }
        public int UserId { get; set; }
        public int LendorId { get; set; }
        public Nullable<decimal> LoanAmount { get; set; }
        public Nullable<decimal> LeftAmount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> MonthlyInterestRate { get; set; }
        public string LoanRequestStatus { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
