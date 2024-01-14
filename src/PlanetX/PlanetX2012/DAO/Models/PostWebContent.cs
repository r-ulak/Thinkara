using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class PostWebContent
    {
        public int PostWebContentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
        public virtual Post Post { get; set; }
    }
}
