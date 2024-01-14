using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class UserDig
    {
        public sbyte DigType { get; set; }
        public Guid PostCommentId { get; set; }
        public int UserId { get; set; }
    }
}
