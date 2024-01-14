using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Merchandise
    {
        public int ItemId { get; set; }
        public Nullable<short> ItemType { get; set; }
        public decimal Cost { get; set; }
        public string Picture { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual Asset Asset { get; set; }
    }
}
