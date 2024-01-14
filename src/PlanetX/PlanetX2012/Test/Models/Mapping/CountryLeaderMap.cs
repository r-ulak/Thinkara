using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class CountryLeaderMap : EntityTypeConfiguration<CountryLeader>
    {
        public CountryLeaderMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.EndDate });

            // Properties
            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("CountryLeader", "planetgeni");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CandidateTypeId).HasColumnName("CandidateTypeId");
            this.Property(t => t.PositionTypeId).HasColumnName("PositionTypeId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
