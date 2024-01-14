using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class BookmarkSubCategoryMap : EntityTypeConfiguration<BookmarkSubCategory>
    {
        public BookmarkSubCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.BookmarkSubCategoryId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("BookmarkSubCategory", "planetgeni");
            this.Property(t => t.BookmarkSubCategoryId).HasColumnName("BookmarkSubCategoryId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.BookmarkCategoryId).HasColumnName("BookmarkCategoryId");

            // Relationships
            this.HasOptional(t => t.BookmarkCategory)
                .WithMany(t => t.BookmarkSubCategories)
                .HasForeignKey(d => d.BookmarkCategoryId);

        }
    }
}
