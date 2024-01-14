using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class BookmarkCategory
    {
        public BookmarkCategory()
        {
            this.BookmarkSubCategories = new List<BookmarkSubCategory>();
        }

        public short BookmarkCategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BookmarkSubCategory> BookmarkSubCategories { get; set; }
    }
}
