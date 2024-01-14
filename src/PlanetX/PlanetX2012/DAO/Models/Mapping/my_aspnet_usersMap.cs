using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_usersMap : EntityTypeConfiguration<my_aspnet_users>
    {
        public my_aspnet_usersMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("my_aspnet_users", "PlanetX");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.applicationId).HasColumnName("applicationId");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.isAnonymous).HasColumnName("isAnonymous");
            this.Property(t => t.lastActivityDate).HasColumnName("lastActivityDate");
        }
    }
}
