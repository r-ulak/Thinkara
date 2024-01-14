using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class UserVoteProgramMap : EntityTypeConfiguration<UserVoteProgram>
    {
        public UserVoteProgramMap()
        {
            // Primary Key
            this.HasKey(t => t.ProgramId);

            // Properties
            this.Property(t => t.ProgramId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("UserVotePrograms", "planetgeni");
            this.Property(t => t.ProgramId).HasColumnName("ProgramId");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
