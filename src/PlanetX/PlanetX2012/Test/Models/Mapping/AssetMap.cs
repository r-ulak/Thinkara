using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class AssetMap : EntityTypeConfiguration<Asset>
    {
        public AssetMap()
        {
            // Primary Key
            this.HasKey(t => t.ItemId);

            // Properties
            this.Property(t => t.ItemId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Asset", "planetgeni");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Qty).HasColumnName("Qty");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.Merchandise)
                .WithOptional(t => t.Asset);

        }
    }
}
