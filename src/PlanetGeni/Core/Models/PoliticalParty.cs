using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PoliticalParty
    {
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public int PartyFounder { get; set; }
        public decimal TotalValue { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int PartySize { get; set; }
        public int CoFounderSize { get; set; }
        public string LogoPictureId { get; set; }
        public decimal MembershipFee { get; set; }
        public short ElectionVictory { get; set; }
        public string Motto { get; set; }
        public string Status { get; set; }
        public string CountryId { get; set; }
    }
}
