using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class BusinessLocationMap : EntityTypeConfiguration<BusinessLocation>
    {
        public BusinessLocationMap()
        {
            // Primary Key
            this.HasKey(t => t.BusinessId);

            // Properties
            this.Property(t => t.BusinessId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("BusinessLocation", "PlanetX");
            this.Property(t => t.BusinessId).HasColumnName("BusinessId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.ProvinceId).HasColumnName("ProvinceId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");

            // Relationships
            this.HasRequired(t => t.Business)
                .WithOptional(t => t.BusinessLocation);
            this.HasRequired(t => t.CityCode)
                .WithMany(t => t.BusinessLocations)
                .HasForeignKey(d => d.CityId);
            this.HasRequired(t => t.CountryCode)
                .WithMany(t => t.BusinessLocations)
                .HasForeignKey(d => d.CountryId);
            this.HasRequired(t => t.ProvinceCode)
                .WithMany(t => t.BusinessLocations)
                .HasForeignKey(d => d.ProvinceId);

        }
    }
}
