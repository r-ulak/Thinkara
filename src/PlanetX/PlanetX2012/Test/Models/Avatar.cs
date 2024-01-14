using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Avatar
    {
        public int AvatarId { get; set; }
        public string Picture { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
