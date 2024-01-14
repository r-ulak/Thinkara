using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DAO.Models
{
    public partial class UserTask
    {
        public Guid TaskId { get; set; }
        public int UserId { get; set; }
        public int AssignerUserId { get; set; }        
        public sbyte CompletionPercent { get; set; }
        public bool Flagged { get; set; }
        [DisplayName("OnClickUrl")]
        public string OnClickUrl { get; set; }
        public string Status { get; set; }
        public string Parms { get; set; }
        public short TaskTypeId { get; set; }
        public System.DateTime DueDate { get; set; }
        public sbyte Priority { get; set; }
        public short DefaultResponse { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
