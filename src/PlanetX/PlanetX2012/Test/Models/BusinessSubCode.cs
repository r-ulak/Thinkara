using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class BusinessSubCode
    {
        public short BusinessSubtypeId { get; set; }
        public string Code { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public short BusinessTypeId { get; set; }
        public virtual BusinessCode BusinessCode { get; set; }
    }
}
