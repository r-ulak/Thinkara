using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class JobMap : EntityTypeConfiguration<Job>
    {
        public JobMap()
        {
            // Primary Key
            this.HasKey(t => t.JobId);

            // Properties
            this.Property(t => t.Frequency)
                .IsRequired()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Job", "planetgeni");
            this.Property(t => t.JobId).HasColumnName("JobId");
            this.Property(t => t.JobTypeId).HasColumnName("JobTypeId");
            this.Property(t => t.JobCodeId).HasColumnName("JobCodeId");
            this.Property(t => t.CompanyId).HasColumnName("CompanyId");
            this.Property(t => t.ExipiryDate).HasColumnName("ExipiryDate");
            this.Property(t => t.Salary).HasColumnName("Salary");
            this.Property(t => t.Frequency).HasColumnName("Frequency");
            this.Property(t => t.HrsPerWeek).HasColumnName("HrsPerWeek");
        }
    }
}
