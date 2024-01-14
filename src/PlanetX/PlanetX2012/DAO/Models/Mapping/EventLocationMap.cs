using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class EventLocationMap : EntityTypeConfiguration<EventLocation>
    {
        public EventLocationMap()
        {
            // Primary Key
            this.HasKey(t => t.EventId);

            // Properties
            this.Property(t => t.EventId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("EventLocation", "PlanetX");
            this.Property(t => t.EventId).HasColumnName("EventId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.ProvinceId).HasColumnName("ProvinceId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");

            // Relationships
            this.HasRequired(t => t.CityCode)
                .WithMany(t => t.EventLocations)
                .HasForeignKey(d => d.CityId);
            this.HasRequired(t => t.CountryCode)
                .WithMany(t => t.EventLocations)
                .HasForeignKey(d => d.CountryId);
            this.HasRequired(t => t.Event)
                .WithOptional(t => t.EventLocation);
            this.HasRequired(t => t.ProvinceCode)
                .WithMany(t => t.EventLocations)
                .HasForeignKey(d => d.ProvinceId);

        }
    }
}
