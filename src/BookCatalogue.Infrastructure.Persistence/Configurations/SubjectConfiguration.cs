using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SubjectCatalogue.Infrastructure.Persistence.Configurations;
internal class SubjectConfiguration: IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasMany(s => s.Books)
            .WithMany(b => b.Subjects);

        builder.HasIndex(s => s.Title)
            .IsUnique()
            .HasOperators("text_pattern_ops");
    }
}
