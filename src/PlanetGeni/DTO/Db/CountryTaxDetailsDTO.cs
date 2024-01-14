using System;
namespace DTO.Db
{
    public class CountryTaxDetailsDTO
    {
        public Guid TaskId { get; set; }
        public string CountryId { get; set; }
        public string Status { get; set; }
        public bool AllowEdit { get; set; }
        public decimal Total { get; set; }
        public CountryTaxTypeDTO[] TaxType { get; set; }
    }


}
