using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ElectionDonationMap : EntityTypeConfiguration<ElectionDonation>
    {
        public ElectionDonationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ElectionId, t.UserId, t.RequestedTo });

            // Properties
            this.Property(t => t.ElectionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RequestedTo)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ElectionDonation", "planetgeni");
            this.Property(t => t.ElectionId).HasColumnName("ElectionId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.RequestedTo).HasColumnName("RequestedTo");
            this.Property(t => t.AmountRecieved).HasColumnName("AmountRecieved");
            this.Property(t => t.DonationDenied).HasColumnName("DonationDenied");
        }
    }
}
