using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class AvatarMap : EntityTypeConfiguration<Avatar>
    {
        public AvatarMap()
        {
            // Primary Key
            this.HasKey(t => t.AvatarId);

            // Properties
            this.Property(t => t.AvatarId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Avatar", "PlanetX");
            this.Property(t => t.AvatarId).HasColumnName("AvatarId");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
