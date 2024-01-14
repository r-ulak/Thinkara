using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Club
    {
        public Club()
        {
            this.PostClubACLs = new List<PostClubACL>();
        }

        public int ClubId { get; set; }
        public int UserId { get; set; }
        public string ClubName { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual ICollection<PostClubACL> PostClubACLs { get; set; }
    }
}
