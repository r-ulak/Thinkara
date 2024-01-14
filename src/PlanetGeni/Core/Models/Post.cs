using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAO.Models
{
    public partial class Post
    {
        public int? UserId { get; set; }
        public string CountryId { get; set; }
        public Nullable<Guid> PartyId { get; set; }
        [Required]
        public System.Guid PostId { get; set; }
        [Required]
        public string PostContent { get; set; }
        public string PostTitle { get; set; }
        public Nullable<int> ChildCommentCount { get; set; }
        public bool CommentEnabled { get; set; }
        public uint DigIt { get; set; }
        public uint CoolIt { get; set; }
        public string ImageName { get; set; }
        public bool IsApproved { get; set; }
        public bool IsSpam { get; set; }
        public bool IsDeleted { get; set; }
        public string Parms { get; set; }
        public sbyte PostContentTypeId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
