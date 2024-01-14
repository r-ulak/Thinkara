using System;
using System.ComponentModel.DataAnnotations;


namespace DTO.Db
{
    public class CommentDTO
    {
        public Guid PostCommentId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Picture { get; set; }
        [Required(ErrorMessage = "PostId is required")]
        public Guid PostId { get; set; }
        public Nullable<Guid> ParentCommentId { get; set; }
        public int ChildCommentCount { get; set; }
        public uint DigIt { get; set; }
        public uint CoolIt { get; set; }
        public sbyte DigType { get; set; }
        [Required(ErrorMessage = "CommentText is required")]
        public string CommentText { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
