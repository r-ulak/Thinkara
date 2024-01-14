using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CountryBudget
    {
        public Guid TaskId { get; set; }
        public string CountryId { get; set; }
        public decimal TotalAmount { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
