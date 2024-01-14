using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public string Message1 { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsSpam { get; set; }
        public Nullable<int> ToId { get; set; }
        public Nullable<bool> IsReply { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
