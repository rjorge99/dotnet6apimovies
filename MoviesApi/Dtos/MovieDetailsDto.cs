namespace MoviesApi.Dtos
{
    public class MovieDetailsDto : MovieDto
    {
        public ICollection<GenreDto> Genres { get; set; }
        public ICollection<ActorMovieDetailDto> Actors { get; set; }
    }
}
