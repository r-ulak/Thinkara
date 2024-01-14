using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_profilesMap : EntityTypeConfiguration<my_aspnet_profiles>
    {
        public my_aspnet_profilesMap()
        {
            // Primary Key
            this.HasKey(t => t.userId);

            // Properties
            this.Property(t => t.userId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.valueindex)
                .HasMaxLength(1073741823);

            this.Property(t => t.stringdata)
                .HasMaxLength(1073741823);

            // Table & Column Mappings
            this.ToTable("my_aspnet_profiles", "PlanetX");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.valueindex).HasColumnName("valueindex");
            this.Property(t => t.stringdata).HasColumnName("stringdata");
            this.Property(t => t.binarydata).HasColumnName("binarydata");
            this.Property(t => t.lastUpdatedDate).HasColumnName("lastUpdatedDate");
        }
    }
}
