using Microsoft.EntityFrameworkCore;
using ChangeDataCapture.ApiService.Entities;
namespace ChangeDataCapture.ApiService
{
    public class MoviesDbContext : DbContext
    {
        public DbSet<Movie> MovieCollection { get; set; }
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(t =>
            t.ToTable("Movies")
             .HasData(
                new Movie { Id = Guid.Parse("0195bda1-76dc-76a2-a0c0-7159220ec134"), Title = "The Matrix", Genre = Genre.SciFi, ReleaseDate = new DateOnly(1999, 3, 31) },
                new Movie { Id = Guid.Parse("0195bda1-76dc-7f46-89a1-75105b2d30d2"), Title = "The Matrix Reloaded", Genre = Genre.SciFi, ReleaseDate = new DateOnly(2003, 5, 15) },
                new Movie { Id = Guid.Parse("0195bda1-76dc-7fdc-b9b3-b882c033b492"), Title = "The Matrix Revolutions", Genre = Genre.SciFi, ReleaseDate = new DateOnly(2003, 11, 5) },
                new Movie { Id = Guid.Parse("0195bda1-76dc-735c-9361-cc0d0b311d7f"), Title = "The Shawshank Redemption", Genre = Genre.Drama, ReleaseDate = new DateOnly(1994, 9, 23) }
                ));
        }
    }
}