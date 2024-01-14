using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CountryRevenue
    {
        public string CountryId { get; set; }
        public decimal Cash { get; set; }
        public string Status { get; set; }
        public sbyte TaxType { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
