using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PartyAgenda
    {
        public short AgendaTypeId { get; set; }
        public Guid PartyId { get; set; }
    }
}
