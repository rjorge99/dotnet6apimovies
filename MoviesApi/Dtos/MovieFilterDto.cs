namespace MoviesApi.Dtos
{
    public class MovieFilterDto
    {
        public int CurrentPage { get; set; } = 1;
        public int RegistersPerPage { get; set; } = 10;
        public PaginationDto Pagination
        {
            get => new PaginationDto
            {
                CurrentPage = CurrentPage,
                RegistersPerPage = RegistersPerPage
            };
        }

        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool InTheaters { get; set; }
        public bool NewReleases { get; set; }

        public string OrderBy { get; set; }
        public bool Ascending { get; set; } = true;
    }
}
