using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PartyEjection
    {
        public System.Guid TaskId { get; set; }
        public System.Guid PartyId { get; set; }
        public int InitatorId { get; set; }
        public int EjecteeId { get; set; }
        public string EjecteeMemberType { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string Status { get; set; }
    }
}
