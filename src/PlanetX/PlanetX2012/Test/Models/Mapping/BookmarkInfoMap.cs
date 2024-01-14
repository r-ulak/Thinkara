using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class BookmarkInfoMap : EntityTypeConfiguration<BookmarkInfo>
    {
        public BookmarkInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.BookmarkInfoId);

            // Properties
            // Table & Column Mappings
            this.ToTable("BookmarkInfo", "planetgeni");
            this.Property(t => t.BookmarkInfoId).HasColumnName("BookmarkInfoId");
            this.Property(t => t.BookmarkId).HasColumnName("BookmarkId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Clicks).HasColumnName("Clicks");
            this.Property(t => t.Privacy).HasColumnName("Privacy");

            // Relationships
            this.HasOptional(t => t.Bookmark)
                .WithMany(t => t.BookmarkInfoes)
                .HasForeignKey(d => d.BookmarkId);

        }
    }
}
