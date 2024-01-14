using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Education
    {
        public int EducationId { get; set; }
        public int UserId { get; set; }
        public sbyte DegreeType { get; set; }
        public short MajorType { get; set; }
        public Nullable<short> SchoolId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
