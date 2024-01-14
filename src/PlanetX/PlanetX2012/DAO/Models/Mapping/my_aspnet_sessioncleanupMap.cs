using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_sessioncleanupMap : EntityTypeConfiguration<my_aspnet_sessioncleanup>
    {
        public my_aspnet_sessioncleanupMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplicationId);

            // Properties
            this.Property(t => t.ApplicationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("my_aspnet_sessioncleanup", "PlanetX");
            this.Property(t => t.LastRun).HasColumnName("LastRun");
            this.Property(t => t.IntervalMinutes).HasColumnName("IntervalMinutes");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
        }
    }
}
