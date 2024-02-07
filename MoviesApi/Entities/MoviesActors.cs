namespace MoviesApi.Entities
{
    public class MoviesActors
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string Character { get; set; }
        public int Order { get; set; }

        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
