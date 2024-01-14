using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Status
    {
        public Status()
        {
            this.ThumbUpDowns = new List<ThumbUpDown>();
        }

        public int StatusId { get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<int> ThumbsUp { get; set; }
        public Nullable<int> ThumbsDown { get; set; }
        public sbyte Privacy { get; set; }
        public bool IsReply { get; set; }
        public bool ToFb { get; set; }
        public bool ToTwitter { get; set; }
        public int UserId { get; set; }
        public virtual ICollection<ThumbUpDown> ThumbUpDowns { get; set; }
    }
}
