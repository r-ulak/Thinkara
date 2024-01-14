using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class MilitaryForceMap : EntityTypeConfiguration<MilitaryForce>
    {
        public MilitaryForceMap()
        {
            // Primary Key
            this.HasKey(t => t.CountryId);

            // Properties
            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("MilitaryForce", "PlanetX");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.Ground).HasColumnName("Ground");
            this.Property(t => t.Air).HasColumnName("Air");
            this.Property(t => t.Navy).HasColumnName("Navy");
            this.Property(t => t.Nuclear).HasColumnName("Nuclear");
            this.Property(t => t.Special).HasColumnName("Special");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.CountryCode)
                .WithOptional(t => t.MilitaryForce);

        }
    }
}
