using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class BusinessSubCodeMap : EntityTypeConfiguration<BusinessSubCode>
    {
        public BusinessSubCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.BusinessSubtypeId);

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
            this.ToTable("BusinessSubCode", "PlanetX");
            this.Property(t => t.BusinessSubtypeId).HasColumnName("BusinessSubtypeId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.BusinessTypeId).HasColumnName("BusinessTypeId");

            // Relationships
            this.HasRequired(t => t.BusinessCode)
                .WithMany(t => t.BusinessSubCodes)
                .HasForeignKey(d => d.BusinessTypeId);

        }
    }
}
