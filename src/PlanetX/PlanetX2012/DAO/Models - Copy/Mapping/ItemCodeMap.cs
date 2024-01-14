using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class ItemCodeMap : EntityTypeConfiguration<ItemCode>
    {
        public ItemCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.ItemType);

            // Properties
            this.Property(t => t.Item)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("ItemCode", "PlanetX");
            this.Property(t => t.ItemType).HasColumnName("ItemType");
            this.Property(t => t.Item).HasColumnName("Item");
        }
    }
}