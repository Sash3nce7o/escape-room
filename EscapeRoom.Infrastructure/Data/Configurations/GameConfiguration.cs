using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EscapeRoom.Infrastructure.Data.Models;
using static EscapeRoom.Infrastructure.Data.Constants.DbConstants;

namespace EscapeRoom.Infrastructure.Data.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.Property(g => g.Title)
                   .IsRequired()
                   .HasMaxLength(GameConstants.TitleMaxLength);

            builder.Property(g => g.Description)
                   .IsRequired()
                   .HasMaxLength(MaxDescriptionLength);

            builder.Property(g => g.CoverImageUrl)
                   .HasMaxLength(GameConstants.CoverUrlMaxLength);

            builder.Property(g => g.TimeLimitMinutes)
                   .IsRequired();

            builder.Property(g => g.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            // Deleting a category must not delete its games.
            builder.HasOne(g => g.Category)
                   .WithMany(c => c.Games)
                   .HasForeignKey(g => g.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Deleting a user must not delete the games they authored.
            builder.HasOne(g => g.Author)
                   .WithMany()
                   .HasForeignKey(g => g.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Catalog queries filter by category and published status.
            builder.HasIndex(g => new { g.CategoryId, g.Status });
        }
    }
}
