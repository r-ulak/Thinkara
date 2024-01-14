using System;
namespace DTO.Db
{
    public class StockPriceDTO
    {
        public short StockId { get; set; }
        public decimal Price { get; set; }
        public decimal TotalUnit { get; set; }
    }


}
