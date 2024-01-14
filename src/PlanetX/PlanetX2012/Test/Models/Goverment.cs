using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Goverment
    {
        public string CountryId { get; set; }
        public int LeaderId { get; set; }
        public sbyte leaderType { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
