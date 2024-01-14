using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class StockMap : EntityTypeConfiguration<Stock>
    {
        public StockMap()
        {
            // Primary Key
            this.HasKey(t => t.StockId);

            // Properties
            this.Property(t => t.StockId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Stocks", "planetgeni");
            this.Property(t => t.StockId).HasColumnName("StockId");
            this.Property(t => t.CurrentValue).HasColumnName("CurrentValue");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
