using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CrimeIncidentSuspect
    {
        public Guid IncidentId{ get; set; }
        public int UserId { get; set; }
        public int SuspectId { get; set; }
    }
}
