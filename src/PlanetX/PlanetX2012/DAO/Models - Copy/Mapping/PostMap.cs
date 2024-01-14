using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            // Primary Key
            this.HasKey(t => t.PostId);

            // Properties
            this.Property(t => t.PostContent)
                .IsRequired()
                .HasMaxLength(1073741823);

            this.Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Post", "PlanetX");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.PostContent).HasColumnName("PostContent");
            this.Property(t => t.CommentEnabled).HasColumnName("CommentEnabled");
            this.Property(t => t.Raters).HasColumnName("Raters");
            this.Property(t => t.Rating).HasColumnName("Rating");
            this.Property(t => t.Slug).HasColumnName("Slug");
            this.Property(t => t.UserACL).HasColumnName("UserACL");
            this.Property(t => t.ClubACL).HasColumnName("ClubACL");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
