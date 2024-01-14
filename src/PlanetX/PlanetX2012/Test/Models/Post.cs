using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Post
    {
        public int UserId { get; set; }
        public System.Guid PostId { get; set; }
        public string PostContent { get; set; }
        public int ChildCommentCount { get; set; }
        public sbyte CommentEnabled { get; set; }
        public int ThumbsUp { get; set; }
        public int ThumbsDown { get; set; }
        public string Slug { get; set; }
        public sbyte UserACL { get; set; }
        public sbyte ClubACL { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSpam { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedAt { get; set; }
    }
}
