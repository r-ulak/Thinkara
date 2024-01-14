using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class WebUserLocationMap : EntityTypeConfiguration<WebUserLocation>
    {
        public WebUserLocationMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("WebUserLocation", "planetgeni");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.ProvinceId).HasColumnName("ProvinceId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");

            // Relationships
            this.HasRequired(t => t.CityCode)
                .WithMany(t => t.WebUserLocations)
                .HasForeignKey(d => d.CityId);
            this.HasRequired(t => t.CountryCode)
                .WithMany(t => t.WebUserLocations)
                .HasForeignKey(d => d.CountryId);
            this.HasRequired(t => t.ProvinceCode)
                .WithMany(t => t.WebUserLocations)
                .HasForeignKey(d => d.ProvinceId);

        }
    }
}
