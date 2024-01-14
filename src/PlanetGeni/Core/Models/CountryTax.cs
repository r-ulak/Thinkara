using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CountryTax
    {
        public System.Guid TaskId { get; set; }
        public string CountryId { get; set; }
        public string Status { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
