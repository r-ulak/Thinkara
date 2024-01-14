using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class ClubMap : EntityTypeConfiguration<Club>
    {
        public ClubMap()
        {
            // Primary Key
            this.HasKey(t => t.ClubId);

            // Properties
            this.Property(t => t.ClubName)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("Club", "PlanetX");
            this.Property(t => t.ClubId).HasColumnName("ClubId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ClubName).HasColumnName("ClubName");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
