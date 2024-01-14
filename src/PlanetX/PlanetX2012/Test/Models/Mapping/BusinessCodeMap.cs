using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class BusinessCodeMap : EntityTypeConfiguration<BusinessCode>
    {
        public BusinessCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.BusinessTypeId);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("BusinessCode", "planetgeni");
            this.Property(t => t.BusinessTypeId).HasColumnName("BusinessTypeId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
