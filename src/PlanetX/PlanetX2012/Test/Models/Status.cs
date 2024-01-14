using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Status
    {
        public int StatusId { get; set; }
        public string Message { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<short> ThumbsUp { get; set; }
        public Nullable<short> ThumbsDown { get; set; }
        public sbyte Privacy { get; set; }
        public bool IsReply { get; set; }
        public bool ToFb { get; set; }
        public bool ToTwitter { get; set; }
        public int UserId { get; set; }
    }
}
