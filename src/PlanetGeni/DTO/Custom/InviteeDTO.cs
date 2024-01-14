using System.ComponentModel.DataAnnotations;
using System;
namespace DTO.Db
{
    public class InviteeDTO
    {
        public int FriendId { get; set; }
        public string EmailId { get; set; }
        public bool AlreadyCurrentPartyMember { get; set; }
        public bool HasPendingPartyInviteForCurrentParty { get; set; }
        public ValidationResult IsValid { get; set; }
        public string FullName { get; set; }
    }


}
