using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserLoanMap : EntityTypeConfiguration<UserLoan>
    {
        public UserLoanMap()
        {
            // Primary Key
            this.HasKey(t => t.LoanId);

            // Properties
            this.Property(t => t.LoanRequestStatus)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UserLoan", "planetgeni");
            this.Property(t => t.LoanId).HasColumnName("LoanId");
            this.Property(t => t.TaskId).HasColumnName("TaskId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.LendorId).HasColumnName("LendorId");
            this.Property(t => t.LoanAmount).HasColumnName("LoanAmount");
            this.Property(t => t.LeftAmount).HasColumnName("LeftAmount");
            this.Property(t => t.PaidAmount).HasColumnName("PaidAmount");
            this.Property(t => t.MonthlyInterestRate).HasColumnName("MonthlyInterestRate");
            this.Property(t => t.LoanRequestStatus).HasColumnName("LoanRequestStatus");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
