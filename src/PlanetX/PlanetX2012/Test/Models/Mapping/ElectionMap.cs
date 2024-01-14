using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ElectionMap : EntityTypeConfiguration<Election>
    {
        public ElectionMap()
        {
            // Primary Key
            this.HasKey(t => t.ElectionId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Election", "planetgeni");
            this.Property(t => t.ElectionId).HasColumnName("ElectionId");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
        }
    }
}
