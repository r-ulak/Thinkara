using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PostWebContentMap : EntityTypeConfiguration<PostWebContent>
    {
        public PostWebContentMap()
        {
            // Primary Key
            this.HasKey(t => t.PostWebContentId);

            // Properties
            this.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(65535);

            this.Property(t => t.Title)
                .HasMaxLength(255);

            this.Property(t => t.Uri)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("PostWebContent", "planetgeni");
            this.Property(t => t.PostWebContentId).HasColumnName("PostWebContentId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Uri).HasColumnName("Uri");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
