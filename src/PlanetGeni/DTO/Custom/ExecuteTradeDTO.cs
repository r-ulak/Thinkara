using System;
namespace DTO.Custom
{
    public class ExecuteTradeDTO
    {
        public Guid SellerStockId { get; set; }
        public int StockId { get; set; }
        public int PurchasedUnit { get; set; }
        public decimal PurchasedPrice { get; set; }
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public decimal TotalValue { get; set; }
        public decimal TaxValue { get; set; }
        public int TaxCode { get; set; }
        public string BuyerCountryId { get; set; }


    }
}
