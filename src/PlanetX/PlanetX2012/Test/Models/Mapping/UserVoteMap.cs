using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserVoteMap : EntityTypeConfiguration<UserVote>
    {
        public UserVoteMap()
        {
            // Primary Key
            this.HasKey(t => t.VoteId);

            // Properties
            // Table & Column Mappings
            this.ToTable("UserVote", "planetgeni");
            this.Property(t => t.VoteId).HasColumnName("VoteId");
            this.Property(t => t.TaskId).HasColumnName("TaskId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
