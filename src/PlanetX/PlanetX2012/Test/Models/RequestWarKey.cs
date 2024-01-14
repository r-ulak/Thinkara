using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class RequestWarKey
    {
        public int RequestId { get; set; }
        public string RequestingCountryId { get; set; }
        public string TaregtCountryId { get; set; }
        public int RequestingUserId { get; set; }
        public System.DateTime RequestedAt { get; set; }
        public string ApprovalStatus { get; set; }
        public string WarStatus { get; set; }
        public string WiningCountryId { get; set; }
    }
}
