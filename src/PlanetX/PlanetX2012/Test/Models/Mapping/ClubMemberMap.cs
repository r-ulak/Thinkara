using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ClubMemberMap : EntityTypeConfiguration<ClubMember>
    {
        public ClubMemberMap()
        {
            // Primary Key
            this.HasKey(t => t.ClubMemberId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ClubMember", "planetgeni");
            this.Property(t => t.ClubMemberId).HasColumnName("ClubMemberId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ClubId).HasColumnName("ClubId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
