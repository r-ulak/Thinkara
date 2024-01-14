using System;
using System.Collections.Generic;

namespace Test.Models
{
    public partial class FeedSubCategory
    {
        public short FeedSubCategoryId { get; set; }
        public string Name { get; set; }
        public Nullable<short> FeedCategoryId { get; set; }
    }
}
