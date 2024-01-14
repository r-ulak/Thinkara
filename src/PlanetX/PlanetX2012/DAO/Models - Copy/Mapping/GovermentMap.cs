using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class GovermentMap : EntityTypeConfiguration<Goverment>
    {
        public GovermentMap()
        {
            // Primary Key
            this.HasKey(t => new { t.CountryId, t.LeaderId, t.leaderType });

            // Properties
            this.Property(t => t.CountryId)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.LeaderId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Goverment", "PlanetX");
            this.Property(t => t.CountryId).HasColumnName("CountryId");
            this.Property(t => t.LeaderId).HasColumnName("LeaderId");
            this.Property(t => t.leaderType).HasColumnName("leaderType");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
