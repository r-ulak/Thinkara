
using System;
namespace DTO.Db
{
    public class AdvertisementPostDTO
    {
        public int[] AdsTypeList { get; set; }
        public int UserId { get; set; }
        public int[] Days { get; set; }
        public sbyte AdsFrequencyTypeId { get; set; }
        public int AdTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ImageSize { get; set; }
        public string PreviewMsg { get; set; }
        public string Message { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Cost { get; set; }
        public decimal CalculatedTotalCost { get; set; }
        public decimal CalculatedTax { get; set; }
        public string CountryId { get; set; }
        public Guid AdvertisementId { get; set; }
    }

}
