using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class Blog
    {
        public int BlogId { get; set; }
        public string Message { get; set; }
        public string Author { get; set; }
        public Nullable<int> UserId { get; set; }
        public string CreatedAt { get; set; }
    }
}
