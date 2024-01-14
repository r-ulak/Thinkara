using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public string Msg { get; set; }
        public Nullable<short> Type { get; set; }
        public sbyte Privacy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
