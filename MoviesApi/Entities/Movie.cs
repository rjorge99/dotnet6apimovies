using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class Movie : IId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool IsInTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }

        public ICollection<MoviesActors> MoviesActors { get; set; }
        public ICollection<MoviesGenres> MoviesGenres { get; set; }
        public ICollection<MoviesMovieTheaters> MoviesMovieTheaters { get; set; }
        public ICollection<Review> Reviews { get; set; }

    }
}
