using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class FeedInfoMap : EntityTypeConfiguration<FeedInfo>
    {
        public FeedInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.FeedInfoId);

            // Properties
            // Table & Column Mappings
            this.ToTable("FeedInfo", "PlanetX");
            this.Property(t => t.FeedInfoId).HasColumnName("FeedInfoId");
            this.Property(t => t.FeedId).HasColumnName("FeedId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Favorite).HasColumnName("Favorite");
            this.Property(t => t.Clicks).HasColumnName("Clicks");
            this.Property(t => t.Privacy).HasColumnName("Privacy");

            // Relationships
            this.HasOptional(t => t.Feed)
                .WithMany(t => t.FeedInfoes)
                .HasForeignKey(d => d.FeedId);

        }
    }
}
