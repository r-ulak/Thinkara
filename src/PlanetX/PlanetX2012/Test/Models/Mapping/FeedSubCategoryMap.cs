using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class FeedSubCategoryMap : EntityTypeConfiguration<FeedSubCategory>
    {
        public FeedSubCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.FeedSubCategoryId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("FeedSubCategory", "planetgeni");
            this.Property(t => t.FeedSubCategoryId).HasColumnName("FeedSubCategoryId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.FeedCategoryId).HasColumnName("FeedCategoryId");
        }
    }
}
