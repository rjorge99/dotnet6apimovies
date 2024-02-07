namespace MoviesApi.Dtos
{
    public class PaginationDto
    {
        public int CurrentPage { get; set; } = 1;

        private int _quantityRegistersPerPage = 10;
        private readonly int _maxQuantityPerPage = 50;

        public int RegistersPerPage
        {
            get => _quantityRegistersPerPage;
            set => _quantityRegistersPerPage = value > _maxQuantityPerPage ? _maxQuantityPerPage : value;
        }

    }
}
