using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class BusinessSubCode
    {
        public int BusinessSubtypeId { get; set; }
        public string Code { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public int BusinessTypeId { get; set; }
        public virtual BusinessCode BusinessCode { get; set; }
    }
}
