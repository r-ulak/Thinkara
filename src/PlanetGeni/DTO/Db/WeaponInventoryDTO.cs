using System;
namespace DAO.Models
{
    public partial class WeaponInventoryDTO
    {
        public int CountryWeaponId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public string WeaponTypeCode { get; set; }
        public decimal PurchasedPrice { get; set; }
        public DateTime PurchasedAt { get; set; }
        public short WeaponCondition { get; set; }
        public int Quantity { get; set; }
    }
}
