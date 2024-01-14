using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Nomination
    {
        public int ElectionId { get; set; }
        public int UserId { get; set; }
        public bool CandidateTypeId { get; set; }
        public sbyte PositionTypeId { get; set; }
    }
}
