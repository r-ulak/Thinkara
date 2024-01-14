using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_sessionsMap : EntityTypeConfiguration<my_aspnet_sessions>
    {
        public my_aspnet_sessionsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SessionId, t.ApplicationId });

            // Properties
            this.Property(t => t.SessionId)
                .IsRequired()
                .HasMaxLength(191);

            this.Property(t => t.ApplicationId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("my_aspnet_sessions", "PlanetX");
            this.Property(t => t.SessionId).HasColumnName("SessionId");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.Created).HasColumnName("Created");
            this.Property(t => t.Expires).HasColumnName("Expires");
            this.Property(t => t.LockDate).HasColumnName("LockDate");
            this.Property(t => t.LockId).HasColumnName("LockId");
            this.Property(t => t.Timeout).HasColumnName("Timeout");
            this.Property(t => t.Locked).HasColumnName("Locked");
            this.Property(t => t.SessionItems).HasColumnName("SessionItems");
            this.Property(t => t.Flags).HasColumnName("Flags");
        }
    }
}
