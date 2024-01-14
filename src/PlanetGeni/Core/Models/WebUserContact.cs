using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class WebUserContact
    {
        public int UserId { get; set; }
        public Guid InvitationId { get; set; }
        public string FriendEmailId { get; set; }
        public int FriendUserId { get; set; }
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        public sbyte PartyInvite { get; set; }
        public sbyte JoinInvite { get; set; }
        public bool Unsubscribe { get; set; }
        public DateTime LastInviteDate { get; set; }
    }
}
