using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            // Primary Key
            this.HasKey(t => t.EventId);

            // Properties
            this.Property(t => t.EventName)
                .HasMaxLength(45);

            this.Property(t => t.Description)
                .HasMaxLength(65535);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Event", "planetgeni");
            this.Property(t => t.EventId).HasColumnName("EventId");
            this.Property(t => t.EventName).HasColumnName("EventName");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.EventType).HasColumnName("EventType");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
