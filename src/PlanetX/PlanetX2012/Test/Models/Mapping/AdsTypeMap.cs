using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class AdsTypeMap : EntityTypeConfiguration<AdsType>
    {
        public AdsTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.AdsTypeId);

            // Properties
            this.Property(t => t.AdName)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("AdsType", "planetgeni");
            this.Property(t => t.AdsTypeId).HasColumnName("AdsTypeId");
            this.Property(t => t.AdName).HasColumnName("AdName");
            this.Property(t => t.Cost).HasColumnName("Cost");
        }
    }
}
