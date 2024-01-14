using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class ClubMember
    {
        public int ClubMemberId { get; set; }
        public int UserId { get; set; }
        public int ClubId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
