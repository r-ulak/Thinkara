using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class ElectionCandidate
    {
        public Guid TaskId { get; set; }
        public int ElectionId { get; set; }
        public int UserId { get; set; }
        public string CandidateTypeId { get; set; }
        public sbyte PositionTypeId { get; set; }
        public string Status { get; set; }
        public string LogoPictureId { get; set; }
        public Nullable<Guid> PartyId { get; set; }
        public DateTime RequestDate { get; set; }
        public string CountryId { get; set; }

    }
}
