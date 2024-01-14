using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PrivacyMap : EntityTypeConfiguration<Privacy>
    {
        public PrivacyMap()
        {
            // Primary Key
            this.HasKey(t => t.PrivacyId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Privacy", "planetgeni");
            this.Property(t => t.PrivacyId).HasColumnName("PrivacyId");
            this.Property(t => t.Profile).HasColumnName("Profile");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Bookmark).HasColumnName("Bookmark");
            this.Property(t => t.Feed).HasColumnName("Feed");
            this.Property(t => t.Activity).HasColumnName("Activity");
            this.Property(t => t.Friend).HasColumnName("Friend");
            this.Property(t => t.FriendList).HasColumnName("FriendList");
            this.Property(t => t.Nickname).HasColumnName("Nickname");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
