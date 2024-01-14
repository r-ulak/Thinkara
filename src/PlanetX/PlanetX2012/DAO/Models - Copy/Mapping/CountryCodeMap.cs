using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class CountryCodeMap : EntityTypeConfiguration<CountryCode>
    {
        public CountryCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryId);

            // Properties
            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CountryCode", "PlanetX");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.Code).HasColumnName("Code");
        }
    }
}
