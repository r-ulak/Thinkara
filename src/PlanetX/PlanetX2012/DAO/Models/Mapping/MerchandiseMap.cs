using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class MerchandiseMap : EntityTypeConfiguration<Merchandise>
    {
        public MerchandiseMap()
        {
            // Primary Key
            this.HasKey(t => t.ItemId);

            // Properties
            this.Property(t => t.ItemId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Merchandise", "PlanetX");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.ItemType).HasColumnName("ItemType");
            this.Property(t => t.Cost).HasColumnName("Cost");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
