namespace MoviesApi.Entities
{
    public class MoviesMovieTheaters
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int MovieTheaterId { get; set; }
        public MoviesMovieTheaters MovieTheaters { get; set; }
    }
}
