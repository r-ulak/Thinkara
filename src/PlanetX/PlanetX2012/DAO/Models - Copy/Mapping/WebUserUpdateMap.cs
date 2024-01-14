using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class WebUserUpdateMap : EntityTypeConfiguration<WebUserUpdate>
    {
        public WebUserUpdateMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NameFirst)
                .HasMaxLength(45);

            this.Property(t => t.NameMIddle)
                .HasMaxLength(45);

            this.Property(t => t.NameLast)
                .HasMaxLength(45);

            this.Property(t => t.EmailId)
                .HasMaxLength(100);

            this.Property(t => t.OldNameFirst)
                .HasMaxLength(45);

            this.Property(t => t.OldNameMIddle)
                .HasMaxLength(45);

            this.Property(t => t.OldNameLast)
                .HasMaxLength(45);

            this.Property(t => t.OldEmailId)
                .HasMaxLength(100);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.ActionType)
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("WebUserUpdate", "PlanetX");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.NameFirst).HasColumnName("NameFirst");
            this.Property(t => t.NameMIddle).HasColumnName("NameMIddle");
            this.Property(t => t.NameLast).HasColumnName("NameLast");
            this.Property(t => t.EmailId).HasColumnName("EmailId");
            this.Property(t => t.OldNameFirst).HasColumnName("OldNameFirst");
            this.Property(t => t.OldNameMIddle).HasColumnName("OldNameMIddle");
            this.Property(t => t.OldNameLast).HasColumnName("OldNameLast");
            this.Property(t => t.OldEmailId).HasColumnName("OldEmailId");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.ActionType).HasColumnName("ActionType");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
