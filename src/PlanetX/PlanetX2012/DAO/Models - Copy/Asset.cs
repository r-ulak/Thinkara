using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Asset
    {
        public int ItemId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<short> Qty { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual Merchandise Merchandise { get; set; }
    }
}
