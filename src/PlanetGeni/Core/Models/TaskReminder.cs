using System;
namespace DAO.Models
{
    public partial class TaskReminder
    {
        public Guid TaskId { get; set; }
        public int ReminderFrequency { get; set; }
        public string ReminderTransPort { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
