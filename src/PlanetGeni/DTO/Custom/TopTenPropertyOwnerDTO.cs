using DAO.Models;
namespace DTO.Custom
{
    public class TopTenPropertyOwnerDTO : WebUserDTO
    {
        public int RankId { get; set; }
        public decimal TotalValue { get; set; }
    }
}
