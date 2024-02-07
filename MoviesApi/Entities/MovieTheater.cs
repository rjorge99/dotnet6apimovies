using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class MovieTheater : IId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        public Point Ubication { get; set; }
        public ICollection<MoviesMovieTheaters> MoviesMovieTheaters { get; set; }
    }
}
