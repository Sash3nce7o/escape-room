using System.ComponentModel.DataAnnotations;
using static EscapeRoom.Infrastructure.Data.Constants.DbConstants;

namespace EscapeRoom.Infrastructure.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(CategoryConstants.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(MaxDescriptionLength)]
        public string? Description { get; set; }

        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
