using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Dtos
{
    public class ReviewPostDto
    {
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }
    }
}
