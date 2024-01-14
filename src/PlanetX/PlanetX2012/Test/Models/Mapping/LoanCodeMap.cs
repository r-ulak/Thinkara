using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class LoanCodeMap : EntityTypeConfiguration<LoanCode>
    {
        public LoanCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.LoanType);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("LoanCode", "planetgeni");
            this.Property(t => t.LoanType).HasColumnName("LoanType");
            this.Property(t => t.Code).HasColumnName("Code");
        }
    }
}
