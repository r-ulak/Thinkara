using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class UserTask
    {
        public System.Guid TaskId { get; set; }
        public int UserId { get; set; }
        public int AssignerUserId { get; set; }
        public System.Guid ReminderId { get; set; }
        public sbyte CompletionPercent { get; set; }
        public bool Flagged { get; set; }
        public string Status { get; set; }
        public string Parms { get; set; }
        public sbyte TaskTypeId { get; set; }
        public System.DateTime DueDate { get; set; }
        public string DefaultResponse { get; set; }
        public sbyte Priority { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
