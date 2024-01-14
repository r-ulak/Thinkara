using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ElectionCandidateMap : EntityTypeConfiguration<ElectionCandidate>
    {
        public ElectionCandidateMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.CandidateTypeId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ElectionCandidate", "planetgeni");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CandidateTypeId).HasColumnName("CandidateTypeId");
        }
    }
}
