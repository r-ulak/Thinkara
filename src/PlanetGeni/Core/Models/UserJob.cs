using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserJob
    {
        public System.Guid TaskId { get; set; }
        public int UserId { get; set; }
        public short JobCodeId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal Salary { get; set; }
        public decimal IncomeYearToDate { get; set; }
        public System.DateTime NextOverTimeCheckIn { get; set; }
        public short CheckInDuration { get; set; }
        public short OverTimeHours { get; set; }
        public short LastCycleOverTimeHours { get; set; }
        public string Status { get; set; }
        public System.DateTime AppliedOn { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
