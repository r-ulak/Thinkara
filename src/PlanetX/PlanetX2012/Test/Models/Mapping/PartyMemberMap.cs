using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PartyMemberMap : EntityTypeConfiguration<PartyMember>
    {
        public PartyMemberMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PartyId, t.UserId });

            // Properties
            this.Property(t => t.PartyId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PartyMember", "planetgeni");
            this.Property(t => t.PartyId).HasColumnName("PartyId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.PartyMemberTypeId).HasColumnName("PartyMemberTypeId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
