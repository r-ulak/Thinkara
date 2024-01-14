using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class BookmarkInfo
    {
        public long BookmarkInfoId { get; set; }
        public Nullable<long> BookmarkId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<short> Clicks { get; set; }
        public sbyte Privacy { get; set; }
        public virtual Bookmark Bookmark { get; set; }
    }
}
