using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class UserConnectionMap : EntityTypeConfiguration<UserConnection>
    {
        public UserConnectionMap()
        {
            // Primary Key
            this.HasKey(t => t.ConnectionId);

            // Properties
            this.Property(t => t.ConnectionId)
                .IsRequired()
                .HasMaxLength(36);

            this.Property(t => t.UserAgent)
                .IsRequired()
                .HasMaxLength(36);

            // Table & Column Mappings
            this.ToTable("UserConnection", "PlanetX");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ConnectionId).HasColumnName("ConnectionId");
            this.Property(t => t.UserAgent).HasColumnName("UserAgent");
        }
    }
}
