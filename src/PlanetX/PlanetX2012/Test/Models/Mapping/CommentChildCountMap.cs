using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class CommentChildCountMap : EntityTypeConfiguration<CommentChildCount>
    {
        public CommentChildCountMap()
        {
            // Primary Key
            this.HasKey(t => t.cnt);

            // Properties
            this.Property(t => t.cnt)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("CommentChildCount", "planetgeni");
            this.Property(t => t.cnt).HasColumnName("cnt");
            this.Property(t => t.ParentCommentId).HasColumnName("ParentCommentId");
        }
    }
}
