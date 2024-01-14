using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ElectionQueueMap : EntityTypeConfiguration<ElectionQueue>
    {
        public ElectionQueueMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ElectionQueue", "planetgeni");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
