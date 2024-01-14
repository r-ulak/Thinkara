using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class GroupMembership
    {
        public int UserId { get; set; }
        public int UserGroupId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
