using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class IndustryMap : EntityTypeConfiguration<Industry>
    {
        public IndustryMap()
        {
            // Primary Key
            this.HasKey(t => t.IndustryId);

            // Properties
            this.Property(t => t.IndustryName)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("Industry", "planetgeni");
            this.Property(t => t.IndustryId).HasColumnName("IndustryId");
            this.Property(t => t.IndustryName).HasColumnName("IndustryName");
        }
    }
}
