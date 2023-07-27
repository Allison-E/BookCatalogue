using BookCatalogue.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

# region Add services to IoC container
builder.Services.AddControllers();
builder.Services.AddApiVersioningExtension();
builder.Services.AddSwaggerExtension();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.
var createDatabaseTask = app.CreateDatabaseAsync();

app.UseSwagger();
app.UseSwaggerUI();
app.UseErrorHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await createDatabaseTask;
app.Run();
#endregion