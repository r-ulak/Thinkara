using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PostTag
    {
        public int PostTagId { get; set; }
        public string PostId { get; set; }
        public int TopicTagId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
