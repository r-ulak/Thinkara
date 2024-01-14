using System;

namespace DTO.Db
{
    public class StartPartyDTO
    {
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public int InitatorId { get; set; }
        public string FullName{ get; set; }
        public string LogoPictureId { get; set; }
        public decimal MembershipFee { get; set; }
        public string Motto { get; set; }
        public string CountryId { get; set; }
        public int[] FriendInvitationList { get; set; }
        public string[] ContactInvitationList { get; set; }
        public bool IsActiveMemberOfDiffrentParty { get; set; }
        public bool IsUniquePartyName { get; set; }
        public bool PartyNameChanged { get; set; }
        public string OriginalPartyName { get; set; }
        public short[] AgendaType { get; set; }
    }
}
