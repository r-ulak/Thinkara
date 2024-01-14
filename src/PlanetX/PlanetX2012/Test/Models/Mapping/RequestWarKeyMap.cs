using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class RequestWarKeyMap : EntityTypeConfiguration<RequestWarKey>
    {
        public RequestWarKeyMap()
        {
            // Primary Key
            this.HasKey(t => t.RequestId);

            // Properties
            this.Property(t => t.RequestingCountryId)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.TaregtCountryId)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.ApprovalStatus)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.WarStatus)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.WiningCountryId)
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("RequestWarKey", "planetgeni");
            this.Property(t => t.RequestId).HasColumnName("RequestId");
            this.Property(t => t.RequestingCountryId).HasColumnName("RequestingCountryId");
            this.Property(t => t.TaregtCountryId).HasColumnName("TaregtCountryId");
            this.Property(t => t.RequestingUserId).HasColumnName("RequestingUserId");
            this.Property(t => t.RequestedAt).HasColumnName("RequestedAt");
            this.Property(t => t.ApprovalStatus).HasColumnName("ApprovalStatus");
            this.Property(t => t.WarStatus).HasColumnName("WarStatus");
            this.Property(t => t.WiningCountryId).HasColumnName("WiningCountryId");
        }
    }
}
