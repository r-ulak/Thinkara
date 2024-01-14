using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class LoanFromBusinessMap : EntityTypeConfiguration<LoanFromBusiness>
    {
        public LoanFromBusinessMap()
        {
            // Primary Key
            this.HasKey(t => t.LoanId);

            // Properties
            // Table & Column Mappings
            this.ToTable("LoanFromBusiness", "PlanetX");
            this.Property(t => t.LoanId).HasColumnName("LoanId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.BusinessId).HasColumnName("BusinessId");
            this.Property(t => t.LoanType).HasColumnName("LoanType");
            this.Property(t => t.LoanAmount).HasColumnName("LoanAmount");
            this.Property(t => t.MonthlyInterestRate).HasColumnName("MonthlyInterestRate");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.Business)
                .WithMany(t => t.LoanFromBusinesses)
                .HasForeignKey(d => d.BusinessId);
            this.HasRequired(t => t.LoanCode)
                .WithMany(t => t.LoanFromBusinesses)
                .HasForeignKey(d => d.LoanType);

        }
    }
}
