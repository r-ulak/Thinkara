using Common;
using System;

namespace DAO.Models
{
    public partial class CrimeReportDTO
    {
        public int UserId { get; set; }
        public decimal WantedScore { get; set; }
        public int ArrestCount { get; set; }
        public decimal LootToDate { get; set; }
        public int SuspectCount { get; set; }
        public int IncidentCount { get; set; }
        public DateTime LastArrestDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal MaxWantedLevel { get; set; }
        public CrimeReportDTO()
        {
            MaxWantedLevel = RulesSettings.RobberyMaxWantedLevel;
        }
    }
}
