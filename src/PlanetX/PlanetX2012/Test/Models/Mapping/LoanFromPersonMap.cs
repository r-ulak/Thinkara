using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class LoanFromPersonMap : EntityTypeConfiguration<LoanFromPerson>
    {
        public LoanFromPersonMap()
        {
            // Primary Key
            this.HasKey(t => t.LoanId);

            // Properties
            // Table & Column Mappings
            this.ToTable("LoanFromPerson", "planetgeni");
            this.Property(t => t.LoanId).HasColumnName("LoanId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.SourceId).HasColumnName("SourceId");
            this.Property(t => t.LoanType).HasColumnName("LoanType");
            this.Property(t => t.LoanAmount).HasColumnName("LoanAmount");
            this.Property(t => t.MonthlyInterestRate).HasColumnName("MonthlyInterestRate");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.LoanCode)
                .WithMany(t => t.LoanFromPersons)
                .HasForeignKey(d => d.LoanType);

        }
    }
}
