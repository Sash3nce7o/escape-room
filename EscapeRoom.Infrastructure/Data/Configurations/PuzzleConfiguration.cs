using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EscapeRoom.Infrastructure.Data.Models;
using static EscapeRoom.Infrastructure.Data.Constants.DbConstants;

namespace EscapeRoom.Infrastructure.Data.Configurations
{
    public class PuzzleConfiguration : IEntityTypeConfiguration<Puzzle>
    {
        public void Configure(EntityTypeBuilder<Puzzle> builder)
        {
            builder.Property(p => p.Title)
                   .IsRequired()
                   .HasMaxLength(PuzzleConstants.TitleMaxLength);

            builder.Property(p => p.Question)
                   .IsRequired()
                   .HasMaxLength(PuzzleConstants.QuestionMaxLength);

            builder.Property(p => p.AnswerHash)
                   .IsRequired()
                   .HasMaxLength(PuzzleConstants.AnswerHashMaxLength);

            // A puzzle belongs to the game aggregate: deleting the game deletes its puzzles.
            builder.HasOne(p => p.Game)
                   .WithMany(g => g.Puzzles)
                   .HasForeignKey(p => p.GameId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Within one game the unlock order must be unique.
            builder.HasIndex(p => new { p.GameId, p.Order })
                   .IsUnique();
        }
    }
}
