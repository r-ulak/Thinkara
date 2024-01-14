using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class AdsFrequencyTypeMap : EntityTypeConfiguration<AdsFrequencyType>
    {
        public AdsFrequencyTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.AdsFrequencyTypeId);

            // Properties
            this.Property(t => t.FrequencyName)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("AdsFrequencyType", "planetgeni");
            this.Property(t => t.AdsFrequencyTypeId).HasColumnName("AdsFrequencyTypeId");
            this.Property(t => t.FrequencyName).HasColumnName("FrequencyName");
        }
    }
}
