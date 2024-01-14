using System;

namespace DAO.Models
{
    public partial class CountryLeader
    {
        public string CountryId { get; set; }
        public int UserId { get; set; }
        public string CandidateTypeId { get; set; }
        public sbyte PositionTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
