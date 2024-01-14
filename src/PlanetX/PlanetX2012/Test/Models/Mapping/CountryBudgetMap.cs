using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class CountryBudgetMap : EntityTypeConfiguration<CountryBudget>
    {
        public CountryBudgetMap()
        {
            // Primary Key
            this.HasKey(t => t.BudgetId);

            // Properties
            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("CountryBudget", "planetgeni");
            this.Property(t => t.BudgetId).HasColumnName("BudgetId");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}
