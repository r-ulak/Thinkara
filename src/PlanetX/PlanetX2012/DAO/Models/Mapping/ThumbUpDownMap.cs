using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class ThumbUpDownMap : EntityTypeConfiguration<ThumbUpDown>
    {
        public ThumbUpDownMap()
        {
            // Primary Key
            this.HasKey(t => t.ThumbUpDownId);

            // Properties
            // Table & Column Mappings
            this.ToTable("ThumbUpDown", "PlanetX");
            this.Property(t => t.ThumbUpDownId).HasColumnName("ThumbUpDownId");
            this.Property(t => t.Flag).HasColumnName("Flag");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.StatusId).HasColumnName("StatusId");
            this.Property(t => t.FriendId).HasColumnName("FriendId");
            this.Property(t => t.UserId).HasColumnName("UserId");

            // Relationships
            this.HasOptional(t => t.Status)
                .WithMany(t => t.ThumbUpDowns)
                .HasForeignKey(d => d.StatusId);

        }
    }
}
