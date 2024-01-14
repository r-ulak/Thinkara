using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class BookmarkCategory
    {
        public BookmarkCategory()
        {
            this.BookmarkSubCategories = new List<BookmarkSubCategory>();
        }

        public int BookmarkCategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BookmarkSubCategory> BookmarkSubCategories { get; set; }
    }
}
