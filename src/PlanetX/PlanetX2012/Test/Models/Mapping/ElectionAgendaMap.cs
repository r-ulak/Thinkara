using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ElectionAgendaMap : EntityTypeConfiguration<ElectionAgenda>
    {
        public ElectionAgendaMap()
        {
            // Primary Key
            this.HasKey(t => t.AgendaTypeId);

            // Properties
            this.Property(t => t.AgendaTypeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AgendaName)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("ElectionAgenda", "planetgeni");
            this.Property(t => t.AgendaTypeId).HasColumnName("AgendaTypeId");
            this.Property(t => t.AgendaName).HasColumnName("AgendaName");
        }
    }
}
