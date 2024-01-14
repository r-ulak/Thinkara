using System;
namespace DTO.Db
{
    public class PayNationDTO
    {
        public int UserId { get; set; }
        public int CountryUserId { get; set; }
        public string CountryId { get; set; }
        public Guid TaskId { get; set; }
        public sbyte FundType { get; set; }
        public sbyte TaxCode { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
    }


}
