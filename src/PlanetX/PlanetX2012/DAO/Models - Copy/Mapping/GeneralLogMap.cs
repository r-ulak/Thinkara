using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class GeneralLogMap : EntityTypeConfiguration<GeneralLog>
    {
        public GeneralLogMap()
        {
            // Primary Key
            this.HasKey(t => t.LogId);

            // Properties
            this.Property(t => t.LogText)
                .HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("GeneralLog", "PlanetX");
            this.Property(t => t.LogId).HasColumnName("LogId");
            this.Property(t => t.LogText).HasColumnName("LogText");
        }
    }
}
