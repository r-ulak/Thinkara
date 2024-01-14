using DTO.Custom;
namespace DAO.Models
{
    public partial class StockSummaryDTO : Stock
    {
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalCurrentValue { get; set; }
        public decimal TotalUnit { get; set; }
        public int Rank { get; set; }
    }
}
