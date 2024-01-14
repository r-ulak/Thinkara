using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class FeedInfo
    {
        public long FeedInfoId { get; set; }
        public Nullable<long> FeedId { get; set; }
        public Nullable<int> UserId { get; set; }
        public sbyte Favorite { get; set; }
        public Nullable<int> Clicks { get; set; }
        public sbyte Privacy { get; set; }
    }
}
