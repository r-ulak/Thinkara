using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class CandidateAgenda
    {
        public int ElectionId { get; set; }
        public int UserId { get; set; }
        public short AgendaTypeId { get; set; }
    }
}
