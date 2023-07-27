using Microsoft.EntityFrameworkCore;

namespace BookCatalogue.Infrastructure.Persistence;

/// <summary>
/// DbContext for the book catalogue application.
/// </summary>
public sealed class BookCatalogueContext: DbContext
{
    public BookCatalogueContext(DbContextOptions<BookCatalogueContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookCatalogueContext).Assembly);
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Subject> Subjects { get; set; }
}
