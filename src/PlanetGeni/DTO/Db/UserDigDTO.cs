using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserDigDTO
    {
        public sbyte OldDigType { get; set; }
        public sbyte DigType { get; set; }
        public Guid PostCommentId { get; set; }
        public int UserId { get; set; }
        public string PostCommentType{ get; set; }
    }
}
