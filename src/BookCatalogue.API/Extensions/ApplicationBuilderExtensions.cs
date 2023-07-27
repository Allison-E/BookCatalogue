using BookCatalogue.API.Middlewares;
using BookCatalogue.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookCatalogue.API.Extensions;

internal static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Registers the <see cref="ErrorHandlingMiddleware"/> in the pipeline.
    /// </summary>
    /// <param name="app">ASP.NET Core app.</param>
    /// <returns><see cref="WebApplication"/></returns>
    public static IApplicationBuilder UseErrorHandler(this WebApplication app)
    {
        return app.UseMiddleware<ErrorHandlingMiddleware>();
    }

    /// <summary>
    /// Creates the database.
    /// </summary>
    /// <param name="app">ASP.NET Core app.</param>
    /// <returns><see cref="WebApplication"/></returns>
    public static async Task<IApplicationBuilder> CreateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Creating database");
            var appDataContext = scope.ServiceProvider.GetRequiredService<BookCatalogueContext>();
            await appDataContext.Database.EnsureCreatedAsync();
            logger.LogInformation("Database created");


            if (!app.Environment.IsProduction())
            {
                logger.LogInformation("Seeding database");
                await SeedDatabaseAsync(app, logger);
            }
        }
        catch (Exception e)
        {
            logger.LogError("Database could not be created. Error message: {ErrorMessage}", e.Message);
        }

        return app;
    }

    private static async Task SeedDatabaseAsync(WebApplication app, ILogger<Program> logger)
    {
        string firstname = "Adam";
        string lastname = "Grant";
        string subject = "Self-help";
        string[] bookTitles =
        {
            "Think Again: The Power of Knowing What You Don't Know",
            "Hidden Potentials: The Science of Achieving Greater Things"
        };

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BookCatalogueContext>();

        var adamGrant = await dbContext.Authors
            .Where(a => a.Firstname == firstname && a.Lastname == lastname)
            .FirstOrDefaultAsync();

        if (adamGrant is null)
        {
            adamGrant = new Author
            {
                Firstname = firstname,
                Lastname = lastname,
            };

            await dbContext.Authors
                .AddAsync(adamGrant);
        }

        var selfHelpSubject = await dbContext.Subjects
            .Where(s => s.Title == subject)
            .FirstOrDefaultAsync();

        if (selfHelpSubject is null)
        {
            selfHelpSubject = new Subject
            {
                Title = subject,
                Description = "Books to help you solve personal problems.",
            };

            await dbContext.Subjects
                .AddAsync(selfHelpSubject);
        }

        var result = await dbContext.SaveChangesAsync();

        if (result < 2)
        {
            logger.LogWarning("An error occurred while seeding the author and subject");
            return;
        }

        var thinkAgain = await dbContext.Books
            .Where(b => b.Title == bookTitles[0])
            .FirstOrDefaultAsync();

        if (thinkAgain is null)
        {
            await dbContext.Books
                .AddAsync(new Book
                {
                    Title = bookTitles[0],
                    Subjects = new List<Subject> { selfHelpSubject },
                    Authors = new List<Author> { adamGrant },
                    YearPublished = new DateOnly(2021, 02, 02),
                });
        }

        var hiddenPotentials = await dbContext.Books
            .Where(b => b.Title == bookTitles[1])
            .FirstOrDefaultAsync();

        if (hiddenPotentials is null)
        {
            await dbContext.Books
                .AddAsync(new Book
                {
                    Title = bookTitles[1],
                    Subjects = new List<Subject> { selfHelpSubject },
                    Authors = new List<Author> { adamGrant },
                    YearPublished = new DateOnly(2023, 10, 24),
                });
        }

        result = await dbContext.SaveChangesAsync();

        if (result < 2)
        {
            logger.LogWarning("An error occurred while seeding the books");
            return;
        }
    }
}
