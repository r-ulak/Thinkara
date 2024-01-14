using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class WebUserMap : EntityTypeConfiguration<WebUser>
    {
        public WebUserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.NameFirst)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.NameMIddle)
                .HasMaxLength(45);

            this.Property(t => t.NameLast)
                .IsRequired()
                .HasMaxLength(45);

            this.Property(t => t.EmailId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("WebUser", "PlanetX");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.NameFirst).HasColumnName("NameFirst");
            this.Property(t => t.NameMIddle).HasColumnName("NameMIddle");
            this.Property(t => t.NameLast).HasColumnName("NameLast");
            this.Property(t => t.EmailId).HasColumnName("EmailId");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.OnlineStatus).HasColumnName("OnlineStatus");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
