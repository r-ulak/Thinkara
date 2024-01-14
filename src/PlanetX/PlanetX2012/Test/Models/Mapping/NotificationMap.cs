using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class NotificationMap : EntityTypeConfiguration<Notification>
    {
        public NotificationMap()
        {
            // Primary Key
            this.HasKey(t => t.NotificationId);

            // Properties
            this.Property(t => t.Msg)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Notification", "planetgeni");
            this.Property(t => t.NotificationId).HasColumnName("NotificationId");
            this.Property(t => t.Msg).HasColumnName("Msg");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Privacy).HasColumnName("Privacy");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
