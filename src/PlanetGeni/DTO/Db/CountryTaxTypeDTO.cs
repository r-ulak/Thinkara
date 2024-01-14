using DAO.Models;
namespace DTO.Db
{
    public class CountryTaxTypeDTO : CountryTaxByType
    {
        public string Description { get; set; }
        public string ImageFont { get; set; }
    }
}
