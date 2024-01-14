using System;

namespace DTO.Db
{
    public class GetPartyMemberTypeDTO
    {
        public Guid PartyId { get; set; }
        public string MemberType { get; set; }
        public DateTime? LastStartDate { get; set; }
    }
}
