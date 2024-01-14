using DAO.Models;
using System;
namespace DTO.Db
{
    public class ElectionCandidateDTO
    {
        public string FullName { get; set; }
        public string Picture { get; set; }
        public string CountryId { get; set; }
        public string CandidateTypeId { get; set; }
        public string ElectionPositionName { get; set; }
        public string PartyName { get; set; }
        public Nullable<Guid> PartyId { get; set; }
        public string LogoPictureId { get; set; }
        public int ElectionId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public string Status { get; set; }
        public string ElectionResult { get; set; }

    }


}
