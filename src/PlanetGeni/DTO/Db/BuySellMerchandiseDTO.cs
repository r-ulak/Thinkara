namespace DAO.Models
{
    public partial class BuySellMerchandiseDTO
    {
        public short MerchandiseTypeId { get; set; }
        public short Quantity { get; set; }
        public short SellingUnit { get; set; }
        public decimal Cost { get; set; }
        public decimal Tax { get; set; }
    }
}
