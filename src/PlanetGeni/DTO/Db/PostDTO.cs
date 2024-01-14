using System;

namespace DTO.Db
{
    public class PostDTO
    {
        public int UserId { get; set; }
        public string CountryId { get; set; }
        public Nullable<Guid> PartyId { get; set; }
        public System.Guid PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public Nullable<int> ChildCommentCount { get; set; }
        public uint DigIt { get; set; }
        public uint CoolIt { get; set; }
        public sbyte DigType { get; set; }
        public bool CommentEnabled { get; set; }
        public string ImageName { get; set; }
        public string Parms { get; set; }
        public int PostContentTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
