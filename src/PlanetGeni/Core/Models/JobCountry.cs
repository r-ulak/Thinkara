using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class JobCountry
    {
        public short JobCodeId { get; set; }
        public string CountryId { get; set; }
        public decimal Salary { get; set; }
        public int QuantityAvailable { get; set; }
    }
}
