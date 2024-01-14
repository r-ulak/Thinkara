using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class ElectionVoting
    {
        public int ElectionId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public string CountryId{ get; set; }
        public string ElectionResult { get; set; }
    }
}
