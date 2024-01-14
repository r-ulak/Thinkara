using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class RsvpStatusCodeMap : EntityTypeConfiguration<RsvpStatusCode>
    {
        public RsvpStatusCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.StatusType);

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("RsvpStatusCode", "planetgeni");
            this.Property(t => t.StatusType).HasColumnName("StatusType");
            this.Property(t => t.Code).HasColumnName("Code");
        }
    }
}
