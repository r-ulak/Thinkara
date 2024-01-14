using System;
namespace DTO.Db
{
    public class EjectPartyDTO
    {
        public int InitatorId { get; set; }
        public string EjectorPartyId { get; set; }
        public string EjecteePartyId { get; set; }
        public string EjecteeMemberStatus { get; set; }
        public string PartyId { get; set; }
        public string PartyName { get; set; }
        public string InitiatorFullName { get; set; }
        public string EjecteeFullName { get; set; }
        public string EjecteeMemberType { get; set; }
        public bool IsEjectorFounderorCoFounder { get; set; }
        public int EjecteeId { get; set; }
    }


}
