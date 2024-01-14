using System;
namespace DAO.Models
{
    public partial class MerchandiseInventoryDTO
    {
        public short MerchandiseTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public sbyte MerchandiseTypeCode { get; set; }
        public decimal PurchasedPrice { get; set; }
        public DateTime PurchasedAt { get; set; }
        public short MerchandiseCondition { get; set; }
        public decimal ResaleRate { get; set; }
        public decimal RentalPrice { get; set; }
        public int Quantity { get; set; }
    }
}
