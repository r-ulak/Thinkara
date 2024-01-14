using DAO.Models;
using System;
namespace DTO.Db
{
    public class CandidateVotingDTO
    {
        public string CountryId { get; set; }
        public string UserCountryId { get; set; }
        public string CandidateCountryId { get; set; }
        public bool HasVotedThisElection { get; set; }
        public Election ElectionInfo { get; set; }
        public sbyte PositionTypeId { get; set; }
        public int[] Candidates { get; set; }
        public int UserId { get; set; }
    }


}
