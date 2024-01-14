using DAO.Models;
using System;
namespace DTO.Db
{
    public class UserJobCodeDTO: IndustryCode
    {
        public Guid TaskId { get; set; }
        public short JobCodeId { get; set; }
        public string Title { get; set; }
        public string JobType { get; set; }
        public short Duration { get; set; }
        public sbyte CheckInDuration { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public decimal OverTimeRate { get; set; }
        public short OverTimeHours { get; set; }
        public decimal IncomeYearToDate { get; set; }
        public System.DateTime NextOverTimeCheckIn { get; set; }
        public sbyte MaxHPW { get; set; }
        public decimal Salary { get; set; }
        public string Status { get; set; }
        public DateTime AppliedOn { get; set; }
        public decimal CompletionPercent { get; set; }
        public string KnobColor { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }


}
