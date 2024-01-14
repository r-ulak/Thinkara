using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class MajorCode
    {
        public short MajorId { get; set; }
        public string MajorName { get; set; }
        public string ImageSrc { get; set; }
        public string Description { get; set; }
        public sbyte DegreeRank { get; set; }
    }
}
