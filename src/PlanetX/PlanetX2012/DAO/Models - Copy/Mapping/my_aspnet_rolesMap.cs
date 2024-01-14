using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_rolesMap : EntityTypeConfiguration<my_aspnet_roles>
    {
        public my_aspnet_rolesMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("my_aspnet_roles", "PlanetX");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.applicationId).HasColumnName("applicationId");
            this.Property(t => t.name).HasColumnName("name");
        }
    }
}
