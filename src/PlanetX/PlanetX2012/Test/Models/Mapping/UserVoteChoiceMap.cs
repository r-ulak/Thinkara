using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserVoteChoiceMap : EntityTypeConfiguration<UserVoteChoice>
    {
        public UserVoteChoiceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ChoiceId, t.TaskTypeId });

            // Properties
            this.Property(t => t.ChoiceId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TaskTypeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ChoiceText)
                .IsRequired()
                .HasMaxLength(1000);

            this.Property(t => t.ChoiceLogo)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("UserVoteChoice", "planetgeni");
            this.Property(t => t.ChoiceId).HasColumnName("ChoiceId");
            this.Property(t => t.TaskTypeId).HasColumnName("TaskTypeId");
            this.Property(t => t.ChoiceText).HasColumnName("ChoiceText");
            this.Property(t => t.ChoiceLogo).HasColumnName("ChoiceLogo");
        }
    }
}
