using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class PartyMemberTypeMap : EntityTypeConfiguration<PartyMemberType>
    {
        public PartyMemberTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.PartyMemberTypeId);

            // Properties
            this.Property(t => t.PartyMemberTypeName)
                .IsRequired()
                .HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("PartyMemberType", "planetgeni");
            this.Property(t => t.PartyMemberTypeId).HasColumnName("PartyMemberTypeId");
            this.Property(t => t.PartyMemberTypeName).HasColumnName("PartyMemberTypeName");
        }
    }
}
