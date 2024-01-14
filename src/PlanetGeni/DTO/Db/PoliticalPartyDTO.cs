using DAO.Models;
using System;
using System.Collections.Generic;
namespace DTO.Db
{
    public class PoliticalPartyDTO
    {
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public string InitatorFullName { get; set; }
        public decimal TotalValue { get; set; }
        public string LogoPictureId { get; set; }
        public int PartySize { get; set; }
        public int ElectionVictory { get; set; }
        public decimal MembershipFee { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Motto { get; set; }
        public string Status { get; set; }
        public string CountryId { get; set; }

    }

}
