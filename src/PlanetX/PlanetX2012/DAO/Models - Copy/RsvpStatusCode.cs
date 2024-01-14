using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class RsvpStatusCode
    {
        public RsvpStatusCode()
        {
            this.EventMemberships = new List<EventMembership>();
        }

        public sbyte StatusType { get; set; }
        public string Code { get; set; }
        public virtual ICollection<EventMembership> EventMemberships { get; set; }
    }
}
