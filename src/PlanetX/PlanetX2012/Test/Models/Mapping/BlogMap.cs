using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class BlogMap : EntityTypeConfiguration<Blog>
    {
        public BlogMap()
        {
            // Primary Key
            this.HasKey(t => t.BlogId);

            // Properties
            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.Author)
                .HasMaxLength(45);

            this.Property(t => t.CreatedAt)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("Blog", "planetgeni");
            this.Property(t => t.BlogId).HasColumnName("BlogId");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
