using DAO.Models;
using System;
namespace DTO.Custom
{
    public class VoteCountingDTO
    {
        public CountryCode CountryCode { get; set; }
        public short Priority { get; set; }
        public int ElectionId { get; set; }
        public int LeaderLimit { get; set; }
    }
}
