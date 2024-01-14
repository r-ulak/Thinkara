using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Privacy
    {
        public int PrivacyId { get; set; }
        public sbyte Profile { get; set; }
        public sbyte Address { get; set; }
        public sbyte Status { get; set; }
        public sbyte Bookmark { get; set; }
        public sbyte Feed { get; set; }
        public sbyte Activity { get; set; }
        public sbyte Friend { get; set; }
        public sbyte FriendList { get; set; }
        public sbyte Nickname { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
