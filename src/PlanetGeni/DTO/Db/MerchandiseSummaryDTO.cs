using DTO.Custom;
namespace DAO.Models
{
    public partial class MerchandiseSummaryDTO 
    {
        public decimal TotalQty { get; set; }
        public decimal AverageCondition { get; set; }
        public decimal TotalPrice { get; set; }
        public short MerchandiseTypeCode { get; set; }
        public int UserId { get; set; }
    }
}
