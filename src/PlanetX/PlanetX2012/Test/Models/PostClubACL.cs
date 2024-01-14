using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class PostClubACL
    {
        public long PostId { get; set; }
        public int ClubId { get; set; }
        public sbyte AccessType { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual Club Club { get; set; }
    }
}
