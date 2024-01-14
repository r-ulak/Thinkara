using System;
using System.ComponentModel.DataAnnotations;
namespace DTO.Db
{
    public partial class UserLoanDTO
    {
        public Guid TaskId { get; set; }
        public string Picture { get; set; }
        public sbyte OnlineStatus { get; set; }
        public int UserId { get; set; }
        public int NextPartyId { get; set; }
        public string FullName { get; set; }
        [Required]
        public decimal LoanAmount { get; set; }
        [Required]
        public decimal MonthlyInterestRate { get; set; }
        public string Status { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PayingAmount { get; set; }
        public decimal LeftAmount { get; set; }
        public string LoanSourceType { get; set; } // B-Borrower, L-Lendor
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

    }

}
