using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class BookmarkSubCategory
    {
        public int BookmarkSubCategoryId { get; set; }
        public string Name { get; set; }
        public Nullable<int> BookmarkCategoryId { get; set; }
        public virtual BookmarkCategory BookmarkCategory { get; set; }
    }
}
