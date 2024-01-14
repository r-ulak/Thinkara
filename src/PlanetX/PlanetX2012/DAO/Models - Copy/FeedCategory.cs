using System;
using System.Collections.Generic;

namespace DAO.Models
{
    public partial class FeedCategory
    {
        public FeedCategory()
        {
            this.FeedSubCategories = new List<FeedSubCategory>();
        }

        public short FeedCategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<FeedSubCategory> FeedSubCategories { get; set; }
    }
}
