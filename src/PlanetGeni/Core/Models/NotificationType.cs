using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class NotificationType
    {
        public short NotificationTypeId { get; set; }
        public string ShortDescription { get; set; }
        public bool EmailNotification { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
    }
}
