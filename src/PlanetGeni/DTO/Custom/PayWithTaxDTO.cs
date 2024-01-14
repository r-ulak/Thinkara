using System;
namespace DTO.Db
{
    public class PayWithTaxDTO
    {
        public int UserId { get; set; }
        public int SourceId { get; set; }
        public string SourceFullName { get; set; }
        public string SourcePicture{ get; set; }
        public string UserFullName { get; set; }
        public string UserPicture { get; set; }
        public decimal Amount { get; set; }
        public sbyte FundType { get; set; }
        public sbyte TaxType { get; set; }
        public string CountryId { get; set; }

    }
}
