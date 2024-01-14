using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class MajorCodeMap : EntityTypeConfiguration<MajorCode>
    {
        public MajorCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.MajorType);

            // Properties
            this.Property(t => t.Major)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("MajorCode", "PlanetX");
            this.Property(t => t.MajorType).HasColumnName("MajorType");
            this.Property(t => t.Major).HasColumnName("Major");
        }
    }
}
