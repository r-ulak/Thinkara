using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PostCommentMap : EntityTypeConfiguration<PostComment>
    {
        public PostCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.PostCommentId);

            // Properties
            this.Property(t => t.CommentText)
                .IsRequired()
                .HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("PostComment", "planetgeni");
            this.Property(t => t.PostCommentId).HasColumnName("PostCommentId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.ParentCommentId).HasColumnName("ParentCommentId");
            this.Property(t => t.ThumbsUp).HasColumnName("ThumbsUp");
            this.Property(t => t.ThumbsDown).HasColumnName("ThumbsDown");
            this.Property(t => t.ChildCommentCount).HasColumnName("ChildCommentCount");
            this.Property(t => t.CommentText).HasColumnName("CommentText");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.IsSpam).HasColumnName("IsSpam");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
