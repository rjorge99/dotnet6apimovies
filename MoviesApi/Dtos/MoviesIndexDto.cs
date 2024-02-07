namespace MoviesApi.Dtos
{
    public class MoviesIndexDto
    {
        public ICollection<MovieDto> NewReleases { get; set; }
        public ICollection<MovieDto> InTheaters { get; set; }
    }
}
