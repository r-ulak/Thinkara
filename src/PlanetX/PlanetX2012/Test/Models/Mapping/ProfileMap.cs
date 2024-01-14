using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class ProfileMap : EntityTypeConfiguration<Profile>
    {
        public ProfileMap()
        {
            // Primary Key
            this.HasKey(t => t.ProfileId);

            // Properties
            this.Property(t => t.AboutMe)
                .HasMaxLength(160);

            this.Property(t => t.Relationship)
                .HasMaxLength(45);

            this.Property(t => t.LookingFor)
                .HasMaxLength(45);

            this.Property(t => t.Phone)
                .HasMaxLength(45);

            this.Property(t => t.Interests)
                .HasMaxLength(255);

            this.Property(t => t.Education)
                .HasMaxLength(255);

            this.Property(t => t.Hobbies)
                .HasMaxLength(255);

            this.Property(t => t.FavMovies)
                .HasMaxLength(255);

            this.Property(t => t.FavArtists)
                .HasMaxLength(255);

            this.Property(t => t.FavBooks)
                .HasMaxLength(255);

            this.Property(t => t.FavAnimals)
                .HasMaxLength(255);

            this.Property(t => t.EverythingElse)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Profile", "planetgeni");
            this.Property(t => t.ProfileId).HasColumnName("ProfileId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Privacy).HasColumnName("Privacy");
            this.Property(t => t.Rating).HasColumnName("Rating");
            this.Property(t => t.Dob).HasColumnName("Dob");
            this.Property(t => t.AboutMe).HasColumnName("AboutMe");
            this.Property(t => t.Relationship).HasColumnName("Relationship");
            this.Property(t => t.LookingFor).HasColumnName("LookingFor");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Interests).HasColumnName("Interests");
            this.Property(t => t.Education).HasColumnName("Education");
            this.Property(t => t.Hobbies).HasColumnName("Hobbies");
            this.Property(t => t.FavMovies).HasColumnName("FavMovies");
            this.Property(t => t.FavArtists).HasColumnName("FavArtists");
            this.Property(t => t.FavBooks).HasColumnName("FavBooks");
            this.Property(t => t.FavAnimals).HasColumnName("FavAnimals");
            this.Property(t => t.Religion).HasColumnName("Religion");
            this.Property(t => t.EverythingElse).HasColumnName("EverythingElse");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
        }
    }
}
