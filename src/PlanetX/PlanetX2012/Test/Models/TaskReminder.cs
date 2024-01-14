using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class TaskReminder
    {
        public System.Guid ReminderId { get; set; }
        public short ReminderFrequency { get; set; }
        public string ReminderTransPort { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    }
}
