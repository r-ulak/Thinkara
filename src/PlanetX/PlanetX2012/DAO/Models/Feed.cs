using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Feed
    {
        public Feed()
        {
            this.FeedInfoes = new List<FeedInfo>();
        }

        public long FeedId { get; set; }
        public string FeedUrl { get; set; }
        public Nullable<short> Rating { get; set; }
        public sbyte Privacy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<short> FeedCategoryId { get; set; }
        public Nullable<short> FeedSubCategoryId { get; set; }
        public virtual ICollection<FeedInfo> FeedInfoes { get; set; }
    }
}
