using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class EventCodeMap : EntityTypeConfiguration<EventCode>
    {
        public EventCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.EventType);

            // Properties
            this.Property(t => t.EventType)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Code)
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("EventCode", "planetgeni");
            this.Property(t => t.EventType).HasColumnName("EventType");
            this.Property(t => t.Code).HasColumnName("Code");
        }
    }
}
