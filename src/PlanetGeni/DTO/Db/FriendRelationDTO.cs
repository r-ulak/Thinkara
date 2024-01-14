using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class FriendRelationDTO
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public string ActionType { get; set; }

    }
}
