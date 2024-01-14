using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class StockCodeMap : EntityTypeConfiguration<StockCode>
    {
        public StockCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.StockId);

            // Properties
            this.Property(t => t.StockName)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("StockCode", "PlanetX");
            this.Property(t => t.StockId).HasColumnName("StockId");
            this.Property(t => t.StockName).HasColumnName("StockName");
            this.Property(t => t.BusinessId).HasColumnName("BusinessId");
            this.Property(t => t.OwnerId).HasColumnName("OwnerId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
