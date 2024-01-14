using System;
namespace DAO.Models
{
    public partial class PartySummaryDTO
    {
        public PartInviteTypeDTO[] PartyInvites { get; set; }
        public int TotalParties { get; set; }
        public int TotalDonation { get; set; }
    }

    public class PartInviteTypeDTO
    {
        public Int64 TotalCount { get; set; }
        public string MemberType { get; set; }
    
    }
}
