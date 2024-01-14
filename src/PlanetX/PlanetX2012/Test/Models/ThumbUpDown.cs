using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class ThumbUpDown
    {
        public int ThumbUpDownId { get; set; }
        public Nullable<bool> Flag { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> FriendId { get; set; }
        public Nullable<int> UserId { get; set; }
        public virtual Status Status { get; set; }
    }
}
