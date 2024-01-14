using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class ApplyJob
    {
        public short JobId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> AppliedDate { get; set; }
        public Nullable<decimal> MatchPercent { get; set; }
    }
}
