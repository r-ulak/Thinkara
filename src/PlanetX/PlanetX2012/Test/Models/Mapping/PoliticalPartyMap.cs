using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PoliticalPartyMap : EntityTypeConfiguration<PoliticalParty>
    {
        public PoliticalPartyMap()
        {
            // Primary Key
            this.HasKey(t => t.PartyId);

            // Properties
            this.Property(t => t.PartyName)
                .IsRequired()
                .HasMaxLength(25);

            this.Property(t => t.LogoPictureId)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("PoliticalParty", "planetgeni");
            this.Property(t => t.PartyId).HasColumnName("PartyId");
            this.Property(t => t.PartyName).HasColumnName("PartyName");
            this.Property(t => t.PartyFounder).HasColumnName("PartyFounder");
            this.Property(t => t.LogoPictureId).HasColumnName("LogoPictureId");
            this.Property(t => t.Active).HasColumnName("Active");
        }
    }
}
