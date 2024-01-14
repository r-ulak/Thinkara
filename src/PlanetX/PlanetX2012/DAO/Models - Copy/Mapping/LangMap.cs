using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class LangMap : EntityTypeConfiguration<Lang>
    {
        public LangMap()
        {
            // Primary Key
            this.HasKey(t => t.LanguageId);

            // Properties
            this.Property(t => t.Lang1)
                .IsRequired()
                .HasMaxLength(45);

            // Table & Column Mappings
            this.ToTable("Lang", "PlanetX");
            this.Property(t => t.LanguageId).HasColumnName("LanguageId");
            this.Property(t => t.Lang1).HasColumnName("Lang");
            this.Property(t => t.UserId).HasColumnName("UserId");
        }
    }
}
