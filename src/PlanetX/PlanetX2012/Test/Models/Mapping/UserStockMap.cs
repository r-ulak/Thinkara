using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserStockMap : EntityTypeConfiguration<UserStock>
    {
        public UserStockMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.StockId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.StockId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("UserStocks", "planetgeni");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.StockId).HasColumnName("StockId");
            this.Property(t => t.PurchasedUnit).HasColumnName("PurchasedUnit");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
