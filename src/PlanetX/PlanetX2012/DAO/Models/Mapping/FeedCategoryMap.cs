using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class FeedCategoryMap : EntityTypeConfiguration<FeedCategory>
    {
        public FeedCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.FeedCategoryId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("FeedCategory", "PlanetX");
            this.Property(t => t.FeedCategoryId).HasColumnName("FeedCategoryId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
