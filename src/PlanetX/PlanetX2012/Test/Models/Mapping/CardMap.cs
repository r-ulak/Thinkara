using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Test.Models.Mapping
{
    public class CardMap : EntityTypeConfiguration<Card>
    {
        public CardMap()
        {
            // Primary Key
            this.HasKey(t => t.CardId);

            // Properties
            // Table & Column Mappings
            this.ToTable("Card", "planetgeni");
            this.Property(t => t.CardId).HasColumnName("CardId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CardType).HasColumnName("CardType");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.ExpireDate).HasColumnName("ExpireDate");
            this.Property(t => t.CreatedAt).HasColumnName("CreatedAt");
            this.Property(t => t.UpdatedAt).HasColumnName("UpdatedAt");
        }
    }
}
