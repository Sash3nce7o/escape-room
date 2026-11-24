using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EscapeRoom.Infrastructure.Data.Enums;
using static EscapeRoom.Infrastructure.Data.Constants.DbConstants;

namespace EscapeRoom.Infrastructure.Data.Models
{
    public class Game
    {
        public Game()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAt = DateTime.UtcNow;
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(GameConstants.TitleMaxLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(MaxDescriptionLength)]
        public string Description { get; set; } = string.Empty;

        [MaxLength(GameConstants.CoverUrlMaxLength)]
        public string? CoverImageUrl { get; set; }

        public Difficulty Difficulty { get; set; } = Difficulty.Easy;

        public GameStatus Status { get; set; } = GameStatus.Draft;

        public int TimeLimitMinutes { get; set; }

        [Required]
        public string CategoryId { get; set; } = string.Empty;
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        [Required]
        public string AuthorId { get; set; } = string.Empty;
        [ForeignKey(nameof(AuthorId))]
        public virtual User Author { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Puzzle> Puzzles { get; set; } = new List<Puzzle>();
    }
}
