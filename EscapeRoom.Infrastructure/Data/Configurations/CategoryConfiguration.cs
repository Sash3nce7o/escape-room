using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EscapeRoom.Infrastructure.Data.Models;
using static EscapeRoom.Infrastructure.Data.Constants.DbConstants;

namespace EscapeRoom.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(CategoryConstants.NameMaxLength);

            builder.Property(c => c.Description)
                   .HasMaxLength(MaxDescriptionLength);

            // Category names are unique — the admin panel uses them for filtering.
            builder.HasIndex(c => c.Name)
                   .IsUnique();
        }
    }
}
