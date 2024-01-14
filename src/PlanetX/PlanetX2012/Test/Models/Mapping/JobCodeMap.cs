using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class JobCodeMap : EntityTypeConfiguration<JobCode>
    {
        public JobCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.JobCodeId);

            // Properties
            this.Property(t => t.JobName)
                .IsRequired()
                .HasMaxLength(250);

            // Table & Column Mappings
            this.ToTable("JobCode", "planetgeni");
            this.Property(t => t.JobCodeId).HasColumnName("JobCodeId");
            this.Property(t => t.JobName).HasColumnName("JobName");
        }
    }
}
