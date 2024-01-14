using System;
namespace DTO.Db
{
    public class DonatePartyDTO
    {
        public int UserId { get; set; }
        public string PartyId { get; set; }
        public string PartyStatus { get; set; }
        public string PartyName { get; set; }
        public decimal Amount { get; set; }
        public bool IsCurrentOrPastParty { get; set; }
    }
}
