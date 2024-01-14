using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageId);

            // Properties
            this.Property(t => t.Message1)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Message", "PlanetX");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.Message1).HasColumnName("Message");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.IsRead).HasColumnName("IsRead");
            this.Property(t => t.IsSpam).HasColumnName("IsSpam");
            this.Property(t => t.To).HasColumnName("To");
            this.Property(t => t.IsReply).HasColumnName("IsReply");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
