using System;

namespace DTO.Db
{
    public class ElectionVotingDTO
    {
        public int ElectionId { get; set; }
        public int Score { get; set; }
        public string ElectionResult { get; set; }

    }
}
