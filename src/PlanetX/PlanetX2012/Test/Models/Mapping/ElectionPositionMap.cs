using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ElectionPositionMap : EntityTypeConfiguration<ElectionPosition>
    {
        public ElectionPositionMap()
        {
            // Primary Key
            this.HasKey(t => t.PositionTypeId);

            // Properties
            this.Property(t => t.ElectionPositionName)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("ElectionPosition", "planetgeni");
            this.Property(t => t.PositionTypeId).HasColumnName("PositionTypeId");
            this.Property(t => t.ElectionPositionName).HasColumnName("ElectionPositionName");
        }
    }
}
