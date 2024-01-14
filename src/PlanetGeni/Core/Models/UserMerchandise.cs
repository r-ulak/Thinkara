using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserMerchandise
    {
        public short MerchandiseTypeId { get; set; }
        public int UserId { get; set; }
        public short Quantity { get; set; }
        public short MerchandiseCondition { get; set; }
        public decimal PurchasedPrice { get; set; }
        public decimal Tax { get; set; }
        public DateTime PurchasedAt { get; set; }
    }
}
