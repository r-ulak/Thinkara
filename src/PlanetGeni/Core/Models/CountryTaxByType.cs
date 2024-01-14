using System;

namespace DAO.Models
{
    public partial class CountryTaxByType
    {
        public Guid TaskId { get; set; }
        public decimal TaxPercent { get; set; }
        public sbyte TaxType { get; set; }
    }
}
