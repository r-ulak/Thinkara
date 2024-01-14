namespace DAO.Models
{
    public partial class WeaponSummaryDTO
    {
        public decimal TotalQty { get; set; }
        public decimal AverageCondition { get; set; }
        public decimal TotalPrice { get; set; }
        public string WeaponTypeCode { get; set; }
        public string CountryId { get; set; }
    }
}
