using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Chat
    {
        public int ChatId { get; set; }
        public Nullable<int> UserId { get; set; }
        public int ToId { get; set; }
        public string Msg { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
