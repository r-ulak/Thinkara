using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_usersinrolesMap : EntityTypeConfiguration<my_aspnet_usersinroles>
    {
        public my_aspnet_usersinrolesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.userId, t.roleId });

            // Properties
            this.Property(t => t.userId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.roleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("my_aspnet_usersinroles", "PlanetX");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.roleId).HasColumnName("roleId");
        }
    }
}
