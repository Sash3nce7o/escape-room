using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EscapeRoom.Infrastructure.Data.Enums;
using static EscapeRoom.Infrastructure.Data.Constants.DbConstants;

namespace EscapeRoom.Infrastructure.Data.Models
{
    public class Puzzle
    {
        public Puzzle()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string GameId { get; set; } = string.Empty;
        [ForeignKey(nameof(GameId))]
        public virtual Game Game { get; set; } = null!;

        [Required]
        [MaxLength(PuzzleConstants.TitleMaxLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(PuzzleConstants.QuestionMaxLength)]
        public string Question { get; set; } = string.Empty;

        [Required]
        [MaxLength(PuzzleConstants.AnswerHashMaxLength)]
        public string AnswerHash { get; set; } = string.Empty;

        public AnswerCheckType CheckType { get; set; } = AnswerCheckType.ExactMatch;

        public int Points { get; set; } = 100;

        // Unlock sequence: a correct answer unlocks the puzzle with the next order value.
        public int Order { get; set; }
    }
}
