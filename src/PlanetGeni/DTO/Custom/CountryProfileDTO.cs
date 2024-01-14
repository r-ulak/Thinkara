using System;
namespace DTO.Db
{
    public class CountryProfileDTO
    {
        public string CountryId { get; set; }
        public int RichestCountryRank { get; set; }
        public int PopulationRank { get; set; }
        public int SecurityAsset { get; set; }
        public decimal TotalBudget { get; set; }
    }


}
