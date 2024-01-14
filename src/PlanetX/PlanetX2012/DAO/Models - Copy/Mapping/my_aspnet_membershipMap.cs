using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace DAO.Models.Mapping
{
    public class my_aspnet_membershipMap : EntityTypeConfiguration<my_aspnet_membership>
    {
        public my_aspnet_membershipMap()
        {
            // Primary Key
            this.HasKey(t => t.userId);

            // Properties
            this.Property(t => t.userId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Email)
                .HasMaxLength(128);

            this.Property(t => t.Comment)
                .HasMaxLength(255);

            this.Property(t => t.Password)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.PasswordKey)
                .HasMaxLength(32);

            this.Property(t => t.PasswordQuestion)
                .HasMaxLength(255);

            this.Property(t => t.PasswordAnswer)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("my_aspnet_membership", "PlanetX");
            this.Property(t => t.userId).HasColumnName("userId");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.Password).HasColumnName("Password");
            this.Property(t => t.PasswordKey).HasColumnName("PasswordKey");
            this.Property(t => t.PasswordFormat).HasColumnName("PasswordFormat");
            this.Property(t => t.PasswordQuestion).HasColumnName("PasswordQuestion");
            this.Property(t => t.PasswordAnswer).HasColumnName("PasswordAnswer");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.LastActivityDate).HasColumnName("LastActivityDate");
            this.Property(t => t.LastLoginDate).HasColumnName("LastLoginDate");
            this.Property(t => t.LastPasswordChangedDate).HasColumnName("LastPasswordChangedDate");
            this.Property(t => t.CreationDate).HasColumnName("CreationDate");
            this.Property(t => t.IsLockedOut).HasColumnName("IsLockedOut");
            this.Property(t => t.LastLockedOutDate).HasColumnName("LastLockedOutDate");
            this.Property(t => t.FailedPasswordAttemptCount).HasColumnName("FailedPasswordAttemptCount");
            this.Property(t => t.FailedPasswordAttemptWindowStart).HasColumnName("FailedPasswordAttemptWindowStart");
            this.Property(t => t.FailedPasswordAnswerAttemptCount).HasColumnName("FailedPasswordAnswerAttemptCount");
            this.Property(t => t.FailedPasswordAnswerAttemptWindowStart).HasColumnName("FailedPasswordAnswerAttemptWindowStart");
        }
    }
}
