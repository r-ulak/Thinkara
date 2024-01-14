using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PostClubACLMap : EntityTypeConfiguration<PostClubACL>
    {
        public PostClubACLMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PostId, t.ClubId });

            // Properties
            this.Property(t => t.PostId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ClubId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PostClubACL", "planetgeni");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.ClubId).HasColumnName("ClubId");
            this.Property(t => t.AccessType).HasColumnName("AccessType");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.Club)
                .WithMany(t => t.PostClubACLs)
                .HasForeignKey(d => d.ClubId);

        }
    }
}
