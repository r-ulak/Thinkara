using System;
namespace DTO.Db
{
    public class RunForOfficeTicketDTO
    {
        public string FullName { get; set; }
        public string PartyName { get; set; }
        public Guid PartyId { get; set; }
        public string Picture { get; set; }
        public string CountryId { get; set; }
        public string LogoPictureId { get; set; }
        public string CandidateTypeId { get; set; }
        public string ElectionPositionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime VotingStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ElectionId { get; set; }
        public int[] CandidateAgendaId { get; set; }

    }


}
