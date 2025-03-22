using AutoFixture;
using ChangeDataCapture.ApiService;
using ChangeDataCapture.ApiService.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<MoviesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("movies-db"));
});
// Add services to the container.
builder.Services.AddProblemDetails();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/movies", async (MoviesDbContext context) =>
{
    return await context.MovieCollection.ToListAsync();
})
.WithName("GetMovies");

app.MapPost("/movies", async (MoviesDbContext context) =>
{
    var fixture = new Fixture();
    var fakeMovie = fixture.Build<Movie>()
                           .With(x => x.ReleaseDate,new DateOnly(1990,10,10))
                           .With(x => x.Id, Guid.CreateVersion7())
                           .Create();
    await context.MovieCollection.AddAsync(fakeMovie);
    await context.SaveChangesAsync();
    return fakeMovie.Id;
})
.WithName("PostMovies");

app.MapDefaultEndpoints();

app.Run();