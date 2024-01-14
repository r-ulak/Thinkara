using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PostComment
    {
        public System.Guid PostCommentId { get; set; }
        public int UserId { get; set; }
        public System.Guid PostId { get; set; }
        public Nullable<System.Guid> ParentCommentId { get; set; }
        public uint DigIt { get; set; }
        public uint CoolIt { get; set; }
        public Nullable<int> ChildCommentCount { get; set; }
        public string CommentText { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSpam { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
