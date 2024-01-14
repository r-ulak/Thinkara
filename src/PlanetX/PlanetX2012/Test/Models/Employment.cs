using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Employment
    {
        public short JobId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
    }
}
