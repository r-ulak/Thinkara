using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class my_aspnet_sessions
    {
        public string SessionId { get; set; }
        public int ApplicationId { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Expires { get; set; }
        public System.DateTime LockDate { get; set; }
        public int LockId { get; set; }
        public int Timeout { get; set; }
        public bool Locked { get; set; }
        public byte[] SessionItems { get; set; }
        public int Flags { get; set; }
    }
}
