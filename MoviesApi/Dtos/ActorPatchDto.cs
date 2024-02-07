using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class ActorPatchDto
    {
        [Required]
        [StringLength(maximumLength: 120)]
        public string Name { get; set; }

        public DateTime Birthday { get; set; }
    }
}
