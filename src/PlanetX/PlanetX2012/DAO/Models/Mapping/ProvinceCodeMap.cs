using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class ProvinceCodeMap : EntityTypeConfiguration<ProvinceCode>
    {
        public ProvinceCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.ProvinceId);

            // Properties
            this.Property(t => t.Province)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("ProvinceCode", "PlanetX");
            this.Property(t => t.ProvinceId).HasColumnName("ProvinceId");
            this.Property(t => t.Province).HasColumnName("Province");
        }
    }
}
