using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class PostCommentMap : EntityTypeConfiguration<PostComment>
    {
        public PostCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.PostCommentId);

            // Properties
            this.Property(t => t.Comment)
                .IsRequired()
                .HasMaxLength(65535);

            this.Property(t => t.Picture)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("PostComment", "PlanetX");
            this.Property(t => t.PostCommentId).HasColumnName("PostCommentId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.ParentCommentId).HasColumnName("ParentCommentId");
            this.Property(t => t.CommentDate).HasColumnName("CommentDate");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.IsSpam).HasColumnName("IsSpam");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.Post)
                .WithMany(t => t.PostComments)
                .HasForeignKey(d => d.PostId);

        }
    }
}
