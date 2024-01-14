using System;
namespace DTO.Db
{
    public class PartyProfileDTO
    {
        public decimal TotalValue { get; set; }
        public int PartySize { get; set; }
        public string PartyName { get; set; }
        public string Motto { get; set; }
        public Guid PartyId { get; set; }
        public string CountryId { get; set; }
        public string LogoPictureId { get; set; }
        public string MemberType { get; set; }
        public Nullable<System.DateTime> MemberStartDate { get; set; }
        public Nullable<System.DateTime> MemberEndDate { get; set; }
      
    }


}
