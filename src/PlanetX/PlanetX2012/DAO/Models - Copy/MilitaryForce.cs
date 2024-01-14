using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class MilitaryForce
    {
        public string CountryId { get; set; }
        public short Ground { get; set; }
        public short Air { get; set; }
        public short Navy { get; set; }
        public short Nuclear { get; set; }
        public short Special { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual CountryCode CountryCode { get; set; }
    }
}
