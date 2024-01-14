using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class BookmarkMap : EntityTypeConfiguration<Bookmark>
    {
        public BookmarkMap()
        {
            // Primary Key
            this.HasKey(t => t.BookmarkId);

            // Properties
            this.Property(t => t.Url)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Bookmark", "PlanetX");
            this.Property(t => t.BookmarkId).HasColumnName("BookmarkId");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Rating).HasColumnName("Rating");
            this.Property(t => t.Privacy).HasColumnName("Privacy");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.BookmarkCategoryId).HasColumnName("BookmarkCategoryId");
            this.Property(t => t.BookmarkSubCategoryId).HasColumnName("BookmarkSubCategoryId");
        }
    }
}
