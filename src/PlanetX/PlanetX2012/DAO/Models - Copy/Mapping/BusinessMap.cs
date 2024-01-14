using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class BusinessMap : EntityTypeConfiguration<Business>
    {
        public BusinessMap()
        {
            // Primary Key
            this.HasKey(t => t.BusinessId);

            // Properties
            this.Property(t => t.BusinessName)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("Business", "PlanetX");
            this.Property(t => t.BusinessId).HasColumnName("BusinessId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.BusinessName).HasColumnName("BusinessName");
            this.Property(t => t.BusinessTypeId).HasColumnName("BusinessTypeId");
            this.Property(t => t.BusinessSubtypeId).HasColumnName("BusinessSubtypeId");
            this.Property(t => t.CityId).HasColumnName("CityId");
            this.Property(t => t.NetProfit).HasColumnName("NetProfit");
            this.Property(t => t.RunningCost).HasColumnName("RunningCost");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
