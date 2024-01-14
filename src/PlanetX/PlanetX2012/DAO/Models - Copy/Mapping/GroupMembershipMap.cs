using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class GroupMembershipMap : EntityTypeConfiguration<GroupMembership>
    {
        public GroupMembershipMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.UserGroupId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserGroupId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("GroupMembership", "PlanetX");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserGroupId).HasColumnName("UserGroupId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");

            // Relationships
            this.HasRequired(t => t.UserGroup)
                .WithMany(t => t.GroupMemberships)
                .HasForeignKey(d => d.UserGroupId);

        }
    }
}
