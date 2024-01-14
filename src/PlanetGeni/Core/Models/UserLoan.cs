using System;

namespace DAO.Models
{
    public partial class UserLoan
    {
        public Guid TaskId { get; set; }
        public int UserId { get; set; }
        public int LendorId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal LeftAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal MonthlyInterestRate { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
