using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class UserVote
    {
        public System.Guid VoteId { get; set; }
        public System.Guid TaskId { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
