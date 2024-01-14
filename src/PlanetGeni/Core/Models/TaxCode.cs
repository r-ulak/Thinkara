using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class TaxCode
    {
        public sbyte TaxType { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public bool AllowEdit { get; set; }
    }
}
