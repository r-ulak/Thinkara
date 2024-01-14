using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class PostUserACL
    {
        public long PostId { get; set; }
        public int UserId { get; set; }
        public sbyte AccessType { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
