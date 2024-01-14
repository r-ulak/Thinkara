using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
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
            this.ToTable("EventLocation", "planetgeni");
            this.Property(t => t.EventId).HasColumnName("EventId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.ProvinceId).HasColumnName("ProvinceId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
        }
    }
}
