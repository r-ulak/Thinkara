using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PartyInvite
    {
        public Guid TaskId { get; set; }
        public Guid PartyId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string EmailId { get; set; }
        public Nullable<System.DateTime> InvitationDate { get; set; }
        public string Status { get; set; }
        public string MemberType { get; set; }
    }
}
