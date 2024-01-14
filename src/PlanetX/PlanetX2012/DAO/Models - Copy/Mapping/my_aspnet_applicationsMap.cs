using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_applicationsMap : EntityTypeConfiguration<my_aspnet_applications>
    {
        public my_aspnet_applicationsMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(256);

            this.Property(t => t.description)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("my_aspnet_applications", "PlanetX");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.description).HasColumnName("description");
        }
    }
}
