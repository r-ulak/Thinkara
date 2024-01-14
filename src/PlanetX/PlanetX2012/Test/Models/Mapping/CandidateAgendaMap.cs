using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class CandidateAgendaMap : EntityTypeConfiguration<CandidateAgenda>
    {
        public CandidateAgendaMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ElectionId, t.UserId });

            // Properties
            this.Property(t => t.ElectionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("CandidateAgenda", "planetgeni");
            this.Property(t => t.ElectionId).HasColumnName("ElectionId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AgendaTypeId).HasColumnName("AgendaTypeId");
        }
    }
}
