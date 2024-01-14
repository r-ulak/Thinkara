using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class FeedSubCategory
    {
        public int FeedSubCategoryId { get; set; }
        public string Name { get; set; }
        public Nullable<int> FeedCategoryId { get; set; }
    }
}
