using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Election
    {
        public int ElectionId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
