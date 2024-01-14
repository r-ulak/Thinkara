using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class JobTypeMap : EntityTypeConfiguration<JobType>
    {
        public JobTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.JobTypeId);

            // Properties
            this.Property(t => t.JobTypeName)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("JobType", "planetgeni");
            this.Property(t => t.JobTypeId).HasColumnName("JobTypeId");
            this.Property(t => t.JobTypeName).HasColumnName("JobTypeName");
        }
    }
}
