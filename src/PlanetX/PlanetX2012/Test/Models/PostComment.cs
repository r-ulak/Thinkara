using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class PostComment
    {
        public System.Guid PostCommentId { get; set; }
        public int UserId { get; set; }
        public System.Guid PostId { get; set; }
        public Nullable<System.Guid> ParentCommentId { get; set; }
        public Nullable<int> ThumbsUp { get; set; }
        public Nullable<int> ThumbsDown { get; set; }
        public Nullable<int> ChildCommentCount { get; set; }
        public string CommentText { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSpam { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
