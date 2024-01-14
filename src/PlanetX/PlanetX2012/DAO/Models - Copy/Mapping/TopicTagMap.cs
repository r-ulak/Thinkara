using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class TopicTagMap : EntityTypeConfiguration<TopicTag>
    {
        public TopicTagMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TopicTagId, t.Tag });

            // Properties
            this.Property(t => t.TopicTagId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Tag)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TopicTag", "PlanetX");
            this.Property(t => t.TopicTagId).HasColumnName("TopicTagId");
            this.Property(t => t.Tag).HasColumnName("Tag");
            this.Property(t => t.TagCount).HasColumnName("TagCount");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
