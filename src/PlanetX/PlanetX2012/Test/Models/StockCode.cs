using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class StockCode
    {
        public short StockId { get; set; }
        public string StockName { get; set; }
        public int BusinessId { get; set; }
        public int OwnerId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
