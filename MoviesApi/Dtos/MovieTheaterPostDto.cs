using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class MovieTheaterPostDto
    {

        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        [Range(-90, 90)]
        public double Latitud { get; set; }
        [Range(-180, 180)]
        public double Longitud { get; set; }
    }
}
