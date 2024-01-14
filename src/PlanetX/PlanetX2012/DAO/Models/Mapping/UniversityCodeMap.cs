using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class UniversityCodeMap : EntityTypeConfiguration<UniversityCode>
    {
        public UniversityCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.UniversityId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("UniversityCode", "PlanetX");
            this.Property(t => t.UniversityId).HasColumnName("UniversityId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
