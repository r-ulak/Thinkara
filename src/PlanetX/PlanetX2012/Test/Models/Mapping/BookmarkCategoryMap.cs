using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class BookmarkCategoryMap : EntityTypeConfiguration<BookmarkCategory>
    {
        public BookmarkCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.BookmarkCategoryId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("BookmarkCategory", "planetgeni");
            this.Property(t => t.BookmarkCategoryId).HasColumnName("BookmarkCategoryId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
