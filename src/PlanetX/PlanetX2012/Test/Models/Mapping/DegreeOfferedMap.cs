using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class DegreeOfferedMap : EntityTypeConfiguration<DegreeOffered>
    {
        public DegreeOfferedMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SchoolId, t.DegreeId, t.MajorId });

            // Properties
            this.Property(t => t.SchoolId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.MajorId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("DegreeOffered", "planetgeni");
            this.Property(t => t.SchoolId).HasColumnName("SchoolId");
            this.Property(t => t.DegreeId).HasColumnName("DegreeId");
            this.Property(t => t.MajorId).HasColumnName("MajorId");
        }
    }
}
