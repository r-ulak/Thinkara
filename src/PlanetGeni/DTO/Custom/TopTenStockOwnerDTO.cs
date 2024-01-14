using DAO.Models;
namespace DTO.Custom
{
    public class TopTenStockOwnerDTO : WebUserDTO
    {
        public decimal TotalPurchaseValue { get; set; }
        public decimal TotalCurrentValue { get; set; }
    }
}
