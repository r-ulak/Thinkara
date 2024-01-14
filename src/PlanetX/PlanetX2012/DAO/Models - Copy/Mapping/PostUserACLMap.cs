using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class PostUserACLMap : EntityTypeConfiguration<PostUserACL>
    {
        public PostUserACLMap()
        {
            // Primary Key
            this.HasKey(t => new { t.PostId, t.UserId });

            // Properties
            this.Property(t => t.PostId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PostUserACL", "PlanetX");
            this.Property(t => t.PostId).HasColumnName("PostId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AccessType).HasColumnName("AccessType");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
