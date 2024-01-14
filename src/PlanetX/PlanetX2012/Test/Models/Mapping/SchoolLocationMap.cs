using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class SchoolLocationMap : EntityTypeConfiguration<SchoolLocation>
    {
        public SchoolLocationMap()
        {
            // Primary Key
            this.HasKey(t => t.SchoolId);

            // Properties
            this.Property(t => t.SchoolId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("SchoolLocation", "planetgeni");
            this.Property(t => t.SchoolId).HasColumnName("SchoolId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.ProvinceId).HasColumnName("ProvinceId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
        }
    }
}
