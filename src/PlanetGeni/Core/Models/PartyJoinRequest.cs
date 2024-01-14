using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PartyJoinRequest
    {
        public System.Guid TaskId { get; set; }
        public System.Guid PartyId { get; set; }
        public int UserId { get; set; }
        public System.DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
