using System;
using System.ComponentModel.DataAnnotations;
namespace DTO.Db
{
    public partial class RequestLoanDTO
    {
        [Required]
        public int LendorId { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal MonthlyIntrestRate { get; set; }
        public decimal MinMonthlyIntrestRate { get; set; }
        public decimal QualifiedAmount { get; set; }

    }

}
