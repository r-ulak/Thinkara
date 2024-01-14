namespace DAO.Models
{
    public partial class MerchandiseTypeDTO
    {
        public short MerchandiseTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string ImageFont { get; set; }
        public decimal ResaleRate { get; set; }
        public int Quantity { get; set; }
        public decimal RentalPrice { get; set; }
        public short MerchandiseTypeCode { get; set; }
    }
}
