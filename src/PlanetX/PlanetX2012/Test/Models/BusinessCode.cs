using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class BusinessCode
    {
        public BusinessCode()
        {
            this.BusinessSubCodes = new List<BusinessSubCode>();
        }

        public short BusinessTypeId { get; set; }
        public string Code { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public virtual ICollection<BusinessSubCode> BusinessSubCodes { get; set; }
    }
}
