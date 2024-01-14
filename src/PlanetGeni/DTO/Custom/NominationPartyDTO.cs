using System;

namespace DTO.Db
{
    public class PartyNominationDTO
    {
        public string PartyName { get; set; }
        public string InitatorPartyId { get; set; }
        public int InitatorId { get; set; }
        public string InitatorFullName { get; set; }
        public string InitatorPicture { get; set; }
        public string PartyId { get; set; }
        public string NominatingMemberType { get; set; }
        public int NomineeId { get; set; }
        public string NomineeIdMemberType { get; set; }
        public string NomineeIdPartyId { get; set; }
        public string NomineeFullName { get; set; }
        public string NomineePicture { get; set; }
        public string PartyMemberType { get; set; }
        public string NomineeIdMemberStatus { get; set; }
        public bool HasPendingNomination { get; set; }

        public string GetPartyMemberType()
        {
            if (NominatingMemberType == "F")
            {
                PartyMemberType = "Founder";
            }
            else if (NominatingMemberType == "C")
            {
                PartyMemberType = "CoFounder";
            }
            else if (NominatingMemberType == "M")
            {
                PartyMemberType = "Member";
            }
            return PartyMemberType;
        }
    }
}
