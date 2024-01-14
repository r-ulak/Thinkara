using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class DegreeCodeMap : EntityTypeConfiguration<DegreeCode>
    {
        public DegreeCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.DegreeType);

            // Properties
            this.Property(t => t.Degree)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("DegreeCode", "PlanetX");
            this.Property(t => t.DegreeType).HasColumnName("DegreeType");
            this.Property(t => t.Degree).HasColumnName("Degree");
        }
    }
}
