using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookCatalogue.Infrastructure.Persistence.Extensions;
public static class ServiceExtensions
{
    /// <summary>
    /// Configures the services for persistence layer of the application.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Application configuration properties.</param>
    public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BookCatalogueContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Default")!, options =>
            {
                options.EnableRetryOnFailure(10);
                options.UseAdminDatabase(configuration.GetConnectionString("AdminDatabase"));
                options.MigrationsAssembly(typeof(BookCatalogueContext).Assembly.FullName);
            });
        });
    }
}
