using BookCatalogue.Web.Data;
using BookCatalogue.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<AuthorService>();
builder.Services.AddSingleton<SubjectService>();
builder.Services.AddHttpClient<AuthorService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BookCatalogue:ApiBaseUrl"]!);
});
builder.Services.AddHttpClient<BookService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BookCatalogue:ApiBaseUrl"]!);
});
builder.Services.AddHttpClient<SubjectService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BookCatalogue:ApiBaseUrl"]!);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
