using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;

namespace BookCatalogue.API.Extensions;
internal static class ServiceExtensions
{
    internal static void AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}\BookCatalogue.API.xml"));
            c.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}\BookCatalogue.Core.xml"));
            c.IncludeXmlComments(string.Format(@$"{AppDomain.CurrentDomain.BaseDirectory}\BookCatalogue.Infrastructure.Persistence.xml"));

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Book Catalogue API",
                Description = "This API will be responsible for handling operations in the book catalogue.",
                Contact = new OpenApiContact
                {
                    Name = "Emmanuel Allison",
                    Email = "emmallison13@gmail.com",
                }
            });
            c.DescribeAllParametersInCamelCase();
            c.SupportNonNullableReferenceTypes();
        });
    }

    internal static void AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        services.AddVersionedApiExplorer(options =>
        {
            // Add the versioned api explorer, which also adds IApiVersionDescriptionProvider service.
            // Note: the specified format code will format the version as "'v'major[.minor][-status]".
            options.GroupNameFormat = "'v'VVV";

            // Note: this option is only necessary when versioning by url segment. The SubstitutionFormat
            // can also be used to control the format of the WebApi version in route templates.
            options.SubstituteApiVersionInUrl = true;
        });
    }
}