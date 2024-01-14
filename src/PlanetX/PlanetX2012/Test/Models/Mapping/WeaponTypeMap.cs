using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class WeaponTypeMap : EntityTypeConfiguration<WeaponType>
    {
        public WeaponTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.WeaponTypeId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.ImageFont)
                .HasMaxLength(255);

            this.Property(t => t.WeaponTypeCode)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("WeaponType", "planetgeni");
            this.Property(t => t.WeaponTypeId).HasColumnName("WeaponTypeId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.ImageFont).HasColumnName("ImageFont");
            this.Property(t => t.Cost).HasColumnName("Cost");
            this.Property(t => t.WeaponTypeCode).HasColumnName("WeaponTypeCode");
            this.Property(t => t.OffenseScore).HasColumnName("OffenseScore");
            this.Property(t => t.DefenseScore).HasColumnName("DefenseScore");
        }
    }
}
