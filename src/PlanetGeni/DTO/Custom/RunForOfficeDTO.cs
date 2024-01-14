using DAO.Models;
using System;
namespace DTO.Db
{
    public class RunForOfficeDTO
    {
        public sbyte PositionTypeId { get; set; }
        public string CandidateTypeId { get; set; }
        public string FullName { get; set; }
        public string PartyName { get; set; }
        public string LogoPictureId { get; set; }
        public string CountryId { get; set; }
        public string PartyId { get; set; }
        public string Picture { get; set; }
        public int[] FriendSelected { get; set; }
        public short[] Agendas { get; set; }
        public int UserId { get; set; }
        public int TotalPopulation { get; set; }
        public int PartySize { get; set; }
        public bool HasPendingApplication { get; set; }
        public string PartyStatus { get; set; }
        public int ConsecutiveTerm { get; set; }
        public decimal TotalCash { get; set; }
        public int NumberofApprovedPartyMembers { get; set; }
        public int NumberOfApprovedCandidate { get; set; }
        public Guid TaskId { get; set; }
        public Election CurrentTerm { get; set; }
    }


}
