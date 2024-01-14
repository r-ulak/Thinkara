using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PrivacyTypeMap : EntityTypeConfiguration<PrivacyType>
    {
        public PrivacyTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.PrivacyTypeId);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("PrivacyType", "planetgeni");
            this.Property(t => t.PrivacyTypeId).HasColumnName("PrivacyTypeId");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
