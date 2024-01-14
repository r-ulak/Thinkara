using System;
namespace DTO.Db
{
    public class PayMeDTO
    {
        public int ReciepentId { get; set; }
        public int SourceUserId { get; set; }
        public string CountryId { get; set; }
        public Guid TaskId { get; set; }
        public sbyte FundType { get; set; }
        public sbyte TaxCode { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
    }


}
