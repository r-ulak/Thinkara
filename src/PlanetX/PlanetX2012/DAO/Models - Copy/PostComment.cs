using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PostComment
    {
        public int PostCommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public int ParentCommentId { get; set; }
        public System.DateTime CommentDate { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public string Picture { get; set; }
        public bool IsSpam { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual Post Post { get; set; }
    }
}
