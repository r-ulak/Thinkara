using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Card
    {
        public int CardId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<sbyte> CardType { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public System.DateTime ExpireDate { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
