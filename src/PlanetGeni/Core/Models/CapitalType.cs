using System;

namespace DAO.Models
{
    public partial class CapitalType
    {
        public short CapitalTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFont { get; set; }
        public decimal Cost { get; set; }
  
    }
}
