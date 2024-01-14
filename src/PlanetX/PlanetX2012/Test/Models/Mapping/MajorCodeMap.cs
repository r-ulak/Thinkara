using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class MajorCodeMap : EntityTypeConfiguration<MajorCode>
    {
        public MajorCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.MajorId);

            // Properties
            this.Property(t => t.MajorName)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.ImageSrc)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("MajorCode", "planetgeni");
            this.Property(t => t.MajorId).HasColumnName("MajorId");
            this.Property(t => t.MajorName).HasColumnName("MajorName");
            this.Property(t => t.ImageSrc).HasColumnName("ImageSrc");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.DegreeRank).HasColumnName("DegreeRank");
        }
    }
}
