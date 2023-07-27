using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookCatalogue.Infrastructure.Persistence.Configurations;
internal class BookConfiguration: IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasMany(b => b.Subjects)
            .WithMany(s => s.Books);

        builder.HasMany(b => b.Authors)
            .WithMany(a => a.Books);

        builder.HasIndex(b => b.YearPublished);

        builder.HasIndex(b => b.Title)
            .HasOperators("text_pattern_ops");
    }
}
