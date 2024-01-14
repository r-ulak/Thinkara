using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class CountryLeader
    {
        public string CountryId { get; set; }
        public int UserId { get; set; }
        public bool CandidateTypeId { get; set; }
        public sbyte PositionTypeId { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    }
}
