using System;

namespace DAO.Models
{
    public partial class PartyNomination
    {
        public System.Guid TaskId { get; set; }
        public System.Guid PartyId { get; set; }
        public int InitatorId { get; set; }
        public int NomineeId { get; set; }
        public string NomineeIdMemberType { get; set; }
        public string NominatingMemberType { get; set; }
        public System.DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string GetPartyMemberType()
        {
            string partyMemberType = "";
            if (NominatingMemberType == "F")
            {
                partyMemberType = "Founder";
            }
            else if (NominatingMemberType == "C")
            {
                partyMemberType = "CoFounder";
            }
            else if (NominatingMemberType == "M")
            {
                partyMemberType = "Member";
            }
            return partyMemberType;
        }
    }

}
