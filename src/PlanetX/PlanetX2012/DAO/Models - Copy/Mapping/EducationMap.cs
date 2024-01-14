using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class EducationMap : EntityTypeConfiguration<Education>
    {
        public EducationMap()
        {
            // Primary Key
            this.HasKey(t => t.EducationId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Education", "PlanetX");
            this.Property(t => t.EducationId).HasColumnName("EducationId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.DegreeType).HasColumnName("DegreeType");
            this.Property(t => t.MajorType).HasColumnName("MajorType");
            this.Property(t => t.UniversityId).HasColumnName("UniversityId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
