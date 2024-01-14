using System;
using DAO.Models;
namespace DTO.Db
{
    public class PartyMemberDTO
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        public string MemberType { get; set; }
        public string MemberStatus { get; set; }
        public Nullable<System.DateTime> MemberStartDate { get; set; }
        public Nullable<System.DateTime> MemberEndDate { get; set; }
        public decimal DonationAmount { get; set; }

    }
}
