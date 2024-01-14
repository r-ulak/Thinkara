using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class CountryBudgetByTypeMap : EntityTypeConfiguration<CountryBudgetByType>
    {
        public CountryBudgetByTypeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BudgetId, t.BudgetType });

            // Properties
            this.Property(t => t.BudgetId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("CountryBudgetByType", "planetgeni");
            this.Property(t => t.BudgetId).HasColumnName("BudgetId");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.AmountLeft).HasColumnName("AmountLeft");
            this.Property(t => t.BudgetType).HasColumnName("BudgetType");
        }
    }
}
