using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class FollowAllDTO
    {
        public int UserId { get; set; }
        public int[] FriendId { get; set; }

    }
}
