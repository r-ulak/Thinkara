using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class PartyMember
    {
        public short PartyId { get; set; }
        public int UserId { get; set; }
        public sbyte Active { get; set; }
        public sbyte PartyMemberTypeId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
