using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class TopicTag
    {
        public int TopicTagId { get; set; }
        public string Tag { get; set; }
        public Nullable<int> TagCount { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
