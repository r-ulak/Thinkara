using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class SchoolCode
    {
        public short SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string ImageSrc { get; set; }
        public string Description { get; set; }
        public decimal CostPerCredit { get; set; }
    }
}
