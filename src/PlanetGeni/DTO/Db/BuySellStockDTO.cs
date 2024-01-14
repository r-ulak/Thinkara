using System;
namespace DAO.Models
{
    public partial class BuySellStockDTO
    {
        public Guid UserStockId { get; set; }
        public short StockId { get; set; }
        public short Quantity { get; set; }
        public decimal OfferPrice { get; set; }
        public decimal Tax { get; set; }
        public string TradeType { get; set; }
        public string OrderType { get; set; }
    }
}
