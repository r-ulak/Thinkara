using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class FeedMap : EntityTypeConfiguration<Feed>
    {
        public FeedMap()
        {
            // Primary Key
            this.HasKey(t => t.FeedId);

            // Properties
            this.Property(t => t.FeedUrl)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Feed", "PlanetX");
            this.Property(t => t.FeedId).HasColumnName("FeedId");
            this.Property(t => t.FeedUrl).HasColumnName("FeedUrl");
            this.Property(t => t.Rating).HasColumnName("Rating");
            this.Property(t => t.Privacy).HasColumnName("Privacy");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.FeedCategoryId).HasColumnName("FeedCategoryId");
            this.Property(t => t.FeedSubCategoryId).HasColumnName("FeedSubCategoryId");
        }
    }
}
