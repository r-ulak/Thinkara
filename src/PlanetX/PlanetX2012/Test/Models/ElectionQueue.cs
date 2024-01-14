using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class ElectionQueue
    {
        public int UserId { get; set; }
        public sbyte Active { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
