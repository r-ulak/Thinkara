using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserTaskMap : EntityTypeConfiguration<UserTask>
    {
        public UserTaskMap()
        {
            // Primary Key
            this.HasKey(t => t.TaskId);

            // Properties
            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.Parms)
                .HasMaxLength(350);

            this.Property(t => t.DefaultResponse)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("UserTask", "planetgeni");
            this.Property(t => t.TaskId).HasColumnName("TaskId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AssignerUserId).HasColumnName("AssignerUserId");
            this.Property(t => t.ReminderId).HasColumnName("ReminderId");
            this.Property(t => t.CompletionPercent).HasColumnName("CompletionPercent");
            this.Property(t => t.Flagged).HasColumnName("Flagged");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Parms).HasColumnName("Parms");
            this.Property(t => t.TaskTypeId).HasColumnName("TaskTypeId");
            this.Property(t => t.DueDate).HasColumnName("DueDate");
            this.Property(t => t.DefaultResponse).HasColumnName("DefaultResponse");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
