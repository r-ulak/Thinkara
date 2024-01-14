using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class AdsTypeListMap : EntityTypeConfiguration<AdsTypeList>
    {
        public AdsTypeListMap()
        {
            // Primary Key
            this.HasKey(t => t.AdsTypeListId);

            // Properties
            this.Property(t => t.AdName)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("AdsTypeList", "planetgeni");
            this.Property(t => t.AdsTypeListId).HasColumnName("AdsTypeListId");
            this.Property(t => t.AdsTypeId).HasColumnName("AdsTypeId");
            this.Property(t => t.AdName).HasColumnName("AdName");
            this.Property(t => t.Cost).HasColumnName("Cost");
        }
    }
}
