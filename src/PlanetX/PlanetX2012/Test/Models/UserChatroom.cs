using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class UserChatroom
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public Nullable<sbyte> Status { get; set; }
    }
}
