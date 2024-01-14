using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class UniversityLocationMap : EntityTypeConfiguration<UniversityLocation>
    {
        public UniversityLocationMap()
        {
            // Primary Key
            this.HasKey(t => t.UniversityId);

            // Properties
            this.Property(t => t.UniversityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("UniversityLocation", "PlanetX");
            this.Property(t => t.UniversityId).HasColumnName("UniversityId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.ProvinceId).HasColumnName("ProvinceId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");

            // Relationships
            this.HasRequired(t => t.CityCode)
                .WithMany(t => t.UniversityLocations)
                .HasForeignKey(d => d.CityId);
            this.HasRequired(t => t.CountryCode)
                .WithMany(t => t.UniversityLocations)
                .HasForeignKey(d => d.CountryId);
            this.HasRequired(t => t.ProvinceCode)
                .WithMany(t => t.UniversityLocations)
                .HasForeignKey(d => d.ProvinceId);
            this.HasRequired(t => t.UniversityCode)
                .WithOptional(t => t.UniversityLocation);

        }
    }
}
