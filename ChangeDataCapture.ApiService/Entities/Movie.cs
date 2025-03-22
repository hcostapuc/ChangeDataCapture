using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeDataCapture.ApiService.Entities
{
    public class Movie
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public decimal Ratting { get; set; }
        public Genre Genre { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
    public enum Genre
    {
        Action,
        Comedy,
        Drama,
        Horror,
        SciFi
    }
}
