using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class TaskTypeMap : EntityTypeConfiguration<TaskType>
    {
        public TaskTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.TaskTypeId);

            // Properties
            this.Property(t => t.ShortDescription)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("TaskType", "planetgeni");
            this.Property(t => t.TaskTypeId).HasColumnName("TaskTypeId");
            this.Property(t => t.ShortDescription).HasColumnName("ShortDescription");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.ChoiceType).HasColumnName("ChoiceType");
            this.Property(t => t.MaxChoiceCount).HasColumnName("MaxChoiceCount");
        }
    }
}
