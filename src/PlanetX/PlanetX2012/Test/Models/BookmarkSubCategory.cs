using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class BookmarkSubCategory
    {
        public short BookmarkSubCategoryId { get; set; }
        public string Name { get; set; }
        public Nullable<short> BookmarkCategoryId { get; set; }
        public virtual BookmarkCategory BookmarkCategory { get; set; }
    }
}
