using System;
namespace DTO.Custom
{
    public class ExpiringUserJobDTO 
    {
        public int UserId { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public decimal IncomeYearToDate { get; set; }
        public decimal Salary { get; set; }
        public DateTime EndDate { get; set; }
    }
}
