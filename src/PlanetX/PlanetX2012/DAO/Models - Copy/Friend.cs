using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Friend
    {
        public int FriendId { get; set; }
        public int UserId { get; set; }
        public Nullable<int> FriendUserId { get; set; }
        public bool IsSubscriber { get; set; }
        public sbyte Privacy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
    }
}
