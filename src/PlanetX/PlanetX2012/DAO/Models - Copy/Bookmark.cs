using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class Bookmark
    {
        public Bookmark()
        {
            this.BookmarkInfoes = new List<BookmarkInfo>();
        }

        public long BookmarkId { get; set; }
        public string Url { get; set; }
        public Nullable<short> Rating { get; set; }
        public sbyte Privacy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public short BookmarkCategoryId { get; set; }
        public Nullable<short> BookmarkSubCategoryId { get; set; }
        public virtual ICollection<BookmarkInfo> BookmarkInfoes { get; set; }
    }
}
