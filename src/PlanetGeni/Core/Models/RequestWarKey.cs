using System;
namespace DAO.Models
{
    public partial class RequestWarKey
    {
        public Guid TaskId{ get; set; }
        public string RequestingCountryId { get; set; }
        public string TaregtCountryId { get; set; }
        public int RequestingUserId { get; set; }
        public DateTime RequestedAt { get; set; }
        public string ApprovalStatus { get; set; }
        public string WarStatus { get; set; }
        public string WiningCountryId { get; set; }
    }
}
