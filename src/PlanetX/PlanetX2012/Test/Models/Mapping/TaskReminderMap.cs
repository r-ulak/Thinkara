using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class TaskReminderMap : EntityTypeConfiguration<TaskReminder>
    {
        public TaskReminderMap()
        {
            // Primary Key
            this.HasKey(t => t.ReminderId);

            // Properties
            this.Property(t => t.ReminderTransPort)
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("TaskReminder", "planetgeni");
            this.Property(t => t.ReminderId).HasColumnName("ReminderId");
            this.Property(t => t.ReminderFrequency).HasColumnName("ReminderFrequency");
            this.Property(t => t.ReminderTransPort).HasColumnName("ReminderTransPort");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
