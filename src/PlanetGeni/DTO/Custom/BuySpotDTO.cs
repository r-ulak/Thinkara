using System;
namespace DTO.Custom
{
    public class BuySpotDTO
    {
        public int UserId { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Cost { get; set; }
        public decimal CalculatedTotalCost { get; set; }
        public decimal CalculatedTax { get; set; }
        public string CountryId { get; set; }
        public int CountryUserId { get; set; }
        public string ImageName { get; set; }
        public string PreviewMsg { get; set; }
        public string Message { get; set; }
        public int ImageBufferLength { get; set; }
    }
}
