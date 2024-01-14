using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Friend
    {
        public int FollowerUserId { get; set; }
        public int FollowingUserId { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
