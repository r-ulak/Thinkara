using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class SchoolCodeMap : EntityTypeConfiguration<SchoolCode>
    {
        public SchoolCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.SchoolId);

            // Properties
            this.Property(t => t.SchoolName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ImageSrc)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("SchoolCode", "planetgeni");
            this.Property(t => t.SchoolId).HasColumnName("SchoolId");
            this.Property(t => t.SchoolName).HasColumnName("SchoolName");
            this.Property(t => t.ImageSrc).HasColumnName("ImageSrc");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.CostPerCredit).HasColumnName("CostPerCredit");
        }
    }
}
