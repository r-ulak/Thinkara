using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class EmploymentMap : EntityTypeConfiguration<Employment>
    {
        public EmploymentMap()
        {
            // Primary Key
            this.HasKey(t => new { t.JobId, t.UserId });

            // Properties
            this.Property(t => t.JobId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Employment", "planetgeni");
            this.Property(t => t.JobId).HasColumnName("JobId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
