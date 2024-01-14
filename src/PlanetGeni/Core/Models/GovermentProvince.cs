using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class GovermentProvince
    {
        public string CountryId { get; set; }
        public int Ground { get; set; }
        public int Air { get; set; }
        public int Navy { get; set; }
        public int Nuclear { get; set; }
        public int Special { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
