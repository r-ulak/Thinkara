using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class MessageInfo
    {
        public System.Guid MessageId { get; set; }
        public Nullable<System.Guid> ParentMessageId { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsSpam { get; set; }
        public int ToId { get; set; }
        public int FromId { get; set; }
    }
}
