using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookCatalogue.Infrastructure.Persistence.Configurations;
internal class AuthorConfiguration: IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasMany(a => a.Books)
            .WithMany(b => b.Authors);

        builder.HasIndex(a => new { a.Firstname, a.Lastname })
            .HasOperators("text_pattern_ops", "text_pattern_ops");
    }
}
