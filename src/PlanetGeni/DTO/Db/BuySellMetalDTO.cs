using System;
namespace DAO.Models
{
    public partial class BuySellMetalDTO
    {
        public decimal GoldDelta { get; set; }
        public decimal SilverDelta { get; set; }
        public decimal Delta { get; set; }
        public string OrderType { get; set; }
        public int UserId { get; set; }
    }
}
