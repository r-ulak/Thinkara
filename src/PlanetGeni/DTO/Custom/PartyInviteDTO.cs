using System;

namespace DTO.Db
{
    public class PartyInviteDTO
    {
        public int UserId { get; set; }
        public string PartyId { get; set; }
        public string PartyName { get; set; }
        public InviteeDTO[] PartyInvites { get; set; }

    }

}
