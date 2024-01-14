using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class CountryWeaponMap : EntityTypeConfiguration<CountryWeapon>
    {
        public CountryWeaponMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryWeaponId);

            // Properties
            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("CountryWeapon", "planetgeni");
            this.Property(t => t.CountryWeaponId).HasColumnName("CountryWeaponId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.WeaponTypeId).HasColumnName("WeaponTypeId");
            this.Property(t => t.WeaponCondition).HasColumnName("WeaponCondition");
            this.Property(t => t.PurchasedPrice).HasColumnName("PurchasedPrice");
            this.Property(t => t.PurchasedAt).HasColumnName("PurchasedAt");
        }
    }
}
