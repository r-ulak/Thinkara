using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class PostTag
    {
        public int PostTagId { get; set; }
        public Nullable<System.Guid> PostId { get; set; }
        public int TopicTagId { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
