using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class DegreeOffered
    {
        public short SchoolId { get; set; }
        public sbyte DegreeId { get; set; }
        public short MajorId { get; set; }
    }
}
