using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class MessageInfoMap : EntityTypeConfiguration<MessageInfo>
    {
        public MessageInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.MessageId);

            // Properties
            // Table & Column Mappings
            this.ToTable("MessageInfo", "planetgeni");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.ParentMessageId).HasColumnName("ParentMessageId");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.IsRead).HasColumnName("IsRead");
            this.Property(t => t.IsSpam).HasColumnName("IsSpam");
            this.Property(t => t.ToId).HasColumnName("ToId");
            this.Property(t => t.FromId).HasColumnName("FromId");
        }
    }
}
