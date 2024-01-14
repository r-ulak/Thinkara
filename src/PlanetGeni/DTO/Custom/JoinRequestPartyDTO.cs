using System;
using System.ComponentModel.DataAnnotations;
namespace DTO.Db
{
    public class JoinRequestPartyDTO
    {
        public string PartyId { get; set; }
        public String PartyName { get; set; }
        public bool IsAlreadyCurrentPartyMember { get; set; }
        public bool HasPendingJoinRequest { get; set; }
        public ValidationResult IsValid { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public DateTime RequestDateTime{ get; set; }
    }


}
