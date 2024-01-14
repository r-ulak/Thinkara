using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class EventMembershipMap : EntityTypeConfiguration<EventMembership>
    {
        public EventMembershipMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.EventId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.EventId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("EventMembership", "PlanetX");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.EventId).HasColumnName("EventId");
            this.Property(t => t.StatusType).HasColumnName("StatusType");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.Event)
                .WithMany(t => t.EventMemberships)
                .HasForeignKey(d => d.EventId);
            this.HasOptional(t => t.RsvpStatusCode)
                .WithMany(t => t.EventMemberships)
                .HasForeignKey(d => d.StatusType);

        }
    }
}