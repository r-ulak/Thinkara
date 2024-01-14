using DAO.Models;
using System;
namespace DTO.Db
{
    public class VotingCandidateDTO
    {
        public string FullName { get; set; }
        public string Picture { get; set; }
        public string CountryId { get; set; }
        public string CandidateTypeId { get; set; }
        public string ElectionPositionName { get; set; }
        public string PartyName { get; set; }
        public Nullable<Guid> PartyId { get; set; }
        public Guid TaskId { get; set; }
        public string LogoPictureId { get; set; }
        public int UserId { get; set; }

    }


}
