using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class DegreeCodeMap : EntityTypeConfiguration<DegreeCode>
    {
        public DegreeCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.DegreeId);

            // Properties
            this.Property(t => t.DegreeName)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.ImageSrc)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("DegreeCode", "planetgeni");
            this.Property(t => t.DegreeId).HasColumnName("DegreeId");
            this.Property(t => t.DegreeName).HasColumnName("DegreeName");
            this.Property(t => t.ImageSrc).HasColumnName("ImageSrc");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
