using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class StocksTransactionMap : EntityTypeConfiguration<StocksTransaction>
    {
        public StocksTransactionMap()
        {
            // Primary Key
            this.HasKey(t => t.StocksTransactionId);

            // Properties
            // Table & Column Mappings
            this.ToTable("StocksTransaction", "planetgeni");
            this.Property(t => t.StocksTransactionId).HasColumnName("StocksTransactionId");
            this.Property(t => t.OwnerId).HasColumnName("OwnerId");
            this.Property(t => t.TransactionType).HasColumnName("TransactionType");
            this.Property(t => t.StockId).HasColumnName("StockId");
            this.Property(t => t.NumberOfUnit).HasColumnName("NumberOfUnit");
            this.Property(t => t.TotalPrice).HasColumnName("TotalPrice");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}