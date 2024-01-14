using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PartyMember
    {
        public Guid PartyId { get; set; }
        public int UserId { get; set; }
        public string MemberType { get; set; }
        public string MemberStatus { get; set; }
        public Nullable<System.DateTime> MemberStartDate { get; set; }
        public Nullable<System.DateTime> MemberEndDate { get; set; }
        public decimal DonationAmount { get; set; }
    }
}
