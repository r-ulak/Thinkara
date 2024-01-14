using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class AdvertisementMap : EntityTypeConfiguration<Advertisement>
    {
        public AdvertisementMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AdvertisementId, t.UserId });

            // Properties
            this.Property(t => t.AdvertisementId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AdsTypeEmail)
                .HasMaxLength(1);

            this.Property(t => t.AdsTypeFeed)
                .HasMaxLength(1);

            this.Property(t => t.AdsTypeEmailAll)
                .HasMaxLength(1);

            this.Property(t => t.DaysS)
                .HasMaxLength(1);

            this.Property(t => t.DaysM)
                .HasMaxLength(1);

            this.Property(t => t.DaysT)
                .HasMaxLength(1);

            this.Property(t => t.DaysW)
                .HasMaxLength(1);

            this.Property(t => t.DaysTh)
                .HasMaxLength(1);

            this.Property(t => t.DaysF)
                .HasMaxLength(1);

            this.Property(t => t.DaysSa)
                .HasMaxLength(1);

            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(65535);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(65535);

            // Table & Column Mappings
            this.ToTable("Advertisement", "planetgeni");
            this.Property(t => t.AdvertisementId).HasColumnName("AdvertisementId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AdsTypeEmail).HasColumnName("AdsTypeEmail");
            this.Property(t => t.AdsTypeFeed).HasColumnName("AdsTypeFeed");
            this.Property(t => t.AdsTypeEmailAll).HasColumnName("AdsTypeEmailAll");
            this.Property(t => t.EventTypeId).HasColumnName("EventTypeId");
            this.Property(t => t.AdsFrequencyTypeId).HasColumnName("AdsFrequencyTypeId");
            this.Property(t => t.DaysS).HasColumnName("DaysS");
            this.Property(t => t.DaysM).HasColumnName("DaysM");
            this.Property(t => t.DaysT).HasColumnName("DaysT");
            this.Property(t => t.DaysW).HasColumnName("DaysW");
            this.Property(t => t.DaysTh).HasColumnName("DaysTh");
            this.Property(t => t.DaysF).HasColumnName("DaysF");
            this.Property(t => t.DaysSa).HasColumnName("DaysSa");
            this.Property(t => t.AdTime).HasColumnName("AdTime");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Cost).HasColumnName("Cost");
        }
    }
}
