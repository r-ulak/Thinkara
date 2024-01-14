using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class UserGroupMap : EntityTypeConfiguration<UserGroup>
    {
        public UserGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.UserGroupId);

            // Properties
            this.Property(t => t.UserGroupName)
                .HasMaxLength(45);

            this.Property(t => t.Description)
                .HasMaxLength(65535);

            this.Property(t => t.Picture)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Url)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("UserGroup", "PlanetX");
            this.Property(t => t.UserGroupId).HasColumnName("UserGroupId");
            this.Property(t => t.UserGroupName).HasColumnName("UserGroupName");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.UserGroupType).HasColumnName("UserGroupType");
            this.Property(t => t.Picture).HasColumnName("Picture");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Active).HasColumnName("Active");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
