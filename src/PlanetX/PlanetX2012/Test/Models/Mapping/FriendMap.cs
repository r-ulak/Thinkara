using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class FriendMap : EntityTypeConfiguration<Friend>
    {
        public FriendMap()
        {
            // Primary Key
            this.HasKey(t => t.FriendId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Friend", "planetgeni");
            this.Property(t => t.FriendId).HasColumnName("FriendId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.FriendUserId).HasColumnName("FriendUserId");
            this.Property(t => t.IsSubscriber).HasColumnName("IsSubscriber");
            this.Property(t => t.Privacy).HasColumnName("Privacy");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
