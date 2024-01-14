using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class ChatMap : EntityTypeConfiguration<Chat>
    {
        public ChatMap()
        {
            // Primary Key
            this.HasKey(t => t.ChatId);

            // Properties
            this.Property(t => t.Msg)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Chat", "PlanetX");
            this.Property(t => t.ChatId).HasColumnName("ChatId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.To).HasColumnName("To");
            this.Property(t => t.Msg).HasColumnName("Msg");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
