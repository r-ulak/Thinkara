using System;
namespace DAO.Models
{
    public partial class UserStockDTO : Stock
    {
        public Guid UserStockId { get; set; }
        public int PurchasedUnit { get; set; }
        public decimal PurchasedPrice { get; set; }
        public System.DateTime PurchasedAt { get; set; }

    }
}
