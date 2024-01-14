using System;


namespace DTO.Db
{
    public class PostCommentDTO
    {
        public PostDTO[] Posts { get; set; }
        public CommentDTO[] PostComments { get; set; }
    }
}
