using DAO.Models;
namespace DTO.Custom
{
    public class TopTenWeaponStackCountryDTO : CountryCode
    {
        public int RankId { get; set; }
        public decimal TotalValue { get; set; }
    }
}
