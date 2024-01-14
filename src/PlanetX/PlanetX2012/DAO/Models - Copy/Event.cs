using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Event
    {
        public Event()
        {
            this.EventMemberships = new List<EventMembership>();
        }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Description { get; set; }
        public short EventType { get; set; }
        public string Picture { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual ICollection<EventMembership> EventMemberships { get; set; }
        public virtual EventLocation EventLocation { get; set; }
    }
}
