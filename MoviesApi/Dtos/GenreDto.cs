using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class GenreDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 40)]
        public string Name { get; set; }
    }
}
