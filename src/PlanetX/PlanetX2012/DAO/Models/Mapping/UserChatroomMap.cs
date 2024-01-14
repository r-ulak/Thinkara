using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class UserChatroomMap : EntityTypeConfiguration<UserChatroom>
    {
        public UserChatroomMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RoomId, t.UserId });

            // Properties
            this.Property(t => t.RoomId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("UserChatroom", "PlanetX");
            this.Property(t => t.RoomId).HasColumnName("RoomId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}
