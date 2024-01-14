using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class EmploymentMap : EntityTypeConfiguration<Employment>
    {
        public EmploymentMap()
        {
            // Primary Key
            this.HasKey(t => new { t.BusinessId, t.UserId });

            // Properties
            this.Property(t => t.BusinessId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.JobTitle)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("Employment", "PlanetX");
            this.Property(t => t.BusinessId).HasColumnName("BusinessId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.JobTitle).HasColumnName("JobTitle");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.Business)
                .WithMany(t => t.Employments)
                .HasForeignKey(d => d.BusinessId);

        }
    }
}
