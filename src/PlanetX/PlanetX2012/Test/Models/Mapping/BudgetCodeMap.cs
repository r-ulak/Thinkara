using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class BudgetCodeMap : EntityTypeConfiguration<BudgetCode>
    {
        public BudgetCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.BudgetType);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("BudgetCode", "planetgeni");
            this.Property(t => t.BudgetType).HasColumnName("BudgetType");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
