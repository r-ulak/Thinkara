using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class UserConnection
    {
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
        public string UserAgent { get; set; }
    }
}
