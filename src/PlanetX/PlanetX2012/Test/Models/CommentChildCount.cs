using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class CommentChildCount
    {
        public long cnt { get; set; }
        public Nullable<System.Guid> ParentCommentId { get; set; }
    }
}
