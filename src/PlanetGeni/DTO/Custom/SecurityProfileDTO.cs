using System;
namespace DTO.Db
{
    public class SecurityProfileDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public decimal Cost { get; set; }
        public decimal PurchasedPrice { get; set; }
        public short WeaponCondition { get; set; }
        public int Quantity { get; set; }
    }

}
