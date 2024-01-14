using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class LeaderCodeMap : EntityTypeConfiguration<LeaderCode>
    {
        public LeaderCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.LeaderType);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("LeaderCode", "PlanetX");
            this.Property(t => t.LeaderType).HasColumnName("LeaderType");
            this.Property(t => t.Code).HasColumnName("Code");
        }
    }
}
