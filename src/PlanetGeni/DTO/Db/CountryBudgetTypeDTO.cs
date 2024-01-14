using DAO.Models;
namespace DTO.Db
{
    public class CountryBudgetTypeDTO : CountryBudgetByType
    {
        public string Description { get; set; }
        public decimal BudgetPercent { get; set; }
        public string ImageFont { get; set; }
        public decimal BudgetRatio { get; set; }
    }
}
