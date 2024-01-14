using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CountryWeapon
    {
        public int CountryWeaponId { get; set; }
        public string CountryId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
        public short WeaponTypeId { get; set; }
        public short WeaponCondition { get; set; }
        public decimal PurchasedPrice { get; set; }
        public Nullable<System.DateTime> PurchasedAt { get; set; }
    }
}
