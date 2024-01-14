using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserNotification
    {
        public System.Guid NotificationId { get; set; }
        public int UserId { get; set; }
        public short NotificationTypeId { get; set; }
        public sbyte Priority { get; set; }
        public bool HasTask { get; set; }
        public string Parms { get; set; }
        public bool EmailSent { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
