using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class CityCodeMap : EntityTypeConfiguration<CityCode>
    {
        public CityCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.CityId);

            // Properties
            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("CityCode", "PlanetX");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
        }
    }
}
