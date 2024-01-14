using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Post
    {
        public Post()
        {
            this.PostComments = new List<PostComment>();
            this.PostWebContents = new List<PostWebContent>();
        }

        public int UserId { get; set; }
        public int PostId { get; set; }
        public string PostContent { get; set; }
        public sbyte CommentEnabled { get; set; }
        public int Raters { get; set; }
        public float Rating { get; set; }
        public string Slug { get; set; }
        public sbyte UserACL { get; set; }
        public sbyte ClubACL { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostWebContent> PostWebContents { get; set; }
    }
}
