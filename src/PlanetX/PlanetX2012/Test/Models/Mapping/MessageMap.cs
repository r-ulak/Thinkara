using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageId);

            // Properties
            this.Property(t => t.MessageContent)
                .IsRequired()
                .HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("Message", "planetgeni");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.MessageContent).HasColumnName("MessageContent");
        }
    }
}
