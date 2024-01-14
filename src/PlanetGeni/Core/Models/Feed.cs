using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Feed
    {
        public long FeedId { get; set; }
        public string FeedUrl { get; set; }
        public Nullable<int> Rating { get; set; }
        public sbyte Privacy { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> FeedCategoryId { get; set; }
        public Nullable<int> FeedSubCategoryId { get; set; }
    }
}
