using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserBankAccountMap : EntityTypeConfiguration<UserBankAccount>
    {
        public UserBankAccountMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserBankAccount", "planetgeni");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Cash).HasColumnName("Cash");
            this.Property(t => t.Gold).HasColumnName("Gold");
            this.Property(t => t.Silver).HasColumnName("Silver");
            this.Property(t => t.Stocks).HasColumnName("Stocks");
            this.Property(t => t.Loan).HasColumnName("Loan");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
