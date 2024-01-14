using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserVoteSelectedChoiceMap : EntityTypeConfiguration<UserVoteSelectedChoice>
    {
        public UserVoteSelectedChoiceMap()
        {
            // Primary Key
            this.HasKey(t => new { t.VoteId, t.ChoiceId });

            // Properties
            this.Property(t => t.ChoiceId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("UserVoteSelectedChoice", "planetgeni");
            this.Property(t => t.VoteId).HasColumnName("VoteId");
            this.Property(t => t.ChoiceId).HasColumnName("ChoiceId");
        }
    }
}
