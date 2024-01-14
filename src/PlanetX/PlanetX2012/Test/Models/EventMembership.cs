using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class EventMembership
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public Nullable<sbyte> StatusType { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
