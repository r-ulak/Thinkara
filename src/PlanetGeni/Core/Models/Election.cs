using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Election
    {
        public int ElectionId { get; set; }
        public string CountryId{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime VotingStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Fee { get; set; }
        public bool StartTermNotified { get; set; }
        public bool VotingStartTermNotified { get; set; }
        public bool LastDayTermNotified { get; set; }
    }
}
