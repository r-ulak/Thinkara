using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PostTagMap : EntityTypeConfiguration<PostTag>
    {
        public PostTagMap()
        {
            // Primary Key
            this.HasKey(t => t.PostTagId);

            // Properties
            // Table & Column Mappings
            this.ToTable("PostTag", "planetgeni");
            this.Property(t => t.PostTagId).HasColumnName("PostTagId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.TopicTagId).HasColumnName("TopicTagId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
