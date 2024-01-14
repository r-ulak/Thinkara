using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class StatusMap : EntityTypeConfiguration<Status>
    {
        public StatusMap()
        {
            // Primary Key
            this.HasKey(t => t.StatusId);

            // Properties
            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Status", "PlanetX");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.ThumbsUp).HasColumnName("ThumbsUp");
            this.Property(t => t.ThumbsDown).HasColumnName("ThumbsDown");
            this.Property(t => t.Privacy).HasColumnName("Privacy");
            this.Property(t => t.IsReply).HasColumnName("IsReply");
            this.Property(t => t.ToFb).HasColumnName("ToFb");
            this.Property(t => t.ToTwitter).HasColumnName("ToTwitter");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
